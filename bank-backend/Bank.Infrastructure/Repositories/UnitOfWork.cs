using System.Collections;
using Bank.Domain.Common;
using Bank.Domain.Interfaces;
using Bank.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace Bank.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation coordinating multiple repositories with transaction support.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly BankDbContext _context;
    private readonly Hashtable _repositories = new();
    private IDbContextTransaction? _transaction;
    private bool _disposed;

    public UnitOfWork(BankDbContext context)
    {
        _context = context;
    }

    public IRepository<T> Repository<T>() where T : BaseEntity
    {
        var typeName = typeof(T).Name;

        if (!_repositories.ContainsKey(typeName))
        {
            var repositoryInstance = new Repository<T>(_context);
            _repositories[typeName] = repositoryInstance;
        }

        return (IRepository<T>)_repositories[typeName]!;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
                await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            _transaction.Dispose();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            _context.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}

