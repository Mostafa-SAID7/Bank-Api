# Implementation Tasks: SRP Refactoring

## Overview

This document outlines the implementation tasks for refactoring large services to follow the Single Responsibility Principle. The refactoring is organized into phases: service decomposition, facade creation, DI registration, testing, and migration.

## Tasks

### Phase 1: BillPaymentService Decomposition

- [x] 1. Create BillerService
  - [x] 1.1 Create IBillerService interface
    - Define GetAvailableBillersAsync()
    - Define GetBillersByCategoryAsync(category)
    - Define SearchBillersAsync(request)
    - Define GetBillerByIdAsync(billerId)
    - _Requirements: 1.1_

  - [x] 1.2 Create BillerService implementation
    - Extract biller-related methods from BillPaymentService
    - Implement all interface methods
    - Add logging
    - _Requirements: 1.1_

  - [x] 1.3 Write unit tests for BillerService
    - Test GetAvailableBillersAsync()
    - Test GetBillersByCategoryAsync()
    - Test SearchBillersAsync()
    - Test GetBillerByIdAsync()
    - _Requirements: 7.1-7.6_

- [x] 2. Create BillPaymentProcessingService
  - [x] 2.1 Create IBillPaymentProcessingService interface
    - Define ScheduleBillPaymentAsync()
    - Define ProcessBillPaymentAsync()
    - Define CancelScheduledPaymentAsync()
    - Define UpdateScheduledPaymentAsync()
    - _Requirements: 1.2_

  - [x] 2.2 Create BillPaymentProcessingService implementation
    - Extract payment lifecycle methods from BillPaymentService
    - Implement all interface methods
    - Add logging and error handling
    - _Requirements: 1.2_

  - [x] 2.3 Write unit tests for BillPaymentProcessingService
    - Test ScheduleBillPaymentAsync()
    - Test ProcessBillPaymentAsync()
    - Test CancelScheduledPaymentAsync()
    - Test UpdateScheduledPaymentAsync()
    - _Requirements: 7.1-7.6_

- [ ] 3. Create BillPaymentQueryService
  - [ ] 3.1 Create IBillPaymentQueryService interface
    - Define GetBillPaymentHistoryAsync()
    - Define GetPendingBillPaymentsAsync()
    - Define GetBillPaymentByIdAsync()
    - _Requirements: 1.3_

  - [ ] 3.2 Create BillPaymentQueryService implementation
    - Extract query methods from BillPaymentService
    - Implement all interface methods
    - Add logging
    - _Requirements: 1.3_

  - [ ] 3.3 Write unit tests for BillPaymentQueryService
    - Test GetBillPaymentHistoryAsync()
    - Test GetPendingBillPaymentsAsync()
    - Test GetBillPaymentByIdAsync()
    - _Requirements: 7.1-7.6_

- [ ] 4. Create BillPaymentValidationService
  - [ ] 4.1 Create IBillPaymentValidationService interface
    - Define ValidateBillPaymentAsync()
    - _Requirements: 1.4_

  - [ ] 4.2 Create BillPaymentValidationService implementation
    - Extract validation logic from BillPaymentService
    - Implement all interface methods
    - Add logging
    - _Requirements: 1.4_

  - [ ] 4.3 Write unit tests for BillPaymentValidationService
    - Test ValidateBillPaymentAsync() with valid input
    - Test ValidateBillPaymentAsync() with invalid input
    - _Requirements: 7.1-7.6_

- [x] 5. Create BillPaymentMappingService
  - [x] 5.1 Create IBillPaymentMappingService interface
    - Define MapToBillPaymentDto()
    - Define MapToBillPaymentHistoryDto()
    - _Requirements: 1.5_

  - [x] 5.2 Create BillPaymentMappingService implementation
    - Extract mapping methods from BillPaymentService
    - Implement all interface methods
    - _Requirements: 1.5_

  - [x] 5.3 Consolidate mapping methods (MAPPING CONSOLIDATION)
    - Remove MapToBillerDto() from BillPaymentService (duplicate)
    - Remove MapToBillPaymentDto() from BillPaymentService (moved to mapping service)
    - Remove MapToBillPaymentHistoryDto() from BillPaymentService (moved to mapping service)
    - Remove MapToBillPresentmentDto() from BillerIntegrationService (duplicate)
    - _Requirements: 1.5_

  - [ ] 5.4 Write unit tests for BillPaymentMappingService
    - Test MapToBillPaymentDto()
    - Test MapToBillPaymentHistoryDto()
    - _Requirements: 7.1-7.6_

- [ ] 6. Create BillPaymentService Facade
  - [ ] 6.1 Update BillPaymentService to be a facade
    - Inject all new services
    - Delegate all methods to appropriate services
    - Preserve original interface
    - _Requirements: 1.6_

  - [ ] 6.2 Write integration tests for BillPaymentService facade
    - Test all original methods still work
    - Test delegation to new services
    - _Requirements: 6.1-6.5_

### Phase 2: NotificationService Decomposition

- [ ] 7. Create NotificationSenderService
  - [ ] 7.1 Create INotificationSenderService interface
    - Define SendNotificationAsync()
    - Define SendNotificationNow()
    - _Requirements: 2.1_

  - [ ] 7.2 Create NotificationSenderService implementation
    - Extract core sending logic from NotificationService
    - Implement all interface methods
    - Add logging
    - _Requirements: 2.1_

  - [ ] 7.3 Write unit tests for NotificationSenderService
    - Test SendNotificationAsync()
    - Test SendNotificationNow()
    - _Requirements: 7.1-7.6_

- [ ] 8. Create NotificationAlertService
  - [ ] 8.1 Create INotificationAlertService interface
    - Define SendTransactionAlertAsync()
    - Define SendSecurityAlertAsync()
    - Define SendLowBalanceAlertAsync()
    - Define SendCardAlertAsync()
    - Define SendPaymentReminderAsync()
    - _Requirements: 2.2_

  - [ ] 8.2 Create NotificationAlertService implementation
    - Extract alert methods from NotificationService
    - Implement all interface methods
    - Add logging
    - _Requirements: 2.2_

  - [ ] 8.3 Write unit tests for NotificationAlertService
    - Test each alert type
    - _Requirements: 7.1-7.6_

- [ ] 9. Create NotificationPreferenceService
  - [ ] 9.1 Create INotificationPreferenceService interface
    - Define UpdatePreferencesAsync()
    - Define GetPreferencesAsync()
    - _Requirements: 2.3_

  - [ ] 9.2 Create NotificationPreferenceService implementation
    - Extract preference methods from NotificationService
    - Implement all interface methods
    - Add logging
    - _Requirements: 2.3_

  - [ ] 9.3 Write unit tests for NotificationPreferenceService
    - Test UpdatePreferencesAsync()
    - Test GetPreferencesAsync()
    - _Requirements: 7.1-7.6_

- [ ] 10. Create NotificationHistoryService
  - [ ] 10.1 Create INotificationHistoryService interface
    - Define GetNotificationHistoryAsync()
    - Define MarkAsReadAsync()
    - Define GetUnreadCountAsync()
    - _Requirements: 2.4_

  - [ ] 10.2 Create NotificationHistoryService implementation
    - Extract history methods from NotificationService
    - Implement all interface methods
    - Add logging
    - _Requirements: 2.4_

  - [ ] 10.3 Write unit tests for NotificationHistoryService
    - Test GetNotificationHistoryAsync()
    - Test MarkAsReadAsync()
    - Test GetUnreadCountAsync()
    - _Requirements: 7.1-7.6_

- [ ] 11. Create NotificationChannelService
  - [ ] 11.1 Create INotificationChannelService interface
    - Define SendEmailNotification()
    - Define SendSmsNotification()
    - Define SendPushNotification()
    - _Requirements: 2.5_

  - [ ] 11.2 Create NotificationChannelService implementation
    - Extract channel methods from NotificationService
    - Implement all interface methods
    - Add logging
    - _Requirements: 2.5_

  - [ ] 11.3 Write unit tests for NotificationChannelService
    - Test SendEmailNotification()
    - Test SendSmsNotification()
    - Test SendPushNotification()
    - _Requirements: 7.1-7.6_

- [ ] 12. Create NotificationSchedulerService
  - [ ] 12.1 Create INotificationSchedulerService interface
    - Define ProcessScheduledNotificationsAsync()
    - Define RetryFailedNotificationsAsync()
    - _Requirements: 2.6_

  - [ ] 12.2 Create NotificationSchedulerService implementation
    - Extract scheduler methods from NotificationService
    - Implement all interface methods
    - Add logging
    - _Requirements: 2.6_

  - [ ] 12.3 Write unit tests for NotificationSchedulerService
    - Test ProcessScheduledNotificationsAsync()
    - Test RetryFailedNotificationsAsync()
    - _Requirements: 7.1-7.6_

- [ ] 13. Create NotificationService Facade
  - [ ] 13.1 Update NotificationService to be a facade
    - Inject all new services
    - Delegate all methods to appropriate services
    - Preserve original interface
    - _Requirements: 2.7_

  - [ ] 13.2 Write integration tests for NotificationService facade
    - Test all original methods still work
    - Test delegation to new services
    - _Requirements: 6.1-6.5_

### Phase 3: StatementService Decomposition

- [ ] 14. Create StatementGenerationService
  - [ ] 14.1 Create IStatementGenerationService interface
    - Define GenerateStatementAsync()
    - Define GenerateConsolidatedStatementAsync()
    - Define RegenerateStatementAsync()
    - _Requirements: 3.1_

  - [ ] 14.2 Create StatementGenerationService implementation
    - Extract generation methods from StatementService
    - Implement all interface methods
    - Add logging
    - _Requirements: 3.1_

  - [ ] 14.3 Write unit tests for StatementGenerationService
    - Test GenerateStatementAsync()
    - Test GenerateConsolidatedStatementAsync()
    - Test RegenerateStatementAsync()
    - _Requirements: 7.1-7.6_

- [ ] 15. Create StatementDeliveryService
  - [ ] 15.1 Create IStatementDeliveryService interface
    - Define DeliverStatementAsync()
    - Define GetDeliveryStatusAsync()
    - _Requirements: 3.2_

  - [ ] 15.2 Create StatementDeliveryService implementation
    - Extract delivery methods from StatementService
    - Implement all interface methods
    - Add logging
    - _Requirements: 3.2_

  - [ ] 15.3 Write unit tests for StatementDeliveryService
    - Test DeliverStatementAsync()
    - Test GetDeliveryStatusAsync()
    - _Requirements: 7.1-7.6_

- [ ] 16. Create StatementQueryService
  - [ ] 16.1 Create IStatementQueryService interface
    - Define GetStatementByIdAsync()
    - Define SearchStatementsAsync()
    - Define GetAccountStatementsAsync()
    - _Requirements: 3.3_

  - [ ] 16.2 Create StatementQueryService implementation
    - Extract query methods from StatementService
    - Implement all interface methods
    - Add logging
    - _Requirements: 3.3_

  - [ ] 16.3 Write unit tests for StatementQueryService
    - Test GetStatementByIdAsync()
    - Test SearchStatementsAsync()
    - Test GetAccountStatementsAsync()
    - _Requirements: 7.1-7.6_

- [ ] 17. Create StatementAnalyticsService
  - [ ] 17.1 Create IStatementAnalyticsService interface
    - Define GetStatementSummaryAsync()
    - _Requirements: 3.4_

  - [ ] 17.2 Create StatementAnalyticsService implementation
    - Extract analytics methods from StatementService
    - Implement all interface methods
    - Add logging
    - _Requirements: 3.4_

  - [ ] 17.3 Write unit tests for StatementAnalyticsService
    - Test GetStatementSummaryAsync()
    - _Requirements: 7.1-7.6_

- [ ] 18. Create StatementTemplateService
  - [ ] 18.1 Create IStatementTemplateService interface
    - Define GetAvailableTemplatesAsync()
    - _Requirements: 3.5_

  - [ ] 18.2 Create StatementTemplateService implementation
    - Extract template methods from StatementService
    - Implement all interface methods
    - _Requirements: 3.5_

  - [ ] 18.3 Write unit tests for StatementTemplateService
    - Test GetAvailableTemplatesAsync()
    - _Requirements: 7.1-7.6_

- [ ] 19. Create StatementValidationService
  - [ ] 19.1 Create IStatementValidationService interface
    - Define ValidateStatementRequestAsync()
    - _Requirements: 3.6_

  - [ ] 19.2 Create StatementValidationService implementation
    - Extract validation methods from StatementService
    - Implement all interface methods
    - Add logging
    - _Requirements: 3.6_

  - [ ] 19.3 Write unit tests for StatementValidationService
    - Test ValidateStatementRequestAsync()
    - _Requirements: 7.1-7.6_

- [ ] 20. Create StatementService Facade
  - [ ] 20.1 Update StatementService to be a facade
    - Inject all new services
    - Delegate all methods to appropriate services
    - Preserve original interface
    - _Requirements: 3.7_

  - [ ] 20.2 Write integration tests for StatementService facade
    - Test all original methods still work
    - Test delegation to new services
    - _Requirements: 6.1-6.5_

### Phase 4: Cross-Cutting Concerns

- [ ] 21. Create DtoMappingService
  - [ ] 21.1 Create IDtoMappingService interface
    - Define mapping methods for all DTOs
    - _Requirements: 4.1_

  - [ ] 21.2 Create DtoMappingService implementation
    - Consolidate mapping logic from all services
    - Implement all interface methods
    - _Requirements: 4.1, 4.6_

  - [ ] 21.3 Update services to use DtoMappingService
    - Replace inline mapping with service calls
    - _Requirements: 4.6_

  - [ ] 21.4 Write unit tests for DtoMappingService
    - Test all mapping methods
    - _Requirements: 7.1-7.6_

- [ ] 22. Create ValidationService
  - [ ] 22.1 Create IValidationService interface
    - Define validation methods for all entities
    - _Requirements: 4.2_

  - [ ] 22.2 Create ValidationService implementation
    - Consolidate validation logic from all services
    - Implement all interface methods
    - _Requirements: 4.2, 4.7_

  - [ ] 22.3 Update services to use ValidationService
    - Replace inline validation with service calls
    - _Requirements: 4.7_

  - [ ] 22.4 Write unit tests for ValidationService
    - Test all validation methods
    - _Requirements: 7.1-7.6_

- [ ] 23. Create FileOperationService
  - [ ] 23.1 Create IFileOperationService interface
    - Define file operation methods
    - _Requirements: 4.3_

  - [ ] 23.2 Create FileOperationService implementation
    - Consolidate file operations from all services
    - Implement all interface methods
    - _Requirements: 4.3_

  - [ ] 23.3 Update services to use FileOperationService
    - Replace inline file operations with service calls
    - _Requirements: 4.3_

  - [ ] 23.4 Write unit tests for FileOperationService
    - Test all file operation methods
    - _Requirements: 7.1-7.6_

### Phase 5: Dependency Injection

- [ ] 24. Update ServiceCollectionExtensions
  - [ ] 24.1 Create AddBillPaymentServices() extension method
    - Register all BillPayment services
    - Register facade
    - _Requirements: 5.1, 5.2_

  - [ ] 24.2 Create AddNotificationServices() extension method
    - Register all Notification services
    - Register facade
    - _Requirements: 5.1, 5.2_

  - [ ] 24.3 Create AddStatementServices() extension method
    - Register all Statement services
    - Register facade
    - _Requirements: 5.1, 5.2_

  - [ ] 24.4 Create AddCrossCuttingServices() extension method
    - Register all cross-cutting services
    - _Requirements: 5.1, 5.2_

  - [ ] 24.5 Verify dependency graph is acyclic
    - Analyze all service dependencies
    - Ensure no circular dependencies
    - _Requirements: 5.3_

  - [ ] 24.6 Write unit tests for DI registration
    - Test all services can be resolved
    - Test facades are registered correctly
    - _Requirements: 5.4, 5.5_

### Phase 6: Backward Compatibility

- [ ] 25. Verify API Endpoint Compatibility
  - [ ] 25.1 Run existing integration tests
    - Verify all endpoints still work
    - Verify response formats unchanged
    - _Requirements: 6.1-6.5_

  - [ ] 25.2 Verify Swagger documentation
    - Verify all endpoints are documented
    - Verify documentation is accurate
    - _Requirements: 6.1-6.5_

  - [ ] 25.3 Test API with sample requests
    - Test each endpoint with sample data
    - Verify responses are correct
    - _Requirements: 6.1-6.5_

### Phase 7: Testing & Verification

- [ ] 26. Run comprehensive test suite
  - [ ] 26.1 Run all unit tests
    - Verify all new services have tests
    - Verify all tests pass
    - _Requirements: 7.1-7.6_

  - [ ] 26.2 Run all integration tests
    - Verify facades work correctly
    - Verify backward compatibility
    - _Requirements: 6.1-6.5_

  - [ ] 26.3 Run end-to-end tests
    - Test complete workflows
    - Verify application works correctly
    - _Requirements: 6.1-6.5_

  - [ ] 26.4 Verify code coverage
    - Ensure all new services have adequate coverage
    - Ensure facades have adequate coverage
    - _Requirements: 7.1-7.6_

- [ ] 27. Verify SRP Adherence
  - [ ] 27.1 Analyze each service for single responsibility
    - Verify each service has one reason to change
    - Verify all methods relate to single responsibility
    - _Requirements: 1.1-1.5, 2.1-2.6, 3.1-3.6, 7.1-7.3_

  - [ ] 27.2 Verify service size constraints
    - Verify each service has fewer than 200 lines
    - Verify each service has fewer than 5 public methods (or is facade)
    - _Requirements: 7.1-7.3_

  - [ ] 27.3 Verify dependency constraints
    - Verify each service has fewer than 5 dependencies
    - Verify all dependencies are injectable
    - _Requirements: 7.4-7.6_

### Phase 8: Documentation & Migration

- [ ] 28. Create Migration Guide
  - [ ] 28.1 Document old vs. new service structure
    - Create comparison table
    - Document service responsibilities
    - _Requirements: 10.1_

  - [ ] 28.2 Provide code examples
    - Show how to use new services
    - Show how to migrate from old services
    - _Requirements: 10.2_

  - [ ] 28.3 Explain facade pattern
    - Document facade pattern usage
    - Explain backward compatibility approach
    - _Requirements: 10.3_

  - [ ] 28.4 List all new services
    - Document each new service
    - Document service responsibilities
    - _Requirements: 10.4_

  - [ ] 28.5 Provide troubleshooting guide
    - Document common issues
    - Provide solutions
    - _Requirements: 10.5_

- [ ] 29. Update project documentation
  - [ ] 29.1 Update STRUCTURE.md
    - Document new service organization
    - Update architecture diagrams
    - _Requirements: 10.1_

  - [ ] 29.2 Update README.md
    - Reference new service structure
    - Update architecture overview
    - _Requirements: 10.1_

  - [ ] 29.3 Create SRP_REFACTORING.md
    - Document refactoring approach
    - Document design decisions
    - _Requirements: 10.1_

### Phase 9: Final Verification

- [ ] 30. Final checkpoint - Complete SRP refactoring verification
  - [ ] 30.1 Verify all services are decomposed
    - Confirm all new services exist
    - Confirm all facades exist
    - _Requirements: 1.1-1.5, 2.1-2.6, 3.1-3.6_

  - [ ] 30.2 Verify all tests pass
    - Run full test suite
    - Verify no test failures
    - _Requirements: 7.1-7.6_

  - [ ] 30.3 Verify backward compatibility
    - Run integration tests
    - Verify all endpoints work
    - _Requirements: 6.1-6.5_

  - [ ] 30.4 Verify DI registration
    - Verify all services resolve
    - Verify no circular dependencies
    - _Requirements: 5.1-5.5_

  - [ ] 30.5 Verify SRP adherence
    - Verify each service has single responsibility
    - Verify service size constraints
    - Verify dependency constraints
    - _Requirements: 1.1-1.5, 2.1-2.6, 3.1-3.6, 7.1-7.6_

  - [ ] 30.6 Verify documentation
    - Verify migration guide is complete
    - Verify project documentation is updated
    - _Requirements: 10.1-10.5_

## Notes

- Each task references specific requirements for traceability
- Checkpoints ensure incremental validation
- Property tests validate universal correctness properties
- Facades maintain backward compatibility during migration
- New code can gradually migrate to use specific services
- Eventually facades can be removed when all code is migrated

