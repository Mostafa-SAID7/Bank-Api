# Requirements: Single Responsibility Principle (SRP) Refactoring

## Introduction

This document defines requirements for refactoring large, multi-responsibility services in the Bank API to follow the Single Responsibility Principle (SRP). The refactoring addresses services that handle multiple distinct concerns, making them difficult to test, maintain, and extend. Each service will be decomposed into focused, single-purpose services with clear boundaries.

## Glossary

- **Monolithic Service**: A service class handling multiple unrelated responsibilities
- **Single Responsibility**: A service with one reason to change, focused on one business concern
- **Service Decomposition**: Breaking a large service into smaller, focused services
- **Dependency Injection**: Injecting dependencies into services rather than creating them internally
- **Cross-Cutting Concern**: Functionality that spans multiple services (logging, validation, mapping)
- **Domain Service**: A service focused on a specific business domain (e.g., BillPayment, Notification)
- **Infrastructure Service**: A service handling technical concerns (e.g., EmailSending, SmsSending)
- **Application Service**: A service orchestrating domain and infrastructure services
- **Repository Pattern**: Data access abstraction layer
- **DTO Mapping**: Converting between domain entities and data transfer objects

## Current State Analysis

### BillPaymentService (Multiple Responsibilities)

**Current Responsibilities**:
1. Biller management (search, retrieve, filter)
2. Bill payment scheduling and processing
3. Payment history retrieval and filtering
4. Payment cancellation and updates
5. Payment validation
6. DTO mapping

**Issues**:
- 470+ lines of code
- 13 public methods covering 6 distinct concerns
- Difficult to test individual concerns
- Changes to biller logic require modifying payment logic
- Tight coupling between concerns

### NotificationService (Multiple Responsibilities)

**Current Responsibilities**:
1. Generic notification sending
2. Specialized alert sending (transaction, security, low balance, card, payment reminder)
3. Notification history retrieval
4. Notification status management (mark as read, unread count)
5. User preference management
6. Scheduled notification processing
7. Failed notification retry logic
8. Channel-specific sending (email, SMS, push, in-app)

**Issues**:
- 520+ lines of code
- 15 public methods covering 8 distinct concerns
- Mixing notification orchestration with channel-specific logic
- Preference management mixed with notification sending
- Difficult to test channel implementations independently
- Changes to one alert type affect entire service

### StatementService (Multiple Responsibilities)

**Current Responsibilities**:
1. Statement generation and creation
2. Consolidated statement generation
3. Statement retrieval and searching
4. Statement summary calculation
5. Statement file download
6. Statement delivery (email, SMS)
7. Statement regeneration
8. Statement cancellation
9. Template management
10. Request validation

**Issues**:
- 850+ lines of code (truncated in review)
- 10+ public methods covering 10 distinct concerns
- Statement generation mixed with delivery logic
- Summary calculation mixed with retrieval logic
- Difficult to test generation independently from delivery

## Requirements

### Requirement 1: Decompose BillPaymentService

**User Story**: As a developer, I want BillPaymentService decomposed into focused services, so that each service has a single responsibility and is easier to test and maintain.

#### Acceptance Criteria

1. WHEN BillPaymentService is decomposed, THE system SHALL create BillerService handling biller retrieval, search, and filtering
2. WHEN BillPaymentService is decomposed, THE system SHALL create BillPaymentProcessingService handling payment scheduling, processing, and lifecycle
3. WHEN BillPaymentService is decomposed, THE system SHALL create BillPaymentQueryService handling payment history, retrieval, and filtering
4. WHEN BillPaymentService is decomposed, THE system SHALL create BillPaymentValidationService handling all payment validation logic
5. WHEN BillPaymentService is decomposed, THE system SHALL create BillPaymentMappingService handling DTO mapping operations
6. WHEN services are decomposed, THE original BillPaymentService interface SHALL be preserved through a facade or adapter pattern
7. WHEN services are decomposed, THE dependency injection registration SHALL be updated to register all new services
8. WHEN services are decomposed, THE existing API controllers SHALL continue to work without modification

### Requirement 2: Decompose NotificationService

**User Story**: As a developer, I want NotificationService decomposed into focused services, so that notification sending, preference management, and channel handling are separated.

#### Acceptance Criteria

1. WHEN NotificationService is decomposed, THE system SHALL create NotificationSenderService handling core notification sending logic
2. WHEN NotificationService is decomposed, THE system SHALL create NotificationAlertService handling specialized alerts (transaction, security, low balance, card, payment reminder)
3. WHEN NotificationService is decomposed, THE system SHALL create NotificationPreferenceService handling user preference management
4. WHEN NotificationService is decomposed, THE system SHALL create NotificationHistoryService handling notification retrieval and status management
5. WHEN NotificationService is decomposed, THE system SHALL create NotificationChannelService handling channel-specific sending (email, SMS, push, in-app)
6. WHEN NotificationService is decomposed, THE system SHALL create NotificationSchedulerService handling scheduled and retry logic
7. WHEN services are decomposed, THE original NotificationService interface SHALL be preserved through a facade or adapter pattern
8. WHEN services are decomposed, THE dependency injection registration SHALL be updated to register all new services
9. WHEN services are decomposed, THE existing API controllers SHALL continue to work without modification

### Requirement 3: Decompose StatementService

**User Story**: As a developer, I want StatementService decomposed into focused services, so that statement generation, delivery, and querying are separated.

#### Acceptance Criteria

1. WHEN StatementService is decomposed, THE system SHALL create StatementGenerationService handling statement creation and generation logic
2. WHEN StatementService is decomposed, THE system SHALL create StatementDeliveryService handling statement delivery via email and SMS
3. WHEN StatementService is decomposed, THE system SHALL create StatementQueryService handling statement retrieval, searching, and filtering
4. WHEN StatementService is decomposed, THE system SHALL create StatementAnalyticsService handling summary calculation and analytics
5. WHEN StatementService is decomposed, THE system SHALL create StatementTemplateService handling template management
6. WHEN StatementService is decomposed, THE system SHALL create StatementValidationService handling request validation
7. WHEN services are decomposed, THE original StatementService interface SHALL be preserved through a facade or adapter pattern
8. WHEN services are decomposed, THE dependency injection registration SHALL be updated to register all new services
9. WHEN services are decomposed, THE existing API controllers SHALL continue to work without modification

### Requirement 4: Extract Cross-Cutting Concerns

**User Story**: As a developer, I want cross-cutting concerns extracted into dedicated services, so that they can be reused across multiple services.

#### Acceptance Criteria

1. WHEN services are refactored, THE system SHALL create DtoMappingService handling all DTO mapping operations
2. WHEN services are refactored, THE system SHALL create ValidationService handling common validation logic
3. WHEN services are refactored, THE system SHALL create FileOperationService handling file operations (save, read, delete)
4. WHEN services are refactored, THE system SHALL create ChannelService abstraction for notification channels
5. WHEN services are refactored, THE system SHALL create AlertService abstraction for specialized alerts
6. WHEN services are refactored, THE system SHALL remove duplicate mapping logic from individual services
7. WHEN services are refactored, THE system SHALL remove duplicate validation logic from individual services

### Requirement 5: Update Dependency Injection

**User Story**: As a developer, I want dependency injection updated to register all new services, so that the application can resolve all dependencies correctly.

#### Acceptance Criteria

1. WHEN services are decomposed, THE ServiceCollectionExtensions.cs file SHALL be updated to register all new services
2. WHEN services are decomposed, THE original service interfaces SHALL be registered as facades or adapters
3. WHEN services are decomposed, THE dependency graph SHALL be acyclic (no circular dependencies)
4. WHEN the application starts, THE dependency injection container SHALL resolve all services without errors
5. WHEN services are decomposed, THE registration order SHALL respect service dependencies

### Requirement 6: Maintain Backward Compatibility

**User Story**: As an API consumer, I want all existing endpoints to continue working after refactoring, so that existing integrations are not broken.

#### Acceptance Criteria

1. WHEN services are refactored, THE existing API endpoints SHALL remain accessible at the same routes
2. WHEN services are refactored, THE request/response formats SHALL remain unchanged
3. WHEN services are refactored, THE HTTP status codes SHALL remain consistent
4. WHEN services are refactored, THE error messages SHALL remain consistent
5. WHEN services are refactored, THE Swagger documentation SHALL display the same endpoints

### Requirement 7: Improve Testability

**User Story**: As a developer, I want services to be more testable, so that unit tests can focus on individual responsibilities.

#### Acceptance Criteria

1. WHEN services are decomposed, EACH service SHALL have a single, clear responsibility
2. WHEN services are decomposed, EACH service SHALL have fewer than 5 public methods (or be a facade)
3. WHEN services are decomposed, EACH service SHALL have fewer than 200 lines of code (excluding comments)
4. WHEN services are decomposed, EACH service SHALL have minimal dependencies (fewer than 5)
5. WHEN services are decomposed, UNIT tests SHALL be able to mock all dependencies
6. WHEN services are decomposed, UNIT tests SHALL focus on one concern per test class

### Requirement 8: Update Service Interfaces

**User Story**: As a developer, I want service interfaces updated to reflect new responsibilities, so that the contract is clear.

#### Acceptance Criteria

1. WHEN services are decomposed, NEW service interfaces SHALL be created for each new service
2. WHEN services are decomposed, EXISTING service interfaces SHALL be preserved for backward compatibility
3. WHEN services are decomposed, INTERFACE methods SHALL be organized by responsibility
4. WHEN services are decomposed, INTERFACE documentation SHALL clearly describe the service's responsibility

### Requirement 9: Update Service Registration

**User Story**: As a developer, I want service registration organized by domain, so that it's easy to understand the service architecture.

#### Acceptance Criteria

1. WHEN services are registered, THE registration code SHALL be organized by domain (Payment, Notification, Statement)
2. WHEN services are registered, THE registration code SHALL use extension methods for each domain
3. WHEN services are registered, THE registration code SHALL include comments explaining each service's responsibility
4. WHEN services are registered, THE registration code SHALL be easy to extend with new services

### Requirement 10: Provide Migration Guide

**User Story**: As a developer, I want a migration guide for the refactoring, so that I understand how to update code that depends on the old services.

#### Acceptance Criteria

1. THE migration guide SHALL document the old service structure and new service structure
2. THE migration guide SHALL provide examples of how to update code that uses the old services
3. THE migration guide SHALL explain the facade pattern used for backward compatibility
4. THE migration guide SHALL list all new services and their responsibilities
5. THE migration guide SHALL provide troubleshooting tips for common issues

## Correctness Properties

### Property 1: Single Responsibility Adherence

*For any* service created during refactoring, the service SHALL have exactly one reason to change, all public methods SHALL relate to that single responsibility, and the service SHALL have fewer than 200 lines of code (excluding comments and blank lines).

**Validates: Requirements 1.1-1.5, 2.1-2.6, 3.1-3.6, 7.1-7.3**

### Property 2: Backward Compatibility Preservation

*For any* API endpoint that existed before refactoring, after refactoring the endpoint SHALL be accessible at the same route, accept the same request format, return the same response format, and return the same HTTP status codes.

**Validates: Requirements 6.1-6.5**

### Property 3: Dependency Injection Correctness

*For any* service in the refactored system, the service SHALL be resolvable from the dependency injection container, all dependencies SHALL be registered, and there SHALL be no circular dependencies in the dependency graph.

**Validates: Requirements 5.1-5.5**

### Property 4: Interface Preservation

*For any* original service interface (IBillPaymentService, INotificationService, IStatementService), the interface SHALL remain unchanged, all original methods SHALL be available, and the interface SHALL be implemented by a facade or adapter service.

**Validates: Requirements 1.6, 2.7, 3.7, 6.1-6.5**

### Property 5: Testability Improvement

*For any* new service created during refactoring, the service SHALL have fewer than 5 public methods (or be a facade), have fewer than 5 dependencies, and all dependencies SHALL be injectable through the constructor.

**Validates: Requirements 7.1-7.6**

### Property 6: Service Cohesion

*For any* service created during refactoring, all public methods SHALL operate on the same data model, all methods SHALL contribute to the same business capability, and there SHALL be no methods that could be moved to another service without breaking cohesion.

**Validates: Requirements 1.1-1.5, 2.1-2.6, 3.1-3.6**

### Property 7: Cross-Cutting Concern Extraction

*For any* cross-cutting concern (DTO mapping, validation, file operations), the concern SHALL be implemented in a dedicated service, the service SHALL be reused across multiple services, and duplicate implementations SHALL be removed.

**Validates: Requirement 4.1-4.7**

