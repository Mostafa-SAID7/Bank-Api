using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bank.Application.Services;

/// <summary>
/// Enhanced service implementation for bill payment operations with external integration
/// </summary>
public class BillPaymentService : IBillPaymentService
{
    private readonly IBillerRepository _billerRepository;
    private readonly IBillPaymentRepository _billPaymentRepository;
    private readonly IAccountService _accountService;
    private readonly ITransactionService _transactionService;
    private readonly IBillerIntegrationService _billerIntegrationService;
    private readonly IPaymentRetryService _paymentRetryService;
    private readonly IPaymentReceiptService _paymentReceiptService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BillPaymentService> _logger;

    public BillPaymentService(
        IBillerRepository billerRepository,
        IBillPaymentRepository billPaymentRepository,
        IAccountService accountService,
        ITransactionService transactionService,
        IBillerIntegrationService billerIntegrationService,
        IPaymentRetryService paymentRetryService,
        IPaymentReceiptService paymentReceiptService,
        IUnitOfWork unitOfWork,
        ILogger<BillPaymentService> logger)
    {
        _billerRepository = billerRepository;
        _billPaymentRepository = billPaymentRepository;
        _accountService = accountService;
        _transactionService = transactionService;
        _billerIntegrationService = billerIntegrationService;
        _paymentRetryService = paymentRetryService;
        _paymentReceiptService = paymentReceiptService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<BillerDto>> GetAvailableBillersAsync()
    {
        var billers = await _billerRepository.GetActiveBillersAsync();
        return billers.Select(MapToBillerDto).ToList();
    }

    public async Task<List<BillerDto>> GetBillersByCategoryAsync(BillerCategory category)
    {
        var billers = await _billerRepository.GetBillersByCategoryAsync(category);
        return billers.Select(MapToBillerDto).ToList();
    }

    public async Task<List<BillerDto>> SearchBillersAsync(BillerSearchRequest request)
    {
        List<Biller> billers;

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            billers = await _billerRepository.SearchBillersByNameAsync(request.SearchTerm);
        }
        else if (request.Category.HasValue)
        {
            billers = await _billerRepository.GetBillersByCategoryAsync(request.Category.Value);
        }
        else
        {
            billers = request.ActiveOnly 
                ? await _billerRepository.GetActiveBillersAsync()
                : (await _billerRepository.GetAllAsync()).ToList();
        }

        if (request.ActiveOnly)
        {
            billers = billers.Where(b => b.IsActive).ToList();
        }

        return billers.Select(MapToBillerDto).ToList();
    }

    public async Task<BillerDto?> GetBillerByIdAsync(Guid billerId)
    {
        var biller = await _billerRepository.GetByIdAsync(billerId);
        return biller != null ? MapToBillerDto(biller) : null;
    }

    public async Task<ScheduleBillPaymentResponse> ScheduleBillPaymentAsync(Guid customerId, ScheduleBillPaymentRequest request)
    {
        // Validate the request
        var (isValid, errorMessage) = await ValidateBillPaymentAsync(customerId, request);
        if (!isValid)
        {
            return new ScheduleBillPaymentResponse
            {
                Status = BillPaymentStatus.Failed,
                Message = errorMessage
            };
        }

        var biller = await _billerRepository.GetByIdAsync(request.BillerId);
        if (biller == null)
        {
            return new ScheduleBillPaymentResponse
            {
                Status = BillPaymentStatus.Failed,
                Message = "Biller not found"
            };
        }

        // Create the bill payment
        var billPayment = new BillPayment
        {
            CustomerId = customerId,
            BillerId = request.BillerId,
            Amount = request.Amount,
            Currency = request.Currency,
            ScheduledDate = request.ScheduledDate,
            Status = BillPaymentStatus.Pending,
            Reference = request.Reference ?? string.Empty,
            Description = request.Description ?? string.Empty
        };

        await _billPaymentRepository.AddAsync(billPayment);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Bill payment scheduled: {PaymentId} for customer {CustomerId} to biller {BillerId}", 
            billPayment.Id, customerId, request.BillerId);

        return new ScheduleBillPaymentResponse
        {
            PaymentId = billPayment.Id,
            Status = BillPaymentStatus.Pending,
            ScheduledDate = request.ScheduledDate,
            ExpectedProcessingDate = biller.CalculateProcessingDate(request.ScheduledDate),
            Message = "Bill payment scheduled successfully"
        };
    }
    public async Task<List<ProcessBillPaymentResponse>> ProcessBillPaymentAsync(DateTime? processingDate = null)
    {
        var processDate = processingDate ?? DateTime.UtcNow;
        var duePayments = await _billPaymentRepository.GetScheduledPaymentsDueAsync(processDate);
        var responses = new List<ProcessBillPaymentResponse>();

        foreach (var payment in duePayments)
        {
            try
            {
                // Check if customer has sufficient funds
                var customerAccounts = await _accountService.GetUserAccountsAsync(payment.CustomerId);
                var primaryAccount = customerAccounts.FirstOrDefault(a => a.Type == AccountType.Checking);

                if (primaryAccount == null || primaryAccount.Balance < payment.Amount)
                {
                    payment.MarkAsFailed();
                    
                    // Schedule for retry
                    await _paymentRetryService.SchedulePaymentRetryAsync(new PaymentRetryRequest
                    {
                        PaymentId = payment.Id,
                        RetryAttempt = 1,
                        FailureReason = "Insufficient funds"
                    });

                    responses.Add(new ProcessBillPaymentResponse
                    {
                        PaymentId = payment.Id,
                        Status = BillPaymentStatus.Failed,
                        Message = "Insufficient funds - scheduled for retry",
                        Success = false
                    });
                    continue;
                }

                // Send payment to external biller system
                var billerRequest = new BillerPaymentRequest(
                    payment.Id,
                    payment.BillerId,
                    payment.Biller.AccountNumber,
                    payment.Biller.RoutingNumber,
                    payment.Amount,
                    payment.Currency,
                    payment.Reference,
                    payment.Description,
                    payment.ScheduledDate
                );

                var billerResponse = await _billerIntegrationService.SendPaymentToBillerAsync(billerRequest);

                if (billerResponse.Success)
                {
                    payment.MarkAsProcessed();
                    
                    // Generate payment receipt
                    await _paymentReceiptService.GeneratePaymentReceiptAsync(payment.Id);
                    
                    responses.Add(new ProcessBillPaymentResponse
                    {
                        PaymentId = payment.Id,
                        Status = BillPaymentStatus.Processed,
                        ProcessedDate = payment.ProcessedDate,
                        Message = "Payment processed successfully",
                        Success = true
                    });
                }
                else
                {
                    payment.MarkAsFailed();
                    
                    // Schedule for retry
                    await _paymentRetryService.SchedulePaymentRetryAsync(new PaymentRetryRequest
                    {
                        PaymentId = payment.Id,
                        RetryAttempt = 1,
                        FailureReason = billerResponse.Message
                    });

                    responses.Add(new ProcessBillPaymentResponse
                    {
                        PaymentId = payment.Id,
                        Status = BillPaymentStatus.Failed,
                        Message = $"Payment failed: {billerResponse.Message} - scheduled for retry",
                        Success = false
                    });
                }

                _logger.LogInformation("Bill payment processed: {PaymentId} for amount {Amount} - Status: {Status}", 
                    payment.Id, payment.Amount, payment.Status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing bill payment {PaymentId}", payment.Id);
                payment.MarkAsFailed();
                
                // Schedule for retry
                await _paymentRetryService.SchedulePaymentRetryAsync(new PaymentRetryRequest
                {
                    PaymentId = payment.Id,
                    RetryAttempt = 1,
                    FailureReason = ex.Message
                });

                responses.Add(new ProcessBillPaymentResponse
                {
                    PaymentId = payment.Id,
                    Status = BillPaymentStatus.Failed,
                    Message = "Payment processing failed - scheduled for retry",
                    Success = false
                });
            }

            _billPaymentRepository.Update(payment);
        }

        if (duePayments.Any())
        {
            await _unitOfWork.SaveChangesAsync();
        }

        return responses;
    }

    public async Task<Bank.Domain.Common.PagedResult<BillPaymentHistoryDto>> GetBillPaymentHistoryAsync(Guid customerId, BillPaymentHistoryRequest request)
    {
        var result = await _billPaymentRepository.GetCustomerPaymentHistoryAsync(
            customerId, 
            request.PageNumber, 
            request.PageSize,
            request.FromDate,
            request.ToDate);

        var historyItems = result.Items.Select(MapToBillPaymentHistoryDto).ToList();

        if (request.Status.HasValue)
        {
            historyItems = historyItems.Where(h => h.Status == request.Status.Value).ToList();
        }

        return new Bank.Domain.Common.PagedResult<BillPaymentHistoryDto>
        {
            Items = historyItems,
            TotalCount = result.TotalCount,
            Page = result.Page,
            PageSize = result.PageSize
        };
    }

    public async Task<List<BillPaymentDto>> GetPendingBillPaymentsAsync(Guid customerId)
    {
        var payments = await _billPaymentRepository.GetCustomerPendingPaymentsAsync(customerId);
        return payments.Select(MapToBillPaymentDto).ToList();
    }

    public async Task<bool> CancelScheduledPaymentAsync(Guid customerId, Guid paymentId)
    {
        var payment = await _billPaymentRepository.GetByIdAsync(paymentId);
        
        if (payment == null || payment.CustomerId != customerId)
        {
            return false;
        }

        if (!payment.CanBeCancelled())
        {
            return false;
        }

        payment.Cancel();
        _billPaymentRepository.Update(payment);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Bill payment cancelled: {PaymentId} by customer {CustomerId}", 
            paymentId, customerId);

        return true;
    }

    public async Task<BillPaymentDto?> GetBillPaymentByIdAsync(Guid customerId, Guid paymentId)
    {
        var payment = await _billPaymentRepository.GetPaymentWithDetailsAsync(paymentId);
        
        if (payment == null || payment.CustomerId != customerId)
        {
            return null;
        }

        return MapToBillPaymentDto(payment);
    }

    public async Task<bool> UpdateScheduledPaymentAsync(Guid customerId, Guid paymentId, UpdateBillPaymentRequest request)
    {
        var payment = await _billPaymentRepository.GetByIdAsync(paymentId);
        
        if (payment == null || payment.CustomerId != customerId || !payment.CanBeCancelled())
        {
            return false;
        }

        // Validate the updated amount
        var biller = await _billerRepository.GetByIdAsync(payment.BillerId);
        if (biller != null && !biller.IsAmountValid(request.Amount))
        {
            return false;
        }

        payment.Amount = request.Amount;
        payment.ScheduledDate = request.ScheduledDate;
        payment.Reference = request.Reference ?? payment.Reference;
        payment.Description = request.Description ?? payment.Description;

        _billPaymentRepository.Update(payment);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<(bool IsValid, string ErrorMessage)> ValidateBillPaymentAsync(Guid customerId, ScheduleBillPaymentRequest request)
    {
        // Check if biller exists and is active
        var biller = await _billerRepository.GetByIdAsync(request.BillerId);
        if (biller == null)
        {
            return (false, "Biller not found");
        }

        if (!biller.IsActive)
        {
            return (false, "Biller is not active");
        }

        // Validate amount
        if (!biller.IsAmountValid(request.Amount))
        {
            return (false, $"Amount must be between {biller.MinAmount:C} and {biller.MaxAmount:C}");
        }

        // Validate scheduled date
        if (request.ScheduledDate.Date < DateTime.UtcNow.Date)
        {
            return (false, "Scheduled date cannot be in the past");
        }

        // Check if customer has sufficient funds (basic check)
        var customerAccounts = await _accountService.GetUserAccountsAsync(customerId);
        var totalBalance = customerAccounts.Where(a => a.Status == AccountStatus.Active).Sum(a => a.Balance);
        
        if (totalBalance < request.Amount)
        {
            return (false, "Insufficient funds");
        }

        return (true, string.Empty);
    }

    #region Private Helper Methods

    private static BillerDto MapToBillerDto(Biller biller)
    {
        string[] supportedMethods = Array.Empty<string>();
        
        if (!string.IsNullOrEmpty(biller.SupportedPaymentMethods))
        {
            try
            {
                supportedMethods = JsonSerializer.Deserialize<string[]>(biller.SupportedPaymentMethods) ?? Array.Empty<string>();
            }
            catch
            {
                // If deserialization fails, return empty array
            }
        }

        return new BillerDto
        {
            Id = biller.Id,
            Name = biller.Name,
            Category = biller.Category,
            AccountNumber = biller.AccountNumber,
            RoutingNumber = biller.RoutingNumber,
            Address = biller.Address,
            IsActive = biller.IsActive,
            SupportedPaymentMethods = supportedMethods,
            MinAmount = biller.MinAmount,
            MaxAmount = biller.MaxAmount,
            ProcessingDays = biller.ProcessingDays,
            CreatedAt = biller.CreatedAt
        };
    }

    private static BillPaymentDto MapToBillPaymentDto(BillPayment payment)
    {
        return new BillPaymentDto
        {
            Id = payment.Id,
            CustomerId = payment.CustomerId,
            BillerId = payment.BillerId,
            BillerName = payment.Biller?.Name ?? string.Empty,
            BillerCategory = payment.Biller?.Category ?? BillerCategory.Other,
            Amount = payment.Amount,
            Currency = payment.Currency,
            ScheduledDate = payment.ScheduledDate,
            ProcessedDate = payment.ProcessedDate,
            Status = payment.Status,
            Reference = payment.Reference,
            Description = payment.Description,
            RecurringPaymentId = payment.RecurringPaymentId,
            CreatedAt = payment.CreatedAt
        };
    }

    private static BillPaymentHistoryDto MapToBillPaymentHistoryDto(BillPayment payment)
    {
        return new BillPaymentHistoryDto
        {
            Id = payment.Id,
            BillerName = payment.Biller?.Name ?? string.Empty,
            BillerCategory = payment.Biller?.Category ?? BillerCategory.Other,
            Amount = payment.Amount,
            Currency = payment.Currency,
            ScheduledDate = payment.ScheduledDate,
            ProcessedDate = payment.ProcessedDate,
            Status = payment.Status,
            Reference = payment.Reference,
            Description = payment.Description,
            IsRecurring = payment.RecurringPaymentId.HasValue
        };
    }

    #endregion
}