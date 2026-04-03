using Bank.Domain.Entities;

namespace Bank.Application.Interfaces.Payment;

/// <summary>
/// Interface for PaymentReceipt generation service
/// Defines contract for generating payment receipt content and confirmation numbers
/// </summary>
public interface IPaymentReceiptGenerationService
{
    /// <summary>
    /// Generates a unique receipt confirmation number
    /// </summary>
    /// <returns>A unique confirmation number string</returns>
    string GenerateReceiptConfirmationNumber();

    /// <summary>
    /// Generates PDF content for a payment receipt
    /// </summary>
    /// <param name="receipt">The PaymentReceipt entity to generate PDF for</param>
    /// <returns>PDF content as byte array</returns>
    byte[] GenerateReceiptPdfContent(PaymentReceipt receipt);
}
