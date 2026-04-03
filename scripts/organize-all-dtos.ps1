#!/usr/bin/env pwsh
<#
.SYNOPSIS
    Organizes ALL domain DTOs into logical subfolders with updated namespaces
.DESCRIPTION
    This script organizes DTOs for all domains following the Account pattern
#>

param(
    [switch]$WhatIf = $false
)

$ErrorActionPreference = "Stop"
$dtoBasePath = "Bank-Api/src/Bank.Application/DTOs"

# Define organization structure for each domain
$domainStructure = @{
    "Auth" = @{
        "Core" = @("LoginRequestDto.cs", "RegisterRequestDto.cs", "AuthResponseDto.cs")
        "TwoFactor" = @("TwoFactorTokenResultDto.cs", "TwoFactorVerificationResultDto.cs", "TwoFactorSetupResultDto.cs", "TwoFactorStatusResultDto.cs", "GenerateTokenRequestDto.cs", "VerifyTokenRequestDto.cs", "VerifyBackupCodeRequestDto.cs", "CompleteSetupRequestDto.cs")
        "Security" = @("IpWhitelistResultDto.cs", "IpWhitelistStatisticsDto.cs", "IpWhitelistInfoDto.cs", "PasswordValidationResultDto.cs", "PasswordPolicyInfoDto.cs", "PasswordChangeRequirementDto.cs")
        "Session" = @("SessionResultDto.cs", "SessionStatisticsDto.cs", "SessionInfoDto.cs", "RefreshTokenRequestDto.cs")
    }
    
    "Card" = @{
        "Core" = @("CardDetailsDto.cs", "CardDto.cs", "CardSummaryDto.cs")
        "Activation" = @("CardActivationDto.cs", "CardIssuanceDto.cs", "CardRenewalDto.cs")
        "Transactions" = @("CardTransactionDto.cs", "CardTransactionSearchDto.cs", "CardTransactionProcessingDto.cs", "CardStatementDto.cs")
        "Fees" = @("CardTransactionFeesDto.cs", "CardLimitDto.cs")
        "Operations" = @("CardBlockDto.cs", "CardPinDto.cs", "CardValidationDto.cs", "CardVoidDto.cs", "CardRefundDto.cs", "CardCaptureDto.cs", "CardAuthorizationDto.cs")
        "Advanced" = @("CardContactlessDto.cs", "CardOnlineTransactionsDto.cs", "CardInternationalTransactionsDto.cs", "CardMerchantRestrictionsDto.cs", "CardSettlementDto.cs", "CardNetworkStatusDto.cs", "CardUsageStatsDto.cs", "PinManagementDTOs.cs", "PagedRequestDto.cs")
    }
    
    "Deposit" = @{
        "Core" = @("DepositProductDto.cs", "DepositSummaryDto.cs", "DepositTransactionDto.cs")
        "FixedDeposit" = @("FixedDepositDto.cs", "InterestTierDto.cs")
        "Interest" = @("InterestCalculationRequestDto.cs", "InterestCalculationResultDto.cs", "ApplyInterestRequestDto.cs", "UpdateInterestRateRequestDto.cs", "InterestRateInfoDto.cs", "MonthlyInterestProcessingSummaryDto.cs")
        "Maturity" = @("MaturityDto.cs", "MaturityNoticeDto.cs")
        "Withdrawal" = @("WithdrawalDto.cs", "DepositCertificateDto.cs")
    }
    
    "Loan" = @{
        "Core" = @("LoanDetailsDto.cs", "LoanApplicationRequestDto.cs", "LoanApplicationResultDto.cs")
        "Application" = @("CreditScoreDto.cs", "CreditScoreResultDto.cs", "LoanApplicationRequestValidator.cs")
        "Approval" = @("ApprovalDecisionDto.cs", "LoanApprovalResultDto.cs")
        "Disbursement" = @("DisbursementResultDto.cs", "PaymentResultDto.cs")
        "Repayment" = @("RepaymentScheduleDto.cs", "RepaymentScheduleEntryDto.cs", "LoanPaymentRequestDto.cs", "AmortizationScheduleDto.cs", "AmortizationEntryDto.cs")
        "Analytics" = @("LoanAnalyticsDtoFile.cs", "LoanPerformanceMetricsDto.cs", "LoanRiskLevelEnum.cs", "PortfolioRiskMetricsDto.cs", "LoanOriginationTrendDto.cs", "CustomerLoanSummaryDto.cs")
        "Configuration" = @("LoanTypeConfigurationDto.cs", "LoanSearchRequestDto.cs", "LoanInterestCalculationResultDto.cs", "EarlyPayoffCalculationDto.cs", "MonthlyPaymentRequestDto.cs", "EarlyPayoffRequestDto.cs", "InterestRateRequestDto.cs", "UpdateLoanRateRequestDto.cs")
    }
    
    "Payment" = @{
        "Core" = @("BillerPaymentRequestDto.cs", "BillerPaymentResponseDto.cs")
        "Beneficiary" = @("AddBeneficiaryRequestDto.cs", "UpdateBeneficiaryRequestDto.cs", "BeneficiaryDetailsDto.cs", "BeneficiaryResultDto.cs", "BeneficiaryVerificationResultDto.cs", "VerifyBeneficiaryRequestDto.cs", "BeneficiarySearchCriteriaDto.cs", "BeneficiarySearchResultDto.cs", "BeneficiaryTransferHistoryDto.cs", "TransferHistoryItemDto.cs", "BeneficiaryStatisticsDto.cs", "UpdateTransferLimitsRequestDto.cs")
        "Biller" = @("BillerPaymentStatusResponseDto.cs", "BillerHealthCheckResponseDto.cs", "BillerAccountValidationResponseDto.cs", "BillPresentmentDto.cs", "BillLineItemDto.cs", "BillPaymentDTOs.cs")
        "Batch" = @("BatchPaymentResponseDto.cs", "BatchPaymentResultDto.cs")
        "Routing" = @("PaymentRoutingPreferencesDto.cs", "PaymentStatusSyncResultDto.cs")
        "Receipt" = @("PaymentReceiptDto.cs", "PaymentRetryRequestDto.cs", "PaymentRetryResultDto.cs")
        "Recurring" = @("CreateRecurringPaymentRequestDto.cs", "UpdateRecurringPaymentRequestDto.cs", "BulkTransferRequestDto.cs", "BulkTransferItemDto.cs", "BulkTransferResultDto.cs", "BulkTransferItemResultDto.cs")
        "Template" = @("PaymentTemplateDTOs.cs")
    }
    
    "Statement" = @{
        "Core" = @("StatementDetailsDto.cs", "GenerateStatementRequestDto.cs", "StatementGenerationResultDto.cs")
        "Search" = @("StatementSearchCriteriaDto.cs", "StatementSearchResultDto.cs")
        "Summary" = @("StatementSummaryDto.cs", "TransactionCategorySummaryDto.cs", "MonthlyTransactionSummaryDto.cs")
        "Delivery" = @("ConsolidatedStatementRequestDto.cs", "StatementDeliveryStatusDto.cs", "DeliverStatementRequestDto.cs")
        "Analytics" = @("StatementStatisticsDto.cs", "StatementTemplateDto.cs")
        "Transaction" = @("StatementTransactionDto.cs", "ValidationResultDto.cs")
    }
    
    "Transaction" = @{
        "Core" = @("TransactionDTOs.cs", "CreateTransactionRequest.cs")
        "Search" = @("TransactionSearchCriteria.cs", "TransactionSearchRequest.cs")
        "Analytics" = @("TransactionStatistics.cs")
        "Fraud" = @("FraudAnalysisResult.cs", "FraudRiskScore.cs", "FraudRule.cs", "SuspiciousActivityReport.cs")
    }
    
    "Shared" = @{
        "Notification" = @("NotificationDetailsDto.cs", "SendNotificationRequestDto.cs", "TransactionAlertRequestDto.cs", "SecurityAlertRequestDto.cs", "NotificationPreferencesRequestDto.cs", "NotificationResponseDto.cs", "NotificationHistoryItemDto.cs", "BulkNotificationRequestDto.cs")
        "Audit" = @("AuditDTOs.cs")
        "RateLimit" = @("RateLimitPolicy.cs", "RateLimitResult.cs", "RateLimitStatus.cs")
    }
}

Write-Host "Starting All DTOs Organization..." -ForegroundColor Cyan
Write-Host ""

foreach ($domain in $domainStructure.Keys) {
    Write-Host "Processing Domain: $domain" -ForegroundColor Yellow
    $domainPath = "$dtoBasePath/$domain"
    
    if (-not (Test-Path $domainPath)) {
        Write-Host "  ✗ Domain folder not found: $domainPath" -ForegroundColor Red
        continue
    }
    
    # Create subfolders
    foreach ($subfolder in $domainStructure[$domain].Keys) {
        $subfolderPath = "$domainPath/$subfolder"
        if (-not (Test-Path $subfolderPath)) {
            New-Item -ItemType Directory -Path $subfolderPath -Force | Out-Null
            Write-Host "  ✓ Created subfolder: $subfolder" -ForegroundColor Green
        }
    }
    
    # Move files to appropriate subfolders
    foreach ($subfolder in $domainStructure[$domain].Keys) {
        $files = $domainStructure[$domain][$subfolder]
        $subfolderPath = "$domainPath/$subfolder"
        
        foreach ($file in $files) {
            $sourcePath = "$domainPath/$file"
            $destPath = "$subfolderPath/$file"
            
            if (Test-Path $sourcePath) {
                # Read and update namespace
                $content = Get-Content $sourcePath -Raw
                $oldNs = "namespace Bank.Application.DTOs.$domain;"
                $newNs = "namespace Bank.Application.DTOs.$domain.$subfolder;"
                $newContent = $content -replace [regex]::Escape($oldNs), $newNs
                
                if (-not $WhatIf) {
                    Set-Content -Path $destPath -Value $newContent -Encoding UTF8
                    Remove-Item $sourcePath -Force
                    Write-Host "    - Moved $file to $subfolder/" -ForegroundColor Green
                } else {
                    Write-Host "    - Would move $file to $subfolder/" -ForegroundColor Cyan
                }
            }
        }
    }
    
    Write-Host ""
}

Write-Host "All DTOs organized successfully!" -ForegroundColor Cyan
