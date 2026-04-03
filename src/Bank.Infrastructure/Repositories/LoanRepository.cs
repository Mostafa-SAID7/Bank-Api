using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Repository implementation for loan data access
/// </summary>
public class LoanRepository : ILoanRepository
{
    private readonly BankDbContext _context;

    public LoanRepository(BankDbContext context)
    {
        _context = context;
    }

    public async Task<Loan?> GetByIdAsync(Guid id)
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.Payments)
            .Include(l => l.Documents)
            .Include(l => l.StatusHistory)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<Loan?> GetByLoanNumberAsync(string loanNumber)
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Include(l => l.Payments)
            .Include(l => l.Documents)
            .Include(l => l.StatusHistory)
            .FirstOrDefaultAsync(l => l.LoanNumber == loanNumber);
    }

    public async Task<List<Loan>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _context.Loans
            .Include(l => l.Payments)
            .Where(l => l.CustomerId == customerId)
            .OrderByDescending(l => l.ApplicationDate)
            .ToListAsync();
    }

    public async Task<List<Loan>> GetByStatusAsync(LoanStatus status)
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Where(l => l.Status == status)
            .OrderByDescending(l => l.ApplicationDate)
            .ToListAsync();
    }

    public async Task<List<Loan>> GetOverdueLoansAsync()
    {
        return await _context.Loans
            .Include(l => l.Customer)
            .Where(l => (l.Status == LoanStatus.Active || l.Status == LoanStatus.Disbursed) &&
                       l.NextPaymentDueDate.HasValue &&
                       l.NextPaymentDueDate.Value < DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<List<Loan>> SearchAsync(
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
        bool sortDescending = true)
    {
        var query = BuildSearchQuery(customerId, type, status, minAmount, maxAmount,
            applicationDateFrom, applicationDateTo, isOverdue);

        // Apply sorting
        query = sortBy.ToLower() switch
        {
            "amount" => sortDescending ? query.OrderByDescending(l => l.PrincipalAmount) : query.OrderBy(l => l.PrincipalAmount),
            "status" => sortDescending ? query.OrderByDescending(l => l.Status) : query.OrderBy(l => l.Status),
            "type" => sortDescending ? query.OrderByDescending(l => l.Type) : query.OrderBy(l => l.Type),
            "loannumber" => sortDescending ? query.OrderByDescending(l => l.LoanNumber) : query.OrderBy(l => l.LoanNumber),
            _ => sortDescending ? query.OrderByDescending(l => l.ApplicationDate) : query.OrderBy(l => l.ApplicationDate)
        };

        return await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> GetSearchCountAsync(
        Guid? customerId = null,
        LoanType? type = null,
        LoanStatus? status = null,
        decimal? minAmount = null,
        decimal? maxAmount = null,
        DateTime? applicationDateFrom = null,
        DateTime? applicationDateTo = null,
        bool? isOverdue = null)
    {
        var query = BuildSearchQuery(customerId, type, status, minAmount, maxAmount,
            applicationDateFrom, applicationDateTo, isOverdue);

        return await query.CountAsync();
    }

    public async Task<Loan> AddAsync(Loan loan)
    {
        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();
        return loan;
    }

    public async Task UpdateAsync(Loan loan)
    {
        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var loan = await _context.Loans.FindAsync(id);
        if (loan != null)
        {
            loan.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Loans.AnyAsync(l => l.Id == id);
    }

    public async Task<string> GenerateNextLoanNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"LN{year}";
        
        var lastLoan = await _context.Loans
            .Where(l => l.LoanNumber.StartsWith(prefix))
            .OrderByDescending(l => l.LoanNumber)
            .FirstOrDefaultAsync();

        int nextNumber = 1;
        if (lastLoan != null && lastLoan.LoanNumber.Length > prefix.Length)
        {
            var numberPart = lastLoan.LoanNumber.Substring(prefix.Length);
            if (int.TryParse(numberPart, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        return $"{prefix}{nextNumber:D6}";
    }

    private IQueryable<Loan> BuildSearchQuery(
        Guid? customerId,
        LoanType? type,
        LoanStatus? status,
        decimal? minAmount,
        decimal? maxAmount,
        DateTime? applicationDateFrom,
        DateTime? applicationDateTo,
        bool? isOverdue)
    {
        var query = _context.Loans
            .Include(l => l.Customer)
            .AsQueryable();

        if (customerId.HasValue)
            query = query.Where(l => l.CustomerId == customerId.Value);

        if (type.HasValue)
            query = query.Where(l => l.Type == type.Value);

        if (status.HasValue)
            query = query.Where(l => l.Status == status.Value);

        if (minAmount.HasValue)
            query = query.Where(l => l.PrincipalAmount >= minAmount.Value);

        if (maxAmount.HasValue)
            query = query.Where(l => l.PrincipalAmount <= maxAmount.Value);

        if (applicationDateFrom.HasValue)
            query = query.Where(l => l.ApplicationDate >= applicationDateFrom.Value);

        if (applicationDateTo.HasValue)
            query = query.Where(l => l.ApplicationDate <= applicationDateTo.Value);

        if (isOverdue.HasValue && isOverdue.Value)
            query = query.Where(l => l.DaysOverdue > 0);

        return query;
    }
}