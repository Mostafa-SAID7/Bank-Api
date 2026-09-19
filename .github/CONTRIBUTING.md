# Contributing to FinBank API

Thank you for taking the time to contribute! Every improvement — bug fix, feature, documentation update, or test — makes this project better.

---

## Table of Contents

1. [Code of Conduct](#code-of-conduct)
2. [Getting Started](#getting-started)
3. [How to Contribute](#how-to-contribute)
4. [Branch Strategy](#branch-strategy)
5. [Commit Message Format](#commit-message-format)
6. [Coding Standards](#coding-standards)
7. [Testing Requirements](#testing-requirements)
8. [Pull Request Process](#pull-request-process)
9. [Architecture Rules](#architecture-rules)

---

## Code of Conduct

By participating in this project, you agree to abide by our [Code of Conduct](CODE_OF_CONDUCT.md). Please read it before contributing.

---

## Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL 14+ (or a hosted instance — Neon, Supabase, Railway)
- Git

### Local Setup

```bash
# 1. Fork the repository on GitHub, then clone your fork
git clone https://github.com/<your-username>/Bank-Api.git
cd Bank-Api

# 2. Set required environment variables
export DATABASE_URL="postgresql://user:password@localhost:5432/bankdb"
export JWT_KEY="your-dev-secret-key-at-least-32-chars"

# 3. Restore and build
dotnet restore src/Bank.Host/Bank.Host.csproj
dotnet build src/Bank.Host/Bank.Host.csproj

# 4. Run
dotnet run --project src/Bank.Host/Bank.Host.csproj
```

API is available at `http://localhost:5000/swagger`.

---

## How to Contribute

### Reporting Bugs

Open a [Bug Report](https://github.com/Mostafa-SAID7/Bank-Api/issues/new?template=bug_report.md) using the issue template. Include reproduction steps, expected vs. actual behavior, and your environment.

> ⚠️ For **security vulnerabilities**, do NOT open a public issue. See [SECURITY.md](.github/SECURITY.md) for private disclosure instructions.

### Suggesting Features

Open a [Feature Request](https://github.com/Mostafa-SAID7/Bank-Api/issues/new?template=feature_request.md). Describe the problem, the proposed solution, and which banking domain it impacts.

### Contributing Code

1. Check [open issues](https://github.com/Mostafa-SAID7/Bank-Api/issues) — comment on one to claim it before starting.
2. For significant changes, open an issue first to discuss the approach.
3. Follow the branch strategy and commit format below.

---

## Branch Strategy

| Branch | Purpose |
|---|---|
| `main` | Production-ready code; protected |
| `develop` | Integration branch for features |
| `feature/<name>` | New features |
| `fix/<name>` | Bug fixes |
| `chore/<name>` | Maintenance, refactoring, deps |
| `docs/<name>` | Documentation only |
| `security/<name>` | Security fixes (fast-track review) |

```bash
# Always branch from develop
git checkout develop
git pull origin develop
git checkout -b feature/your-feature-name
```

---

## Commit Message Format

This project follows [Conventional Commits](https://www.conventionalcommits.org/).

```
<type>(<scope>): <short description>

[optional body]

[optional footer]
```

### Types

| Type | When to use |
|---|---|
| `feat` | New feature |
| `fix` | Bug fix |
| `docs` | Documentation only |
| `chore` | Tooling, dependencies, build |
| `refactor` | Code change that is not a fix or feature |
| `test` | Adding or updating tests |
| `security` | Security fix |
| `perf` | Performance improvement |

### Scopes

Use the module or layer name: `auth`, `accounts`, `transactions`, `payments`, `notifications`, `loans`, `cards`, `deposits`, `infrastructure`, `ci`, `docker`.

### Examples

```
feat(payments): add recurring payment scheduling
fix(auth): resolve refresh token expiry edge case
security(jwt): remove hardcoded fallback secret
docs(structure): update modular monolith layout
chore(deps): bump Npgsql to 9.0.4
```

---

## Coding Standards

### General Rules

- Write **clean, readable code** — clarity over cleverness.
- Every public type and method needs an XML doc comment.
- No `// TODO` or `// FIXME` in merged code — open an issue instead.
- No magic strings — use `const` or configuration keys.
- No hardcoded credentials or secrets of any kind.

### Architecture Rules (enforced by `Bank.Architecture.Tests`)

1. **No cross-module coupling** — a module must not reference another module's `Domain` or `Infrastructure` project.
2. **No cross-module table access** — cross-module reads use a contract in `Bank.Contracts` or an integration event.
3. **No EF Core in Application layer** — `DbContext` and EF queries belong in Infrastructure only.
4. **No HttpContext in Application/Domain** — use `ICurrentUser` abstraction.
5. **Financial commands must carry an `IdempotencyKey`.**

### Naming Conventions

| Element | Convention |
|---|---|
| Classes, interfaces | `PascalCase` |
| Methods, properties | `PascalCase` |
| Private fields | `_camelCase` |
| Constants | `PascalCase` |
| Local variables | `camelCase` |

### Security-Specific Rules

- Validate **all** inputs with FluentValidation before any business logic executes.
- Use parameterized queries only (EF Core handles this automatically).
- Sanitize any user-controlled data before logging (CWE-117).
- Never log passwords, tokens, or PII.

---

## Testing Requirements

All PRs must maintain or improve test coverage.

### Test Layers

| Project | Scope |
|---|---|
| `Bank.Domain.Tests` | Unit tests for domain entities, policies, value objects |
| `Bank.Application.Tests` | Unit tests for command/query handlers |
| `Bank.Infrastructure.Tests` | Integration tests with a real test database |
| `Bank.Api.IntegrationTests` | End-to-end API route tests |
| `Bank.Architecture.Tests` | Automated architecture boundary enforcement |

### Running Tests

```bash
# Run all tests
dotnet test src/Bank.sln

# Run a specific project
dotnet test src/Bank.Domain.Tests/Bank.Domain.Tests.csproj

# Run with coverage
dotnet test src/Bank.sln --collect:"XPlat Code Coverage"
```

### Test Requirements

- New features: unit tests for domain logic + handler tests.
- Bug fixes: a test that would have caught the bug.
- API changes: integration tests for the affected routes.
- Security fixes: a test verifying the fix prevents the vulnerability.

---

## Pull Request Process

1. **Ensure all tests pass locally** before opening a PR.
2. **Fill in the PR template completely** — empty sections will cause the PR to be returned.
3. **Keep PRs focused** — one feature or fix per PR. Large PRs are harder to review.
4. **Link the issue** your PR resolves using `Closes #<issue-number>`.
5. **Pass all CI checks** — CodeQL, SonarCloud quality gate, and build must be green.
6. **At least one approval** is required from a CODEOWNER before merging.
7. **Squash commits** on merge to keep `main` history clean.

### PR Checklist (quick reference)

- [ ] Tests added/updated and passing
- [ ] No hardcoded secrets or credentials
- [ ] Input validation implemented
- [ ] Architecture rules not violated
- [ ] Documentation updated if needed
- [ ] CHANGELOG entry added for user-visible changes

---

## Getting Help

See [SUPPORT.md](.github/SUPPORT.md) for all support channels.
