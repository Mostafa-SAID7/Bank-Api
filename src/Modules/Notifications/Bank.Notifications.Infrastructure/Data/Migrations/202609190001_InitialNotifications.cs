using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bank.Notifications.Infrastructure.Data.Migrations;

[DbContext(typeof(NotificationsDbContext))]
[Migration("202609190001_InitialNotifications")]
public sealed class InitialNotifications : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(name: "notifications");

        migrationBuilder.CreateTable(
            name: "Notifications",
            schema: "notifications",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                UserId = table.Column<Guid>(type: "uuid", nullable: false),
                Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Subject = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Message = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                Channel = table.Column<int>(type: "integer", nullable: false),
                Priority = table.Column<int>(type: "integer", nullable: false),
                IdempotencyKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                ScheduledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                Data = table.Column<string>(type: "jsonb", nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_Notifications", notification => notification.Id));

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_IdempotencyKey",
            schema: "notifications",
            table: "Notifications",
            column: "IdempotencyKey",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_UserId_CreatedAt",
            schema: "notifications",
            table: "Notifications",
            columns: new[] { "UserId", "CreatedAt" });

        migrationBuilder.CreateIndex(
            name: "IX_Notifications_ScheduledAt",
            schema: "notifications",
            table: "Notifications",
            column: "ScheduledAt");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Notifications", schema: "notifications");
    }
}
