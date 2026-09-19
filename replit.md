# Bank API modular monolith

## Current architecture

The application is being migrated from the original horizontal
`Bank.Api` / `Bank.Application` / `Bank.Domain` / `Bank.Infrastructure` layout
to a modular monolith. `Bank.Host` is the single composition root and remains
the only deployable process.

The first increment establishes:

- `BuildingBlocks/` for framework-level abstractions only
- `Bank.Contracts/` for DTOs, integration events, and small cross-module
  contracts
- `Modules/Payments/` and `Modules/Notifications/` as the pilot module seams
- a shared `IModule` registration and endpoint-mapping contract

The existing controllers, services, and database context remain active while
the pilots are moved incrementally. API routes must not change during the
migration.

## Local run

The project targets .NET 9 and uses PostgreSQL. Configure the required
connection string and JWT settings through Replit Secrets or environment
variables; do not commit `.env` files containing real credentials.

```bash
dotnet restore src/Bank.Host/Bank.Host.csproj
dotnet run --project src/Bank.Host/Bank.Host.csproj
```

The host listens on port 5000 and Swagger is available at `/swagger`.

## Module rules

- A module may reference `BuildingBlocks` and `Bank.Contracts`, but not another
  module's Domain or Infrastructure project.
- Modules own their application and infrastructure registrations.
- Cross-module calls use a small contract or an integration event; no module
  reads another module's tables directly.
- Financial commands must carry an idempotency key.
- A transaction is owned by one module; distributed transactions are not used.

## Migration order

Notifications is the first pilot, followed by Payments, then Core Banking,
Identity, Deposits, Loans, Cards, Statements, and Audit/Compliance. Delete the
legacy horizontal projects only after all features have moved and API
integration tests confirm route compatibility.