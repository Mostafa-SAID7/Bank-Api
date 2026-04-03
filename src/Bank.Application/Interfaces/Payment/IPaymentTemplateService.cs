using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

public interface IPaymentTemplateService
{
    // Template management
    Task<PaymentTemplate> CreateTemplateAsync(CreatePaymentTemplateRequest request);
    Task<PaymentTemplate?> GetTemplateAsync(Guid id);
    Task<IEnumerable<PaymentTemplate>> GetUserTemplatesAsync(Guid userId);
    Task<IEnumerable<PaymentTemplate>> GetAccountTemplatesAsync(Guid accountId);
    Task<IEnumerable<PaymentTemplate>> GetTemplatesByCategoryAsync(Guid userId, PaymentTemplateCategory category);
    Task<bool> UpdateTemplateAsync(Guid id, UpdatePaymentTemplateRequest request);
    Task<bool> DeleteTemplateAsync(Guid id);
    Task<bool> ActivateTemplateAsync(Guid id);
    Task<bool> DeactivateTemplateAsync(Guid id);
    
    // Template usage
    Task<Transaction> ExecuteTemplateAsync(Guid templateId, ExecuteTemplateRequest request);
    Task<IEnumerable<PaymentTemplate>> GetMostUsedTemplatesAsync(Guid userId, int count = 10);
    Task<IEnumerable<PaymentTemplate>> GetRecentlyUsedTemplatesAsync(Guid userId, int count = 10);
    
    // Template search and filtering
    Task<IEnumerable<PaymentTemplate>> SearchTemplatesAsync(Guid userId, string searchTerm);
    Task<IEnumerable<PaymentTemplate>> GetTemplatesByTagsAsync(Guid userId, string[] tags);
}