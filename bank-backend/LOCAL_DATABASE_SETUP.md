# Local Database Setup Guide

This guide will help you set up the banking application with a local SQL Server database.

## Prerequisites

1. **SQL Server LocalDB** (recommended) or **SQL Server Express**
2. **.NET 9 SDK**
3. **EF Core Tools** (will be installed automatically)

## Connection String Options

The application supports multiple local database configurations:

### Option 1: SQL Server LocalDB (Recommended)
```json
"Server=(localdb)\\mssqllocaldb;Database=BankingSystemDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

### Option 2: SQL Server Express
```json
"Server=.\\SQLEXPRESS;Database=BankingSystemDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false"
```

### Option 3: Full SQL Server Instance
```json
"Server=localhost;Database=BankingSystemDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;Encrypt=false"
```

## Setup Methods

### Method 1: Automatic Setup (Recommended)

1. **Run the batch file:**
   ```cmd
   apply-migrations.bat
   ```

2. **Or run the PowerShell script:**
   ```powershell
   .\apply-local-migrations.ps1
   ```

### Method 2: Manual Setup

1. **Install EF Core Tools (if not already installed):**
   ```cmd
   dotnet tool install --global dotnet-ef
   ```

2. **Apply migrations:**
   ```cmd
   dotnet ef database update --project Bank.Infrastructure --startup-project Bank.Api
   ```

3. **Run the application:**
   ```cmd
   dotnet run --project Bank.Api
   ```

### Method 3: Using Different Environments

**For LocalDB (default):**
```cmd
set ASPNETCORE_ENVIRONMENT=Development
dotnet run --project Bank.Api
```

**For SQL Server Express:**
```cmd
set ASPNETCORE_ENVIRONMENT=Local
dotnet run --project Bank.Api
```

## Configuration Files

- `appsettings.json` - Production settings (LocalDB)
- `appsettings.Development.json` - Development settings (LocalDB)
- `appsettings.Local.json` - Local SQL Server Express settings

## Database Features

The local database will include:

✅ **ASP.NET Identity Tables** (with optimized key lengths)
✅ **Banking Core Tables** (Accounts, Transactions, Cards)
✅ **Bill Payment System** (Billers, BillPayments, BillPresentments)
✅ **Loan Management** (Loans, LoanPayments, LoanDocuments)
✅ **Deposit Products** (FixedDeposits, DepositProducts, InterestTiers)
✅ **Notification System** (Notifications, NotificationPreferences)
✅ **Audit & Security** (AuditLogs, Sessions, PasswordPolicies)
✅ **Background Jobs** (Hangfire tables)

## Troubleshooting

### LocalDB Issues
```cmd
# Check LocalDB instances
sqllocaldb info

# Create LocalDB instance if needed
sqllocaldb create mssqllocaldb
sqllocaldb start mssqllocaldb
```

### SQL Server Express Issues
1. Ensure SQL Server Express is installed and running
2. Check Windows Services for "SQL Server (SQLEXPRESS)"
3. Verify TCP/IP is enabled in SQL Server Configuration Manager

### Connection Issues
1. Check if the database service is running
2. Verify connection string in appsettings.json
3. Ensure Windows Authentication is working
4. Try different connection string options above

### Migration Issues
```cmd
# Clean and rebuild
dotnet clean
dotnet build

# Remove existing migrations (if needed)
dotnet ef migrations remove --project Bank.Infrastructure --startup-project Bank.Api

# Add new migration
dotnet ef migrations add InitialCreate --project Bank.Infrastructure --startup-project Bank.Api

# Update database
dotnet ef database update --project Bank.Infrastructure --startup-project Bank.Api
```

## Default Admin User

After successful setup, you can log in with:
- **Email:** admin@finbank.com
- **Password:** Admin123!

## API Endpoints

Once running, the API will be available at:
- **HTTP:** http://localhost:5000
- **HTTPS:** https://localhost:5001
- **Swagger:** https://localhost:5001/swagger

## Next Steps

1. Test the API endpoints using Swagger UI
2. Create additional users through the registration endpoint
3. Explore the banking features (accounts, transactions, cards, etc.)
4. Check the database tables in SQL Server Management Studio or Azure Data Studio