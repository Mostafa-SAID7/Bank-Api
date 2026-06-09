#!/bin/bash

# Neon Database Migration Setup Script
# This script automates Phase 1 pre-migration setup tasks
# Prerequisites: neonctl CLI installed and authenticated

set -e

# Color codes for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Configuration
PROJECT_NAME="bank-api-prod"
PROJECT_REGION="us-east-1"
BRANCHES=("production" "staging" "development")

echo -e "${YELLOW}=== Neon Database Migration - Phase 1 Setup ===${NC}"
echo ""

# Step 1: Check neonctl installation
echo -e "${YELLOW}Step 1: Checking neonctl installation...${NC}"
if ! command -v neonctl &> /dev/null; then
    echo -e "${RED}Error: neonctl CLI not found. Install it with: npm install -g neonctl${NC}"
    exit 1
fi
echo -e "${GREEN}✓ neonctl is installed${NC}"
echo ""

# Step 2: Check authentication
echo -e "${YELLOW}Step 2: Checking Neon authentication...${NC}"
if ! neonctl projects list &> /dev/null; then
    echo -e "${YELLOW}⚠ Not authenticated. Starting authentication flow...${NC}"
    neonctl auth
fi
echo -e "${GREEN}✓ Authentication successful${NC}"
echo ""

# Step 3: List existing organizations
echo -e "${YELLOW}Step 3: Listing organizations...${NC}"
ORG_ID=$(neonctl orgs list --output json 2>/dev/null | jq -r '.[0].id // empty')

if [ -z "$ORG_ID" ]; then
    echo -e "${RED}Error: No organizations found${NC}"
    exit 1
fi

ORG_NAME=$(neonctl orgs list --output json 2>/dev/null | jq -r '.[0].name')
echo -e "${GREEN}✓ Organization: $ORG_NAME (ID: $ORG_ID)${NC}"
echo ""

# Step 4: Create Neon project
echo -e "${YELLOW}Step 4: Creating Neon project '$PROJECT_NAME'...${NC}"
PROJECT_RESPONSE=$(neonctl projects create \
    --name "$PROJECT_NAME" \
    --region "$PROJECT_REGION" \
    --output json 2>/dev/null || echo "{}")

PROJECT_ID=$(echo "$PROJECT_RESPONSE" | jq -r '.id // empty')

if [ -z "$PROJECT_ID" ]; then
    # Project might already exist, try to find it
    echo -e "${YELLOW}⚠ Could not create project. Checking if it exists...${NC}"
    PROJECT_ID=$(neonctl projects list --output json 2>/dev/null | jq -r ".[] | select(.name==\"$PROJECT_NAME\") | .id" | head -n1)
    
    if [ -z "$PROJECT_ID" ]; then
        echo -e "${RED}Error: Could not create or find project${NC}"
        exit 1
    fi
    echo -e "${GREEN}✓ Found existing project: $PROJECT_ID${NC}"
else
    echo -e "${GREEN}✓ Created project: $PROJECT_ID${NC}"
fi
echo ""

# Step 5: Create branches
echo -e "${YELLOW}Step 5: Creating database branches...${NC}"

for BRANCH in "${BRANCHES[@]}"; do
    echo "Creating branch: $BRANCH"
    
    # Determine parent branch
    if [ "$BRANCH" == "production" ]; then
        PARENT="main"
    elif [ "$BRANCH" == "staging" ]; then
        PARENT="production"
    else
        PARENT="staging"
    fi
    
    # Try to create branch
    BRANCH_RESPONSE=$(neonctl branches create \
        --project-id "$PROJECT_ID" \
        --branch-name "$BRANCH" \
        --parent "$PARENT" \
        --output json 2>/dev/null || echo "{}")
    
    BRANCH_ID=$(echo "$BRANCH_RESPONSE" | jq -r '.id // empty')
    
    if [ -z "$BRANCH_ID" ]; then
        # Branch might already exist
        BRANCH_ID=$(neonctl branches list --project-id "$PROJECT_ID" --output json 2>/dev/null | \
            jq -r ".[] | select(.name==\"$BRANCH\") | .id" | head -n1)
        
        if [ -z "$BRANCH_ID" ]; then
            echo -e "${RED}✗ Failed to create/find branch: $BRANCH${NC}"
            continue
        fi
        echo -e "${GREEN}✓ Found existing branch: $BRANCH (ID: $BRANCH_ID)${NC}"
    else
        echo -e "${GREEN}✓ Created branch: $BRANCH (ID: $BRANCH_ID)${NC}"
    fi
done
echo ""

# Step 6: Get connection strings
echo -e "${YELLOW}Step 6: Retrieving connection strings...${NC}"
echo ""

# Create .env.neon.local with actual connection strings
ENV_FILE=".env.neon.local"
> "$ENV_FILE"  # Clear file

for BRANCH in "${BRANCHES[@]}"; do
    echo "Getting connection string for: $BRANCH"
    
    CONN_STRING=$(neonctl connection-string \
        --project-id "$PROJECT_ID" \
        --branch-name "$BRANCH" 2>/dev/null || echo "")
    
    if [ -z "$CONN_STRING" ]; then
        echo -e "${RED}✗ Failed to get connection string for: $BRANCH${NC}"
        continue
    fi
    
    # Map branch to environment variable name
    ENV_VAR_NAME=""
    case "$BRANCH" in
        development) ENV_VAR_NAME="DATABASE_URL_DEV" ;;
        staging) ENV_VAR_NAME="DATABASE_URL_STAGING" ;;
        production) ENV_VAR_NAME="DATABASE_URL_PROD" ;;
    esac
    
    # Save to file
    echo "# ${BRANCH} branch" >> "$ENV_FILE"
    echo "$ENV_VAR_NAME='$CONN_STRING'" >> "$ENV_FILE"
    echo "" >> "$ENV_FILE"
    
    echo -e "${GREEN}✓ $BRANCH: Retrieved${NC}"
done

echo -e "${GREEN}✓ Connection strings saved to: $ENV_FILE${NC}"
echo ""

# Step 7: Summary
echo -e "${GREEN}=== Phase 1 Setup Complete ===${NC}"
echo ""
echo "Project Details:"
echo "  Organization: $ORG_NAME (ID: $ORG_ID)"
echo "  Project: $PROJECT_NAME (ID: $PROJECT_ID)"
echo "  Region: $PROJECT_REGION"
echo ""
echo "Branches created:"
for BRANCH in "${BRANCHES[@]}"; do
    echo "  - $BRANCH"
done
echo ""
echo "Next steps:"
echo "  1. Review connection strings in: $ENV_FILE"
echo "  2. Copy DATABASE_URL_DEV to .env for local development"
echo "  3. Store DATABASE_URL_PROD and DATABASE_URL_STAGING in Kubernetes secrets"
echo "  4. Run Phase 2: Schema migration"
echo ""
echo -e "${YELLOW}Keep $ENV_FILE secure and never commit to version control!${NC}"

