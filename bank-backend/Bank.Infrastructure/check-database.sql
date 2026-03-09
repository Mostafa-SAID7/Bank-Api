-- Database Check Script
-- Run this to see what tables already exist and verify connection

PRINT 'Checking database connection and existing tables...';
PRINT 'Database: ' + DB_NAME();
PRINT 'Server: ' + @@SERVERNAME;
PRINT 'Current Time: ' + CONVERT(varchar, GETDATE(), 120);
PRINT '';

-- Check existing tables
PRINT '=== EXISTING TABLES ===';
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME,
    TABLE_TYPE
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;

PRINT '';
PRINT '=== ASP.NET IDENTITY TABLES ===';
SELECT 
    TABLE_NAME,
    CASE 
        WHEN TABLE_NAME IN ('AspNetUserLogins', 'AspNetUserTokens') THEN 'May have key length warnings'
        ELSE 'OK'
    END as Status
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_NAME LIKE 'AspNet%'
ORDER BY TABLE_NAME;

PRINT '';
PRINT '=== MIGRATION HISTORY ===';
IF EXISTS (SELECT * FROM sys.tables WHERE name = '__EFMigrationsHistory')
BEGIN
    SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory ORDER BY MigrationId;
END
ELSE
BEGIN
    PRINT 'No migration history found.';
END

PRINT '';
PRINT '=== CARDS TABLE CHECK ===';
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Cards')
BEGIN
    PRINT 'Cards table exists - checking structure:';
    SELECT 
        COLUMN_NAME,
        DATA_TYPE,
        IS_NULLABLE,
        COLUMN_DEFAULT
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Cards'
    ORDER BY ORDINAL_POSITION;
END
ELSE
BEGIN
    PRINT 'Cards table does not exist.';
END

PRINT '';
PRINT 'Database check completed.';