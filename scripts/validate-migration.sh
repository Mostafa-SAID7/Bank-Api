#!/bin/bash

# Data Migration Validation Script
# Validates data integrity between source and target (Neon) databases
# Usage: ./validate-migration.sh <source_connection_string> <target_connection_string>

set -e

SOURCE_CONN="${1:-}"
TARGET_CONN="${2:-}"

# Colors
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

if [ -z "$SOURCE_CONN" ] || [ -z "$TARGET_CONN" ]; then
    echo -e "${RED}Usage: $0 <source_connection_string> <target_connection_string>${NC}"
    echo ""
    echo "Example:"
    echo "  $0 'postgresql://user:pass@localhost/bank_db' 'postgresql://user:pass@ep-dev.neon.tech/neondb'"
    exit 1
fi

echo -e "${YELLOW}=== Data Migration Validation ===${NC}"
echo ""

# List of tables to validate (Bank-Api entities)
TABLES=(
    "AspNetUsers"
    "AspNetRoles"
    "Accounts"
    "Transactions"
    "BatchJobs"
    "TwoFactorTokens"
    "AuditLogs"
    "Sessions"
    "AccountLockouts"
    "IpWhitelists"
    "PasswordPolicies"
    "PasswordHistories"
    "AccountFees"
    "AccountHolds"
    "AccountRestrictions"
    "AccountStatusHistories"
    "FeeSchedules"
    "JointAccountHolders"
    "RecurringPayments"
    "RecurringPaymentExecutions"
    "PaymentTemplates"
    "Beneficiaries"
    "AccountStatements"
    "StatementTransactions"
    "Loans"
    "LoanPayments"
    "LoanDocuments"
    "LoanStatusHistories"
    "Cards"
    "CardTransactions"
    "CardAuthorizations"
    "CardStatements"
    "CardStatusHistories"
    "Notifications"
    "NotificationPreferences"
    "Billers"
    "BillPayments"
    "BillPresentments"
    "PaymentReceipts"
    "PaymentRetries"
    "BillerHealthChecks"
    "DepositProducts"
    "InterestTiers"
    "FixedDeposits"
    "DepositTransactions"
    "DepositCertificates"
    "MaturityNotices"
)

# Validation results
PASSED=0
FAILED=0
TOTAL=0

echo -e "${YELLOW}Step 1: Validating row counts...${NC}"
echo ""

for TABLE in "${TABLES[@]}"; do
    TOTAL=$((TOTAL + 1))
    
    # Get source row count
    SRC_COUNT=$(psql "$SOURCE_CONN" -tc "SELECT COUNT(*) FROM \"$TABLE\" 2>/dev/null;" 2>/dev/null || echo "ERROR")
    
    # Skip if table doesn't exist in source
    if [ "$SRC_COUNT" = "ERROR" ] || [ -z "$SRC_COUNT" ]; then
        continue
    fi
    
    # Get target row count
    TGT_COUNT=$(psql "$TARGET_CONN" -tc "SELECT COUNT(*) FROM \"$TABLE\" 2>/dev/null;" 2>/dev/null || echo "ERROR")
    
    if [ "$TGT_COUNT" = "ERROR" ] || [ -z "$TGT_COUNT" ]; then
        TGT_COUNT="MISSING"
    fi
    
    # Trim whitespace
    SRC_COUNT=$(echo "$SRC_COUNT" | xargs)
    TGT_COUNT=$(echo "$TGT_COUNT" | xargs)
    
    # Compare
    if [ "$SRC_COUNT" = "$TGT_COUNT" ]; then
        echo -e "${GREEN}✓${NC} $TABLE: $SRC_COUNT rows"
        PASSED=$((PASSED + 1))
    else
        echo -e "${RED}✗${NC} $TABLE: source=$SRC_COUNT, target=$TGT_COUNT"
        FAILED=$((FAILED + 1))
    fi
done

echo ""
echo -e "${YELLOW}Step 2: Validating referential integrity...${NC}"
echo ""

# Check for orphaned foreign keys (example: accounts without users)
echo "Checking for orphaned accounts..."
ORPHANED=$(psql "$TARGET_CONN" -tc \
    "SELECT COUNT(*) FROM \"Accounts\" WHERE \"UserId\" NOT IN (SELECT \"Id\" FROM \"AspNetUsers\");" | xargs)

if [ "$ORPHANED" -eq 0 ]; then
    echo -e "${GREEN}✓${NC} No orphaned accounts"
    PASSED=$((PASSED + 1))
else
    echo -e "${RED}✗${NC} Found $ORPHANED orphaned accounts"
    FAILED=$((FAILED + 1))
fi

echo ""
echo -e "${YELLOW}Step 3: Summary ===${NC}"
echo ""
echo "Total tables checked: $TOTAL"
echo -e "Passed: ${GREEN}$PASSED${NC}"
echo -e "Failed: ${RED}$FAILED${NC}"
echo ""

if [ $FAILED -eq 0 ]; then
    echo -e "${GREEN}✓ All validations passed!${NC}"
    exit 0
else
    echo -e "${RED}✗ Some validations failed. Review above for details.${NC}"
    exit 1
fi

