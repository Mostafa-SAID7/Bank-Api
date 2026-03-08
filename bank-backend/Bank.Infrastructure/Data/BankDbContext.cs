using Bank.Domain.Entities;
using Bank.Domain.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure;

/// <summary>
/// Application database context using ASP.NET Core Identity with Guid keys.
/// Includes automatic soft delete filtering and audit field population.
/// </summary>
public class BankDbContext : IdentityDbContext<User, Role, Guid>
{
    public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<BatchJob> BatchJobs => Set<BatchJob>();
    public DbSet<TwoFactorToken> TwoFactorTokens => Set<TwoFactorToken>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global soft delete query filter for entities inheriting BaseEntity
        modelBuilder.Entity<Account>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Transaction>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<BatchJob>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TwoFactorToken>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);

        // AuditLog configuration - immutable, no soft delete
        modelBuilder.Entity<AuditLog>()
            .HasOne(al => al.User)
            .WithMany()
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AuditLog>()
            .HasIndex(al => new { al.UserId, al.CreatedAt })
            .HasDatabaseName("IX_AuditLogs_UserId_CreatedAt");

        modelBuilder.Entity<AuditLog>()
            .HasIndex(al => new { al.EntityType, al.EntityId })
            .HasDatabaseName("IX_AuditLogs_EntityType_EntityId");

        modelBuilder.Entity<AuditLog>()
            .HasIndex(al => al.EventType)
            .HasDatabaseName("IX_AuditLogs_EventType");

        modelBuilder.Entity<AuditLog>()
            .HasIndex(al => al.Action)
            .HasDatabaseName("IX_AuditLogs_Action");

        modelBuilder.Entity<AuditLog>()
            .HasIndex(al => al.IpAddress)
            .HasDatabaseName("IX_AuditLogs_IpAddress");

        modelBuilder.Entity<AuditLog>()
            .HasIndex(al => al.CreatedAt)
            .HasDatabaseName("IX_AuditLogs_CreatedAt");

        modelBuilder.Entity<AuditLog>()
            .Property(al => al.Action)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<AuditLog>()
            .Property(al => al.EntityType)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<AuditLog>()
            .Property(al => al.EntityId)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<AuditLog>()
            .Property(al => al.IpAddress)
            .HasMaxLength(45);

        modelBuilder.Entity<AuditLog>()
            .Property(al => al.UserAgent)
            .HasMaxLength(500);

        // Account configuration
        modelBuilder.Entity<Account>()
            .HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<Account>()
            .Property(a => a.Balance)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.AccountNumber)
            .IsUnique();

        // Transaction configuration
        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.FromAccount)
            .WithMany(a => a.SentTransactions)
            .HasForeignKey(t => t.FromAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToAccount)
            .WithMany(a => a.ReceivedTransactions)
            .HasForeignKey(t => t.ToAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(18, 2);

        // TwoFactorToken configuration
        modelBuilder.Entity<TwoFactorToken>()
            .HasOne(t => t.User)
            .WithMany(u => u.TwoFactorTokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TwoFactorToken>()
            .HasIndex(t => new { t.UserId, t.Token, t.ExpiresAt })
            .HasDatabaseName("IX_TwoFactorTokens_UserId_Token_ExpiresAt");

        modelBuilder.Entity<TwoFactorToken>()
            .HasIndex(t => t.ExpiresAt)
            .HasDatabaseName("IX_TwoFactorTokens_ExpiresAt");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<Domain.Common.BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        // Also set audit on Identity User
        foreach (var entry in ChangeTracker.Entries<User>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
