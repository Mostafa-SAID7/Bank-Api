using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for TwoFactorToken entity
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

