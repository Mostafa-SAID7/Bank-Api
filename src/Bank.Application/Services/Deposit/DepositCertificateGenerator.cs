using Bank.Application.DTOs;
using Bank.Application.Helpers.Deposit;
using Bank.Application.Helpers.Shared;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace Bank.Application.Services;

/// <summary>
/// Specialized service for deposit certificate generation and management
/// Centralizes all certificate-related generation logic to eliminate duplication
/// </summary>
public sealed class DepositCertificateGenerator : IDepositCertificateGenerator
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<DepositCertificateGenerator> _logger;

    public DepositCertificateGenerator(
        IUnitOfWork unitOfWork,
        IAuditLogService auditLogService,
        ILogger<DepositCertificateGenerator> logger)
    {
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    /// <summary>
    /// Generates a new deposit certificate for a fixed deposit
    /// </summary>
    public async Task<DepositCertificateDto> GenerateCertificateAsync(Guid depositId, Guid generatedByUserId)
    {
        var deposit = await _unitOfWork.Repository<FixedDeposit>()
            .GetByIdAsync(depositId);
        
        if (deposit == null)
            throw new InvalidOperationException($"Fixed deposit {depositId} not found");

        var certificate = new DepositCertificate
        {
            FixedDepositId = depositId,
            Status = DepositCertificateStatus.Generated,
            IssueDate = DateTime.UtcNow,
            CertificateTemplate = "StandardDepositCertificate",
            CertificateContent = GenerateCertificateContent(deposit),
            GeneratedByUserId = generatedByUserId,
            CertificateNumber = GeneratorHelper.GenerateCertificateNumber()
        };

        // Generate security hash after setting certificate number
        GenerateSecurityHash(certificate);

        await _unitOfWork.Repository<DepositCertificate>().AddAsync(certificate);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            generatedByUserId,
            "DepositCertificate",
            "Generate",
            certificate.Id.ToString(),
            $"Generated certificate {certificate.CertificateNumber} for deposit {deposit.DepositNumber}");

        _logger.LogInformation("Generated certificate {CertificateNumber} for deposit {DepositId}", 
            certificate.CertificateNumber, depositId);

        return DepositMappingHelper.MapToDepositCertificateDto(certificate);
    }

    /// <summary>
    /// Retrieves a certificate by ID
    /// </summary>
    public async Task<DepositCertificateDto?> GetCertificateAsync(Guid certificateId)
    {
        var certificate = await _unitOfWork.Repository<DepositCertificate>().GetByIdAsync(certificateId);
        return certificate == null ? null : DepositMappingHelper.MapToDepositCertificateDto(certificate);
    }

    /// <summary>
    /// Retrieves all certificates for a fixed deposit
    /// </summary>
    public async Task<IEnumerable<DepositCertificateDto>> GetCertificatesByDepositAsync(Guid fixedDepositId)
    {
        var certificates = await _unitOfWork.Repository<DepositCertificate>()
            .FindAsync(c => c.FixedDepositId == fixedDepositId);

        return certificates
            .OrderByDescending(c => c.IssueDate)
            .Select(DepositMappingHelper.MapToDepositCertificateDto);
    }

    /// <summary>
    /// Marks certificate as issued and delivered
    /// </summary>
    public async Task<bool> IssueCertificateAsync(Guid certificateId, string deliveryMethod, string? deliveryAddress, Guid issuedByUserId)
    {
        var certificate = await _unitOfWork.Repository<DepositCertificate>().GetByIdAsync(certificateId);
        if (certificate == null)
            return false;

        certificate.Status = DepositCertificateStatus.Issued;
        certificate.DeliveryMethod = deliveryMethod;
        certificate.DeliveryAddress = deliveryAddress;
        certificate.IssuedByUserId = issuedByUserId;

        _unitOfWork.Repository<DepositCertificate>().Update(certificate);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            issuedByUserId,
            "DepositCertificate",
            "Issue",
            certificateId.ToString(),
            $"Issued certificate {certificate.CertificateNumber} via {deliveryMethod}");

        _logger.LogInformation("Issued certificate {CertificateNumber} via {DeliveryMethod}", 
            certificate.CertificateNumber, deliveryMethod);

        return true;
    }

    /// <summary>
    /// Marks certificate as delivered
    /// </summary>
    public async Task<bool> MarkAsDeliveredAsync(Guid certificateId, string deliveryReference)
    {
        var certificate = await _unitOfWork.Repository<DepositCertificate>().GetByIdAsync(certificateId);
        if (certificate == null)
            return false;

        certificate.MarkAsDelivered(deliveryReference);
        _unitOfWork.Repository<DepositCertificate>().Update(certificate);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Marked certificate {CertificateNumber} as delivered with reference {DeliveryReference}",
            certificate.CertificateNumber, deliveryReference);

        return true;
    }

    /// <summary>
    /// Replaces/cancels a certificate due to damage, loss, or requested changes
    /// </summary>
    public async Task<DepositCertificateDto> ReplaceCertificateAsync(Guid certificateId, string reason, Guid replacedByUserId)
    {
        var oldCertificate = await _unitOfWork.Repository<DepositCertificate>().GetByIdAsync(certificateId);
        if (oldCertificate == null)
            throw new InvalidOperationException($"Certificate {certificateId} not found");

        // Cancel the old certificate
        oldCertificate.Cancel(reason, replacedByUserId);
        _unitOfWork.Repository<DepositCertificate>().Update(oldCertificate);

        // Generate new certificate
        var deposit = await _unitOfWork.Repository<FixedDeposit>().GetByIdAsync(oldCertificate.FixedDepositId);
        if (deposit == null)
            throw new InvalidOperationException($"Fixed deposit {oldCertificate.FixedDepositId} not found");

        var newCertificate = new DepositCertificate
        {
            FixedDepositId = oldCertificate.FixedDepositId,
            Status = DepositCertificateStatus.Generated,
            IssueDate = DateTime.UtcNow,
            CertificateTemplate = oldCertificate.CertificateTemplate,
            CertificateContent = GenerateCertificateContent(deposit),
            GeneratedByUserId = replacedByUserId,
            CertificateNumber = GeneratorHelper.GenerateCertificateNumber(),
            ReplacedCertificateId = certificateId
        };

        GenerateSecurityHash(newCertificate);

        await _unitOfWork.Repository<DepositCertificate>().AddAsync(newCertificate);
        await _unitOfWork.SaveChangesAsync();

        await _auditLogService.LogUserActionAsync(
            replacedByUserId,
            "DepositCertificate",
            "Replace",
            newCertificate.Id.ToString(),
            $"Replaced certificate {oldCertificate.CertificateNumber} with {newCertificate.CertificateNumber}. Reason: {reason}");

        _logger.LogInformation("Replaced certificate {OldCertificateNumber} with {NewCertificateNumber}. Reason: {Reason}",
            oldCertificate.CertificateNumber, newCertificate.CertificateNumber, reason);

        return DepositMappingHelper.MapToDepositCertificateDto(newCertificate);
    }

    /// <summary>
    /// Retrieves certificate PDF (if available)
    /// </summary>
    public async Task<byte[]?> GetCertificatePdfAsync(Guid certificateId)
    {
        var certificate = await _unitOfWork.Repository<DepositCertificate>().GetByIdAsync(certificateId);
        if (certificate?.CertificatePdf == null)
            return null;

        _logger.LogInformation("Retrieved PDF for certificate {CertificateNumber}", certificate.CertificateNumber);
        return certificate.CertificatePdf;
    }

    /// <summary>
    /// Generates certificate content HTML
    /// </summary>
    private static string GenerateCertificateContent(FixedDeposit deposit)
    {
        return $@"
DEPOSIT CERTIFICATE

Certificate Number: {deposit.DepositNumber}
Customer: {deposit.Customer?.UserName ?? "Unknown"}
Principal Amount: {deposit.PrincipalAmount:C}
Interest Rate: {deposit.InterestRate}%
Term: {deposit.TermDays} days
Start Date: {deposit.StartDate:yyyy-MM-dd}
Maturity Date: {deposit.MaturityDate:yyyy-MM-dd}
Maturity Amount: {deposit.CalculateMaturityAmount():C}

This certificate confirms the deposit details above.
";
    }

    /// <summary>
    /// Generates security hash for certificate verification
    /// </summary>
    private static void GenerateSecurityHash(DepositCertificate certificate)
    {
        var content = $"{certificate.CertificateNumber}{certificate.FixedDepositId}{certificate.IssueDate:yyyyMMddHHmmss}";
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
        certificate.SecurityHash = Convert.ToBase64String(hashBytes);
    }
}
