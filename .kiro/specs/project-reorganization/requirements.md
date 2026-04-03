# Requirements Document

## Introduction

This document defines the requirements for reorganizing the Bank API Backend project structure to improve maintainability, code navigation, and adherence to SOLID principles. The reorganization addresses large service files (60KB+), flat folder structures, misplaced assets, and unnecessary frontend files in an API-only backend project.

## Glossary

- **Reorganization_System**: The automated tooling and manual processes that restructure the project
- **Service_File**: A C# class file containing business logic in the Application layer
- **Controller_File**: A C# class file containing API endpoints in the API layer
- **Domain_Folder**: A logical grouping of related files (Account, Auth, Card, Loan, Payment, Transaction)
- **Namespace**: The C# namespace declaration that must match the folder structure
- **Using_Statement**: C# import statements that reference other namespaces
- **wwwroot**: The static file directory in ASP.NET Core projects
- **DevOps_Config**: Configuration files for Docker, Kubernetes, and deployment
- **Asset_File**: Non-code files like screenshots, images, and documentation
- **DI_Registration**: Dependency Injection service registration in ServiceCollectionExtensions.cs
- **Build_Process**: The dotnet build and test commands that verify code correctness

## Requirements

### Requirement 1: Create Domain-Based Folder Structure

**User Story:** As a developer, I want services and controllers organized into domain-based subfolders, so that I can quickly locate related files and understand the system architecture.

#### Acceptance Criteria

1. THE Reorganization_System SHALL create subfolder structure for Services: Account, Auth, Card, Deposit, Loan, Payment, Statement, Transaction, Shared, Background
2. THE Reorganization_System SHALL create subfolder structure for Controllers: Account, Auth, Card, Loan, Payment, Transaction, Admin
3. THE Reorganization_System SHALL create subfolder structure for Repositories: Account, Card, Loan, Payment, Transaction
4. THE Reorganization_System SHALL create subfolder structure for Tests: Unit/Services, Integration/Api, E2E
5. THE Reorganization_System SHALL create subfolder structure for wwwroot: css, js, images, pages
6. THE Reorganization_System SHALL create assets folder with subfolders: screenshots, images

### Requirement 2: Move Service Files to Domain Folders

**User Story:** As a developer, I want service files moved to their appropriate domain folders, so that related business logic is grouped together.

#### Acceptance Criteria

1. WHEN moving Account services, THE Reorganization_System SHALL relocate AccountService.cs, AccountLifecycleService.cs, AccountValidationService.cs, AccountLockoutService.cs, JointAccountService.cs to Services/Account
2. WHEN moving Auth services, THE Reorganization_System SHALL relocate AuthService.cs, TwoFactorAuthService.cs, SessionService.cs, PasswordPolicyService.cs, IpWhitelistService.cs, TokenGenerationService.cs to Services/Auth
3. WHEN moving Card services, THE Reorganization_System SHALL relocate CardService.cs, CardNetworkService.cs, PinManagementService.cs to Services/Card
4. WHEN moving Deposit services, THE Reorganization_System SHALL relocate DepositService.cs, DepositMaturityService.cs, DepositWithdrawalService.cs, DepositCertificateGenerator.cs, DepositBackgroundService.cs to Services/Deposit
5. WHEN moving Loan services, THE Reorganization_System SHALL relocate LoanService.cs, LoanAnalyticsService.cs, LoanInterestCalculationService.cs, LoanBackgroundService.cs to Services/Loan
6. WHEN moving Payment services, THE Reorganization_System SHALL relocate BillPaymentService.cs, BillPresentmentService.cs, BillerIntegrationService.cs, RecurringPaymentService.cs, PaymentTemplateService.cs, PaymentRetryService.cs, PaymentReceiptService.cs, BillPaymentBackgroundService.cs, BeneficiaryService.cs to Services/Payment
7. WHEN moving Statement services, THE Reorganization_System SHALL relocate StatementService.cs, StatementGenerator.cs to Services/Statement
8. WHEN moving Transaction services, THE Reorganization_System SHALL relocate TransactionService.cs, TransferEligibilityService.cs, FraudDetectionService.cs to Services/Transaction
9. WHEN moving Shared services, THE Reorganization_System SHALL relocate AuditLogService.cs, AuditEventPublisher.cs, NotificationService.cs, ValidationService.cs, CalculationService.cs, InterestCalculationService.cs, FeeCalculationService.cs to Services/Shared
10. WHEN moving Background services, THE Reorganization_System SHALL relocate BillerHealthCheckBackgroundService.cs, PaymentRetryBackgroundService.cs, BatchService.cs, BatchPaymentService.cs to Services/Background

### Requirement 3: Move Controller Files to Domain Folders

**User Story:** As a developer, I want controller files moved to their appropriate domain folders, so that API endpoints are logically organized.

#### Acceptance Criteria

1. WHEN moving Account controllers, THE Reorganization_System SHALL relocate AccountController.cs, JointAccountController.cs, ProfileController.cs, StatementController.cs, NotificationController.cs, InterestCalculationController.cs to Controllers/Account
2. WHEN moving Auth controllers, THE Reorganization_System SHALL relocate AuthController.cs, TwoFactorAuthController.cs, SessionController.cs, SecurityController.cs to Controllers/Auth
3. WHEN moving Card controllers, THE Reorganization_System SHALL relocate CardController.cs, PinManagementController.cs to Controllers/Card
4. WHEN moving Loan controllers, THE Reorganization_System SHALL relocate LoansController.cs, LoanAnalyticsController.cs, LoanInterestController.cs to Controllers/Loan
5. WHEN moving Payment controllers, THE Reorganization_System SHALL relocate BillPaymentController.cs, BillPaymentManagementController.cs, BillerManagementController.cs, BillPresentmentController.cs, RecurringPaymentController.cs, PaymentTemplateController.cs, BeneficiaryController.cs to Controllers/Payment
6. WHEN moving Transaction controllers, THE Reorganization_System SHALL relocate TransactionController.cs, DepositController.cs to Controllers/Transaction
7. WHEN moving Admin controllers, THE Reorganization_System SHALL relocate AdminController.cs, AuditController.cs, BatchController.cs to Controllers/Admin

### Requirement 4: Update Namespaces After File Moves

**User Story:** As a developer, I want namespaces automatically updated after file moves, so that the code compiles without namespace errors.

#### Acceptance Criteria

1. WHEN a Service_File is moved to a Domain_Folder, THE Reorganization_System SHALL update the Namespace from "Bank.Application.Services" to "Bank.Application.Services.{DomainName}"
2. WHEN a Controller_File is moved to a Domain_Folder, THE Reorganization_System SHALL update the Namespace from "Bank.Api.Controllers" to "Bank.Api.Controllers.{DomainName}"
3. WHEN a Repository file is moved to a Domain_Folder, THE Reorganization_System SHALL update the Namespace from "Bank.Infrastructure.Repositories" to "Bank.Infrastructure.Repositories.{DomainName}"
4. FOR ALL moved files, THE Reorganization_System SHALL preserve the class name and file structure

### Requirement 5: Update Using Statements After File Moves

**User Story:** As a developer, I want using statements automatically updated after file moves, so that references to moved classes remain valid.

#### Acceptance Criteria

1. WHEN a Service_File references a moved service, THE Reorganization_System SHALL update the Using_Statement to include the new domain-specific namespace
2. WHEN a Controller_File references a moved service, THE Reorganization_System SHALL update the Using_Statement to include the new domain-specific namespace
3. WHEN DI_Registration references a moved service, THE Reorganization_System SHALL update the Using_Statement to include the new domain-specific namespace
4. FOR ALL files with updated using statements, THE Build_Process SHALL complete without compilation errors

### Requirement 6: Reorganize wwwroot Static Files

**User Story:** As a developer, I want static files organized into proper subfolders, so that CSS, JavaScript, images, and HTML pages are easy to locate.

#### Acceptance Criteria

1. THE Reorganization_System SHALL move community-car.css to wwwroot/css/styles.css
2. THE Reorganization_System SHALL move Home.html, Docs.html, 404.html to wwwroot/pages/
3. THE Reorganization_System SHALL move all image files to wwwroot/images/
4. WHEN HTML files reference CSS files, THE Reorganization_System SHALL update paths from "/community-car.css" to "/css/styles.css"
5. WHEN HTML files reference image files, THE Reorganization_System SHALL update paths to "/images/{filename}"

### Requirement 7: Move Screenshots to Assets Folder

**User Story:** As a developer, I want frontend screenshots moved to an assets folder, so that the project root is cleaner and assets are properly organized.

#### Acceptance Criteria

1. THE Reorganization_System SHALL move all files from screenshots/ to assets/screenshots/
2. WHEN all screenshot files are moved, THE Reorganization_System SHALL remove the empty screenshots/ folder
3. THE Reorganization_System SHALL preserve all screenshot filenames during the move

### Requirement 8: Remove Unnecessary Frontend Files

**User Story:** As a developer, I want frontend-specific DevOps files removed, so that the API-only backend project doesn't contain irrelevant configuration.

#### Acceptance Criteria

1. THE Reorganization_System SHALL remove devops/docker/Dockerfile.frontend
2. THE Reorganization_System SHALL remove devops/kubernetes/frontend.yaml
3. THE Reorganization_System SHALL preserve all backend-related DevOps files

### Requirement 9: Provide Dry Run Mode

**User Story:** As a developer, I want to preview reorganization changes before applying them, so that I can verify the plan without modifying files.

#### Acceptance Criteria

1. WHEN the Reorganization_System is invoked with -DryRun flag, THE Reorganization_System SHALL display all planned file moves without executing them
2. WHEN the Reorganization_System is invoked with -DryRun flag, THE Reorganization_System SHALL display all planned folder creations without executing them
3. WHEN the Reorganization_System is invoked with -DryRun flag, THE Reorganization_System SHALL display all planned file deletions without executing them
4. WHEN the Reorganization_System completes in dry run mode, THE Reorganization_System SHALL display a summary message indicating no changes were made

### Requirement 10: Verify Build Success After Reorganization

**User Story:** As a developer, I want the application to build successfully after reorganization, so that I know the refactoring didn't break the codebase.

#### Acceptance Criteria

1. WHEN the reorganization is complete, THE Build_Process SHALL execute "dotnet restore" without errors
2. WHEN the reorganization is complete, THE Build_Process SHALL execute "dotnet build" without errors
3. WHEN the reorganization is complete, THE Build_Process SHALL execute "dotnet test" without test failures
4. WHEN the application starts, THE application SHALL apply database migrations successfully
5. WHEN the application starts, THE application SHALL serve Swagger UI at /swagger endpoint
6. WHEN the application starts, THE application SHALL serve static files from wwwroot with updated paths

### Requirement 11: Maintain Backward Compatibility for API Endpoints

**User Story:** As an API consumer, I want all API endpoints to remain accessible after reorganization, so that existing integrations continue to work.

#### Acceptance Criteria

1. WHEN controllers are moved to domain folders, THE Reorganization_System SHALL preserve all route attributes
2. WHEN the application starts after reorganization, THE application SHALL expose the same API endpoints as before reorganization
3. WHEN Swagger UI is accessed, THE Swagger_Documentation SHALL display all endpoints grouped by controller
4. FOR ALL API endpoints, the HTTP methods, routes, and request/response formats SHALL remain unchanged

### Requirement 12: Update Documentation After Reorganization

**User Story:** As a developer, I want documentation updated to reflect the new structure, so that new team members can understand the organization.

#### Acceptance Criteria

1. THE Reorganization_System SHALL update docs/STRUCTURE.md to reflect the new folder structure
2. THE Reorganization_System SHALL update README.md to reference the new organization
3. THE Reorganization_System SHALL preserve REORGANIZATION_PLAN.md as historical reference
4. THE Reorganization_System SHALL create a summary of changes in the commit message

### Requirement 13: Provide Verbose Logging Option

**User Story:** As a developer, I want detailed logging during reorganization, so that I can troubleshoot issues if they occur.

#### Acceptance Criteria

1. WHEN the Reorganization_System is invoked with -Verbose flag, THE Reorganization_System SHALL log each folder creation operation
2. WHEN the Reorganization_System is invoked with -Verbose flag, THE Reorganization_System SHALL log each file move operation with source and destination paths
3. WHEN the Reorganization_System is invoked with -Verbose flag, THE Reorganization_System SHALL log each file deletion operation
4. WHEN a file move operation fails, THE Reorganization_System SHALL log the error message and continue with remaining operations

### Requirement 14: Handle Missing Files Gracefully

**User Story:** As a developer, I want the reorganization script to handle missing files gracefully, so that the process doesn't fail if some expected files don't exist.

#### Acceptance Criteria

1. WHEN a Service_File does not exist at the expected source path, THE Reorganization_System SHALL log a skip message and continue
2. WHEN a Controller_File does not exist at the expected source path, THE Reorganization_System SHALL log a skip message and continue
3. WHEN the screenshots folder does not exist, THE Reorganization_System SHALL log a skip message and continue
4. WHEN a DevOps file to be deleted does not exist, THE Reorganization_System SHALL log a skip message and continue
5. FOR ALL missing files, THE Reorganization_System SHALL complete successfully without errors

### Requirement 15: Create Backup Before Reorganization

**User Story:** As a developer, I want a backup created before reorganization, so that I can revert changes if something goes wrong.

#### Acceptance Criteria

1. THE Reorganization_System SHALL create a Git branch named "backup-before-reorganization" before making changes
2. THE Reorganization_System SHALL commit all current changes to the backup branch
3. THE Reorganization_System SHALL create a new branch named "feature/project-reorganization" for the reorganization work
4. WHEN the backup is created, THE Reorganization_System SHALL display the backup branch name to the user
