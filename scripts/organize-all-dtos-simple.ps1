#!/usr/bin/env pwsh
# Organize all DTOs into subfolders by category

$dtoBasePath = "Bank-Api/src/Bank.Application/DTOs"

# Auth organization
Write-Host "Organizing Auth DTOs..." -ForegroundColor Cyan
$authPath = "$dtoBasePath/Auth"
@("Core", "TwoFactor", "Security", "Session") | ForEach-Object { 
    New-Item -ItemType Directory -Path "$authPath/$_" -Force | Out-Null 
}

# Card organization
Write-Host "Organizing Card DTOs..." -ForegroundColor Cyan
$cardPath = "$dtoBasePath/Card"
@("Core", "Activation", "Transactions", "Fees", "Operations", "Advanced") | ForEach-Object { 
    New-Item -ItemType Directory -Path "$cardPath/$_" -Force | Out-Null 
}

# Deposit organization
Write-Host "Organizing Deposit DTOs..." -ForegroundColor Cyan
$depositPath = "$dtoBasePath/Deposit"
@("Core", "FixedDeposit", "Interest", "Maturity", "Withdrawal") | ForEach-Object { 
    New-Item -ItemType Directory -Path "$depositPath/$_" -Force | Out-Null 
}

# Loan organization
Write-Host "Organizing Loan DTOs..." -ForegroundColor Cyan
$loanPath = "$dtoBasePath/Loan"
@("Core", "Application", "Approval", "Disbursement", "Repayment", "Analytics", "Configuration") | ForEach-Object { 
    New-Item -ItemType Directory -Path "$loanPath/$_" -Force | Out-Null 
}

# Payment organization
Write-Host "Organizing Payment DTOs..." -ForegroundColor Cyan
$paymentPath = "$dtoBasePath/Payment"
@("Core", "Beneficiary", "Biller", "Batch", "Routing", "Receipt", "Recurring", "Template") | ForEach-Object { 
    New-Item -ItemType Directory -Path "$paymentPath/$_" -Force | Out-Null 
}

# Statement organization
Write-Host "Organizing Statement DTOs..." -ForegroundColor Cyan
$statementPath = "$dtoBasePath/Statement"
@("Core", "Search", "Summary", "Delivery", "Analytics", "Transaction") | ForEach-Object { 
    New-Item -ItemType Directory -Path "$statementPath/$_" -Force | Out-Null 
}

# Transaction organization
Write-Host "Organizing Transaction DTOs..." -ForegroundColor Cyan
$transactionPath = "$dtoBasePath/Transaction"
@("Core", "Search", "Analytics", "Fraud") | ForEach-Object { 
    New-Item -ItemType Directory -Path "$transactionPath/$_" -Force | Out-Null 
}

# Shared organization
Write-Host "Organizing Shared DTOs..." -ForegroundColor Cyan
$sharedPath = "$dtoBasePath/Shared"
@("Notification", "Audit", "RateLimit") | ForEach-Object { 
    New-Item -ItemType Directory -Path "$sharedPath/$_" -Force | Out-Null 
}

Write-Host ""
Write-Host "All subfolders created successfully!" -ForegroundColor Green
