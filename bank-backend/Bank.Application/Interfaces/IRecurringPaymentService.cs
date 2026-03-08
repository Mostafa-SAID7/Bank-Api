using Bank.Application.DTOs;
using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

public interface IRecurringPaymentService
{
    // Recurring payment management
    Task<RecurringPayment> CreateRecurringPaymentAsync(CreateRecurringPaymentRequest request);
    Task<RecurringPayment?> GetRecurringPaymentAsync(Guid id);
    Task<IEnumerable<RecurringPayment>> GetUserRecurringPaymentsAsync(Guid userId);
    Task<IEnumerable<RecurringPayment>> GetAccountRecurringPaymentsAsync(Guid accountId);
    Task<bool> UpdateRecurringPaymentAsync(Guid id, UpdateRecurringPaymentRequest request);
    Task<bool> PauseRecurringPaymentAsync(Guid id, string reason);
    Task<bool> ResumeRecurringPaymentAsync(Guid id);
    Task<bool> CancelRecurringPaymentAsync(Guid id, string reason);
    
    // Execution management
    Task<IEnumerable<RecurringPayment>> GetDueRecurringPaymentsAsync();
    Task<bool> ExecuteRecurringPaymentAsync(Guid recurringPaymentId);
    Task<int> ProcessDueRecurringPaymentsAsync();
    Task<IEnumerable<RecurringPaymentExecution>> GetExecutionHistoryAsync(Guid recurringPaymentId);
    
    // Bulk operations
    Task<BulkTransferResult> ProcessBulkTransfersAsync(BulkTransferRequest request);
    Task<BulkTransferResult> GetBulkTransferStatusAsync(Guid batchId);
}