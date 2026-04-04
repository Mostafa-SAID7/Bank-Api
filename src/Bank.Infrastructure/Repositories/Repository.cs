using System.Linq.Expressions;
using Bank.Domain.Common;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Generic repository implementation with soft delete query filtering.
/// </summary>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly BankDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(BankDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.Where(e => !e.IsDeleted).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).Where(e => !e.IsDeleted).ToListAsync();
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(e => !e.IsDeleted).FirstOrDefaultAsync(predicate);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void SoftDelete(T entity, string? deletedBy = null)
    {
        entity.SoftDelete(deletedBy);
        _dbSet.Update(entity);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var query = _dbSet.Where(e => !e.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);
        return await query.CountAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(e => !e.IsDeleted).AnyAsync(predicate);
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> ListAsync(Expression<Func<T, bool>>? predicate = null, int page = 1, int pageSize = 10)
    {
        var query = _dbSet.Where(e => !e.IsDeleted);
        if (predicate != null)
            query = query.Where(predicate);

        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> SearchAsync(string searchTerm, int page = 1, int pageSize = 10)
    {
        // Default search implementation - can be overridden in specific repositories
        // This is a placeholder that returns everything if no override is provided
        return await ListAsync(null, page, pageSize);
    }

    public IQueryable<T> Query()
    {
        return _dbSet.Where(e => !e.IsDeleted).AsQueryable();
    }
}

