using Bank.Domain.Entities;
using Account = Bank.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for AccountRestriction entity
/// </summary>
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


