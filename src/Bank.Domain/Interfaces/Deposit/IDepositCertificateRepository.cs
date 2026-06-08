using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for DepositCertificate entity
/// </summary>
public interface IDepositCertificateRepository : IRepository<DepositCertificate>
{
    Task<IEnumerable<DepositCertificate>> GetCertificatesByDepositAsync(Guid depositId);
    Task<DepositCertificate?> GetByCertificateNumberAsync(string certificateNumber);
    Task<IEnumerable<DepositCertificate>> GetPendingDeliveryAsync();
}
