# PowerShell script to apply migrations to local database
Write-Host "Applying migrations to local database..." -ForegroundColor Green

# Set environment to use local database
$env:ASPNETCORE_ENVIRONMENT = "Development"

# Navigate to the API project directory
Set-Location "Bank.Api"

try {
    Write-Host "Checking EF Core tools..." -ForegroundColor Yellow
    dotnet tool list --global | Select-String "dotnet-ef"
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Installing EF Core tools..." -ForegroundColor Yellow
        dotnet tool install --global dotnet-ef
    }

    Write-Host "Creating/updating database..." -ForegroundColor Yellow
    dotnet ef database update --verbose

    if ($LASTEXITCODE -eq 0) {
        Write-Host "Database migrations applied successfully!" -ForegroundColor Green
        Write-Host "Database: BankingSystemDb" -ForegroundColor Cyan
        Write-Host "Server: (localdb)\mssqllocaldb" -ForegroundColor Cyan
    } else {
        Write-Host "Migration failed. Check the error messages above." -ForegroundColor Red
    }
}
catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}
finally {
    Set-Location ".."
}

Write-Host ""
Write-Host "You can now run the application with:" -ForegroundColor Green
Write-Host "dotnet run --project Bank.Api" -ForegroundColor Cyan