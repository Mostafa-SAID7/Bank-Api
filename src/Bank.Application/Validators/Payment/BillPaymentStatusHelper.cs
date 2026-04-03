using Bank.Domain.Enums;

namespace Bank.Application.Validators.Payment;

/// <summary>
/// Helper for bill payment status messages
/// </summary>
public static class BillPaymentStatusHelper
{
    /// <summary>
    /// Gets status message for bill payment status
    /// </summary>
    /// <param name="status">Bill payment status</param>
    /// <returns>Status message</returns>
    public static string GetStatusMessage(BillPaymentStatus status)
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
}
