using Bank.Domain.Entities;
using Account = Bank.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for AccountStatusHistory entity
/// </summary>
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


