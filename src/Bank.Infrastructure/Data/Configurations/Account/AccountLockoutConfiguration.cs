using Bank.Domain.Entities;
using Account = Bank.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for AccountLockout entity
/// </summary>
public class AccountLockoutConfiguration : IEntityTypeConfiguration<AccountLockout>
{
    public void Configure(EntityTypeBuilder<AccountLockout> builder)
    {
        builder.HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.LockedByUser)
            .WithMany()
            .HasForeignKey(l => l.LockedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(l => l.UserId)
            .IsUnique()
            .HasDatabaseName("IX_AccountLockouts_UserId");

        builder.HasIndex(l => l.IsCurrentlyLocked)
            .HasDatabaseName("IX_AccountLockouts_IsCurrentlyLocked");

        builder.HasIndex(l => l.LockedUntil)
            .HasDatabaseName("IX_AccountLockouts_LockedUntil");

        builder.Property(l => l.IpAddress)
            .HasMaxLength(45);

        builder.Property(l => l.UserAgent)
            .HasMaxLength(500);
    }
}


