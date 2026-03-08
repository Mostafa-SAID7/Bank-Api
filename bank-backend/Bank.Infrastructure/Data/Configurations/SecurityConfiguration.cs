using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for security-related entities
/// </summary>
public class TwoFactorTokenConfiguration : IEntityTypeConfiguration<TwoFactorToken>
{
    public void Configure(EntityTypeBuilder<TwoFactorToken> builder)
    {
        builder.HasOne(t => t.User)
            .WithMany(u => u.TwoFactorTokens)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(t => new { t.UserId, t.Token, t.ExpiresAt })
            .HasDatabaseName("IX_TwoFactorTokens_UserId_Token_ExpiresAt");

        builder.HasIndex(t => t.ExpiresAt)
            .HasDatabaseName("IX_TwoFactorTokens_ExpiresAt");
    }
}

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.SessionToken)
            .IsUnique()
            .HasDatabaseName("IX_Sessions_SessionToken");

        builder.HasIndex(s => s.RefreshToken)
            .HasDatabaseName("IX_Sessions_RefreshToken");

        builder.HasIndex(s => new { s.UserId, s.Status })
            .HasDatabaseName("IX_Sessions_UserId_Status");

        builder.HasIndex(s => s.ExpiresAt)
            .HasDatabaseName("IX_Sessions_ExpiresAt");

        builder.HasIndex(s => s.IpAddress)
            .HasDatabaseName("IX_Sessions_IpAddress");

        builder.Property(s => s.SessionToken)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(s => s.RefreshToken)
            .HasMaxLength(128);

        builder.Property(s => s.IpAddress)
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(s => s.UserAgent)
            .HasMaxLength(500)
            .IsRequired();
    }
}

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

public class IpWhitelistConfiguration : IEntityTypeConfiguration<IpWhitelist>
{
    public void Configure(EntityTypeBuilder<IpWhitelist> builder)
    {
        builder.HasOne(w => w.CreatedByUser)
            .WithMany()
            .HasForeignKey(w => w.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(w => w.ApprovedByUser)
            .WithMany()
            .HasForeignKey(w => w.ApprovedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(w => new { w.IpAddress, w.Type })
            .HasDatabaseName("IX_IpWhitelists_IpAddress_Type");

        builder.HasIndex(w => new { w.Type, w.IsActive })
            .HasDatabaseName("IX_IpWhitelists_Type_IsActive");

        builder.HasIndex(w => w.ExpiresAt)
            .HasDatabaseName("IX_IpWhitelists_ExpiresAt");

        builder.Property(w => w.IpAddress)
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(w => w.IpRange)
            .HasMaxLength(50);

        builder.Property(w => w.Description)
            .HasMaxLength(500)
            .IsRequired();
    }
}

public class PasswordPolicyConfiguration : IEntityTypeConfiguration<PasswordPolicy>
{
    public void Configure(EntityTypeBuilder<PasswordPolicy> builder)
    {
        builder.HasIndex(p => p.IsDefault)
            .HasDatabaseName("IX_PasswordPolicies_IsDefault");

        builder.HasIndex(p => p.ComplexityLevel)
            .IsUnique()
            .HasDatabaseName("IX_PasswordPolicies_ComplexityLevel");

        builder.HasIndex(p => p.Name)
            .IsUnique()
            .HasDatabaseName("IX_PasswordPolicies_Name");

        builder.Property(p => p.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.AllowedSpecialCharacters)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(500);
    }
}

public class PasswordHistoryConfiguration : IEntityTypeConfiguration<PasswordHistory>
{
    public void Configure(EntityTypeBuilder<PasswordHistory> builder)
    {
        builder.HasOne(h => h.User)
            .WithMany()
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(h => new { h.UserId, h.PasswordSetAt })
            .HasDatabaseName("IX_PasswordHistories_UserId_PasswordSetAt");

        builder.HasIndex(h => new { h.UserId, h.IsCurrentPassword })
            .HasDatabaseName("IX_PasswordHistories_UserId_IsCurrentPassword");

        builder.Property(h => h.PasswordHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(h => h.PasswordSalt)
            .HasMaxLength(128);
    }
}