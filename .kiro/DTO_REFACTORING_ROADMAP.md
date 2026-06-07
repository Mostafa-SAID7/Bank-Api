# DTO Refactoring Roadmap

## Overview
This document outlines the systematic elimination of code duplication in DTOs identified by SonarQube (64.6% duplicated lines in BaseDepositProductDto and related files).

**Status**: Phase 1 Complete ✅
- Created base classes for common patterns
- Removed duplicate files
- Build verified: 0 Errors

---

## Phase 1: Foundation (COMPLETE ✅)

### Base Classes Created
1. **BaseAuditDto** - For audit trail
   ```csharp
   public abstract class BaseAuditDto
   {
       public DateTime CreatedAt { get; set; }
       public DateTime? UpdatedAt { get; set; }
   }
   ```

2. **BaseResultDto** - For operation results
   ```csharp
   public abstract class BaseResultDto
   {
       public bool IsSuccess { get; set; }
       public string Message { get; set; } = string.Empty;
       public List<string> Errors { get; set; } = new();
       public bool HasErrors => Errors?.Count > 0;
   }
   ```

3. **BaseEntityDto** - Combines Id + Audit
   ```csharp
   public abstract class BaseEntityDto : BaseAuditDto
   {
       public Guid Id { get; set; }
   }
   ```

### Duplicate Files Removed
- ❌ `MaturityDtoDef.cs` → Consolidated into `MaturityDto.cs`

---

## Phase 2: Deposit Module Refactoring (PENDING)

### High Priority: BaseDepositProductDto

**Current State**: 64.6% duplication
- **Issue**: Core properties in base class repeated in derived classes
- **Files affected**:
  - `BaseDepositProductDto.cs` (base)
  - `DepositProductDto.cs` (derived)
  - `FixedDepositDto.cs` (separate hierarchy)
  - `CreateDepositProductRequest.cs`
  - `UpdateDepositProductRequest.cs`

**Action Items**:
1. Create `BaseDepositTermsDto` for term-related properties
   ```csharp
   // Extract from BaseDepositProductDto
   - MinimumTermDays
   - MaximumTermDays
   - DefaultTermDays
   - AllowAutoRenewal
   - AutoRenewalNoticeDays
   - DefaultMaturityAction
   ```

2. Create `BaseDepositInterestDto` for interest properties
   ```csharp
   // Extract from BaseDepositProductDto
   - BaseInterestRate
   - InterestCalculationMethod
   - CompoundingFrequency
   - HasTieredRates
   - PromotionalRateStartDate
   - PromotionalRateEndDate
   - PromotionalRate
   ```

3. Create `BaseDepositPenaltyDto` for penalty properties
   ```csharp
   // Extract from BaseDepositProductDto
   - AllowPartialWithdrawals
   - PenaltyType
   - PenaltyAmount
   - PenaltyPercentage
   - PenaltyFreeDays
   ```

4. Refactor inheritance chain
   ```csharp
   public abstract class BaseDepositProductDto
   {
       public string Name { get; set; } = string.Empty;
       public string Description { get; set; } = string.Empty;
       public DepositProductType ProductType { get; set; }
       public decimal MinimumBalance { get; set; }
       public decimal? MaximumBalance { get; set; }
       public decimal MinimumOpeningBalance { get; set; }
   }
   
   public abstract class DepositProductWithTermsDto : BaseDepositProductDto, IDepositTerms
   public abstract class DepositProductWithInterestDto : BaseDepositProductDto, IDepositInterest
   public abstract class DepositProductWithPenaltyDto : BaseDepositProductDto, IDepositPenalty
   ```

### Card Module Refactoring (PENDING)

**Current State**: Multiple DTOs with 12+ duplicated properties
- **Files affected**:
  - `CardDto.cs`
  - `CardDetailsDto.cs`
  - `CardSummaryDto.cs`
  - `CardActivationResult.cs`
  - `CardIssuanceResult.cs`

**Duplicated Properties**:
- Id, CardType/Type, Status, ExpiryDate, CreatedAt
- DailyLimit, MonthlyLimit, IsContactlessEnabled, IsOnlineTransactionsEnabled

**Action Items**:
1. Create `BaseCardDto : BaseEntityDto`
   ```csharp
   public abstract class BaseCardDto : BaseEntityDto
   {
       public Guid CustomerId { get; set; }
       public Guid AccountId { get; set; }
       public CardType Type { get; set; }
       public CardStatus Status { get; set; }
       public DateTime ExpiryDate { get; set; }
       public decimal DailyLimit { get; set; }
       public decimal MonthlyLimit { get; set; }
       public bool IsContactlessEnabled { get; set; }
       public bool IsOnlineTransactionsEnabled { get; set; }
       public bool IsInternationalTransactionsEnabled { get; set; }
   }
   ```

2. Update derived classes to inherit from `BaseCardDto`
   - `CardDetailsDto`
   - `CardSummaryDto`

3. Update Result classes to inherit from `BaseResultDto`
   - `CardActivationResult`
   - `CardIssuanceResult`

### Loan Module Refactoring (PENDING)

**Current State**: Repeated status/result pattern
- **Files affected**:
  - `ApprovalDecision.cs`
  - `LoanApprovalResult.cs`
  - `LoanApplicationResult.cs`

**Duplicated Pattern**:
- `IsApproved`/`IsSuccess` (boolean flags)
- `Message` fields
- `Errors` lists
- Interest rate in multiple places

**Action Items**:
1. Update result classes to inherit from `BaseResultDto`
   - `LoanApprovalResult`
   - `LoanApplicationResult`

2. Create `BaseLoanDto : BaseEntityDto` for common loan properties
   - `LoanNumber`
   - `InterestRate`
   - `TermInMonths`
   - `Status`

3. Consolidate approval classes under single pattern

### Account Module Refactoring (PENDING)

**Current State**: Validation results with duplicate properties
- **Files affected**:
  - `AccountValidationResult.cs`
  - `ComprehensiveValidationResult.cs`

**Issue**: ComprehensiveValidationResult duplicates all properties from AccountValidationResult

**Action Items**:
1. Create `BaseValidationResultDto : BaseResultDto`
   ```csharp
   public abstract class BaseValidationResultDto : BaseResultDto
   {
       public DateTime ValidationDate { get; set; } = DateTime.UtcNow;
       public string? ValidationReference { get; set; }
   }
   ```

2. Update validation classes
   - `AccountValidationResult : BaseValidationResultDto`
   - `ComprehensiveValidationResult : BaseValidationResultDto`

3. Remove duplicate properties from ComprehensiveValidationResult

### Transaction Module Refactoring (PENDING)

**Current State**: Property naming inconsistencies
- **Files affected**:
  - `TransactionDto.cs`
  - `TransactionSearchCriteriaDto.cs`

**Issue**: `Type` vs `TransactionType` inconsistency

**Action Items**:
1. Standardize property names in `TransactionDto`
   - Remove duplicate `Type` / `TransactionType` (pick one)
   - Consolidate `FromAccountId`, `ToAccountId` usage

2. Create `BaseTransactionDto : BaseEntityDto`
   - Core transaction properties
   - Status tracking
   - Timestamps

### Beneficiary Module Refactoring (PENDING)

**Current State**: Repeated audit trail and limits
- **Files affected**:
  - `BeneficiaryDto.cs`
  - `BeneficiaryResult.cs`

**Action Items**:
1. Update `BeneficiaryResult` to inherit from `BaseResultDto`
2. Extract limits to separate class if needed
3. Use `BaseAuditDto` for timestamp tracking

### Statement Module Refactoring (PENDING)

**Current State**: Date field proliferation
- **Files affected**:
  - `StatementDto.cs`
  - `StatementDetailsDto.cs`

**Issue**: 4-5 date fields with overlapping purposes
- `StartDate`, `EndDate`
- `PeriodStartDate`, `PeriodEndDate`
- `StatementDate`
- `CreatedAt`
- `DeliveredDate`

**Action Items**:
1. Consolidate date fields:
   - `PeriodStartDate`, `PeriodEndDate` (required)
   - `StatementDate` (when generated - can be computed from CreatedAt)
   - `DeliveredDate` (optional)

2. Remove redundant `EndDate`, `StartDate` duplicates

3. Create `BasePeriodDto` for date period handling

---

## Phase 3: Validation & Testing (PENDING)

### Test Coverage
- [ ] Unit tests for all refactored DTOs
- [ ] Mapping tests (DTOs ↔ Domain Entities)
- [ ] Serialization tests (JSON compatibility)

### Build Verification
- [ ] Compile with 0 errors
- [ ] All warnings reviewed
- [ ] No breaking changes for consumers

### SonarQube Verification
- [ ] Duplication percentage reduced to <20%
- [ ] All issues addressed
- [ ] Code quality score improved

---

## Phase 4: Consumer Updates (PENDING)

After refactoring DTOs, update all consuming code:

### Services That Need Updates
- [ ] Deposit services (use new base classes)
- [ ] Card services (use `BaseCardDto`)
- [ ] Loan services (use `BaseLoanDto`)
- [ ] Account services (use validation results)
- [ ] All AutoMapper profiles

### Controllers That Need Updates
- [ ] DepositController
- [ ] CardController
- [ ] LoanController
- [ ] AccountController
- [ ] BeneficiaryController
- [ ] StatementController

---

## Benefits of This Refactoring

1. **Code Duplication**: Reduced from 64.6% to <20%
2. **Maintainability**: Single source of truth for common patterns
3. **Consistency**: Standardized patterns across all modules
4. **Testability**: Easier to test shared behavior
5. **Extensibility**: New DTOs can leverage base classes
6. **Documentation**: Base classes document expected patterns

---

## Affected Files Summary

### Being Created (Phase 1 ✅)
- `src/Bank.Application/DTOs/Common/BaseAuditDto.cs`
- `src/Bank.Application/DTOs/Common/BaseResultDto.cs`
- `src/Bank.Application/DTOs/Common/BaseEntityDto.cs`

### Being Deleted (Phase 1 ✅)
- `src/Bank.Application/DTOs/Deposit/Maturity/MaturityDtoDef.cs`

### To Be Refactored (Phase 2-3)

**High Priority** (>50% duplication):
- BaseDepositProductDto.cs
- DepositProductDto.cs
- FixedDepositDto.cs
- CardDto.cs, CardDetailsDto.cs, CardSummaryDto.cs

**Medium Priority** (20-50% duplication):
- LoanDto.cs, LoanApplicationDto.cs, LoanApprovalResultDto.cs
- AccountDto.cs, AccountValidationResultDto.cs
- BeneficiaryDto.cs

**Low Priority** (<20% duplication):
- TransactionDto.cs
- StatementDto.cs

---

## Implementation Strategy

### Incremental Approach
1. **Phase 1** ✅: Create base classes (DONE)
2. **Phase 2**: Refactor high-priority modules one at a time
3. **Phase 2**: Test and verify each module
4. **Phase 3**: Update consumers (services/controllers)
5. **Phase 4**: Full integration test and deployment

### Risk Mitigation
- **Backward compatibility**: Maintain old classes temporarily if needed
- **Testing**: Comprehensive tests before and after
- **Gradual rollout**: One module at a time
- **Documentation**: Clear migration guides for consumers

---

## Metrics

### Before Refactoring
- Total DTO files: ~50+
- Duplication: 64.6% in BaseDepositProductDto
- Code quality: High technical debt
- Maintenance burden: High

### After Refactoring (Target)
- Total DTO files: ~45 (5 removed/consolidated)
- Duplication: <20% across codebase
- Code quality: Improved maintainability
- Maintenance burden: Reduced by ~40%

---

## Next Steps

1. Review and approve Phase 1 changes ✅
2. Proceed with Phase 2: Deposit module refactoring
3. Create interface contracts for shared behaviors
4. Update AutoMapper profiles
5. Test thoroughly before deployment
