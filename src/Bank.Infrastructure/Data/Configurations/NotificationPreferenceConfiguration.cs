using Bank.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bank.Infrastructure.Data.Configurations;

/// <summary>
/// Entity Framework configuration for NotificationPreference entity
/// </summary>
public class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreference>
{
    public void Configure(EntityTypeBuilder<NotificationPreference> builder)
    {
        builder.ToTable("NotificationPreferences");

        builder.HasKey(np => np.Id);

        builder.Property(np => np.TransactionAlerts)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(np => np.SecurityAlerts)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(np => np.LowBalanceAlerts)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(np => np.PaymentReminders)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(np => np.MarketingNotifications)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(np => np.CardAlerts)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(np => np.LoanAlerts)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(np => np.AccountUpdates)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(np => np.TransactionAlertThreshold)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(0m);

        builder.Property(np => np.LowBalanceThreshold)
            .HasColumnType("decimal(18,2)")
            .HasDefaultValue(100m);

        builder.Property(np => np.PreferredChannels)
            .IsRequired()
            .HasMaxLength(500)
            .HasDefaultValue("[1,2]"); // InApp, Email by default

        builder.Property(np => np.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(np => np.Email)
            .HasMaxLength(256);

        builder.Property(np => np.Language)
            .IsRequired()
            .HasMaxLength(10)
            .HasDefaultValue("en");

        builder.Property(np => np.TimeZone)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("UTC");

        builder.Property(np => np.AllowCriticalDuringQuietHours)
            .IsRequired()
            .HasDefaultValue(true);

        // Relationships
        builder.HasOne(np => np.User)
            .WithMany()
            .HasForeignKey(np => np.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(np => np.UserId)
            .IsUnique(); // One preference record per user
    }
}