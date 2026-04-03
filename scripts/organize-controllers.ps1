# Script to organize controller files into domain-based subfolders
# This script moves controller files from flat structure to domain-organized structure

param(
    [switch]$DryRun,
    [switch]$Verbose
)

$ErrorActionPreference = "Continue"
$repoRoot = Split-Path -Parent $PSScriptRoot
$controllersPath = Join-Path $repoRoot "src\Bank.Api\Controllers"

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

function Move-ControllerFile {
    param(
        [string]$FileName,
        [string]$Domain
    )
    
    $sourcePath = Join-Path $controllersPath $FileName
    $targetDir = Join-Path $controllersPath $Domain
    $targetPath = Join-Path $targetDir $FileName
    
    if (Test-Path $sourcePath) {
        if ($Verbose) { Write-Log "Moving $FileName to $Domain/" }
        
        Ensure-Directory $targetDir
        
        if (-not $DryRun) {
            Move-Item -Path $sourcePath -Destination $targetPath -Force
            
            # Update namespace in the moved file
            $content = Get-Content $targetPath -Raw
            $content = $content -replace 'namespace Bank\.Api\.Controllers;', "namespace Bank.Api.Controllers.$Domain;"
            Set-Content -Path $targetPath -Value $content -NoNewline
            
            Write-Log "Moved and updated namespace: $FileName -> $Domain/" "SUCCESS"
        } else {
            Write-Log "[DRY RUN] Would move: $FileName -> $Domain/" "INFO"
        }
    } else {
        Write-Log "File not found (skipping): $FileName" "WARNING"
    }
}

# Domain mappings based on requirements
$accountControllers = @(
    "AccountController.cs",
    "JointAccountController.cs",
    "ProfileController.cs",
    "StatementController.cs",
    "NotificationController.cs",
    "InterestCalculationController.cs"
)

$authControllers = @(
    "AuthController.cs",
    "TwoFactorAuthController.cs",
    "SessionController.cs",
    "SecurityController.cs"
)

$cardControllers = @(
    "CardController.cs",
    "PinManagementController.cs"
)

$loanControllers = @(
    "LoansController.cs",
    "LoanAnalyticsController.cs",
    "LoanInterestController.cs"
)

$paymentControllers = @(
    "BillPaymentController.cs",
    "BillPaymentManagementController.cs",
    "BillerManagementController.cs",
    "BillPresentmentController.cs",
    "RecurringPaymentController.cs",
    "PaymentTemplateController.cs",
    "BeneficiaryController.cs"
)

$transactionControllers = @(
    "TransactionController.cs",
    "DepositController.cs"
)

$adminControllers = @(
    "AdminController.cs",
    "AuditController.cs",
    "BatchController.cs"
)

Write-Log "Starting controller organization..." "INFO"
Write-Log "Controllers path: $controllersPath" "INFO"

if ($DryRun) {
    Write-Log "DRY RUN MODE - No files will be moved" "WARNING"
}

# Move Account controllers
Write-Log "Processing Account controllers..." "INFO"
foreach ($file in $accountControllers) {
    Move-ControllerFile -FileName $file -Domain "Account"
}

# Move Auth controllers
Write-Log "Processing Auth controllers..." "INFO"
foreach ($file in $authControllers) {
    Move-ControllerFile -FileName $file -Domain "Auth"
}

# Move Card controllers
Write-Log "Processing Card controllers..." "INFO"
foreach ($file in $cardControllers) {
    Move-ControllerFile -FileName $file -Domain "Card"
}

# Move Loan controllers
Write-Log "Processing Loan controllers..." "INFO"
foreach ($file in $loanControllers) {
    Move-ControllerFile -FileName $file -Domain "Loan"
}

# Move Payment controllers
Write-Log "Processing Payment controllers..." "INFO"
foreach ($file in $paymentControllers) {
    Move-ControllerFile -FileName $file -Domain "Payment"
}

# Move Transaction controllers
Write-Log "Processing Transaction controllers..." "INFO"
foreach ($file in $transactionControllers) {
    Move-ControllerFile -FileName $file -Domain "Transaction"
}

# Move Admin controllers
Write-Log "Processing Admin controllers..." "INFO"
foreach ($file in $adminControllers) {
    Move-ControllerFile -FileName $file -Domain "Admin"
}

Write-Log "Controller organization complete!" "SUCCESS"

if ($DryRun) {
    Write-Log "DRY RUN COMPLETE - No changes were made" "WARNING"
}
