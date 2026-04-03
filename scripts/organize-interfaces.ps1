#!/usr/bin/env pwsh
# Script to organize Interfaces into domain-based subfolders

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
# Interfaces Organization
# ============================================================================

Write-Log "Starting Interfaces organization..." "SUCCESS"

$interfacesBasePath = Join-Path $basePath "src\Bank.Application\Interfaces"

# Account Interfaces
$accountInterfaces = @(
    "IAccountService.cs",
    "IAccountLifecycleService.cs",
    "IAccountValidationService.cs",
    "IAccountLockoutService.cs",
    "IJointAccountService.cs"
)

# Auth Interfaces
$authInterfaces = @(
    "IAuthService.cs",
    "ITwoFactorAuthService.cs",
    "ISessionService.cs",
    "IPasswordPolicyService.cs",
    "IIpWhitelistService.cs",
    "ITokenGenerationService.cs"
)

# Card Interfaces
$cardInterfaces = @(
    "ICardService.cs",
    "ICardNetworkService.cs",
    "IPinManagementService.cs"
)

# Deposit Interfaces
$depositInterfaces = @(
    "IDepositService.cs",
    "IDepositProductRepository.cs"
)

# Loan Interfaces
$loanInterfaces = @(
    "ILoanService.cs",
    "ILoanAnalyticsService.cs",
    "ILoanInterestCalculationService.cs"
)

# Payment Interfaces
$paymentInterfaces = @(
    "IBillPaymentService.cs",
    "IBillPresentmentService.cs",
    "IBillerIntegrationService.cs",
    "IRecurringPaymentService.cs",
    "IPaymentTemplateService.cs",
    "IPaymentRetryService.cs",
    "IPaymentReceiptService.cs",
    "IBeneficiaryService.cs"
)

# Statement Interfaces
$statementInterfaces = @(
    "IStatementService.cs",
    "IStatementGenerator.cs"
)

# Transaction Interfaces
$transactionInterfaces = @(
    "ITransactionService.cs",
    "ITransferEligibilityService.cs",
    "IFraudDetectionService.cs"
)

# Shared Interfaces
$sharedInterfaces = @(
    "IAuditLogService.cs",
    "IAuditEventPublisher.cs",
    "INotificationService.cs",
    "IValidationService.cs",
    "ICalculationService.cs",
    "IInterestCalculationService.cs",
    "IFeeCalculationService.cs",
    "IRateLimitingService.cs",
    "IEmailService.cs",
    "ISmsService.cs"
)

# Background Interfaces
$backgroundInterfaces = @(
    "IBatchService.cs",
    "IBatchPaymentService.cs"
)

# Move Account Interfaces
foreach ($file in $accountInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Account\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Auth Interfaces
foreach ($file in $authInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Auth\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Card Interfaces
foreach ($file in $cardInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Card\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Deposit Interfaces
foreach ($file in $depositInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Deposit\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Loan Interfaces
foreach ($file in $loanInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Loan\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Payment Interfaces
foreach ($file in $paymentInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Payment\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Statement Interfaces
foreach ($file in $statementInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Statement\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Transaction Interfaces
foreach ($file in $transactionInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Transaction\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Shared Interfaces
foreach ($file in $sharedInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Shared\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Background Interfaces
foreach ($file in $backgroundInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Background\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

Write-Log "Interfaces organization completed!" "SUCCESS"

# ============================================================================
# Summary
# ============================================================================

if ($DryRun) {
    Write-Log "DRY-RUN MODE: No files were actually moved" "WARN"
    Write-Log "Run without -DryRun flag to apply changes" "INFO"
} else {
    Write-Log "Organization complete! Interfaces have been moved to domain folders" "SUCCESS"
    Write-Log "Next steps:" "INFO"
    Write-Log "1. Update namespaces in moved files" "INFO"
    Write-Log "2. Update using statements in dependent files" "INFO"
    Write-Log "3. Run 'dotnet build' to verify" "INFO"
}
