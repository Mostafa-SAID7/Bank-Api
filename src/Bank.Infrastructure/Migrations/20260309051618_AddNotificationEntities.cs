using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardNumber = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false, comment: "Encrypted card number"),
                    MaskedCardNumber = table.Column<string>(type: "nvarchar(19)", maxLength: 19, nullable: false, comment: "Masked card number for display"),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "date", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ActivationChannel = table.Column<int>(type: "int", nullable: true),
                    SecurityCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Encrypted security code"),
                    DailyLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 5000m),
                    MonthlyLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 50000m),
                    AtmDailyLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 2000m),
                    ContactlessEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    OnlineTransactionsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    InternationalTransactionsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PinHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PinSetDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FailedPinAttempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastBlockedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastBlockReason = table.Column<int>(type: "int", nullable: true),
                    BlockedMerchantCategories = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "JSON array of blocked merchant categories"),
                    CardName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousStatus = table.Column<int>(type: "int", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ChangedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Channel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true),
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
                    table.PrimaryKey("PK_CardStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardStatusHistories_AspNetUsers_ChangedBy",
                        column: x => x.ChangedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CardStatusHistories_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NetworkTransactionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AuthorizationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "USD"),
                    OriginalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OriginalCurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(10,6)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    AuthorizationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SettlementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MerchantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MerchantId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MerchantCategory = table.Column<int>(type: "int", nullable: true),
                    MerchantCity = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MerchantCountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    TerminalId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsContactless = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsInternational = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PinUsed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DeclineReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FraudScore = table.Column<decimal>(type: "decimal(5,4)", nullable: true),
                    IsFraudulent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    FeeDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BalanceAfterTransaction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true, comment: "Additional transaction metadata in JSON format"),
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
                    table.PrimaryKey("PK_CardTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardTransactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardTransactions_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_AccountId",
                table: "Cards",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardNumber",
                table: "Cards",
                column: "CardNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CustomerId",
                table: "Cards",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CustomerId_Status",
                table: "Cards",
                columns: new[] { "CustomerId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ExpiryDate",
                table: "Cards",
                column: "ExpiryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Status",
                table: "Cards",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CardStatusHistories_CardId",
                table: "CardStatusHistories",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardStatusHistories_CardId_ChangeDate",
                table: "CardStatusHistories",
                columns: new[] { "CardId", "ChangeDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CardStatusHistories_ChangeDate",
                table: "CardStatusHistories",
                column: "ChangeDate");

            migrationBuilder.CreateIndex(
                name: "IX_CardStatusHistories_ChangedBy",
                table: "CardStatusHistories",
                column: "ChangedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_AccountId",
                table: "CardTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_CardId",
                table: "CardTransactions",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_CardId_Status",
                table: "CardTransactions",
                columns: new[] { "CardId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_CardId_TransactionDate",
                table: "CardTransactions",
                columns: new[] { "CardId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_FraudScore",
                table: "CardTransactions",
                column: "FraudScore",
                filter: "[FraudScore] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_MerchantId",
                table: "CardTransactions",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_NetworkTransactionId",
                table: "CardTransactions",
                column: "NetworkTransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_SettlementDate",
                table: "CardTransactions",
                column: "SettlementDate");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_Status",
                table: "CardTransactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_TransactionDate",
                table: "CardTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_TransactionId",
                table: "CardTransactions",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_CardTransactions_Type",
                table: "CardTransactions",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardStatusHistories");

            migrationBuilder.DropTable(
                name: "CardTransactions");

            migrationBuilder.DropTable(
                name: "Cards");
        }
    }
}
