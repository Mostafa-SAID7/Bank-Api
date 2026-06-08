# Bank.Application Layer Refactoring Plan

## Overview
This document outlines the systematic refactoring of the Application layer to achieve clean architecture, proper CQRS implementation, and consistency across Commands, Queries, Handlers, and Services.

**Current Status**: 7.5/10 - Good structure with organizational issues
**Target Status**: 9.5/10 - Clean, consistent, complete CQRS

---

## Critical Issues (MUST FIX)

### ISSUE #1: Eliminate MissingDtos.cs (CRITICAL)
**File**: `src/Bank.Application/DTOs/MissingDtos.cs`
**Problem**: 20+ DTOs scattered in single file, violates SRP
**Impact**: CRITICAL - Makes codebase hard to navigate

**DTOs to Relocate**:
```
CardTransactionFilterRequest 
  → src/Bank.Application/DTOs/Card/Transaction/CardTransactionFilterRequest.cs

CreateBeneficiaryRequest 
  → src/Bank.Application/DTOs/Payment/Beneficiary/CreateBeneficiaryRequest.cs

CreateBillPaymentRequest 
  → src/Bank.Application/DTOs/Payment/BillPayment/CreateBillPaymentRequest.cs

CreateJointAccountRequest, JointAccountDto, JointAccountHolderDetailsDto
  → src/Bank.Application/DTOs/Account/JointAccount/

UpdateCardRequest, CreateCardRequest
  → src/Bank.Application/DTOs/Card/Core/

CreateDepositRequest, DepositDto
  → src/Bank.Application/DTOs/Deposit/Core/

UpdateLoanRequest, CreateLoanRequest
  → src/Bank.Application/DTOs/Loan/Core/

CreateLoanPaymentRequest, LoanPaymentDto
  → src/Bank.Application/DTOs/Loan/Payment/

TwoFactorTokenDto, CreateTwoFactorTokenRequest
  → src/Bank.Application/DTOs/Auth/TwoFactor/

UserDto, CreateUserRequest, UpdateUserRequest
  → src/Bank.Application/DTOs/Account/User/

CreateStatementRequest
  → src/Bank.Application/DTOs/Statement/Core/
```

---

### ISSUE #2: Remove Circular Dependency (HIGH SEVERITY)
**File**: `src/Bank.Application/Services/Statement/StatementService.cs` (line 9)
**Problem**: `using Bank.Application.Services;` - self-referential
**Action**: Remove this line immediately
**Impact**: Cleans up namespace pollution

---

### ISSUE #3: Fix InitiateTransactionCommandHandler (HIGH SEVERITY)
**File**: `src/Bank.Application/Commands/Transaction/InitiateTransactionCommand.cs`
**Problem**: Uses `class` instead of `sealed class`
**Action**: Change `public class InitiateTransactionCommandHandler` → `public sealed class`
**Pattern**: All handlers must be sealed to prevent inheritance

---

## High Priority Issues (MUST DO)

### ISSUE #4: Complete Commands Coverage
**Problem**: Only 2 Command categories (Account, Transaction). Missing: Card, Deposit, Loan, Payment, Statement

**Required Commands to Create**:

#### Card Commands
```csharp
// src/Bank.Application/Commands/Card/CreateCardCommand.cs
public sealed record CreateCardCommand(
    Guid CustomerId,
    Guid AccountId,
    CardType CardType,
    string? CardName,
    decimal? DailyLimit,
    decimal? MonthlyLimit
) : IRequest<CardDto>;

// Similar for: ActivateCardCommand, DeactivateCardCommand, UpdateCardCommand
```

#### Deposit Commands
```csharp
// src/Bank.Application/Commands/Deposit/CreateDepositCommand.cs
public sealed record CreateDepositCommand(
    Guid AccountId,
    Guid DepositProductId,
    decimal PrincipalAmount,
    int? TermDays
) : IRequest<DepositDto>;

// Similar for: WithdrawDepositCommand, RenewDepositCommand
```

#### Loan Commands
```csharp
// src/Bank.Application/Commands/Loan/CreateLoanCommand.cs
public sealed record CreateLoanCommand(
    Guid CustomerId,
    LoanType Type,
    decimal RequestedAmount,
    int TermInMonths,
    string Purpose
) : IRequest<LoanDto>;

// Similar for: ApproveLoanCommand, DisburseLoanCommand, RecordPaymentCommand
```

#### Payment Commands
```csharp
// src/Bank.Application/Commands/Payment/CreatePaymentCommand.cs
public sealed record CreatePaymentCommand(
    Guid FromAccountId,
    Guid ToAccountId,
    decimal Amount,
    string Description
) : IRequest<TransactionDto>;

// Similar for: CancelPaymentCommand, CreateBeneficiaryCommand
```

#### Statement Commands
```csharp
// src/Bank.Application/Commands/Statement/GenerateStatementCommand.cs
public sealed record GenerateStatementCommand(
    Guid AccountId,
    DateTime StartDate,
    DateTime EndDate,
    StatementFormat Format
) : IRequest<StatementDto>;

// Similar for: DeliverStatementCommand
```

---

### ISSUE #5: Complete Queries Coverage
**Problem**: Only Account and Transaction queries. Missing: Card, Deposit, Loan, Payment, Statement

**Required Queries to Create**:

#### Card Queries
```csharp
// src/Bank.Application/Queries/Card/GetCardByIdQuery.cs
public sealed record GetCardByIdQuery(Guid CardId) : IRequest<CardDetailsDto>;

// Similar for: GetAccountCardsQuery, GetCardTransactionsQuery, SearchCardsQuery
```

#### Deposit Queries
```csharp
// src/Bank.Application/Queries/Deposit/GetDepositByIdQuery.cs
public sealed record GetDepositByIdQuery(Guid DepositId) : IRequest<DepositDto>;

// Similar for: GetAccountDepositsQuery, GetDepositMaturityQuery
```

#### Loan Queries
```csharp
// src/Bank.Application/Queries/Loan/GetLoanByIdQuery.cs
public sealed record GetLoanByIdQuery(Guid LoanId) : IRequest<LoanDto>;

// Similar for: GetAccountLoansQuery, GetLoanPaymentScheduleQuery
```

#### Payment Queries
```csharp
// src/Bank.Application/Queries/Payment/GetBeneficiariesQuery.cs
public sealed record GetBeneficiariesQuery(Guid CustomerId) : IRequest<List<BeneficiaryDto>>;

// Similar for: GetRecurringPaymentsQuery, SearchPaymentsQuery
```

#### Statement Queries
```csharp
// src/Bank.Application/Queries/Statement/GetStatementByIdQuery.cs
public sealed record GetStatementByIdQuery(Guid StatementId) : IRequest<StatementDetailsDto>;

// Similar for: GetAccountStatementsQuery, SearchStatementsQuery
```

---

## Medium Priority Issues (SHOULD DO)

### ISSUE #6: Standardize DTO Naming Conventions
**Current State**: Mixed Request/Dto/Response suffixes
**Target State**: Clear, consistent naming

**Naming Convention**:
```
API Input (Request):     CreateAccountRequest, UpdateAccountRequest
API Output (Response):   AccountResponse, AccountDetailsResponse
Internal Transfer (DTO): AccountDto, AccountDetailsDto
Search Criteria:         AccountSearchCriteria (not AccountSearchRequest)
Filter:                  AccountFilter (not AccountFilterRequest)
```

**Action Items**:
- All `*Request` classes for API inputs → Keep as is
- All `*Dto` classes for internal DTOs → Keep as is
- Add `*Response` for API outputs where missing
- Rename `*FilterRequest` → `*Filter`
- Rename `*SearchRequest` → `*SearchCriteria`

### ISSUE #7: Move Background Services to Infrastructure
**Current Location**: `src/Bank.Application/Services/Background/`
**Target Location**: `src/Bank.Infrastructure/BackgroundJobs/`

**Files to Move**:
- `BillerHealthCheckBackgroundService.cs`
- `PaymentRetryBackgroundService.cs`
- `DepositBackgroundService.cs`
- `LoanBackgroundService.cs`

**Reason**: Background jobs are infrastructure concerns, not application layer

---

## Low Priority Issues (NICE TO HAVE)

### ISSUE #8: Create Event Handler Subfolders
**Current Structure**:
```
EventHandlers/
├── AuditLogCreatedEventHandler.cs
├── AuditLogDeletedEventHandler.cs
├── AuditLogUpdatedEventHandler.cs
└── AuthSecurityEventHandler.cs
```

**Target Structure**:
```
EventHandlers/
├── Audit/
│   ├── AuditLogCreatedEventHandler.cs
│   ├── AuditLogDeletedEventHandler.cs
│   └── AuditLogUpdatedEventHandler.cs
└── Security/
    └── AuthSecurityEventHandler.cs
```

### ISSUE #9: Create Base Classes for Common Patterns
**Add**:
```csharp
// src/Bank.Application/Common/BaseApplicationRequest.cs
public abstract record BaseApplicationRequest<TResponse> : IRequest<TResponse>
{
    public Guid RequestId { get; init; } = Guid.NewGuid();
    public DateTime RequestedAt { get; init; } = DateTime.UtcNow;
}

// src/Bank.Application/Common/BaseApplicationResponse.cs
public abstract class BaseApplicationResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}

// src/Bank.Application/Common/BaseApplicationService.cs
public abstract class BaseApplicationService
{
    protected readonly ILogger Logger;
    protected readonly IUnitOfWork UnitOfWork;

    protected BaseApplicationService(ILogger logger, IUnitOfWork unitOfWork)
    {
        Logger = logger;
        UnitOfWork = unitOfWork;
    }

    protected virtual void LogOperation(string operation)
    {
        Logger.LogInformation($"Executing operation: {operation}");
    }
}
```

---

## Implementation Roadmap

### Phase 1: Critical Fixes (TODAY - 1 hour)
- [x] Fix InitiateTransactionCommandHandler (make sealed)
- [x] Remove circular dependency from StatementService
- [ ] Delete MissingDtos.cs (after relocating all DTOs)
- [ ] Create proper DTO structure in domain folders

### Phase 2: Complete CQRS (TOMORROW - 4 hours)
- [ ] Create all Card Commands and Queries
- [ ] Create all Deposit Commands and Queries
- [ ] Create all Loan Commands and Queries
- [ ] Create all Payment Commands and Queries
- [ ] Create all Statement Commands and Queries
- [ ] Create handlers for all new commands/queries

### Phase 3: Code Consistency (THIS WEEK - 2 hours)
- [ ] Standardize DTO naming conventions
- [ ] Move Background Services to Infrastructure
- [ ] Create Event Handler subfolders
- [ ] Verify all naming patterns

### Phase 4: Add Abstractions (NEXT WEEK - 2 hours)
- [ ] Create base classes for common patterns
- [ ] Update services to use bases
- [ ] Create base request/response classes
- [ ] Update handlers to use bases

### Phase 5: Verification (NEXT WEEK - 1 hour)
- [ ] Build with 0 errors
- [ ] Review architecture
- [ ] Update documentation
- [ ] Commit and push

---

## Folder Structure After Refactoring

```
src/Bank.Application/
├── Commands/
│   ├── Account/
│   ├── Card/          ← NEW
│   ├── Deposit/       ← NEW
│   ├── Loan/          ← NEW
│   ├── Payment/       ← NEW
│   ├── Statement/     ← NEW
│   ├── Transaction/
│   └── Behaviors/
├── Queries/
│   ├── Account/
│   ├── Card/          ← NEW
│   ├── Deposit/       ← NEW
│   ├── Loan/          ← NEW
│   ├── Payment/       ← NEW
│   ├── Statement/     ← NEW
│   └── Transaction/
├── DTOs/
│   ├── Common/
│   ├── Account/       ← Includes User, JointAccount (from MissingDtos.cs)
│   ├── Auth/          ← Includes TwoFactor (from MissingDtos.cs)
│   ├── Card/          ← Includes Transaction, CreateCardRequest (from MissingDtos.cs)
│   ├── Deposit/       ← Includes CreateDepositRequest, DepositDto (from MissingDtos.cs)
│   ├── Loan/          ← Includes CreateLoanRequest, LoanPaymentDto (from MissingDtos.cs)
│   ├── Payment/       ← Includes Beneficiary, BillPayment (from MissingDtos.cs)
│   └── Statement/     ← Includes CreateStatementRequest (from MissingDtos.cs)
├── Handlers/
│   ├── CommandHandlers/
│   └── QueryHandlers/
├── Services/
│   ├── Account/
│   ├── Auth/
│   ├── Card/
│   ├── Deposit/
│   ├── Loan/
│   ├── Payment/
│   ├── Shared/
│   ├── Statement/
│   └── Transaction/
├── Interfaces/
│   ├── [Mirrors Services structure]
├── Validators/
│   ├── [Domain validators]
├── Helpers/
│   ├── [Static utilities]
├── EventHandlers/
│   ├── Audit/         ← NEW subfolder
│   ├── Security/      ← NEW subfolder
│   └── Domain/
├── Common/            ← NEW
│   ├── BaseApplicationRequest.cs
│   ├── BaseApplicationResponse.cs
│   ├── BaseApplicationService.cs
│   └── Behaviors.cs
├── Mappings/
│   ├── [AutoMapper profiles]
└── DependencyInjection.cs
```

---

## Expected Outcomes

### Before Refactoring:
- Commands: 2 categories (Account, Transaction)
- Queries: 2 categories (Account, Transaction)
- DTOs: Scattered across domain folders + MissingDtos.cs
- Naming: Mostly consistent except DTOs
- CQRS Coverage: ~30%
- Architecture Grade: 7.5/10

### After Refactoring:
- Commands: 7 categories (Account, Card, Deposit, Loan, Payment, Statement, Transaction)
- Queries: 7 categories (complete CQRS coverage)
- DTOs: All properly organized in domain folders
- Naming: 100% consistent across entire layer
- CQRS Coverage: ~95%
- Architecture Grade: 9.5/10

---

## Success Criteria

✅ **Build**: 0 Errors, warnings reduced
✅ **CQRS Coverage**: >90% of operations have Command or Query
✅ **Naming**: 100% consistent across all types
✅ **Organization**: No DTOs scattered in multiple locations
✅ **Dependencies**: Zero circular dependencies
✅ **Patterns**: All handlers sealed, all services have interfaces
✅ **Documentation**: Clear folder structure and patterns

---

## Notes
- All Commands return Entity or DTO (immutable response)
- All Queries return DTO or primitive
- All Handlers use sealed class pattern
- All requests are record types (immutable)
- All services have corresponding interfaces
- All circular dependencies eliminated
