using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Configuration for ASP.NET Identity UserToken table to handle key length limitations
/// </summary>
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

