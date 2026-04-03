# Design: Single Responsibility Principle (SRP) Refactoring

## Overview

This design document outlines the technical approach for refactoring large, multi-responsibility services in the Bank API to follow the Single Responsibility Principle. The refactoring decomposes monolithic services into focused, single-purpose services while maintaining backward compatibility through facade patterns.

## Architecture

### Current vs. Proposed Architecture

**Current Architecture (Monolithic)**:
```
BillPaymentService (470+ lines)
├── Biller Management
├── Payment Scheduling
├── Payment Processing
├── Payment History
├── Payment Validation
└── DTO Mapping

NotificationService (520+ lines)
├── Generic Notification Sending
├── Specialized Alerts
├── Preference Management
├── History Management
├── Channel-Specific Sending
└── Scheduled Processing

StatementService (850+ lines)
├── Statement Generation
├── Statement Delivery
├── Statement Querying
├── Statement Analytics
├── Template Management
└── Validation
```

**Proposed Architecture (Decomposed)**:
```
BillPaymentService (Facade)
├── BillerService
├── BillPaymentProcessingService
├── BillPaymentQueryService
├── BillPaymentValidationService
└── BillPaymentMappingService

NotificationService (Facade)
├── NotificationSenderService
├── NotificationAlertService
├── NotificationPreferenceService
├── NotificationHistoryService
├── NotificationChannelService
└── NotificationSchedulerService

StatementService (Facade)
├── StatementGenerationService
├── StatementDeliveryService
├── StatementQueryService
├── StatementAnalyticsService
├── StatementTemplateService
└── StatementValidationService
```

### Service Decomposition Strategy

#### 1. BillPaymentService Decomposition

**BillerService** (Biller Management)
- Responsibility: Manage biller data and queries
- Methods:
  - `GetAvailableBillersAsync()`
  - `GetBillersByCategoryAsync(category)`
  - `SearchBillersAsync(request)`
  - `GetBillerByIdAsync(billerId)`
- Dependencies: IBillerRepository, ILogger

**BillPaymentProcessingService** (Payment Lifecycle)
- Responsibility: Process payment scheduling and lifecycle
- Methods:
  - `ScheduleBillPaymentAsync(customerId, request)`
  - `ProcessBillPaymentAsync(processingDate)`
  - `CancelScheduledPaymentAsync(customerId, paymentId)`
  - `UpdateScheduledPaymentAsync(customerId, paymentId, request)`
- Dependencies: IBillPaymentRepository, IAccountService, ITransactionService, IBillerIntegrationService, IPaymentRetryService, IPaymentReceiptService, IUnitOfWork, ILogger

**BillPaymentQueryService** (Payment Queries)
- Responsibility: Retrieve and filter payment data
- Methods:
  - `GetBillPaymentHistoryAsync(customerId, request)`
  - `GetPendingBillPaymentsAsync(customerId)`
  - `GetBillPaymentByIdAsync(customerId, paymentId)`
- Dependencies: IBillPaymentRepository, ILogger

**BillPaymentValidationService** (Payment Validation)
- Responsibility: Validate payment requests
- Methods:
  - `ValidateBillPaymentAsync(customerId, request)`
- Dependencies: IBillerRepository, IAccountService, ILogger

**BillPaymentMappingService** (DTO Mapping)
- Responsibility: Map domain entities to DTOs
- Methods:
  - `MapToBillerDto(biller)`
  - `MapToBillPaymentDto(payment)`
  - `MapToBillPaymentHistoryDto(payment)`
- Dependencies: None (static utility methods or instance methods)

**BillPaymentService (Facade)**
- Responsibility: Provide backward-compatible interface
- Delegates to: BillerService, BillPaymentProcessingService, BillPaymentQueryService, BillPaymentValidationService, BillPaymentMappingService
- Methods: All original methods, delegating to appropriate services

#### 2. NotificationService Decomposition

**NotificationSenderService** (Core Notification Sending)
- Responsibility: Send notifications to users
- Methods:
  - `SendNotificationAsync(request)`
  - `SendNotificationNow(notification)`
- Dependencies: IUnitOfWork, ILogger

**NotificationAlertService** (Specialized Alerts)
- Responsibility: Send domain-specific alerts
- Methods:
  - `SendTransactionAlertAsync(request)`
  - `SendSecurityAlertAsync(request)`
  - `SendLowBalanceAlertAsync(userId, accountId, balance, threshold)`
  - `SendCardAlertAsync(userId, cardId, alertType, message)`
  - `SendPaymentReminderAsync(userId, paymentType, amount, dueDate)`
- Dependencies: NotificationSenderService, ILogger

**NotificationPreferenceService** (User Preferences)
- Responsibility: Manage user notification preferences
- Methods:
  - `UpdatePreferencesAsync(request)`
  - `GetPreferencesAsync(userId)`
  - `GetUserPreferencesAsync(userId)`
- Dependencies: IUnitOfWork, ILogger

**NotificationHistoryService** (History & Status)
- Responsibility: Retrieve and manage notification history
- Methods:
  - `GetNotificationHistoryAsync(userId, page, pageSize)`
  - `MarkAsReadAsync(notificationId, userId)`
  - `GetUnreadCountAsync(userId)`
- Dependencies: IUnitOfWork, ILogger

**NotificationChannelService** (Channel-Specific Sending)
- Responsibility: Send notifications via specific channels
- Methods:
  - `SendEmailNotification(notification)`
  - `SendSmsNotification(notification)`
  - `SendPushNotification(notification)`
- Dependencies: IEmailService, ISmsService, ILogger

**NotificationSchedulerService** (Scheduled & Retry Logic)
- Responsibility: Handle scheduled and failed notifications
- Methods:
  - `ProcessScheduledNotificationsAsync()`
  - `RetryFailedNotificationsAsync()`
- Dependencies: IUnitOfWork, NotificationSenderService, ILogger

**NotificationService (Facade)**
- Responsibility: Provide backward-compatible interface
- Delegates to: All above services
- Methods: All original methods, delegating to appropriate services

#### 3. StatementService Decomposition

**StatementGenerationService** (Statement Creation)
- Responsibility: Generate statement documents
- Methods:
  - `GenerateStatementAsync(request, requestedByUserId)`
  - `GenerateConsolidatedStatementAsync(request, requestedByUserId)`
  - `RegenerateStatementAsync(statementId, requestedByUserId)`
- Dependencies: IUnitOfWork, IStatementGenerator, IAuditLogService, ILogger

**StatementDeliveryService** (Statement Delivery)
- Responsibility: Deliver statements to users
- Methods:
  - `DeliverStatementAsync(statementId, deliveryMethod, deliveryAddress)`
  - `GetDeliveryStatusAsync(statementId)`
  - `DeliverViaEmailAsync(statement, deliveryAddress)`
  - `DeliverViaSmsAsync(statement, deliveryAddress)`
- Dependencies: IUnitOfWork, IEmailService, ISmsService, ILogger

**StatementQueryService** (Statement Retrieval)
- Responsibility: Query and retrieve statements
- Methods:
  - `GetStatementByIdAsync(statementId)`
  - `SearchStatementsAsync(criteria)`
  - `GetAccountStatementsAsync(accountId, limit)`
- Dependencies: IUnitOfWork, ILogger

**StatementAnalyticsService** (Summary & Analytics)
- Responsibility: Calculate statement summaries and analytics
- Methods:
  - `GetStatementSummaryAsync(accountId, startDate, endDate)`
  - `CalculateStatistics(transactions)`
- Dependencies: IUnitOfWork, ILogger

**StatementTemplateService** (Template Management)
- Responsibility: Manage statement templates
- Methods:
  - `GetAvailableTemplatesAsync()`
- Dependencies: None (returns static templates)

**StatementValidationService** (Request Validation)
- Responsibility: Validate statement requests
- Methods:
  - `ValidateStatementRequestAsync(request)`
- Dependencies: IUnitOfWork, ILogger

**StatementService (Facade)**
- Responsibility: Provide backward-compatible interface
- Delegates to: All above services
- Methods: All original methods, delegating to appropriate services

### Facade Pattern Implementation

The facade pattern maintains backward compatibility while delegating to specialized services:

```csharp
public class BillPaymentService : IBillPaymentService
{
    private readonly IBillerService _billerService;
    private readonly IBillPaymentProcessingService _processingService;
    private readonly IBillPaymentQueryService _queryService;
    private readonly IBillPaymentValidationService _validationService;
    private readonly IBillPaymentMappingService _mappingService;

    public BillPaymentService(
        IBillerService billerService,
        IBillPaymentProcessingService processingService,
        IBillPaymentQueryService queryService,
        IBillPaymentValidationService validationService,
        IBillPaymentMappingService mappingService)
    {
        _billerService = billerService;
        _processingService = processingService;
        _queryService = queryService;
        _validationService = validationService;
        _mappingService = mappingService;
    }

    // Delegate to appropriate service
    public async Task<List<BillerDto>> GetAvailableBillersAsync()
        => await _billerService.GetAvailableBillersAsync();

    public async Task<ScheduleBillPaymentResponse> ScheduleBillPaymentAsync(
        Guid customerId, ScheduleBillPaymentRequest request)
        => await _orchestrationService.ScheduleBillPaymentAsync(customerId, request);

    // ... other methods delegate similarly
}
```

### Cross-Cutting Concerns Extraction

#### DtoMappingService

**Responsibility**: Centralize DTO mapping logic

**Methods**:
- `MapToBillerDto(biller)`
- `MapToBillPaymentDto(payment)`
- `MapToBillPaymentHistoryDto(payment)`
- `MapToNotificationDto(notification)`
- `MapToStatementDto(statement)`
- ... (other mappings)

**Benefits**:
- Single source of truth for mapping logic
- Easier to maintain and test
- Reusable across services

#### ValidationService

**Responsibility**: Centralize validation logic

**Methods**:
- `ValidateBillPaymentAsync(customerId, request)`
- `ValidateStatementRequestAsync(request)`
- `ValidateNotificationPreferencesAsync(request)`
- ... (other validations)

**Benefits**:
- Consistent validation across services
- Easier to update validation rules
- Reusable validation logic

#### FileOperationService

**Responsibility**: Handle file operations

**Methods**:
- `SaveStatementFileAsync(content, fileName, statementId)`
- `ReadStatementFileAsync(filePath)`
- `DeleteStatementFileAsync(filePath)`
- `CalculateFileHashAsync(content)`

**Benefits**:
- Centralized file handling
- Consistent error handling
- Easier to test file operations

### Dependency Injection Registration

**ServiceCollectionExtensions.cs** (Updated):

```csharp
public static IServiceCollection AddBillPaymentServices(this IServiceCollection services)
{
    // Register individual services
    services.AddScoped<IBillerService, BillerService>();
    services.AddScoped<IBillPaymentProcessingService, BillPaymentProcessingService>();
    services.AddScoped<IBillPaymentQueryService, BillPaymentQueryService>();
    services.AddScoped<IBillPaymentValidationService, BillPaymentValidationService>();
    services.AddScoped<IBillPaymentMappingService, BillPaymentMappingService>();
    
    // Register facade for backward compatibility
    services.AddScoped<IBillPaymentService, BillPaymentService>();
    
    return services;
}

public static IServiceCollection AddNotificationServices(this IServiceCollection services)
{
    // Register individual services
    services.AddScoped<INotificationSenderService, NotificationSenderService>();
    services.AddScoped<INotificationAlertService, NotificationAlertService>();
    services.AddScoped<INotificationPreferenceService, NotificationPreferenceService>();
    services.AddScoped<INotificationHistoryService, NotificationHistoryService>();
    services.AddScoped<INotificationChannelService, NotificationChannelService>();
    services.AddScoped<INotificationSchedulerService, NotificationSchedulerService>();
    
    // Register facade for backward compatibility
    services.AddScoped<INotificationService, NotificationService>();
    
    return services;
}

public static IServiceCollection AddStatementServices(this IServiceCollection services)
{
    // Register individual services
    services.AddScoped<IStatementGenerationService, StatementGenerationService>();
    services.AddScoped<IStatementDeliveryService, StatementDeliveryService>();
    services.AddScoped<IStatementQueryService, StatementQueryService>();
    services.AddScoped<IStatementAnalyticsService, StatementAnalyticsService>();
    services.AddScoped<IStatementTemplateService, StatementTemplateService>();
    services.AddScoped<IStatementValidationService, StatementValidationService>();
    
    // Register facade for backward compatibility
    services.AddScoped<IStatementService, StatementService>();
    
    return services;
}

public static IServiceCollection AddCrossCuttingServices(this IServiceCollection services)
{
    services.AddScoped<IDtoMappingService, DtoMappingService>();
    services.AddScoped<IValidationService, ValidationService>();
    services.AddScoped<IFileOperationService, FileOperationService>();
    
    return services;
}
```

### Service Interfaces

#### BillPaymentService Interfaces

```csharp
public interface IBillerService
{
    Task<List<BillerDto>> GetAvailableBillersAsync();
    Task<List<BillerDto>> GetBillersByCategoryAsync(BillerCategory category);
    Task<List<BillerDto>> SearchBillersAsync(BillerSearchRequest request);
    Task<BillerDto?> GetBillerByIdAsync(Guid billerId);
}

public interface IBillPaymentProcessingService
{
    Task<ScheduleBillPaymentResponse> ScheduleBillPaymentAsync(Guid customerId, ScheduleBillPaymentRequest request);
    Task<List<ProcessBillPaymentResponse>> ProcessBillPaymentAsync(DateTime? processingDate = null);
    Task<bool> CancelScheduledPaymentAsync(Guid customerId, Guid paymentId);
    Task<bool> UpdateScheduledPaymentAsync(Guid customerId, Guid paymentId, UpdateBillPaymentRequest request);
}

public interface IBillPaymentQueryService
{
    Task<PagedResult<BillPaymentHistoryDto>> GetBillPaymentHistoryAsync(Guid customerId, BillPaymentHistoryRequest request);
    Task<List<BillPaymentDto>> GetPendingBillPaymentsAsync(Guid customerId);
    Task<BillPaymentDto?> GetBillPaymentByIdAsync(Guid customerId, Guid paymentId);
}

public interface IBillPaymentValidationService
{
    Task<(bool IsValid, string ErrorMessage)> ValidateBillPaymentAsync(Guid customerId, ScheduleBillPaymentRequest request);
}

public interface IBillPaymentMappingService
{
    BillerDto MapToBillerDto(Biller biller);
    BillPaymentDto MapToBillPaymentDto(BillPayment payment);
    BillPaymentHistoryDto MapToBillPaymentHistoryDto(BillPayment payment);
}
```

#### NotificationService Interfaces

```csharp
public interface INotificationSenderService
{
    Task<NotificationResponse> SendNotificationAsync(SendNotificationRequest request);
    Task SendNotificationNow(Notification notification);
}

public interface INotificationAlertService
{
    Task<NotificationResponse> SendTransactionAlertAsync(TransactionAlertRequest request);
    Task<NotificationResponse> SendSecurityAlertAsync(SecurityAlertRequest request);
    Task<NotificationResponse> SendLowBalanceAlertAsync(string userId, string accountId, decimal currentBalance, decimal threshold);
    Task<NotificationResponse> SendCardAlertAsync(string userId, string cardId, string alertType, string message);
    Task<NotificationResponse> SendPaymentReminderAsync(string userId, string paymentType, decimal amount, DateTime dueDate);
}

public interface INotificationPreferenceService
{
    Task<bool> UpdatePreferencesAsync(NotificationPreferencesRequest request);
    Task<NotificationPreferencesRequest?> GetPreferencesAsync(string userId);
}

public interface INotificationHistoryService
{
    Task<List<NotificationHistoryItem>> GetNotificationHistoryAsync(string userId, int page = 1, int pageSize = 20);
    Task<bool> MarkAsReadAsync(string notificationId, string userId);
    Task<int> GetUnreadCountAsync(string userId);
}

public interface INotificationChannelService
{
    Task SendEmailNotification(Notification notification);
    Task SendSmsNotification(Notification notification);
    Task SendPushNotification(Notification notification);
}

public interface INotificationSchedulerService
{
    Task ProcessScheduledNotificationsAsync();
    Task RetryFailedNotificationsAsync();
}
```

#### StatementService Interfaces

```csharp
public interface IStatementGenerationService
{
    Task<StatementGenerationResult> GenerateStatementAsync(GenerateStatementRequest request, Guid requestedByUserId);
    Task<StatementGenerationResult> GenerateConsolidatedStatementAsync(ConsolidatedStatementRequest request, Guid requestedByUserId);
    Task<StatementGenerationResult> RegenerateStatementAsync(Guid statementId, Guid requestedByUserId);
}

public interface IStatementDeliveryService
{
    Task<bool> DeliverStatementAsync(Guid statementId, StatementDeliveryMethod deliveryMethod, string deliveryAddress);
    Task<StatementDeliveryStatus> GetDeliveryStatusAsync(Guid statementId);
}

public interface IStatementQueryService
{
    Task<StatementDto?> GetStatementByIdAsync(Guid statementId);
    Task<StatementSearchResult> SearchStatementsAsync(StatementSearchCriteria criteria);
    Task<List<StatementDto>> GetAccountStatementsAsync(Guid accountId, int? limit = null);
}

public interface IStatementAnalyticsService
{
    Task<StatementSummary> GetStatementSummaryAsync(Guid accountId, DateTime startDate, DateTime endDate);
}

public interface IStatementTemplateService
{
    Task<List<StatementTemplate>> GetAvailableTemplatesAsync();
}

public interface IStatementValidationService
{
    Task<(bool IsValid, List<string> Errors)> ValidateStatementRequestAsync(GenerateStatementRequest request);
}
```

### Migration Path

**Phase 1: Create New Services**
1. Create new service interfaces
2. Create new service implementations
3. Register new services in DI container
4. Write unit tests for new services

**Phase 2: Create Facades**
1. Create facade implementations
2. Update facade to delegate to new services
3. Verify backward compatibility
4. Update integration tests

**Phase 3: Update Controllers**
1. Controllers continue using original interfaces (no changes needed)
2. DI container injects facade implementations
3. Facades delegate to new services

**Phase 4: Gradual Migration**
1. New code can inject specific services directly
2. Old code continues using facades
3. Gradually migrate old code to use specific services
4. Eventually remove facades when all code is migrated

## Correctness Properties

### Property 1: Single Responsibility Adherence

*For any* service created during refactoring, the service SHALL have exactly one reason to change, all public methods SHALL relate to that single responsibility, and the service SHALL have fewer than 200 lines of code (excluding comments and blank lines).

**Validation Approach**:
- Analyze each service's public methods
- Verify all methods operate on the same domain concept
- Count lines of code (excluding comments)
- Verify service has single reason to change

### Property 2: Backward Compatibility Preservation

*For any* API endpoint that existed before refactoring, after refactoring the endpoint SHALL be accessible at the same route, accept the same request format, return the same response format, and return the same HTTP status codes.

**Validation Approach**:
- Run existing integration tests
- Compare API responses before and after refactoring
- Verify Swagger documentation matches

### Property 3: Dependency Injection Correctness

*For any* service in the refactored system, the service SHALL be resolvable from the dependency injection container, all dependencies SHALL be registered, and there SHALL be no circular dependencies in the dependency graph.

**Validation Approach**:
- Attempt to resolve each service from DI container
- Analyze dependency graph for cycles
- Verify all dependencies are registered

### Property 4: Interface Preservation

*For any* original service interface (IBillPaymentService, INotificationService, IStatementService), the interface SHALL remain unchanged, all original methods SHALL be available, and the interface SHALL be implemented by a facade or adapter service.

**Validation Approach**:
- Compare original and new interface definitions
- Verify all methods are present
- Verify facade implements all methods

### Property 5: Testability Improvement

*For any* new service created during refactoring, the service SHALL have fewer than 5 public methods (or be a facade), have fewer than 5 dependencies, and all dependencies SHALL be injectable through the constructor.

**Validation Approach**:
- Count public methods in each service
- Count constructor dependencies
- Verify all dependencies are injectable

### Property 6: Service Cohesion

*For any* service created during refactoring, all public methods SHALL operate on the same data model, all methods SHALL contribute to the same business capability, and there SHALL be no methods that could be moved to another service without breaking cohesion.

**Validation Approach**:
- Analyze method groupings
- Verify methods operate on same domain
- Verify no methods are orphaned

### Property 7: Cross-Cutting Concern Extraction

*For any* cross-cutting concern (DTO mapping, validation, file operations), the concern SHALL be implemented in a dedicated service, the service SHALL be reused across multiple services, and duplicate implementations SHALL be removed.

**Validation Approach**:
- Identify cross-cutting concerns
- Verify dedicated services exist
- Search for duplicate implementations
- Verify services are reused

