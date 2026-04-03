#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Updates all using statements for Account DTOs to match new subfolder structure
.DESCRIPTION
    This script:
    1. Finds all .cs files in Services and Controllers
    2. Identifies which Account DTO classes are used
    3. Replaces old namespace with correct subfolder namespace
    4. Reports all changes
#>

param(
    [switch]$WhatIf = $false
)

$ErrorActionPreference = "Stop"
$changedFiles = 0
$totalFiles = 0

Write-Host "Starting Account DTO Using Statement Update..." -ForegroundColor Cyan
Write-Host ""

# Mapping of DTO class names to their new namespaces
$dtoNamespaceMap = @{
    # Core
    "AccountDto" = "Bank.Application.DTOs.Account.Core"
    "CreateAccountRequest" = "Bank.Application.DTOs.Account.Core"
    "UpdateAccountRequest" = "Bank.Application.DTOs.Account.Core"
    
    # Validation
    "AccountNumberValidationResult" = "Bank.Application.DTOs.Account.Validation"
    "AccountValidationResult" = "Bank.Application.DTOs.Account.Validation"
    "BankInformationResult" = "Bank.Application.DTOs.Account.Validation"
    "BeneficiaryAccountValidationRequest" = "Bank.Application.DTOs.Account.Validation"
    "ComprehensiveValidationResult" = "Bank.Application.DTOs.Account.Validation"
    "ExternalAccountValidationRequest" = "Bank.Application.DTOs.Account.Validation"
    "IbanValidationResult" = "Bank.Application.DTOs.Account.Validation"
    "RoutingNumberValidationResult" = "Bank.Application.DTOs.Account.Validation"
    "SwiftValidationResult" = "Bank.Application.DTOs.Account.Validation"
    
    # Lockout
    "AccountLockoutInfo" = "Bank.Application.DTOs.Account.Lockout"
    "LockoutResult" = "Bank.Application.DTOs.Account.Lockout"
    "LockoutStatistics" = "Bank.Application.DTOs.Account.Lockout"
    
    # Profile
    "ProfileDto" = "Bank.Application.DTOs.Account.Profile"
    "ProfileResponse" = "Bank.Application.DTOs.Account.Profile"
    "UpdateProfileRequest" = "Bank.Application.DTOs.Account.Profile"
    
    # JointAccount
    "AddJointHolderRequest" = "Bank.Application.DTOs.Account.JointAccount"
    "ConvertToJointAccountRequest" = "Bank.Application.DTOs.Account.JointAccount"
    "ConvertToSingleAccountRequest" = "Bank.Application.DTOs.Account.JointAccount"
    "JointAccountHolderDto" = "Bank.Application.DTOs.Account.JointAccount"
    "JointAccountSummary" = "Bank.Application.DTOs.Account.JointAccount"
    "RemoveJointHolderRequest" = "Bank.Application.DTOs.Account.JointAccount"
    "TransactionPermissionRequest" = "Bank.Application.DTOs.Account.JointAccount"
    "TransactionPermissionResult" = "Bank.Application.DTOs.Account.JointAccount"
    "UpdateJointHolderRequest" = "Bank.Application.DTOs.Account.JointAccount"
    
    # Transfer
    "BeneficiaryLimitsResult" = "Bank.Application.DTOs.Account.Transfer"
    "DailyTransferSummary" = "Bank.Application.DTOs.Account.Transfer"
    "LimitCheckResult" = "Bank.Application.DTOs.Account.Transfer"
    "TransferEligibilityRequest" = "Bank.Application.DTOs.Account.Transfer"
    "TransferEligibilityResult" = "Bank.Application.DTOs.Account.Transfer"
    "TransferHistorySummary" = "Bank.Application.DTOs.Account.Transfer"
    "TransferPreValidationRequest" = "Bank.Application.DTOs.Account.Transfer"
    "TransferPreValidationResult" = "Bank.Application.DTOs.Account.Transfer"
}

# Find all service and controller files
$servicePath = "Bank-Api/src/Bank.Application/Services"
$controllerPath = "Bank-Api/src/Bank.Api/Controllers"

$files = @()
if (Test-Path $servicePath) {
    $files += Get-ChildItem -Path $servicePath -Recurse -File -Filter "*.cs"
}
if (Test-Path $controllerPath) {
    $files += Get-ChildItem -Path $controllerPath -Recurse -File -Filter "*.cs"
}

Write-Host "Found $($files.Count) files to check" -ForegroundColor Yellow
Write-Host ""

foreach ($file in $files) {
    $totalFiles++
    $content = Get-Content $file.FullName -Raw
    $newContent = $content
    $fileChanged = $false
    
    # Check each DTO class
    foreach ($dtoClass in $dtoNamespaceMap.Keys) {
        $newNamespace = $dtoNamespaceMap[$dtoClass]
        
        # Check if this DTO is used in the file
        if ($content -match "\b$dtoClass\b") {
            # Replace old namespace with new one
            $oldUsing = "using Bank.Application.DTOs.Account;"
            $newUsing = "using $newNamespace;"
            
            if ($newContent -match [regex]::Escape($oldUsing)) {
                # Only replace if the new using doesn't already exist
                if (-not ($newContent -match [regex]::Escape($newUsing))) {
                    $newContent = $newContent -replace [regex]::Escape($oldUsing), $newUsing
                    $fileChanged = $true
                }
            }
        }
    }
    
    if ($fileChanged) {
        if (-not $WhatIf) {
            Set-Content -Path $file.FullName -Value $newContent -Encoding UTF8
            $changedFiles++
            Write-Host "✓ Updated: $($file.Name)" -ForegroundColor Green
        } else {
            Write-Host "→ Would update: $($file.Name)" -ForegroundColor Cyan
        }
    }
}

Write-Host ""
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Total Files Checked: $totalFiles" -ForegroundColor White
Write-Host "  Files Updated: $changedFiles" -ForegroundColor Green

if ($WhatIf) {
    Write-Host ""
    Write-Host "WhatIf mode: No changes were made. Run without -WhatIf to apply changes." -ForegroundColor Cyan
}

Write-Host ""
Write-Host "Script completed!" -ForegroundColor Green
