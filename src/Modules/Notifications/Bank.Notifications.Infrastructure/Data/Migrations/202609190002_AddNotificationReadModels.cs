using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bank.Notifications.Infrastructure.Data.Migrations;

[DbContext(typeof(NotificationsDbContext))]
[Migration("202609190002_AddNotificationReadModels")]
public sealed class AddNotificationReadModels : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(name: "Status", table: "Notifications", schema: "notifications", type: "integer", nullable: false, defaultValue: 1);
        migrationBuilder.AddColumn<DateTimeOffset>(name: "SentAt", table: "Notifications", schema: "notifications", type: "timestamp with time zone", nullable: true);
        migrationBuilder.AddColumn<DateTimeOffset>(name: "ReadAt", table: "Notifications", schema: "notifications", type: "timestamp with time zone", nullable: true);
        migrationBuilder.AddColumn<string>(name: "ErrorMessage", table: "Notifications", schema: "notifications", type: "text", nullable: true);
        migrationBuilder.CreateTable(
            name: "NotificationPreferences", schema: "notifications",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                TransactionAlerts = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                SecurityAlerts = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                LowBalanceAlerts = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                PaymentReminders = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                MarketingNotifications = table.Column<bool>(type: "boolean", nullable: false),
                TransactionAlertThreshold = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                LowBalanceThreshold = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 100m),
                PreferredChannels = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, defaultValue: "[1,2]"),
                PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                Language = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "en"),
                TimeZone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "UTC")
            },
            constraints: table => table.PrimaryKey("PK_NotificationPreferences", preference => preference.UserId));
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable("NotificationPreferences", "notifications");
        migrationBuilder.DropColumn("Status", "Notifications", "notifications");
        migrationBuilder.DropColumn("SentAt", "Notifications", "notifications");
        migrationBuilder.DropColumn("ReadAt", "Notifications", "notifications");
        migrationBuilder.DropColumn("ErrorMessage", "Notifications", "notifications");
    }
}
