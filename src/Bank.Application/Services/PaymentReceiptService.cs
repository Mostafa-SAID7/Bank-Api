using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Application.Utilities;
using Bank.Domain.Common;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text.Json;

namespace Bank.Application.Services;

/// <summary>
/// Service implementation for payment receipt generation and management
/// </summary>
public class PaymentReceiptService : IPaymentReceiptService
{
    private readonly IPaymentReceiptRepository _paymentReceiptRepository;
    private readonly IBillPaymentRepository _billPaymentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PaymentReceiptService> _logger;

    public PaymentReceiptService(
        IPaymentReceiptRepository paymentReceiptRepository,
        IBillPaymentRepository billPaymentRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<PaymentReceiptService> logger)
    {
        _paymentReceiptRepository = paymentReceiptRepository;
        _billPaymentRepository = billPaymentRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PaymentReceiptDto> GeneratePaymentReceiptAsync(Guid paymentId)
    {
        try
        {
            // Check if receipt already exists
            var existingReceipt = await _paymentReceiptRepository.GetByPaymentIdAsync(paymentId);
            if (existingReceipt != null)
            {
                return MapToPaymentReceiptDto(existingReceipt);
            }

            var payment = await _billPaymentRepository.GetPaymentWithDetailsAsync(paymentId);
            if (payment == null)
            {
                throw new InvalidOperationException($"Payment {paymentId} not found");
            }

            var customer = await _userRepository.GetByIdAsync(payment.CustomerId);
            if (customer == null)
            {
                throw new InvalidOperationException($"Customer {payment.CustomerId} not found");
            }

            // Generate unique receipt number
            string receiptNumber;
            do
            {
                receiptNumber = PaymentReceipt.GenerateReceiptNumber();
            } while (await _paymentReceiptRepository.ReceiptNumberExistsAsync(receiptNumber));

            // Create receipt
            var receipt = new PaymentReceipt
            {
                PaymentId = paymentId,
                ReceiptNumber = receiptNumber,
                CustomerId = payment.CustomerId,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                BillerName = payment.Biller?.Name ?? "Unknown Biller",
                Amount = payment.Amount,
                Currency = payment.Currency,
                PaymentDate = payment.ScheduledDate,
                ProcessedDate = payment.ProcessedDate ?? DateTime.UtcNow,
                ConfirmationNumber = GenerateConfirmationNumber(),
                Reference = payment.Reference,
                PaymentMethod = Bank.Domain.Enums.PaymentMethod.ACH, // Default, could be enhanced to track actual method
                Status = payment.Status,
                ReceiptDataJson = JsonSerializer.Serialize(new
                {
                    BillerCategory = payment.Biller?.Category.ToString(),
                    BillerAddress = payment.Biller?.Address,
                    PaymentDescription = payment.Description,
                    GeneratedAt = DateTime.UtcNow,
                    GeneratedBy = "System"
                })
            };

            await _paymentReceiptRepository.AddAsync(receipt);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Payment receipt generated: {ReceiptNumber} for payment {PaymentId}", 
                receiptNumber, paymentId);

            return MapToPaymentReceiptDto(receipt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating payment receipt for payment {PaymentId}", paymentId);
            throw;
        }
    }

    public async Task<PaymentReceiptDto?> GetReceiptByNumberAsync(string receiptNumber)
    {
        try
        {
            var receipt = await _paymentReceiptRepository.GetByReceiptNumberAsync(receiptNumber);
            return receipt != null ? MapToPaymentReceiptDto(receipt) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting receipt by number {ReceiptNumber}", receiptNumber);
            return null;
        }
    }

    public async Task<PaymentReceiptDto?> GetReceiptByPaymentIdAsync(Guid paymentId)
    {
        try
        {
            var receipt = await _paymentReceiptRepository.GetByPaymentIdAsync(paymentId);
            return receipt != null ? MapToPaymentReceiptDto(receipt) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting receipt by payment ID {PaymentId}", paymentId);
            return null;
        }
    }

    public async Task<Domain.Common.PagedResult<PaymentReceiptDto>> GetCustomerReceiptsAsync(
        Guid customerId, 
        int pageNumber = 1, 
        int pageSize = 20,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        try
        {
            var result = await _paymentReceiptRepository.GetCustomerReceiptsAsync(
                customerId, pageNumber, pageSize, fromDate, toDate);

            var receiptDtos = result.Items.Select(MapToPaymentReceiptDto).ToList();

            return new Bank.Domain.Common.PagedResult<PaymentReceiptDto>
            {
                Items = receiptDtos,
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer receipts for customer {CustomerId}", customerId);
            return new Bank.Domain.Common.PagedResult<PaymentReceiptDto>
            {
                Items = new List<PaymentReceiptDto>(),
                TotalCount = 0,
                Page = pageNumber,
                PageSize = pageSize
            };
        }
    }

    public async Task<bool> UpdateReceiptStatusAsync(Guid paymentId, BillPaymentStatus status)
    {
        try
        {
            var receipt = await _paymentReceiptRepository.GetByPaymentIdAsync(paymentId);
            if (receipt == null)
            {
                _logger.LogWarning("Receipt not found for payment {PaymentId} when updating status", paymentId);
                return false;
            }

            receipt.UpdateStatus(status);
            _paymentReceiptRepository.Update(receipt);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Receipt {ReceiptNumber} status updated to {Status}", 
                receipt.ReceiptNumber, status);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating receipt status for payment {PaymentId}", paymentId);
            return false;
        }
    }

    public async Task<byte[]> GenerateReceiptPdfAsync(string receiptNumber)
    {
        try
        {
            var receipt = await _paymentReceiptRepository.GetByReceiptNumberAsync(receiptNumber);
            if (receipt == null)
            {
                throw new InvalidOperationException($"Receipt {receiptNumber} not found");
            }

            // In a real implementation, this would use a PDF generation library like iTextSharp or PdfSharp
            // For now, we'll return a mock PDF content
            var pdfContent = GenerateMockPdfContent(receipt);
            
            _logger.LogInformation("PDF generated for receipt {ReceiptNumber}", receiptNumber);
            
            return pdfContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF for receipt {ReceiptNumber}", receiptNumber);
            throw;
        }
    }

    public async Task<bool> ResendReceiptAsync(string receiptNumber, string deliveryMethod = "email")
    {
        try
        {
            var receipt = await _paymentReceiptRepository.GetByReceiptNumberAsync(receiptNumber);
            if (receipt == null)
            {
                _logger.LogWarning("Receipt {ReceiptNumber} not found for resend", receiptNumber);
                return false;
            }

            // In a real implementation, this would integrate with email/SMS services
            // For now, we'll simulate the resend operation
            await Task.Delay(100); // Simulate processing time

            _logger.LogInformation("Receipt {ReceiptNumber} resent via {DeliveryMethod} to customer {CustomerId}", 
                receiptNumber, deliveryMethod, receipt.CustomerId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resending receipt {ReceiptNumber}", receiptNumber);
            return false;
        }
    }

    public async Task<bool> ValidateReceiptAsync(string receiptNumber, string confirmationNumber)
    {
        try
        {
            var receipt = await _paymentReceiptRepository.GetByReceiptNumberAsync(receiptNumber);
            if (receipt == null)
            {
                return false;
            }

            var isValid = receipt.ConfirmationNumber.Equals(confirmationNumber, StringComparison.OrdinalIgnoreCase);
            
            _logger.LogInformation("Receipt validation for {ReceiptNumber}: {IsValid}", receiptNumber, isValid);
            
            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating receipt {ReceiptNumber}", receiptNumber);
            return false;
        }
    }

    #region Private Helper Methods

    private static PaymentReceiptDto MapToPaymentReceiptDto(PaymentReceipt receipt)
    {
        return new PaymentReceiptDto
        {
            PaymentId = receipt.PaymentId,
            ReceiptNumber = receipt.ReceiptNumber,
            CustomerId = receipt.CustomerId,
            CustomerName = receipt.CustomerName,
            BillerName = receipt.BillerName,
            Amount = receipt.Amount,
            Currency = receipt.Currency,
            PaymentDate = receipt.PaymentDate,
            ProcessedDate = receipt.ProcessedDate,
            ConfirmationNumber = receipt.ConfirmationNumber,
            Reference = receipt.Reference,
            PaymentMethod = receipt.PaymentMethod,
            ProcessingFee = receipt.ProcessingFee,
            Status = receipt.Status,
            CreatedAt = receipt.CreatedAt
        };
    }

    private static string GenerateConfirmationNumber()
    {
        return TokenGenerationHelper.GenerateConfirmationNumber();
    }

    private static byte[] GenerateMockPdfContent(PaymentReceipt receipt)
    {
        // In a real implementation, this would generate an actual PDF
        // For now, we'll return mock content that represents a PDF structure
        var pdfContent = $@"
%PDF-1.4
1 0 obj
<<
/Type /Catalog
/Pages 2 0 R
>>
endobj

2 0 obj
<<
/Type /Pages
/Kids [3 0 R]
/Count 1
>>
endobj

3 0 obj
<<
/Type /Page
/Parent 2 0 R
/MediaBox [0 0 612 792]
/Contents 4 0 R
>>
endobj

4 0 obj
<<
/Length 200
>>
stream
BT
/F1 12 Tf
50 750 Td
(PAYMENT RECEIPT) Tj
0 -20 Td
(Receipt Number: {receipt.ReceiptNumber}) Tj
0 -20 Td
(Customer: {receipt.CustomerName}) Tj
0 -20 Td
(Biller: {receipt.BillerName}) Tj
0 -20 Td
(Amount: {receipt.Currency} {receipt.Amount:F2}) Tj
0 -20 Td
(Date: {receipt.ProcessedDate:yyyy-MM-dd}) Tj
0 -20 Td
(Confirmation: {receipt.ConfirmationNumber}) Tj
ET
endstream
endobj

xref
0 5
0000000000 65535 f 
0000000010 00000 n 
0000000053 00000 n 
0000000110 00000 n 
0000000205 00000 n 
trailer
<<
/Size 5
/Root 1 0 R
>>
startxref
456
%%EOF";

        return System.Text.Encoding.UTF8.GetBytes(pdfContent);
    }

    #endregion
}