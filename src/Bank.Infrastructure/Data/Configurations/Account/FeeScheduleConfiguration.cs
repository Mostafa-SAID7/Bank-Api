using Bank.Domain.Entities;
using Account = Bank.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for FeeSchedule entity
/// </summary>
public class FeeScheduleConfiguration : IEntityTypeConfiguration<FeeSchedule>
{
    public void Configure(EntityTypeBuilder<FeeSchedule> builder)
    {
        builder.HasOne(f => f.CreatedByUser)
            .WithMany()
            .HasForeignKey(f => f.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(f => f.Amount)
            .HasPrecision(18, 2);

        builder.Property(f => f.MinimumBalanceThreshold)
            .HasPrecision(18, 2);

        builder.Property(f => f.MaximumBalanceThreshold)
            .HasPrecision(18, 2);

        builder.Property(f => f.WaiverMinimumBalance)
            .HasPrecision(18, 2);
    }
}


