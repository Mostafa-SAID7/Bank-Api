# Neon Database Migration - Technical Design Document

## Overview

This document provides the technical design for migrating the Bank-Api database from a traditional self-hosted PostgreSQL to Neon serverless Postgres. The design translates 15 requirements into actionable implementation tasks, with specific tools, scripts, and procedures.

## Architecture

### Current State (As-Is)
- Bank-Api: C# .NET 6+ backend with Entity Framework Core (EF Core)
- Database: Self-hosted PostgreSQL (connection string in appsettings)
- Schema: 45+ entities with relationships (Users, Accounts, Loans, Deposits, Cards, Payments, etc.)
- Infrastructure: Docker Compose for local dev, Kubernetes for prod, Terraform for IaC
- Deployment: GitHub Actions CI/CD pipelines

### Target State (To-Be)
- Bank-Api: Same codebase, updated connection strings
- Database: Neon serverless Postgres with three branches:
  - **Production**: Main branch, no autoscaling, point-in-time recovery enabled
  - **Staging**: Autoscaling between 0.5-1 CU during off-peak (22:00-06:00), 1 CU during peak
  - **Development**: Scale-to-zero after 5 minutes inactivity, quick resume on connection
- Configuration: Environment-specific connection strings in appsettings and Kubernetes secrets
- Monitoring: Prometheus metrics + Grafana dashboards integrated with Neon

---

## Design Phases

### Phase 1: Pre-Migration Setup (Week 1)

#### 1.1 Neon Project Initialization
**Objective**: Create Neon infrastructure with proper branches and compute configuration.

**Steps**:
1. Create Neon account at console.neon.tech
2. Create new project: "bank-api-prod"
3. Create three branches from main:
   - `production` (parent: main, compute: 1 CU fixed)
   - `staging` (parent: production, compute: 0.5-1 CU autoscaling)
   - `development` (parent: staging, compute: scale-to-zero after 5 min)
4. Retrieve connection strings for all three branches
5. Store connection strings securely:
   - Production: Kubernetes secret `neon-prod-db`
   - Staging: Kubernetes secret `neon-staging-db`
   - Development: Local `.env` file (gitignored)

**Neon CLI Commands**:
```bash
# Authenticate
neonctl auth

# List organizations
neonctl orgs list

# Create project
neonctl projects create --name bank-api-prod --region us-east-1

# Get project ID (returned from above or use neonctl projects list)
PROJECT_ID=proj-xxxxx

# List branches
neonctl branches list --project-id $PROJECT_ID

# Create branches
neonctl branches create --project-id $PROJECT_ID --branch-name production --parent main
neonctl branches create --project-id $PROJECT_ID --branch-name staging --parent production
neonctl branches create --project-id $PROJECT_ID --branch-name development --parent staging

# Get connection strings
neonctl connection-string --project-id $PROJECT_ID --branch-name production
neonctl connection-string --project-id $PROJECT_ID --branch-name staging
neonctl connection-string --project-id $PROJECT_ID --branch-name development
```

**Output Format**: Connection strings will look like:
```
postgresql://user:password@ep-xxxxx.us-east-1.aws.neon.tech/main?sslmode=require
```

---

#### 1.2 Source Database Backup & Preparation
**Objective**: Ensure source database is backed up and ready for migration.

**Steps**:
1. Create full backup of current PostgreSQL database:
   ```bash
   pg_dump -h localhost -U postgres -d bank_db -F custom > bank_db_backup_$(date +%Y%m%d_%H%M%S).dump
   ```

2. Verify backup integrity by restoring to temporary database:
   ```bash
   createdb bank_db_test
   pg_restore -h localhost -U postgres -d bank_db_test bank_db_backup_20240101_120000.dump
   ```

3. Document schema structure:
   - Table count, row counts per table
   - Index count, foreign key count
   - Sequence current values
   - Extensions used (uuid-ossp, etc.)

4. Verify EF Core entity mappings match database schema:
   - Run EF Core validation: `dotnet ef dbcontext info`
   - Compare EntityConfigurations with actual schema
   - Document any mismatches

5. Enable logical replication on source database:
   ```sql
   -- As PostgreSQL superuser
   ALTER SYSTEM SET wal_level = logical;
   ALTER SYSTEM SET max_wal_senders = 10;
   ALTER SYSTEM SET max_replication_slots = 10;
   -- Restart PostgreSQL service
   sudo systemctl restart postgresql
   ```

---

#### 1.3 Entity Framework Core Configuration Update
**Objective**: Prepare EF Core to work with Neon connection strings.

**Changes to make**:
1. Update `BankDbContext.cs` to support parameterized connection strings:
   ```csharp
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
       if (!optionsBuilder.IsConfigured)
       {
           var connectionString = _configuration.GetConnectionString("NeonDatabase");
           optionsBuilder.UseNpgsql(connectionString, options =>
           {
               options.SetPostgresVersion(new Version(14, 0)); // Neon PostgreSQL version
               options.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelaySeconds: 5);
           });
       }
   }
   ```

2. Update `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "NeonDatabase": "postgresql://user:pass@ep-prod.neon.tech/main?sslmode=require"
     }
   }
   ```

3. Update `appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "NeonDatabase": "postgresql://user:pass@ep-dev.neon.tech/main?sslmode=require"
     }
   }
   ```

4. Create `appsettings.Staging.json`:
   ```json
   {
     "ConnectionStrings": {
       "NeonDatabase": "postgresql://user:pass@ep-staging.neon.tech/main?sslmode=require"
     }
   }
   ```

5. Support environment variable override:
   ```csharp
   var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
       ?? _configuration.GetConnectionString("NeonDatabase");
   ```

---

### Phase 2: Schema Migration (Week 2)

#### 2.1 Create Target Schema on Neon (Development Branch)
**Objective**: Apply EF Core migrations to create schema on Neon development branch.

**Steps**:
1. Temporarily update connection string to development branch
2. Run EF Core migrations:
   ```bash
   cd src/Bank.Api
   dotnet ef database update --context BankDbContext --environment Development
   ```

3. Verify schema creation:
   ```sql
   SELECT table_name FROM information_schema.tables 
   WHERE table_schema = 'public' ORDER BY table_name;
   ```

4. Compare table count:
   - Source: ~45 tables
   - Target: Should match

5. Enable required PostgreSQL extensions:
   ```sql
   CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
   CREATE EXTENSION IF NOT EXISTS "pg_trgm";
   ```

---

#### 2.2 Create Schema on Staging and Production Branches
**Objective**: Apply same schema to staging and production branches.

**Steps**:
1. Update connection string to staging branch
2. Run migrations:
   ```bash
   dotnet ef database update --context BankDbContext --environment Staging
   ```

3. Verify schema creation on staging
4. Update connection string to production branch
5. Run migrations:
   ```bash
   dotnet ef database update --context BankDbContext
   ```

6. Verify schema creation on production

**Result**: All three Neon branches have identical schema.

---

### Phase 3: Data Migration (Week 3)

#### 3.1 Set Up Logical Replication
**Objective**: Stream data from source to Neon continuously.

**Tools**: `pg_dump` for bulk copy + logical replication for incremental sync

**Steps**:
1. Create publication on source database:
   ```sql
   -- On source database
   CREATE PUBLICATION bank_api_pub FOR ALL TABLES;
   ```

2. Initial bulk copy using pg_dump:
   ```bash
   # Dump data only (no schema) from source
   pg_dump -h source-host -U postgres -d bank_db \
     --data-only --no-acl --no-owner | \
   # Restore to Neon (with connection pooling)
   psql -h ep-prod.neon.tech -U neondb -d main
   ```

3. Validate row counts on each major table:
   ```bash
   # Create validation script
   for table in users accounts transactions deposits loans cards payments; do
     source_count=$(psql -h source-host -U postgres -d bank_db -tc \
       "SELECT COUNT(*) FROM $table;")
     target_count=$(psql -h ep-prod.neon.tech -U neondb -d main -tc \
       "SELECT COUNT(*) FROM $table;")
     echo "$table: source=$source_count, target=$target_count"
   done
   ```

4. Monitor replication lag:
   ```sql
   -- Check on Neon (subscriber)
   SELECT slot_name, restart_lsn, confirmed_flush_lsn FROM pg_replication_slots;
   SELECT * FROM pg_stat_replication;
   ```

---

#### 3.2 Data Validation & Checksum Verification
**Objective**: Ensure all data migrated correctly.

**Steps**:
1. Row count validation (all tables):
   ```bash
   # Script to compare row counts
   declare -a tables=(
     "users" "accounts" "transactions" "deposits" 
     "loans" "cards" "payments" "beneficiaries"
   )
   
   for table in "${tables[@]}"; do
     src_count=$(psql -h source -U user -d bank_db -tc \
       "SELECT COUNT(*) FROM $table")
     tgt_count=$(psql -h ep-prod.neon.tech -U neondb -d main -tc \
       "SELECT COUNT(*) FROM $table")
     
     if [ "$src_count" -ne "$tgt_count" ]; then
       echo "MISMATCH: $table (src: $src_count, tgt: $tgt_count)"
     fi
   done
   ```

2. Checksum validation on sample records:
   ```sql
   -- Compare checksum of first 100 records in users table
   SELECT md5(string_agg(id::text, ',' ORDER BY id))
   FROM users LIMIT 100;
   ```

3. Validate referential integrity:
   ```sql
   -- Check for orphaned records (accounts with non-existent users)
   SELECT COUNT(*) FROM accounts 
   WHERE user_id NOT IN (SELECT id FROM users);
   ```

4. Verify identity sequences:
   ```sql
   -- Check current values match
   SELECT relname, last_value FROM pg_sequences 
   WHERE schemaname = 'public' ORDER BY relname;
   ```

---

### Phase 4: Configuration & Deployment (Week 4)

#### 4.1 Update Docker Compose
**Objective**: Remove local PostgreSQL, use Neon instead.

**Current docker-compose.yml**:
```yaml
services:
  postgres:
    image: postgres:14
    environment:
      POSTGRES_PASSWORD: password
    ports:
      - "5432:5432"
```

**New docker-compose.yml** (remove postgres service entirely):
```yaml
version: '3.8'
services:
  bank-api:
    build: .
    ports:
      - "5000:5000"
    environment:
      DATABASE_URL: ${DATABASE_URL}
      ASPNETCORE_ENVIRONMENT: Development
    env_file:
      - .env
```

**New .env file** (gitignored):
```
DATABASE_URL=postgresql://user:pass@ep-dev.neon.tech/main?sslmode=require
```

---

#### 4.2 Update Kubernetes Deployment
**Objective**: Inject Neon connection strings via secrets and ConfigMaps.

**Create Kubernetes Secret**:
```bash
kubectl create secret generic neon-prod-db \
  --from-literal=connection-string='postgresql://user:pass@ep-prod.neon.tech/main?sslmode=require' \
  --namespace=production
```

**Update backend.yaml**:
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: bank-api-backend
  namespace: production
spec:
  template:
    spec:
      containers:
      - name: bank-api
        image: bank-api:latest
        env:
        - name: DATABASE_URL
          valueFrom:
            secretKeyRef:
              name: neon-prod-db
              key: connection-string
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
```

---

#### 4.3 Update Terraform IaC
**Objective**: Document Neon project in Terraform (read-only, since Neon is managed via console/CLI).

**devops/terraform/neon.tf**:
```hcl
# Note: Neon project created manually via console or neonctl
# This is a reference for documentation purposes

locals {
  neon_project_id = "proj-xxxxx"
  neon_region     = "us-east-1"
  
  neon_branches = {
    production = {
      compute_size = 1
      autoscaling  = false
    }
    staging = {
      compute_size = 0.5
      autoscaling  = true
    }
    development = {
      compute_size = 0.25
      autoscaling  = true
      scale_to_zero = true
    }
  }
}

# Document connection strings as outputs
output "neon_connection_strings" {
  description = "Neon database connection strings by environment"
  value = {
    production  = "postgresql://user:pass@ep-prod.neon.tech/main?sslmode=require"
    staging     = "postgresql://user:pass@ep-staging.neon.tech/main?sslmode=require"
    development = "postgresql://user:pass@ep-dev.neon.tech/main?sslmode=require"
  }
  sensitive = true
}
```

---

### Phase 5: Zero-Downtime Cutover (Week 5)

#### 5.1 Pre-Cutover Validation (Dry Run)
**Objective**: Verify all cutover procedures before production cutover.

**Steps**:
1. Verify replication lag is < 1 second:
   ```sql
   SELECT slot_name, restart_lsn, confirmed_flush_lsn 
   FROM pg_replication_slots;
   ```

2. Stop writes to staging branch (simulate read-only):
   - Update application config to read-only flag
   - Verify no errors in application logs

3. Wait for replication to complete (verify lag = 0)

4. Finalize replication and disconnect from source

5. Update config to point to staging branch

6. Restart application and verify health checks pass

7. Rollback: Switch config back to source database

---

#### 5.2 Production Cutover (Sunday 02:00 AM UTC)
**Objective**: Execute zero-downtime cutover to production Neon branch.

**Timeline**:
- T-24h: Send maintenance notification to users
- T-0h: Begin cutover procedure
- T+5min: Application should be running on Neon
- T+30min: All health checks passed

**Cutover Steps**:
1. Verify replication lag < 1 second (max 5 second check timeout)
2. Set application to read-only mode (respond 503 to write requests)
3. Wait for in-flight transactions to complete (max 30 second drain window)
4. Stop replication
5. Finalize replication on target
6. Update application config: DATABASE_URL -> production Neon endpoint
7. Restart application (max 60 second timeout)
8. Run health checks (test query, business logic test, API endpoint test)
9. If health checks pass: declare success, send user notification
10. If health checks fail: initiate rollback (switch config back, restart, verify)

---

### Phase 6: Post-Migration (Week 6+)

#### 6.1 Data Validation & Smoke Tests
**Objective**: Verify all functionality works with Neon.

**Smoke Test Suite** (automated):
- Authentication: Login flow, session management
- Accounts: Create, read, update account
- Transactions: Deposit, withdrawal, transfer
- Loans: Create, approve, disburse loan
- Deposits: Create fixed deposit, calculate interest
- Cards: Create card, set PIN
- Payments: Create beneficiary, make payment
- Reporting: Generate statements, account summary

**Business Logic Validation**:
```bash
# Example: Verify interest calculation still works
curl -X GET "http://localhost:5000/api/deposits/1/interest" 
# Compare calculated interest with pre-migration values
```

---

#### 6.2 Monitoring Setup
**Objective**: Monitor Neon database performance.

**Prometheus Scrape Config** (prometheus.yml):
```yaml
scrape_configs:
  - job_name: 'neon-metrics'
    static_configs:
      - targets: ['neon-exporter:9090']
```

**Metrics to Monitor**:
- CPU utilization (alert > 80%)
- Active connections (alert > 90% of limit)
- Storage used (alert > 90%)
- Query latency (p95, p99)
- Replication lag (if applicable)

**Grafana Dashboard**: Create dashboard with these metrics

---

#### 6.3 Cost Optimization
**Objective**: Optimize Neon resource usage and costs.

**Configuration**:
- Production: Fixed 1 CU (no autoscaling)
- Staging: 0.5-1 CU autoscaling (peak hours: 06:00-22:00 UTC, 1 CU; off-peak: 0.5 CU)
- Development: Scale-to-zero after 5 minutes inactivity

**Cost Estimate**:
- Production: ~$100/month (fixed compute)
- Staging: ~$20/month (autoscaling savings)
- Development: ~$5/month (scale-to-zero)
- **Total**: ~$125/month (vs. ~$200/month for dedicated server)

---

## Rollback Plan

If critical issues occur during/after cutover:

**Immediate Rollback Steps**:
1. Revert DATABASE_URL to source database connection string
2. Restart application
3. Verify health checks pass on source database
4. Send user notification: "Issue resolved, service restored"

**Recovery Time Objective (RTO)**: 15 minutes
**Recovery Point Objective (RPO)**: Cutover timestamp (use data from source database)

---

## Risk Mitigation

| Risk | Mitigation |
|------|-----------|
| Data loss during migration | Full backup before migration; verify row counts pre/post |
| Replication lag > 1 sec during cutover | Monitor lag continuously; abort cutover if lag increases |
| Neon endpoint unavailable | Health checks; automatic rollback to source |
| Connection pooling limits exhausted | Configure PgBouncer or Neon connection pooling |
| EF Core schema mismatch | Validate schema before data migration |
| Cutover during business hours | Schedule cutover Sunday 02:00 AM UTC (off-peak) |

---

## Success Criteria

- [ ] All 15 requirements satisfied
- [ ] Zero-downtime cutover executed successfully
- [ ] Data integrity validated (row counts, checksums, referential integrity)
- [ ] All smoke tests passing
- [ ] Application health checks passing
- [ ] No rollbacks triggered
- [ ] Cost < $150/month
- [ ] Monitoring dashboards showing normal metrics

---

## Timeline Summary

| Phase | Duration | Status |
|-------|----------|--------|
| Phase 1: Setup | 1 week | Not started |
| Phase 2: Schema | 1 week | Not started |
| Phase 3: Data | 1 week | Not started |
| Phase 4: Config | 1 week | Not started |
| Phase 5: Cutover | 1 day | Not started |
| Phase 6: Post-Migration | 1+ week | Not started |
| **Total** | **6 weeks** | |

---

## Design Review Questions

1. **Connection Pooling**: Should we use PgBouncer or Neon's built-in connection pooling?
2. **Failover Strategy**: Should we keep source database as warm standby post-migration?
3. **Backup Strategy**: Neon's automated backups (30 days), or keep external backups?
4. **Monitoring**: Integrate with existing Prometheus/Grafana setup, or use Neon's dashboard?
5. **Cost**: Is ~$125/month acceptable compared to current database costs?

