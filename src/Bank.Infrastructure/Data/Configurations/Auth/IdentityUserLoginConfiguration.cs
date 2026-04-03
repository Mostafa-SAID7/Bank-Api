using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations.Auth;

/// <summary>
/// Configuration for ASP.NET Identity UserLogin table to handle key length limitations
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
