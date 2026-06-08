using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Interface for deposit certificate generation and management
/// Handles all certificate-related operations for fixed deposits
/// </summary>
public interface IDepositCertificateGenerator
{
    /// <summary>
    /// Generates a new deposit certificate for a fixed deposit
    /// </summary>
    Task<DepositCertificateDto> GenerateCertificateAsync(Guid depositId, Guid generatedByUserId);

    /// <summary>
    /// Retrieves a certificate by ID
    /// </summary>
    Task<DepositCertificateDto?> GetCertificateAsync(Guid certificateId);

    /// <summary>
    /// Retrieves all certificates for a fixed deposit
    /// </summary>
    Task<IEnumerable<DepositCertificateDto>> GetCertificatesByDepositAsync(Guid fixedDepositId);

    /// <summary>
    /// Marks certificate as issued and delivered
    /// </summary>
    Task<bool> IssueCertificateAsync(Guid certificateId, string deliveryMethod, string? deliveryAddress, Guid issuedByUserId);

    /// <summary>
    /// Marks certificate as delivered
    /// </summary>
    Task<bool> MarkAsDeliveredAsync(Guid certificateId, string deliveryReference);

    /// <summary>
    /// Replaces/cancels a certificate due to damage, loss, or requested changes
    /// </summary>
    Task<DepositCertificateDto> ReplaceCertificateAsync(Guid certificateId, string reason, Guid replacedByUserId);

    /// <summary>
    /// Retrieves certificate PDF (if available)
    /// </summary>
    Task<byte[]?> GetCertificatePdfAsync(Guid certificateId);
}
