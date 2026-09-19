# Neon PostgreSQL Setup Guide

This project uses [Neon](https://neon.tech) as its hosted PostgreSQL provider.
Neon supports branching (development, staging, production), autoscaling, and scale-to-zero.

> **Never commit real connection strings.** Store them in:
> - `.env` (gitignored) for local development
> - Kubernetes secrets for staging/production
> - Replit / Railpack secrets for hosted environments

---

## Connection String Format

```
DATABASE_URL=postgresql://user:password@ep-xxxxx.region.aws.neon.tech/neondb?sslmode=require
```

Set this as an environment variable — not in `appsettings.json`.

---

## Branch Strategy

| Branch | Purpose | Compute |
|---|---|---|
| `production` | Live user traffic | 1 CU fixed, always-on |
| `staging` | Pre-production validation | 0.5–1 CU autoscaling |
| `development` | Local dev and feature testing | Scale-to-zero after 5 min |

**Estimated cost:** ~$125/month total (dev ~$5, staging ~$20, prod ~$100).

---

## Quick Setup with Neon CLI

```bash
# Install CLI
npm install -g neonctl

# Login
neonctl auth

# Create project
neonctl projects create --name bank-api-prod --region us-east-1

# Create branches (run in order)
neonctl branches create --project-id <proj-id> --branch-name production
neonctl branches create --project-id <proj-id> --branch-name staging   --parent production
neonctl branches create --project-id <proj-id> --branch-name development --parent staging

# Get connection strings
neonctl connection-string --project-id <proj-id> --branch-name development
neonctl connection-string --project-id <proj-id> --branch-name staging
neonctl connection-string --project-id <proj-id> --branch-name production
```

---

## Local Development

```bash
# Set the development connection string
export DATABASE_URL="postgresql://neondb:password@ep-dev-xxxxx.us-east-1.aws.neon.tech/neondb?sslmode=require"

# Run the app — migrations apply automatically on startup
dotnet run --project src/Bank.Host/Bank.Host.csproj
```

---

## Staging / Production (Kubernetes)

```bash
kubectl create secret generic neon-staging-db \
  --from-literal=connection-string='postgresql://...' \
  --namespace=staging

kubectl create secret generic neon-prod-db \
  --from-literal=connection-string='postgresql://...' \
  --namespace=production
```

---

## Migration Timeline (Initial Setup)

| Phase | Week | Task |
|---|---|---|
| 1 | Week 1 | Create Neon project and branches, update EF Core config |
| 2 | Week 2 | Run EF Core migrations to all three branches, verify schema |
| 3 | Week 3 | Bulk copy data from source DB, validate row counts |
| 4 | Week 4 | Update Docker Compose, Kubernetes manifests, Terraform |
| 5 | Week 5 | Staging dry-run → production cutover (Sunday 02:00 UTC) |
| 6 | Week 6+ | Smoke tests, performance monitoring, cost optimization |

---

## Security Checklist

- [ ] Rotate credentials regularly via Neon console
- [ ] Restrict `.env` file permissions: `chmod 600 .env`
- [ ] Use Kubernetes secrets — never environment variables in CI logs
- [ ] Enable Neon IP allowlisting for production branch
- [ ] Enable Neon audit logging for production
