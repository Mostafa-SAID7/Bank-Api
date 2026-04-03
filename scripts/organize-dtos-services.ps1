#!/usr/bin/env pwsh
# Script to organize DTOs and Services into domain-based subfolders

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
# DTOs Organization
# ============================================================================

Write-Log "Starting DTOs organization..." "SUCCESS"

$dtosBasePath = Join-Path $basePath "src\Bank.Application\DTOs"

# Account DTOs
$accountDtos = @(
    "AccountDtos.cs",
    "AccountLockoutDTOs.cs",
    "AccountValidationDTOs.cs",
    "JointAccountDTOs.cs",
    "ProfileDtos.cs"
)

# Auth DTOs
$authDtos = @(
    "AuthDtos.cs",
    "TwoFactorDTOs.cs",
    "SessionDTOs.cs",
    "PasswordPolicyDTOs.cs",
    "IpWhitelistDTOs.cs",
    "SecurityDTOs.cs"
)

# Card DTOs
$cardDtos = @(
    "CardDtos.cs",
    "PinManagementDTOs.cs"
)

# Deposit DTOs
$depositDtos = @(
    "DepositDTOs.cs",
    "InterestCalculationDTOs.cs"
)

# Loan DTOs
$loanDtos = @(
    "LoanDtos.cs"
)

# Payment DTOs
$paymentDtos = @(
    "BillPaymentDTOs.cs",
    "BillerIntegrationDTOs.cs",
    "BeneficiaryDTOs.cs",
    "RecurringPaymentDTOs.cs",
    "PaymentTemplateDTOs.cs"
)

# Statement DTOs
$statementDtos = @(
    "StatementDTOs.cs"
)

# Transaction DTOs
$transactionDtos = @(
    "TransactionDTOs.cs",
    "FraudAnalysisResult.cs",
    "FraudRiskScore.cs",
    "FraudRule.cs",
    "SuspiciousActivityReport.cs"
)

# Shared DTOs
$sharedDtos = @(
    "AuditDTOs.cs",
    "NotificationDTOs.cs",
    "RateLimitPolicy.cs",
    "RateLimitResult.cs",
    "RateLimitStatus.cs"
)

# Move Account DTOs
foreach ($file in $accountDtos) {
    $source = Join-Path $dtosBasePath $file
    $dest = Join-Path $dtosBasePath "Account\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Auth DTOs
foreach ($file in $authDtos) {
    $source = Join-Path $dtosBasePath $file
    $dest = Join-Path $dtosBasePath "Auth\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Card DTOs
foreach ($file in $cardDtos) {
    $source = Join-Path $dtosBasePath $file
    $dest = Join-Path $dtosBasePath "Card\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Deposit DTOs
foreach ($file in $depositDtos) {
    $source = Join-Path $dtosBasePath $file
    $dest = Join-Path $dtosBasePath "Deposit\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Loan DTOs
foreach ($file in $loanDtos) {
    $source = Join-Path $dtosBasePath $file
    $dest = Join-Path $dtosBasePath "Loan\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Payment DTOs
foreach ($file in $paymentDtos) {
    $source = Join-Path $dtosBasePath $file
    $dest = Join-Path $dtosBasePath "Payment\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Statement DTOs
foreach ($file in $statementDtos) {
    $source = Join-Path $dtosBasePath $file
    $dest = Join-Path $dtosBasePath "Statement\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Transaction DTOs
foreach ($file in $transactionDtos) {
    $source = Join-Path $dtosBasePath $file
    $dest = Join-Path $dtosBasePath "Transaction\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Shared DTOs
foreach ($file in $sharedDtos) {
    $source = Join-Path $dtosBasePath $file
    $dest = Join-Path $dtosBasePath "Shared\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

Write-Log "DTOs organization completed!" "SUCCESS"

# ============================================================================
# Services Organization
# ============================================================================

Write-Log "Starting Services organization..." "SUCCESS"

$servicesBasePath = Join-Path $basePath "src\Bank.Application\Services"

# Account Services
$accountServices = @(
    "AccountService.cs",
    "AccountLifecycleService.cs",
    "AccountValidationService.cs",
    "AccountLockoutService.cs",
    "JointAccountService.cs"
)

# Auth Services
$authServices = @(
    "AuthService.cs",
    "TwoFactorAuthService.cs",
    "SessionService.cs",
    "PasswordPolicyService.cs",
    "IpWhitelistService.cs",
    "TokenGenerationService.cs"
)

# Card Services
$cardServices = @(
    "CardService.cs",
    "CardNetworkService.cs",
    "PinManagementService.cs"
)

# Deposit Services
$depositServices = @(
    "DepositService.cs",
    "DepositMaturityService.cs",
    "DepositWithdrawalService.cs",
    "DepositCertificateGenerator.cs",
    "DepositBackgroundService.cs"
)

# Loan Services
$loanServices = @(
    "LoanService.cs",
    "LoanAnalyticsService.cs",
    "LoanInterestCalculationService.cs",
    "LoanBackgroundService.cs"
)

# Payment Services
$paymentServices = @(
    "BillPaymentService.cs",
    "BillPresentmentService.cs",
    "BillerIntegrationService.cs",
    "RecurringPaymentService.cs",
    "PaymentTemplateService.cs",
    "PaymentRetryService.cs",
    "PaymentReceiptService.cs",
    "BillPaymentBackgroundService.cs",
    "BeneficiaryService.cs"
)

# Statement Services
$statementServices = @(
    "StatementService.cs",
    "StatementGenerator.cs"
)

# Transaction Services
$transactionServices = @(
    "TransactionService.cs",
    "TransferEligibilityService.cs",
    "FraudDetectionService.cs"
)

# Shared Services
$sharedServices = @(
    "AuditLogService.cs",
    "AuditEventPublisher.cs",
    "NotificationService.cs",
    "ValidationService.cs",
    "CalculationService.cs",
    "InterestCalculationService.cs",
    "FeeCalculationService.cs"
)

# Background Services
$backgroundServices = @(
    "BillerHealthCheckBackgroundService.cs",
    "PaymentRetryBackgroundService.cs",
    "BatchService.cs",
    "BatchPaymentService.cs"
)

# Move Account Services
foreach ($file in $accountServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Account\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Auth Services
foreach ($file in $authServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Auth\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Card Services
foreach ($file in $cardServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Card\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Deposit Services
foreach ($file in $depositServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Deposit\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Loan Services
foreach ($file in $loanServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Loan\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Payment Services
foreach ($file in $paymentServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Payment\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Statement Services
foreach ($file in $statementServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Statement\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Transaction Services
foreach ($file in $transactionServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Transaction\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Shared Services
foreach ($file in $sharedServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Shared\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Background Services
foreach ($file in $backgroundServices) {
    $source = Join-Path $servicesBasePath $file
    $dest = Join-Path $servicesBasePath "Background\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

Write-Log "Services organization completed!" "SUCCESS"

# ============================================================================
# Summary
# ============================================================================

if ($DryRun) {
    Write-Log "DRY-RUN MODE: No files were actually moved" "WARN"
    Write-Log "Run without -DryRun flag to apply changes" "INFO"
} else {
    Write-Log "Organization complete! Files have been moved to domain folders" "SUCCESS"
    Write-Log "Next steps:" "INFO"
    Write-Log "1. Update namespaces in moved files" "INFO"
    Write-Log "2. Update using statements in dependent files" "INFO"
    Write-Log "3. Run 'dotnet build' to verify" "INFO"
}
