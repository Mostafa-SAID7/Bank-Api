using Bank.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Account;

/// <summary>
/// Entity Framework configuration for AccountHold entity
/// </summary>
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
