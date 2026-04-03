# Bank API Backend - Implementation & Verification Plan

## ✅ DTO Reorganization Complete

**The DTO reorganization has been successfully completed!**

All 195 DTO files are now organized into a clean, hierarchical structure:
- **145 files moved** to appropriate subfolders
- **23 files updated** with new using statements
- **All namespaces updated** to match folder hierarchy
- **Zero DTO-related compilation errors**

See **[DTO_MIGRATION_COMPLETE.md](DTO_MIGRATION_COMPLETE.md)** for complete details.

---

## Overview
This document outlines the step-by-step plan to verify, configure, and run the Bank API Backend application.

---

## Phase 1: Pre-Flight Checks ✅

### 1.1 Verify Prerequisites
- [ ] .NET 9.0 SDK or later installed (project uses .NET 9.0)
- [ ] SQL Server LocalDB or SQL Server instance available
- [ ] Visual Studio 2022 / VS Code / Rider (optional)
- [ ] Git installed

**Verification Commands:**
```bash
# Check .NET version (should be 9.0.x or later)
dotnet --version

# Check SQL Server LocalDB
sqllocaldb info

# List available LocalDB instances
sqllocaldb info mssqllocaldb
```

### 1.2 Project Structure Verification
```
Bank-Api/
├── src/
│   ├── Bank.Api/              # Web API Layer
│   ├── Bank.Application/      # Business Logic (CQRS)
│   ├── Bank.Domain/           # Domain Entities
│   ├── Bank.Infrastructure/   # Data Access & External Services
│   └── Bank.Tests/            # Unit & Integration Tests
└── Bank.sln                   # Solution File
```

---

## Phase 2: Configuration Setup 🔧

### 2.1 Database Configuration

**Current Connection String:**
```json
"Server=(localdb)\\mssqllocaldb;Database=BankingSystemLocalDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
```

**Options:**

**Option A: Use LocalDB (Recommended for Development)**
- No changes needed
- LocalDB will auto-create database on first run

**Option B: Use SQL Server**
Update `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BankingSystemDb;User Id=sa;Password=YourPassword;TrustServerCertificate=true"
}
```

**Option C: Use SQL Server with Windows Auth**
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BankingSystemDb;Integrated Security=true;TrustServerCertificate=true"
}
```

### 2.2 JWT Configuration
Current settings in `appsettings.json`:
```json
"Jwt": {
  "Key": "A_Very_Strong_And_Secret_Key_For_Bank_Simulator",
  "Issuer": "BankSimulator",
  "Audience": "BankSimulatorFrontend"
}
```

**For Production:** Change the JWT Key to a secure random string (minimum 32 characters)

### 2.3 Environment-Specific Settings

Create `appsettings.Development.json` if needed:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Information"
    }
  },
  "DatabaseSettings": {
    "SkipMigrations": false,
    "SkipSeeding": false
  }
}
```

### 2.4 DTO Namespace Structure (Updated)

All DTOs are now organized into a hierarchical structure with specific namespaces:

**Example Using Statements:**
```csharp
// Auth DTOs
using Bank.Application.DTOs.Auth.Core;
using Bank.Application.DTOs.Auth.TwoFactor;
using Bank.Application.DTOs.Auth.Security;
using Bank.Application.DTOs.Auth.Session;

// Card DTOs
using Bank.Application.DTOs.Card.Core;
using Bank.Application.DTOs.Card.Activation;
using Bank.Application.DTOs.Card.Transactions;
using Bank.Application.DTOs.Card.Operations;

// Loan DTOs
using Bank.Application.DTOs.Loan.Core;
using Bank.Application.DTOs.Loan.Approval;
using Bank.Application.DTOs.Loan.Repayment;

// Payment DTOs
using Bank.Application.DTOs.Payment.Beneficiary;
using Bank.Application.DTOs.Payment.Biller;
using Bank.Application.DTOs.Payment.Recurring;

// Shared DTOs
using Bank.Application.DTOs.Shared.Notification;
using Bank.Application.DTOs.Shared.Audit;
```

**DTO Organization by Domain:**
- **Account**: Core, Validation, Lockout, Profile, JointAccount, Transfer
- **Auth**: Core, TwoFactor, Security, Session
- **Card**: Core, Activation, Transactions, Fees, Operations, Advanced
- **Deposit**: Core, FixedDeposit, Interest, Maturity, Withdrawal
- **Loan**: Core, Application, Approval, Disbursement, Repayment, Analytics, Configuration
- **Payment**: Core, Beneficiary, Biller, Batch, Routing, Receipt, Recurring, Template
- **Statement**: Core, Search, Summary, Delivery, Analytics, Transaction
- **Transaction**: Core, Search, Analytics, Fraud
- **Shared**: Notification, Audit, RateLimit

---

## Phase 3: Build & Restore 🔨

### 3.1 Restore NuGet Packages
```bash
cd Bank-Api/src
dotnet restore
```

### 3.2 Build Solution
```bash
# Build entire solution
dotnet build

# Or build specific project
dotnet build Bank.Api/Bank.Api.csproj
```

### 3.3 Check for Build Errors
```bash
# Clean and rebuild if needed
dotnet clean
dotnet build --no-incremental
```

---

## Phase 4: Database Setup 💾

### 4.1 Entity Framework Migrations

**IMPORTANT: Database Migration Issue Fix**

The project has migration conflicts. Follow these steps to fix:

**Step 1: Reset LocalDB completely**
```bash
sqllocaldb stop mssqllocaldb
sqllocaldb delete mssqllocaldb
sqllocaldb create mssqllocaldb
sqllocaldb start mssqllocaldb
```

**Step 2: Delete orphaned database files**
```bash
# Delete any leftover database files
Remove-Item -Path "C:\Users\$env:USERNAME\BankingSystemLocalDb.mdf" -Force -ErrorAction SilentlyContinue
Remove-Item -Path "C:\Users\$env:USERNAME\BankingSystemLocalDb_log.ldf" -Force -ErrorAction SilentlyContinue
```

**Step 3: Use application auto-migration**
The application automatically applies migrations on startup. Just run the app:
```bash
cd Bank-Api/src/Bank.Api
dotnet run
```

**Alternative: Manual migration (if auto-migration is disabled)**
```bash
cd Bank.Infrastructure
dotnet ef database update --startup-project ../Bank.Api
```

**Check existing migrations:**
```bash
cd Bank.Infrastructure
dotnet ef migrations list --startup-project ../Bank.Api
```

### 4.2 Verify Database Creation
```sql
-- Connect to SQL Server and verify
USE BankingSystemLocalDb;
GO

-- List all tables
SELECT TABLE_NAME 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE';
```

### 4.3 Data Seeding
The application automatically seeds initial data on startup:
- Default admin user
- Sample accounts
- Test data (if in development mode)

---

## Phase 5: Run Application 🚀

### 5.1 Run from Command Line
```bash
cd Bank-Api/src/Bank.Api
dotnet run
```

### 5.2 Run with Watch (Auto-reload)
```bash
dotnet watch run
```

### 5.3 Run with Specific Profile
```bash
# Development
dotnet run --launch-profile "Development"

# Production
dotnet run --launch-profile "Production"
```

### 5.4 Expected Output
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

---

## Phase 6: Verification & Testing ✔️

### 6.1 DTO Structure Verification

**Verify all DTOs are in correct subfolders:**
```bash
# Check DTO folder structure
Get-ChildItem -Path "Bank-Api/src/Bank.Application/DTOs" -Directory -Recurse | Select-Object FullName

# Verify no orphaned DTO files in root folders
Get-ChildItem -Path "Bank-Api/src/Bank.Application/DTOs/Auth" -Filter "*.cs" | Where-Object { $_.Directory.Name -eq "Auth" }
Get-ChildItem -Path "Bank-Api/src/Bank.Application/DTOs/Card" -Filter "*.cs" | Where-Object { $_.Directory.Name -eq "Card" }
```

**Expected Result:** All DTO files should be in subfolders, not in domain root folders.

### 6.2 Health Check Endpoints
```bash
# Basic health check
curl http://localhost:5000/health

# Detailed health check
curl http://localhost:5000/health/ready
```

### 6.3 Access Swagger UI
Open browser and navigate to:
```
https://localhost:5001/swagger
```

### 6.4 Test Static Files (wwwroot)
```
https://localhost:5001/Home.html
https://localhost:5001/Docs.html
https://localhost:5001/404.html
```

### 6.5 Test API Endpoints

**Register a new user:**
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test@123456",
    "firstName": "John",
    "lastName": "Doe",
    "phoneNumber": "+1234567890"
  }'
```

**Login:**
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test@123456"
  }'
```

**Get Accounts (with token):**
```bash
curl -X GET https://localhost:5001/api/Account \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

### 6.6 Run Unit Tests
```bash
cd Bank-Api/src
dotnet test
```

---

## Phase 7: Troubleshooting 🔍

### Common Issues & Solutions

#### Issue 1: Database Connection Failed
**Error:** `Cannot open database "BankingSystemLocalDb"`

**Solutions:**
1. Check if SQL Server LocalDB is running:
   ```bash
   sqllocaldb start mssqllocaldb
   ```

2. Create LocalDB instance if missing:
   ```bash
   sqllocaldb create mssqllocaldb
   sqllocaldb start mssqllocaldb
   ```

3. Verify connection string in `appsettings.json`

#### Issue 2: Missing Database Tables (BillPayments, FixedDeposits, etc.)
**Error:** `Invalid object name 'BillPayments'` or `Invalid object name 'FixedDeposits'`

**Root Cause:** Not all migrations were applied successfully

**Solution:**
1. Stop the running application (Ctrl+C)
2. Reset the database completely:
   ```bash
   # Stop and recreate LocalDB
   sqllocaldb stop mssqllocaldb
   sqllocaldb delete mssqllocaldb
   sqllocaldb create mssqllocaldb
   sqllocaldb start mssqllocaldb
   
   # Delete orphaned files
   Remove-Item -Path "C:\Users\$env:USERNAME\BankingSystemLocalDb.mdf" -Force -ErrorAction SilentlyContinue
   Remove-Item -Path "C:\Users\$env:USERNAME\BankingSystemLocalDb_log.ldf" -Force -ErrorAction SilentlyContinue
   ```

3. Run the application - it will auto-create all tables:
   ```bash
   cd Bank-Api/src/Bank.Api
   dotnet run
   ```

The application will automatically apply all 10 migrations and create all required tables.

#### Issue 3: Migration Conflicts
**Error:** `There is already an object named 'Cards' in the database`

**Solution:**
This indicates a partial migration. Follow the steps in Issue 2 to completely reset the database.

#### Issue 3: Port Already in Use
**Error:** `Address already in use`

**Solution:**
1. Change ports in `launchSettings.json`
2. Or kill the process using the port:
   ```bash
   # Windows
   netstat -ano | findstr :5000
   taskkill /PID <PID> /F
   ```

#### Issue 4: JWT Authentication Fails
**Error:** `401 Unauthorized`

**Solutions:**
1. Verify JWT token is included in Authorization header
2. Check token expiration
3. Verify JWT Key in `appsettings.json` matches

#### Issue 5: CORS Errors
**Error:** `CORS policy blocked`

**Solution:**
Check CORS configuration in `ServiceCollectionExtensions.cs`

---

## Phase 8: Production Deployment Checklist 📋

### 8.1 Security
- [ ] Change JWT secret key to strong random value
- [ ] Enable HTTPS only
- [ ] Configure proper CORS origins
- [ ] Set secure cookie policies
- [ ] Enable rate limiting
- [ ] Configure proper logging (remove sensitive data)

### 8.2 Database
- [ ] Use production database server
- [ ] Enable connection pooling
- [ ] Configure backup strategy
- [ ] Set up database monitoring
- [ ] Disable auto-migrations in production

### 8.3 Configuration
- [ ] Set `ASPNETCORE_ENVIRONMENT=Production`
- [ ] Configure production connection strings
- [ ] Set up application insights / monitoring
- [ ] Configure email service for notifications
- [ ] Set up proper error handling

### 8.4 Performance
- [ ] Enable response caching
- [ ] Configure Redis for distributed caching
- [ ] Enable response compression
- [ ] Set up CDN for static files
- [ ] Configure load balancing

---

## Quick Start Commands 🎯

```bash
# 1. Navigate to project
cd Bank-Api/src

# 2. Restore packages
dotnet restore

# 3. Build solution
dotnet build

# 4. Apply migrations
cd Bank.Infrastructure
dotnet ef database update --startup-project ../Bank.Api

# 5. Run application
cd ../Bank.Api
dotnet run

# 6. Open browser
start https://localhost:5001/Home.html
```

---

## Verification Checklist ✅

- [ ] .NET SDK installed and verified
- [ ] DTO structure verified (all files in subfolders)
- [ ] DTO namespaces correct (Bank.Application.DTOs.{Domain}.{Subfolder})
- [ ] Using statements updated in services/controllers/mappings
- [ ] Database connection successful
- [ ] Migrations applied successfully
- [ ] Application builds without DTO-related errors
- [ ] Application starts successfully
- [ ] Swagger UI accessible
- [ ] Static files (Home.html) load correctly
- [ ] Blue color theme displays correctly
- [ ] API endpoints respond correctly
- [ ] Authentication works (register/login)
- [ ] Database seeding completed
- [ ] Unit tests pass

---

## Support & Resources 📚

- **Swagger Documentation:** https://localhost:5001/swagger
- **API Home:** https://localhost:5001/Home.html
- **API Docs:** https://localhost:5001/Docs.html
- **.NET Documentation:** https://docs.microsoft.com/dotnet
- **EF Core Documentation:** https://docs.microsoft.com/ef/core

---

## Next Steps 🎯

1. Review and execute Phase 1-3 (Prerequisites, Configuration, Build)
2. Set up database (Phase 4)
3. Run the application (Phase 5)
4. Verify all endpoints (Phase 6)
5. Address any issues using troubleshooting guide (Phase 7)
6. Plan for production deployment (Phase 8)

---

**Last Updated:** 2024
**Version:** 1.0
**Status:** Ready for Implementation
