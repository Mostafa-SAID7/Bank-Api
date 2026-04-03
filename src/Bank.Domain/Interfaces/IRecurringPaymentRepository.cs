using Bank.Domain.Entities;

namespace Bank.Domain.Interfaces;

public interface IRecurringPaymentRepository
{
    Task<RecurringPayment?> GetByIdAsync(Guid id);
    Task<IEnumerable<RecurringPayment>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<RecurringPayment>> GetByAccountIdAsync(Guid accountId);
    Task<IEnumerable<RecurringPayment>> GetDuePaymentsAsync();
    Task<RecurringPayment> AddAsync(RecurringPayment recurringPayment);
    Task UpdateAsync(RecurringPayment recurringPayment);
    Task DeleteAsync(Guid id);
    
    // Execution management
    Task<RecurringPaymentExecution> AddExecutionAsync(RecurringPaymentExecution execution);
    Task<IEnumerable<RecurringPaymentExecution>> GetExecutionHistoryAsync(Guid recurringPaymentId);
}