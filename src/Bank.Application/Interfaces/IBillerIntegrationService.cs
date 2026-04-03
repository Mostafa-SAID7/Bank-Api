using Bank.Application.DTOs;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service interface for external biller system integration
/// </summary>
public interface IBillerIntegrationService
{
    /// <summary>
    /// Send payment to external biller system
    /// </summary>
    Task<BillerPaymentResponse> SendPaymentToBillerAsync(BillerPaymentRequest request);

    /// <summary>
    /// Get payment status from external biller system
    /// </summary>
    Task<BillerPaymentStatusResponse> GetPaymentStatusAsync(string externalReference);

    /// <summary>
    /// Check biller connectivity and health
    /// </summary>
    Task<BillerHealthCheckResponse> CheckBillerHealthAsync(Guid billerId);

    /// <summary>
    /// Get supported payment methods for a biller
    /// </summary>
    Task<List<string>> GetSupportedPaymentMethodsAsync(Guid billerId);

    /// <summary>
    /// Validate biller account information
    /// </summary>
    Task<BillerAccountValidationResponse> ValidateBillerAccountAsync(Guid billerId, string accountNumber);

    /// <summary>
    /// Get bill presentment data from participating billers
    /// </summary>
    Task<List<BillPresentmentDto>> GetBillPresentmentAsync(Guid customerId, Guid billerId);

    /// <summary>
    /// Process payment batch to external systems
    /// </summary>
    Task<BatchPaymentResponse> ProcessPaymentBatchAsync(List<BillerPaymentRequest> payments);

    /// <summary>
    /// Get payment routing preferences for a biller
    /// </summary>
    Task<PaymentRoutingPreferences> GetPaymentRoutingPreferencesAsync(Guid billerId);

    /// <summary>
    /// Synchronize payment status with external systems
    /// </summary>
    Task<List<PaymentStatusSyncResult>> SynchronizePaymentStatusAsync(List<Guid> paymentIds);
}