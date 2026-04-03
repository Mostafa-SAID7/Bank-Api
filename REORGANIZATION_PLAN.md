# Bank API Backend - Project Reorganization Plan

## 🎯 Objectives
1. Break down large service files (60KB+) into smaller, focused modules
2. Organize files into logical subfolder structures
3. Improve maintainability and code navigation
4. Follow SOLID principles and separation of concerns
5. Clean up unnecessary files and folders

---

## 📊 Current Issues Identified

### Large Service Files (Need Refactoring)
| File | Size | Action Required |
|------|------|----------------|
| DepositService.cs | 60.5 KB | Split into multiple services |
| CardNetworkService.cs | 43 KB | Split into network-specific handlers |
| CardService.cs | 38.7 KB | Split into card operations modules |
| StatementGenerator.cs | 34.5 KB | Split by statement types |
| StatementService.cs | 34.1 KB | Split into generation/delivery |
| AccountLifecycleService.cs | 30.7 KB | Split by lifecycle stages |
| AccountValidationService.cs | 28.3 KB | Split by validation types |
| LoanService.cs | 26.8 KB | Split into loan operations |

### Folders to Clean Up
- `screenshots/` - 40+ frontend screenshots (not needed for API backend)
- `devops/kubernetes/frontend.yaml` - Frontend config (API only)
- `devops/docker/Dockerfile.frontend` - Frontend dockerfile (API only)

---

## 🗂️ New Folder Structure

```
Bank-Api/
├── .github/                          # GitHub workflows (keep as is)
├── .kiro/                            # Kiro configuration (keep as is)
├── docs/                             # Documentation
│   ├── api/                          # NEW: API documentation
│   ├── architecture/                 # NEW: Architecture diagrams
│   ├── deployment/                   # NEW: Deployment guides
│   └── [existing docs]
├── devops/                           # DevOps configurations
│   ├── docker/
│   │   ├── backend/                  # NEW: Backend-specific
│   │   └── database/                 # NEW: Database-specific
│   ├── kubernetes/
│   │   ├── backend/                  # NEW: Backend K8s configs
│   │   └── database/                 # NEW: Database K8s configs
│   ├── monitoring/
│   ├── scripts/
│   └── terraform/
├── assets/                           # NEW: Project assets
│   ├── images/                       # Logo, icons
│   └── screenshots/                  # Move screenshots here
├── src/
│   ├── Bank.Api/                     # Web API Layer
│   │   ├── Controllers/
│   │   │   ├── Account/              # NEW: Account controllers
│   │   │   ├── Auth/                 # NEW: Auth controllers
│   │   │   ├── Card/                 # NEW: Card controllers
│   │   │   ├── Loan/                 # NEW: Loan controllers
│   │   │   ├── Payment/              # NEW: Payment controllers
│   │   │   └── Transaction/          # NEW: Transaction controllers
│   │   ├── Extensions/
│   │   ├── Middleware/
│   │   ├── Filters/                  # NEW: Action filters
│   │   ├── wwwroot/
│   │   │   ├── css/                  # NEW: Stylesheets
│   │   │   ├── js/                   # NEW: Scripts
│   │   │   ├── images/               # NEW: Images
│   │   │   └── pages/                # NEW: HTML pages
│   │   └── [config files]
│   ├── Bank.Application/             # Business Logic Layer
│   │   ├── Commands/
│   │   ├── Queries/                  # NEW: CQRS Queries
│   │   ├── DTOs/
│   │   ├── Services/
│   │   │   ├── Account/              # NEW: Account services
│   │   │   │   ├── AccountService.cs
│   │   │   │   ├── AccountLifecycleService.cs
│   │   │   │   ├── AccountValidationService.cs
│   │   │   │   └── AccountLockoutService.cs
│   │   │   ├── Auth/                 # NEW: Authentication services
│   │   │   │   ├── AuthService.cs
│   │   │   │   ├── TwoFactorAuthService.cs
│   │   │   │   ├── SessionService.cs
│   │   │   │   └── PasswordPolicyService.cs
│   │   │   ├── Card/                 # NEW: Card services
│   │   │   │   ├── CardService.cs
│   │   │   │   ├── CardNetworkService.cs
│   │   │   │   ├── PinManagementService.cs
│   │   │   │   └── CardValidationService.cs
│   │   │   ├── Deposit/              # NEW: Deposit services
│   │   │   │   ├── DepositService.cs
│   │   │   │   ├── DepositMaturityService.cs
│   │   │   │   ├── DepositWithdrawalService.cs
│   │   │   │   ├── DepositCertificateGenerator.cs
│   │   │   │   └── DepositBackgroundService.cs
│   │   │   ├── Loan/                 # NEW: Loan services
│   │   │   │   ├── LoanService.cs
│   │   │   │   ├── LoanAnalyticsService.cs
│   │   │   │   ├── LoanInterestCalculationService.cs
│   │   │   │   └── LoanBackgroundService.cs
│   │   │   ├── Payment/              # NEW: Payment services
│   │   │   │   ├── BillPaymentService.cs
│   │   │   │   ├── BillPresentmentService.cs
│   │   │   │   ├── BillerIntegrationService.cs
│   │   │   │   ├── RecurringPaymentService.cs
│   │   │   │   ├── PaymentTemplateService.cs
│   │   │   │   ├── PaymentRetryService.cs
│   │   │   │   ├── PaymentReceiptService.cs
│   │   │   │   └── BillPaymentBackgroundService.cs
│   │   │   ├── Statement/            # NEW: Statement services
│   │   │   │   ├── StatementService.cs
│   │   │   │   └── StatementGenerator.cs
│   │   │   ├── Transaction/          # NEW: Transaction services
│   │   │   │   ├── TransactionService.cs
│   │   │   │   ├── TransferEligibilityService.cs
│   │   │   │   └── FraudDetectionService.cs
│   │   │   ├── Shared/               # NEW: Shared services
│   │   │   │   ├── AuditLogService.cs
│   │   │   │   ├── NotificationService.cs
│   │   │   │   ├── ValidationService.cs
│   │   │   │   ├── CalculationService.cs
│   │   │   │   ├── InterestCalculationService.cs
│   │   │   │   ├── FeeCalculationService.cs
│   │   │   │   └── TokenGenerationService.cs
│   │   │   └── Background/           # NEW: Background services
│   │   │       ├── BillerHealthCheckBackgroundService.cs
│   │   │       └── PaymentRetryBackgroundService.cs
│   │   ├── Validators/
│   │   ├── Utilities/
│   │   └── EventHandlers/
│   ├── Bank.Domain/                  # Domain Layer
│   │   ├── Entities/
│   │   │   ├── Account/              # NEW: Account entities
│   │   │   ├── Auth/                 # NEW: Auth entities
│   │   │   ├── Card/                 # NEW: Card entities
│   │   │   ├── Loan/                 # NEW: Loan entities
│   │   │   ├── Payment/              # NEW: Payment entities
│   │   │   └── Transaction/          # NEW: Transaction entities
│   │   ├── Enums/
│   │   ├── ValueObjects/
│   │   ├── Events/
│   │   └── Interfaces/
│   ├── Bank.Infrastructure/          # Infrastructure Layer
│   │   ├── Data/
│   │   ├── Migrations/
│   │   ├── Repositories/
│   │   │   ├── Account/              # NEW: Account repos
│   │   │   ├── Card/                 # NEW: Card repos
│   │   │   ├── Loan/                 # NEW: Loan repos
│   │   │   ├── Payment/              # NEW: Payment repos
│   │   │   └── Transaction/          # NEW: Transaction repos
│   │   └── Services/
│   │       ├── Email/                # NEW: Email services
│   │       ├── Sms/                  # NEW: SMS services
│   │       └── External/             # NEW: External integrations
│   └── Bank.Tests/                   # Test Layer
│       ├── Unit/                     # NEW: Unit tests
│       │   ├── Services/
│       │   ├── Validators/
│       │   └── Utilities/
│       ├── Integration/              # NEW: Integration tests
│       │   ├── Api/
│       │   ├── Database/
│       │   └── Services/
│       └── E2E/                      # NEW: End-to-end tests
└── [root files]
```

---

## 🔨 Refactoring Strategy

### Phase 1: Service Decomposition

#### 1.1 DepositService.cs (60.5 KB) → Split into:
```
Services/Deposit/
├── DepositService.cs                 # Core deposit operations (15 KB)
├── DepositInterestService.cs         # Interest calculations (12 KB)
├── DepositMaturityService.cs         # Maturity handling (12 KB)
├── DepositRenewalService.cs          # Auto-renewal logic (10 KB)
├── DepositWithdrawalService.cs       # Withdrawal operations (8 KB)
└── DepositCertificateGenerator.cs    # Certificate generation (3 KB)
```

#### 1.2 CardNetworkService.cs (43 KB) → Split into:
```
Services/Card/Network/
├── CardNetworkService.cs             # Base network service (10 KB)
├── VisaNetworkHandler.cs             # Visa-specific logic (10 KB)
├── MastercardNetworkHandler.cs       # Mastercard logic (10 KB)
├── NetworkAuthorizationService.cs    # Authorization (8 KB)
└── NetworkSettlementService.cs       # Settlement (5 KB)
```

#### 1.3 CardService.cs (38.7 KB) → Split into:
```
Services/Card/
├── CardService.cs                    # Core card operations (12 KB)
├── CardIssuanceService.cs            # Card issuance (10 KB)
├── CardActivationService.cs          # Activation logic (8 KB)
├── CardLimitService.cs               # Limit management (8 KB)
└── CardBlockingService.cs            # Block/unblock (5 KB)
```

#### 1.4 StatementGenerator.cs (34.5 KB) → Split into:
```
Services/Statement/Generators/
├── AccountStatementGenerator.cs      # Account statements (12 KB)
├── CardStatementGenerator.cs         # Card statements (10 KB)
├── LoanStatementGenerator.cs         # Loan statements (8 KB)
└── TaxStatementGenerator.cs          # Tax statements (5 KB)
```

#### 1.5 StatementService.cs (34.1 KB) → Split into:
```
Services/Statement/
├── StatementService.cs               # Core statement service (10 KB)
├── StatementGenerationService.cs     # Generation logic (12 KB)
├── StatementDeliveryService.cs       # Delivery (email/download) (8 KB)
└── StatementSchedulingService.cs     # Scheduling (4 KB)
```

### Phase 2: Folder Reorganization

#### 2.1 Move Screenshots
```bash
# Create assets folder
mkdir -p assets/screenshots

# Move all screenshots
mv screenshots/* assets/screenshots/

# Remove old folder
rmdir screenshots
```

#### 2.2 Reorganize wwwroot
```bash
# Create subfolders
mkdir -p src/Bank.Api/wwwroot/{css,js,images,pages}

# Move files
mv src/Bank.Api/wwwroot/*.css src/Bank.Api/wwwroot/css/
mv src/Bank.Api/wwwroot/*.html src/Bank.Api/wwwroot/pages/
mv src/Bank.Api/wwwroot/images/logo.png src/Bank.Api/wwwroot/images/
```

#### 2.3 Clean Up DevOps
```bash
# Remove frontend-specific files
rm devops/docker/Dockerfile.frontend
rm devops/kubernetes/frontend.yaml

# Reorganize docker files
mkdir -p devops/docker/{backend,database}
mv devops/docker/Dockerfile.backend devops/docker/backend/Dockerfile
```

### Phase 3: Controller Organization

#### 3.1 Group Controllers by Domain
```
Controllers/
├── Account/
│   ├── AccountController.cs
│   ├── AccountLifecycleController.cs
│   └── JointAccountController.cs
├── Auth/
│   ├── AuthController.cs
│   ├── TwoFactorAuthController.cs
│   └── SessionController.cs
├── Card/
│   ├── CardController.cs
│   └── PinManagementController.cs
├── Loan/
│   ├── LoansController.cs
│   ├── LoanAnalyticsController.cs
│   └── LoanInterestController.cs
├── Payment/
│   ├── BillPaymentController.cs
│   ├── BillPaymentManagementController.cs
│   ├── BillerManagementController.cs
│   ├── BillPresentmentController.cs
│   ├── RecurringPaymentController.cs
│   └── PaymentTemplateController.cs
└── Transaction/
    ├── TransactionController.cs
    ├── DepositController.cs
    └── BeneficiaryController.cs
```

---

## 📋 Implementation Steps

### Step 1: Backup Current State
```bash
# Create a backup branch
git checkout -b backup-before-reorganization
git add .
git commit -m "Backup before reorganization"
git push origin backup-before-reorganization

# Create reorganization branch
git checkout -b feature/project-reorganization
```

### Step 2: Create New Folder Structure
```bash
# Navigate to project root
cd Bank-Api

# Create new service folders
mkdir -p src/Bank.Application/Services/{Account,Auth,Card,Deposit,Loan,Payment,Statement,Transaction,Shared,Background}

# Create new controller folders
mkdir -p src/Bank.Api/Controllers/{Account,Auth,Card,Loan,Payment,Transaction}

# Create new repository folders
mkdir -p src/Bank.Infrastructure/Repositories/{Account,Card,Loan,Payment,Transaction}

# Create new test folders
mkdir -p src/Bank.Tests/{Unit/Services,Integration/Api,E2E}

# Create assets folder
mkdir -p assets/screenshots

# Create wwwroot subfolders
mkdir -p src/Bank.Api/wwwroot/{css,js,images,pages}
```

### Step 3: Move and Refactor Services (Priority Order)

#### 3.1 High Priority (Large Files)
1. DepositService.cs → Split into Deposit folder
2. CardNetworkService.cs → Split into Card/Network folder
3. CardService.cs → Split into Card folder
4. StatementGenerator.cs → Split into Statement/Generators
5. StatementService.cs → Split into Statement folder

#### 3.2 Medium Priority
6. AccountLifecycleService.cs → Move to Account folder
7. AccountValidationService.cs → Move to Account folder
8. LoanService.cs → Split into Loan folder
9. BillerIntegrationService.cs → Move to Payment folder
10. LoanInterestCalculationService.cs → Move to Loan folder

#### 3.3 Low Priority (Already Small)
- Move remaining services to appropriate folders
- No splitting needed, just organization

### Step 4: Update Namespaces
After moving files, update namespaces:
```csharp
// Old
namespace Bank.Application.Services;

// New
namespace Bank.Application.Services.Account;
namespace Bank.Application.Services.Auth;
namespace Bank.Application.Services.Card;
// etc.
```

### Step 5: Update Using Statements
Update all files that reference moved services:
```csharp
// Old
using Bank.Application.Services;

// New
using Bank.Application.Services.Account;
using Bank.Application.Services.Auth;
```

### Step 6: Move Controllers
```bash
# Move account controllers
mv src/Bank.Api/Controllers/AccountController.cs src/Bank.Api/Controllers/Account/
mv src/Bank.Api/Controllers/JointAccountController.cs src/Bank.Api/Controllers/Account/

# Move auth controllers
mv src/Bank.Api/Controllers/AuthController.cs src/Bank.Api/Controllers/Auth/
mv src/Bank.Api/Controllers/TwoFactorAuthController.cs src/Bank.Api/Controllers/Auth/

# Continue for all controllers...
```

### Step 7: Reorganize wwwroot
```bash
# Move CSS files
mv src/Bank.Api/wwwroot/community-car.css src/Bank.Api/wwwroot/css/styles.css

# Move HTML files
mv src/Bank.Api/wwwroot/Home.html src/Bank.Api/wwwroot/pages/
mv src/Bank.Api/wwwroot/Docs.html src/Bank.Api/wwwroot/pages/
mv src/Bank.Api/wwwroot/404.html src/Bank.Api/wwwroot/pages/

# Update references in HTML files
# Change: href="/community-car.css"
# To: href="/css/styles.css"
```

### Step 8: Move Screenshots
```bash
# Move all screenshots
mv screenshots/* assets/screenshots/

# Remove empty folder
rmdir screenshots
```

### Step 9: Clean Up DevOps
```bash
# Remove frontend files
rm devops/docker/Dockerfile.frontend
rm devops/kubernetes/frontend.yaml

# Reorganize remaining files
mkdir -p devops/docker/backend
mv devops/docker/Dockerfile.backend devops/docker/backend/Dockerfile
```

### Step 10: Update Documentation
Update all documentation files to reflect new structure:
- docs/STRUCTURE.md
- docs/PROJECT_SETUP.md
- README.md

### Step 11: Build and Test
```bash
# Restore packages
dotnet restore

# Build solution
dotnet build

# Run tests
dotnet test

# Check for errors
dotnet build --no-incremental
```

### Step 12: Update DI Registration
Update service registration in `ServiceCollectionExtensions.cs` to reflect new namespaces:
```csharp
// Group by domain
services.AddScoped<IAccountService, AccountService>();
services.AddScoped<IAccountLifecycleService, AccountLifecycleService>();
// etc.
```

---

## ✅ Verification Checklist

After reorganization, verify:

- [ ] All files moved to correct folders
- [ ] All namespaces updated
- [ ] All using statements updated
- [ ] Solution builds without errors
- [ ] All tests pass
- [ ] Controllers accessible via API
- [ ] Static files (wwwroot) load correctly
- [ ] Swagger documentation works
- [ ] Database migrations still work
- [ ] Application starts successfully
- [ ] No broken references
- [ ] Documentation updated

---

## 🎯 Expected Benefits

1. **Improved Navigation**: Easier to find related files
2. **Better Maintainability**: Smaller, focused files
3. **Clearer Separation**: Domain-driven folder structure
4. **Reduced Complexity**: Large files split into manageable pieces
5. **Better Testing**: Easier to test smaller, focused services
6. **Team Collaboration**: Clearer ownership and responsibilities
7. **Faster Builds**: Better caching with organized structure

---

## 📊 Before vs After Metrics

| Metric | Before | After | Improvement |
|--------|--------|-------|-------------|
| Largest Service File | 60.5 KB | ~15 KB | 75% reduction |
| Services > 30 KB | 5 files | 0 files | 100% reduction |
| Avg Service Size | 15 KB | 8 KB | 47% reduction |
| Root-level folders | 7 | 5 | Cleaner root |
| Service subfolders | 0 | 9 | Better organization |
| Controller subfolders | 0 | 6 | Better organization |

---

## 🚀 Quick Start Commands

```bash
# 1. Backup current state
git checkout -b backup-before-reorganization
git add . && git commit -m "Backup before reorganization"

# 2. Create reorganization branch
git checkout -b feature/project-reorganization

# 3. Run reorganization script (to be created)
./scripts/reorganize.sh

# 4. Build and test
dotnet build
dotnet test

# 5. Commit changes
git add .
git commit -m "Reorganize project structure"
git push origin feature/project-reorganization
```

---

## 📝 Notes

- This is a major refactoring - plan for 2-3 days of work
- Test thoroughly after each phase
- Keep backup branch until fully verified
- Update CI/CD pipelines if paths change
- Communicate changes to team members
- Update IDE project settings if needed

---

**Status**: Ready for Implementation
**Priority**: High
**Estimated Effort**: 2-3 days
**Risk Level**: Medium (requires thorough testing)
