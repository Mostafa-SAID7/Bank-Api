-- Safe Migration Script for Banking Database
-- This script checks for existing objects before creating them

-- Create __EFMigrationsHistory table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END

-- Add CustomerId column to Accounts table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Accounts') AND name = 'CustomerId')
BEGIN
    ALTER TABLE [Accounts] ADD [CustomerId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END

-- Create Billers table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Billers')
BEGIN
    CREATE TABLE [Billers] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [Category] int NOT NULL,
        [AccountNumber] nvarchar(50) NOT NULL,
        [RoutingNumber] nvarchar(20) NOT NULL,
        [Address] nvarchar(500) NOT NULL,
        [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
        [SupportedPaymentMethods] nvarchar(1000) NOT NULL DEFAULT N'[]',
        [MinAmount] decimal(18,2) NOT NULL DEFAULT 0.01,
        [MaxAmount] decimal(18,2) NOT NULL DEFAULT 10000.0,
        [ProcessingDays] int NOT NULL DEFAULT 1,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Billers] PRIMARY KEY ([Id])
    );
    
    -- Create indexes for Billers
    CREATE UNIQUE INDEX [IX_Billers_AccountNumber_RoutingNumber] ON [Billers] ([AccountNumber], [RoutingNumber]);
    CREATE INDEX [IX_Billers_Category] ON [Billers] ([Category]);
    CREATE INDEX [IX_Billers_IsActive] ON [Billers] ([IsActive]);
    CREATE INDEX [IX_Billers_Name] ON [Billers] ([Name]);
END

-- Create BillPayments table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BillPayments')
BEGIN
    CREATE TABLE [BillPayments] (
        [Id] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [BillerId] uniqueidentifier NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [Currency] nvarchar(3) NOT NULL DEFAULT N'USD',
        [ScheduledDate] datetime2 NOT NULL,
        [ProcessedDate] datetime2 NULL,
        [Status] int NOT NULL DEFAULT 1,
        [Reference] nvarchar(100) NOT NULL DEFAULT N'',
        [Description] nvarchar(500) NOT NULL DEFAULT N'',
        [RecurringPaymentId] uniqueidentifier NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_BillPayments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BillPayments_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_BillPayments_Billers_BillerId] FOREIGN KEY ([BillerId]) REFERENCES [Billers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_BillPayments_RecurringPayments_RecurringPaymentId] FOREIGN KEY ([RecurringPaymentId]) REFERENCES [RecurringPayments] ([Id]) ON DELETE SET NULL
    );
    
    -- Create indexes for BillPayments
    CREATE INDEX [IX_BillPayments_BillerId] ON [BillPayments] ([BillerId]);
    CREATE INDEX [IX_BillPayments_CustomerId] ON [BillPayments] ([CustomerId]);
    CREATE INDEX [IX_BillPayments_CustomerId_Status] ON [BillPayments] ([CustomerId], [Status]);
    CREATE INDEX [IX_BillPayments_RecurringPaymentId] ON [BillPayments] ([RecurringPaymentId]);
    CREATE INDEX [IX_BillPayments_ScheduledDate] ON [BillPayments] ([ScheduledDate]);
    CREATE INDEX [IX_BillPayments_Status] ON [BillPayments] ([Status]);
    CREATE INDEX [IX_BillPayments_Status_ScheduledDate] ON [BillPayments] ([Status], [ScheduledDate]);
END

-- Create BillPresentments table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BillPresentments')
BEGIN
    CREATE TABLE [BillPresentments] (
        [Id] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [BillerId] uniqueidentifier NOT NULL,
        [AccountNumber] nvarchar(50) NOT NULL,
        [AmountDue] decimal(18,2) NOT NULL,
        [MinimumPayment] decimal(18,2) NOT NULL,
        [DueDate] datetime2 NOT NULL,
        [StatementDate] datetime2 NOT NULL,
        [Currency] nvarchar(3) NOT NULL DEFAULT N'USD',
        [Status] int NOT NULL DEFAULT 1,
        [BillNumber] nvarchar(100) NOT NULL,
        [ExternalBillId] nvarchar(100) NOT NULL,
        [PaidDate] datetime2 NULL,
        [PaymentId] uniqueidentifier NULL,
        [LineItemsJson] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_BillPresentments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_BillPresentments_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_BillPresentments_BillPayments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [BillPayments] ([Id]) ON DELETE SET NULL,
        CONSTRAINT [FK_BillPresentments_Billers_BillerId] FOREIGN KEY ([BillerId]) REFERENCES [Billers] ([Id]) ON DELETE NO ACTION
    );
    
    -- Create indexes for BillPresentments
    CREATE INDEX [IX_BillPresentments_BillerId] ON [BillPresentments] ([BillerId]);
    CREATE INDEX [IX_BillPresentments_CustomerId] ON [BillPresentments] ([CustomerId]);
    CREATE INDEX [IX_BillPresentments_CustomerId_Status] ON [BillPresentments] ([CustomerId], [Status]);
    CREATE INDEX [IX_BillPresentments_DueDate] ON [BillPresentments] ([DueDate]);
    CREATE UNIQUE INDEX [IX_BillPresentments_ExternalBillId] ON [BillPresentments] ([ExternalBillId]);
    CREATE INDEX [IX_BillPresentments_PaymentId] ON [BillPresentments] ([PaymentId]);
    CREATE INDEX [IX_BillPresentments_Status] ON [BillPresentments] ([Status]);
END

-- Create DepositProducts table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DepositProducts')
BEGIN
    CREATE TABLE [DepositProducts] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [ProductType] int NOT NULL,
        [IsActive] bit NOT NULL,
        [MinimumTermDays] int NULL,
        [MaximumTermDays] int NULL,
        [DefaultTermDays] int NULL,
        [MinimumBalance] decimal(18,2) NOT NULL,
        [MaximumBalance] decimal(18,2) NULL,
        [MinimumOpeningBalance] decimal(18,2) NOT NULL,
        [BaseInterestRate] decimal(5,4) NOT NULL,
        [InterestCalculationMethod] int NOT NULL,
        [CompoundingFrequency] int NOT NULL,
        [HasTieredRates] bit NOT NULL,
        [AllowPartialWithdrawals] bit NOT NULL,
        [PenaltyType] int NOT NULL,
        [PenaltyAmount] decimal(18,2) NULL,
        [PenaltyPercentage] decimal(5,4) NULL,
        [PenaltyFreeDays] int NULL,
        [DefaultMaturityAction] int NOT NULL,
        [AllowAutoRenewal] bit NOT NULL,
        [AutoRenewalNoticeDays] int NULL,
        [PromotionalRateStartDate] datetime2 NULL,
        [PromotionalRateEndDate] datetime2 NULL,
        [PromotionalRate] decimal(5,4) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_DepositProducts] PRIMARY KEY ([Id])
    );
    
    -- Create indexes for DepositProducts
    CREATE INDEX [IX_DepositProducts_IsActive] ON [DepositProducts] ([IsActive]);
    CREATE INDEX [IX_DepositProducts_ProductType] ON [DepositProducts] ([ProductType]);
    CREATE INDEX [IX_DepositProducts_ProductType_IsActive] ON [DepositProducts] ([ProductType], [IsActive]);
END

-- Create NotificationPreferences table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NotificationPreferences')
BEGIN
    CREATE TABLE [NotificationPreferences] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [TransactionAlerts] bit NOT NULL DEFAULT CAST(1 AS bit),
        [SecurityAlerts] bit NOT NULL DEFAULT CAST(1 AS bit),
        [LowBalanceAlerts] bit NOT NULL DEFAULT CAST(1 AS bit),
        [PaymentReminders] bit NOT NULL DEFAULT CAST(1 AS bit),
        [MarketingNotifications] bit NOT NULL DEFAULT CAST(0 AS bit),
        [CardAlerts] bit NOT NULL DEFAULT CAST(1 AS bit),
        [LoanAlerts] bit NOT NULL DEFAULT CAST(1 AS bit),
        [AccountUpdates] bit NOT NULL DEFAULT CAST(1 AS bit),
        [TransactionAlertThreshold] decimal(18,2) NOT NULL DEFAULT 0.0,
        [LowBalanceThreshold] decimal(18,2) NOT NULL DEFAULT 100.0,
        [PreferredChannels] nvarchar(500) NOT NULL DEFAULT N'[1,2]',
        [PhoneNumber] nvarchar(20) NULL,
        [Email] nvarchar(256) NULL,
        [Language] nvarchar(10) NOT NULL DEFAULT N'en',
        [TimeZone] nvarchar(50) NOT NULL DEFAULT N'UTC',
        [QuietHoursStart] time NULL,
        [QuietHoursEnd] time NULL,
        [AllowCriticalDuringQuietHours] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_NotificationPreferences] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_NotificationPreferences_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    CREATE UNIQUE INDEX [IX_NotificationPreferences_UserId] ON [NotificationPreferences] ([UserId]);
END

-- Create Notifications table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications')
BEGIN
    CREATE TABLE [Notifications] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Type] int NOT NULL,
        [Subject] nvarchar(200) NOT NULL,
        [Message] nvarchar(2000) NOT NULL,
        [Channel] int NOT NULL,
        [Status] int NOT NULL,
        [Priority] int NOT NULL,
        [SentAt] datetime2 NULL,
        [ReadAt] datetime2 NULL,
        [ScheduledAt] datetime2 NULL,
        [ExpiresAt] datetime2 NULL,
        [Data] nvarchar(max) NULL,
        [ErrorMessage] nvarchar(1000) NULL,
        [RetryCount] int NOT NULL DEFAULT 0,
        [MaxRetries] int NOT NULL DEFAULT 3,
        [ExternalReferenceId] nvarchar(100) NULL,
        [TemplateId] nvarchar(50) NULL,
        [Language] nvarchar(10) NOT NULL DEFAULT N'en',
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Notifications_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    -- Create indexes for Notifications
    CREATE INDEX [IX_Notifications_CreatedAt] ON [Notifications] ([CreatedAt]);
    CREATE INDEX [IX_Notifications_ScheduledAt] ON [Notifications] ([ScheduledAt]);
    CREATE INDEX [IX_Notifications_Status] ON [Notifications] ([Status]);
    CREATE INDEX [IX_Notifications_Type] ON [Notifications] ([Type]);
    CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
    CREATE INDEX [IX_Notifications_UserId_Status] ON [Notifications] ([UserId], [Status]);
END

-- Record all migrations as applied
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
SELECT * FROM (VALUES 
    (N'20260306183019_InitialCreate', N'9.0.3'),
    (N'20260308055601_AddAuditLogsTable', N'9.0.3'),
    (N'20260308093217_AddSessionManagementAndSecurityEntities', N'9.0.3'),
    (N'20260308133813_AddRecurringPaymentsAndTemplates', N'9.0.3'),
    (N'20260308153419_AddBeneficiaryEntity', N'9.0.3'),
    (N'20260308192742_AddLoanEntities', N'9.0.3'),
    (N'20260309051618_AddNotificationEntities', N'9.0.3'),
    (N'20260309051809_UpdateNotificationEntities', N'9.0.3'),
    (N'20260309123722_UpdateBillPresentmentAndCurrentModel', N'9.0.3')
) AS v([MigrationId], [ProductVersion])
WHERE NOT EXISTS (
    SELECT 1 FROM [__EFMigrationsHistory] h 
    WHERE h.[MigrationId] = v.[MigrationId]
);

PRINT 'Safe migration completed successfully!';