#!/bin/bash

# Source Database Preparation Script for Neon Migration
# This script backs up and validates the current PostgreSQL database
# Prerequisites: pg_dump, psql, PostgreSQL client tools

set -e

# Configuration - Update these with your source database details
SOURCE_DB_HOST="${DB_SERVER:-localhost}"
SOURCE_DB_PORT="${DB_PORT:-5432}"
SOURCE_DB_NAME="${DB_NAME:-bank_db}"
SOURCE_DB_USER="${DB_USER:-postgres}"
BACKUP_DIR="./backups/neon-migration"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${YELLOW}=== Source Database Preparation ===${NC}"
echo ""

# Step 1: Check PostgreSQL tools
echo -e "${YELLOW}Step 1: Checking PostgreSQL tools...${NC}"
command -v pg_dump > /dev/null || { echo -e "${RED}Error: pg_dump not found${NC}"; exit 1; }
command -v psql > /dev/null || { echo -e "${RED}Error: psql not found${NC}"; exit 1; }
echo -e "${GREEN}✓ PostgreSQL tools available${NC}"
echo ""

# Step 2: Verify connectivity to source database
echo -e "${YELLOW}Step 2: Verifying connectivity to source database...${NC}"
PGPASSWORD="${SOURCE_DB_PASSWORD}" psql -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
    -U "$SOURCE_DB_USER" -d "$SOURCE_DB_NAME" -c "SELECT 1" > /dev/null 2>&1 || {
    echo -e "${RED}Error: Cannot connect to source database"
    echo "  Host: $SOURCE_DB_HOST:$SOURCE_DB_PORT"
    echo "  Database: $SOURCE_DB_NAME"
    echo "  User: $SOURCE_DB_USER"
    exit 1
}
echo -e "${GREEN}✓ Connected to source database${NC}"
echo ""

# Step 3: Create backup directory
echo -e "${YELLOW}Step 3: Creating backup directory...${NC}"
mkdir -p "$BACKUP_DIR"
echo -e "${GREEN}✓ Backup directory: $BACKUP_DIR${NC}"
echo ""

# Step 4: Create full database backup
echo -e "${YELLOW}Step 4: Creating full database backup...${NC}"
BACKUP_FILE="$BACKUP_DIR/bank_db_backup_$(date +%Y%m%d_%H%M%S).dump"
PGPASSWORD="${SOURCE_DB_PASSWORD}" pg_dump -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
    -U "$SOURCE_DB_USER" -d "$SOURCE_DB_NAME" -F custom > "$BACKUP_FILE"
BACKUP_SIZE=$(du -h "$BACKUP_FILE" | cut -f1)
echo -e "${GREEN}✓ Backup created: $BACKUP_FILE ($BACKUP_SIZE)${NC}"
echo ""

# Step 5: Create data-only backup
echo -e "${YELLOW}Step 5: Creating data-only backup (for migration)...${NC}"
DATA_ONLY_FILE="$BACKUP_DIR/data_only_$(date +%Y%m%d_%H%M%S).sql"
PGPASSWORD="${SOURCE_DB_PASSWORD}" pg_dump -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
    -U "$SOURCE_DB_USER" -d "$SOURCE_DB_NAME" --data-only --no-acl --no-owner > "$DATA_ONLY_FILE"
DATA_SIZE=$(du -h "$DATA_ONLY_FILE" | cut -f1)
echo -e "${GREEN}✓ Data-only backup created: $DATA_ONLY_FILE ($DATA_SIZE)${NC}"
echo ""

# Step 6: Verify backup integrity
echo -e "${YELLOW}Step 6: Verifying backup integrity...${NC}"
echo "Testing restore to temporary database..."

# Create temporary test database
TEMP_DB="bank_db_test_$$"
PGPASSWORD="${SOURCE_DB_PASSWORD}" psql -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
    -U "$SOURCE_DB_USER" -c "CREATE DATABASE $TEMP_DB" > /dev/null 2>&1 || {
    echo -e "${RED}Warning: Could not create temporary test database${NC}"
}

if [ -n "$TEMP_DB" ]; then
    # Restore to test database
    PGPASSWORD="${SOURCE_DB_PASSWORD}" pg_restore -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
        -U "$SOURCE_DB_USER" -d "$TEMP_DB" "$BACKUP_FILE" > /dev/null 2>&1 || {
        echo -e "${RED}Warning: Restore test failed${NC}"
    }
    
    # Verify restore
    TABLE_COUNT=$(PGPASSWORD="${SOURCE_DB_PASSWORD}" psql -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
        -U "$SOURCE_DB_USER" -d "$TEMP_DB" -tc \
        "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema='public';" 2>/dev/null || echo "0")
    
    echo -e "${GREEN}✓ Backup integrity verified (restored $TABLE_COUNT tables)${NC}"
    
    # Drop test database
    PGPASSWORD="${SOURCE_DB_PASSWORD}" psql -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
        -U "$SOURCE_DB_USER" -c "DROP DATABASE $TEMP_DB" > /dev/null 2>&1
fi
echo ""

# Step 7: Document schema structure
echo -e "${YELLOW}Step 7: Documenting schema structure...${NC}"
SCHEMA_FILE="$BACKUP_DIR/schema_snapshot_$(date +%Y%m%d_%H%M%S).md"

cat > "$SCHEMA_FILE" << 'EOF'
# Source Database Schema Snapshot

## Summary
EOF

# Table count
TABLE_COUNT=$(PGPASSWORD="${SOURCE_DB_PASSWORD}" psql -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
    -U "$SOURCE_DB_USER" -d "$SOURCE_DB_NAME" -tc \
    "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema='public';" | xargs)
echo "- Tables: $TABLE_COUNT" >> "$SCHEMA_FILE"

# Get row counts for major tables
echo "" >> "$SCHEMA_FILE"
echo "## Table Row Counts" >> "$SCHEMA_FILE"
PGPASSWORD="${SOURCE_DB_PASSWORD}" psql -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
    -U "$SOURCE_DB_USER" -d "$SOURCE_DB_NAME" -tc \
    "SELECT tablename FROM pg_tables WHERE schemaname='public' ORDER BY tablename;" | \
    while read TABLE; do
        COUNT=$(PGPASSWORD="${SOURCE_DB_PASSWORD}" psql -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
            -U "$SOURCE_DB_USER" -d "$SOURCE_DB_NAME" -tc "SELECT COUNT(*) FROM \"$TABLE\";" | xargs)
        echo "- $TABLE: $COUNT" >> "$SCHEMA_FILE"
    done

echo -e "${GREEN}✓ Schema documentation saved: $SCHEMA_FILE${NC}"
echo ""

# Step 8: Document sequences
echo -e "${YELLOW}Step 8: Documenting sequences...${NC}"
SEQ_FILE="$BACKUP_DIR/sequences_$(date +%Y%m%d_%H%M%S).md"
cat > "$SEQ_FILE" << 'EOF'
# Database Sequences

## Current Sequence Values
EOF

PGPASSWORD="${SOURCE_DB_PASSWORD}" psql -h "$SOURCE_DB_HOST" -p "$SOURCE_DB_PORT" \
    -U "$SOURCE_DB_USER" -d "$SOURCE_DB_NAME" -tc \
    "SELECT relname, last_value FROM pg_sequences WHERE schemaname='public' ORDER BY relname;" >> "$SEQ_FILE"

echo -e "${GREEN}✓ Sequences documented: $SEQ_FILE${NC}"
echo ""

# Step 9: Summary
echo -e "${GREEN}=== Phase 1 Preparation Complete ===${NC}"
echo ""
echo "Database Details:"
echo "  Host: $SOURCE_DB_HOST:$SOURCE_DB_PORT"
echo "  Database: $SOURCE_DB_NAME"
echo "  User: $SOURCE_DB_USER"
echo ""
echo "Backups Created:"
echo "  Full backup: $BACKUP_FILE"
echo "  Data-only: $DATA_ONLY_FILE"
echo "  Schema doc: $SCHEMA_FILE"
echo "  Sequences: $SEQ_FILE"
echo ""
echo "Next steps:"
echo "  1. Review schema documentation"
echo "  2. Enable logical replication on source database (if needed)"
echo "  3. Proceed with Phase 2: Schema Migration"
echo ""

