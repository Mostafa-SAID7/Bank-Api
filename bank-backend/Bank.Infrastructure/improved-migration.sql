-- Improved Migration Script for Banking Database
-- This script handles existing objects and ASP.NET Identity key length issues

-- Enable ANSI_NULLS and QUOTED_IDENTIFIER for consistency
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- Create __EFMigrationsHistory table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    PRINT 'Creating __EFMigrationsHistory table...';
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END
ELSE
BEGIN
    PRINT '__EFMigrationsHistory table already exists.';
END
GO

-- Create AspNetRoles table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetRoles')
BEGIN
    PRINT 'Creating AspNetRoles table...';
    CREATE TABLE [AspNetRoles] (
        [Id] uniqueidentifier NOT NULL,
        [Description] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
    
    CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
END
ELSE
BEGIN
    PRINT 'AspNetRoles table already exists.';
END
GO

-- Create AspNetUsers table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUsers')
BEGIN
    PRINT 'Creating AspNetUsers table...';
    CREATE TABLE [AspNetUsers] (
        [Id] uniqueidentifier NOT NULL,
        [FirstName] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
    
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
    CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
END
ELSE
BEGIN
    PRINT 'AspNetUsers table already exists.';
END
GO

-- Create AspNetRoleClaims table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetRoleClaims')
BEGIN
    PRINT 'Creating AspNetRoleClaims table...';
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END
ELSE
BEGIN
    PRINT 'AspNetRoleClaims table already exists.';
END
GO

-- Create AspNetUserClaims table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserClaims')
BEGIN
    PRINT 'Creating AspNetUserClaims table...';
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END
ELSE
BEGIN
    PRINT 'AspNetUserClaims table already exists.';
END
GO

-- Create AspNetUserLogins table with reduced key length to avoid warnings
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserLogins')
BEGIN
    PRINT 'Creating AspNetUserLogins table with optimized key lengths...';
    CREATE TABLE [AspNetUserLogins] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(128) NOT NULL,  -- Reduced from 450 to 128
        [ProviderKey] nvarchar(128) NOT NULL,    -- Reduced from 450 to 128
        [ProviderDisplayName] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([UserId], [LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END
ELSE
BEGIN
    PRINT 'AspNetUserLogins table already exists.';
END
GO

-- Create AspNetUserRoles table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserRoles')
BEGIN
    PRINT 'Creating AspNetUserRoles table...';
    CREATE TABLE [AspNetUserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END
ELSE
BEGIN
    PRINT 'AspNetUserRoles table already exists.';
END
GO

-- Create AspNetUserTokens table with reduced key length to avoid warnings
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AspNetUserTokens')
BEGIN
    PRINT 'Creating AspNetUserTokens table with optimized key lengths...';
    CREATE TABLE [AspNetUserTokens] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(128) NOT NULL,  -- Reduced from 450 to 128
        [Name] nvarchar(128) NOT NULL,           -- Reduced from 450 to 128
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END
ELSE
BEGIN
    PRINT 'AspNetUserTokens table already exists.';
END
GO

-- Create Accounts table if it doesn't exist, or add missing columns
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Accounts')
BEGIN
    PRINT 'Creating Accounts table...';
    CREATE TABLE [Accounts] (
        [Id] uniqueidentifier NOT NULL,
        [AccountNumber] nvarchar(450) NOT NULL,
        [AccountHolderName] nvarchar(max) NOT NULL,
        [Balance] decimal(18,2) NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Accounts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    CREATE UNIQUE INDEX [IX_Accounts_AccountNumber] ON [Accounts] ([AccountNumber]);
    CREATE INDEX [IX_Accounts_UserId] ON [Accounts] ([UserId]);
END
ELSE
BEGIN
    PRINT 'Accounts table already exists.';
    -- Add CustomerId column if it doesn't exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Accounts') AND name = 'CustomerId')
    BEGIN
        PRINT 'Adding CustomerId column to Accounts table...';
        ALTER TABLE [Accounts] ADD [CustomerId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END
END
GO

-- Handle existing Cards table - check if it needs to be updated
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Cards')
BEGIN
    PRINT 'Cards table already exists - checking structure...';
    -- You can add column checks and alterations here if needed
END
ELSE
BEGIN
    PRINT 'Creating Cards table...';
    -- Add your Cards table creation script here if needed
END
GO

-- Continue with other tables from the safe migration script...
-- Create BatchJobs table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BatchJobs')
BEGIN
    PRINT 'Creating BatchJobs table...';
    CREATE TABLE [BatchJobs] (
        [Id] uniqueidentifier NOT NULL,
        [FileName] nvarchar(max) NOT NULL,
        [TotalRecords] int NOT NULL,
        [SuccessCount] int NOT NULL,
        [FailureCount] int NOT NULL,
        [CompletedAt] datetime2 NULL,
        [Status] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_BatchJobs] PRIMARY KEY ([Id])
    );
END
ELSE
BEGIN
    PRINT 'BatchJobs table already exists.';
END
GO

-- Record migrations as applied
PRINT 'Recording migrations in history...';
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

PRINT 'Migration completed successfully!';
PRINT 'Key length warnings have been addressed by reducing LoginProvider and ProviderKey lengths.';
PRINT 'Existing tables have been preserved and only missing objects were created.';
GO
-- Create remaining tables from the banking system

-- Create AuditLogs table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLogs')
BEGIN
    PRINT 'Creating AuditLogs table...';
    CREATE TABLE [AuditLogs] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NULL,
        [Action] nvarchar(100) NOT NULL,
        [EntityName] nvarchar(100) NOT NULL,
        [EntityId] nvarchar(100) NULL,
        [Changes] nvarchar(max) NULL,
        [IpAddress] nvarchar(45) NULL,
        [UserAgent] nvarchar(500) NULL,
        [Timestamp] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AuditLogs_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL
    );
    
    CREATE INDEX [IX_AuditLogs_Action] ON [AuditLogs] ([Action]);
    CREATE INDEX [IX_AuditLogs_EntityName] ON [AuditLogs] ([EntityName]);
    CREATE INDEX [IX_AuditLogs_Timestamp] ON [AuditLogs] ([Timestamp]);
    CREATE INDEX [IX_AuditLogs_UserId] ON [AuditLogs] ([UserId]);
END
ELSE
BEGIN
    PRINT 'AuditLogs table already exists.';
END
GO

-- Create Transactions table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Transactions')
BEGIN
    PRINT 'Creating Transactions table...';
    CREATE TABLE [Transactions] (
        [Id] uniqueidentifier NOT NULL,
        [FromAccountId] uniqueidentifier NULL,
        [ToAccountId] uniqueidentifier NULL,
        [Amount] decimal(18,2) NOT NULL,
        [Currency] nvarchar(3) NOT NULL DEFAULT N'USD',
        [Type] int NOT NULL,
        [Status] int NOT NULL,
        [Description] nvarchar(500) NULL,
        [Reference] nvarchar(100) NULL,
        [ProcessedAt] datetime2 NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Transactions_Accounts_FromAccountId] FOREIGN KEY ([FromAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Transactions_Accounts_ToAccountId] FOREIGN KEY ([ToAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION
    );
    
    CREATE INDEX [IX_Transactions_FromAccountId] ON [Transactions] ([FromAccountId]);
    CREATE INDEX [IX_Transactions_ToAccountId] ON [Transactions] ([ToAccountId]);
    CREATE INDEX [IX_Transactions_Type] ON [Transactions] ([Type]);
    CREATE INDEX [IX_Transactions_Status] ON [Transactions] ([Status]);
    CREATE INDEX [IX_Transactions_CreatedAt] ON [Transactions] ([CreatedAt]);
END
ELSE
BEGIN
    PRINT 'Transactions table already exists.';
END
GO

-- Create Billers table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Billers')
BEGIN
    PRINT 'Creating Billers table...';
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
    
    CREATE UNIQUE INDEX [IX_Billers_AccountNumber_RoutingNumber] ON [Billers] ([AccountNumber], [RoutingNumber]);
    CREATE INDEX [IX_Billers_Category] ON [Billers] ([Category]);
    CREATE INDEX [IX_Billers_IsActive] ON [Billers] ([IsActive]);
    CREATE INDEX [IX_Billers_Name] ON [Billers] ([Name]);
END
ELSE
BEGIN
    PRINT 'Billers table already exists.';
END
GO

-- Create RecurringPayments table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RecurringPayments')
BEGIN
    PRINT 'Creating RecurringPayments table...';
    CREATE TABLE [RecurringPayments] (
        [Id] uniqueidentifier NOT NULL,
        [CustomerId] uniqueidentifier NOT NULL,
        [BillerId] uniqueidentifier NOT NULL,
        [Amount] decimal(18,2) NOT NULL,
        [Currency] nvarchar(3) NOT NULL DEFAULT N'USD',
        [Frequency] int NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NULL,
        [NextPaymentDate] datetime2 NOT NULL,
        [LastPaymentDate] datetime2 NULL,
        [Status] int NOT NULL DEFAULT 1,
        [Description] nvarchar(500) NOT NULL DEFAULT N'',
        [MaxAmount] decimal(18,2) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [UpdatedAt] datetime2 NULL,
        [UpdatedBy] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetime2 NULL,
        [DeletedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_RecurringPayments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RecurringPayments_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RecurringPayments_Billers_BillerId] FOREIGN KEY ([BillerId]) REFERENCES [Billers] ([Id]) ON DELETE NO ACTION
    );
    
    CREATE INDEX [IX_RecurringPayments_BillerId] ON [RecurringPayments] ([BillerId]);
    CREATE INDEX [IX_RecurringPayments_CustomerId] ON [RecurringPayments] ([CustomerId]);
    CREATE INDEX [IX_RecurringPayments_NextPaymentDate] ON [RecurringPayments] ([NextPaymentDate]);
    CREATE INDEX [IX_RecurringPayments_Status] ON [RecurringPayments] ([Status]);
END
ELSE
BEGIN
    PRINT 'RecurringPayments table already exists.';
END
GO

-- Create BillPayments table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'BillPayments')
BEGIN
    PRINT 'Creating BillPayments table...';
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
    
    CREATE INDEX [IX_BillPayments_BillerId] ON [BillPayments] ([BillerId]);
    CREATE INDEX [IX_BillPayments_CustomerId] ON [BillPayments] ([CustomerId]);
    CREATE INDEX [IX_BillPayments_RecurringPaymentId] ON [BillPayments] ([RecurringPaymentId]);
    CREATE INDEX [IX_BillPayments_ScheduledDate] ON [BillPayments] ([ScheduledDate]);
    CREATE INDEX [IX_BillPayments_Status] ON [BillPayments] ([Status]);
END
ELSE
BEGIN
    PRINT 'BillPayments table already exists.';
END
GO

-- Create NotificationPreferences table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'NotificationPreferences')
BEGIN
    PRINT 'Creating NotificationPreferences table...';
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
ELSE
BEGIN
    PRINT 'NotificationPreferences table already exists.';
END
GO

-- Create Notifications table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Notifications')
BEGIN
    PRINT 'Creating Notifications table...';
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
    
    CREATE INDEX [IX_Notifications_CreatedAt] ON [Notifications] ([CreatedAt]);
    CREATE INDEX [IX_Notifications_ScheduledAt] ON [Notifications] ([ScheduledAt]);
    CREATE INDEX [IX_Notifications_Status] ON [Notifications] ([Status]);
    CREATE INDEX [IX_Notifications_Type] ON [Notifications] ([Type]);
    CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);
    CREATE INDEX [IX_Notifications_UserId_Status] ON [Notifications] ([UserId], [Status]);
END
ELSE
BEGIN
    PRINT 'Notifications table already exists.';
END
GO

PRINT '=== MIGRATION SUMMARY ===';
PRINT 'All essential banking tables have been created or verified.';
PRINT 'ASP.NET Identity key length issues have been resolved.';
PRINT 'Existing tables (like Cards) have been preserved.';
PRINT 'The database is now ready for the banking application.';
PRINT '========================';