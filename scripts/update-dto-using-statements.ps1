#!/usr/bin/env pwsh
# Update all using statements in services and controllers to use new DTO namespaces

$updatedCount = 0

# Define mapping of old generic using statements to new specific ones
$usingMappings = @{
    "using Bank.Application.DTOs.Auth;" = @(
        "using Bank.Application.DTOs.Auth.Core;",
        "using Bank.Application.DTOs.Auth.TwoFactor;",
        "using Bank.Application.DTOs.Auth.Security;",
        "using Bank.Application.DTOs.Auth.Session;"
    )
    "using Bank.Application.DTOs.Card;" = @(
        "using Bank.Application.DTOs.Card.Core;",
        "using Bank.Application.DTOs.Card.Activation;",
        "using Bank.Application.DTOs.Card.Transactions;",
        "using Bank.Application.DTOs.Card.Fees;",
        "using Bank.Application.DTOs.Card.Operations;",
        "using Bank.Application.DTOs.Card.Advanced;"
    )
    "using Bank.Application.DTOs.Deposit;" = @(
        "using Bank.Application.DTOs.Deposit.Core;",
        "using Bank.Application.DTOs.Deposit.FixedDeposit;",
        "using Bank.Application.DTOs.Deposit.Interest;",
        "using Bank.Application.DTOs.Deposit.Maturity;",
        "using Bank.Application.DTOs.Deposit.Withdrawal;"
    )
    "using Bank.Application.DTOs.Loan;" = @(
        "using Bank.Application.DTOs.Loan.Core;",
        "using Bank.Application.DTOs.Loan.Application;",
        "using Bank.Application.DTOs.Loan.Approval;",
        "using Bank.Application.DTOs.Loan.Disbursement;",
        "using Bank.Application.DTOs.Loan.Repayment;",
        "using Bank.Application.DTOs.Loan.Analytics;",
        "using Bank.Application.DTOs.Loan.Configuration;"
    )
    "using Bank.Application.DTOs.Payment;" = @(
        "using Bank.Application.DTOs.Payment.Core;",
        "using Bank.Application.DTOs.Payment.Beneficiary;",
        "using Bank.Application.DTOs.Payment.Biller;",
        "using Bank.Application.DTOs.Payment.Batch;",
        "using Bank.Application.DTOs.Payment.Routing;",
        "using Bank.Application.DTOs.Payment.Receipt;",
        "using Bank.Application.DTOs.Payment.Recurring;",
        "using Bank.Application.DTOs.Payment.Template;"
    )
    "using Bank.Application.DTOs.Statement;" = @(
        "using Bank.Application.DTOs.Statement.Core;",
        "using Bank.Application.DTOs.Statement.Search;",
        "using Bank.Application.DTOs.Statement.Summary;",
        "using Bank.Application.DTOs.Statement.Delivery;",
        "using Bank.Application.DTOs.Statement.Analytics;",
        "using Bank.Application.DTOs.Statement.Transaction;"
    )
    "using Bank.Application.DTOs.Transaction;" = @(
        "using Bank.Application.DTOs.Transaction.Core;",
        "using Bank.Application.DTOs.Transaction.Search;",
        "using Bank.Application.DTOs.Transaction.Analytics;",
        "using Bank.Application.DTOs.Transaction.Fraud;"
    )
    "using Bank.Application.DTOs.Shared;" = @(
        "using Bank.Application.DTOs.Shared.Notification;",
        "using Bank.Application.DTOs.Shared.Audit;",
        "using Bank.Application.DTOs.Shared.RateLimit;"
    )
    "using Bank.Application.DTOs.Account;" = @(
        "using Bank.Application.DTOs.Account.Core;",
        "using Bank.Application.DTOs.Account.Validation;",
        "using Bank.Application.DTOs.Account.Lockout;",
        "using Bank.Application.DTOs.Account.Profile;",
        "using Bank.Application.DTOs.Account.JointAccount;",
        "using Bank.Application.DTOs.Account.Transfer;"
    )
}

# Find all C# files in Services, Controllers, and Mappings
$filesToUpdate = @()
$filesToUpdate += Get-ChildItem -Path "Bank-Api/src/Bank.Application/Services" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue
$filesToUpdate += Get-ChildItem -Path "Bank-Api/src/Bank.Api/Controllers" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue
$filesToUpdate += Get-ChildItem -Path "Bank-Api/src/Bank.Application/Mappings" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue
$filesToUpdate += Get-ChildItem -Path "Bank-Api/src/Bank.Application/Validators" -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue

Write-Host "Found $($filesToUpdate.Count) files to check" -ForegroundColor Cyan

foreach ($file in $filesToUpdate) {
    $content = Get-Content $file.FullName -Raw -Encoding UTF8
    $originalContent = $content
    $hasChanges = $false
    
    foreach ($oldUsing in $usingMappings.Keys) {
        if ($content -match [regex]::Escape($oldUsing)) {
            # Replace old using with all new using statements
            $newUsings = $usingMappings[$oldUsing] -join "`n"
            $content = $content -replace [regex]::Escape($oldUsing), $newUsings
            $hasChanges = $true
        }
    }
    
    if ($hasChanges) {
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8 -Force
        Write-Host "  [OK] Updated: $($file.Name)" -ForegroundColor Green
        $updatedCount++
    }
}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "UPDATE COMPLETE" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Total Files Updated: $updatedCount" -ForegroundColor Green
Write-Host ""
