# Implementation Plan: Project Reorganization

## Overview

This implementation plan reorganizes the Bank API Backend project structure by creating domain-based folders, moving service and controller files, updating namespaces and using statements, reorganizing static assets, and verifying build success. The implementation uses a PowerShell script that supports dry-run mode and verbose logging.

## Tasks

- [ ] 1. Create PowerShell script foundation and folder structure
  - Create reorganize-structure.ps1 script in scripts/ directory
  - Implement parameter handling for -DryRun and -Verbose flags
  - Implement Ensure-Directory function to create domain folders
  - Create all required domain folders: Services (Account, Auth, Card, Deposit, Loan, Payment, Statement, Transaction, Shared, Background), Controllers (Account, Auth, Card, Loan, Payment, Transaction, Admin), Repositories (Account, Card, Loan, Payment, Transaction), wwwroot subfolders (css, js, images, pages), assets/screenshots
  - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 9.1, 9.2, 13.1_

- [ ]* 1.1 Write property test for folder structure creation
  - **Property 1: Complete Folder Structure Creation**
  - **Validates: Requirements 1.1, 1.2, 1.3, 1.4, 1.5, 1.6**

- [ ] 2. Implement file movement engine for services
  - [ ] 2.1 Create Move-FileWithLog function with dry-run support
    - Implement file existence checking
    - Implement logging for move operations
    - Respect -DryRun flag to preview without executing
    - Handle missing files gracefully with skip messages
    - _Requirements: 9.1, 13.2, 14.1_

  - [ ] 2.2 Define service-to-domain mapping arrays
    - Create arrays for Account, Auth, Card, Deposit, Loan, Payment, Statement, Transaction, Shared, and Background services
    - Map each service file to its target domain folder
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8, 2.9, 2.10_

  - [ ] 2.3 Implement service file relocation logic
    - Iterate through service mapping arrays
    - Move each service file to its domain folder using Move-FileWithLog
    - Log successful moves and skipped files
    - _Requirements: 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8, 2.9, 2.10_

- [ ]* 2.4 Write property test for service file relocation
  - **Property 2: Service File Domain Relocation**
  - **Validates: Requirements 2.1, 2.2, 2.3, 2.4, 2.5, 2.6, 2.7, 2.8, 2.9, 2.10**

- [ ] 3. Implement file movement engine for controllers
  - [ ] 3.1 Define controller-to-domain mapping arrays
    - Create arrays for Account, Auth, Card, Loan, Payment, Transaction, and Admin controllers
    - Map each controller file to its target domain folder
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7_

  - [ ] 3.2 Implement controller file relocation logic
    - Iterate through controller mapping arrays
    - Move each controller file to its domain folder using Move-FileWithLog
    - Log successful moves and skipped files
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7_

- [ ]* 3.3 Write property test for controller file relocation
  - **Property 3: Controller File Domain Relocation**
  - **Validates: Requirements 3.1, 3.2, 3.3, 3.4, 3.5, 3.6, 3.7**

- [ ] 4. Checkpoint - Verify file moves
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 5. Implement code transformation engine for namespaces
  - [ ] 5.1 Create Update-Namespace function
    - Implement regex pattern to match namespace declarations
    - Replace "Bank.Application.Services" with "Bank.Application.Services.{Domain}"
    - Replace "Bank.Api.Controllers" with "Bank.Api.Controllers.{Domain}"
    - Replace "Bank.Infrastructure.Repositories" with "Bank.Infrastructure.Repositories.{Domain}"
    - Preserve class names and file structure
    - _Requirements: 4.1, 4.2, 4.3, 4.4_

  - [ ] 5.2 Apply namespace updates to all moved service files
    - Iterate through all service files in domain folders
    - Determine domain from folder path
    - Update namespace declaration using Update-Namespace
    - _Requirements: 4.1_

  - [ ] 5.3 Apply namespace updates to all moved controller files
    - Iterate through all controller files in domain folders
    - Determine domain from folder path
    - Update namespace declaration using Update-Namespace
    - _Requirements: 4.2_

- [ ]* 5.4 Write property test for namespace transformation
  - **Property 4: Namespace Transformation Correctness**
  - **Validates: Requirements 4.1, 4.2, 4.3, 4.4**

- [ ] 6. Implement code transformation engine for using statements
  - [ ] 6.1 Create Update-UsingStatements function
    - Parse using statements in C# files
    - Identify references to moved types
    - Update using statements to include domain-specific namespaces
    - Remove duplicate using statements
    - _Requirements: 5.1, 5.2, 5.3_

  - [ ] 6.2 Update using statements in all service files
    - Scan all service files for using statements
    - Update references to moved services
    - _Requirements: 5.1_

  - [ ] 6.3 Update using statements in all controller files
    - Scan all controller files for using statements
    - Update references to moved services
    - _Requirements: 5.2_

  - [ ] 6.4 Update using statements in DI registration file
    - Update ServiceCollectionExtensions.cs
    - Update all service registrations to use domain-specific namespaces
    - _Requirements: 5.3_

- [ ]* 6.5 Write property test for using statement updates
  - **Property 5: Using Statement Update Completeness**
  - **Validates: Requirements 5.1, 5.2, 5.3, 5.4**

- [ ] 7. Checkpoint - Verify code transformations
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 8. Implement asset reorganization engine for wwwroot
  - [ ] 8.1 Move CSS files to wwwroot/css/
    - Move community-car.css to wwwroot/css/styles.css
    - _Requirements: 6.1_

  - [ ] 8.2 Move HTML files to wwwroot/pages/
    - Move Home.html, Docs.html, 404.html to wwwroot/pages/
    - _Requirements: 6.2_

  - [ ] 8.3 Move image files to wwwroot/images/
    - Move all image files to wwwroot/images/
    - _Requirements: 6.3_

  - [ ] 8.4 Update HTML file references
    - Update CSS references from "/community-car.css" to "/css/styles.css"
    - Update image references to "/images/{filename}"
    - _Requirements: 6.4, 6.5_

- [ ]* 8.5 Write property test for wwwroot reorganization
  - **Property 6: wwwroot Asset Organization**
  - **Validates: Requirements 6.2, 6.3, 6.4, 6.5**

- [ ] 9. Implement asset reorganization engine for screenshots
  - [ ] 9.1 Move screenshots to assets folder
    - Create assets/screenshots/ directory
    - Move all files from screenshots/ to assets/screenshots/
    - Preserve all filenames
    - _Requirements: 7.1, 7.3_

  - [ ] 9.2 Remove empty screenshots directory
    - Remove screenshots/ folder after all files are moved
    - _Requirements: 7.2_

- [ ]* 9.3 Write property test for screenshot migration
  - **Property 7: Screenshot Migration Preservation**
  - **Validates: Requirements 7.1, 7.3**

- [ ] 10. Implement DevOps cleanup
  - Remove devops/docker/Dockerfile.frontend
  - Remove devops/kubernetes/frontend.yaml
  - Verify backend DevOps files are preserved
  - _Requirements: 8.1, 8.2, 8.3_

- [ ]* 10.1 Write property test for DevOps file handling
  - **Property 8: Backend DevOps File Preservation**
  - **Validates: Requirements 8.1, 8.2, 8.3**

- [ ] 11. Checkpoint - Verify asset reorganization
  - Ensure all tests pass, ask the user if questions arise.

- [ ] 12. Implement verification engine
  - [ ] 12.1 Implement dotnet restore verification
    - Execute "dotnet restore" command
    - Capture exit code and output
    - Log success or failure
    - _Requirements: 10.1_

  - [ ] 12.2 Implement dotnet build verification
    - Execute "dotnet build --no-incremental" command
    - Capture exit code and compilation errors
    - Log success or failure
    - _Requirements: 10.2_

  - [ ] 12.3 Implement dotnet test verification
    - Execute "dotnet test" command
    - Capture exit code and test results
    - Log success or failure
    - _Requirements: 10.3_

  - [ ] 12.4 Implement application startup verification
    - Verify database migrations apply successfully
    - Verify Swagger UI is accessible at /swagger
    - Verify static files are served from reorganized wwwroot
    - _Requirements: 10.4, 10.5, 10.6_

- [ ]* 12.5 Write property test for build verification
  - **Property 10: Build Process Success**
  - **Validates: Requirements 10.1, 10.2, 10.3**

- [ ]* 12.6 Write property test for application runtime integrity
  - **Property 11: Application Runtime Integrity**
  - **Validates: Requirements 10.4, 10.5, 10.6**

- [ ]* 12.7 Write property test for API endpoint preservation
  - **Property 12: API Endpoint Preservation**
  - **Validates: Requirements 11.1, 11.2, 11.3, 11.4**

- [ ] 13. Implement logging and error handling
  - [ ] 13.1 Implement verbose logging for all operations
    - Log folder creation operations when -Verbose flag is set
    - Log file move operations with source and destination paths
    - Log file deletion operations
    - _Requirements: 13.1, 13.2, 13.3_

  - [ ] 13.2 Implement graceful error handling
    - Catch file operation errors and log error messages
    - Continue execution after errors
    - Track all errors for final summary
    - _Requirements: 13.4, 14.1, 14.2, 14.3, 14.4, 14.5_

- [ ]* 13.3 Write property test for verbose logging
  - **Property 13: Verbose Logging Completeness**
  - **Validates: Requirements 13.1, 13.2, 13.3, 13.4**

- [ ]* 13.4 Write property test for graceful error handling
  - **Property 14: Graceful Missing File Handling**
  - **Validates: Requirements 14.1, 14.2, 14.3, 14.4, 14.5**

- [ ] 14. Implement dry-run mode verification
  - Verify -DryRun flag prevents all file modifications
  - Verify dry-run displays all planned operations
  - Verify dry-run displays summary message
  - _Requirements: 9.1, 9.2, 9.3, 9.4_

- [ ]* 14.1 Write property test for dry-run mode
  - **Property 9: Dry Run Mode Non-Modification**
  - **Validates: Requirements 9.1, 9.2, 9.3**

- [ ] 15. Implement Git backup functionality
  - Create backup branch "backup-before-reorganization"
  - Commit all current changes to backup branch
  - Create feature branch "feature/project-reorganization"
  - Display backup branch name to user
  - _Requirements: 15.1, 15.2, 15.3, 15.4_

- [ ] 16. Implement reorganization summary report
  - Track total files processed, moved, failed
  - Track namespaces updated and using statements updated
  - Track directories created and files deleted
  - Display build and test results
  - Display execution duration
  - List all errors and warnings
  - _Requirements: 9.4, 13.1, 13.2, 13.3, 13.4_

- [ ] 17. Final checkpoint - Complete reorganization verification
  - Run full reorganization with -Verbose flag
  - Verify all files moved correctly
  - Verify all namespaces updated
  - Verify all using statements updated
  - Verify build succeeds
  - Verify tests pass
  - Verify application starts successfully
  - Verify API endpoints are accessible
  - Ensure all tests pass, ask the user if questions arise.

## Notes

- Tasks marked with `*` are optional and can be skipped for faster MVP
- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation
- Property tests validate universal correctness properties
- The PowerShell script should be idempotent and safe to run multiple times
- Dry-run mode allows previewing changes before applying them
- Verbose logging helps troubleshoot issues during reorganization
