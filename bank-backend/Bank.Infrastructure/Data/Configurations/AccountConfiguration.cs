using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Account and related entities
/// </summary>
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        // Account configuration
        builder.HasOne(a => a.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.UserId);

        builder.Property(a => a.Balance)
            .HasPrecision(18, 2);

        builder.HasIndex(a => a.AccountNumber)
            .IsUnique();
    }
}

public class AccountFeeConfiguration : IEntityTypeConfiguration<AccountFee>
{
    public void Configure(EntityTypeBuilder<AccountFee> builder)
    {
        builder.HasOne(f => f.Account)
            .WithMany(a => a.Fees)
            .HasForeignKey(f => f.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Transaction)
            .WithMany()
            .HasForeignKey(f => f.TransactionId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(f => f.Amount)
            .HasPrecision(18, 2);
    }
}

public class AccountHoldConfiguration : IEntityTypeConfiguration<AccountHold>
{
    public void Configure(EntityTypeBuilder<AccountHold> builder)
    {
        builder.HasOne(h => h.Account)
            .WithMany(a => a.Holds)
            .HasForeignKey(h => h.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.PlacedByUser)
            .WithMany()
            .HasForeignKey(h => h.PlacedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.ReleasedByUser)
            .WithMany()
            .HasForeignKey(h => h.ReleasedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(h => h.Amount)
            .HasPrecision(18, 2);
    }
}

public class AccountRestrictionConfiguration : IEntityTypeConfiguration<AccountRestriction>
{
    public void Configure(EntityTypeBuilder<AccountRestriction> builder)
    {
        builder.HasOne(r => r.Account)
            .WithMany(a => a.Restrictions)
            .HasForeignKey(r => r.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.AppliedByUser)
            .WithMany()
            .HasForeignKey(r => r.AppliedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.RemovedByUser)
            .WithMany()
            .HasForeignKey(r => r.RemovedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(r => r.DailyLimit)
            .HasPrecision(18, 2);

        builder.Property(r => r.MonthlyLimit)
            .HasPrecision(18, 2);
    }
}

public class AccountStatusHistoryConfiguration : IEntityTypeConfiguration<AccountStatusHistory>
{
    public void Configure(EntityTypeBuilder<AccountStatusHistory> builder)
    {
        builder.HasOne(h => h.Account)
            .WithMany(a => a.StatusHistory)
            .HasForeignKey(h => h.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.ChangedByUser)
            .WithMany()
            .HasForeignKey(h => h.ChangedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class FeeScheduleConfiguration : IEntityTypeConfiguration<FeeSchedule>
{
    public void Configure(EntityTypeBuilder<FeeSchedule> builder)
    {
        builder.HasOne(f => f.CreatedByUser)
            .WithMany()
            .HasForeignKey(f => f.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(f => f.Amount)
            .HasPrecision(18, 2);

        builder.Property(f => f.MinimumBalanceThreshold)
            .HasPrecision(18, 2);

        builder.Property(f => f.MaximumBalanceThreshold)
            .HasPrecision(18, 2);

        builder.Property(f => f.WaiverMinimumBalance)
            .HasPrecision(18, 2);
    }
}

public class JointAccountHolderConfiguration : IEntityTypeConfiguration<JointAccountHolder>
{
    public void Configure(EntityTypeBuilder<JointAccountHolder> builder)
    {
        builder.HasOne(j => j.Account)
            .WithMany(a => a.JointHolders)
            .HasForeignKey(j => j.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(j => j.User)
            .WithMany()
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(j => j.AddedByUser)
            .WithMany()
            .HasForeignKey(j => j.AddedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(j => j.RemovedByUser)
            .WithMany()
            .HasForeignKey(j => j.RemovedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(j => j.TransactionLimit)
            .HasPrecision(18, 2);

        builder.Property(j => j.DailyLimit)
            .HasPrecision(18, 2);

        builder.HasIndex(j => new { j.AccountId, j.UserId })
            .HasDatabaseName("IX_JointAccountHolders_AccountId_UserId");
    }
}