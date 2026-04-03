# Design Document: Project Reorganization

## Overview

This design document outlines the technical approach for reorganizing the Bank API Backend project structure. The reorganization addresses maintainability issues caused by large service files (60KB+), flat folder structures, and misplaced assets. The solution implements an automated PowerShell script that moves files into domain-based folders, updates namespaces and using statements, reorganizes static assets, and verifies build success.

The reorganization follows Clean Architecture principles and domain-driven design, grouping related files by business domain (Account, Auth, Card, Deposit, Loan, Payment, Statement, Transaction) rather than by technical layer alone. This improves code navigation, reduces cognitive load, and makes the codebase more maintainable for teams.

The system operates in two modes: dry-run (preview changes without applying) and execution mode (apply changes). It includes comprehensive logging, error handling, backup creation via Git branching, and post-reorganization build verification.

## Architecture

### High-Level Architecture

The reorganization system consists of four main components:

1. **File Movement Engine**: Orchestrates the physical relocation of files from flat directories to domain-based subfolders
2. **Code Transformation Engine**: Updates C# namespaces and using statements to reflect new file locations
3. **Asset Reorganization Engine**: Restructures wwwroot static files and moves screenshots to assets folder
4. **Verification Engine**: Validates that the reorganization maintains build integrity and API compatibility

```
┌─────────────────────────────────────────────────────────────┐
│                  Reorganization System                       │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌──────────────────┐      ┌──────────────────────┐        │
│  │  File Movement   │      │  Code Transformation │        │
│  │     Engine       │─────▶│       Engine         │        │
│  └──────────────────┘      └──────────────────────┘        │
│           │                          │                       │
│           │                          │                       │
│           ▼                          ▼                       │
│  ┌──────────────────┐      ┌──────────────────────┐        │
│  │ Asset Reorg      │      │   Verification       │        │
│  │    Engine        │      │      Engine          │        │
│  └──────────────────┘      └──────────────────────┘        │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

### Domain Organization Strategy

The system organizes files into the following domain folders:

**Services Layer** (Bank.Application.Services):
- Account: Account management, lifecycle, validation, lockout, joint accounts
- Auth: Authentication, 2FA, sessions, password policies, IP whitelisting, token generation
- Card: Card operations, network integration, PIN management
- Deposit: Deposit products, maturity handling, withdrawals, certificates
- Loan: Loan operations, analytics, interest calculations
- Payment: Bill payments, presentment, biller integration, recurring payments, templates, retries, receipts
- Statement: Statement generation and delivery
- Transaction: Transaction processing, transfer eligibility, fraud detection
- Shared: Cross-cutting services (audit, notifications, validation, calculations)
- Background: Background jobs and scheduled tasks

**Controllers Layer** (Bank.Api.Controllers):
- Account: Account, joint account, profile, statement, notification, interest calculation controllers
- Auth: Authentication, 2FA, session, security controllers
- Card: Card, PIN management controllers
- Loan: Loan, analytics, interest controllers
- Payment: Bill payment, biller management, presentment, recurring, template, beneficiary controllers
- Transaction: Transaction, deposit controllers
- Admin: Admin, audit, batch controllers

**Repositories Layer** (Bank.Infrastructure.Repositories):
- Account: Account-related repositories
- Card: Card-related repositories
- Loan: Loan-related repositories
- Payment: Payment-related repositories
- Transaction: Transaction-related repositories

### Namespace Transformation Rules

The system applies the following namespace transformation rules:

1. **Service Files**: `Bank.Application.Services` → `Bank.Application.Services.{Domain}`
2. **Controller Files**: `Bank.Api.Controllers` → `Bank.Api.Controllers.{Domain}`
3. **Repository Files**: `Bank.Infrastructure.Repositories` → `Bank.Infrastructure.Repositories.{Domain}`

Where `{Domain}` is one of: Account, Auth, Card, Deposit, Loan, Payment, Statement, Transaction, Shared, Background, Admin.

### Using Statement Update Strategy

After moving files, the system must update using statements in three categories of files:

1. **Moved Service Files**: Update references to other moved services
2. **Controller Files**: Update references to moved services they depend on
3. **DI Registration File** (ServiceCollectionExtensions.cs): Update all service registrations

The update algorithm:
1. Parse each C# file to identify using statements
2. For each using statement matching `Bank.Application.Services`, `Bank.Api.Controllers`, or `Bank.Infrastructure.Repositories`
3. Determine if the referenced type has been moved to a domain folder
4. If moved, append the domain name to the namespace
5. Remove duplicate using statements
6. Preserve using statement ordering (System namespaces first, then third-party, then project namespaces)

## Components and Interfaces

### File Movement Engine

**Responsibilities**:
- Create domain-based folder structure
- Move service files to appropriate domain folders
- Move controller files to appropriate domain folders
- Move repository files to appropriate domain folders
- Handle missing files gracefully
- Log all operations

**Key Functions**:

```powershell
function Ensure-Directory {
    param([string]$Path)
    # Creates directory if it doesn't exist
    # Logs creation in verbose mode
    # Respects dry-run flag
}

function Move-FileWithLog {
    param([string]$Source, [string]$Destination)
    # Checks if source file exists
    # Logs the move operation
    # Performs move unless in dry-run mode
    # Handles errors gracefully
}
```

**File Movement Mapping**:

The engine uses predefined arrays to map files to domains:

```powershell
$accountServices = @(
    "AccountService.cs",
    "AccountLifecycleService.cs",
    "AccountValidationService.cs",
    "AccountLockoutService.cs",
    "JointAccountService.cs"
)

$authServices = @(
    "AuthService.cs",
    "TwoFactorAuthService.cs",
    "SessionService.cs",
    "PasswordPolicyService.cs",
    "IpWhitelistService.cs",
    "TokenGenerationService.cs"
)

# ... similar arrays for other domains
```

### Code Transformation Engine

**Responsibilities**:
- Update namespace declarations in moved files
- Update using statements in all affected files
- Preserve code structure and formatting
- Handle edge cases (partial classes, nested namespaces)

**Namespace Update Algorithm**:

```
For each moved file:
  1. Read file content
  2. Locate namespace declaration (pattern: "namespace Bank.{Layer}.{Type};")
  3. Determine target domain based on file location
  4. Replace namespace with "namespace Bank.{Layer}.{Type}.{Domain};"
  5. Write updated content back to file
```

**Using Statement Update Algorithm**:

```
For each C# file in the project:
  1. Read file content
  2. Parse using statements
  3. For each using statement:
     a. Check if it references a moved namespace
     b. If yes, determine the domain of the referenced type
     c. Update using statement to include domain
  4. Remove duplicate using statements
  5. Write updated content back to file
```

**Implementation Approach**:

The PowerShell script will use regex patterns to identify and update namespaces:

```powershell
function Update-Namespace {
    param(
        [string]$FilePath,
        [string]$Domain
    )
    
    $content = Get-Content $FilePath -Raw
    
    # Update namespace declaration
    $content = $content -replace `
        'namespace Bank\.Application\.Services;', `
        "namespace Bank.Application.Services.$Domain;"
    
    Set-Content $FilePath $content
}
```

### Asset Reorganization Engine

**Responsibilities**:
- Reorganize wwwroot static files into subfolders (css, js, images, pages)
- Move screenshots from project root to assets/screenshots
- Update HTML file references to reflect new paths
- Remove empty directories after moves
- Clean up unnecessary DevOps files

**wwwroot Reorganization**:

```
Before:
wwwroot/
├── community-car.css
├── Home.html
├── Docs.html
├── 404.html
└── images/
    └── logo.png

After:
wwwroot/
├── css/
│   └── styles.css (renamed from community-car.css)
├── js/
├── images/
│   └── logo.png
└── pages/
    ├── Home.html
    ├── Docs.html
    └── 404.html
```

**HTML Path Update Algorithm**:

```
For each HTML file in wwwroot/pages:
  1. Read file content
  2. Replace "/community-car.css" with "/css/styles.css"
  3. Replace "/images/{filename}" references to ensure correct path
  4. Write updated content back to file
```

**Screenshot Migration**:

```
1. Create assets/screenshots directory
2. Move all files from screenshots/ to assets/screenshots/
3. Preserve all filenames
4. Remove empty screenshots/ directory
```

**DevOps Cleanup**:

Remove frontend-specific files:
- devops/docker/Dockerfile.frontend
- devops/kubernetes/frontend.yaml

### Verification Engine

**Responsibilities**:
- Execute dotnet restore to verify package references
- Execute dotnet build to verify compilation
- Execute dotnet test to verify test integrity
- Verify API endpoint accessibility
- Generate verification report

**Build Verification Steps**:

```
1. dotnet restore
   - Ensures all NuGet packages are restored
   - Verifies project references are intact
   
2. dotnet build --no-incremental
   - Forces full rebuild
   - Catches namespace and using statement errors
   - Verifies all types can be resolved
   
3. dotnet test
   - Runs all unit and integration tests
   - Verifies business logic still works
   - Catches runtime errors
```

**API Endpoint Verification**:

After reorganization, the system should verify:
1. Application starts successfully
2. Swagger UI is accessible at /swagger
3. All controllers are discovered and registered
4. Route attributes are preserved
5. Static files are served from new wwwroot structure

## Data Models

### File Movement Record

```csharp
public class FileMovementRecord
{
    public string SourcePath { get; set; }
    public string DestinationPath { get; set; }
    public string Domain { get; set; }
    public FileType Type { get; set; } // Service, Controller, Repository
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
}

public enum FileType
{
    Service,
    Controller,
    Repository,
    StaticAsset,
    Screenshot,
    DevOpsConfig
}
```

### Namespace Transformation Record

```csharp
public class NamespaceTransformation
{
    public string FilePath { get; set; }
    public string OldNamespace { get; set; }
    public string NewNamespace { get; set; }
    public bool Success { get; set; }
}
```

### Using Statement Update Record

```csharp
public class UsingStatementUpdate
{
    public string FilePath { get; set; }
    public List<string> OldUsings { get; set; }
    public List<string> NewUsings { get; set; }
    public int UpdateCount { get; set; }
}
```

### Reorganization Summary

```csharp
public class ReorganizationSummary
{
    public int TotalFilesProcessed { get; set; }
    public int FilesMovedSuccessfully { get; set; }
    public int FilesMovesFailed { get; set; }
    public int NamespacesUpdated { get; set; }
    public int UsingStatementsUpdated { get; set; }
    public int DirectoriesCreated { get; set; }
    public int FilesDeleted { get; set; }
    public bool BuildSuccessful { get; set; }
    public bool TestsSuccessful { get; set; }
    public TimeSpan Duration { get; set; }
    public List<string> Errors { get; set; }
    public List<string> Warnings { get; set; }
}
```


## Correctness Properties

*A property is a characteristic or behavior that should hold true across all valid executions of a system—essentially, a formal statement about what the system should do. Properties serve as the bridge between human-readable specifications and machine-verifiable correctness guarantees.*

### Property 1: Complete Folder Structure Creation

*For any* execution of the reorganization system, all required domain folders SHALL be created at their expected paths, including Services subfolders (Account, Auth, Card, Deposit, Loan, Payment, Statement, Transaction, Shared, Background), Controllers subfolders (Account, Auth, Card, Loan, Payment, Transaction, Admin), Repositories subfolders (Account, Card, Loan, Payment, Transaction), Tests subfolders (Unit/Services, Integration/Api, E2E), wwwroot subfolders (css, js, images, pages), and assets subfolders (screenshots, images).

**Validates: Requirements 1.1, 1.2, 1.3, 1.4, 1.5, 1.6**

### Property 2: Service File Domain Relocation

*For any* service file in the predefined service-to-domain mapping, after reorganization the file SHALL exist at the path Services/{Domain}/{FileName} where {Domain} is the assigned domain (Account, Auth, Card, Deposit, Loan, Payment, Statement, Transaction, Shared, or Background) and SHALL NOT exist at the original flat Services/ path.

**Validates: Requirements 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8, 2.9, 2.10**

### Property 3: Controller File Domain Relocation

*For any* controller file in the predefined controller-to-domain mapping, after reorganization the file SHALL exist at the path Controllers/{Domain}/{FileName} where {Domain} is the assigned domain (Account, Auth, Card, Loan, Payment, Transaction, or Admin) and SHALL NOT exist at the original flat Controllers/ path.

**Validates: Requirements 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7**

### Property 4: Namespace Transformation Correctness

*For any* C# file moved to a domain subfolder, the namespace declaration SHALL match the pattern "Bank.{Layer}.{Type}.{Domain}" where {Layer} is Application/Api/Infrastructure, {Type} is Services/Controllers/Repositories, and {Domain} matches the folder name, and the class name SHALL remain unchanged from the original file.

**Validates: Requirements 4.1, 4.2, 4.3, 4.4**

### Property 5: Using Statement Update Completeness

*For any* C# file that references a moved type, the using statements SHALL include the domain-specific namespace (e.g., "Bank.Application.Services.Account" instead of "Bank.Application.Services"), and the file SHALL compile without namespace resolution errors.

**Validates: Requirements 5.1, 5.2, 5.3, 5.4**

### Property 6: wwwroot Asset Organization

*For any* HTML file in wwwroot/pages/, all CSS references SHALL use the path "/css/styles.css", all image references SHALL use the path "/images/{filename}", and all HTML files (Home.html, Docs.html, 404.html) SHALL exist in wwwroot/pages/ rather than wwwroot root.

**Validates: Requirements 6.2, 6.3, 6.4, 6.5**

### Property 7: Screenshot Migration Preservation

*For any* file that existed in screenshots/ before reorganization, after reorganization the file SHALL exist in assets/screenshots/ with the same filename, and the original screenshots/ directory SHALL NOT exist.

**Validates: Requirements 7.1, 7.3**

### Property 8: Backend DevOps File Preservation

*For any* backend-related DevOps file (Dockerfile.backend, backend.yaml, database.yaml, docker-compose.yml), the file SHALL exist at its original path after reorganization, while frontend-specific files (Dockerfile.frontend, frontend.yaml) SHALL NOT exist.

**Validates: Requirements 8.1, 8.2, 8.3**

### Property 9: Dry Run Mode Non-Modification

*For any* execution with the -DryRun flag, no files SHALL be moved, no folders SHALL be created, no files SHALL be deleted, and the console output SHALL display all planned operations with a summary indicating no changes were made.

**Validates: Requirements 9.1, 9.2, 9.3**

### Property 10: Build Process Success

*For any* execution of the reorganization system (not in dry-run mode), the commands "dotnet restore", "dotnet build", and "dotnet test" SHALL all complete with exit code 0 (success) when executed in the project root directory.

**Validates: Requirements 10.1, 10.2, 10.3**

### Property 11: Application Runtime Integrity

*For any* execution of the reorganized application, the application SHALL start successfully, database migrations SHALL apply without errors, the Swagger UI SHALL be accessible at /swagger, and static files SHALL be served from the reorganized wwwroot structure.

**Validates: Requirements 10.4, 10.5, 10.6**

### Property 12: API Endpoint Preservation

*For any* API endpoint that existed before reorganization, after reorganization the endpoint SHALL be accessible at the same route with the same HTTP method, the route attributes in controller files SHALL be unchanged, and the Swagger documentation SHALL display all endpoints.

**Validates: Requirements 11.1, 11.2, 11.3, 11.4**

### Property 13: Verbose Logging Completeness

*For any* execution with the -Verbose flag, the console output SHALL include log entries for each folder creation, each file move (with source and destination paths), and each file deletion, and when a file operation fails, an error message SHALL be logged and execution SHALL continue.

**Validates: Requirements 13.1, 13.2, 13.3, 13.4**

### Property 14: Graceful Missing File Handling

*For any* file in the reorganization plan that does not exist at its expected source path, the system SHALL log a skip message, continue processing remaining files, and complete with exit code 0 (success) rather than throwing an error.

**Validates: Requirements 14.1, 14.2, 14.3, 14.4, 14.5**

## Error Handling

### File System Errors

**Missing Source Files**:
- When a file to be moved doesn't exist, log a warning and continue
- Track missing files in a list for the final summary
- Don't treat missing files as fatal errors

**Permission Errors**:
- If a file move fails due to permissions, log the error with the file path
- Continue with remaining operations
- Report all permission errors in the final summary

**Disk Space Errors**:
- Before starting reorganization, check available disk space
- If insufficient space, abort with a clear error message
- Recommend cleanup actions to the user

### Code Transformation Errors

**Namespace Update Failures**:
- If a namespace pattern doesn't match expected format, log a warning
- Preserve the original namespace rather than corrupting the file
- Add the file to a manual review list in the summary

**Using Statement Parsing Errors**:
- If a C# file has syntax errors that prevent parsing, log the error
- Skip using statement updates for that file
- Add the file to a manual review list

**Encoding Issues**:
- Detect file encoding before reading (UTF-8, UTF-16, etc.)
- Preserve original encoding when writing files
- If encoding detection fails, log an error and skip the file

### Build Verification Errors

**Restore Failures**:
- If "dotnet restore" fails, capture the error output
- Display the error to the user
- Recommend checking NuGet package sources and network connectivity

**Compilation Errors**:
- If "dotnet build" fails, capture all compilation errors
- Group errors by file for easier diagnosis
- Provide guidance on common issues (missing using statements, namespace mismatches)

**Test Failures**:
- If "dotnet test" fails, capture test results
- Display which tests failed and why
- Note that test failures may indicate business logic issues, not reorganization issues

### Git Operation Errors

**Branch Creation Failures**:
- If backup branch creation fails (e.g., branch already exists), prompt user to delete or rename existing branch
- Don't proceed with reorganization until backup is successful

**Uncommitted Changes**:
- Before creating backup, check for uncommitted changes
- If found, prompt user to commit or stash changes
- Provide clear instructions on how to proceed

### Rollback Strategy

If critical errors occur during reorganization:

1. **Automatic Rollback**: If file moves fail midway, attempt to restore moved files to original locations
2. **Git Rollback**: User can switch back to backup branch: `git checkout backup-before-reorganization`
3. **Manual Rollback**: Provide a list of all operations performed so user can manually reverse them

### Error Reporting

All errors should be reported with:
- **Severity**: Warning, Error, or Critical
- **Context**: Which phase of reorganization (file movement, namespace update, build verification)
- **File Path**: The specific file involved (if applicable)
- **Suggested Action**: What the user should do to resolve the issue

Example error message:
```
[ERROR] Failed to move file: src/Bank.Application/Services/AccountService.cs
Reason: Access denied (file may be locked by another process)
Suggested Action: Close any IDEs or editors that may have the file open, then re-run the script
```

## Testing Strategy

### Dual Testing Approach

This feature requires both unit tests and property-based tests to ensure comprehensive coverage:

**Unit Tests**: Focus on specific examples, edge cases, and error conditions
- Test specific file moves (e.g., AccountService.cs moves to Services/Account/)
- Test specific namespace transformations
- Test error handling for missing files
- Test dry-run mode output format
- Test Git branch creation

**Property-Based Tests**: Verify universal properties across all inputs
- Test that all files in the service mapping are moved correctly (Property 2)
- Test that all namespaces follow the correct pattern (Property 4)
- Test that the system handles any missing file gracefully (Property 14)
- Test that dry-run mode never modifies files regardless of input (Property 9)

### Property-Based Testing Configuration

**Testing Library**: Use Pester for PowerShell script testing (built-in to PowerShell)

**Test Iterations**: Each property test should run with at least 100 different scenarios (e.g., different combinations of missing files, different file sets)

**Test Tagging**: Each property test must reference its design document property using the format:
```powershell
# Feature: project-reorganization, Property 2: Service File Domain Relocation
Describe "Service File Domain Relocation" {
    It "Should move all service files to their assigned domain folders" {
        # Test implementation
    }
}
```

### Unit Test Examples

**Test: Specific File Move**
```powershell
Describe "AccountService File Move" {
    It "Should move AccountService.cs to Services/Account/" {
        # Arrange: Create test file structure
        # Act: Run reorganization
        # Assert: File exists at new location
    }
}
```

**Test: Namespace Update**
```powershell
Describe "Namespace Transformation" {
    It "Should update AccountService namespace to include Account domain" {
        # Arrange: Create file with old namespace
        # Act: Run namespace update
        # Assert: File contains new namespace
    }
}
```

**Test: Dry Run Mode**
```powershell
Describe "Dry Run Mode" {
    It "Should not move files when -DryRun flag is set" {
        # Arrange: Create test file structure
        # Act: Run with -DryRun
        # Assert: Files remain at original locations
    }
}
```

### Property Test Examples

**Property Test: All Service Files Relocated**
```powershell
# Feature: project-reorganization, Property 2: Service File Domain Relocation
Describe "Service File Domain Relocation Property" {
    It "Should relocate all service files in the mapping to their domain folders" -ForEach (1..100) {
        # Generate random subset of service files
        # Run reorganization
        # Verify all files are in correct domain folders
    }
}
```

**Property Test: Namespace Pattern Correctness**
```powershell
# Feature: project-reorganization, Property 4: Namespace Transformation Correctness
Describe "Namespace Transformation Property" {
    It "Should update all moved files to have domain-specific namespaces" -ForEach (1..100) {
        # Generate random set of moved files
        # Run namespace update
        # Verify all namespaces match pattern Bank.{Layer}.{Type}.{Domain}
    }
}
```

**Property Test: Graceful Error Handling**
```powershell
# Feature: project-reorganization, Property 14: Graceful Missing File Handling
Describe "Missing File Handling Property" {
    It "Should handle missing files gracefully without failing" -ForEach (1..100) {
        # Generate random set of missing files
        # Run reorganization
        # Verify exit code is 0 and execution completes
    }
}
```

### Integration Tests

**Test: End-to-End Reorganization**
```powershell
Describe "Complete Reorganization" {
    It "Should successfully reorganize the entire project" {
        # Arrange: Clone repository to temp location
        # Act: Run full reorganization
        # Assert: All properties hold, build succeeds, tests pass
    }
}
```

**Test: Build Verification**
```powershell
Describe "Post-Reorganization Build" {
    It "Should build successfully after reorganization" {
        # Arrange: Run reorganization
        # Act: Execute dotnet build
        # Assert: Exit code is 0, no compilation errors
    }
}
```

**Test: API Endpoint Preservation**
```powershell
Describe "API Compatibility" {
    It "Should preserve all API endpoints after reorganization" {
        # Arrange: Get endpoint list before reorganization
        # Act: Run reorganization and start application
        # Assert: All original endpoints are still accessible
    }
}
```

### Test Execution Strategy

1. **Pre-Commit Tests**: Run unit tests before committing changes to the reorganization script
2. **CI/CD Tests**: Run full test suite (unit + property + integration) in CI pipeline
3. **Manual Verification**: After reorganization, manually verify:
   - Application starts successfully
   - Swagger UI displays all endpoints
   - Sample API requests work correctly
   - Static files load in browser

### Test Data Management

**Test Repository**: Create a minimal test repository that mimics the Bank API structure:
- Sample service files with various sizes
- Sample controller files
- Sample wwwroot files
- Sample screenshots

**Test Isolation**: Each test should:
- Create its own temporary directory
- Copy test files to temp directory
- Run reorganization on temp directory
- Clean up temp directory after test

**Test Repeatability**: Tests should be idempotent:
- Running the same test multiple times should produce the same result
- Tests should not depend on external state
- Tests should not interfere with each other

