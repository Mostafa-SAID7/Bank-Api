using Bank.CoreBanking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bank.CoreBanking.Infrastructure.Data;

/// <summary>
/// EF Core DbContext for Core Banking module
/// Schema: core_banking in PostgreSQL
/// </summary>
public sealed class CoreBankingDbContext : DbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Ledger> LedgerEntries => Set<Ledger>();

    public CoreBankingDbContext(DbContextOptions<CoreBankingDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Set schema
        modelBuilder.HasDefaultSchema("core_banking");

        // Account configuration
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.AccountNumber).HasMaxLength(50).IsRequired();
            entity.Property(a => a.AccountHolderName).HasMaxLength(255).IsRequired();
            entity.Property(a => a.Status).HasConversion<int>();
            entity.Property(a => a.Type).HasConversion<int>();
            entity.HasIndex(a => a.AccountNumber).IsUnique();
            entity.HasIndex(a => a.CustomerId);
            entity.HasIndex(a => a.Status);
        });

        // Transaction configuration
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("transactions");
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Status).HasConversion<int>();
            entity.Property(t => t.Type).HasConversion<int>();
            entity.Property(t => t.IdempotencyKey).HasMaxLength(100);
            entity.HasIndex(t => t.IdempotencyKey).IsUnique();
            entity.HasIndex(t => t.FromAccountId);
            entity.HasIndex(t => t.ToAccountId);
            entity.HasIndex(t => t.Status);
        });

        // Ledger configuration
        modelBuilder.Entity<Ledger>(entity =>
        {
            entity.ToTable("ledger_entries");
            entity.HasKey(l => l.Id);
            entity.Property(l => l.Reference).HasMaxLength(100);
            entity.Property(l => l.Description).HasMaxLength(500);
            entity.HasIndex(l => l.TransactionId);
            entity.HasIndex(l => l.AccountId);
            entity.HasIndex(l => l.PostedAtUtc);
        });
    }
}
