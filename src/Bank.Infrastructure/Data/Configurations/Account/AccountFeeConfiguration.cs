using Bank.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Account;

/// <summary>
/// Entity Framework configuration for AccountFee entity
/// </summary>
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
