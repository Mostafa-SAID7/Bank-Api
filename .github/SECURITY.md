# Security Policy

## Supported Versions

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |
| < 1.0   | :x:                |

---

## Reporting a Vulnerability

> **⚠️ Do NOT report security vulnerabilities through public GitHub issues.**

### Critical Issues (financial transactions, authentication, data privacy)

Email **security@bankproject.com** — response within **24 hours**.

### Non-Critical Issues

Use [GitHub's private vulnerability reporting](https://github.com/Mostafa-SAID7/Bank-Api/security/advisories/new)
or email security@bankproject.com — response within **1 week**.

### What to Include

- Description of the vulnerability
- Steps to reproduce
- Potential impact
- Suggested fix (if available)

---

## Security Measures in Place

### Authentication & Authorization
- JWT Bearer tokens with refresh and rotation pipelines
- ASP.NET Core Identity for standard identity management
- Role-Based Access Control (Admin, Manager, User, Auditor)
- Resource-based authorization policies
- 2FA via TOTP (ASP.NET Core Identity)

### Data Protection
- Encryption at rest and in transit (HTTPS/TLS enforced)
- Secure password hashing via Identity's built-in hasher
- Input validation (FluentValidation) and sanitization
- SQL injection prevention (parameterized EF Core queries)
- PII anonymization for audit logs

### API Security
- Rate limiting and IP whitelisting middleware
- Strict CORS configuration
- Request/response security headers middleware
- API versioning

### Automated Scanning
| Tool | Purpose |
|---|---|
| **Dependabot** | Automatic dependency updates |
| **CodeQL** | Static code analysis (GitHub Advanced Security) |
| **Trivy** | Container vulnerability scanning |
| **SonarCloud** | Code quality and security gate |
| **OWASP Dependency Check** | Known CVE detection |

---

## Compliance Targets

- PCI DSS — payment card data protection
- GDPR — personal data privacy
- SOX — financial reporting integrity

---

## Security Best Practices for Contributors

1. **Never commit secrets** — use environment variables or a secrets vault
2. **Validate all inputs** — FluentValidation is the canonical approach
3. **Use parameterized queries** — no string concatenation in EF Core calls
4. **Handle errors safely** — do not expose stack traces or internal details
5. **Keep dependencies updated** — Dependabot PRs should be merged promptly
6. **Follow secure coding standards** — SonarCloud gate must pass before merge

---

*The security of users' financial data is our top priority. Thank you for helping us keep it safe.*