# Script to organize EF configuration files into domain-based subfolders
# This script moves configuration files from flat structure to domain-organized structure

param(
    [switch]$DryRun,
    [switch]$Verbose
)

$ErrorActionPreference = "Continue"
$repoRoot = Split-Path -Parent $PSScriptRoot
$configurationsPath = Join-Path $repoRoot "src\Bank.Infrastructure\Data\Configurations"

function Write-Log {
    param([string]$Message, [string]$Level = "INFO")
    $timestamp = Get-Date -Format "yyyy-MM-dd HH:mm:ss"
    $color = switch ($Level) {
        "ERROR" { "Red" }
        "WARNING" { "Yellow" }
        "SUCCESS" { "Green" }
        default { "White" }
    }
    Write-Host "[$timestamp] [$Level] $Message" -ForegroundColor $color
}

function Ensure-Directory {
    param([string]$Path)
    if (-not (Test-Path $Path)) {
        if ($Verbose) { Write-Log "Creating directory: $Path" }
        if (-not $DryRun) {
            New-Item -ItemType Directory -Path $Path -Force | Out-Null
        }
    }
}

function Move-ConfigurationFile {
    param(
        [string]$FileName,
        [string]$Domain
    )
    
    $sourcePath = Join-Path $configurationsPath $FileName
    $targetDir = Join-Path $configurationsPath $Domain
    $targetPath = Join-Path $targetDir $FileName
    
    if (Test-Path $sourcePath) {
        if ($Verbose) { Write-Log "Moving $FileName to $Domain/" }
        
        Ensure-Directory $targetDir
        
        if (-not $DryRun) {
            Move-Item -Path $sourcePath -Destination $targetPath -Force
            
            # Update namespace in the moved file
            $content = Get-Content $targetPath -Raw
            $content = $content -replace 'namespace Bank\.Infrastructure\.Data\.Configurations;', "namespace Bank.Infrastructure.Data.Configurations.$Domain;"
            Set-Content -Path $targetPath -Value $content -NoNewline
            
            Write-Log "Moved and updated namespace: $FileName -> $Domain/" "SUCCESS"
        } else {
            Write-Log "[DRY RUN] Would move: $FileName -> $Domain/" "INFO"
        }
    } else {
        Write-Log "File not found (skipping): $FileName" "WARNING"
    }
}

# Domain mappings
$accountConfigurations = @(
    "AccountConfiguration.cs"
)

$authConfigurations = @(
    "IdentityConfiguration.cs"
)

$cardConfigurations = @(
    "CardConfiguration.cs",
    "CardStatusHistoryConfiguration.cs",
    "CardTransactionConfiguration.cs"
)

$depositConfigurations = @(
    "DepositCertificateConfiguration.cs",
    "DepositProductConfiguration.cs",
    "DepositTransactionConfiguration.cs",
    "FixedDepositConfiguration.cs",
    "InterestTierConfiguration.cs"
)

$loanConfigurations = @(
    "LoanConfiguration.cs"
)

$paymentConfigurations = @(
    "BeneficiaryConfiguration.cs",
    "BillPaymentConfiguration.cs"
)

$statementConfigurations = @(
    "AccountStatementConfiguration.cs"
)

$transactionConfigurations = @(
    "TransactionConfiguration.cs"
)

$sharedConfigurations = @(
    "AuditConfiguration.cs",
    "NotificationConfiguration.cs",
    "NotificationPreferenceConfiguration.cs",
    "SystemConfiguration.cs"
)

Write-Log "Starting configuration organization..." "INFO"
Write-Log "Configuration path: $configurationsPath" "INFO"

if ($DryRun) {
    Write-Log "DRY RUN MODE - No files will be moved" "WARNING"
}

# Move Account configurations
Write-Log "Processing Account configurations..." "INFO"
foreach ($file in $accountConfigurations) {
    Move-ConfigurationFile -FileName $file -Domain "Account"
}

# Move Auth configurations
Write-Log "Processing Auth configurations..." "INFO"
foreach ($file in $authConfigurations) {
    Move-ConfigurationFile -FileName $file -Domain "Auth"
}

# Move Card configurations
Write-Log "Processing Card configurations..." "INFO"
foreach ($file in $cardConfigurations) {
    Move-ConfigurationFile -FileName $file -Domain "Card"
}

# Move Deposit configurations
Write-Log "Processing Deposit configurations..." "INFO"
foreach ($file in $depositConfigurations) {
    Move-ConfigurationFile -FileName $file -Domain "Deposit"
}

# Move Loan configurations
Write-Log "Processing Loan configurations..." "INFO"
foreach ($file in $loanConfigurations) {
    Move-ConfigurationFile -FileName $file -Domain "Loan"
}

# Move Payment configurations
Write-Log "Processing Payment configurations..." "INFO"
foreach ($file in $paymentConfigurations) {
    Move-ConfigurationFile -FileName $file -Domain "Payment"
}

# Move Statement configurations
Write-Log "Processing Statement configurations..." "INFO"
foreach ($file in $statementConfigurations) {
    Move-ConfigurationFile -FileName $file -Domain "Statement"
}

# Move Transaction configurations
Write-Log "Processing Transaction configurations..." "INFO"
foreach ($file in $transactionConfigurations) {
    Move-ConfigurationFile -FileName $file -Domain "Transaction"
}

# Move Shared configurations
Write-Log "Processing Shared configurations..." "INFO"
foreach ($file in $sharedConfigurations) {
    Move-ConfigurationFile -FileName $file -Domain "Shared"
}

Write-Log "Configuration organization complete!" "SUCCESS"
Write-Log "Note: SecurityConfiguration.cs and PaymentConfiguration.cs contain multiple configurations and need to be split manually" "WARNING"

if ($DryRun) {
    Write-Log "DRY RUN COMPLETE - No changes were made" "WARNING"
}
