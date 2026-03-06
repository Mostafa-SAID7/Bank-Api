using Bank.Application.Interfaces;
using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bank.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Transaction> InitiateTransactionAsync(Guid fromAccountId, Guid toAccountId, decimal amount, TransactionType type, string description)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var fromAccount = await _unitOfWork.Repository<Account>().GetByIdAsync(fromAccountId) 
                ?? throw new Exception("Sender account not found.");
            var toAccount = await _unitOfWork.Repository<Account>().GetByIdAsync(toAccountId) 
                ?? throw new Exception("Recipient account not found.");

            if (fromAccount.Balance < amount)
                throw new Exception("Insufficient funds.");

            var transaction = new Transaction
            {
                FromAccountId = fromAccountId,
                ToAccountId = toAccountId,
                Amount = amount,
                Type = type,
                Description = description,
                Status = TransactionStatus.Processing
            };

            // RTGS = instant settlement, ACH/WPS = deferred
            if (type == TransactionType.RTGS)
            {
                fromAccount.Balance -= amount;
                toAccount.Balance += amount;
                transaction.Status = TransactionStatus.Completed;
                transaction.ProcessedAt = DateTime.UtcNow;
                _unitOfWork.Repository<Account>().Update(fromAccount);
                _unitOfWork.Repository<Account>().Update(toAccount);
            }
            else
            {
                transaction.Status = TransactionStatus.Pending;
                fromAccount.Balance -= amount;
                _unitOfWork.Repository<Account>().Update(fromAccount);
            }

            await _unitOfWork.Repository<Transaction>().AddAsync(transaction);
            await _unitOfWork.CommitTransactionAsync();

            return transaction;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync(Guid accountId)
    {
        return await _unitOfWork.Repository<Transaction>().Query()
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid id)
    {
        return await _unitOfWork.Repository<Transaction>().Query()
            .Include(t => t.FromAccount)
            .Include(t => t.ToAccount)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
