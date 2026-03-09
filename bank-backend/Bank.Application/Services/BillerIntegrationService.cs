using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bank.Application.Services;

/// <summary>
/// Service implementation for external biller system integration
/// </summary>
public class BillerIntegrationService : IBillerIntegrationService
{
    private readonly IBillerRepository _billerRepository;
    private readonly IBillerHealthCheckRepository _billerHealthCheckRepository;
    private readonly IPaymentRetryRepository _paymentRetryRepository;
    private readonly IBillPresentmentRepository _billPresentmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<BillerIntegrationService> _logger;
    private readonly HttpClient _httpClient;

    // Configuration constants
    private const int MaxRetryAttempts = 3;
    private const int BaseRetryDelayMinutes = 5;
    private const int HealthCheckTimeoutSeconds = 30;

    public BillerIntegrationService(
        IBillerRepository billerRepository,
        IBillerHealthCheckRepository billerHealthCheckRepository,
        IPaymentRetryRepository paymentRetryRepository,
        IBillPresentmentRepository billPresentmentRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<BillerIntegrationService> logger,
        HttpClient httpClient)
    {
        _billerRepository = billerRepository;
        _billerHealthCheckRepository = billerHealthCheckRepository;
        _paymentRetryRepository = paymentRetryRepository;
        _billPresentmentRepository = billPresentmentRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<BillerPaymentResponse> SendPaymentToBillerAsync(BillerPaymentRequest request)
    {
        var biller = await _billerRepository.GetByIdAsync(request.BillerId);
        if (biller == null)
        {
            return new BillerPaymentResponse
            {
                Success = false,
                Status = BillPaymentStatus.Failed,
                Message = "Biller not found"
            };
        }

        try
        {
            // Check biller health before sending payment
            var healthCheck = await CheckBillerHealthAsync(request.BillerId);
            if (!healthCheck.IsHealthy)
            {
                _logger.LogWarning("Biller {BillerId} is unhealthy, payment may fail", request.BillerId);
            }

            // Simulate external API call based on biller type
            var response = await ProcessPaymentByBillerType(biller, request);
            
            _logger.LogInformation("Payment sent to biller {BillerId}: {PaymentId} - Status: {Status}", 
                request.BillerId, request.PaymentId, response.Status);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending payment {PaymentId} to biller {BillerId}", 
                request.PaymentId, request.BillerId);

            return new BillerPaymentResponse
            {
                Success = false,
                Status = BillPaymentStatus.Failed,
                Message = $"Payment processing failed: {ex.Message}",
                ProcessedDate = DateTime.UtcNow
            };
        }
    }

    public async Task<BillerPaymentStatusResponse> GetPaymentStatusAsync(string externalReference)
    {
        try
        {
            // Simulate external API call to get payment status
            // In a real implementation, this would call the actual biller API
            await Task.Delay(100); // Simulate network delay

            // Mock response based on external reference pattern
            var status = SimulatePaymentStatusCheck(externalReference);
            
            return new BillerPaymentStatusResponse
            {
                ExternalReference = externalReference,
                Status = status,
                LastUpdated = DateTime.UtcNow,
                StatusMessage = GetStatusMessage(status),
                DeliveredDate = status == BillPaymentStatus.Delivered ? DateTime.UtcNow.AddHours(-1) : null
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment status for reference {ExternalReference}", externalReference);
            
            return new BillerPaymentStatusResponse
            {
                ExternalReference = externalReference,
                Status = BillPaymentStatus.Failed,
                LastUpdated = DateTime.UtcNow,
                StatusMessage = "Status check failed"
            };
        }
    }

    public async Task<BillerHealthCheckResponse> CheckBillerHealthAsync(Guid billerId)
    {
        var startTime = DateTime.UtcNow;
        var biller = await _billerRepository.GetByIdAsync(billerId);
        
        if (biller == null)
        {
            return new BillerHealthCheckResponse
            {
                BillerId = billerId,
                IsHealthy = false,
                Status = "Biller not found",
                LastChecked = startTime,
                ResponseTime = TimeSpan.Zero
            };
        }

        try
        {
            // Simulate health check API call
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(HealthCheckTimeoutSeconds));
            
            // Mock health check based on biller category
            var isHealthy = await SimulateBillerHealthCheck(biller, cts.Token);
            var responseTime = DateTime.UtcNow - startTime;

            var healthCheck = new BillerHealthCheck
            {
                BillerId = billerId,
                CheckDate = DateTime.UtcNow
            };

            if (isHealthy)
            {
                healthCheck.MarkAsHealthy(responseTime);
            }
            else
            {
                healthCheck.MarkAsUnhealthy("Simulated health check failure", responseTime);
            }

            await _billerHealthCheckRepository.AddAsync(healthCheck);
            await _unitOfWork.SaveChangesAsync();

            return new BillerHealthCheckResponse
            {
                BillerId = billerId,
                IsHealthy = isHealthy,
                Status = isHealthy ? "Healthy" : "Unhealthy",
                LastChecked = healthCheck.CheckDate,
                ResponseTime = responseTime,
                ErrorMessage = isHealthy ? null : "Simulated health check failure",
                HealthMetrics = new Dictionary<string, object>
                {
                    ["ConsecutiveFailures"] = healthCheck.ConsecutiveFailures,
                    ["LastSuccessfulCheck"] = healthCheck.LastSuccessfulCheck
                }
            };
        }
        catch (OperationCanceledException)
        {
            var responseTime = DateTime.UtcNow - startTime;
            
            var healthCheck = new BillerHealthCheck
            {
                BillerId = billerId,
                CheckDate = DateTime.UtcNow
            };
            healthCheck.MarkAsUnhealthy("Health check timeout", responseTime);

            await _billerHealthCheckRepository.AddAsync(healthCheck);
            await _unitOfWork.SaveChangesAsync();

            return new BillerHealthCheckResponse
            {
                BillerId = billerId,
                IsHealthy = false,
                Status = "Timeout",
                LastChecked = DateTime.UtcNow,
                ResponseTime = responseTime,
                ErrorMessage = "Health check timed out"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking health for biller {BillerId}", billerId);
            
            var responseTime = DateTime.UtcNow - startTime;
            var healthCheck = new BillerHealthCheck
            {
                BillerId = billerId,
                CheckDate = DateTime.UtcNow
            };
            healthCheck.MarkAsUnhealthy(ex.Message, responseTime);

            await _billerHealthCheckRepository.AddAsync(healthCheck);
            await _unitOfWork.SaveChangesAsync();

            return new BillerHealthCheckResponse
            {
                BillerId = billerId,
                IsHealthy = false,
                Status = "Error",
                LastChecked = DateTime.UtcNow,
                ResponseTime = responseTime,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<List<string>> GetSupportedPaymentMethodsAsync(Guid billerId)
    {
        var biller = await _billerRepository.GetByIdAsync(billerId);
        if (biller == null)
        {
            return new List<string>();
        }

        try
        {
            if (!string.IsNullOrEmpty(biller.SupportedPaymentMethods))
            {
                return JsonSerializer.Deserialize<List<string>>(biller.SupportedPaymentMethods) ?? new List<string>();
            }

            // Default payment methods based on biller category
            return biller.Category switch
            {
                BillerCategory.Utilities => new List<string> { "ACH", "Check", "RealTimePayment" },
                BillerCategory.CreditCard => new List<string> { "ACH", "Wire", "RealTimePayment" },
                BillerCategory.Insurance => new List<string> { "ACH", "Check" },
                BillerCategory.Mortgage => new List<string> { "ACH", "Wire", "Check" },
                BillerCategory.Telecom => new List<string> { "ACH", "CreditCard", "DebitCard" },
                _ => new List<string> { "ACH", "Check" }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting supported payment methods for biller {BillerId}", billerId);
            return new List<string> { "ACH" }; // Default fallback
        }
    }

    public async Task<BillerAccountValidationResponse> ValidateBillerAccountAsync(Guid billerId, string accountNumber)
    {
        var biller = await _billerRepository.GetByIdAsync(billerId);
        if (biller == null)
        {
            return new BillerAccountValidationResponse
            {
                IsValid = false,
                Message = "Biller not found"
            };
        }

        try
        {
            // Simulate account validation API call
            await Task.Delay(200); // Simulate network delay

            // Mock validation logic
            var isValid = !string.IsNullOrWhiteSpace(accountNumber) && 
                         accountNumber.Length >= 6 && 
                         accountNumber.Length <= 20;

            return new BillerAccountValidationResponse
            {
                IsValid = isValid,
                AccountStatus = isValid ? "Active" : "Invalid",
                BillerName = biller.Name,
                Message = isValid ? "Account validation successful" : "Invalid account number format",
                ValidationDetails = new Dictionary<string, string>
                {
                    ["AccountNumberLength"] = accountNumber.Length.ToString(),
                    ["BillerCategory"] = biller.Category.ToString(),
                    ["ValidationDate"] = DateTime.UtcNow.ToString("O")
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating account {AccountNumber} for biller {BillerId}", 
                accountNumber, billerId);

            return new BillerAccountValidationResponse
            {
                IsValid = false,
                Message = "Account validation failed",
                BillerName = biller.Name
            };
        }
    }

    public async Task<List<BillPresentmentDto>> GetBillPresentmentAsync(Guid customerId, Guid billerId)
    {
        try
        {
            // In a real implementation, this would call external biller APIs
            // For now, we'll return existing bill presentments from our database
            var presentments = await _billPresentmentRepository.GetUnpaidBillPresentmentsAsync(customerId, billerId);
            
            return presentments.Select(MapToBillPresentmentDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bill presentment for customer {CustomerId} and biller {BillerId}", 
                customerId, billerId);
            return new List<BillPresentmentDto>();
        }
    }

    public async Task<BatchPaymentResponse> ProcessPaymentBatchAsync(List<BillerPaymentRequest> payments)
    {
        var batchId = Guid.NewGuid().ToString("N")[..8];
        var results = new List<BatchPaymentResult>();
        var startTime = DateTime.UtcNow;

        _logger.LogInformation("Processing payment batch {BatchId} with {Count} payments", batchId, payments.Count);

        foreach (var payment in payments)
        {
            try
            {
                var response = await SendPaymentToBillerAsync(payment);
                
                results.Add(new BatchPaymentResult
                {
                    PaymentId = payment.PaymentId,
                    Success = response.Success,
                    ExternalReference = response.ExternalReference,
                    Message = response.Message,
                    Status = response.Status
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment {PaymentId} in batch {BatchId}", 
                    payment.PaymentId, batchId);

                results.Add(new BatchPaymentResult
                {
                    PaymentId = payment.PaymentId,
                    Success = false,
                    Message = $"Batch processing failed: {ex.Message}",
                    Status = BillPaymentStatus.Failed
                });
            }
        }

        var successfulPayments = results.Count(r => r.Success);
        var failedPayments = results.Count - successfulPayments;

        _logger.LogInformation("Batch {BatchId} completed: {Successful} successful, {Failed} failed", 
            batchId, successfulPayments, failedPayments);

        return new BatchPaymentResponse
        {
            BatchId = batchId,
            TotalPayments = payments.Count,
            SuccessfulPayments = successfulPayments,
            FailedPayments = failedPayments,
            ProcessedDate = DateTime.UtcNow,
            Results = results
        };
    }

    public async Task<PaymentRoutingPreferences> GetPaymentRoutingPreferencesAsync(Guid billerId)
    {
        var biller = await _billerRepository.GetByIdAsync(billerId);
        if (biller == null)
        {
            return new PaymentRoutingPreferences
            {
                BillerId = billerId,
                PreferredMethod = PaymentMethod.ACH,
                SupportedMethods = new List<PaymentMethod> { PaymentMethod.ACH }
            };
        }

        // Mock routing preferences based on biller category
        var preferences = biller.Category switch
        {
            BillerCategory.Utilities => new PaymentRoutingPreferences
            {
                BillerId = billerId,
                PreferredMethod = PaymentMethod.ACH,
                SupportedMethods = new List<PaymentMethod> { PaymentMethod.ACH, PaymentMethod.Check, PaymentMethod.RealTimePayment },
                ProcessingWindow = TimeSpan.FromHours(2),
                MaxDailyAmount = 50000m,
                MaxDailyTransactions = 1000,
                RequiresPreAuthorization = false
            },
            BillerCategory.CreditCard => new PaymentRoutingPreferences
            {
                BillerId = billerId,
                PreferredMethod = PaymentMethod.RealTimePayment,
                SupportedMethods = new List<PaymentMethod> { PaymentMethod.ACH, PaymentMethod.Wire, PaymentMethod.RealTimePayment },
                ProcessingWindow = TimeSpan.FromMinutes(30),
                MaxDailyAmount = 100000m,
                MaxDailyTransactions = 500,
                RequiresPreAuthorization = true
            },
            _ => new PaymentRoutingPreferences
            {
                BillerId = billerId,
                PreferredMethod = PaymentMethod.ACH,
                SupportedMethods = new List<PaymentMethod> { PaymentMethod.ACH, PaymentMethod.Check },
                ProcessingWindow = TimeSpan.FromHours(4),
                MaxDailyAmount = 25000m,
                MaxDailyTransactions = 200,
                RequiresPreAuthorization = false
            }
        };

        preferences.RoutingRules = new Dictionary<string, object>
        {
            ["BillerCategory"] = biller.Category.ToString(),
            ["ProcessingDays"] = biller.ProcessingDays,
            ["IsActive"] = biller.IsActive,
            ["LastUpdated"] = DateTime.UtcNow
        };

        return preferences;
    }

    public async Task<List<PaymentStatusSyncResult>> SynchronizePaymentStatusAsync(List<Guid> paymentIds)
    {
        var results = new List<PaymentStatusSyncResult>();

        foreach (var paymentId in paymentIds)
        {
            try
            {
                // This would typically involve calling external APIs to get current status
                // For now, we'll simulate the synchronization
                await Task.Delay(50); // Simulate network delay

                var syncResult = new PaymentStatusSyncResult
                {
                    PaymentId = paymentId,
                    Success = true,
                    OldStatus = BillPaymentStatus.Processing,
                    NewStatus = BillPaymentStatus.Delivered,
                    SyncDate = DateTime.UtcNow,
                    Message = "Status synchronized successfully"
                };

                results.Add(syncResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error synchronizing status for payment {PaymentId}", paymentId);
                
                results.Add(new PaymentStatusSyncResult
                {
                    PaymentId = paymentId,
                    Success = false,
                    SyncDate = DateTime.UtcNow,
                    Message = $"Synchronization failed: {ex.Message}"
                });
            }
        }

        return results;
    }

    #region Private Helper Methods

    private async Task<BillerPaymentResponse> ProcessPaymentByBillerType(Biller biller, BillerPaymentRequest request)
    {
        // Simulate different processing based on biller category
        var processingDelay = biller.Category switch
        {
            BillerCategory.Utilities => 100,
            BillerCategory.CreditCard => 50,
            BillerCategory.Insurance => 200,
            BillerCategory.Mortgage => 300,
            BillerCategory.Telecom => 75,
            _ => 150
        };

        await Task.Delay(processingDelay);

        // Simulate success/failure based on amount (higher amounts have higher chance of additional verification)
        var successRate = request.Amount > 10000 ? 0.85 : 0.95;
        var isSuccess = new Random().NextDouble() < successRate;

        if (isSuccess)
        {
            return new BillerPaymentResponse
            {
                Success = true,
                ExternalReference = $"EXT-{Guid.NewGuid():N}"[..16],
                ConfirmationNumber = $"CNF-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}",
                Status = BillPaymentStatus.Processing,
                ProcessedDate = DateTime.UtcNow,
                Message = "Payment submitted successfully",
                ProcessingFee = CalculateProcessingFee(request.Amount, request.PaymentMethod),
                ExpectedDeliveryDate = DateTime.UtcNow.AddDays(biller.ProcessingDays)
            };
        }
        else
        {
            return new BillerPaymentResponse
            {
                Success = false,
                Status = BillPaymentStatus.Failed,
                ProcessedDate = DateTime.UtcNow,
                Message = "Payment processing failed - insufficient funds or account validation error"
            };
        }
    }

    private async Task<bool> SimulateBillerHealthCheck(Biller biller, CancellationToken cancellationToken)
    {
        // Simulate network delay
        await Task.Delay(new Random().Next(100, 1000), cancellationToken);

        // Simulate health based on biller category (some are more reliable than others)
        var healthRate = biller.Category switch
        {
            BillerCategory.Utilities => 0.95,
            BillerCategory.CreditCard => 0.98,
            BillerCategory.Insurance => 0.90,
            BillerCategory.Mortgage => 0.92,
            BillerCategory.Telecom => 0.88,
            _ => 0.85
        };

        return new Random().NextDouble() < healthRate;
    }

    private static BillPaymentStatus SimulatePaymentStatusCheck(string externalReference)
    {
        // Simulate status progression based on reference pattern
        var hash = externalReference.GetHashCode();
        var statusIndex = Math.Abs(hash) % 5;

        return statusIndex switch
        {
            0 => BillPaymentStatus.Processing,
            1 => BillPaymentStatus.Processed,
            2 => BillPaymentStatus.Delivered,
            3 => BillPaymentStatus.Failed,
            _ => BillPaymentStatus.Processing
        };
    }

    private static string GetStatusMessage(BillPaymentStatus status)
    {
        return status switch
        {
            BillPaymentStatus.Pending => "Payment is pending processing",
            BillPaymentStatus.Processing => "Payment is being processed",
            BillPaymentStatus.Processed => "Payment has been processed successfully",
            BillPaymentStatus.Delivered => "Payment has been delivered to the biller",
            BillPaymentStatus.Failed => "Payment processing failed",
            BillPaymentStatus.Cancelled => "Payment was cancelled",
            BillPaymentStatus.Returned => "Payment was returned by the biller",
            _ => "Unknown status"
        };
    }

    private static decimal? CalculateProcessingFee(decimal amount, PaymentMethod paymentMethod)
    {
        return paymentMethod switch
        {
            PaymentMethod.ACH => Math.Max(0.50m, amount * 0.001m),
            PaymentMethod.Wire => 15.00m,
            PaymentMethod.Check => 2.50m,
            PaymentMethod.RealTimePayment => Math.Max(1.00m, amount * 0.002m),
            PaymentMethod.CreditCard => amount * 0.025m,
            PaymentMethod.DebitCard => amount * 0.015m,
            _ => 1.00m
        };
    }

    private static BillPresentmentDto MapToBillPresentmentDto(BillPresentment presentment)
    {
        var lineItems = new List<BillLineItemDto>();
        
        if (!string.IsNullOrEmpty(presentment.LineItemsJson))
        {
            try
            {
                lineItems = JsonSerializer.Deserialize<List<BillLineItemDto>>(presentment.LineItemsJson) ?? new List<BillLineItemDto>();
            }
            catch
            {
                // If deserialization fails, return empty list
            }
        }

        return new BillPresentmentDto
        {
            Id = presentment.Id,
            CustomerId = presentment.CustomerId,
            BillerId = presentment.BillerId,
            BillerName = presentment.Biller?.Name ?? string.Empty,
            AccountNumber = presentment.AccountNumber,
            AmountDue = presentment.AmountDue,
            MinimumPayment = presentment.MinimumPayment,
            DueDate = presentment.DueDate,
            StatementDate = presentment.StatementDate,
            Currency = presentment.Currency,
            Status = presentment.Status,
            BillNumber = presentment.BillNumber,
            LineItems = lineItems,
            CreatedAt = presentment.CreatedAt,
            PaidDate = presentment.PaidDate
        };
    }

    #endregion
}