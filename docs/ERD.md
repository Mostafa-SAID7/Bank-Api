# Entity Relationship Diagram

## Domain Overview

The Bank API domain is organized around nine business modules.
The diagram below shows the primary entities, their key attributes,
and how they relate across the system.

---

## ERD

```mermaid
erDiagram
    %% ── Identity ──────────────────────────────────────────────────
    User {
        Guid   Id            PK
        string UserName
        string Email
        string PasswordHash
        string NormalizedEmail
        bool   TwoFactorEnabled
        bool   IsDeleted
        bool   IsActive
        string Roles
    }

    Session {
        Guid     Id          PK
        Guid     UserId      FK
        string   Token
        datetime CreatedAt
        datetime ExpiresAt
        bool     IsRevoked
    }

    %% ── Core Banking ──────────────────────────────────────────────
    Account {
        Guid    Id               PK
        Guid    UserId           FK
        string  AccountNumber
        string  AccountType
        decimal Balance
        string  Currency
        bool    IsActive
        bool    IsDeleted
        datetime CreatedAt
    }

    Transaction {
        Guid     Id              PK
        Guid     FromAccountId   FK
        Guid     ToAccountId     FK
        decimal  Amount
        string   Currency
        string   TransactionType
        string   Status
        string   Description
        string   IdempotencyKey
        datetime Timestamp
    }

    %% ── Payments ──────────────────────────────────────────────────
    Beneficiary {
        Guid   Id           PK
        Guid   UserId       FK
        string Name
        string AccountNumber
        string BankName
        bool   IsActive
    }

    BillPayment {
        Guid     Id          PK
        Guid     UserId      FK
        Guid     AccountId   FK
        string   BillerCode
        string   BillerName
        decimal  Amount
        string   Status
        datetime ScheduledAt
        datetime PaidAt
    }

    %% ── Loans ─────────────────────────────────────────────────────
    Loan {
        Guid     Id           PK
        Guid     UserId       FK
        decimal  Principal
        decimal  InterestRate
        int      TermMonths
        string   Status
        decimal  OutstandingBalance
        datetime StartDate
        datetime MaturityDate
    }

    LoanPayment {
        Guid     Id         PK
        Guid     LoanId     FK
        decimal  Amount
        datetime PaidAt
        string   Status
    }

    %% ── Cards ─────────────────────────────────────────────────────
    Card {
        Guid     Id          PK
        Guid     UserId      FK
        Guid     AccountId   FK
        string   CardNumber
        string   CardType
        string   Status
        datetime ExpiryDate
        string   PinHash
        bool     IsBlocked
    }

    %% ── Deposits ──────────────────────────────────────────────────
    Deposit {
        Guid     Id              PK
        Guid     UserId          FK
        Guid     AccountId       FK
        string   ProductCode
        decimal  Principal
        decimal  InterestRate
        int      TermDays
        string   Status
        datetime MaturityDate
    }

    %% ── Statements ────────────────────────────────────────────────
    Statement {
        Guid     Id          PK
        Guid     AccountId   FK
        string   PeriodMonth
        string   FileUrl
        datetime GeneratedAt
    }

    %% ── Notifications ─────────────────────────────────────────────
    Notification {
        Guid     Id         PK
        Guid     UserId     FK
        string   Channel
        string   Subject
        string   Body
        string   Status
        datetime SentAt
    }

    %% ── Audit ─────────────────────────────────────────────────────
    AuditLog {
        Guid     Id          PK
        Guid     UserId      FK
        string   EntityType
        string   EntityId
        string   Action
        string   OldValues
        string   NewValues
        string   IpAddress
        datetime Timestamp
    }

    %% ── Relationships ─────────────────────────────────────────────
    User         ||--o{ Session       : "has"
    User         ||--o{ Account       : "owns"
    User         ||--o{ Beneficiary   : "saves"
    User         ||--o{ BillPayment   : "pays"
    User         ||--o{ Loan          : "applies for"
    User         ||--o{ Card          : "holds"
    User         ||--o{ Deposit       : "places"
    User         ||--o{ Notification  : "receives"
    User         ||--o{ AuditLog      : "generates"

    Account      ||--o{ Transaction   : "source of"
    Account      ||--o{ Transaction   : "destination of"
    Account      ||--o{ BillPayment   : "funded by"
    Account      ||--o{ Card          : "linked to"
    Account      ||--o{ Deposit       : "linked to"
    Account      ||--o{ Statement     : "has"

    Loan         ||--o{ LoanPayment   : "repaid via"
```

---

## Key Design Notes

| Aspect | Decision |
|---|---|
| **Soft delete** | All core entities use `IsDeleted` flag; no hard deletes on financial data |
| **Currency** | Stored as string (ISO 4217 code) alongside every monetary amount |
| **Idempotency** | `Transaction` carries an `IdempotencyKey` to prevent duplicate processing |
| **Audit trail** | `AuditLog` is immutable — no updates or deletes allowed on audit rows |
| **Module ownership** | Each cluster of entities is owned by exactly one module (see `ADR-001`) |
| **Cross-module access** | Modules communicate via `Bank.Contracts` events, not direct table joins |