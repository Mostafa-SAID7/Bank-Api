# Neon Database Migration - Implementation Tasks

## Overview

This document contains the breakdown of all implementation tasks for migrating Bank-Api to Neon. Tasks are organized by phase and status.

---

## Phase 1: Pre-Migration Setup (Week 1)

### Task 1.1.1: Create Neon Project
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 1 hour  
**Depends on**: None

**Definition of Done**:
- [ ] Neon account created
- [ ] Project "bank-api-prod" created in region us-east-1
- [ ] Project ID documented
- [ ] Can connect to default database

**Acceptance Criteria**:
- Project accessible via console.neon.tech
- Default connection string generated and tested

---

### Task 1.1.2: Create Neon Branches
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 2 hours  
**Depends on**: Task 1.1.1

**Definition of Done**:
- [ ] `production` branch created with fixed 1 CU compute
- [ ] `staging` branch created with 0.5-1 CU autoscaling
- [ ] `development` branch created with scale-to-zero after 5 min
- [ ] All three connection strings retrieved and tested
- [ ] Connection strings stored securely:
  - Production: Kubernetes secret `neon-prod-db`
  - Staging: Kubernetes secret `neon-staging-db`
  - Development: `.env` file (gitignored)

**Acceptance Criteria**:
- Each branch connection successful with psql
- Connection strings follow format: `postgresql://...@ep-xxxxx.neon.tech/main`
- No hardcoded connection strings in version control

**Neon CLI Commands**:
```bash
neonctl branches create --project-id $PROJECT_ID --branch-name production --parent main
neonctl branches create --project-id $PROJECT_ID --branch-name staging --parent production
neonctl branches create --project-id $PROJECT_ID --branch-name development --parent staging
neonctl connection-string --project-id $PROJECT_ID --branch-name production
```

---

### Task 1.2.1: Backup Source Database
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 1 hour  
**Depends on**: None

**Definition of Done**:
- [ ] Full backup created: `bank_db_backup_YYYYMMDD_HHMMSS.dump`
- [ ] Backup file size documented
- [ ] Backup location: `/backups/neon-migration/`
- [ ] Backup process verified (restore test successful)

**Acceptance Criteria**:
- Backup file exists and is readable
- Restore to temporary database succeeds
- All tables present in restored database

**Command**:
```bash
pg_dump -h localhost -U postgres -d bank_db -F custom \
  > /backups/neon-migration/bank_db_backup_$(date +%Y%m%d_%H%M%S).dump
```

---

### Task 1.2.2: Document Source Schema
**Status**: ⬜ Not Started  
**Owner**: Database Admin  
**Effort**: 2 hours  
**Depends on**: Task 1.2.1

**Definition of Done**:
- [ ] Schema documentation created: `schema_snapshot.md`
- [ ] Table count documented (expected: ~45)
- [ ] Row counts per table captured
- [ ] Index count, foreign key count captured
- [ ] Sequence current values captured
- [ ] PostgreSQL extensions listed

**Sample Documentation Format**:
```
# Schema Snapshot - 2024-01-15

## Summary
- Tables: 45
- Indexes: 120
- Foreign Keys: 35
- Sequences: 12
- Total Records: 150,000

## Table Counts
- users: 5,000
- accounts: 8,500
- transactions: 50,000
- deposits: 5,200
- loans: 2,100
... (etc)

## Extensions
- uuid-ossp
- pg_trgm
```

---

### Task 1.2.3: Verify EF Core Mappings
**Status**: ⬜ Not Started  
**Owner**: Backend Developer  
**Effort**: 3 hours  
**Depends on**: Task 1.2.2

**Definition of Done**:
- [ ] EF Core validation run: `dotnet ef dbcontext info`
- [ ] All entities verified against database schema
- [ ] Mismatch report created (if any)
- [ ] Entity mapping documentation updated

**Acceptance Criteria**:
- No validation errors
- All 45+ entities map correctly to database tables
- All relationships properly configured

**Command**:
```bash
cd src/Bank.Api
dotnet ef dbcontext info --verbose
```

---

### Task 1.2.4: Enable Logical Replication
**Status**: ⬜ Not Started  
**Owner**: Database Admin  
**Effort**: 1 hour  
**Depends on**: None (can run before Phase 2)

**Definition of Done**:
- [ ] PostgreSQL wal_level set to 'logical'
- [ ] max_wal_senders = 10
- [ ] max_replication_slots = 10
- [ ] PostgreSQL service restarted
- [ ] Configuration verified

**Acceptance Criteria**:
- Replication enabled: `SHOW wal_level;` returns 'logical'
- Publication created: `SELECT * FROM pg_publication;`

**SQL Commands**:
```sql
ALTER SYSTEM SET wal_level = logical;
ALTER SYSTEM SET max_wal_senders = 10;
ALTER SYSTEM SET max_replication_slots = 10;
```

---

### Task 1.3.1: Update BankDbContext for Neon
**Status**: ⬜ Not Started  
**Owner**: Backend Developer  
**Effort**: 2 hours  
**Depends on**: None

**Definition of Done**:
- [ ] `BankDbContext.cs` updated with parameterized connection string
- [ ] Retry logic added (3 retries, 5 sec delay)
- [ ] PostgreSQL version set to 14.0
- [ ] Connection pooling configured
- [ ] Connection timeout: 30 seconds
- [ ] Changes reviewed and tested locally

**Code Changes** (src/Bank.Infrastructure/Data/BankDbContext.cs):
```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
            ?? _configuration.GetConnectionString("NeonDatabase");
        
        optionsBuilder.UseNpgsql(connectionString, options =>
        {
            options.SetPostgresVersion(new Version(14, 0));
            options.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelaySeconds: 5);
            options.CommandTimeout(30);
        });
    }
}
```

**Acceptance Criteria**:
- Code compiles without errors
- Retry mechanism works in unit tests
- Environment variable override tested

---

### Task 1.3.2: Update appsettings Files
**Status**: ⬜ Not Started  
**Owner**: Backend Developer  
**Effort**: 1 hour  
**Depends on**: Task 1.3.1

**Definition of Done**:
- [ ] `appsettings.json` updated with production Neon connection
- [ ] `appsettings.Development.json` updated with dev Neon connection
- [ ] `appsettings.Staging.json` created with staging Neon connection
- [ ] No hardcoded passwords in files
- [ ] All connection strings use parameterized approach
- [ ] Files verified with sample connection test

**File Updates**:
- `src/Bank.Api/appsettings.json`: Production Neon connection
- `src/Bank.Api/appsettings.Development.json`: Development Neon connection
- `src/Bank.Api/appsettings.Staging.json`: Staging Neon connection (new file)

**Acceptance Criteria**:
- All files valid JSON
- Connection strings follow Neon format
- No secrets committed to version control
- Configuration reads successfully at runtime

---

## Phase 2: Schema Migration (Week 2)

### Task 2.1.1: Apply Migrations to Development Branch
**Status**: ⬜ Not Started  
**Owner**: Backend Developer  
**Effort**: 2 hours  
**Depends on**: Task 1.1.2, Task 1.3.2

**Definition of Done**:
- [ ] Development branch connection string active
- [ ] All pending migrations applied: `dotnet ef database update`
- [ ] Schema creation verified (45+ tables exist)
- [ ] All indexes created
- [ ] All foreign keys created
- [ ] PostgreSQL extensions enabled (uuid-ossp, pg_trgm)
- [ ] Verification script run successfully

**Commands**:
```bash
cd src/Bank.Api
export DATABASE_URL=$(cat .env | grep DATABASE_URL | cut -d'=' -f2)
dotnet ef database update --context BankDbContext --environment Development

# Verify
psql "$DATABASE_URL" -c "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema='public';"
```

**Acceptance Criteria**:
- No migration errors
- 45+ tables created
- All indexes present
- All constraints present

---

### Task 2.1.2: Apply Migrations to Staging Branch
**Status**: ⬜ Not Started  
**Owner**: Backend Developer  
**Effort**: 1 hour  
**Depends on**: Task 2.1.1

**Definition of Done**:
- [ ] Staging branch connection string active
- [ ] All migrations applied successfully
- [ ] Schema structure matches development branch
- [ ] Verification script confirms table count

**Commands**:
```bash
export NEON_STAGING_URL=$(get-from-kubernetes-secret neon-staging-db)
export DATABASE_URL=$NEON_STAGING_URL
dotnet ef database update --context BankDbContext --environment Staging
```

**Acceptance Criteria**:
- Staging schema matches development schema
- No migration errors

---

### Task 2.1.3: Apply Migrations to Production Branch
**Status**: ⬜ Not Started  
**Owner**: Backend Developer  
**Effort**: 1 hour  
**Depends on**: Task 2.1.2

**Definition of Done**:
- [ ] Production branch connection string active
- [ ] All migrations applied successfully
- [ ] Schema structure matches staging branch
- [ ] Verification confirms production schema ready for data

**Commands**:
```bash
export NEON_PROD_URL=$(get-from-kubernetes-secret neon-prod-db)
export DATABASE_URL=$NEON_PROD_URL
dotnet ef database update --context BankDbContext
```

**Acceptance Criteria**:
- Production schema matches staging schema
- All three branches now have identical schema
- Ready for data migration

---

## Phase 3: Data Migration (Week 3)

### Task 3.1.1: Create Publication on Source
**Status**: ⬜ Not Started  
**Owner**: Database Admin  
**Effort**: 30 mins  
**Depends on**: Task 1.2.4

**Definition of Done**:
- [ ] Publication "bank_api_pub" created
- [ ] ALL tables included in publication
- [ ] Publication verified

**SQL**:
```sql
CREATE PUBLICATION bank_api_pub FOR ALL TABLES;
SELECT * FROM pg_publication;
```

---

### Task 3.1.2: Bulk Copy Data to Production Branch
**Status**: ⬜ Not Started  
**Owner**: Database Admin  
**Effort**: 3 hours  
**Depends on**: Task 2.1.3, Task 3.1.1

**Definition of Done**:
- [ ] Data dumped from source (--data-only)
- [ ] Data imported to production Neon branch
- [ ] Row count validation passed (all tables match)
- [ ] Bulk copy log saved: `bulk_copy_YYYYMMDD.log`

**Commands**:
```bash
# Dump data only from source
pg_dump -h localhost -U postgres -d bank_db \
  --data-only --no-acl --no-owner > data_only.sql

# Restore to Neon production
psql $NEON_PROD_URL -f data_only.sql > bulk_copy_$(date +%Y%m%d).log 2>&1

# Verify row counts
bash scripts/validate_row_counts.sh $NEON_PROD_URL
```

---

### Task 3.1.3: Validate Data Migration
**Status**: ⬜ Not Started  
**Owner**: QA  
**Effort**: 2 hours  
**Depends on**: Task 3.1.2

**Definition of Done**:
- [ ] Row count validation completed (all tables)
- [ ] Checksum validation on sample records
- [ ] Referential integrity verified
- [ ] Identity sequences verified
- [ ] Validation report created: `data_validation_report.md`

**Validation Script** (scripts/validate_row_counts.sh):
```bash
#!/bin/bash
TARGET_DB=$1

tables=("users" "accounts" "transactions" "deposits" "loans" "cards" "payments")

for table in "${tables[@]}"; do
  src=$(psql -h localhost -U postgres -d bank_db -tc "SELECT COUNT(*) FROM $table")
  tgt=$(psql "$TARGET_DB" -tc "SELECT COUNT(*) FROM $table")
  
  if [ "$src" -ne "$tgt" ]; then
    echo "❌ MISMATCH: $table (src: $src, tgt: $tgt)"
  else
    echo "✅ OK: $table ($src records)"
  fi
done
```

**Acceptance Criteria**:
- All row counts match source database
- No orphaned records (referential integrity OK)
- All identity sequences at correct values
- Checksum samples match

---

## Phase 4: Configuration & Deployment (Week 4)

### Task 4.1.1: Update docker-compose.yml
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 1 hour  
**Depends on**: Task 1.1.2

**Definition of Done**:
- [ ] PostgreSQL service removed from docker-compose.yml
- [ ] DATABASE_URL environment variable added
- [ ] .env file created with development connection string
- [ ] .env added to .gitignore (if not already)
- [ ] Local development tested with docker-compose up

**Changes**:
- Remove: `postgres` service definition
- Keep: `bank-api` service with environment variable injection
- New file: `.env` (gitignored)

**Acceptance Criteria**:
- `docker-compose up` starts successfully
- Application connects to Neon dev branch
- No local PostgreSQL container started

---

### Task 4.2.1: Create Kubernetes Secrets
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 1 hour  
**Depends on**: Task 1.1.2

**Definition of Done**:
- [ ] Secret `neon-prod-db` created in `production` namespace
- [ ] Secret `neon-staging-db` created in `staging` namespace
- [ ] Secret values verified with kubectl
- [ ] Only DevOps team can view secrets

**Commands**:
```bash
kubectl create secret generic neon-prod-db \
  --from-literal=connection-string='postgresql://...' \
  --namespace=production

kubectl create secret generic neon-staging-db \
  --from-literal=connection-string='postgresql://...' \
  --namespace=staging
```

---

### Task 4.2.2: Update Kubernetes Deployment
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 2 hours  
**Depends on**: Task 4.2.1

**Definition of Done**:
- [ ] backend.yaml updated to use Neon secrets
- [ ] DATABASE_URL environment variable injected from secret
- [ ] No hardcoded connection strings in manifest
- [ ] Deployment tested in staging environment
- [ ] Health checks configured

**Changes** (devops/kubernetes/backend.yaml):
```yaml
containers:
- name: bank-api
  env:
  - name: DATABASE_URL
    valueFrom:
      secretKeyRef:
        name: neon-prod-db
        key: connection-string
  - name: ASPNETCORE_ENVIRONMENT
    value: "Production"
  livenessProbe:
    httpGet:
      path: /health
      port: 5000
    initialDelaySeconds: 30
    periodSeconds: 10
```

---

### Task 4.3.1: Document Neon in Terraform
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 1 hour  
**Depends on**: Task 1.1.2

**Definition of Done**:
- [ ] File `devops/terraform/neon.tf` created
- [ ] Project ID documented
- [ ] Branch configuration documented
- [ ] Connection strings as outputs (sensitive)
- [ ] Comments explain why resource is not IaC-managed

**Acceptance Criteria**:
- Terraform validates without errors
- Outputs show correct connection strings
- Clear explanation why Neon managed via console/CLI

---

## Phase 5: Zero-Downtime Cutover (Day 1)

### Task 5.1.1: Create Cutover Runbook
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 3 hours  
**Depends on**: Design document (design.md)

**Definition of Done**:
- [ ] Runbook created: `docs/cutover-runbook.md`
- [ ] Step-by-step procedures documented
- [ ] All commands provided
- [ ] Health check procedures detailed
- [ ] Rollback procedures included
- [ ] Escalation contacts listed

**Content**:
- Pre-cutover checklist
- Cutover step-by-step (with timeouts)
- Health check validation
- Rollback procedures
- Post-cutover validation
- Estimated duration: 30 minutes

---

### Task 5.1.2: Execute Dry-Run Cutover (Staging)
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 4 hours  
**Depends on**: Task 5.1.1

**Definition of Done**:
- [ ] Dry-run executed on staging database
- [ ] All steps completed successfully
- [ ] Health checks passed on target
- [ ] Rollback executed and verified
- [ ] Dry-run log saved: `cutover_dryrun_YYYYMMDD.log`
- [ ] Lessons learned documented

**Procedure**:
1. Verify replication lag < 1 sec
2. Set staging app to read-only mode (test)
3. Wait for replication to catch up
4. Switch config to production branch (test)
5. Verify health checks pass
6. Rollback to staging branch
7. Verify rollback succeeded

**Acceptance Criteria**:
- Dry-run completed without errors
- Rollback successful
- Confidence level for production cutover: HIGH

---

### Task 5.2.1: Execute Production Cutover
**Status**: ⬜ Not Started  
**Owner**: DevOps (with on-call team)  
**Effort**: 2 hours  
**Depends on**: Task 5.1.2, Task 3.1.3

**Definition of Done**:
- [ ] Cutover executed Sunday 02:00 AM UTC
- [ ] Zero errors during cutover
- [ ] All health checks passed
- [ ] User notification sent
- [ ] Cutover log saved
- [ ] Rollback not needed

**Timeline**:
- T-24h: User notification (24-hour warning)
- T-5min: Final pre-cutover checks
- T+0min: Begin cutover
- T+5min: Application should be on Neon
- T+30min: All health checks confirmed

**Acceptance Criteria**:
- Application running on Neon production branch
- All health checks green
- No user-visible errors
- < 2 minute service interruption

---

## Phase 6: Post-Migration (Week 6+)

### Task 6.1.1: Run Smoke Tests
**Status**: ⬜ Not Started  
**Owner**: QA  
**Effort**: 3 hours  
**Depends on**: Task 5.2.1

**Definition of Done**:
- [ ] All smoke tests passed
- [ ] Test report generated
- [ ] No critical issues found
- [ ] Test log: `smoke_test_report_YYYYMMDD.md`

**Test Cases**:
- User authentication (login/logout)
- Account creation and retrieval
- Transaction processing
- Loan operations
- Deposit operations
- Card operations
- Payment processing
- Reporting/statements

**Acceptance Criteria**:
- 100% of smoke tests passing
- No functional regressions
- Application performance acceptable

---

### Task 6.1.2: Data Validation (Post-Cutover)
**Status**: ⬜ Not Started  
**Owner**: QA  
**Effort**: 2 hours  
**Depends on**: Task 5.2.1

**Definition of Done**:
- [ ] Post-cutover row count validation completed
- [ ] Checksums verified
- [ ] Referential integrity confirmed
- [ ] Identity sequences verified
- [ ] Final validation report: `post_cutover_validation_YYYYMMDD.md`

**Acceptance Criteria**:
- All data matches pre-cutover snapshot
- No data loss
- All relationships intact

---

### Task 6.2.1: Setup Monitoring & Alerts
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 3 hours  
**Depends on**: Task 5.2.1

**Definition of Done**:
- [ ] Prometheus scrape config updated
- [ ] Grafana dashboard created
- [ ] Alerts configured:
  - CPU > 80%
  - Connections > 90%
  - Storage > 90%
  - Query latency p95 > 500ms
- [ ] Alert channels tested

**Metrics**:
- CPU utilization
- Active connections
- Storage used
- Query latency (p95, p99)
- Transaction throughput

---

### Task 6.3.1: Document Neon Operations
**Status**: ⬜ Not Started  
**Owner**: DevOps  
**Effort**: 2 hours  
**Depends on**: Task 5.2.1

**Definition of Done**:
- [ ] Operations manual created: `docs/neon-operations.md`
- [ ] Cost optimization documented
- [ ] Scaling procedures documented
- [ ] Backup/recovery procedures documented
- [ ] Best practices documented

**Sections**:
- Accessing Neon console
- Scaling compute resources
- Branch management
- Backup/recovery procedures
- Performance tuning
- Cost monitoring
- Support contacts

---

## Summary

**Total Tasks**: 30  
**Total Effort**: ~50 hours  
**Timeline**: 6 weeks  
**Dependencies**: Linear progression through phases

---

## Legend

- ⬜ Not Started
- 🟨 In Progress
- 🟩 Completed
- ❌ Blocked
- ⚠️ At Risk

