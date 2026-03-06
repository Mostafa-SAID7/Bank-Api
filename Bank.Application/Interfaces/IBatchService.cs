using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Application.Interfaces;

public interface IBatchService
{
    Task<BatchJob> CreateBatchJobAsync(string fileName, int totalRecords);
    Task ProcessBatchAsync(Guid jobId, IEnumerable<TransactionRequest> transactions);
    Task<BatchJob?> GetBatchJobStatusAsync(Guid jobId);
}

public record TransactionRequest(Guid FromAccountId, Guid ToAccountId, decimal Amount, TransactionType Type, string Description);
