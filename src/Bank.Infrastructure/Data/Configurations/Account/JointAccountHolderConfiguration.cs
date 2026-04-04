using Bank.Domain.Entities;
using Account = Bank.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for JointAccountHolder entity
/// </summary>
public class JointAccountHolderConfiguration : IEntityTypeConfiguration<JointAccountHolder>
{
    public void Configure(EntityTypeBuilder<JointAccountHolder> builder)
    {
        builder.HasOne(j => j.Account)
            .WithMany(a => a.JointHolders)
            .HasForeignKey(j => j.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(j => j.User)
            .WithMany()
            .HasForeignKey(j => j.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(j => j.AddedByUser)
            .WithMany()
            .HasForeignKey(j => j.AddedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(j => j.RemovedByUser)
            .WithMany()
            .HasForeignKey(j => j.RemovedByUserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(j => j.TransactionLimit)
            .HasPrecision(18, 2);

        builder.Property(j => j.DailyLimit)
            .HasPrecision(18, 2);

        builder.HasIndex(j => new { j.AccountId, j.UserId })
            .HasDatabaseName("IX_JointAccountHolders_AccountId_UserId");
    }
}


