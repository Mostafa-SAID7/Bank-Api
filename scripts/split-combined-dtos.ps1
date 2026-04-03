#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Splits combined DTO files into individual files, one class per file
.DESCRIPTION
    This script:
    1. Identifies files with multiple type definitions
    2. Extracts each type into a separate file
    3. Maintains correct namespaces and using statements
    4. Deletes the original combined file
    5. Reports all changes
#>

param(
    [string]$DtoBasePath = "Bank-Api/src/Bank.Application/DTOs",
    [switch]$WhatIf = $false
)

$ErrorActionPreference = "Stop"
$splitFiles = 0
$newFilesCreated = 0
$deletedFiles = 0

Write-Host "Starting DTO File Splitting Script..." -ForegroundColor Cyan
Write-Host "Base Path: $DtoBasePath" -ForegroundColor Cyan
Write-Host ""

# Files that need splitting (from the analysis)
$filesToSplit = @(
    @{ File = "AccountLockoutDTOs.cs"; Folder = "Account"; Types = @("LockoutResult", "LockoutStatistics", "AccountLockoutInfo") },
    @{ File = "ProfileDtos.cs"; Folder = "Account"; Types = @("ProfileDto", "ProfileResponse", "UpdateProfileRequest") },
    @{ File = "AuthDtos.cs"; Folder = "Auth"; Types = @("LoginRequest", "RegisterRequest", "AuthResponse") },
    @{ File = "IpWhitelistDTOs.cs"; Folder = "Auth"; Types = @("IpWhitelistResult", "IpWhitelistStatistics", "IpWhitelistInfo") },
    @{ File = "PasswordPolicyDTOs.cs"; Folder = "Auth"; Types = @("PasswordValidationResult", "PasswordPolicyInfo", "PasswordChangeRequirement") },
    @{ File = "SecurityDTOs.cs"; Folder = "Auth"; Types = @("AddIpWhitelistRequest", "ApproveIpWhitelistRequest", "ValidatePasswordRequest", "GeneratePasswordRequest", "LockAccountRequest") },
    @{ File = "SessionDTOs.cs"; Folder = "Auth"; Types = @("SessionResult", "SessionStatistics", "SessionInfo", "RefreshTokenRequest") },
    @{ File = "PinManagementDTOs.cs"; Folder = "Card"; Types = @("SetPinRequest", "ResetPinRequest", "VerifyPinRequest", "PinOperationResponse", "PinVerificationResult") },
    @{ File = "BillPaymentDTOs.cs"; Folder = "Payment"; Types = @("BillPresentmentStatus", "CreateBillerRequest", "UpdateBillerRequest", "BillerDto", "ScheduleBillPaymentRequest", "UpdateBillPaymentRequest", "BillPaymentDto", "BillPaymentHistoryDto", "BillerSearchRequest", "BillPaymentHistoryRequest", "ScheduleBillPaymentResponse", "ProcessBillPaymentResponse") },
    @{ File = "PaymentTemplateDTOs.cs"; Folder = "Payment"; Types = @("CreatePaymentTemplateRequest", "UpdatePaymentTemplateRequest", "ExecuteTemplateRequest") },
    @{ File = "AuditDTOs.cs"; Folder = "Shared"; Types = @("AuditLogDto", "RecurringPaymentDto", "SessionDto") },
    @{ File = "TransactionDTOs.cs"; Folder = "Transaction"; Types = @("TransactionDto", "CreateTransactionRequest", "TransactionSearchCriteria", "TransactionStatistics", "TransactionSearchRequest") }
)

Write-Host "Files to split: $($filesToSplit.Count)" -ForegroundColor Yellow
Write-Host ""

foreach ($fileInfo in $filesToSplit) {
    $filePath = Join-Path $DtoBasePath $fileInfo.Folder $fileInfo.File
    
    if (-not (Test-Path $filePath)) {
        Write-Host "WARNING: File not found: $filePath" -ForegroundColor Red
        continue
    }
    
    Write-Host "Processing: $($fileInfo.File) ($($fileInfo.Types.Count) types)" -ForegroundColor Yellow
    
    $content = Get-Content $filePath -Raw
    $namespace = "Bank.Application.DTOs.$($fileInfo.Folder)"
    
    # Extract using statements
    $usingStatements = @()
    $usingMatches = [regex]::Matches($content, '^using\s+([^;]+);', 'Multiline')
    foreach ($match in $usingMatches) {
        $usingStatements += $match.Groups[1].Value
    }
    
    # Remove duplicates and sort
    $usingStatements = $usingStatements | Select-Object -Unique | Sort-Object
    
    # For each type, extract and create new file
    foreach ($typeName in $fileInfo.Types) {
        # Extract the type definition
        $typePattern = "(?:public\s+(?:class|record|interface|enum|struct)\s+$([regex]::Escape($typeName))\s*(?:\([^)]*\))?\s*(?::\s*[^{]*)?\s*\{(?:[^{}]|(?:\{[^{}]*\}))*\})"
        
        if ($content -match $typePattern) {
            $typeContent = $matches[0]
            
            # Create new file name
            $newFileName = "$($typeName)Dto.cs"
            if ($typeName -match "Request$|Response$|Result$|Dto$|Info$|Summary$|Statistics$|Enum$") {
                $newFileName = "$typeName.cs"
            }
            
            $newFilePath = Join-Path $DtoBasePath $fileInfo.Folder $newFileName
            
            # Build new file content
            $newFileContent = ""
            foreach ($using in $usingStatements) {
                $newFileContent += "using $using;`n"
            }
            $newFileContent += "`nnamespace $namespace;`n`n"
            $newFileContent += $typeContent + "`n"
            
            if (-not $WhatIf) {
                Set-Content -Path $newFilePath -Value $newFileContent -Encoding UTF8
                $newFilesCreated++
                Write-Host "  ✓ Created: $newFileName" -ForegroundColor Green
            } else {
                Write-Host "  → Would create: $newFileName" -ForegroundColor Cyan
            }
        } else {
            Write-Host "  ✗ Could not extract: $typeName" -ForegroundColor Red
        }
    }
    
    # Delete original file
    if (-not $WhatIf) {
        Remove-Item $filePath -Force
        $deletedFiles++
        Write-Host "  ✓ Deleted: $($fileInfo.File)" -ForegroundColor Green
    } else {
        Write-Host "  → Would delete: $($fileInfo.File)" -ForegroundColor Cyan
    }
    
    $splitFiles++
    Write-Host ""
}

Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Files Processed: $splitFiles" -ForegroundColor White
Write-Host "  New Files Created: $newFilesCreated" -ForegroundColor Green
Write-Host "  Files Deleted: $deletedFiles" -ForegroundColor Green

if ($WhatIf) {
    Write-Host ""
    Write-Host "WhatIf mode: No changes were made. Run without -WhatIf to apply changes." -ForegroundColor Cyan
}

Write-Host ""
Write-Host "Script completed!" -ForegroundColor Green
