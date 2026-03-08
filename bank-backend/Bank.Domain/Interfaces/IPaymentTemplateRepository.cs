using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

public interface IPaymentTemplateRepository
{
    Task<PaymentTemplate?> GetByIdAsync(Guid id);
    Task<IEnumerable<PaymentTemplate>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<PaymentTemplate>> GetByAccountIdAsync(Guid accountId);
    Task<IEnumerable<PaymentTemplate>> GetByCategoryAsync(Guid userId, PaymentTemplateCategory category);
    Task<IEnumerable<PaymentTemplate>> GetMostUsedAsync(Guid userId, int count);
    Task<IEnumerable<PaymentTemplate>> GetRecentlyUsedAsync(Guid userId, int count);
    Task<IEnumerable<PaymentTemplate>> SearchAsync(Guid userId, string searchTerm);
    Task<IEnumerable<PaymentTemplate>> GetByTagsAsync(Guid userId, string[] tags);
    Task<PaymentTemplate> AddAsync(PaymentTemplate template);
    Task UpdateAsync(PaymentTemplate template);
    Task DeleteAsync(Guid id);
}