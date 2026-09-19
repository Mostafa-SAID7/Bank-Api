# ADR-002: Incremental module migration order

- **Status:** Accepted
- **Date:** 2026-09-19

## Decision

Migrate Notifications first because it is an edge capability with few business
invariants. Migrate Payments second because it provides a useful test of
explicit contracts with Core Banking and event consumers. Continue with Core
Banking, Identity, Deposits, Loans, Cards, Statements, and Audit.

Each module is moved only after:

- its domain, application, infrastructure, and presentation projects exist;
- its data ownership and migrations are explicit;
- its API routes retain compatibility tests;
- its module registration is independent of the legacy horizontal layer.

## Consequence

The repository temporarily contains legacy and modular code. This is
intentional. The old projects are removed only after the last module has
migrated and the complete API integration suite passes.