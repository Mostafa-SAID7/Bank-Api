# Bank API Project Reorganization Script
# This script reorganizes the project structure for better maintainability

param(
    [switch]$DryRun = $false,
    [switch]$Verbose = $false
)

$ErrorActionPreference = "Stop"

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Bank API Project Reorganization Script" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if ($DryRun) {
    Write-Host "[DRY RUN MODE] No changes will be made" -ForegroundColor Yellow
    Write-Host ""
}

# Get script directory
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$projectRoot = Split-Path -Parent $scriptPath

Write-Host "Project Root: $projectRoot" -ForegroundColor Green
Write-Host ""

# Function to create directory if it doesn't exist
function Ensure-Directory {
    param([string]$Path)
    
    if (-not (Test-Path $Path)) {
        if ($Verbose) { Write-Host "  Creating: $Path" -ForegroundColor Gray }
        if (-not $DryRun) {
            New-Item -ItemType Directory -Path $Path -Force | Out-Null
        }
    }
}

# Function to move file with logging
function Move-FileWithLog {
    param(
        [string]$Source,
        [string]$Destination
    )
    
    if (Test-Path $Source) {
        Write-Host "  Moving: $(Split-Path -Leaf $Source) -> $Destination" -ForegroundColor Yellow
        if (-not $DryRun) {
            Move-Item -Path $Source -Destination $Destination -Force
        }
    } else {
        if ($Verbose) { Write-Host "  Skipping (not found): $Source" -ForegroundColor Gray }
    }
}

# ============================================
# PHASE 1: Create New Folder Structure
# ============================================
Write-Host "Phase 1: Creating new folder structure..." -ForegroundColor Cyan

$newFolders = @(
    # Application Services
    "src/Bank.Application/Services/Account",
    "src/Bank.Application/Services/Auth",
    "src/Bank.Application/Services/Card",
    "src/Bank.Application/Services/Deposit",
    "src/Bank.Application/Services/Loan",
    "src/Bank.Application/Services/Payment",
    "src/Bank.Application/Services/Statement",
    "src/Bank.Application/Services/Transaction",
    "src/Bank.Application/Services/Shared",
    "src/Bank.Application/Services/Background",
    
    # API Controllers
    "src/Bank.Api/Controllers/Account",
    "src/Bank.Api/Controllers/Auth",
    "src/Bank.Api/Controllers/Card",
    "src/Bank.Api/Controllers/Loan",
    "src/Bank.Api/Controllers/Payment",
    "src/Bank.Api/Controllers/Transaction",
    "src/Bank.Api/Controllers/Admin",
    
    # wwwroot organization
    "src/Bank.Api/wwwroot/css",
    "src/Bank.Api/wwwroot/js",
    "src/Bank.Api/wwwroot/images",
    "src/Bank.Api/wwwroot/pages",
    
    # Infrastructure Repositories
    "src/Bank.Infrastructure/Repositories/Account",
    "src/Bank.Infrastructure/Repositories/Card",
    "src/Bank.Infrastructure/Repositories/Loan",
    "src/Bank.Infrastructure/Repositories/Payment",
    "src/Bank.Infrastructure/Repositories/Transaction",
    
    # Tests
    "src/Bank.Tests/Unit/Services",
    "src/Bank.Tests/Integration/Api",
    "src/Bank.Tests/E2E",
    
    # Assets
    "assets/screenshots",
    "assets/images"
)

foreach ($folder in $newFolders) {
    $fullPath = Join-Path $projectRoot $folder
    Ensure-Directory $fullPath
}

Write-Host "✓ Folder structure created" -ForegroundColor Green
Write-Host ""

# ============================================
# PHASE 2: Move Services to Subfolders
# ============================================
Write-Host "Phase 2: Organizing services..." -ForegroundColor Cyan

$servicesPath = Join-Path $projectRoot "src/Bank.Application/Services"

# Account Services
Write-Host "  Account Services..." -ForegroundColor White
$accountServices = @(
    "AccountService.cs",
    "AccountLifecycleService.cs",
    "AccountValidationService.cs",
    "AccountLockoutService.cs",
    "JointAccountService.cs"
)
foreach ($service in $accountServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Account/$service")
}

# Auth Services
Write-Host "  Auth Services..." -ForegroundColor White
$authServices = @(
    "AuthService.cs",
    "TwoFactorAuthService.cs",
    "SessionService.cs",
    "PasswordPolicyService.cs",
    "IpWhitelistService.cs",
    "TokenGenerationService.cs"
)
foreach ($service in $authServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Auth/$service")
}

# Card Services
Write-Host "  Card Services..." -ForegroundColor White
$cardServices = @(
    "CardService.cs",
    "CardNetworkService.cs",
    "PinManagementService.cs"
)
foreach ($service in $cardServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Card/$service")
}

# Deposit Services
Write-Host "  Deposit Services..." -ForegroundColor White
$depositServices = @(
    "DepositService.cs",
    "DepositMaturityService.cs",
    "DepositWithdrawalService.cs",
    "DepositCertificateGenerator.cs",
    "DepositBackgroundService.cs"
)
foreach ($service in $depositServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Deposit/$service")
}

# Loan Services
Write-Host "  Loan Services..." -ForegroundColor White
$loanServices = @(
    "LoanService.cs",
    "LoanAnalyticsService.cs",
    "LoanInterestCalculationService.cs",
    "LoanBackgroundService.cs"
)
foreach ($service in $loanServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Loan/$service")
}

# Payment Services
Write-Host "  Payment Services..." -ForegroundColor White
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
foreach ($service in $paymentServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Payment/$service")
}

# Statement Services
Write-Host "  Statement Services..." -ForegroundColor White
$statementServices = @(
    "StatementService.cs",
    "StatementGenerator.cs"
)
foreach ($service in $statementServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Statement/$service")
}

# Transaction Services
Write-Host "  Transaction Services..." -ForegroundColor White
$transactionServices = @(
    "TransactionService.cs",
    "TransferEligibilityService.cs",
    "FraudDetectionService.cs"
)
foreach ($service in $transactionServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Transaction/$service")
}

# Shared Services
Write-Host "  Shared Services..." -ForegroundColor White
$sharedServices = @(
    "AuditLogService.cs",
    "AuditEventPublisher.cs",
    "NotificationService.cs",
    "ValidationService.cs",
    "CalculationService.cs",
    "InterestCalculationService.cs",
    "FeeCalculationService.cs"
)
foreach ($service in $sharedServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Shared/$service")
}

# Background Services
Write-Host "  Background Services..." -ForegroundColor White
$backgroundServices = @(
    "BillerHealthCheckBackgroundService.cs",
    "PaymentRetryBackgroundService.cs",
    "BatchService.cs",
    "BatchPaymentService.cs"
)
foreach ($service in $backgroundServices) {
    Move-FileWithLog `
        -Source (Join-Path $servicesPath $service) `
        -Destination (Join-Path $servicesPath "Background/$service")
}

Write-Host "✓ Services organized" -ForegroundColor Green
Write-Host ""

# ============================================
# PHASE 3: Move Controllers to Subfolders
# ============================================
Write-Host "Phase 3: Organizing controllers..." -ForegroundColor Cyan

$controllersPath = Join-Path $projectRoot "src/Bank.Api/Controllers"

# Account Controllers
Write-Host "  Account Controllers..." -ForegroundColor White
$accountControllers = @(
    "AccountController.cs",
    "JointAccountController.cs"
)
foreach ($controller in $accountControllers) {
    Move-FileWithLog `
        -Source (Join-Path $controllersPath $controller) `
        -Destination (Join-Path $controllersPath "Account/$controller")
}

# Auth Controllers
Write-Host "  Auth Controllers..." -ForegroundColor White
$authControllers = @(
    "AuthController.cs",
    "TwoFactorAuthController.cs",
    "SessionController.cs",
    "SecurityController.cs"
)
foreach ($controller in $authControllers) {
    Move-FileWithLog `
        -Source (Join-Path $controllersPath $controller) `
        -Destination (Join-Path $controllersPath "Auth/$controller")
}

# Card Controllers
Write-Host "  Card Controllers..." -ForegroundColor White
$cardControllers = @(
    "CardController.cs",
    "PinManagementController.cs"
)
foreach ($controller in $cardControllers) {
    Move-FileWithLog `
        -Source (Join-Path $controllersPath $controller) `
        -Destination (Join-Path $controllersPath "Card/$controller")
}

# Loan Controllers
Write-Host "  Loan Controllers..." -ForegroundColor White
$loanControllers = @(
    "LoansController.cs",
    "LoanAnalyticsController.cs",
    "LoanInterestController.cs"
)
foreach ($controller in $loanControllers) {
    Move-FileWithLog `
        -Source (Join-Path $controllersPath $controller) `
        -Destination (Join-Path $controllersPath "Loan/$controller")
}

# Payment Controllers
Write-Host "  Payment Controllers..." -ForegroundColor White
$paymentControllers = @(
    "BillPaymentController.cs",
    "BillPaymentManagementController.cs",
    "BillerManagementController.cs",
    "BillPresentmentController.cs",
    "RecurringPaymentController.cs",
    "PaymentTemplateController.cs",
    "BeneficiaryController.cs"
)
foreach ($controller in $paymentControllers) {
    Move-FileWithLog `
        -Source (Join-Path $controllersPath $controller) `
        -Destination (Join-Path $controllersPath "Payment/$controller")
}

# Transaction Controllers
Write-Host "  Transaction Controllers..." -ForegroundColor White
$transactionControllers = @(
    "TransactionController.cs",
    "DepositController.cs"
)
foreach ($controller in $transactionControllers) {
    Move-FileWithLog `
        -Source (Join-Path $controllersPath $controller) `
        -Destination (Join-Path $controllersPath "Transaction/$controller")
}

# Admin Controllers
Write-Host "  Admin Controllers..." -ForegroundColor White
$adminControllers = @(
    "AdminController.cs",
    "AuditController.cs",
    "BatchController.cs"
)
foreach ($controller in $adminControllers) {
    Move-FileWithLog `
        -Source (Join-Path $controllersPath $controller) `
        -Destination (Join-Path $controllersPath "Admin/$controller")
}

# Profile/Statement Controllers (keep in root or move to appropriate folder)
Write-Host "  Other Controllers..." -ForegroundColor White
$otherControllers = @(
    "ProfileController.cs",
    "StatementController.cs",
    "NotificationController.cs",
    "InterestCalculationController.cs"
)
foreach ($controller in $otherControllers) {
    Move-FileWithLog `
        -Source (Join-Path $controllersPath $controller) `
        -Destination (Join-Path $controllersPath "Account/$controller")
}

Write-Host "✓ Controllers organized" -ForegroundColor Green
Write-Host ""

# ============================================
# PHASE 4: Reorganize wwwroot
# ============================================
Write-Host "Phase 4: Organizing wwwroot..." -ForegroundColor Cyan

$wwwrootPath = Join-Path $projectRoot "src/Bank.Api/wwwroot"

# Move CSS files
Write-Host "  Moving CSS files..." -ForegroundColor White
if (Test-Path (Join-Path $wwwrootPath "community-car.css")) {
    Move-FileWithLog `
        -Source (Join-Path $wwwrootPath "community-car.css") `
        -Destination (Join-Path $wwwrootPath "css/styles.css")
}

# Move HTML files
Write-Host "  Moving HTML files..." -ForegroundColor White
$htmlFiles = @("Home.html", "Docs.html", "404.html")
foreach ($htmlFile in $htmlFiles) {
    Move-FileWithLog `
        -Source (Join-Path $wwwrootPath $htmlFile) `
        -Destination (Join-Path $wwwrootPath "pages/$htmlFile")
}

# Move images
Write-Host "  Moving images..." -ForegroundColor White
if (Test-Path (Join-Path $wwwrootPath "images")) {
    $imageFiles = Get-ChildItem -Path (Join-Path $wwwrootPath "images") -File
    foreach ($imageFile in $imageFiles) {
        Move-FileWithLog `
            -Source $imageFile.FullName `
            -Destination (Join-Path $wwwrootPath "images/$($imageFile.Name)")
    }
}

Write-Host "✓ wwwroot organized" -ForegroundColor Green
Write-Host ""

# ============================================
# PHASE 5: Move Screenshots
# ============================================
Write-Host "Phase 5: Moving screenshots..." -ForegroundColor Cyan

$screenshotsSource = Join-Path $projectRoot "screenshots"
$screenshotsDestination = Join-Path $projectRoot "assets/screenshots"

if (Test-Path $screenshotsSource) {
    $screenshotFiles = Get-ChildItem -Path $screenshotsSource -File
    Write-Host "  Moving $($screenshotFiles.Count) screenshot files..." -ForegroundColor White
    
    foreach ($screenshot in $screenshotFiles) {
        Move-FileWithLog `
            -Source $screenshot.FullName `
            -Destination (Join-Path $screenshotsDestination $screenshot.Name)
    }
    
    if (-not $DryRun) {
        Remove-Item -Path $screenshotsSource -Force -ErrorAction SilentlyContinue
    }
    Write-Host "✓ Screenshots moved" -ForegroundColor Green
} else {
    Write-Host "  No screenshots folder found" -ForegroundColor Gray
}

Write-Host ""

# ============================================
# PHASE 6: Clean Up DevOps
# ============================================
Write-Host "Phase 6: Cleaning up DevOps files..." -ForegroundColor Cyan

$devopsPath = Join-Path $projectRoot "devops"

# Remove frontend-specific files
Write-Host "  Removing frontend-specific files..." -ForegroundColor White
$frontendFiles = @(
    "docker/Dockerfile.frontend",
    "kubernetes/frontend.yaml"
)

foreach ($file in $frontendFiles) {
    $fullPath = Join-Path $devopsPath $file
    if (Test-Path $fullPath) {
        Write-Host "  Removing: $file" -ForegroundColor Yellow
        if (-not $DryRun) {
            Remove-Item -Path $fullPath -Force
        }
    }
}

Write-Host "✓ DevOps cleaned up" -ForegroundColor Green
Write-Host ""

# ============================================
# Summary
# ============================================
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Reorganization Complete!" -ForegroundColor Green
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

if ($DryRun) {
    Write-Host "[DRY RUN] No actual changes were made" -ForegroundColor Yellow
    Write-Host "Run without -DryRun to apply changes" -ForegroundColor Yellow
} else {
    Write-Host "Next Steps:" -ForegroundColor Cyan
    Write-Host "1. Update namespaces in moved files" -ForegroundColor White
    Write-Host "2. Update using statements" -ForegroundColor White
    Write-Host "3. Update HTML file references (css/js paths)" -ForegroundColor White
    Write-Host "4. Build solution: dotnet build" -ForegroundColor White
    Write-Host "5. Run tests: dotnet test" -ForegroundColor White
    Write-Host "6. Commit changes: git add . && git commit -m 'Reorganize project structure'" -ForegroundColor White
}

Write-Host ""
Write-Host "For detailed information, see REORGANIZATION_PLAN.md" -ForegroundColor Cyan
