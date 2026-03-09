IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AspNetRoles] (
    [Id] uniqueidentifier NOT NULL,
    [Description] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

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

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Accounts] (
    [Id] uniqueidentifier NOT NULL,
    [AccountNumber] nvarchar(450) NOT NULL,
    [AccountHolderName] nvarchar(max) NOT NULL,
    [Balance] decimal(18,2) NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
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

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [AspNetUserTokens] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Transactions] (
    [Id] uniqueidentifier NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [ProcessedAt] datetime2 NULL,
    [Status] int NOT NULL,
    [Type] int NOT NULL,
    [FromAccountId] uniqueidentifier NOT NULL,
    [ToAccountId] uniqueidentifier NOT NULL,
    [BatchJobId] uniqueidentifier NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Transactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Transactions_Accounts_FromAccountId] FOREIGN KEY ([FromAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Transactions_Accounts_ToAccountId] FOREIGN KEY ([ToAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Transactions_BatchJobs_BatchJobId] FOREIGN KEY ([BatchJobId]) REFERENCES [BatchJobs] ([Id])
);

CREATE UNIQUE INDEX [IX_Accounts_AccountNumber] ON [Accounts] ([AccountNumber]);

CREATE INDEX [IX_Accounts_UserId] ON [Accounts] ([UserId]);

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

CREATE INDEX [IX_Transactions_BatchJobId] ON [Transactions] ([BatchJobId]);

CREATE INDEX [IX_Transactions_FromAccountId] ON [Transactions] ([FromAccountId]);

CREATE INDEX [IX_Transactions_ToAccountId] ON [Transactions] ([ToAccountId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260306183019_InitialCreate', N'9.0.3');

ALTER TABLE [AspNetUsers] ADD [LastTwoFactorUsed] datetime2 NULL;

ALTER TABLE [AspNetUsers] ADD [TwoFactorBackupCodes] nvarchar(max) NULL;

ALTER TABLE [AspNetUsers] ADD [TwoFactorSecretKey] nvarchar(max) NULL;

ALTER TABLE [AspNetUsers] ADD [TwoFactorSetupDate] datetime2 NULL;

ALTER TABLE [AspNetUsers] ADD [TwoFactorStatus] int NOT NULL DEFAULT 0;

CREATE TABLE [AuditLogs] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NULL,
    [Action] nvarchar(100) NOT NULL,
    [EntityType] nvarchar(50) NOT NULL,
    [EntityId] nvarchar(50) NOT NULL,
    [OldValues] nvarchar(max) NULL,
    [NewValues] nvarchar(max) NULL,
    [IpAddress] nvarchar(45) NULL,
    [UserAgent] nvarchar(500) NULL,
    [EventType] int NOT NULL,
    [AdditionalData] nvarchar(max) NULL,
    [SessionId] nvarchar(max) NULL,
    [RequestId] nvarchar(max) NULL,
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

CREATE TABLE [TwoFactorTokens] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [Token] nvarchar(450) NOT NULL,
    [Method] int NOT NULL,
    [Destination] nvarchar(max) NOT NULL,
    [ExpiresAt] datetime2 NOT NULL,
    [IsUsed] bit NOT NULL,
    [UsedAt] datetime2 NULL,
    [IpAddress] nvarchar(max) NULL,
    [UserAgent] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_TwoFactorTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TwoFactorTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AuditLogs_Action] ON [AuditLogs] ([Action]);

CREATE INDEX [IX_AuditLogs_CreatedAt] ON [AuditLogs] ([CreatedAt]);

CREATE INDEX [IX_AuditLogs_EntityType_EntityId] ON [AuditLogs] ([EntityType], [EntityId]);

CREATE INDEX [IX_AuditLogs_EventType] ON [AuditLogs] ([EventType]);

CREATE INDEX [IX_AuditLogs_IpAddress] ON [AuditLogs] ([IpAddress]);

CREATE INDEX [IX_AuditLogs_UserId_CreatedAt] ON [AuditLogs] ([UserId], [CreatedAt]);

CREATE INDEX [IX_TwoFactorTokens_ExpiresAt] ON [TwoFactorTokens] ([ExpiresAt]);

CREATE INDEX [IX_TwoFactorTokens_UserId_Token_ExpiresAt] ON [TwoFactorTokens] ([UserId], [Token], [ExpiresAt]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260308055601_AddAuditLogsTable', N'9.0.3');

CREATE TABLE [AccountLockouts] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [FailedAttempts] int NOT NULL,
    [LockedUntil] datetime2 NULL,
    [LockoutReason] int NULL,
    [IpAddress] nvarchar(45) NULL,
    [UserAgent] nvarchar(500) NULL,
    [LastFailedAttempt] datetime2 NULL,
    [LastSuccessfulLogin] datetime2 NULL,
    [IsCurrentlyLocked] bit NOT NULL,
    [LockoutNotes] nvarchar(max) NULL,
    [LockedByUserId] uniqueidentifier NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_AccountLockouts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AccountLockouts_AspNetUsers_LockedByUserId] FOREIGN KEY ([LockedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_AccountLockouts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [IpWhitelists] (
    [Id] uniqueidentifier NOT NULL,
    [IpAddress] nvarchar(45) NOT NULL,
    [IpRange] nvarchar(50) NULL,
    [Type] int NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [IsActive] bit NOT NULL,
    [ExpiresAt] datetime2 NULL,
    [CreatedByUserId] uniqueidentifier NOT NULL,
    [ApprovedByUserId] uniqueidentifier NULL,
    [ApprovedAt] datetime2 NULL,
    [ApprovalNotes] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_IpWhitelists] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_IpWhitelists_AspNetUsers_ApprovedByUserId] FOREIGN KEY ([ApprovedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_IpWhitelists_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PasswordHistories] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [PasswordHash] nvarchar(256) NOT NULL,
    [PasswordSetAt] datetime2 NOT NULL,
    [PasswordSalt] nvarchar(128) NULL,
    [IsCurrentPassword] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_PasswordHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PasswordHistories_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PasswordPolicies] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(100) NOT NULL,
    [ComplexityLevel] int NOT NULL,
    [MinimumLength] int NOT NULL,
    [MaximumLength] int NOT NULL,
    [RequireUppercase] bit NOT NULL,
    [RequireLowercase] bit NOT NULL,
    [RequireDigits] bit NOT NULL,
    [RequireSpecialCharacters] bit NOT NULL,
    [AllowedSpecialCharacters] nvarchar(100) NOT NULL,
    [MinimumUniqueCharacters] int NOT NULL,
    [PreventCommonPasswords] bit NOT NULL,
    [PreventUserInfoInPassword] bit NOT NULL,
    [PasswordHistoryCount] int NOT NULL,
    [MaxPasswordAge] time NOT NULL,
    [MinPasswordAge] time NULL,
    [MaxFailedAttempts] int NOT NULL,
    [LockoutDuration] time NOT NULL,
    [IsDefault] bit NOT NULL,
    [IsActive] bit NOT NULL,
    [Description] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_PasswordPolicies] PRIMARY KEY ([Id])
);

CREATE TABLE [Sessions] (
    [Id] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [SessionToken] nvarchar(128) NOT NULL,
    [RefreshToken] nvarchar(128) NULL,
    [ExpiresAt] datetime2 NOT NULL,
    [RefreshTokenExpiresAt] datetime2 NULL,
    [Status] int NOT NULL,
    [IpAddress] nvarchar(45) NOT NULL,
    [UserAgent] nvarchar(500) NOT NULL,
    [DeviceFingerprint] nvarchar(max) NULL,
    [LastActivityAt] datetime2 NOT NULL,
    [TerminatedAt] datetime2 NULL,
    [TerminationReason] nvarchar(max) NULL,
    [IsAdminSession] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Sessions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Sessions_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_AccountLockouts_IsCurrentlyLocked] ON [AccountLockouts] ([IsCurrentlyLocked]);

CREATE INDEX [IX_AccountLockouts_LockedByUserId] ON [AccountLockouts] ([LockedByUserId]);

CREATE INDEX [IX_AccountLockouts_LockedUntil] ON [AccountLockouts] ([LockedUntil]);

CREATE UNIQUE INDEX [IX_AccountLockouts_UserId] ON [AccountLockouts] ([UserId]);

CREATE INDEX [IX_IpWhitelists_ApprovedByUserId] ON [IpWhitelists] ([ApprovedByUserId]);

CREATE INDEX [IX_IpWhitelists_CreatedByUserId] ON [IpWhitelists] ([CreatedByUserId]);

CREATE INDEX [IX_IpWhitelists_ExpiresAt] ON [IpWhitelists] ([ExpiresAt]);

CREATE INDEX [IX_IpWhitelists_IpAddress_Type] ON [IpWhitelists] ([IpAddress], [Type]);

CREATE INDEX [IX_IpWhitelists_Type_IsActive] ON [IpWhitelists] ([Type], [IsActive]);

CREATE INDEX [IX_PasswordHistories_UserId_IsCurrentPassword] ON [PasswordHistories] ([UserId], [IsCurrentPassword]);

CREATE INDEX [IX_PasswordHistories_UserId_PasswordSetAt] ON [PasswordHistories] ([UserId], [PasswordSetAt]);

CREATE UNIQUE INDEX [IX_PasswordPolicies_ComplexityLevel] ON [PasswordPolicies] ([ComplexityLevel]);

CREATE INDEX [IX_PasswordPolicies_IsDefault] ON [PasswordPolicies] ([IsDefault]);

CREATE UNIQUE INDEX [IX_PasswordPolicies_Name] ON [PasswordPolicies] ([Name]);

CREATE INDEX [IX_Sessions_ExpiresAt] ON [Sessions] ([ExpiresAt]);

CREATE INDEX [IX_Sessions_IpAddress] ON [Sessions] ([IpAddress]);

CREATE INDEX [IX_Sessions_RefreshToken] ON [Sessions] ([RefreshToken]);

CREATE UNIQUE INDEX [IX_Sessions_SessionToken] ON [Sessions] ([SessionToken]);

CREATE INDEX [IX_Sessions_UserId_Status] ON [Sessions] ([UserId], [Status]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260308093217_AddSessionManagementAndSecurityEntities', N'9.0.3');

ALTER TABLE [Transactions] ADD [Reference] nvarchar(max) NULL;

ALTER TABLE [Accounts] ADD [ClosedDate] datetime2 NULL;

ALTER TABLE [Accounts] ADD [ClosureReason] nvarchar(max) NULL;

ALTER TABLE [Accounts] ADD [CompoundingFrequency] int NOT NULL DEFAULT 0;

ALTER TABLE [Accounts] ADD [DormancyDate] datetime2 NULL;

ALTER TABLE [Accounts] ADD [DormancyPeriodDays] int NOT NULL DEFAULT 0;

ALTER TABLE [Accounts] ADD [FeeWaiverEligible] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Accounts] ADD [HasHolds] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Accounts] ADD [HasRestrictions] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Accounts] ADD [InterestRate] decimal(18,2) NOT NULL DEFAULT 0.0;

ALTER TABLE [Accounts] ADD [IsJointAccount] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Accounts] ADD [LastActivityDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Accounts] ADD [LastFeeCalculationDate] datetime2 NULL;

ALTER TABLE [Accounts] ADD [LastInterestCalculationDate] datetime2 NULL;

ALTER TABLE [Accounts] ADD [MinimumBalance] decimal(18,2) NOT NULL DEFAULT 0.0;

ALTER TABLE [Accounts] ADD [MinimumSignaturesRequired] int NOT NULL DEFAULT 0;

ALTER TABLE [Accounts] ADD [MonthlyMaintenanceFee] decimal(18,2) NOT NULL DEFAULT 0.0;

ALTER TABLE [Accounts] ADD [MultipleSignatureThreshold] decimal(18,2) NULL;

ALTER TABLE [Accounts] ADD [OpenedDate] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

ALTER TABLE [Accounts] ADD [RequiresMultipleSignatures] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Accounts] ADD [Status] int NOT NULL DEFAULT 0;

ALTER TABLE [Accounts] ADD [Type] int NOT NULL DEFAULT 0;

CREATE TABLE [AccountFees] (
    [Id] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [Type] int NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [CalculatedDate] datetime2 NOT NULL,
    [AppliedDate] datetime2 NULL,
    [IsWaived] bit NOT NULL,
    [WaiverReason] nvarchar(max) NULL,
    [WaivedByUserId] uniqueidentifier NULL,
    [Frequency] int NOT NULL,
    [NextCalculationDate] datetime2 NULL,
    [TransactionId] uniqueidentifier NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_AccountFees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AccountFees_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AccountFees_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [AccountHolds] (
    [Id] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [Type] int NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NULL,
    [PlacedDate] datetime2 NOT NULL,
    [ExpiryDate] datetime2 NULL,
    [ReleasedDate] datetime2 NULL,
    [PlacedByUserId] uniqueidentifier NOT NULL,
    [ReleasedByUserId] uniqueidentifier NULL,
    [ReferenceNumber] nvarchar(max) NULL,
    [Notes] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_AccountHolds] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AccountHolds_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AccountHolds_AspNetUsers_PlacedByUserId] FOREIGN KEY ([PlacedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AccountHolds_AspNetUsers_ReleasedByUserId] FOREIGN KEY ([ReleasedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [AccountRestrictions] (
    [Id] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [Type] int NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [AppliedDate] datetime2 NOT NULL,
    [ExpiryDate] datetime2 NULL,
    [RemovedDate] datetime2 NULL,
    [AppliedByUserId] uniqueidentifier NOT NULL,
    [RemovedByUserId] uniqueidentifier NULL,
    [DailyLimit] decimal(18,2) NULL,
    [MonthlyLimit] decimal(18,2) NULL,
    [TransactionCountLimit] int NULL,
    [Notes] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_AccountRestrictions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AccountRestrictions_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AccountRestrictions_AspNetUsers_AppliedByUserId] FOREIGN KEY ([AppliedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AccountRestrictions_AspNetUsers_RemovedByUserId] FOREIGN KEY ([RemovedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [AccountStatusHistories] (
    [Id] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [FromStatus] int NOT NULL,
    [ToStatus] int NOT NULL,
    [ChangedDate] datetime2 NOT NULL,
    [Reason] nvarchar(max) NOT NULL,
    [Notes] nvarchar(max) NULL,
    [ChangedByUserId] uniqueidentifier NOT NULL,
    [SystemReference] nvarchar(max) NULL,
    [IsSystemGenerated] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_AccountStatusHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AccountStatusHistories_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AccountStatusHistories_AspNetUsers_ChangedByUserId] FOREIGN KEY ([ChangedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [FeeSchedules] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Type] int NOT NULL,
    [AccountType] int NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Frequency] int NOT NULL,
    [IsActive] bit NOT NULL,
    [EffectiveDate] datetime2 NOT NULL,
    [ExpiryDate] datetime2 NULL,
    [MinimumBalanceThreshold] decimal(18,2) NULL,
    [MaximumBalanceThreshold] decimal(18,2) NULL,
    [DormancyDaysThreshold] int NULL,
    [TransactionCountThreshold] int NULL,
    [WaiverMinimumBalance] decimal(18,2) NULL,
    [WaiverForPremiumAccounts] bit NOT NULL,
    [WaiverConditions] nvarchar(max) NULL,
    [CreatedByUserId] uniqueidentifier NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_FeeSchedules] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FeeSchedules_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [JointAccountHolders] (
    [Id] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [UserId] uniqueidentifier NOT NULL,
    [Role] int NOT NULL,
    [AddedDate] datetime2 NOT NULL,
    [RemovedDate] datetime2 NULL,
    [AddedByUserId] uniqueidentifier NOT NULL,
    [RemovedByUserId] uniqueidentifier NULL,
    [RequiresSignature] bit NOT NULL,
    [TransactionLimit] decimal(18,2) NULL,
    [DailyLimit] decimal(18,2) NULL,
    [Notes] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_JointAccountHolders] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_JointAccountHolders_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_JointAccountHolders_AspNetUsers_AddedByUserId] FOREIGN KEY ([AddedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_JointAccountHolders_AspNetUsers_RemovedByUserId] FOREIGN KEY ([RemovedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_JointAccountHolders_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PaymentTemplates] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [FromAccountId] uniqueidentifier NOT NULL,
    [ToAccountId] uniqueidentifier NULL,
    [BeneficiaryName] nvarchar(200) NULL,
    [BeneficiaryAccountNumber] nvarchar(50) NULL,
    [BeneficiaryBankCode] nvarchar(20) NULL,
    [Amount] decimal(18,2) NULL,
    [Type] int NOT NULL,
    [Reference] nvarchar(100) NULL,
    [Notes] nvarchar(1000) NULL,
    [IsActive] bit NOT NULL,
    [UsageCount] int NOT NULL,
    [LastUsedDate] datetime2 NULL,
    [CreatedByUserId] uniqueidentifier NOT NULL,
    [Category] int NOT NULL,
    [Tags] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_PaymentTemplates] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PaymentTemplates_Accounts_FromAccountId] FOREIGN KEY ([FromAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PaymentTemplates_Accounts_ToAccountId] FOREIGN KEY ([ToAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_PaymentTemplates_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [RecurringPayments] (
    [Id] uniqueidentifier NOT NULL,
    [FromAccountId] uniqueidentifier NOT NULL,
    [ToAccountId] uniqueidentifier NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Reference] nvarchar(max) NULL,
    [Frequency] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [EndDate] datetime2 NULL,
    [MaxOccurrences] int NULL,
    [NextExecutionDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [ExecutionCount] int NOT NULL,
    [LastExecutionDate] datetime2 NULL,
    [PausedDate] datetime2 NULL,
    [PauseReason] nvarchar(max) NULL,
    [FailureCount] int NOT NULL,
    [MaxRetries] int NOT NULL,
    [LastFailureReason] nvarchar(max) NULL,
    [CreatedByUserId] uniqueidentifier NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_RecurringPayments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RecurringPayments_Accounts_FromAccountId] FOREIGN KEY ([FromAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RecurringPayments_Accounts_ToAccountId] FOREIGN KEY ([ToAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RecurringPayments_AspNetUsers_CreatedByUserId] FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [RecurringPaymentExecutions] (
    [Id] uniqueidentifier NOT NULL,
    [RecurringPaymentId] uniqueidentifier NOT NULL,
    [ScheduledDate] datetime2 NOT NULL,
    [ExecutedDate] datetime2 NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Status] int NOT NULL,
    [TransactionId] uniqueidentifier NULL,
    [FailureReason] nvarchar(max) NULL,
    [RetryCount] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_RecurringPaymentExecutions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RecurringPaymentExecutions_RecurringPayments_RecurringPaymentId] FOREIGN KEY ([RecurringPaymentId]) REFERENCES [RecurringPayments] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_RecurringPaymentExecutions_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE SET NULL
);

CREATE INDEX [IX_AccountFees_AccountId] ON [AccountFees] ([AccountId]);

CREATE INDEX [IX_AccountFees_TransactionId] ON [AccountFees] ([TransactionId]);

CREATE INDEX [IX_AccountHolds_AccountId] ON [AccountHolds] ([AccountId]);

CREATE INDEX [IX_AccountHolds_PlacedByUserId] ON [AccountHolds] ([PlacedByUserId]);

CREATE INDEX [IX_AccountHolds_ReleasedByUserId] ON [AccountHolds] ([ReleasedByUserId]);

CREATE INDEX [IX_AccountRestrictions_AccountId] ON [AccountRestrictions] ([AccountId]);

CREATE INDEX [IX_AccountRestrictions_AppliedByUserId] ON [AccountRestrictions] ([AppliedByUserId]);

CREATE INDEX [IX_AccountRestrictions_RemovedByUserId] ON [AccountRestrictions] ([RemovedByUserId]);

CREATE INDEX [IX_AccountStatusHistories_AccountId] ON [AccountStatusHistories] ([AccountId]);

CREATE INDEX [IX_AccountStatusHistories_ChangedByUserId] ON [AccountStatusHistories] ([ChangedByUserId]);

CREATE INDEX [IX_FeeSchedules_CreatedByUserId] ON [FeeSchedules] ([CreatedByUserId]);

CREATE INDEX [IX_JointAccountHolders_AccountId_UserId] ON [JointAccountHolders] ([AccountId], [UserId]);

CREATE INDEX [IX_JointAccountHolders_AddedByUserId] ON [JointAccountHolders] ([AddedByUserId]);

CREATE INDEX [IX_JointAccountHolders_RemovedByUserId] ON [JointAccountHolders] ([RemovedByUserId]);

CREATE INDEX [IX_JointAccountHolders_UserId] ON [JointAccountHolders] ([UserId]);

CREATE INDEX [IX_PaymentTemplates_CreatedByUserId_Category] ON [PaymentTemplates] ([CreatedByUserId], [Category]);

CREATE INDEX [IX_PaymentTemplates_CreatedByUserId_IsActive] ON [PaymentTemplates] ([CreatedByUserId], [IsActive]);

CREATE INDEX [IX_PaymentTemplates_CreatedByUserId_LastUsedDate] ON [PaymentTemplates] ([CreatedByUserId], [LastUsedDate]);

CREATE INDEX [IX_PaymentTemplates_CreatedByUserId_UsageCount] ON [PaymentTemplates] ([CreatedByUserId], [UsageCount]);

CREATE INDEX [IX_PaymentTemplates_FromAccountId] ON [PaymentTemplates] ([FromAccountId]);

CREATE INDEX [IX_PaymentTemplates_ToAccountId] ON [PaymentTemplates] ([ToAccountId]);

CREATE INDEX [IX_RecurringPaymentExecutions_RecurringPaymentId_ScheduledDate] ON [RecurringPaymentExecutions] ([RecurringPaymentId], [ScheduledDate]);

CREATE INDEX [IX_RecurringPaymentExecutions_TransactionId] ON [RecurringPaymentExecutions] ([TransactionId]);

CREATE INDEX [IX_RecurringPayments_CreatedByUserId] ON [RecurringPayments] ([CreatedByUserId]);

CREATE INDEX [IX_RecurringPayments_FromAccountId] ON [RecurringPayments] ([FromAccountId]);

CREATE INDEX [IX_RecurringPayments_Status_NextExecutionDate] ON [RecurringPayments] ([Status], [NextExecutionDate]);

CREATE INDEX [IX_RecurringPayments_ToAccountId] ON [RecurringPayments] ([ToAccountId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260308133813_AddRecurringPaymentsAndTemplates', N'9.0.3');

ALTER TABLE [Transactions] ADD [BeneficiaryId] uniqueidentifier NULL;

CREATE TABLE [Beneficiaries] (
    [Id] uniqueidentifier NOT NULL,
    [CustomerId] uniqueidentifier NOT NULL,
    [Name] nvarchar(200) NOT NULL,
    [AccountNumber] nvarchar(50) NOT NULL,
    [AccountName] nvarchar(200) NULL,
    [BankName] nvarchar(200) NOT NULL,
    [BankCode] nvarchar(20) NOT NULL,
    [SwiftCode] nvarchar(11) NULL,
    [IbanNumber] nvarchar(34) NULL,
    [RoutingNumber] nvarchar(20) NULL,
    [Type] int NOT NULL,
    [Category] int NOT NULL,
    [IsVerified] bit NOT NULL,
    [VerifiedDate] datetime2 NULL,
    [VerifiedByUserId] uniqueidentifier NULL,
    [Status] int NOT NULL,
    [DailyTransferLimit] decimal(18,2) NULL,
    [MonthlyTransferLimit] decimal(18,2) NULL,
    [SingleTransferLimit] decimal(18,2) NULL,
    [IsActive] bit NOT NULL,
    [Notes] nvarchar(1000) NULL,
    [Reference] nvarchar(100) NULL,
    [LastTransferDate] datetime2 NULL,
    [TransferCount] int NOT NULL,
    [TotalTransferAmount] decimal(18,2) NOT NULL,
    [ArchivedDate] datetime2 NULL,
    [ArchiveReason] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Beneficiaries] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Beneficiaries_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Beneficiaries_AspNetUsers_VerifiedByUserId] FOREIGN KEY ([VerifiedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL
);

CREATE INDEX [IX_Transactions_BeneficiaryId] ON [Transactions] ([BeneficiaryId]);

CREATE INDEX [IX_Beneficiaries_CustomerId_AccountNumber_BankCode] ON [Beneficiaries] ([CustomerId], [AccountNumber], [BankCode]);

CREATE INDEX [IX_Beneficiaries_CustomerId_Category] ON [Beneficiaries] ([CustomerId], [Category]);

CREATE INDEX [IX_Beneficiaries_CustomerId_IsActive] ON [Beneficiaries] ([CustomerId], [IsActive]);

CREATE INDEX [IX_Beneficiaries_CustomerId_Type] ON [Beneficiaries] ([CustomerId], [Type]);

CREATE INDEX [IX_Beneficiaries_IsVerified] ON [Beneficiaries] ([IsVerified]);

CREATE INDEX [IX_Beneficiaries_LastTransferDate] ON [Beneficiaries] ([LastTransferDate]);

CREATE INDEX [IX_Beneficiaries_Status] ON [Beneficiaries] ([Status]);

CREATE INDEX [IX_Beneficiaries_VerifiedByUserId] ON [Beneficiaries] ([VerifiedByUserId]);

ALTER TABLE [Transactions] ADD CONSTRAINT [FK_Transactions_Beneficiaries_BeneficiaryId] FOREIGN KEY ([BeneficiaryId]) REFERENCES [Beneficiaries] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260308153419_AddBeneficiaryEntity', N'9.0.3');

CREATE TABLE [AccountStatements] (
    [Id] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [StatementDate] datetime2 NOT NULL,
    [PeriodStartDate] datetime2 NOT NULL,
    [PeriodEndDate] datetime2 NOT NULL,
    [StatementNumber] nvarchar(50) NOT NULL,
    [StatementSequence] int NOT NULL,
    [OpeningBalance] decimal(18,2) NOT NULL,
    [ClosingBalance] decimal(18,2) NOT NULL,
    [AverageBalance] decimal(18,2) NOT NULL,
    [MinimumBalance] decimal(18,2) NOT NULL,
    [MaximumBalance] decimal(18,2) NOT NULL,
    [TotalTransactions] int NOT NULL,
    [DebitTransactions] int NOT NULL,
    [CreditTransactions] int NOT NULL,
    [TotalDebits] decimal(18,2) NOT NULL,
    [TotalCredits] decimal(18,2) NOT NULL,
    [TotalFees] decimal(18,2) NOT NULL,
    [InterestEarned] decimal(18,2) NOT NULL,
    [InterestCharged] decimal(18,2) NOT NULL,
    [Status] nvarchar(20) NOT NULL,
    [Format] nvarchar(20) NOT NULL,
    [DeliveryMethod] nvarchar(20) NOT NULL,
    [FilePath] nvarchar(500) NULL,
    [FileName] nvarchar(255) NULL,
    [FileSizeBytes] bigint NULL,
    [FileHash] nvarchar(100) NULL,
    [DeliveredDate] datetime2 NULL,
    [DeliveryReference] nvarchar(100) NULL,
    [IsDelivered] bit NOT NULL,
    [RequestedByUserId] uniqueidentifier NULL,
    [RequestedDate] datetime2 NULL,
    [RequestReason] nvarchar(500) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_AccountStatements] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AccountStatements_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AccountStatements_AspNetUsers_RequestedByUserId] FOREIGN KEY ([RequestedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [Loans] (
    [Id] uniqueidentifier NOT NULL,
    [LoanNumber] nvarchar(20) NOT NULL,
    [CustomerId] uniqueidentifier NOT NULL,
    [Type] int NOT NULL,
    [PrincipalAmount] decimal(18,2) NOT NULL,
    [InterestRate] decimal(5,4) NOT NULL,
    [TermInMonths] int NOT NULL,
    [CalculationMethod] int NOT NULL,
    [Status] int NOT NULL,
    [ApplicationDate] datetime2 NOT NULL,
    [ApprovalDate] datetime2 NULL,
    [DisbursementDate] datetime2 NULL,
    [MaturityDate] datetime2 NULL,
    [OutstandingBalance] decimal(18,2) NOT NULL,
    [TotalInterestPaid] decimal(18,2) NOT NULL,
    [TotalPrincipalPaid] decimal(18,2) NOT NULL,
    [MonthlyPaymentAmount] decimal(18,2) NOT NULL,
    [CreditScore] int NULL,
    [CreditScoreRange] int NULL,
    [CreditScoringDate] datetime2 NULL,
    [DaysOverdue] int NOT NULL,
    [LastPaymentDate] datetime2 NULL,
    [NextPaymentDueDate] datetime2 NULL,
    [Purpose] nvarchar(500) NOT NULL,
    [RequestedAmount] decimal(18,2) NOT NULL,
    [RejectionReason] nvarchar(1000) NULL,
    [ApprovedBy] uniqueidentifier NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Loans] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Loans_AspNetUsers_ApprovedBy] FOREIGN KEY ([ApprovedBy]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Loans_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [StatementTransactions] (
    [Id] uniqueidentifier NOT NULL,
    [StatementId] uniqueidentifier NOT NULL,
    [TransactionId] uniqueidentifier NOT NULL,
    [TransactionDate] datetime2 NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [Reference] nvarchar(100) NULL,
    [Amount] decimal(18,2) NOT NULL,
    [RunningBalance] decimal(18,2) NOT NULL,
    [Type] nvarchar(50) NOT NULL,
    [Status] nvarchar(20) NOT NULL,
    [Category] nvarchar(100) NULL,
    [Memo] nvarchar(500) NULL,
    [IsReconciled] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_StatementTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_StatementTransactions_AccountStatements_StatementId] FOREIGN KEY ([StatementId]) REFERENCES [AccountStatements] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_StatementTransactions_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [LoanDocuments] (
    [Id] uniqueidentifier NOT NULL,
    [LoanId] uniqueidentifier NOT NULL,
    [DocumentType] int NOT NULL,
    [DocumentName] nvarchar(255) NOT NULL,
    [FilePath] nvarchar(500) NOT NULL,
    [ContentType] nvarchar(100) NOT NULL,
    [FileSize] bigint NOT NULL,
    [IsRequired] bit NOT NULL,
    [IsVerified] bit NOT NULL,
    [VerifiedDate] datetime2 NULL,
    [VerifiedBy] uniqueidentifier NULL,
    [Description] nvarchar(1000) NULL,
    [VerificationNotes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_LoanDocuments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_LoanDocuments_AspNetUsers_VerifiedBy] FOREIGN KEY ([VerifiedBy]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_LoanDocuments_Loans_LoanId] FOREIGN KEY ([LoanId]) REFERENCES [Loans] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [LoanPayments] (
    [Id] uniqueidentifier NOT NULL,
    [LoanId] uniqueidentifier NOT NULL,
    [PaymentAmount] decimal(18,2) NOT NULL,
    [PrincipalAmount] decimal(18,2) NOT NULL,
    [InterestAmount] decimal(18,2) NOT NULL,
    [LateFeeAmount] decimal(18,2) NOT NULL,
    [OutstandingBalanceAfterPayment] decimal(18,2) NOT NULL,
    [PaymentDate] datetime2 NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [TransactionReference] nvarchar(100) NULL,
    [PaymentMethod] nvarchar(50) NULL,
    [Notes] nvarchar(1000) NULL,
    [ProcessedDate] datetime2 NULL,
    [ProcessedBy] uniqueidentifier NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_LoanPayments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_LoanPayments_AspNetUsers_ProcessedBy] FOREIGN KEY ([ProcessedBy]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_LoanPayments_Loans_LoanId] FOREIGN KEY ([LoanId]) REFERENCES [Loans] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [LoanStatusHistories] (
    [Id] uniqueidentifier NOT NULL,
    [LoanId] uniqueidentifier NOT NULL,
    [FromStatus] int NOT NULL,
    [ToStatus] int NOT NULL,
    [StatusChangeDate] datetime2 NOT NULL,
    [ChangedBy] uniqueidentifier NULL,
    [Reason] nvarchar(500) NULL,
    [Notes] nvarchar(1000) NULL,
    [SystemReference] nvarchar(100) NULL,
    [IsSystemGenerated] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_LoanStatusHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_LoanStatusHistories_AspNetUsers_ChangedBy] FOREIGN KEY ([ChangedBy]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_LoanStatusHistories_Loans_LoanId] FOREIGN KEY ([LoanId]) REFERENCES [Loans] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_AccountStatements_AccountId] ON [AccountStatements] ([AccountId]);

CREATE INDEX [IX_AccountStatements_AccountId_Period] ON [AccountStatements] ([AccountId], [PeriodStartDate], [PeriodEndDate]);

CREATE INDEX [IX_AccountStatements_IsDelivered] ON [AccountStatements] ([IsDelivered]);

CREATE INDEX [IX_AccountStatements_RequestedByUserId] ON [AccountStatements] ([RequestedByUserId]);

CREATE INDEX [IX_AccountStatements_StatementDate] ON [AccountStatements] ([StatementDate]);

CREATE UNIQUE INDEX [IX_AccountStatements_StatementNumber] ON [AccountStatements] ([StatementNumber]);

CREATE INDEX [IX_AccountStatements_Status] ON [AccountStatements] ([Status]);

CREATE INDEX [IX_LoanDocuments_DocumentType] ON [LoanDocuments] ([DocumentType]);

CREATE INDEX [IX_LoanDocuments_IsVerified] ON [LoanDocuments] ([IsVerified]);

CREATE INDEX [IX_LoanDocuments_LoanId] ON [LoanDocuments] ([LoanId]);

CREATE INDEX [IX_LoanDocuments_VerifiedBy] ON [LoanDocuments] ([VerifiedBy]);

CREATE INDEX [IX_LoanPayments_DueDate] ON [LoanPayments] ([DueDate]);

CREATE INDEX [IX_LoanPayments_LoanId] ON [LoanPayments] ([LoanId]);

CREATE INDEX [IX_LoanPayments_PaymentDate] ON [LoanPayments] ([PaymentDate]);

CREATE INDEX [IX_LoanPayments_ProcessedBy] ON [LoanPayments] ([ProcessedBy]);

CREATE INDEX [IX_LoanPayments_Status] ON [LoanPayments] ([Status]);

CREATE INDEX [IX_Loans_ApplicationDate] ON [Loans] ([ApplicationDate]);

CREATE INDEX [IX_Loans_ApprovedBy] ON [Loans] ([ApprovedBy]);

CREATE INDEX [IX_Loans_CustomerId] ON [Loans] ([CustomerId]);

CREATE UNIQUE INDEX [IX_Loans_LoanNumber] ON [Loans] ([LoanNumber]);

CREATE INDEX [IX_Loans_NextPaymentDueDate] ON [Loans] ([NextPaymentDueDate]);

CREATE INDEX [IX_Loans_Status] ON [Loans] ([Status]);

CREATE INDEX [IX_Loans_Type] ON [Loans] ([Type]);

CREATE INDEX [IX_LoanStatusHistories_ChangedBy] ON [LoanStatusHistories] ([ChangedBy]);

CREATE INDEX [IX_LoanStatusHistories_FromStatus] ON [LoanStatusHistories] ([FromStatus]);

CREATE INDEX [IX_LoanStatusHistories_LoanId] ON [LoanStatusHistories] ([LoanId]);

CREATE INDEX [IX_LoanStatusHistories_StatusChangeDate] ON [LoanStatusHistories] ([StatusChangeDate]);

CREATE INDEX [IX_LoanStatusHistories_ToStatus] ON [LoanStatusHistories] ([ToStatus]);

CREATE INDEX [IX_StatementTransactions_Category] ON [StatementTransactions] ([Category]);

CREATE INDEX [IX_StatementTransactions_StatementId] ON [StatementTransactions] ([StatementId]);

CREATE INDEX [IX_StatementTransactions_TransactionDate] ON [StatementTransactions] ([TransactionDate]);

CREATE INDEX [IX_StatementTransactions_TransactionId] ON [StatementTransactions] ([TransactionId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260308192742_AddLoanEntities', N'9.0.3');

CREATE TABLE [Cards] (
    [Id] uniqueidentifier NOT NULL,
    [CardNumber] nvarchar(19) NOT NULL,
    [MaskedCardNumber] nvarchar(19) NOT NULL,
    [CustomerId] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [Type] int NOT NULL,
    [Status] int NOT NULL,
    [ExpiryDate] date NOT NULL,
    [IssueDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [ActivationDate] datetime2 NULL,
    [ActivationChannel] int NULL,
    [SecurityCode] nvarchar(255) NOT NULL,
    [DailyLimit] decimal(18,2) NOT NULL DEFAULT 5000.0,
    [MonthlyLimit] decimal(18,2) NOT NULL DEFAULT 50000.0,
    [AtmDailyLimit] decimal(18,2) NOT NULL DEFAULT 2000.0,
    [ContactlessEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [OnlineTransactionsEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [InternationalTransactionsEnabled] bit NOT NULL DEFAULT CAST(0 AS bit),
    [PinHash] nvarchar(255) NULL,
    [PinSetDate] datetime2 NULL,
    [FailedPinAttempts] int NOT NULL DEFAULT 0,
    [LastBlockedDate] datetime2 NULL,
    [LastBlockReason] int NULL,
    [BlockedMerchantCategories] nvarchar(1000) NULL,
    [CardName] nvarchar(50) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Cards] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cards_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Cards_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);
DECLARE @defaultSchema AS sysname;
SET @defaultSchema = SCHEMA_NAME();
DECLARE @description AS sql_variant;
SET @description = N'Encrypted card number';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Cards', 'COLUMN', N'CardNumber';
SET @description = N'Masked card number for display';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Cards', 'COLUMN', N'MaskedCardNumber';
SET @description = N'Encrypted security code';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Cards', 'COLUMN', N'SecurityCode';
SET @description = N'JSON array of blocked merchant categories';
EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', @defaultSchema, 'TABLE', N'Cards', 'COLUMN', N'BlockedMerchantCategories';

CREATE TABLE [CardStatusHistories] (
    [Id] uniqueidentifier NOT NULL,
    [CardId] uniqueidentifier NOT NULL,
    [PreviousStatus] int NOT NULL,
    [NewStatus] int NOT NULL,
    [Reason] nvarchar(200) NULL,
    [ChangedBy] uniqueidentifier NULL,
    [ChangeDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(500) NULL,
    [Channel] nvarchar(50) NULL,
    [IpAddress] nvarchar(45) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_CardStatusHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CardStatusHistories_AspNetUsers_ChangedBy] FOREIGN KEY ([ChangedBy]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_CardStatusHistories_Cards_CardId] FOREIGN KEY ([CardId]) REFERENCES [Cards] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [CardTransactions] (
    [Id] uniqueidentifier NOT NULL,
    [CardId] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [TransactionId] uniqueidentifier NULL,
    [NetworkTransactionId] nvarchar(50) NOT NULL,
    [AuthorizationCode] nvarchar(20) NULL,
    [Amount] decimal(18,2) NOT NULL,
    [CurrencyCode] nvarchar(3) NOT NULL DEFAULT N'USD',
    [OriginalAmount] decimal(18,2) NULL,
    [OriginalCurrencyCode] nvarchar(3) NULL,
    [ExchangeRate] decimal(10,6) NULL,
    [Type] int NOT NULL,
    [Status] int NOT NULL,
    [TransactionDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [AuthorizationDate] datetime2 NULL,
    [SettlementDate] datetime2 NULL,
    [MerchantName] nvarchar(100) NULL,
    [MerchantId] nvarchar(50) NULL,
    [MerchantCategory] int NULL,
    [MerchantCity] nvarchar(50) NULL,
    [MerchantCountryCode] nvarchar(2) NULL,
    [TerminalId] nvarchar(20) NULL,
    [IsContactless] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsOnline] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsInternational] bit NOT NULL DEFAULT CAST(0 AS bit),
    [PinUsed] bit NOT NULL DEFAULT CAST(0 AS bit),
    [Description] nvarchar(200) NULL,
    [DeclineReason] nvarchar(100) NULL,
    [FraudScore] decimal(5,4) NULL,
    [IsFraudulent] bit NOT NULL DEFAULT CAST(0 AS bit),
    [Fee] decimal(18,2) NOT NULL DEFAULT 0.0,
    [FeeDescription] nvarchar(100) NULL,
    [BalanceAfterTransaction] decimal(18,2) NULL,
    [Metadata] nvarchar(2000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_CardTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CardTransactions_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CardTransactions_Cards_CardId] FOREIGN KEY ([CardId]) REFERENCES [Cards] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CardTransactions_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE SET NULL
);
DECLARE @defaultSchema1 AS sysname;
SET @defaultSchema1 = SCHEMA_NAME();
DECLARE @description1 AS sql_variant;
SET @description1 = N'Additional transaction metadata in JSON format';
EXEC sp_addextendedproperty 'MS_Description', @description1, 'SCHEMA', @defaultSchema1, 'TABLE', N'CardTransactions', 'COLUMN', N'Metadata';

CREATE INDEX [IX_Cards_AccountId] ON [Cards] ([AccountId]);

CREATE UNIQUE INDEX [IX_Cards_CardNumber] ON [Cards] ([CardNumber]);

CREATE INDEX [IX_Cards_CustomerId] ON [Cards] ([CustomerId]);

CREATE INDEX [IX_Cards_CustomerId_Status] ON [Cards] ([CustomerId], [Status]);

CREATE INDEX [IX_Cards_ExpiryDate] ON [Cards] ([ExpiryDate]);

CREATE INDEX [IX_Cards_Status] ON [Cards] ([Status]);

CREATE INDEX [IX_CardStatusHistories_CardId] ON [CardStatusHistories] ([CardId]);

CREATE INDEX [IX_CardStatusHistories_CardId_ChangeDate] ON [CardStatusHistories] ([CardId], [ChangeDate]);

CREATE INDEX [IX_CardStatusHistories_ChangeDate] ON [CardStatusHistories] ([ChangeDate]);

CREATE INDEX [IX_CardStatusHistories_ChangedBy] ON [CardStatusHistories] ([ChangedBy]);

CREATE INDEX [IX_CardTransactions_AccountId] ON [CardTransactions] ([AccountId]);

CREATE INDEX [IX_CardTransactions_CardId] ON [CardTransactions] ([CardId]);

CREATE INDEX [IX_CardTransactions_CardId_Status] ON [CardTransactions] ([CardId], [Status]);

CREATE INDEX [IX_CardTransactions_CardId_TransactionDate] ON [CardTransactions] ([CardId], [TransactionDate]);

CREATE INDEX [IX_CardTransactions_FraudScore] ON [CardTransactions] ([FraudScore]) WHERE [FraudScore] IS NOT NULL;

CREATE INDEX [IX_CardTransactions_MerchantId] ON [CardTransactions] ([MerchantId]);

CREATE UNIQUE INDEX [IX_CardTransactions_NetworkTransactionId] ON [CardTransactions] ([NetworkTransactionId]);

CREATE INDEX [IX_CardTransactions_SettlementDate] ON [CardTransactions] ([SettlementDate]);

CREATE INDEX [IX_CardTransactions_Status] ON [CardTransactions] ([Status]);

CREATE INDEX [IX_CardTransactions_TransactionDate] ON [CardTransactions] ([TransactionDate]);

CREATE INDEX [IX_CardTransactions_TransactionId] ON [CardTransactions] ([TransactionId]);

CREATE INDEX [IX_CardTransactions_Type] ON [CardTransactions] ([Type]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260309051618_AddNotificationEntities', N'9.0.3');

CREATE TABLE [Cards] (
    [Id] uniqueidentifier NOT NULL,
    [CardNumber] nvarchar(19) NOT NULL,
    [MaskedCardNumber] nvarchar(19) NOT NULL,
    [CustomerId] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [Type] int NOT NULL,
    [Status] int NOT NULL,
    [ExpiryDate] date NOT NULL,
    [IssueDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [ActivationDate] datetime2 NULL,
    [ActivationChannel] int NULL,
    [SecurityCode] nvarchar(255) NOT NULL,
    [DailyLimit] decimal(18,2) NOT NULL DEFAULT 5000.0,
    [MonthlyLimit] decimal(18,2) NOT NULL DEFAULT 50000.0,
    [AtmDailyLimit] decimal(18,2) NOT NULL DEFAULT 2000.0,
    [ContactlessEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [OnlineTransactionsEnabled] bit NOT NULL DEFAULT CAST(1 AS bit),
    [InternationalTransactionsEnabled] bit NOT NULL DEFAULT CAST(0 AS bit),
    [PinHash] nvarchar(255) NULL,
    [PinSetDate] datetime2 NULL,
    [FailedPinAttempts] int NOT NULL DEFAULT 0,
    [LastBlockedDate] datetime2 NULL,
    [LastBlockReason] int NULL,
    [BlockedMerchantCategories] nvarchar(1000) NULL,
    [CardName] nvarchar(50) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_Cards] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Cards_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Cards_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);
DECLARE @defaultSchema2 AS sysname;
SET @defaultSchema2 = SCHEMA_NAME();
DECLARE @description2 AS sql_variant;
SET @description2 = N'Encrypted card number';
EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Cards', 'COLUMN', N'CardNumber';
SET @description2 = N'Masked card number for display';
EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Cards', 'COLUMN', N'MaskedCardNumber';
SET @description2 = N'Encrypted security code';
EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Cards', 'COLUMN', N'SecurityCode';
SET @description2 = N'JSON array of blocked merchant categories';
EXEC sp_addextendedproperty 'MS_Description', @description2, 'SCHEMA', @defaultSchema2, 'TABLE', N'Cards', 'COLUMN', N'BlockedMerchantCategories';

CREATE TABLE [CardStatusHistories] (
    [Id] uniqueidentifier NOT NULL,
    [CardId] uniqueidentifier NOT NULL,
    [PreviousStatus] int NOT NULL,
    [NewStatus] int NOT NULL,
    [Reason] nvarchar(200) NULL,
    [ChangedBy] uniqueidentifier NULL,
    [ChangeDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(500) NULL,
    [Channel] nvarchar(50) NULL,
    [IpAddress] nvarchar(45) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_CardStatusHistories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CardStatusHistories_AspNetUsers_ChangedBy] FOREIGN KEY ([ChangedBy]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_CardStatusHistories_Cards_CardId] FOREIGN KEY ([CardId]) REFERENCES [Cards] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [CardTransactions] (
    [Id] uniqueidentifier NOT NULL,
    [CardId] uniqueidentifier NOT NULL,
    [AccountId] uniqueidentifier NOT NULL,
    [TransactionId] uniqueidentifier NULL,
    [NetworkTransactionId] nvarchar(50) NOT NULL,
    [AuthorizationCode] nvarchar(20) NULL,
    [Amount] decimal(18,2) NOT NULL,
    [CurrencyCode] nvarchar(3) NOT NULL DEFAULT N'USD',
    [OriginalAmount] decimal(18,2) NULL,
    [OriginalCurrencyCode] nvarchar(3) NULL,
    [ExchangeRate] decimal(10,6) NULL,
    [Type] int NOT NULL,
    [Status] int NOT NULL,
    [TransactionDate] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [AuthorizationDate] datetime2 NULL,
    [SettlementDate] datetime2 NULL,
    [MerchantName] nvarchar(100) NULL,
    [MerchantId] nvarchar(50) NULL,
    [MerchantCategory] int NULL,
    [MerchantCity] nvarchar(50) NULL,
    [MerchantCountryCode] nvarchar(2) NULL,
    [TerminalId] nvarchar(20) NULL,
    [IsContactless] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsOnline] bit NOT NULL DEFAULT CAST(0 AS bit),
    [IsInternational] bit NOT NULL DEFAULT CAST(0 AS bit),
    [PinUsed] bit NOT NULL DEFAULT CAST(0 AS bit),
    [Description] nvarchar(200) NULL,
    [DeclineReason] nvarchar(100) NULL,
    [FraudScore] decimal(5,4) NULL,
    [IsFraudulent] bit NOT NULL DEFAULT CAST(0 AS bit),
    [Fee] decimal(18,2) NOT NULL DEFAULT 0.0,
    [FeeDescription] nvarchar(100) NULL,
    [BalanceAfterTransaction] decimal(18,2) NULL,
    [Metadata] nvarchar(2000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_CardTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CardTransactions_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CardTransactions_Cards_CardId] FOREIGN KEY ([CardId]) REFERENCES [Cards] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_CardTransactions_Transactions_TransactionId] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE SET NULL
);
DECLARE @defaultSchema3 AS sysname;
SET @defaultSchema3 = SCHEMA_NAME();
DECLARE @description3 AS sql_variant;
SET @description3 = N'Additional transaction metadata in JSON format';
EXEC sp_addextendedproperty 'MS_Description', @description3, 'SCHEMA', @defaultSchema3, 'TABLE', N'CardTransactions', 'COLUMN', N'Metadata';

CREATE INDEX [IX_Cards_AccountId] ON [Cards] ([AccountId]);

CREATE UNIQUE INDEX [IX_Cards_CardNumber] ON [Cards] ([CardNumber]);

CREATE INDEX [IX_Cards_CustomerId] ON [Cards] ([CustomerId]);

CREATE INDEX [IX_Cards_CustomerId_Status] ON [Cards] ([CustomerId], [Status]);

CREATE INDEX [IX_Cards_ExpiryDate] ON [Cards] ([ExpiryDate]);

CREATE INDEX [IX_Cards_Status] ON [Cards] ([Status]);

CREATE INDEX [IX_CardStatusHistories_CardId] ON [CardStatusHistories] ([CardId]);

CREATE INDEX [IX_CardStatusHistories_CardId_ChangeDate] ON [CardStatusHistories] ([CardId], [ChangeDate]);

CREATE INDEX [IX_CardStatusHistories_ChangeDate] ON [CardStatusHistories] ([ChangeDate]);

CREATE INDEX [IX_CardStatusHistories_ChangedBy] ON [CardStatusHistories] ([ChangedBy]);

CREATE INDEX [IX_CardTransactions_AccountId] ON [CardTransactions] ([AccountId]);

CREATE INDEX [IX_CardTransactions_CardId] ON [CardTransactions] ([CardId]);

CREATE INDEX [IX_CardTransactions_CardId_Status] ON [CardTransactions] ([CardId], [Status]);

CREATE INDEX [IX_CardTransactions_CardId_TransactionDate] ON [CardTransactions] ([CardId], [TransactionDate]);

CREATE INDEX [IX_CardTransactions_FraudScore] ON [CardTransactions] ([FraudScore]) WHERE [FraudScore] IS NOT NULL;

CREATE INDEX [IX_CardTransactions_MerchantId] ON [CardTransactions] ([MerchantId]);

CREATE UNIQUE INDEX [IX_CardTransactions_NetworkTransactionId] ON [CardTransactions] ([NetworkTransactionId]);

CREATE INDEX [IX_CardTransactions_SettlementDate] ON [CardTransactions] ([SettlementDate]);

CREATE INDEX [IX_CardTransactions_Status] ON [CardTransactions] ([Status]);

CREATE INDEX [IX_CardTransactions_TransactionDate] ON [CardTransactions] ([TransactionDate]);

CREATE INDEX [IX_CardTransactions_TransactionId] ON [CardTransactions] ([TransactionId]);

CREATE INDEX [IX_CardTransactions_Type] ON [CardTransactions] ([Type]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260309051809_UpdateNotificationEntities', N'9.0.3');

ALTER TABLE [CardTransactions] DROP CONSTRAINT [FK_CardTransactions_Accounts_AccountId];

ALTER TABLE [CardTransactions] DROP CONSTRAINT [FK_CardTransactions_Transactions_TransactionId];

DROP INDEX [IX_CardTransactions_AccountId] ON [CardTransactions];

DROP INDEX [IX_CardTransactions_FraudScore] ON [CardTransactions];

DROP INDEX [IX_CardTransactions_NetworkTransactionId] ON [CardTransactions];

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'AccountId');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [AccountId];

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'AuthorizationDate');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [AuthorizationDate];

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'BalanceAfterTransaction');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [BalanceAfterTransaction];

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'ExchangeRate');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [ExchangeRate];

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'Fee');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [Fee];

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'FraudScore');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [FraudScore];

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'IsFraudulent');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [IsFraudulent];

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'MerchantCountryCode');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [MerchantCountryCode];

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'Metadata');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [Metadata];

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'NetworkTransactionId');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [NetworkTransactionId];

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'OriginalCurrencyCode');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [OriginalCurrencyCode];

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'PinUsed');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [PinUsed];

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'TerminalId');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [CardTransactions] DROP COLUMN [TerminalId];

EXEC sp_rename N'[CardTransactions].[Type]', N'TransactionType', 'COLUMN';

EXEC sp_rename N'[CardTransactions].[TransactionId]', N'OriginalTransactionId', 'COLUMN';

EXEC sp_rename N'[CardTransactions].[OriginalAmount]', N'Fees', 'COLUMN';

EXEC sp_rename N'[CardTransactions].[MerchantCity]', N'MerchantCountry', 'COLUMN';

EXEC sp_rename N'[CardTransactions].[FeeDescription]', N'Reference', 'COLUMN';

EXEC sp_rename N'[CardTransactions].[CurrencyCode]', N'Currency', 'COLUMN';

EXEC sp_rename N'[CardTransactions].[IX_CardTransactions_Type]', N'IX_CardTransactions_TransactionType', 'INDEX';

EXEC sp_rename N'[CardTransactions].[IX_CardTransactions_TransactionId]', N'IX_CardTransactions_OriginalTransactionId', 'INDEX';

DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'MerchantName');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var17 + '];');
UPDATE [CardTransactions] SET [MerchantName] = N'' WHERE [MerchantName] IS NULL;
ALTER TABLE [CardTransactions] ALTER COLUMN [MerchantName] nvarchar(100) NOT NULL;
ALTER TABLE [CardTransactions] ADD DEFAULT N'' FOR [MerchantName];

DROP INDEX [IX_CardTransactions_MerchantId] ON [CardTransactions];
DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'MerchantId');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var18 + '];');
UPDATE [CardTransactions] SET [MerchantId] = N'' WHERE [MerchantId] IS NULL;
ALTER TABLE [CardTransactions] ALTER COLUMN [MerchantId] nvarchar(50) NOT NULL;
ALTER TABLE [CardTransactions] ADD DEFAULT N'' FOR [MerchantId];
CREATE INDEX [IX_CardTransactions_MerchantId] ON [CardTransactions] ([MerchantId]);

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[CardTransactions]') AND [c].[name] = N'MerchantCategory');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [CardTransactions] DROP CONSTRAINT [' + @var19 + '];');
UPDATE [CardTransactions] SET [MerchantCategory] = 0 WHERE [MerchantCategory] IS NULL;
ALTER TABLE [CardTransactions] ALTER COLUMN [MerchantCategory] int NOT NULL;
ALTER TABLE [CardTransactions] ADD DEFAULT 0 FOR [MerchantCategory];

ALTER TABLE [CardTransactions] ADD [CardId1] uniqueidentifier NULL;

ALTER TABLE [CardTransactions] ADD [FeeBreakdown] nvarchar(500) NULL;

ALTER TABLE [CardTransactions] ADD [ProcessorResponse] nvarchar(200) NULL;

ALTER TABLE [Accounts] ADD [CustomerId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';

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

CREATE TABLE [CardAuthorizations] (
    [Id] uniqueidentifier NOT NULL,
    [CardId] uniqueidentifier NOT NULL,
    [AuthorizationCode] nvarchar(max) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Currency] nvarchar(max) NOT NULL,
    [MerchantId] nvarchar(max) NOT NULL,
    [MerchantName] nvarchar(max) NOT NULL,
    [MerchantCategory] int NOT NULL,
    [MerchantCountry] nvarchar(max) NULL,
    [TransactionDate] datetime2 NOT NULL,
    [ExpiresAt] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [IsInternational] bit NOT NULL,
    [IsOnline] bit NOT NULL,
    [IsContactless] bit NOT NULL,
    [CapturedAmount] decimal(18,2) NULL,
    [CaptureDate] datetime2 NULL,
    [VoidDate] datetime2 NULL,
    [VoidReason] nvarchar(max) NULL,
    [ProcessorResponse] nvarchar(max) NULL,
    [NetworkTransactionId] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_CardAuthorizations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CardAuthorizations_Cards_CardId] FOREIGN KEY ([CardId]) REFERENCES [Cards] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [CardStatements] (
    [Id] uniqueidentifier NOT NULL,
    [CardId] uniqueidentifier NOT NULL,
    [FromDate] datetime2 NOT NULL,
    [ToDate] datetime2 NOT NULL,
    [GeneratedDate] datetime2 NOT NULL,
    [Format] int NOT NULL,
    [FileName] nvarchar(max) NULL,
    [FilePath] nvarchar(max) NULL,
    [TransactionCount] int NOT NULL,
    [TotalSpent] decimal(18,2) NOT NULL,
    [TotalFees] decimal(18,2) NOT NULL,
    [PreviousBalance] decimal(18,2) NOT NULL,
    [CurrentBalance] decimal(18,2) NOT NULL,
    [MinimumPayment] decimal(18,2) NOT NULL,
    [PaymentDueDate] datetime2 NULL,
    [Status] int NOT NULL,
    [DeliveryMethod] int NULL,
    [DeliveryAddress] nvarchar(max) NULL,
    [DeliveredAt] datetime2 NULL,
    [GeneratedBy] nvarchar(max) NULL,
    [Notes] nvarchar(max) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_CardStatements] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CardStatements_Cards_CardId] FOREIGN KEY ([CardId]) REFERENCES [Cards] ([Id]) ON DELETE CASCADE
);

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

CREATE TABLE [BillerHealthChecks] (
    [Id] uniqueidentifier NOT NULL,
    [BillerId] uniqueidentifier NOT NULL,
    [IsHealthy] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CheckDate] datetime2 NOT NULL,
    [ResponseTime] time NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [ErrorMessage] nvarchar(1000) NULL,
    [HealthMetricsJson] nvarchar(max) NOT NULL,
    [ConsecutiveFailures] int NOT NULL DEFAULT 0,
    [LastSuccessfulCheck] datetime2 NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_BillerHealthChecks] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BillerHealthChecks_Billers_BillerId] FOREIGN KEY ([BillerId]) REFERENCES [Billers] ([Id]) ON DELETE CASCADE
);

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

CREATE TABLE [FixedDeposits] (
    [Id] uniqueidentifier NOT NULL,
    [DepositNumber] nvarchar(50) NOT NULL,
    [CustomerId] uniqueidentifier NOT NULL,
    [DepositProductId] uniqueidentifier NOT NULL,
    [LinkedAccountId] uniqueidentifier NOT NULL,
    [PrincipalAmount] decimal(18,2) NOT NULL,
    [InterestRate] decimal(5,4) NOT NULL,
    [TermDays] int NOT NULL,
    [StartDate] datetime2 NOT NULL,
    [MaturityDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [InterestCalculationMethod] int NOT NULL,
    [CompoundingFrequency] int NOT NULL,
    [AccruedInterest] decimal(18,2) NOT NULL,
    [LastInterestCalculationDate] datetime2 NOT NULL,
    [MaturityAction] int NOT NULL,
    [AutoRenewalEnabled] bit NOT NULL,
    [RenewalTermDays] int NULL,
    [RenewalNoticeDate] datetime2 NULL,
    [CustomerConsentReceived] bit NOT NULL,
    [PenaltyType] int NOT NULL,
    [PenaltyAmount] decimal(18,2) NULL,
    [PenaltyPercentage] decimal(5,4) NULL,
    [ClosureDate] datetime2 NULL,
    [ClosedByUserId] uniqueidentifier NULL,
    [ClosureReason] nvarchar(500) NULL,
    [PenaltyApplied] decimal(18,2) NULL,
    [NetAmountPaid] decimal(18,2) NULL,
    [RenewedFromDepositId] uniqueidentifier NULL,
    [RenewedToDepositId] uniqueidentifier NULL,
    [RenewalCount] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_FixedDeposits] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_FixedDeposits_Accounts_LinkedAccountId] FOREIGN KEY ([LinkedAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_FixedDeposits_AspNetUsers_ClosedByUserId] FOREIGN KEY ([ClosedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_FixedDeposits_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_FixedDeposits_DepositProducts_DepositProductId] FOREIGN KEY ([DepositProductId]) REFERENCES [DepositProducts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_FixedDeposits_FixedDeposits_RenewedFromDepositId] FOREIGN KEY ([RenewedFromDepositId]) REFERENCES [FixedDeposits] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [InterestTiers] (
    [Id] uniqueidentifier NOT NULL,
    [DepositProductId] uniqueidentifier NOT NULL,
    [TierName] nvarchar(100) NOT NULL,
    [MinimumBalance] decimal(18,2) NOT NULL,
    [MaximumBalance] decimal(18,2) NULL,
    [InterestRate] decimal(5,4) NOT NULL,
    [TierBasis] int NOT NULL,
    [IsActive] bit NOT NULL,
    [DisplayOrder] int NOT NULL,
    [EffectiveFromDate] datetime2 NULL,
    [EffectiveToDate] datetime2 NULL,
    [IsPromotional] bit NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_InterestTiers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_InterestTiers_DepositProducts_DepositProductId] FOREIGN KEY ([DepositProductId]) REFERENCES [DepositProducts] ([Id]) ON DELETE CASCADE
);

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

CREATE TABLE [PaymentReceipts] (
    [Id] uniqueidentifier NOT NULL,
    [PaymentId] uniqueidentifier NOT NULL,
    [ReceiptNumber] nvarchar(50) NOT NULL,
    [CustomerId] uniqueidentifier NOT NULL,
    [CustomerName] nvarchar(200) NOT NULL,
    [BillerName] nvarchar(200) NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Currency] nvarchar(3) NOT NULL DEFAULT N'USD',
    [PaymentDate] datetime2 NOT NULL,
    [ProcessedDate] datetime2 NOT NULL,
    [ConfirmationNumber] nvarchar(100) NOT NULL,
    [Reference] nvarchar(100) NOT NULL,
    [PaymentMethod] int NOT NULL,
    [ProcessingFee] decimal(18,2) NULL,
    [Status] int NOT NULL,
    [ReceiptDataJson] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_PaymentReceipts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PaymentReceipts_AspNetUsers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PaymentReceipts_BillPayments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [BillPayments] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [PaymentRetries] (
    [Id] uniqueidentifier NOT NULL,
    [PaymentId] uniqueidentifier NOT NULL,
    [AttemptNumber] int NOT NULL,
    [AttemptDate] datetime2 NOT NULL,
    [NextRetryDate] datetime2 NOT NULL,
    [BackoffDelay] time NOT NULL,
    [FailureReason] nvarchar(1000) NOT NULL,
    [Status] int NOT NULL,
    [IsMaxRetriesReached] bit NOT NULL DEFAULT CAST(0 AS bit),
    [RetryMetadataJson] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_PaymentRetries] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PaymentRetries_BillPayments_PaymentId] FOREIGN KEY ([PaymentId]) REFERENCES [BillPayments] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [DepositCertificates] (
    [Id] uniqueidentifier NOT NULL,
    [FixedDepositId] uniqueidentifier NOT NULL,
    [CertificateNumber] nvarchar(50) NOT NULL,
    [Status] int NOT NULL,
    [IssueDate] datetime2 NOT NULL,
    [DeliveryDate] datetime2 NULL,
    [DeliveryMethod] nvarchar(50) NOT NULL,
    [DeliveryAddress] nvarchar(500) NULL,
    [DeliveryReference] nvarchar(100) NULL,
    [CertificateTemplate] nvarchar(100) NOT NULL,
    [CertificateContent] nvarchar(4000) NOT NULL,
    [CertificatePdf] varbinary(max) NULL,
    [PdfFileName] nvarchar(255) NULL,
    [DigitalSignature] nvarchar(1000) NULL,
    [SecurityHash] nvarchar(500) NULL,
    [VerificationDate] datetime2 NULL,
    [ReplacedCertificateId] uniqueidentifier NULL,
    [ReplacementReason] nvarchar(500) NULL,
    [ReplacedByUserId] uniqueidentifier NULL,
    [GeneratedByUserId] uniqueidentifier NULL,
    [IssuedByUserId] uniqueidentifier NULL,
    [ProcessingNotes] nvarchar(1000) NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_DepositCertificates] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DepositCertificates_AspNetUsers_GeneratedByUserId] FOREIGN KEY ([GeneratedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_DepositCertificates_AspNetUsers_IssuedByUserId] FOREIGN KEY ([IssuedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_DepositCertificates_AspNetUsers_ReplacedByUserId] FOREIGN KEY ([ReplacedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_DepositCertificates_DepositCertificates_ReplacedCertificateId] FOREIGN KEY ([ReplacedCertificateId]) REFERENCES [DepositCertificates] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_DepositCertificates_FixedDeposits_FixedDepositId] FOREIGN KEY ([FixedDepositId]) REFERENCES [FixedDeposits] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [DepositTransactions] (
    [Id] uniqueidentifier NOT NULL,
    [FixedDepositId] uniqueidentifier NOT NULL,
    [TransactionReference] nvarchar(50) NOT NULL,
    [TransactionType] int NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [TransactionDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [InterestPeriodStart] datetime2 NULL,
    [InterestPeriodEnd] datetime2 NULL,
    [InterestRate] decimal(5,4) NULL,
    [InterestDays] int NULL,
    [PenaltyType] int NULL,
    [PenaltyReason] nvarchar(500) NULL,
    [ProcessedByUserId] uniqueidentifier NULL,
    [ProcessedDate] datetime2 NULL,
    [ProcessingNotes] nvarchar(1000) NULL,
    [RelatedTransactionId] uniqueidentifier NULL,
    [AccountTransactionId] uniqueidentifier NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_DepositTransactions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DepositTransactions_AspNetUsers_ProcessedByUserId] FOREIGN KEY ([ProcessedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_DepositTransactions_DepositTransactions_RelatedTransactionId] FOREIGN KEY ([RelatedTransactionId]) REFERENCES [DepositTransactions] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_DepositTransactions_FixedDeposits_FixedDepositId] FOREIGN KEY ([FixedDepositId]) REFERENCES [FixedDeposits] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DepositTransactions_Transactions_AccountTransactionId] FOREIGN KEY ([AccountTransactionId]) REFERENCES [Transactions] ([Id]) ON DELETE SET NULL
);

CREATE TABLE [MaturityNotices] (
    [Id] uniqueidentifier NOT NULL,
    [FixedDepositId] uniqueidentifier NOT NULL,
    [NoticeNumber] nvarchar(50) NOT NULL,
    [NoticeType] int NOT NULL,
    [NoticeDate] datetime2 NOT NULL,
    [MaturityDate] datetime2 NOT NULL,
    [Status] int NOT NULL,
    [Subject] nvarchar(200) NOT NULL,
    [Content] nvarchar(4000) NOT NULL,
    [TemplateUsed] nvarchar(100) NULL,
    [DeliveryChannel] int NOT NULL,
    [DeliveryAddress] nvarchar(500) NULL,
    [DeliveryDate] datetime2 NULL,
    [DeliveryReference] nvarchar(100) NULL,
    [DeliveryAttempts] int NOT NULL,
    [CustomerResponseDate] datetime2 NULL,
    [CustomerChoice] int NULL,
    [CustomerInstructions] nvarchar(1000) NULL,
    [ConsentReceived] bit NOT NULL,
    [GeneratedByUserId] uniqueidentifier NULL,
    [ProcessedDate] datetime2 NULL,
    [ProcessingNotes] nvarchar(1000) NULL,
    [FollowUpDate] datetime2 NULL,
    [RequiresFollowUp] bit NOT NULL,
    [RemindersSent] int NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [CreatedBy] nvarchar(max) NULL,
    [UpdatedAt] datetime2 NULL,
    [UpdatedBy] nvarchar(max) NULL,
    [IsDeleted] bit NOT NULL,
    [DeletedAt] datetime2 NULL,
    [DeletedBy] nvarchar(max) NULL,
    CONSTRAINT [PK_MaturityNotices] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_MaturityNotices_AspNetUsers_GeneratedByUserId] FOREIGN KEY ([GeneratedByUserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_MaturityNotices_FixedDeposits_FixedDepositId] FOREIGN KEY ([FixedDepositId]) REFERENCES [FixedDeposits] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_CardTransactions_AuthorizationCode] ON [CardTransactions] ([AuthorizationCode]) WHERE [AuthorizationCode] IS NOT NULL;

CREATE INDEX [IX_CardTransactions_CardId1] ON [CardTransactions] ([CardId1]);

CREATE INDEX [IX_BillerHealthChecks_BillerId] ON [BillerHealthChecks] ([BillerId]);

CREATE INDEX [IX_BillerHealthChecks_BillerId_CheckDate] ON [BillerHealthChecks] ([BillerId], [CheckDate]);

CREATE INDEX [IX_BillerHealthChecks_CheckDate] ON [BillerHealthChecks] ([CheckDate]);

CREATE INDEX [IX_BillerHealthChecks_IsHealthy] ON [BillerHealthChecks] ([IsHealthy]);

CREATE UNIQUE INDEX [IX_Billers_AccountNumber_RoutingNumber] ON [Billers] ([AccountNumber], [RoutingNumber]);

CREATE INDEX [IX_Billers_Category] ON [Billers] ([Category]);

CREATE INDEX [IX_Billers_IsActive] ON [Billers] ([IsActive]);

CREATE INDEX [IX_Billers_Name] ON [Billers] ([Name]);

CREATE INDEX [IX_BillPayments_BillerId] ON [BillPayments] ([BillerId]);

CREATE INDEX [IX_BillPayments_CustomerId] ON [BillPayments] ([CustomerId]);

CREATE INDEX [IX_BillPayments_CustomerId_Status] ON [BillPayments] ([CustomerId], [Status]);

CREATE INDEX [IX_BillPayments_RecurringPaymentId] ON [BillPayments] ([RecurringPaymentId]);

CREATE INDEX [IX_BillPayments_ScheduledDate] ON [BillPayments] ([ScheduledDate]);

CREATE INDEX [IX_BillPayments_Status] ON [BillPayments] ([Status]);

CREATE INDEX [IX_BillPayments_Status_ScheduledDate] ON [BillPayments] ([Status], [ScheduledDate]);

CREATE INDEX [IX_BillPresentments_BillerId] ON [BillPresentments] ([BillerId]);

CREATE INDEX [IX_BillPresentments_CustomerId] ON [BillPresentments] ([CustomerId]);

CREATE INDEX [IX_BillPresentments_CustomerId_Status] ON [BillPresentments] ([CustomerId], [Status]);

CREATE INDEX [IX_BillPresentments_DueDate] ON [BillPresentments] ([DueDate]);

CREATE UNIQUE INDEX [IX_BillPresentments_ExternalBillId] ON [BillPresentments] ([ExternalBillId]);

CREATE INDEX [IX_BillPresentments_PaymentId] ON [BillPresentments] ([PaymentId]);

CREATE INDEX [IX_BillPresentments_Status] ON [BillPresentments] ([Status]);

CREATE INDEX [IX_CardAuthorizations_CardId] ON [CardAuthorizations] ([CardId]);

CREATE INDEX [IX_CardStatements_CardId] ON [CardStatements] ([CardId]);

CREATE UNIQUE INDEX [IX_DepositCertificates_CertificateNumber] ON [DepositCertificates] ([CertificateNumber]);

CREATE INDEX [IX_DepositCertificates_FixedDepositId] ON [DepositCertificates] ([FixedDepositId]);

CREATE INDEX [IX_DepositCertificates_GeneratedByUserId] ON [DepositCertificates] ([GeneratedByUserId]);

CREATE INDEX [IX_DepositCertificates_IssueDate] ON [DepositCertificates] ([IssueDate]);

CREATE INDEX [IX_DepositCertificates_IssuedByUserId] ON [DepositCertificates] ([IssuedByUserId]);

CREATE INDEX [IX_DepositCertificates_ReplacedByUserId] ON [DepositCertificates] ([ReplacedByUserId]);

CREATE INDEX [IX_DepositCertificates_ReplacedCertificateId] ON [DepositCertificates] ([ReplacedCertificateId]);

CREATE INDEX [IX_DepositCertificates_Status] ON [DepositCertificates] ([Status]);

CREATE INDEX [IX_DepositProducts_IsActive] ON [DepositProducts] ([IsActive]);

CREATE INDEX [IX_DepositProducts_ProductType] ON [DepositProducts] ([ProductType]);

CREATE INDEX [IX_DepositProducts_ProductType_IsActive] ON [DepositProducts] ([ProductType], [IsActive]);

CREATE INDEX [IX_DepositTransactions_AccountTransactionId] ON [DepositTransactions] ([AccountTransactionId]);

CREATE INDEX [IX_DepositTransactions_FixedDepositId] ON [DepositTransactions] ([FixedDepositId]);

CREATE INDEX [IX_DepositTransactions_FixedDepositId_TransactionDate] ON [DepositTransactions] ([FixedDepositId], [TransactionDate]);

CREATE INDEX [IX_DepositTransactions_FixedDepositId_TransactionType] ON [DepositTransactions] ([FixedDepositId], [TransactionType]);

CREATE INDEX [IX_DepositTransactions_ProcessedByUserId] ON [DepositTransactions] ([ProcessedByUserId]);

CREATE INDEX [IX_DepositTransactions_RelatedTransactionId] ON [DepositTransactions] ([RelatedTransactionId]);

CREATE INDEX [IX_DepositTransactions_TransactionDate] ON [DepositTransactions] ([TransactionDate]);

CREATE UNIQUE INDEX [IX_DepositTransactions_TransactionReference] ON [DepositTransactions] ([TransactionReference]);

CREATE INDEX [IX_DepositTransactions_TransactionType] ON [DepositTransactions] ([TransactionType]);

CREATE INDEX [IX_FixedDeposits_ClosedByUserId] ON [FixedDeposits] ([ClosedByUserId]);

CREATE INDEX [IX_FixedDeposits_CustomerId] ON [FixedDeposits] ([CustomerId]);

CREATE INDEX [IX_FixedDeposits_CustomerId_Status] ON [FixedDeposits] ([CustomerId], [Status]);

CREATE UNIQUE INDEX [IX_FixedDeposits_DepositNumber] ON [FixedDeposits] ([DepositNumber]);

CREATE INDEX [IX_FixedDeposits_DepositProductId] ON [FixedDeposits] ([DepositProductId]);

CREATE INDEX [IX_FixedDeposits_LinkedAccountId] ON [FixedDeposits] ([LinkedAccountId]);

CREATE INDEX [IX_FixedDeposits_MaturityDate] ON [FixedDeposits] ([MaturityDate]);

CREATE UNIQUE INDEX [IX_FixedDeposits_RenewedFromDepositId] ON [FixedDeposits] ([RenewedFromDepositId]) WHERE [RenewedFromDepositId] IS NOT NULL;

CREATE INDEX [IX_FixedDeposits_Status] ON [FixedDeposits] ([Status]);

CREATE INDEX [IX_FixedDeposits_Status_MaturityDate] ON [FixedDeposits] ([Status], [MaturityDate]);

CREATE INDEX [IX_InterestTiers_DepositProductId] ON [InterestTiers] ([DepositProductId]);

CREATE INDEX [IX_InterestTiers_DepositProductId_IsActive] ON [InterestTiers] ([DepositProductId], [IsActive]);

CREATE INDEX [IX_InterestTiers_DepositProductId_MinimumBalance] ON [InterestTiers] ([DepositProductId], [MinimumBalance]);

CREATE INDEX [IX_InterestTiers_DisplayOrder] ON [InterestTiers] ([DisplayOrder]);

CREATE INDEX [IX_MaturityNotices_FixedDepositId] ON [MaturityNotices] ([FixedDepositId]);

CREATE INDEX [IX_MaturityNotices_GeneratedByUserId] ON [MaturityNotices] ([GeneratedByUserId]);

CREATE INDEX [IX_MaturityNotices_MaturityDate] ON [MaturityNotices] ([MaturityDate]);

CREATE UNIQUE INDEX [IX_MaturityNotices_NoticeNumber] ON [MaturityNotices] ([NoticeNumber]);

CREATE INDEX [IX_MaturityNotices_NoticeType] ON [MaturityNotices] ([NoticeType]);

CREATE INDEX [IX_MaturityNotices_Status] ON [MaturityNotices] ([Status]);

CREATE INDEX [IX_MaturityNotices_Status_NoticeDate] ON [MaturityNotices] ([Status], [NoticeDate]);

CREATE UNIQUE INDEX [IX_NotificationPreferences_UserId] ON [NotificationPreferences] ([UserId]);

CREATE INDEX [IX_Notifications_CreatedAt] ON [Notifications] ([CreatedAt]);

CREATE INDEX [IX_Notifications_ScheduledAt] ON [Notifications] ([ScheduledAt]);

CREATE INDEX [IX_Notifications_Status] ON [Notifications] ([Status]);

CREATE INDEX [IX_Notifications_Type] ON [Notifications] ([Type]);

CREATE INDEX [IX_Notifications_UserId] ON [Notifications] ([UserId]);

CREATE INDEX [IX_Notifications_UserId_Status] ON [Notifications] ([UserId], [Status]);

CREATE INDEX [IX_PaymentReceipts_ConfirmationNumber] ON [PaymentReceipts] ([ConfirmationNumber]);

CREATE INDEX [IX_PaymentReceipts_CustomerId] ON [PaymentReceipts] ([CustomerId]);

CREATE UNIQUE INDEX [IX_PaymentReceipts_PaymentId] ON [PaymentReceipts] ([PaymentId]);

CREATE UNIQUE INDEX [IX_PaymentReceipts_ReceiptNumber] ON [PaymentReceipts] ([ReceiptNumber]);

CREATE INDEX [IX_PaymentReceipts_Status] ON [PaymentReceipts] ([Status]);

CREATE INDEX [IX_PaymentRetries_IsMaxRetriesReached] ON [PaymentRetries] ([IsMaxRetriesReached]);

CREATE INDEX [IX_PaymentRetries_NextRetryDate] ON [PaymentRetries] ([NextRetryDate]);

CREATE INDEX [IX_PaymentRetries_PaymentId] ON [PaymentRetries] ([PaymentId]);

CREATE UNIQUE INDEX [IX_PaymentRetries_PaymentId_AttemptNumber] ON [PaymentRetries] ([PaymentId], [AttemptNumber]);

CREATE INDEX [IX_PaymentRetries_Status] ON [PaymentRetries] ([Status]);

ALTER TABLE [CardTransactions] ADD CONSTRAINT [FK_CardTransactions_CardTransactions_OriginalTransactionId] FOREIGN KEY ([OriginalTransactionId]) REFERENCES [CardTransactions] ([Id]) ON DELETE SET NULL;

ALTER TABLE [CardTransactions] ADD CONSTRAINT [FK_CardTransactions_Cards_CardId1] FOREIGN KEY ([CardId1]) REFERENCES [Cards] ([Id]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260309123722_UpdateBillPresentmentAndCurrentModel', N'9.0.3');

COMMIT;
GO

