@echo off
echo Applying migrations to local database...

set ASPNETCORE_ENVIRONMENT=Development

echo Checking if LocalDB is available...
sqllocaldb info mssqllocaldb

if %errorlevel% neq 0 (
    echo Creating LocalDB instance...
    sqllocaldb create mssqllocaldb
    sqllocaldb start mssqllocaldb
)

echo Applying EF Core migrations...
dotnet ef database update --project Bank.Infrastructure --startup-project Bank.Api --verbose

if %errorlevel% equ 0 (
    echo.
    echo ✅ Database migrations applied successfully!
    echo Database: BankingSystemDb
    echo Server: (localdb)\mssqllocaldb
    echo.
    echo You can now run: dotnet run --project Bank.Api
) else (
    echo.
    echo ❌ Migration failed. Check the error messages above.
)

pause