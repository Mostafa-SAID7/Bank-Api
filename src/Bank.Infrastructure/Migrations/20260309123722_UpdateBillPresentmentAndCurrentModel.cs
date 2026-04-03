using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBillPresentmentAndCurrentModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardTransactions_Accounts_AccountId",
                table: "CardTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CardTransactions_Transactions_TransactionId",
                table: "CardTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CardTransactions_AccountId",
                table: "CardTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CardTransactions_FraudScore",
                table: "CardTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CardTransactions_NetworkTransactionId",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "AuthorizationDate",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "BalanceAfterTransaction",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "Fee",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "FraudScore",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "IsFraudulent",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "MerchantCountryCode",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "NetworkTransactionId",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "OriginalCurrencyCode",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "PinUsed",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "TerminalId",
                table: "CardTransactions");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "CardTransactions",
                newName: "TransactionType");

            migrationBuilder.RenameColumn(
                name: "TransactionId",
                table: "CardTransactions",
                newName: "OriginalTransactionId");

            migrationBuilder.RenameColumn(
                name: "OriginalAmount",
                table: "CardTransactions",
                newName: "Fees");

            migrationBuilder.RenameColumn(
                name: "MerchantCity",
                table: "CardTransactions",
                newName: "MerchantCountry");

            migrationBuilder.RenameColumn(
                name: "FeeDescription",
                table: "CardTransactions",
                newName: "Reference");

            migrationBuilder.RenameColumn(
                name: "CurrencyCode",
                table: "CardTransactions",
                newName: "Currency");

            migrationBuilder.RenameIndex(
                name: "IX_CardTransactions_Type",
                table: "CardTransactions",
                newName: "IX_CardTransactions_TransactionType");

            migrationBuilder.RenameIndex(
                name: "IX_CardTransactions_TransactionId",
                table: "CardTransactions",
                newName: "IX_CardTransactions_OriginalTransactionId");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantName",
                table: "CardTransactions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MerchantId",
                table: "CardTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MerchantCategory",
                table: "CardTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CardId1",
                table: "CardTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FeeBreakdown",
                table: "CardTransactions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessorResponse",
                table: "CardTransactions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Accounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Billers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoutingNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SupportedPaymentMethods = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, defaultValue: "[]"),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 0.01m),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, defaultValue: 10000.00m),
                    ProcessingDays = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
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
                    table.PrimaryKey("PK_Billers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardAuthorizations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorizationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MerchantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MerchantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MerchantCategory = table.Column<int>(type: "int", nullable: false),
                    MerchantCountry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsInternational = table.Column<bool>(type: "bit", nullable: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false),
                    IsContactless = table.Column<bool>(type: "bit", nullable: false),
                    CapturedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CaptureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VoidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VoidReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessorResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetworkTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_CardAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardAuthorizations_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardStatements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GeneratedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Format = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionCount = table.Column<int>(type: "int", nullable: false),
                    TotalSpent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DeliveryMethod = table.Column<int>(type: "int", nullable: true),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GeneratedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_CardStatements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardStatements_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MinimumTermDays = table.Column<int>(type: "int", nullable: true),
                    MaximumTermDays = table.Column<int>(type: "int", nullable: true),
                    DefaultTermDays = table.Column<int>(type: "int", nullable: true),
                    MinimumBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MaximumBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MinimumOpeningBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BaseInterestRate = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    InterestCalculationMethod = table.Column<int>(type: "int", nullable: false),
                    CompoundingFrequency = table.Column<int>(type: "int", nullable: false),
                    HasTieredRates = table.Column<bool>(type: "bit", nullable: false),
                    AllowPartialWithdrawals = table.Column<bool>(type: "bit", nullable: false),
                    PenaltyType = table.Column<int>(type: "int", nullable: false),
                    PenaltyAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PenaltyPercentage = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: true),
                    PenaltyFreeDays = table.Column<int>(type: "int", nullable: true),
                    DefaultMaturityAction = table.Column<int>(type: "int", nullable: false),
                    AllowAutoRenewal = table.Column<bool>(type: "bit", nullable: false),
                    AutoRenewalNoticeDays = table.Column<int>(type: "int", nullable: true),
                    PromotionalRateStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromotionalRateEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromotionalRate = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: true),
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
                    table.PrimaryKey("PK_DepositProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionAlerts = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    SecurityAlerts = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LowBalanceAlerts = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    PaymentReminders = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    MarketingNotifications = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CardAlerts = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LoanAlerts = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AccountUpdates = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    TransactionAlertThreshold = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    LowBalanceThreshold = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 100m),
                    PreferredChannels = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, defaultValue: "[1,2]"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "en"),
                    TimeZone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "UTC"),
                    QuietHoursStart = table.Column<TimeSpan>(type: "time", nullable: true),
                    QuietHoursEnd = table.Column<TimeSpan>(type: "time", nullable: true),
                    AllowCriticalDuringQuietHours = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_NotificationPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Channel = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RetryCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MaxRetries = table.Column<int>(type: "int", nullable: false, defaultValue: 3),
                    ExternalReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TemplateId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "en"),
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
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillerHealthChecks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsHealthy = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResponseTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HealthMetricsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsecutiveFailures = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastSuccessfulCheck = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_BillerHealthChecks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillerHealthChecks_Billers_BillerId",
                        column: x => x.BillerId,
                        principalTable: "Billers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, defaultValue: ""),
                    RecurringPaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_BillPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillPayments_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillPayments_Billers_BillerId",
                        column: x => x.BillerId,
                        principalTable: "Billers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillPayments_RecurringPayments_RecurringPaymentId",
                        column: x => x.RecurringPaymentId,
                        principalTable: "RecurringPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FixedDeposits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepositNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepositProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    InterestRate = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    TermDays = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaturityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InterestCalculationMethod = table.Column<int>(type: "int", nullable: false),
                    CompoundingFrequency = table.Column<int>(type: "int", nullable: false),
                    AccruedInterest = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LastInterestCalculationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaturityAction = table.Column<int>(type: "int", nullable: false),
                    AutoRenewalEnabled = table.Column<bool>(type: "bit", nullable: false),
                    RenewalTermDays = table.Column<int>(type: "int", nullable: true),
                    RenewalNoticeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerConsentReceived = table.Column<bool>(type: "bit", nullable: false),
                    PenaltyType = table.Column<int>(type: "int", nullable: false),
                    PenaltyAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    PenaltyPercentage = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: true),
                    ClosureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ClosureReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PenaltyApplied = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    NetAmountPaid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    RenewedFromDepositId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RenewedToDepositId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RenewalCount = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_FixedDeposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedDeposits_Accounts_LinkedAccountId",
                        column: x => x.LinkedAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedDeposits_AspNetUsers_ClosedByUserId",
                        column: x => x.ClosedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_FixedDeposits_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedDeposits_DepositProducts_DepositProductId",
                        column: x => x.DepositProductId,
                        principalTable: "DepositProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FixedDeposits_FixedDeposits_RenewedFromDepositId",
                        column: x => x.RenewedFromDepositId,
                        principalTable: "FixedDeposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "InterestTiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepositProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TierName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinimumBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MaximumBalance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    InterestRate = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    TierBasis = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    EffectiveFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPromotional = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_InterestTiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterestTiers_DepositProducts_DepositProductId",
                        column: x => x.DepositProductId,
                        principalTable: "DepositProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillPresentments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BillerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AmountDue = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MinimumPayment = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    BillNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ExternalBillId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaidDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LineItemsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_BillPresentments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillPresentments_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BillPresentments_BillPayments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "BillPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BillPresentments_Billers_BillerId",
                        column: x => x.BillerId,
                        principalTable: "Billers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentReceipts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BillerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConfirmationNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    ProcessingFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReceiptDataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_PaymentReceipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentReceipts_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReceipts_BillPayments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "BillPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentRetries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttemptNumber = table.Column<int>(type: "int", nullable: false),
                    AttemptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NextRetryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BackoffDelay = table.Column<TimeSpan>(type: "time", nullable: false),
                    FailureReason = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsMaxRetriesReached = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RetryMetadataJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_PaymentRetries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentRetries_BillPayments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "BillPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FixedDepositId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertificateNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeliveryReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CertificateTemplate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CertificateContent = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    CertificatePdf = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PdfFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DigitalSignature = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SecurityHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    VerificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplacedCertificateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReplacementReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReplacedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GeneratedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IssuedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProcessingNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                    table.PrimaryKey("PK_DepositCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositCertificates_AspNetUsers_GeneratedByUserId",
                        column: x => x.GeneratedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepositCertificates_AspNetUsers_IssuedByUserId",
                        column: x => x.IssuedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepositCertificates_AspNetUsers_ReplacedByUserId",
                        column: x => x.ReplacedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepositCertificates_DepositCertificates_ReplacedCertificateId",
                        column: x => x.ReplacedCertificateId,
                        principalTable: "DepositCertificates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepositCertificates_FixedDeposits_FixedDepositId",
                        column: x => x.FixedDepositId,
                        principalTable: "FixedDeposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DepositTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FixedDepositId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InterestPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterestPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InterestRate = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: true),
                    InterestDays = table.Column<int>(type: "int", nullable: true),
                    PenaltyType = table.Column<int>(type: "int", nullable: true),
                    PenaltyReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProcessedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessingNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RelatedTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_DepositTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepositTransactions_AspNetUsers_ProcessedByUserId",
                        column: x => x.ProcessedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepositTransactions_DepositTransactions_RelatedTransactionId",
                        column: x => x.RelatedTransactionId,
                        principalTable: "DepositTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_DepositTransactions_FixedDeposits_FixedDepositId",
                        column: x => x.FixedDepositId,
                        principalTable: "FixedDeposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepositTransactions_Transactions_AccountTransactionId",
                        column: x => x.AccountTransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MaturityNotices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FixedDepositId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoticeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NoticeType = table.Column<int>(type: "int", nullable: false),
                    NoticeDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaturityDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    TemplateUsed = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveryChannel = table.Column<int>(type: "int", nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveryReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveryAttempts = table.Column<int>(type: "int", nullable: false),
                    CustomerResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CustomerChoice = table.Column<int>(type: "int", nullable: true),
                    CustomerInstructions = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ConsentReceived = table.Column<bool>(type: "bit", nullable: false),
                    GeneratedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProcessedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessingNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    FollowUpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequiresFollowUp = table.Column<bool>(type: "bit", nullable: false),
                    RemindersSent = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_MaturityNotices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaturityNotices_AspNetUsers_GeneratedByUserId",
                        column: x => x.GeneratedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MaturityNotices_FixedDeposits_FixedDepositId",
                        column: x => x.FixedDepositId,
                        principalTable: "FixedDeposits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_AuthorizationCode",
                table: "CardTransactions",
                column: "AuthorizationCode",
                filter: "[AuthorizationCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_CardId1",
                table: "CardTransactions",
                column: "CardId1");

            migrationBuilder.CreateIndex(
                name: "IX_BillerHealthChecks_BillerId",
                table: "BillerHealthChecks",
                column: "BillerId");

            migrationBuilder.CreateIndex(
                name: "IX_BillerHealthChecks_BillerId_CheckDate",
                table: "BillerHealthChecks",
                columns: new[] { "BillerId", "CheckDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BillerHealthChecks_CheckDate",
                table: "BillerHealthChecks",
                column: "CheckDate");

            migrationBuilder.CreateIndex(
                name: "IX_BillerHealthChecks_IsHealthy",
                table: "BillerHealthChecks",
                column: "IsHealthy");

            migrationBuilder.CreateIndex(
                name: "IX_Billers_AccountNumber_RoutingNumber",
                table: "Billers",
                columns: new[] { "AccountNumber", "RoutingNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Billers_Category",
                table: "Billers",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Billers_IsActive",
                table: "Billers",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Billers_Name",
                table: "Billers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_BillerId",
                table: "BillPayments",
                column: "BillerId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_CustomerId",
                table: "BillPayments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_CustomerId_Status",
                table: "BillPayments",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_RecurringPaymentId",
                table: "BillPayments",
                column: "RecurringPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_ScheduledDate",
                table: "BillPayments",
                column: "ScheduledDate");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_Status",
                table: "BillPayments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BillPayments_Status_ScheduledDate",
                table: "BillPayments",
                columns: new[] { "Status", "ScheduledDate" });

            migrationBuilder.CreateIndex(
                name: "IX_BillPresentments_BillerId",
                table: "BillPresentments",
                column: "BillerId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPresentments_CustomerId",
                table: "BillPresentments",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPresentments_CustomerId_Status",
                table: "BillPresentments",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_BillPresentments_DueDate",
                table: "BillPresentments",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_BillPresentments_ExternalBillId",
                table: "BillPresentments",
                column: "ExternalBillId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillPresentments_PaymentId",
                table: "BillPresentments",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BillPresentments_Status",
                table: "BillPresentments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CardAuthorizations_CardId",
                table: "CardAuthorizations",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardStatements_CardId",
                table: "CardStatements",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCertificates_CertificateNumber",
                table: "DepositCertificates",
                column: "CertificateNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositCertificates_FixedDepositId",
                table: "DepositCertificates",
                column: "FixedDepositId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCertificates_GeneratedByUserId",
                table: "DepositCertificates",
                column: "GeneratedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCertificates_IssueDate",
                table: "DepositCertificates",
                column: "IssueDate");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCertificates_IssuedByUserId",
                table: "DepositCertificates",
                column: "IssuedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCertificates_ReplacedByUserId",
                table: "DepositCertificates",
                column: "ReplacedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCertificates_ReplacedCertificateId",
                table: "DepositCertificates",
                column: "ReplacedCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositCertificates_Status",
                table: "DepositCertificates",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_DepositProducts_IsActive",
                table: "DepositProducts",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_DepositProducts_ProductType",
                table: "DepositProducts",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_DepositProducts_ProductType_IsActive",
                table: "DepositProducts",
                columns: new[] { "ProductType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_AccountTransactionId",
                table: "DepositTransactions",
                column: "AccountTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_FixedDepositId",
                table: "DepositTransactions",
                column: "FixedDepositId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_FixedDepositId_TransactionDate",
                table: "DepositTransactions",
                columns: new[] { "FixedDepositId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_FixedDepositId_TransactionType",
                table: "DepositTransactions",
                columns: new[] { "FixedDepositId", "TransactionType" });

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_ProcessedByUserId",
                table: "DepositTransactions",
                column: "ProcessedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_RelatedTransactionId",
                table: "DepositTransactions",
                column: "RelatedTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_TransactionDate",
                table: "DepositTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_TransactionReference",
                table: "DepositTransactions",
                column: "TransactionReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DepositTransactions_TransactionType",
                table: "DepositTransactions",
                column: "TransactionType");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_ClosedByUserId",
                table: "FixedDeposits",
                column: "ClosedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_CustomerId",
                table: "FixedDeposits",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_CustomerId_Status",
                table: "FixedDeposits",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_DepositNumber",
                table: "FixedDeposits",
                column: "DepositNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_DepositProductId",
                table: "FixedDeposits",
                column: "DepositProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_LinkedAccountId",
                table: "FixedDeposits",
                column: "LinkedAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_MaturityDate",
                table: "FixedDeposits",
                column: "MaturityDate");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_RenewedFromDepositId",
                table: "FixedDeposits",
                column: "RenewedFromDepositId",
                unique: true,
                filter: "[RenewedFromDepositId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_Status",
                table: "FixedDeposits",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FixedDeposits_Status_MaturityDate",
                table: "FixedDeposits",
                columns: new[] { "Status", "MaturityDate" });

            migrationBuilder.CreateIndex(
                name: "IX_InterestTiers_DepositProductId",
                table: "InterestTiers",
                column: "DepositProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InterestTiers_DepositProductId_IsActive",
                table: "InterestTiers",
                columns: new[] { "DepositProductId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_InterestTiers_DepositProductId_MinimumBalance",
                table: "InterestTiers",
                columns: new[] { "DepositProductId", "MinimumBalance" });

            migrationBuilder.CreateIndex(
                name: "IX_InterestTiers_DisplayOrder",
                table: "InterestTiers",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_MaturityNotices_FixedDepositId",
                table: "MaturityNotices",
                column: "FixedDepositId");

            migrationBuilder.CreateIndex(
                name: "IX_MaturityNotices_GeneratedByUserId",
                table: "MaturityNotices",
                column: "GeneratedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaturityNotices_MaturityDate",
                table: "MaturityNotices",
                column: "MaturityDate");

            migrationBuilder.CreateIndex(
                name: "IX_MaturityNotices_NoticeNumber",
                table: "MaturityNotices",
                column: "NoticeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MaturityNotices_NoticeType",
                table: "MaturityNotices",
                column: "NoticeType");

            migrationBuilder.CreateIndex(
                name: "IX_MaturityNotices_Status",
                table: "MaturityNotices",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_MaturityNotices_Status_NoticeDate",
                table: "MaturityNotices",
                columns: new[] { "Status", "NoticeDate" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreferences_UserId",
                table: "NotificationPreferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ScheduledAt",
                table: "Notifications",
                column: "ScheduledAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Status",
                table: "Notifications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Type",
                table: "Notifications",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_Status",
                table: "Notifications",
                columns: new[] { "UserId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceipts_ConfirmationNumber",
                table: "PaymentReceipts",
                column: "ConfirmationNumber");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceipts_CustomerId",
                table: "PaymentReceipts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceipts_PaymentId",
                table: "PaymentReceipts",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceipts_ReceiptNumber",
                table: "PaymentReceipts",
                column: "ReceiptNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceipts_Status",
                table: "PaymentReceipts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRetries_IsMaxRetriesReached",
                table: "PaymentRetries",
                column: "IsMaxRetriesReached");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRetries_NextRetryDate",
                table: "PaymentRetries",
                column: "NextRetryDate");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRetries_PaymentId",
                table: "PaymentRetries",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRetries_PaymentId_AttemptNumber",
                table: "PaymentRetries",
                columns: new[] { "PaymentId", "AttemptNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRetries_Status",
                table: "PaymentRetries",
                column: "Status");

            migrationBuilder.AddForeignKey(
                name: "FK_CardTransactions_CardTransactions_OriginalTransactionId",
                table: "CardTransactions",
                column: "OriginalTransactionId",
                principalTable: "CardTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CardTransactions_Cards_CardId1",
                table: "CardTransactions",
                column: "CardId1",
                principalTable: "Cards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardTransactions_CardTransactions_OriginalTransactionId",
                table: "CardTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_CardTransactions_Cards_CardId1",
                table: "CardTransactions");

            migrationBuilder.DropTable(
                name: "BillerHealthChecks");

            migrationBuilder.DropTable(
                name: "BillPresentments");

            migrationBuilder.DropTable(
                name: "CardAuthorizations");

            migrationBuilder.DropTable(
                name: "CardStatements");

            migrationBuilder.DropTable(
                name: "DepositCertificates");

            migrationBuilder.DropTable(
                name: "DepositTransactions");

            migrationBuilder.DropTable(
                name: "InterestTiers");

            migrationBuilder.DropTable(
                name: "MaturityNotices");

            migrationBuilder.DropTable(
                name: "NotificationPreferences");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PaymentReceipts");

            migrationBuilder.DropTable(
                name: "PaymentRetries");

            migrationBuilder.DropTable(
                name: "FixedDeposits");

            migrationBuilder.DropTable(
                name: "BillPayments");

            migrationBuilder.DropTable(
                name: "DepositProducts");

            migrationBuilder.DropTable(
                name: "Billers");

            migrationBuilder.DropIndex(
                name: "IX_CardTransactions_AuthorizationCode",
                table: "CardTransactions");

            migrationBuilder.DropIndex(
                name: "IX_CardTransactions_CardId1",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "CardId1",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "FeeBreakdown",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "ProcessorResponse",
                table: "CardTransactions");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "TransactionType",
                table: "CardTransactions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Reference",
                table: "CardTransactions",
                newName: "FeeDescription");

            migrationBuilder.RenameColumn(
                name: "OriginalTransactionId",
                table: "CardTransactions",
                newName: "TransactionId");

            migrationBuilder.RenameColumn(
                name: "MerchantCountry",
                table: "CardTransactions",
                newName: "MerchantCity");

            migrationBuilder.RenameColumn(
                name: "Fees",
                table: "CardTransactions",
                newName: "OriginalAmount");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "CardTransactions",
                newName: "CurrencyCode");

            migrationBuilder.RenameIndex(
                name: "IX_CardTransactions_TransactionType",
                table: "CardTransactions",
                newName: "IX_CardTransactions_Type");

            migrationBuilder.RenameIndex(
                name: "IX_CardTransactions_OriginalTransactionId",
                table: "CardTransactions",
                newName: "IX_CardTransactions_TransactionId");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantName",
                table: "CardTransactions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "MerchantId",
                table: "CardTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "MerchantCategory",
                table: "CardTransactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "CardTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthorizationDate",
                table: "CardTransactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "BalanceAfterTransaction",
                table: "CardTransactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                table: "CardTransactions",
                type: "decimal(10,6)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "CardTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "FraudScore",
                table: "CardTransactions",
                type: "decimal(5,4)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFraudulent",
                table: "CardTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MerchantCountryCode",
                table: "CardTransactions",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "CardTransactions",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                comment: "Additional transaction metadata in JSON format");

            migrationBuilder.AddColumn<string>(
                name: "NetworkTransactionId",
                table: "CardTransactions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OriginalCurrencyCode",
                table: "CardTransactions",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PinUsed",
                table: "CardTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TerminalId",
                table: "CardTransactions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_AccountId",
                table: "CardTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_FraudScore",
                table: "CardTransactions",
                column: "FraudScore",
                filter: "[FraudScore] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_NetworkTransactionId",
                table: "CardTransactions",
                column: "NetworkTransactionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CardTransactions_Accounts_AccountId",
                table: "CardTransactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CardTransactions_Transactions_TransactionId",
                table: "CardTransactions",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
