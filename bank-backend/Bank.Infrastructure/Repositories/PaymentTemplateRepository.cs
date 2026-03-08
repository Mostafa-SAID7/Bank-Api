using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Infrastructure.Data;
using Bank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Bank.Infrastructure.Repositories;

public class PaymentTemplateRepository : IPaymentTemplateRepository
{
    private readonly BankDbContext _context;

    public PaymentTemplateRepository(BankDbContext context)
    {
        _context = context;
    }

    public async Task<PaymentTemplate?> GetByIdAsync(Guid id)
    {
        return await _context.PaymentTemplates
            .Include(pt => pt.FromAccount)
            .Include(pt => pt.ToAccount)
            .Include(pt => pt.CreatedByUser)
            .FirstOrDefaultAsync(pt => pt.Id == id);
    }

    public async Task<IEnumerable<PaymentTemplate>> GetByUserIdAsync(Guid userId)
    {
        return await _context.PaymentTemplates
            .Include(pt => pt.FromAccount)
            .Include(pt => pt.ToAccount)
            .Where(pt => pt.CreatedByUserId == userId && pt.IsActive)
            .OrderBy(pt => pt.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<PaymentTemplate>> GetByAccountIdAsync(Guid accountId)
    {
        return await _context.PaymentTemplates
            .Include(pt => pt.FromAccount)
            .Include(pt => pt.ToAccount)
            .Where(pt => pt.FromAccountId == accountId && pt.IsActive)
            .OrderBy(pt => pt.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<PaymentTemplate>> GetByCategoryAsync(Guid userId, PaymentTemplateCategory category)
    {
        return await _context.PaymentTemplates
            .Include(pt => pt.FromAccount)
            .Include(pt => pt.ToAccount)
            .Where(pt => pt.CreatedByUserId == userId && pt.Category == category && pt.IsActive)
            .OrderBy(pt => pt.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<PaymentTemplate>> GetMostUsedAsync(Guid userId, int count)
    {
        return await _context.PaymentTemplates
            .Include(pt => pt.FromAccount)
            .Include(pt => pt.ToAccount)
            .Where(pt => pt.CreatedByUserId == userId && pt.IsActive)
            .OrderByDescending(pt => pt.UsageCount)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<PaymentTemplate>> GetRecentlyUsedAsync(Guid userId, int count)
    {
        return await _context.PaymentTemplates
            .Include(pt => pt.FromAccount)
            .Include(pt => pt.ToAccount)
            .Where(pt => pt.CreatedByUserId == userId && pt.IsActive && pt.LastUsedDate != null)
            .OrderByDescending(pt => pt.LastUsedDate)
            .Take(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<PaymentTemplate>> SearchAsync(Guid userId, string searchTerm)
    {
        var lowerSearchTerm = searchTerm.ToLower();
        return await _context.PaymentTemplates
            .Include(pt => pt.FromAccount)
            .Include(pt => pt.ToAccount)
            .Where(pt => pt.CreatedByUserId == userId && pt.IsActive &&
                        (pt.Name.ToLower().Contains(lowerSearchTerm) ||
                         pt.Description.ToLower().Contains(lowerSearchTerm) ||
                         (pt.BeneficiaryName != null && pt.BeneficiaryName.ToLower().Contains(lowerSearchTerm)) ||
                         (pt.Reference != null && pt.Reference.ToLower().Contains(lowerSearchTerm))))
            .OrderBy(pt => pt.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<PaymentTemplate>> GetByTagsAsync(Guid userId, string[] tags)
    {
        var templates = await _context.PaymentTemplates
            .Include(pt => pt.FromAccount)
            .Include(pt => pt.ToAccount)
            .Where(pt => pt.CreatedByUserId == userId && pt.IsActive && pt.Tags != null)
            .ToListAsync();

        // Filter by tags in memory since we need to deserialize JSON
        return templates.Where(pt =>
        {
            if (string.IsNullOrEmpty(pt.Tags))
                return false;

            try
            {
                var templateTags = JsonSerializer.Deserialize<string[]>(pt.Tags);
                return templateTags != null && tags.Any(tag => templateTags.Contains(tag, StringComparer.OrdinalIgnoreCase));
            }
            catch
            {
                return false;
            }
        }).OrderBy(pt => pt.Name);
    }

    public async Task<PaymentTemplate> AddAsync(PaymentTemplate template)
    {
        _context.PaymentTemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task UpdateAsync(PaymentTemplate template)
    {
        _context.PaymentTemplates.Update(template);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var template = await _context.PaymentTemplates.FindAsync(id);
        if (template != null)
        {
            _context.PaymentTemplates.Remove(template);
            await _context.SaveChangesAsync();
        }
    }
}
