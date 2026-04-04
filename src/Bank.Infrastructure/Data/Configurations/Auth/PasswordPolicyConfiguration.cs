using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for PasswordPolicy entity
/// </summary>
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

