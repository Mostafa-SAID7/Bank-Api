using Bank.Application.DTOs;
using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bank.Application.Services;

/// <summary>
/// Service for managing beneficiaries (payees) for fund transfers
/// </summary>
public class BeneficiaryService : IBeneficiaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<BeneficiaryService> _logger;

    public BeneficiaryService(
        IUnitOfWork unitOfWork,
        IAuditLogService auditLogService,
        ILogger<BeneficiaryService> logger)
    {
        _unitOfWork = unitOfWork;
        _auditLogService = auditLogService;
        _logger = logger;
    }

    public async Task<BeneficiaryResult> AddBeneficiaryAsync(Guid customerId, AddBeneficiaryRequest request)
    {
        try
        {
            // Validate account details first
            var validationResult = await ValidateAccountDetailsAsync(request);
            if (!validationResult.Success)
            {
                return new BeneficiaryResult
                {
                    Success = false,
                    Message = "Account validation failed",
                    Errors = validationResult.ValidationErrors
                };
            }

            // Check for duplicate beneficiary
            var existingBeneficiary = await _unitOfWork.Repository<Beneficiary>()
                .FirstOrDefaultAsync(b => b.CustomerId == customerId && 
                                        b.AccountNumber == request.AccountNumber && 
                                        b.BankCode == request.BankCode &&
                                        b.IsActive);

            if (existingBeneficiary != null)
            {
                return new BeneficiaryResult
                {
                    Success = false,
                    Message = "Beneficiary with this account already exists"
                };
            }

            var beneficiary = new Beneficiary
            {
                CustomerId = customerId,
                Name = request.Name,
                AccountNumber = request.AccountNumber,
                AccountName = request.AccountName,
                BankName = request.BankName,
                BankCode = request.BankCode,
                SwiftCode = request.SwiftCode,
                IbanNumber = request.IbanNumber,
                RoutingNumber = request.RoutingNumber,
                Type = request.Type,
                Category = request.Category,
                DailyTransferLimit = request.DailyTransferLimit,
                MonthlyTransferLimit = request.MonthlyTransferLimit,
                SingleTransferLimit = request.SingleTransferLimit,
                Notes = request.Notes,
                Reference = request.Reference,
                Status = BeneficiaryStatus.Pending
            };

            await _unitOfWork.Repository<Beneficiary>().AddAsync(beneficiary);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("Beneficiary Added", 
                $"New beneficiary {request.Name} added for customer {customerId}", customerId);

            _logger.LogInformation("Beneficiary {BeneficiaryName} added for customer {CustomerId}", 
                request.Name, customerId);

            return new BeneficiaryResult
            {
                Success = true,
                Message = "Beneficiary added successfully",
                Beneficiary = MapToDto(beneficiary)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding beneficiary for customer {CustomerId}", customerId);
            return new BeneficiaryResult
            {
                Success = false,
                Message = "An error occurred while adding beneficiary"
            };
        }
    }

    public async Task<BeneficiaryResult> UpdateBeneficiaryAsync(Guid beneficiaryId, UpdateBeneficiaryRequest request, Guid updatedByUserId)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            if (beneficiary == null)
            {
                return new BeneficiaryResult
                {
                    Success = false,
                    Message = "Beneficiary not found"
                };
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(request.Name))
                beneficiary.Name = request.Name;
            
            if (!string.IsNullOrEmpty(request.AccountName))
                beneficiary.AccountName = request.AccountName;
            
            if (!string.IsNullOrEmpty(request.BankName))
                beneficiary.BankName = request.BankName;
            
            if (request.Category.HasValue)
                beneficiary.Category = request.Category.Value;
            
            if (request.DailyTransferLimit.HasValue)
                beneficiary.DailyTransferLimit = request.DailyTransferLimit;
            
            if (request.MonthlyTransferLimit.HasValue)
                beneficiary.MonthlyTransferLimit = request.MonthlyTransferLimit;
            
            if (request.SingleTransferLimit.HasValue)
                beneficiary.SingleTransferLimit = request.SingleTransferLimit;
            
            if (!string.IsNullOrEmpty(request.Notes))
                beneficiary.Notes = request.Notes;
            
            if (!string.IsNullOrEmpty(request.Reference))
                beneficiary.Reference = request.Reference;

            _unitOfWork.Repository<Beneficiary>().Update(beneficiary);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("Beneficiary Updated", 
                $"Beneficiary {beneficiary.Name} updated", updatedByUserId);

            return new BeneficiaryResult
            {
                Success = true,
                Message = "Beneficiary updated successfully",
                Beneficiary = MapToDto(beneficiary)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating beneficiary {BeneficiaryId}", beneficiaryId);
            return new BeneficiaryResult
            {
                Success = false,
                Message = "An error occurred while updating beneficiary"
            };
        }
    }

    public async Task<BeneficiaryDto?> GetBeneficiaryByIdAsync(Guid beneficiaryId)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            return beneficiary != null ? MapToDto(beneficiary) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving beneficiary {BeneficiaryId}", beneficiaryId);
            return null;
        }
    }

    public async Task<List<BeneficiaryDto>> GetCustomerBeneficiariesAsync(Guid customerId, bool activeOnly = true)
    {
        try
        {
            var beneficiaries = await _unitOfWork.Repository<Beneficiary>()
                .FindAsync(b => b.CustomerId == customerId && (!activeOnly || b.IsActive));

            return beneficiaries.Select(MapToDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving beneficiaries for customer {CustomerId}", customerId);
            return new List<BeneficiaryDto>();
        }
    }

    public async Task<BeneficiarySearchResult> SearchBeneficiariesAsync(BeneficiarySearchCriteria criteria)
    {
        try
        {
            var query = _unitOfWork.Repository<Beneficiary>().Query()
                .Where(b => b.CustomerId == criteria.CustomerId);

            // Apply filters
            if (!string.IsNullOrEmpty(criteria.Name))
                query = query.Where(b => b.Name.Contains(criteria.Name));

            if (!string.IsNullOrEmpty(criteria.AccountNumber))
                query = query.Where(b => b.AccountNumber.Contains(criteria.AccountNumber));

            if (!string.IsNullOrEmpty(criteria.BankCode))
                query = query.Where(b => b.BankCode == criteria.BankCode);

            if (criteria.Type.HasValue)
                query = query.Where(b => b.Type == criteria.Type.Value);

            if (criteria.Category.HasValue)
                query = query.Where(b => b.Category == criteria.Category.Value);

            if (criteria.Status.HasValue)
                query = query.Where(b => b.Status == criteria.Status.Value);

            if (criteria.IsVerified.HasValue)
                query = query.Where(b => b.IsVerified == criteria.IsVerified.Value);

            if (criteria.IsActive.HasValue)
                query = query.Where(b => b.IsActive == criteria.IsActive.Value);

            if (criteria.CreatedFrom.HasValue)
                query = query.Where(b => b.CreatedAt >= criteria.CreatedFrom.Value);

            if (criteria.CreatedTo.HasValue)
                query = query.Where(b => b.CreatedAt <= criteria.CreatedTo.Value);

            var totalCount = await query.CountAsync();
            var beneficiaries = await query
                .OrderByDescending(b => b.CreatedAt)
                .Skip((criteria.PageNumber - 1) * criteria.PageSize)
                .Take(criteria.PageSize)
                .ToListAsync();

            return new BeneficiarySearchResult
            {
                Beneficiaries = beneficiaries.Select(MapToDto).ToList(),
                TotalCount = totalCount,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / criteria.PageSize)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching beneficiaries for customer {CustomerId}", criteria.CustomerId);
            return new BeneficiarySearchResult();
        }
    }

    public async Task<BeneficiaryVerificationResult> VerifyBeneficiaryAsync(Guid beneficiaryId, Guid verifiedByUserId)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            if (beneficiary == null)
            {
                return new BeneficiaryVerificationResult
                {
                    Success = false,
                    Message = "Beneficiary not found"
                };
            }

            // Perform account validation (this would integrate with external services)
            var isValid = await ValidateExternalAccountAsync(beneficiary);
            
            if (isValid)
            {
                beneficiary.Verify(verifiedByUserId);
                _unitOfWork.Repository<Beneficiary>().Update(beneficiary);
                await _unitOfWork.SaveChangesAsync();

                await _auditLogService.LogAsync("Beneficiary Verified", 
                    $"Beneficiary {beneficiary.Name} verified", verifiedByUserId);

                return new BeneficiaryVerificationResult
                {
                    Success = true,
                    Message = "Beneficiary verified successfully",
                    IsAccountValid = true,
                    AccountHolderName = beneficiary.AccountName,
                    BankName = beneficiary.BankName
                };
            }

            return new BeneficiaryVerificationResult
            {
                Success = false,
                Message = "Account validation failed",
                IsAccountValid = false,
                ValidationErrors = new List<string> { "Unable to verify account details" }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying beneficiary {BeneficiaryId}", beneficiaryId);
            return new BeneficiaryVerificationResult
            {
                Success = false,
                Message = "An error occurred during verification"
            };
        }
    }

    public async Task<bool> ArchiveBeneficiaryAsync(Guid beneficiaryId, string reason, Guid archivedByUserId)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            if (beneficiary == null) return false;

            beneficiary.Archive(reason);
            _unitOfWork.Repository<Beneficiary>().Update(beneficiary);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("Beneficiary Archived", 
                $"Beneficiary {beneficiary.Name} archived: {reason}", archivedByUserId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error archiving beneficiary {BeneficiaryId}", beneficiaryId);
            return false;
        }
    }

    public async Task<bool> ReactivateBeneficiaryAsync(Guid beneficiaryId, Guid reactivatedByUserId)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            if (beneficiary == null) return false;

            beneficiary.Reactivate();
            _unitOfWork.Repository<Beneficiary>().Update(beneficiary);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("Beneficiary Reactivated", 
                $"Beneficiary {beneficiary.Name} reactivated", reactivatedByUserId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reactivating beneficiary {BeneficiaryId}", beneficiaryId);
            return false;
        }
    }

    public async Task<bool> CanReceiveTransfersAsync(Guid beneficiaryId)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            return beneficiary?.CanReceiveTransfers() ?? false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking transfer eligibility for beneficiary {BeneficiaryId}", beneficiaryId);
            return false;
        }
    }

    public async Task<bool> ValidateTransferLimitsAsync(Guid beneficiaryId, decimal amount)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            return beneficiary?.IsTransferWithinLimits(amount) ?? false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating transfer limits for beneficiary {BeneficiaryId}", beneficiaryId);
            return false;
        }
    }

    public async Task RecordTransferAsync(Guid beneficiaryId, decimal amount)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            if (beneficiary != null)
            {
                beneficiary.RecordTransfer(amount);
                _unitOfWork.Repository<Beneficiary>().Update(beneficiary);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording transfer for beneficiary {BeneficiaryId}", beneficiaryId);
        }
    }

    public async Task<BeneficiaryTransferHistory> GetTransferHistoryAsync(Guid beneficiaryId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            if (beneficiary == null)
            {
                return new BeneficiaryTransferHistory { BeneficiaryId = beneficiaryId };
            }

            var query = _unitOfWork.Repository<Transaction>().Query()
                .Where(t => t.ToAccountId == Guid.Parse(beneficiary.AccountNumber)); // This would need proper relationship

            if (fromDate.HasValue)
                query = query.Where(t => t.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(t => t.CreatedAt <= toDate.Value);

            var transactions = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();

            return new BeneficiaryTransferHistory
            {
                BeneficiaryId = beneficiaryId,
                BeneficiaryName = beneficiary.Name,
                Transfers = transactions.Select(t => new TransferHistoryItem
                {
                    TransactionId = t.Id,
                    Amount = t.Amount,
                    TransferDate = t.CreatedAt,
                    Description = t.Description,
                    Reference = t.Reference,
                    Status = t.Status
                }).ToList(),
                TotalAmount = transactions.Sum(t => t.Amount),
                TotalCount = transactions.Count
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving transfer history for beneficiary {BeneficiaryId}", beneficiaryId);
            return new BeneficiaryTransferHistory { BeneficiaryId = beneficiaryId };
        }
    }

    public async Task<BeneficiaryStatistics> GetBeneficiaryStatisticsAsync(Guid customerId)
    {
        try
        {
            var beneficiaries = await _unitOfWork.Repository<Beneficiary>()
                .FindAsync(b => b.CustomerId == customerId);

            return new BeneficiaryStatistics
            {
                TotalBeneficiaries = beneficiaries.Count(),
                ActiveBeneficiaries = beneficiaries.Count(b => b.IsActive),
                VerifiedBeneficiaries = beneficiaries.Count(b => b.IsVerified),
                PendingVerification = beneficiaries.Count(b => b.Status == BeneficiaryStatus.Pending),
                BeneficiariesByCategory = beneficiaries.GroupBy(b => b.Category)
                    .ToDictionary(g => g.Key, g => g.Count()),
                BeneficiariesByType = beneficiaries.GroupBy(b => b.Type)
                    .ToDictionary(g => g.Key, g => g.Count()),
                TotalTransferAmount = beneficiaries.Sum(b => b.TotalTransferAmount),
                TotalTransfers = beneficiaries.Sum(b => b.TransferCount)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving beneficiary statistics for customer {CustomerId}", customerId);
            return new BeneficiaryStatistics();
        }
    }

    public async Task<BeneficiaryVerificationResult> ValidateAccountDetailsAsync(AddBeneficiaryRequest request)
    {
        try
        {
            var errors = new List<string>();

            // Basic validation
            if (string.IsNullOrEmpty(request.Name))
                errors.Add("Beneficiary name is required");

            if (string.IsNullOrEmpty(request.AccountNumber))
                errors.Add("Account number is required");

            if (string.IsNullOrEmpty(request.BankCode))
                errors.Add("Bank code is required");

            // Type-specific validation
            if (request.Type == BeneficiaryType.International)
            {
                if (string.IsNullOrEmpty(request.SwiftCode) && string.IsNullOrEmpty(request.IbanNumber))
                    errors.Add("SWIFT code or IBAN is required for international transfers");
            }

            // Account number format validation (basic)
            if (!string.IsNullOrEmpty(request.AccountNumber) && request.AccountNumber.Length < 8)
                errors.Add("Account number must be at least 8 digits");

            return new BeneficiaryVerificationResult
            {
                Success = errors.Count == 0,
                Message = errors.Count == 0 ? "Validation successful" : "Validation failed",
                ValidationErrors = errors,
                IsAccountValid = errors.Count == 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating account details");
            return new BeneficiaryVerificationResult
            {
                Success = false,
                Message = "Validation error occurred"
            };
        }
    }

    public async Task<bool> UpdateTransferLimitsAsync(Guid beneficiaryId, decimal? dailyLimit, decimal? monthlyLimit, decimal? singleLimit, Guid updatedByUserId)
    {
        try
        {
            var beneficiary = await _unitOfWork.Repository<Beneficiary>().GetByIdAsync(beneficiaryId);
            if (beneficiary == null) return false;

            beneficiary.UpdateTransferLimits(dailyLimit, monthlyLimit, singleLimit);
            _unitOfWork.Repository<Beneficiary>().Update(beneficiary);
            await _unitOfWork.SaveChangesAsync();

            await _auditLogService.LogAsync("Beneficiary Limits Updated", 
                $"Transfer limits updated for beneficiary {beneficiary.Name}", updatedByUserId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating transfer limits for beneficiary {BeneficiaryId}", beneficiaryId);
            return false;
        }
    }

    #region Private Methods

    private static BeneficiaryDto MapToDto(Beneficiary beneficiary)
    {
        return new BeneficiaryDto
        {
            Id = beneficiary.Id,
            CustomerId = beneficiary.CustomerId,
            Name = beneficiary.Name,
            AccountNumber = beneficiary.AccountNumber,
            AccountName = beneficiary.AccountName,
            BankName = beneficiary.BankName,
            BankCode = beneficiary.BankCode,
            SwiftCode = beneficiary.SwiftCode,
            IbanNumber = beneficiary.IbanNumber,
            RoutingNumber = beneficiary.RoutingNumber,
            Type = beneficiary.Type,
            Category = beneficiary.Category,
            IsVerified = beneficiary.IsVerified,
            VerifiedDate = beneficiary.VerifiedDate,
            Status = beneficiary.Status,
            DailyTransferLimit = beneficiary.DailyTransferLimit,
            MonthlyTransferLimit = beneficiary.MonthlyTransferLimit,
            SingleTransferLimit = beneficiary.SingleTransferLimit,
            IsActive = beneficiary.IsActive,
            Notes = beneficiary.Notes,
            Reference = beneficiary.Reference,
            LastTransferDate = beneficiary.LastTransferDate,
            TransferCount = beneficiary.TransferCount,
            TotalTransferAmount = beneficiary.TotalTransferAmount,
            CreatedAt = beneficiary.CreatedAt,
            UpdatedAt = beneficiary.UpdatedAt
        };
    }

    private async Task<bool> ValidateExternalAccountAsync(Beneficiary beneficiary)
    {
        // This would integrate with external bank validation services
        // For now, return true for demonstration
        await Task.Delay(100); // Simulate API call
        return true;
    }

    #endregion
}