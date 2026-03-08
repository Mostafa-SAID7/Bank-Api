using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionManagementAndSecurityEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountLockouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FailedAttempts = table.Column<int>(type: "int", nullable: false),
                    LockedUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LockoutReason = table.Column<int>(type: "int", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LastFailedAttempt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastSuccessfulLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrentlyLocked = table.Column<bool>(type: "bit", nullable: false),
                    LockoutNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LockedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLockouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountLockouts_AspNetUsers_LockedByUserId",
                        column: x => x.LockedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AccountLockouts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IpWhitelists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    IpRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpWhitelists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IpWhitelists_AspNetUsers_ApprovedByUserId",
                        column: x => x.ApprovedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_IpWhitelists_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordSetAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PasswordSalt = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    IsCurrentPassword = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordHistories_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordPolicies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ComplexityLevel = table.Column<int>(type: "int", nullable: false),
                    MinimumLength = table.Column<int>(type: "int", nullable: false),
                    MaximumLength = table.Column<int>(type: "int", nullable: false),
                    RequireUppercase = table.Column<bool>(type: "bit", nullable: false),
                    RequireLowercase = table.Column<bool>(type: "bit", nullable: false),
                    RequireDigits = table.Column<bool>(type: "bit", nullable: false),
                    RequireSpecialCharacters = table.Column<bool>(type: "bit", nullable: false),
                    AllowedSpecialCharacters = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinimumUniqueCharacters = table.Column<int>(type: "int", nullable: false),
                    PreventCommonPasswords = table.Column<bool>(type: "bit", nullable: false),
                    PreventUserInfoInPassword = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHistoryCount = table.Column<int>(type: "int", nullable: false),
                    MaxPasswordAge = table.Column<TimeSpan>(type: "time", nullable: false),
                    MinPasswordAge = table.Column<TimeSpan>(type: "time", nullable: true),
                    MaxFailedAttempts = table.Column<int>(type: "int", nullable: false),
                    LockoutDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionToken = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshTokenExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DeviceFingerprint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastActivityAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TerminatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TerminationReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAdminSession = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountLockouts_IsCurrentlyLocked",
                table: "AccountLockouts",
                column: "IsCurrentlyLocked");

            migrationBuilder.CreateIndex(
                name: "IX_AccountLockouts_LockedByUserId",
                table: "AccountLockouts",
                column: "LockedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountLockouts_LockedUntil",
                table: "AccountLockouts",
                column: "LockedUntil");

            migrationBuilder.CreateIndex(
                name: "IX_AccountLockouts_UserId",
                table: "AccountLockouts",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IpWhitelists_ApprovedByUserId",
                table: "IpWhitelists",
                column: "ApprovedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IpWhitelists_CreatedByUserId",
                table: "IpWhitelists",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_IpWhitelists_ExpiresAt",
                table: "IpWhitelists",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_IpWhitelists_IpAddress_Type",
                table: "IpWhitelists",
                columns: new[] { "IpAddress", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_IpWhitelists_Type_IsActive",
                table: "IpWhitelists",
                columns: new[] { "Type", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordHistories_UserId_IsCurrentPassword",
                table: "PasswordHistories",
                columns: new[] { "UserId", "IsCurrentPassword" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordHistories_UserId_PasswordSetAt",
                table: "PasswordHistories",
                columns: new[] { "UserId", "PasswordSetAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PasswordPolicies_ComplexityLevel",
                table: "PasswordPolicies",
                column: "ComplexityLevel",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordPolicies_IsDefault",
                table: "PasswordPolicies",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordPolicies_Name",
                table: "PasswordPolicies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_ExpiresAt",
                table: "Sessions",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_IpAddress",
                table: "Sessions",
                column: "IpAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_RefreshToken",
                table: "Sessions",
                column: "RefreshToken");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SessionToken",
                table: "Sessions",
                column: "SessionToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId_Status",
                table: "Sessions",
                columns: new[] { "UserId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLockouts");

            migrationBuilder.DropTable(
                name: "IpWhitelists");

            migrationBuilder.DropTable(
                name: "PasswordHistories");

            migrationBuilder.DropTable(
                name: "PasswordPolicies");

            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}

