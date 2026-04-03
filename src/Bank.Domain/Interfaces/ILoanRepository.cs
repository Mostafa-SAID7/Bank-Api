using Bank.Domain.Entities;
using Bank.Domain.Enums;

namespace Bank.Domain.Interfaces;

/// <summary>
/// Repository interface for loan data access
/// </summary>
public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(Guid id);
    Task<Loan?> GetByLoanNumberAsync(string loanNumber);
    Task<List<Loan>> GetByCustomerIdAsync(Guid customerId);
    Task<List<Loan>> GetByStatusAsync(LoanStatus status);
    Task<List<Loan>> GetOverdueLoansAsync();
    Task<List<Loan>> SearchAsync(
        Guid? customerId = null,
        LoanType? type = null,
        LoanStatus? status = null,
        decimal? minAmount = null,
        decimal? maxAmount = null,
        DateTime? applicationDateFrom = null,
        DateTime? applicationDateTo = null,
        bool? isOverdue = null,
        int skip = 0,
        int take = 10,
        string sortBy = "ApplicationDate",
        bool sortDescending = true);
    Task<int> GetSearchCountAsync(
        Guid? customerId = null,
        LoanType? type = null,
        LoanStatus? status = null,
        decimal? minAmount = null,
        decimal? maxAmount = null,
        DateTime? applicationDateFrom = null,
        DateTime? applicationDateTo = null,
        bool? isOverdue = null);
    Task<Loan> AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<string> GenerateNextLoanNumberAsync();
}