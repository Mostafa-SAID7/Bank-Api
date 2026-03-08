using Bank.Domain.Entities;
using Bank.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bank.Infrastructure.Data;

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
    public DbSet<RecurringPayment> RecurringPayments => Set<RecurringPayment>();
    public DbSet<RecurringPaymentExecution> RecurringPaymentExecutions => Set<RecurringPaymentExecution>();
    public DbSet<PaymentTemplate> PaymentTemplates => Set<PaymentTemplate>();
    public DbSet<Beneficiary> Beneficiaries => Set<Beneficiary>();
    public DbSet<AccountStatement> AccountStatements => Set<AccountStatement>();
    public DbSet<StatementTransaction> StatementTransactions => Set<StatementTransaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from the Configurations folder
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankDbContext).Assembly);

        // Global soft delete query filter for entities inheriting BaseEntity
        ApplySoftDeleteFilters(modelBuilder);
    }

    private static void ApplySoftDeleteFilters(ModelBuilder modelBuilder)
    {
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
        modelBuilder.Entity<RecurringPayment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<RecurringPaymentExecution>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<PaymentTemplate>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Beneficiary>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<AccountStatement>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<StatementTransaction>().HasQueryFilter(e => !e.IsDeleted);
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