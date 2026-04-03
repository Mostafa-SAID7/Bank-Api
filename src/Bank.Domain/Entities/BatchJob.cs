using Bank.Domain.Common;
using Bank.Domain.Enums;

namespace Bank.Domain.Entities;

public class BatchJob : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public int TotalRecords { get; set; }
    public int SuccessCount { get; set; }
    public int FailureCount { get; set; }
    public DateTime? CompletedAt { get; set; }
    public BatchJobStatus Status { get; set; } = BatchJobStatus.Pending;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
