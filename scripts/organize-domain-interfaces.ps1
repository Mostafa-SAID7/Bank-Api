#!/usr/bin/env pwsh
# Script to organize Domain Interfaces into domain-based subfolders

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
# Domain Interfaces Organization
# ============================================================================

Write-Log "Starting Domain Interfaces organization..." "SUCCESS"

$interfacesBasePath = Join-Path $basePath "src\Bank.Domain\Interfaces"

# Account Repositories
$accountInterfaces = @(
    "IAccountLockoutRepository.cs"
)

# Auth Repositories
$authInterfaces = @(
    "IUserRepository.cs",
    "ISessionRepository.cs",
    "IPasswordHistoryRepository.cs",
    "IPasswordPolicyRepository.cs",
    "IIpWhitelistRepository.cs"
)

# Card Repositories
$cardInterfaces = @(
    "ICardRepository.cs",
    "ICardTransactionRepository.cs"
)

# Loan Repositories
$loanInterfaces = @(
    "ILoanRepository.cs"
)

# Payment Repositories
$paymentInterfaces = @(
    "IBillPaymentRepository.cs",
    "IBillPresentmentRepository.cs",
    "IBillerRepository.cs",
    "IBillerHealthCheckRepository.cs",
    "IBeneficiaryRepository.cs",
    "IRecurringPaymentRepository.cs",
    "IPaymentTemplateRepository.cs",
    "IPaymentRetryRepository.cs",
    "IPaymentReceiptRepository.cs"
)

# Statement Repositories
$statementInterfaces = @(
    "IStatementRepository.cs"
)

# Shared Repositories
$sharedInterfaces = @(
    "IAuditLogRepository.cs"
)

# Core Interfaces (stay at root)
$coreInterfaces = @(
    "IRepository.cs",
    "IUnitOfWork.cs"
)

# Move Account Repositories
foreach ($file in $accountInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Account\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Auth Repositories
foreach ($file in $authInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Auth\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Card Repositories
foreach ($file in $cardInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Card\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Loan Repositories
foreach ($file in $loanInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Loan\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Payment Repositories
foreach ($file in $paymentInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Payment\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Statement Repositories
foreach ($file in $statementInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Statement\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

# Move Shared Repositories
foreach ($file in $sharedInterfaces) {
    $source = Join-Path $interfacesBasePath $file
    $dest = Join-Path $interfacesBasePath "Shared\$file"
    Move-FileToFolder -SourcePath $source -DestinationPath $dest
}

Write-Log "Domain Interfaces organization completed!" "SUCCESS"
Write-Log "Note: IRepository.cs and IUnitOfWork.cs remain at root as core interfaces" "INFO"

# ============================================================================
# Summary
# ============================================================================

if ($DryRun) {
    Write-Log "DRY-RUN MODE: No files were actually moved" "WARN"
    Write-Log "Run without -DryRun flag to apply changes" "INFO"
} else {
    Write-Log "Organization complete! Domain interfaces have been moved to domain folders" "SUCCESS"
    Write-Log "Next steps:" "INFO"
    Write-Log "1. Update namespaces in moved files" "INFO"
    Write-Log "2. Update using statements in dependent files" "INFO"
    Write-Log "3. Run 'dotnet build' to verify" "INFO"
}
