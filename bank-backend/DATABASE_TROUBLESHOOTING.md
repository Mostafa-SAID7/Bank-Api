# Database Connectivity Troubleshooting Guide

## Current Issue
The application cannot connect to the remote database server `db43977.public.databaseasp.net`.

**Error Details:**
- Error Number: 53
- Message: "The network path was not found"
- Provider: Named Pipes Provider, error: 40

## Immediate Solutions

### Option 1: Run in Development Mode (Recommended)
The application now supports offline development mode:

```bash
# Set environment to Development
$env:ASPNETCORE_ENVIRONMENT="Development"

# Run the application
dotnet run --project bank-backend/Bank.Api
```

This will:
- Skip database migrations
- Skip data seeding
- Use reduced connection timeouts
- Allow the API to start without database connectivity

### Option 2: Manual Database Setup
If you can connect to the database manually:

1. **Use SQL Server Management Studio or similar tool**
2. **Connect to:** `db43977.public.databaseasp.net`
3. **Database:** `db43977`
4. **Credentials:** `db43977` / `3Sp-m9?A7+Kt`
5. **Run the migration script:** `bank-backend/Bank.Infrastructure/improved-migration.sql`

### Option 3: Test Connectivity
Run the connection test utility:

```bash
dotnet run --project bank-backend/TestConnectionUtility
```

## Troubleshooting Steps

### 1. Network Connectivity
```bash
# Test if the server is reachable
ping db43977.public.databaseasp.net

# Test if the SQL Server port is open (usually 1433)
telnet db43977.public.databaseasp.net 1433
```

### 2. Check Firewall Settings
- Ensure your firewall allows outbound connections on port 1433
- Check if your network/ISP blocks database connections
- Try from a different network if possible

### 3. Verify Database Service Status
- Contact your database provider to confirm the service is running
- Check if there are any maintenance windows or outages
- Verify the connection string details are correct

### 4. Alternative Connection Methods
Try different connection approaches:

```csharp
// TCP/IP instead of Named Pipes
"Server=tcp:db43977.public.databaseasp.net,1433;Database=db43977;..."

// Explicit protocol
"Data Source=tcp:db43977.public.databaseasp.net;Initial Catalog=db43977;..."
```

## Configuration Files Updated

1. **appsettings.Development.json** - Offline mode configuration
2. **Program.cs** - Skip migrations/seeding in development
3. **ServiceCollectionExtensions.cs** - Reduced timeouts for development
4. **DataSeedingExtensions.cs** - Better error handling

## Migration Scripts Available

1. **improved-migration.sql** - Handles existing tables, fixes key length warnings
2. **safe-migration.sql** - Original safe migration approach
3. **check-database.sql** - Database verification script

## Next Steps

1. **Try development mode first** to get the API running
2. **Test database connectivity** using the troubleshooting steps
3. **Run migration script manually** if you can connect to the database
4. **Contact database provider** if connectivity issues persist

## Production Deployment

For production deployment, ensure:
- Database server is accessible from your deployment environment
- Connection string is properly configured
- Firewall rules allow database connections
- SSL certificates are properly configured if required