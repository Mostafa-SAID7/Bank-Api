using Bank.Application.DTOs;

namespace Bank.Application.Interfaces;

/// <summary>
/// Service for managing deposit certificates (generation, delivery, retrieval)
/// </summary>
public interface IDepositCertificateService
{
    Task<DepositCertificateDto> GenerateCertificateAsync(Guid depositId, Guid generatedByUserId);
    Task<DepositCertificateDto?> GetCertificateAsync(Guid certificateId);
    Task<byte[]> GetCertificatePdfAsync(Guid certificateId);
    Task<bool> DeliverCertificateAsync(Guid certificateId, string deliveryMethod, string deliveryAddress, Guid deliveredByUserId);
}
