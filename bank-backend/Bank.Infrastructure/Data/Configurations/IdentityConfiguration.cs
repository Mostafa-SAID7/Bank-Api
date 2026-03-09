using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for ASP.NET Identity tables to handle key length limitations
/// </summary>
public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserLogin<Guid>> builder)
    {
        // Reduce key lengths to avoid SQL Server 900-byte limit warnings
        builder.Property(l => l.LoginProvider)
            .HasMaxLength(128); // Reduced from default 450
            
        builder.Property(l => l.ProviderKey)
            .HasMaxLength(128); // Reduced from default 450
    }
}

public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
{
    public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
    {
        // Reduce key lengths to avoid SQL Server 900-byte limit warnings
        builder.Property(t => t.LoginProvider)
            .HasMaxLength(128); // Reduced from default 450
            
        builder.Property(t => t.Name)
            .HasMaxLength(128); // Reduced from default 450
    }
}