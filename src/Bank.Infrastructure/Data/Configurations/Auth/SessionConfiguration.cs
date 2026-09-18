using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for Session entity
/// </summary>
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

        builder.HasIndex(s => s.RefreshTokenHash)
            .HasDatabaseName("IX_Sessions_RefreshTokenHash");

        builder.HasIndex(s => new { s.UserId, s.Status })
            .HasDatabaseName("IX_Sessions_UserId_Status");

        builder.HasIndex(s => s.ExpiresAt)
            .HasDatabaseName("IX_Sessions_ExpiresAt");

        builder.HasIndex(s => s.IpAddress)
            .HasDatabaseName("IX_Sessions_IpAddress");

        builder.Property(s => s.SessionToken)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(s => s.RefreshTokenHash)
            .HasMaxLength(256)
            .IsRequired(false);

        builder.Property(s => s.ReplacedByToken)
            .HasMaxLength(256)
            .IsRequired(false);



        builder.Property(s => s.IpAddress)
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(s => s.UserAgent)
            .HasMaxLength(500)
            .IsRequired();
    }
}

