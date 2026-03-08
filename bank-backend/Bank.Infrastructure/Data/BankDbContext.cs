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
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<AccountLockout> AccountLockouts => Set<AccountLockout>();
    public DbSet<IpWhitelist> IpWhitelists => Set<IpWhitelist>();
    public DbSet<PasswordPolicy> PasswordPolicies => Set<PasswordPolicy>();
    public DbSet<PasswordHistory> PasswordHistories => Set<PasswordHistory>();
    public DbSet<AccountFee> AccountFees => Set<AccountFee>();
    public DbSet<AccountHold> AccountHolds => Set<AccountHold>();
    public DbSet<AccountRestriction> AccountRestrictions => Set<AccountRestriction>();
    public DbSet<AccountStatusHistory> AccountStatusHistories => Set<AccountStatusHistory>();
    public DbSet<FeeSchedule> FeeSchedules => Set<FeeSchedule>();
    public DbSet<JointAccountHolder> JointAccountHolders => Set<JointAccountHolder>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global soft delete query filter for entities inheriting BaseEntity
        modelBuilder.Entity<Account>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Transaction>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<BatchJob>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TwoFactorToken>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Session>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AccountLockout>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<IpWhitelist>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<PasswordPolicy>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<PasswordHistory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AccountFee>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AccountHold>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AccountRestriction>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AccountStatusHistory>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<FeeSchedule>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<JointAccountHolder>().HasQueryFilter(e => !e.IsDeleted);

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

        // Session configuration
        modelBuilder.Entity<Session>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Session>()
            .HasIndex(s => s.SessionToken)
            .IsUnique()
            .HasDatabaseName("IX_Sessions_SessionToken");

        modelBuilder.Entity<Session>()
            .HasIndex(s => s.RefreshToken)
            .HasDatabaseName("IX_Sessions_RefreshToken");

        modelBuilder.Entity<Session>()
            .HasIndex(s => new { s.UserId, s.Status })
            .HasDatabaseName("IX_Sessions_UserId_Status");

        modelBuilder.Entity<Session>()
            .HasIndex(s => s.ExpiresAt)
            .HasDatabaseName("IX_Sessions_ExpiresAt");

        modelBuilder.Entity<Session>()
            .HasIndex(s => s.IpAddress)
            .HasDatabaseName("IX_Sessions_IpAddress");

        modelBuilder.Entity<Session>()
            .Property(s => s.SessionToken)
            .HasMaxLength(128)
            .IsRequired();

        modelBuilder.Entity<Session>()
            .Property(s => s.RefreshToken)
            .HasMaxLength(128);

        modelBuilder.Entity<Session>()
            .Property(s => s.IpAddress)
            .HasMaxLength(45)
            .IsRequired();

        modelBuilder.Entity<Session>()
            .Property(s => s.UserAgent)
            .HasMaxLength(500)
            .IsRequired();

        // AccountLockout configuration
        modelBuilder.Entity<AccountLockout>()
            .HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AccountLockout>()
            .HasOne(l => l.LockedByUser)
            .WithMany()
            .HasForeignKey(l => l.LockedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AccountLockout>()
            .HasIndex(l => l.UserId)
            .IsUnique()
            .HasDatabaseName("IX_AccountLockouts_UserId");

        modelBuilder.Entity<AccountLockout>()
            .HasIndex(l => l.IsCurrentlyLocked)
            .HasDatabaseName("IX_AccountLockouts_IsCurrentlyLocked");

        modelBuilder.Entity<AccountLockout>()
            .HasIndex(l => l.LockedUntil)
            .HasDatabaseName("IX_AccountLockouts_LockedUntil");

        modelBuilder.Entity<AccountLockout>()
            .Property(l => l.IpAddress)
            .HasMaxLength(45);

        modelBuilder.Entity<AccountLockout>()
            .Property(l => l.UserAgent)
            .HasMaxLength(500);

        // IpWhitelist configuration
        modelBuilder.Entity<IpWhitelist>()
            .HasOne(w => w.CreatedByUser)
            .WithMany()
            .HasForeignKey(w => w.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IpWhitelist>()
            .HasOne(w => w.ApprovedByUser)
            .WithMany()
            .HasForeignKey(w => w.ApprovedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<IpWhitelist>()
            .HasIndex(w => new { w.IpAddress, w.Type })
            .HasDatabaseName("IX_IpWhitelists_IpAddress_Type");

        modelBuilder.Entity<IpWhitelist>()
            .HasIndex(w => new { w.Type, w.IsActive })
            .HasDatabaseName("IX_IpWhitelists_Type_IsActive");

        modelBuilder.Entity<IpWhitelist>()
            .HasIndex(w => w.ExpiresAt)
            .HasDatabaseName("IX_IpWhitelists_ExpiresAt");

        modelBuilder.Entity<IpWhitelist>()
            .Property(w => w.IpAddress)
            .HasMaxLength(45)
            .IsRequired();

        modelBuilder.Entity<IpWhitelist>()
            .Property(w => w.IpRange)
            .HasMaxLength(50);

        modelBuilder.Entity<IpWhitelist>()
            .Property(w => w.Description)
            .HasMaxLength(500)
            .IsRequired();

        // PasswordPolicy configuration
        modelBuilder.Entity<PasswordPolicy>()
            .HasIndex(p => p.IsDefault)
            .HasDatabaseName("IX_PasswordPolicies_IsDefault");

        modelBuilder.Entity<PasswordPolicy>()
            .HasIndex(p => p.ComplexityLevel)
            .IsUnique()
            .HasDatabaseName("IX_PasswordPolicies_ComplexityLevel");

        modelBuilder.Entity<PasswordPolicy>()
            .HasIndex(p => p.Name)
            .IsUnique()
            .HasDatabaseName("IX_PasswordPolicies_Name");

        modelBuilder.Entity<PasswordPolicy>()
            .Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<PasswordPolicy>()
            .Property(p => p.AllowedSpecialCharacters)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<PasswordPolicy>()
            .Property(p => p.Description)
            .HasMaxLength(500);

        // PasswordHistory configuration
        modelBuilder.Entity<PasswordHistory>()
            .HasOne(h => h.User)
            .WithMany()
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PasswordHistory>()
            .HasIndex(h => new { h.UserId, h.PasswordSetAt })
            .HasDatabaseName("IX_PasswordHistories_UserId_PasswordSetAt");

        modelBuilder.Entity<PasswordHistory>()
            .HasIndex(h => new { h.UserId, h.IsCurrentPassword })
            .HasDatabaseName("IX_PasswordHistories_UserId_IsCurrentPassword");

        modelBuilder.Entity<PasswordHistory>()
            .Property(h => h.PasswordHash)
            .HasMaxLength(256)
            .IsRequired();

        modelBuilder.Entity<PasswordHistory>()
            .Property(h => h.PasswordSalt)
            .HasMaxLength(128);

        // AccountFee configuration
        modelBuilder.Entity<AccountFee>()
            .HasOne(f => f.Account)
            .WithMany(a => a.Fees)
            .HasForeignKey(f => f.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AccountFee>()
            .HasOne(f => f.Transaction)
            .WithMany()
            .HasForeignKey(f => f.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AccountFee>()
            .Property(f => f.Amount)
            .HasPrecision(18, 2);

        // AccountHold configuration
        modelBuilder.Entity<AccountHold>()
            .HasOne(h => h.Account)
            .WithMany(a => a.Holds)
            .HasForeignKey(h => h.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AccountHold>()
            .HasOne(h => h.PlacedByUser)
            .WithMany()
            .HasForeignKey(h => h.PlacedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AccountHold>()
            .HasOne(h => h.ReleasedByUser)
            .WithMany()
            .HasForeignKey(h => h.ReleasedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AccountHold>()
            .Property(h => h.Amount)
            .HasPrecision(18, 2);

        // AccountRestriction configuration
        modelBuilder.Entity<AccountRestriction>()
            .HasOne(r => r.Account)
            .WithMany(a => a.Restrictions)
            .HasForeignKey(r => r.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AccountRestriction>()
            .HasOne(r => r.AppliedByUser)
            .WithMany()
            .HasForeignKey(r => r.AppliedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AccountRestriction>()
            .HasOne(r => r.RemovedByUser)
            .WithMany()
            .HasForeignKey(r => r.RemovedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<AccountRestriction>()
            .Property(r => r.DailyLimit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<AccountRestriction>()
            .Property(r => r.MonthlyLimit)
            .HasPrecision(18, 2);

        // AccountStatusHistory configuration
        modelBuilder.Entity<AccountStatusHistory>()
            .HasOne(h => h.Account)
            .WithMany(a => a.StatusHistory)
            .HasForeignKey(h => h.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AccountStatusHistory>()
            .HasOne(h => h.ChangedByUser)
            .WithMany()
            .HasForeignKey(h => h.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        // FeeSchedule configuration
        modelBuilder.Entity<FeeSchedule>()
            .HasOne(f => f.CreatedByUser)
            .WithMany()
            .HasForeignKey(f => f.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FeeSchedule>()
            .Property(f => f.Amount)
            .HasPrecision(18, 2);

        modelBuilder.Entity<FeeSchedule>()
            .Property(f => f.MinimumBalanceThreshold)
            .HasPrecision(18, 2);

        modelBuilder.Entity<FeeSchedule>()
            .Property(f => f.MaximumBalanceThreshold)
            .HasPrecision(18, 2);

        modelBuilder.Entity<FeeSchedule>()
            .Property(f => f.WaiverMinimumBalance)
            .HasPrecision(18, 2);

        // JointAccountHolder configuration
        modelBuilder.Entity<JointAccountHolder>()
            .HasOne(j => j.Account)
            .WithMany(a => a.JointHolders)
            .HasForeignKey(j => j.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JointAccountHolder>()
            .HasOne(j => j.User)
            .WithMany()
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JointAccountHolder>()
            .HasOne(j => j.AddedByUser)
            .WithMany()
            .HasForeignKey(j => j.AddedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JointAccountHolder>()
            .HasOne(j => j.RemovedByUser)
            .WithMany()
            .HasForeignKey(j => j.RemovedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<JointAccountHolder>()
            .Property(j => j.TransactionLimit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<JointAccountHolder>()
            .Property(j => j.DailyLimit)
            .HasPrecision(18, 2);

        modelBuilder.Entity<JointAccountHolder>()
            .HasIndex(j => new { j.AccountId, j.UserId })
            .HasDatabaseName("IX_JointAccountHolders_AccountId_UserId");
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
