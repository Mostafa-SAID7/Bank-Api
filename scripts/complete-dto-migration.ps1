#!/usr/bin/env pwsh
# Complete DTO Migration - Move all files to subfolders and update namespaces

$dtoBasePath = "Bank-Api/src/Bank.Application/DTOs"
$movedCount = 0

# ============================================================================
# AUTH DTOs - Complete remaining files
# ============================================================================
Write-Host "`n=== AUTH DTOs ===" -ForegroundColor Cyan

$authMappings = @{
    "AddIpWhitelistRequestDto.cs" = "Security"
    "ApproveIpWhitelistRequestDto.cs" = "Security"
    "GeneratePasswordRequestDto.cs" = "Security"
    "LockAccountRequestDto.cs" = "Security"
    "SessionDTOs.cs" = "Session"
    "ValidatePasswordRequestDto.cs" = "Security"
}

foreach ($file in $authMappings.Keys) {
    $subfolder = $authMappings[$file]
    $source = "$dtoBasePath/Auth/$file"
    $dest = "$dtoBasePath/Auth/$subfolder/$file"
    
    if (Test-Path $source) {
        $content = Get-Content $source -Raw -Encoding UTF8
        $oldNs = "namespace Bank.Application.DTOs.Auth;"
        $newNs = "namespace Bank.Application.DTOs.Auth.$subfolder;"
        $newContent = $content -replace [regex]::Escape($oldNs), $newNs
        
        $destDir = Split-Path $dest
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        
        Set-Content -Path $dest -Value $newContent -Encoding UTF8 -Force
        Remove-Item $source -Force
        Write-Host "  [OK] $file -> $subfolder/" -ForegroundColor Green
        $movedCount++
    }
}

# ============================================================================
# CARD DTOs - Move all files
# ============================================================================
Write-Host "`n=== CARD DTOs ===" -ForegroundColor Cyan

$cardMappings = @{
    "CardDetailsDto.cs" = "Core"
    "CardDto.cs" = "Core"
    "CardSummaryDto.cs" = "Core"
    "AccountSummaryDto.cs" = "Core"
    "CardActivationDto.cs" = "Activation"
    "CardIssuanceDto.cs" = "Activation"
    "CardRenewalDto.cs" = "Activation"
    "CardTransactionDto.cs" = "Transactions"
    "CardTransactionSearchDto.cs" = "Transactions"
    "CardTransactionProcessingDto.cs" = "Transactions"
    "CardStatementDto.cs" = "Transactions"
    "CardTransactionFeesDto.cs" = "Fees"
    "CardLimitDto.cs" = "Fees"
    "CardBlockDto.cs" = "Operations"
    "CardPinDto.cs" = "Operations"
    "CardValidationDto.cs" = "Operations"
    "CardVoidDto.cs" = "Operations"
    "CardRefundDto.cs" = "Operations"
    "CardCaptureDto.cs" = "Operations"
    "CardAuthorizationDto.cs" = "Operations"
    "CardContactlessDto.cs" = "Advanced"
    "CardOnlineTransactionsDto.cs" = "Advanced"
    "CardInternationalTransactionsDto.cs" = "Advanced"
    "CardMerchantRestrictionsDto.cs" = "Advanced"
    "CardSettlementDto.cs" = "Advanced"
    "CardNetworkStatusDto.cs" = "Advanced"
    "CardUsageStatsDto.cs" = "Advanced"
    "PinManagementDTOs.cs" = "Advanced"
    "PagedRequestDto.cs" = "Advanced"
    "CardBatchSettlementDto.cs" = "Advanced"
}

foreach ($file in $cardMappings.Keys) {
    $subfolder = $cardMappings[$file]
    $source = "$dtoBasePath/Card/$file"
    $dest = "$dtoBasePath/Card/$subfolder/$file"
    
    if (Test-Path $source) {
        $content = Get-Content $source -Raw -Encoding UTF8
        $oldNs = "namespace Bank.Application.DTOs.Card;"
        $newNs = "namespace Bank.Application.DTOs.Card.$subfolder;"
        $newContent = $content -replace [regex]::Escape($oldNs), $newNs
        
        $destDir = Split-Path $dest
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        
        Set-Content -Path $dest -Value $newContent -Encoding UTF8 -Force
        Remove-Item $source -Force
        Write-Host "  [OK] $file -> $subfolder/" -ForegroundColor Green
        $movedCount++
    }
}

# ============================================================================
# DEPOSIT DTOs - Move all files
# ============================================================================
Write-Host "`n=== DEPOSIT DTOs ===" -ForegroundColor Cyan

$depositMappings = @{
    "DepositProductDto.cs" = "Core"
    "DepositSummaryDto.cs" = "Core"
    "DepositTransactionDto.cs" = "Core"
    "FixedDepositDto.cs" = "FixedDeposit"
    "InterestTierDto.cs" = "FixedDeposit"
    "InterestCalculationRequestDto.cs" = "Interest"
    "InterestCalculationResultDto.cs" = "Interest"
    "ApplyInterestRequestDto.cs" = "Interest"
    "UpdateInterestRateRequestDto.cs" = "Interest"
    "InterestRateInfoDto.cs" = "Interest"
    "MonthlyInterestProcessingSummaryDto.cs" = "Interest"
    "MaturityDto.cs" = "Maturity"
    "MaturityNoticeDto.cs" = "Maturity"
    "WithdrawalDto.cs" = "Withdrawal"
    "DepositCertificateDto.cs" = "Withdrawal"
}

foreach ($file in $depositMappings.Keys) {
    $subfolder = $depositMappings[$file]
    $source = "$dtoBasePath/Deposit/$file"
    $dest = "$dtoBasePath/Deposit/$subfolder/$file"
    
    if (Test-Path $source) {
        $content = Get-Content $source -Raw -Encoding UTF8
        $oldNs = "namespace Bank.Application.DTOs.Deposit;"
        $newNs = "namespace Bank.Application.DTOs.Deposit.$subfolder;"
        $newContent = $content -replace [regex]::Escape($oldNs), $newNs
        
        $destDir = Split-Path $dest
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        
        Set-Content -Path $dest -Value $newContent -Encoding UTF8 -Force
        Remove-Item $source -Force
        Write-Host "  [OK] $file -> $subfolder/" -ForegroundColor Green
        $movedCount++
    }
}

# ============================================================================
# LOAN DTOs - Move all files
# ============================================================================
Write-Host "`n=== LOAN DTOs ===" -ForegroundColor Cyan

$loanMappings = @{
    "LoanDetailsDto.cs" = "Core"
    "LoanApplicationRequestDto.cs" = "Core"
    "LoanApplicationResultDto.cs" = "Core"
    "CreditScoreDto.cs" = "Application"
    "CreditScoreResultDto.cs" = "Application"
    "ApprovalDecisionDto.cs" = "Approval"
    "LoanApprovalResultDto.cs" = "Approval"
    "DisbursementResultDto.cs" = "Disbursement"
    "PaymentResultDto.cs" = "Disbursement"
    "RepaymentScheduleDto.cs" = "Repayment"
    "RepaymentScheduleEntryDto.cs" = "Repayment"
    "LoanPaymentRequestDto.cs" = "Repayment"
    "AmortizationScheduleDto.cs" = "Repayment"
    "AmortizationEntryDto.cs" = "Repayment"
    "LoanAnalyticsDtoFile.cs" = "Analytics"
    "LoanPerformanceMetricsDto.cs" = "Analytics"
    "LoanRiskLevelEnum.cs" = "Analytics"
    "PortfolioRiskMetricsDto.cs" = "Analytics"
    "LoanOriginationTrendDto.cs" = "Analytics"
    "CustomerLoanSummaryDto.cs" = "Analytics"
    "LoanTypeConfigurationDto.cs" = "Configuration"
    "LoanSearchRequestDto.cs" = "Configuration"
    "LoanInterestCalculationResultDto.cs" = "Configuration"
    "EarlyPayoffCalculationDto.cs" = "Configuration"
    "MonthlyPaymentRequestDto.cs" = "Configuration"
    "EarlyPayoffRequestDto.cs" = "Configuration"
    "InterestRateRequestDto.cs" = "Configuration"
    "UpdateLoanRateRequestDto.cs" = "Configuration"
}

foreach ($file in $loanMappings.Keys) {
    $subfolder = $loanMappings[$file]
    $source = "$dtoBasePath/Loan/$file"
    $dest = "$dtoBasePath/Loan/$subfolder/$file"
    
    if (Test-Path $source) {
        $content = Get-Content $source -Raw -Encoding UTF8
        $oldNs = "namespace Bank.Application.DTOs.Loan;"
        $newNs = "namespace Bank.Application.DTOs.Loan.$subfolder;"
        $newContent = $content -replace [regex]::Escape($oldNs), $newNs
        
        $destDir = Split-Path $dest
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        
        Set-Content -Path $dest -Value $newContent -Encoding UTF8 -Force
        Remove-Item $source -Force
        Write-Host "  [OK] $file -> $subfolder/" -ForegroundColor Green
        $movedCount++
    }
}

# ============================================================================
# PAYMENT DTOs - Move all files
# ============================================================================
Write-Host "`n=== PAYMENT DTOs ===" -ForegroundColor Cyan

$paymentMappings = @{
    "BillerPaymentRequestDto.cs" = "Core"
    "BillerPaymentResponseDto.cs" = "Core"
    "AddBeneficiaryRequestDto.cs" = "Beneficiary"
    "UpdateBeneficiaryRequestDto.cs" = "Beneficiary"
    "BeneficiaryDetailsDto.cs" = "Beneficiary"
    "BeneficiaryResultDto.cs" = "Beneficiary"
    "BeneficiaryVerificationResultDto.cs" = "Beneficiary"
    "VerifyBeneficiaryRequestDto.cs" = "Beneficiary"
    "BeneficiarySearchCriteriaDto.cs" = "Beneficiary"
    "BeneficiarySearchResultDto.cs" = "Beneficiary"
    "BeneficiaryTransferHistoryDto.cs" = "Beneficiary"
    "TransferHistoryItemDto.cs" = "Beneficiary"
    "BeneficiaryStatisticsDto.cs" = "Beneficiary"
    "UpdateTransferLimitsRequestDto.cs" = "Beneficiary"
    "BillerPaymentStatusResponseDto.cs" = "Biller"
    "BillerHealthCheckResponseDto.cs" = "Biller"
    "BillerAccountValidationResponseDto.cs" = "Biller"
    "BillPresentmentDto.cs" = "Biller"
    "BillLineItemDto.cs" = "Biller"
    "BillPaymentDTOs.cs" = "Biller"
    "BatchPaymentResponseDto.cs" = "Batch"
    "BatchPaymentResultDto.cs" = "Batch"
    "PaymentRoutingPreferencesDto.cs" = "Routing"
    "PaymentStatusSyncResultDto.cs" = "Routing"
    "PaymentReceiptDto.cs" = "Receipt"
    "PaymentRetryRequestDto.cs" = "Receipt"
    "PaymentRetryResultDto.cs" = "Receipt"
    "CreateRecurringPaymentRequestDto.cs" = "Recurring"
    "UpdateRecurringPaymentRequestDto.cs" = "Recurring"
    "BulkTransferRequestDto.cs" = "Recurring"
    "BulkTransferItemDto.cs" = "Recurring"
    "BulkTransferResultDto.cs" = "Recurring"
    "BulkTransferItemResultDto.cs" = "Recurring"
    "PaymentTemplateDTOs.cs" = "Template"
}

foreach ($file in $paymentMappings.Keys) {
    $subfolder = $paymentMappings[$file]
    $source = "$dtoBasePath/Payment/$file"
    $dest = "$dtoBasePath/Payment/$subfolder/$file"
    
    if (Test-Path $source) {
        $content = Get-Content $source -Raw -Encoding UTF8
        $oldNs = "namespace Bank.Application.DTOs.Payment;"
        $newNs = "namespace Bank.Application.DTOs.Payment.$subfolder;"
        $newContent = $content -replace [regex]::Escape($oldNs), $newNs
        
        $destDir = Split-Path $dest
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        
        Set-Content -Path $dest -Value $newContent -Encoding UTF8 -Force
        Remove-Item $source -Force
        Write-Host "  [OK] $file -> $subfolder/" -ForegroundColor Green
        $movedCount++
    }
}

# ============================================================================
# STATEMENT DTOs - Move all files
# ============================================================================
Write-Host "`n=== STATEMENT DTOs ===" -ForegroundColor Cyan

$statementMappings = @{
    "StatementDetailsDto.cs" = "Core"
    "GenerateStatementRequestDto.cs" = "Core"
    "StatementGenerationResultDto.cs" = "Core"
    "StatementSearchCriteriaDto.cs" = "Search"
    "StatementSearchResultDto.cs" = "Search"
    "StatementSummaryDto.cs" = "Summary"
    "TransactionCategorySummaryDto.cs" = "Summary"
    "MonthlyTransactionSummaryDto.cs" = "Summary"
    "ConsolidatedStatementRequestDto.cs" = "Delivery"
    "StatementDeliveryStatusDto.cs" = "Delivery"
    "DeliverStatementRequestDto.cs" = "Delivery"
    "StatementStatisticsDto.cs" = "Analytics"
    "StatementTemplateDto.cs" = "Analytics"
    "StatementTransactionDto.cs" = "Transaction"
    "ValidationResultDto.cs" = "Transaction"
}

foreach ($file in $statementMappings.Keys) {
    $subfolder = $statementMappings[$file]
    $source = "$dtoBasePath/Statement/$file"
    $dest = "$dtoBasePath/Statement/$subfolder/$file"
    
    if (Test-Path $source) {
        $content = Get-Content $source -Raw -Encoding UTF8
        $oldNs = "namespace Bank.Application.DTOs.Statement;"
        $newNs = "namespace Bank.Application.DTOs.Statement.$subfolder;"
        $newContent = $content -replace [regex]::Escape($oldNs), $newNs
        
        $destDir = Split-Path $dest
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        
        Set-Content -Path $dest -Value $newContent -Encoding UTF8 -Force
        Remove-Item $source -Force
        Write-Host "  [OK] $file -> $subfolder/" -ForegroundColor Green
        $movedCount++
    }
}

# ============================================================================
# TRANSACTION DTOs - Move all files
# ============================================================================
Write-Host "`n=== TRANSACTION DTOs ===" -ForegroundColor Cyan

$transactionMappings = @{
    "TransactionDTOs.cs" = "Core"
    "CreateTransactionRequest.cs" = "Core"
    "TransactionSearchCriteria.cs" = "Search"
    "TransactionSearchRequest.cs" = "Search"
    "TransactionStatistics.cs" = "Analytics"
    "FraudAnalysisResult.cs" = "Fraud"
    "FraudRiskScore.cs" = "Fraud"
    "FraudRule.cs" = "Fraud"
    "SuspiciousActivityReport.cs" = "Fraud"
}

foreach ($file in $transactionMappings.Keys) {
    $subfolder = $transactionMappings[$file]
    $source = "$dtoBasePath/Transaction/$file"
    $dest = "$dtoBasePath/Transaction/$subfolder/$file"
    
    if (Test-Path $source) {
        $content = Get-Content $source -Raw -Encoding UTF8
        $oldNs = "namespace Bank.Application.DTOs.Transaction;"
        $newNs = "namespace Bank.Application.DTOs.Transaction.$subfolder;"
        $newContent = $content -replace [regex]::Escape($oldNs), $newNs
        
        $destDir = Split-Path $dest
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        
        Set-Content -Path $dest -Value $newContent -Encoding UTF8 -Force
        Remove-Item $source -Force
        Write-Host "  [OK] $file -> $subfolder/" -ForegroundColor Green
        $movedCount++
    }
}

# ============================================================================
# SHARED DTOs - Move all files
# ============================================================================
Write-Host "`n=== SHARED DTOs ===" -ForegroundColor Cyan

$sharedMappings = @{
    "NotificationDetailsDto.cs" = "Notification"
    "SendNotificationRequestDto.cs" = "Notification"
    "TransactionAlertRequestDto.cs" = "Notification"
    "SecurityAlertRequestDto.cs" = "Notification"
    "NotificationPreferencesRequestDto.cs" = "Notification"
    "NotificationResponseDto.cs" = "Notification"
    "NotificationHistoryItemDto.cs" = "Notification"
    "BulkNotificationRequestDto.cs" = "Notification"
    "AuditDTOs.cs" = "Audit"
    "RateLimitPolicy.cs" = "RateLimit"
    "RateLimitResult.cs" = "RateLimit"
    "RateLimitStatus.cs" = "RateLimit"
}

foreach ($file in $sharedMappings.Keys) {
    $subfolder = $sharedMappings[$file]
    $source = "$dtoBasePath/Shared/$file"
    $dest = "$dtoBasePath/Shared/$subfolder/$file"
    
    if (Test-Path $source) {
        $content = Get-Content $source -Raw -Encoding UTF8
        $oldNs = "namespace Bank.Application.DTOs.Shared;"
        $newNs = "namespace Bank.Application.DTOs.Shared.$subfolder;"
        $newContent = $content -replace [regex]::Escape($oldNs), $newNs
        
        $destDir = Split-Path $dest
        if (-not (Test-Path $destDir)) {
            New-Item -ItemType Directory -Path $destDir -Force | Out-Null
        }
        
        Set-Content -Path $dest -Value $newContent -Encoding UTF8 -Force
        Remove-Item $source -Force
        Write-Host "  [OK] $file -> $subfolder/" -ForegroundColor Green
        $movedCount++
    }
}

# ============================================================================
# Summary
# ============================================================================
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "MIGRATION COMPLETE" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Total Files Moved: $movedCount" -ForegroundColor Green
Write-Host ""
