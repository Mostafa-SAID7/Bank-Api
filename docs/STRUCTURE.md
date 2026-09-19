# Project Structure

## Overview

The project has migrated from a horizontal layered layout to a **modular monolith**.
`Bank.Host` is the single deployable process and composition root. Business capabilities
are organized as vertical modules under `src/Modules/`. Legacy horizontal projects
(`Bank.Api`, `Bank.Application`, `Bank.Domain`, `Bank.Infrastructure`) remain active
during the incremental migration and will be removed after each module is fully extracted.

---

## Repository Layout

```
Bank-Api/
├── src/
│   ├── Bank.Host/                      # Single composition root & entry point
│   │   ├── Program.cs                  # App startup, module registration
│   │   └── Bank.Host.csproj
│   │
│   ├── Bank.Contracts/                 # Cross-module DTOs & integration events
│   │   └── Bank.Contracts.csproj
│   │
│   ├── BuildingBlocks/                 # Shared framework abstractions (no business logic)
│   │   ├── Bank.BuildingBlocks.Domain/
│   │   ├── Bank.BuildingBlocks.Application/
│   │   └── Bank.BuildingBlocks.Infrastructure/
│   │
│   ├── Modules/                        # Vertical business modules
│   │   ├── Payments/
│   │   │   ├── Bank.Payments.Domain/
│   │   │   ├── Bank.Payments.Application/
│   │   │   ├── Bank.Payments.Infrastructure/
│   │   │   └── Bank.Payments.Presentation/
│   │   └── Notifications/
│   │       ├── Bank.Notifications.Domain/
│   │       ├── Bank.Notifications.Application/
│   │       ├── Bank.Notifications.Infrastructure/
│   │       └── Bank.Notifications.Presentation/
│   │
│   ├── — Legacy horizontal layer (being migrated) —
│   ├── Bank.Api/                       # All controllers (route compatibility layer)
│   ├── Bank.Application/               # CQRS commands, queries, services
│   ├── Bank.Domain/                    # Core domain entities and value objects
│   ├── Bank.Infrastructure/            # EF Core, BankDbContext, repositories
│   │
│   └── — Test projects —
│       ├── Bank.Domain.Tests/
│       ├── Bank.Application.Tests/
│       ├── Bank.Infrastructure.Tests/
│       ├── Bank.Api.IntegrationTests/
│       └── Bank.Architecture.Tests/    # ArchUnit-style layer boundary tests
│
├── docs/                               # Developer reference documentation
│   └── architecture/                   # Architecture Decision Records (ADRs)
├── devops/                             # Infrastructure & CI/CD scripts
├── .github/                            # GitHub configuration (workflows, templates)
├── Dockerfile                          # Multi-stage production Docker build
├── docker-compose.yml                  # Local development orchestration
├── README.md
└── CHANGELOG.md
```

---

## Module Rules

1. **Host only composes** — `Bank.Host` contains middleware and module registration; no business logic.
2. **No cross-module coupling** — A module may reference `BuildingBlocks` and `Bank.Contracts` but not another module's `Domain` or `Infrastructure` project.
3. **No cross-module table access** — Modules own their own schema. Cross-module reads use a contract or integration event.
4. **Financial commands carry an idempotency key.**
5. **Transactions are local** — No distributed transactions.

---

## Module Migration Order

Per [ADR-002](architecture/ADR-002-module-migration-order.md):

| Order | Module | Status |
|---|---|---|
| 1 | Notifications | ✅ Seam created |
| 2 | Payments | ✅ Seam created |
| 3 | Core Banking | ⏳ Pending |
| 4 | Identity | ⏳ Pending |
| 5 | Deposits | ⏳ Pending |
| 6 | Loans | ⏳ Pending |
| 7 | Cards | ⏳ Pending |
| 8 | Statements | ⏳ Pending |
| 9 | Audit & Compliance | ⏳ Pending |

Legacy horizontal projects are deleted only after a module's migration is complete and API integration tests confirm route compatibility.

---

## Layer Responsibilities

### Bank.Host
- Application composition root
- Module registration via `IModule`
- Middleware pipeline configuration
- No business logic

### BuildingBlocks
- Framework-level abstractions (base entities, domain events, result types)
- Shared by all modules; must contain no business rules

### Bank.Contracts
- Integration event definitions
- Cross-module use-case contracts (DTOs)
- No business logic

### Module (vertical slice)
| Sub-project | Responsibility |
|---|---|
| `*.Domain` | Entities, value objects, domain events, business invariants |
| `*.Application` | Command/query handlers, application services, interfaces |
| `*.Infrastructure` | EF configurations, migrations, external adapters |
| `*.Presentation` | Controllers, minimal API endpoints, module route registration |

---

## Design Patterns

| Pattern | Where Used |
|---|---|
| **Clean Architecture** | Hard dependency inversion between all layers |
| **CQRS + MediatR** | Commands (writes) and Queries (reads) strictly separated |
| **Repository + Unit of Work** | Data access in Infrastructure layer |
| **Domain Events** | Cross-aggregate communication within a module |
| **Integration Events** | Cross-module communication via `Bank.Contracts` |
| **Soft Delete** | Audit-safe deletion via `IsDeleted` flag |
| **Audit Trail** | Immutable `AuditLog` for financial data changes |