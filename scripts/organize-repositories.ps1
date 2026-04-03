# Script to organize repository files into domain-based subfolders
# This script moves repository files from flat structure to domain-organized structure

param(
    [switch]$DryRun,
    [switch]$Verbose
)

$ErrorActionPreference = "Continue"
$repoRoot = Split-Path -Parent $PSScriptRoot
$repositoriesPath = Join-Path $repoRoot "src\Bank.Infrastructure\Repositories"

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

function Move-RepositoryFile {
    param(
        [string]$FileName,
        [string]$Domain
    )
    
    $sourcePath = Join-Path $repositoriesPath $FileName
    $targetDir = Join-Path $repositoriesPath $Domain
    $targetPath = Join-Path $targetDir $FileName
    
    if (Test-Path $sourcePath) {
        if ($Verbose) { Write-Log "Moving $FileName to $Domain/" }
        
        Ensure-Directory $targetDir
        
        if (-not $DryRun) {
            Move-Item -Path $sourcePath -Destination $targetPath -Force
            
            # Update namespace in the moved file
            $content = Get-Content $targetPath -Raw
            $content = $content -replace 'namespace Bank\.Infrastructure\.Repositories;', "namespace Bank.Infrastructure.Repositories.$Domain;"
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
$accountRepositories = @(
    "AccountLockoutRepository.cs"
)

$authRepositories = @(
    "IpWhitelistRepository.cs",
    "PasswordHistoryRepository.cs",
    "PasswordPolicyRepository.cs",
    "SessionRepository.cs",
    "UserRepository.cs"
)

$cardRepositories = @(
    "CardRepository.cs",
    "CardTransactionRepository.cs"
)

$depositRepositories = @(
    "DepositCertificateRepository.cs",
    "DepositProductRepository.cs",
    "FixedDepositRepository.cs",
    "InterestTierRepository.cs"
)

$loanRepositories = @(
    "LoanRepository.cs"
)

$paymentRepositories = @(
    "BeneficiaryRepository.cs",
    "BillerHealthCheckRepository.cs",
    "BillerRepository.cs",
    "BillPaymentRepository.cs",
    "BillPresentmentRepository.cs",
    "PaymentReceiptRepository.cs",
    "PaymentRetryRepository.cs",
    "PaymentTemplateRepository.cs",
    "RecurringPaymentRepository.cs"
)

$statementRepositories = @(
    "StatementRepository.cs"
)

$sharedRepositories = @(
    "AuditLogRepository.cs"
)

Write-Log "Starting repository organization..." "INFO"
Write-Log "Repository path: $repositoriesPath" "INFO"

if ($DryRun) {
    Write-Log "DRY RUN MODE - No files will be moved" "WARNING"
}

# Move Account repositories
Write-Log "Processing Account repositories..." "INFO"
foreach ($file in $accountRepositories) {
    Move-RepositoryFile -FileName $file -Domain "Account"
}

# Move Auth repositories
Write-Log "Processing Auth repositories..." "INFO"
foreach ($file in $authRepositories) {
    Move-RepositoryFile -FileName $file -Domain "Auth"
}

# Move Card repositories
Write-Log "Processing Card repositories..." "INFO"
foreach ($file in $cardRepositories) {
    Move-RepositoryFile -FileName $file -Domain "Card"
}

# Move Deposit repositories
Write-Log "Processing Deposit repositories..." "INFO"
foreach ($file in $depositRepositories) {
    Move-RepositoryFile -FileName $file -Domain "Deposit"
}

# Move Loan repositories
Write-Log "Processing Loan repositories..." "INFO"
foreach ($file in $loanRepositories) {
    Move-RepositoryFile -FileName $file -Domain "Loan"
}

# Move Payment repositories
Write-Log "Processing Payment repositories..." "INFO"
foreach ($file in $paymentRepositories) {
    Move-RepositoryFile -FileName $file -Domain "Payment"
}

# Move Statement repositories
Write-Log "Processing Statement repositories..." "INFO"
foreach ($file in $statementRepositories) {
    Move-RepositoryFile -FileName $file -Domain "Statement"
}

# Move Shared repositories
Write-Log "Processing Shared repositories..." "INFO"
foreach ($file in $sharedRepositories) {
    Move-RepositoryFile -FileName $file -Domain "Shared"
}

Write-Log "Repository organization complete!" "SUCCESS"
Write-Log "Note: Repository.cs and UnitOfWork.cs remain at root level (base classes)" "INFO"

if ($DryRun) {
    Write-Log "DRY RUN COMPLETE - No changes were made" "WARNING"
}
