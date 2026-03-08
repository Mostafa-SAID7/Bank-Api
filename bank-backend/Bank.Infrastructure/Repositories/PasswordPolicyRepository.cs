using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for PasswordPolicy entity
/// </summary>
public class PasswordPolicyRepository : Repository<PasswordPolicy>, IPasswordPolicyRepository
{
    public PasswordPolicyRepository(BankDbContext context) : base(context)
    {
    }

    public async Task<PasswordPolicy?> GetDefaultPolicyAsync()
    {
        return await _context.PasswordPolicies
            .FirstOrDefaultAsync(p => p.IsDefault && p.IsActive);
    }

    public async Task<PasswordPolicy?> GetByComplexityLevelAsync(PasswordComplexityLevel complexityLevel)
    {
        return await _context.PasswordPolicies
            .FirstOrDefaultAsync(p => p.ComplexityLevel == complexityLevel && p.IsActive);
    }

    public async Task<List<PasswordPolicy>> GetActivePoliciesAsync()
    {
        return await _context.PasswordPolicies
            .Where(p => p.IsActive)
            .OrderBy(p => p.ComplexityLevel)
            .ToListAsync();
    }

    public async Task ClearDefaultPolicyAsync()
    {
        var defaultPolicies = await _context.PasswordPolicies
            .Where(p => p.IsDefault)
            .ToListAsync();

        foreach (var policy in defaultPolicies)
        {
            policy.Deactivate(); // This also clears the default flag
        }

        await _context.SaveChangesAsync();
    }

    public async Task<PasswordPolicy?> GetByNameAsync(string name)
    {
        return await _context.PasswordPolicies
            .FirstOrDefaultAsync(p => p.Name == name);
    }
}
