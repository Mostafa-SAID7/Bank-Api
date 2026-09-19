# ADR-001: Modular monolith boundaries

- **Status:** Accepted
- **Date:** 2026-09-19

## Decision

Keep one API host, one deployment, and one PostgreSQL database initially.
Organize business capabilities as modules with explicit ownership:

| Module | Owns |
| --- | --- |
| Identity | Users, roles, sessions, password policy, 2FA |
| Core Banking | Accounts, ledger, balances, transfers |
| Payments | Beneficiaries, bill payments, recurring payments, templates |
| Loans | Loan lifecycle, payments, documents |
| Cards | Cards, authorizations, transactions, PIN |
| Deposits | Deposit products, fixed deposits, interest, maturity |
| Statements | Statement generation and delivery |
| Notifications | Email, SMS, and preferences |
| Audit | Audit trail and compliance consumers |

Accounts and transactions stay together in Core Banking until a later
extraction decision because their consistency boundary is financial.

## Rules

1. The host contains composition and middleware, not business logic.
2. A module does not reference another module's Domain or Infrastructure
   project.
3. A module does not query another module's tables directly.
4. `Bank.Contracts` contains only DTOs, integration events, and narrowly
   scoped use-case contracts.
5. Synchronous calls are limited to operations that need an immediate result.
   Other communication uses idempotent integration-event consumers.
6. Transactions are local to a module. Distributed transactions are out of
   scope.
7. Existing API routes and request/response contracts are compatibility
   constraints during migration.

## Consequences

The first increments create the module seams and host composition without
moving all business code at once. This allows route and data compatibility to
be tested after each pilot. The legacy horizontal projects remain temporarily
because deleting them before the final module migration would create a
large, unreviewable change.