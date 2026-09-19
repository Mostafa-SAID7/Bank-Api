# Modular monolith review

## Baseline

The imported solution already has useful domain folders, but its ownership is
horizontal:

- `Bank.Api` owns all controllers and the process entry point.
- `Bank.Application` owns commands, queries, DTOs, interfaces, and services
  for every business capability.
- `Bank.Domain` owns entities for every business capability.
- `Bank.Infrastructure` owns one `BankDbContext`, all configurations,
  repositories, migrations, and external adapters.

The database context currently exposes accounts, transactions, identity,
payments, loans, cards, deposits, statements, notifications, and audit data
through one persistence project. That is the main coupling point to remove
gradually.

## Target mapping

| Current area | Target module |
| --- | --- |
| `Entities/Account`, `Entities/Transaction` | Core Banking |
| `Entities/Auth` | Identity |
| `Entities/Payment` | Payments |
| `Entities/Loan` | Loans |
| `Entities/Card` | Cards |
| `Entities/Deposit` | Deposits |
| `Entities/Statement`, account statement services | Statements |
| shared notification entities/services | Notifications |
| `AuditLog` and audit handlers/middleware | Audit & Compliance |

## Important coupling found

1. `BankDbContext` is the persistence boundary for every domain, so moving
   entities without schema migrations would only create a cosmetic modular
   layout.
2. `Bank.Application` global usings expose DTOs from every capability to every
   service, which makes compile-time ownership implicit.
3. Payment services and transaction services currently share the generic
   repository/unit-of-work layer. Payments must call a small Core Banking
   posting contract instead of reaching into account tables.
4. Audit is currently present in application event handlers and API middleware.
   It should become an event consumer rather than a business-service
   dependency.
5. API controllers already provide a route inventory. They must be moved only
   with route-compatibility integration tests.

## Current implementation

The first increment adds `Bank.Host`, `BuildingBlocks`, `Bank.Contracts`,
Payments and Notifications module seams, and project-graph architecture tests.
The legacy projects remain active to keep the API behavior stable.

## Next migration slice

Move Notifications completely:

1. Move notification entities, preferences, interfaces, services, persistence
   configurations, and adapters into the Notifications projects.
2. Add an idempotent consumer for `PaymentCompleted` and `PaymentFailed`.
3. Add module integration tests and preserve existing notification routes.
4. Move the owned tables to the `notifications` schema.

Then repeat the pattern for Payments using
`ICoreBankingPostingContract`, followed by the remaining modules in the ADR
order.