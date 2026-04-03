#!/usr/bin/env pwsh
# Script to organize Entities into domain-based subfolders

param(
    [switch]$DryRun,
    [switch]$Verbose
)

$ErrorActionPreference = "Continue"
$basePath = Split-Path -Parent $PSScriptRoot

function Write-Log {
    param([string]$Message, [string]$Level = "INFO")
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $color = switch ($Level) {
        "ERROR" { "Red" }
        "WARN" { "Yellow" }
        "SUCCESS" { "Green" }
        default { "White" }
    }
    Write-Host "[$timestamp] [$Level] $Message" -ForegroundColor $color
}

function Move-FileToFolder {
    param(
        [string]$SourcePath,
        [string]$DestinationPath
    )
    
    if (-not (Test-Path $SourcePath)) {
        if ($Verbose) { Write-Log "SKIP: Source file not found: $SourcePath" "WARN" }
        return $false
    }
    
    $destDir = Split-Path -Parent $DestinationPath
    if (-not (Test-Path $destDir)) {
        if ($Verbose) { Write-Log "Creating directory: $destDir" }
        if (-not $DryRun) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
    }
    
    if ($DryRun) {
        Write-Log "DRY-RUN: Would move $SourcePath -> $DestinationPath" "INFO"
    } else {
        if ($Verbose) { Write-Log "Moving: $SourcePath -> $DestinationPath" }
        Move-Item -Path $SourcePath -Destination $DestinationPath -Force
    }
    return $true
}

# ============================================================================
# Entities Organization
# ============================================================================

Write-Log "Starting Entities organization..." "SUCCESS"

$entitiesBasePath = Join-Path $basePath "src\Bank.Domain\Entities"

# Account Entities
$accountEntities = @(
    "Account.cs",
    "AccountFee.cs",
    "AccountHold.cs",
    "AccountLockout.cs",
    "AccountRestriction.cs",
    "AccountStatement.cs",
    "AccountStatusHistory.cs",
    "JointAccountHolder.cs"
)

# Auth Entities
$authEntities = @(
    "User.cs",
    "Session.cs",
    "TwoFactorToken.cs",
    "PasswordHistory.cs",
    "PasswordPolicy.cs",
    "IpWhitelist.cs"
)

# Card Entities
$cardEntities = @(
    "Card.cs",
    "CardAuthorization.cs",
    "CardStatement.cs",
    "CardStatusHistory.cs",
    "CardTransaction.cs"
)

# Deposit Entities
$depositEntities = @(
    "DepositProduct.cs",
    "FixedDeposit.cs",
    "DepositTransaction.cs",
    "DepositCertificate.cs",
    "InterestTier.cs",
    "MaturityNotice.cs"
)

# Loan Entities
$loanEntities = @(
    "Loan.cs",
    "LoanDocument.cs",
    "LoanPayment.cs",
    "LoanStatusHistory.cs"
)

# Payment Entities
$paymentEntities = @(
    "BillPayment.cs",
    "BillPresentment.cs",
    "Biller.cs",
    "BillerHealthCheck.cs",
    "Beneficiary.cs",
    "RecurringPayment.cs",
    "PaymentTemplate.cs",
    "PaymentRetry.cs",
    "PaymentReceipt.cs"
)

# Transaction Entities
$transactionEntities = @(
    "Transaction.cs"
)

# Shared Entities
$sharedEntities = @(
    "AuditLog.cs",
    "Notification.cs",
    "NotificationPreference.cs",
    "FeeSchedule.cs",
    "BatchJob.cs"
)

# Move Account Entities
foreach ($file in $accountEntities) {
    $source = Join-Path $entitiesBasePath $file
    $dest = Join-Path $entitiesBasePath "Account\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Auth Entities
foreach ($file in $authEntities) {
    $source = Join-Path $entitiesBasePath $file
    $dest = Join-Path $entitiesBasePath "Auth\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Card Entities
foreach ($file in $cardEntities) {
    $source = Join-Path $entitiesBasePath $file
    $dest = Join-Path $entitiesBasePath "Card\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Deposit Entities
foreach ($file in $depositEntities) {
    $source = Join-Path $entitiesBasePath $file
    $dest = Join-Path $entitiesBasePath "Deposit\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Loan Entities
foreach ($file in $loanEntities) {
    $source = Join-Path $entitiesBasePath $file
    $dest = Join-Path $entitiesBasePath "Loan\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Payment Entities
foreach ($file in $paymentEntities) {
    $source = Join-Path $entitiesBasePath $file
    $dest = Join-Path $entitiesBasePath "Payment\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Transaction Entities
foreach ($file in $transactionEntities) {
    $source = Join-Path $entitiesBasePath $file
    $dest = Join-Path $entitiesBasePath "Transaction\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Shared Entities
foreach ($file in $sharedEntities) {
    $source = Join-Path $entitiesBasePath $file
    $dest = Join-Path $entitiesBasePath "Shared\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

Write-Log "Entities organization completed!" "SUCCESS"

# ============================================================================
# Summary
# ============================================================================

if ($DryRun) {
    Write-Log "DRY-RUN MODE: No files were actually moved" "WARN"
    Write-Log "Run without -DryRun flag to apply changes" "INFO"
} else {
    Write-Log "Organization complete! Entities have been moved to domain folders" "SUCCESS"
    Write-Log "Next steps:" "INFO"
    Write-Log "1. Update namespaces in moved files" "INFO"
    Write-Log "2. Update using statements in dependent files" "INFO"
    Write-Log "3. Run 'dotnet build' to verify" "INFO"
}
