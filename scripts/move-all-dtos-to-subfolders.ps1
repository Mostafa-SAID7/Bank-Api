#!/usr/bin/env pwsh
# Move all DTO files to their appropriate subfolders and update namespaces

$dtoBasePath = "Bank-Api/src/Bank.Application/DTOs"
$movedCount = 0
$updatedCount = 0

function Move-DtoFile {
    param(
        [string]$SourcePath,
        [string]$DestPath,
        [string]$Domain,
        [string]$Subfolder
    )
    
    if (Test-Path $SourcePath) {
        $content = Get-Content $SourcePath -Raw
        
        # Update namespace
        $oldNs = "namespace Bank.Application.DTOs.$Domain;"
        $newNs = "namespace Bank.Application.DTOs.$Domain.$Subfolder;"
        $newContent = $content -replace [regex]::Escape($oldNs), $newNs
        
        # Write to destination
        Set-Content -Path $DestPath -Value $newContent -Encoding UTF8
        Remove-Item $SourcePath -Force
        
        Write-Host "  ✓ $([System.IO.Path]::GetFileName($SourcePath)) -> $Subfolder/" -ForegroundColor Green
        return $true
    }
    return $false
}

# Auth DTOs
Write-Host "Moving Auth DTOs..." -ForegroundColor Cyan
$authPath = "$dtoBasePath/Auth"
$authFiles = @{
    "Core" = @("LoginRequestDto.cs", "RegisterRequestDto.cs", "AuthResponseDto.cs")
    "TwoFactor" = @("TwoFactorTokenResultDto.cs", "TwoFactorVerificationResultDto.cs", "TwoFactorSetupResultDto.cs", "TwoFactorStatusResultDto.cs", "GenerateTokenRequestDto.cs", "VerifyTokenRequestDto.cs", "VerifyBackupCodeRequestDto.cs", "CompleteSetupRequestDto.cs")
    "Security" = @("IpWhitelistResultDto.cs", "IpWhitelistStatisticsDto.cs", "IpWhitelistInfoDto.cs", "PasswordValidationResultDto.cs", "PasswordPolicyInfoDto.cs", "PasswordChangeRequirementDto.cs")
    "Session" = @("SessionResultDto.cs", "SessionStatisticsDto.cs", "SessionInfoDto.cs", "RefreshTokenRequestDto.cs")
}

foreach ($subfolder in $authFiles.Keys) {
    foreach ($file in $authFiles[$subfolder]) {
        if (Move-DtoFile "$authPath/$file" "$authPath/$subfolder/$file" "Auth" $subfolder) {
            $movedCount++
        }
    }
}

# Card DTOs
Write-Host "Moving Card DTOs..." -ForegroundColor Cyan
$cardPath = "$dtoBasePath/Card"
$cardFiles = @{
    "Core" = @("CardDetailsDto.cs", "CardDto.cs", "CardSummaryDto.cs")
    "Activation" = @("CardActivationDto.cs", "CardIssuanceDto.cs", "CardRenewalDto.cs")
    "Transactions" = @("CardTransactionDto.cs", "CardTransactionSearchDto.cs", "CardTransactionProcessingDto.cs", "CardStatementDto.cs")
    "Fees" = @("CardTransactionFeesDto.cs", "CardLimitDto.cs")
    "Operations" = @("CardBlockDto.cs", "CardPinDto.cs", "CardValidationDto.cs", "CardVoidDto.cs", "CardRefundDto.cs", "CardCaptureDto.cs", "CardAuthorizationDto.cs")
    "Advanced" = @("CardContactlessDto.cs", "CardOnlineTransactionsDto.cs", "CardInternationalTransactionsDto.cs", "CardMerchantRestrictionsDto.cs", "CardSettlementDto.cs", "CardNetworkStatusDto.cs", "CardUsageStatsDto.cs", "PinManagementDTOs.cs", "PagedRequestDto.cs")
}

foreach ($subfolder in $cardFiles.Keys) {
    foreach ($file in $cardFiles[$subfolder]) {
        if (Move-DtoFile "$cardPath/$file" "$cardPath/$subfolder/$file" "Card" $subfolder) {
            $movedCount++
        }
    }
}

# Deposit DTOs
Write-Host "Moving Deposit DTOs..." -ForegroundColor Cyan
$depositPath = "$dtoBasePath/Deposit"
$depositFiles = @{
    "Core" = @("DepositProductDto.cs", "DepositSummaryDto.cs", "DepositTransactionDto.cs")
    "FixedDeposit" = @("FixedDepositDto.cs", "InterestTierDto.cs")
    "Interest" = @("InterestCalculationRequestDto.cs", "InterestCalculationResultDto.cs", "ApplyInterestRequestDto.cs", "UpdateInterestRateRequestDto.cs", "InterestRateInfoDto.cs", "MonthlyInterestProcessingSummaryDto.cs")
    "Maturity" = @("MaturityDto.cs", "MaturityNoticeDto.cs")
    "Withdrawal" = @("WithdrawalDto.cs", "DepositCertificateDto.cs")
}

foreach ($subfolder in $depositFiles.Keys) {
    foreach ($file in $depositFiles[$subfolder]) {
        if (Move-DtoFile "$depositPath/$file" "$depositPath/$subfolder/$file" "Deposit" $subfolder) {
            $movedCount++
        }
    }
}

# Loan DTOs
Write-Host "Moving Loan DTOs..." -ForegroundColor Cyan
$loanPath = "$dtoBasePath/Loan"
$loanFiles = @{
    "Core" = @("LoanDetailsDto.cs", "LoanApplicationRequestDto.cs", "LoanApplicationResultDto.cs")
    "Application" = @("CreditScoreDto.cs", "CreditScoreResultDto.cs")
    "Approval" = @("ApprovalDecisionDto.cs", "LoanApprovalResultDto.cs")
    "Disbursement" = @("DisbursementResultDto.cs", "PaymentResultDto.cs")
    "Repayment" = @("RepaymentScheduleDto.cs", "RepaymentScheduleEntryDto.cs", "LoanPaymentRequestDto.cs", "AmortizationScheduleDto.cs", "AmortizationEntryDto.cs")
    "Analytics" = @("LoanAnalyticsDtoFile.cs", "LoanPerformanceMetricsDto.cs", "LoanRiskLevelEnum.cs", "PortfolioRiskMetricsDto.cs", "LoanOriginationTrendDto.cs", "CustomerLoanSummaryDto.cs")
    "Configuration" = @("LoanTypeConfigurationDto.cs", "LoanSearchRequestDto.cs", "LoanInterestCalculationResultDto.cs", "EarlyPayoffCalculationDto.cs", "MonthlyPaymentRequestDto.cs", "EarlyPayoffRequestDto.cs", "InterestRateRequestDto.cs", "UpdateLoanRateRequestDto.cs")
}

foreach ($subfolder in $loanFiles.Keys) {
    foreach ($file in $loanFiles[$subfolder]) {
        if (Move-DtoFile "$loanPath/$file" "$loanPath/$subfolder/$file" "Loan" $subfolder) {
            $movedCount++
        }
    }
}

# Payment DTOs
Write-Host "Moving Payment DTOs..." -ForegroundColor Cyan
$paymentPath = "$dtoBasePath/Payment"
$paymentFiles = @{
    "Core" = @("BillerPaymentRequestDto.cs", "BillerPaymentResponseDto.cs")
    "Beneficiary" = @("AddBeneficiaryRequestDto.cs", "UpdateBeneficiaryRequestDto.cs", "BeneficiaryDetailsDto.cs", "BeneficiaryResultDto.cs", "BeneficiaryVerificationResultDto.cs", "VerifyBeneficiaryRequestDto.cs", "BeneficiarySearchCriteriaDto.cs", "BeneficiarySearchResultDto.cs", "BeneficiaryTransferHistoryDto.cs", "TransferHistoryItemDto.cs", "BeneficiaryStatisticsDto.cs", "UpdateTransferLimitsRequestDto.cs")
    "Biller" = @("BillerPaymentStatusResponseDto.cs", "BillerHealthCheckResponseDto.cs", "BillerAccountValidationResponseDto.cs", "BillPresentmentDto.cs", "BillLineItemDto.cs", "BillPaymentDTOs.cs")
    "Batch" = @("BatchPaymentResponseDto.cs", "BatchPaymentResultDto.cs")
    "Routing" = @("PaymentRoutingPreferencesDto.cs", "PaymentStatusSyncResultDto.cs")
    "Receipt" = @("PaymentReceiptDto.cs", "PaymentRetryRequestDto.cs", "PaymentRetryResultDto.cs")
    "Recurring" = @("CreateRecurringPaymentRequestDto.cs", "UpdateRecurringPaymentRequestDto.cs", "BulkTransferRequestDto.cs", "BulkTransferItemDto.cs", "BulkTransferResultDto.cs", "BulkTransferItemResultDto.cs")
    "Template" = @("PaymentTemplateDTOs.cs")
}

foreach ($subfolder in $paymentFiles.Keys) {
    foreach ($file in $paymentFiles[$subfolder]) {
        if (Move-DtoFile "$paymentPath/$file" "$paymentPath/$subfolder/$file" "Payment" $subfolder) {
            $movedCount++
        }
    }
}

# Statement DTOs
Write-Host "Moving Statement DTOs..." -ForegroundColor Cyan
$statementPath = "$dtoBasePath/Statement"
$statementFiles = @{
    "Core" = @("StatementDetailsDto.cs", "GenerateStatementRequestDto.cs", "StatementGenerationResultDto.cs")
    "Search" = @("StatementSearchCriteriaDto.cs", "StatementSearchResultDto.cs")
    "Summary" = @("StatementSummaryDto.cs", "TransactionCategorySummaryDto.cs", "MonthlyTransactionSummaryDto.cs")
    "Delivery" = @("ConsolidatedStatementRequestDto.cs", "StatementDeliveryStatusDto.cs", "DeliverStatementRequestDto.cs")
    "Analytics" = @("StatementStatisticsDto.cs", "StatementTemplateDto.cs")
    "Transaction" = @("StatementTransactionDto.cs", "ValidationResultDto.cs")
}

foreach ($subfolder in $statementFiles.Keys) {
    foreach ($file in $statementFiles[$subfolder]) {
        if (Move-DtoFile "$statementPath/$file" "$statementPath/$subfolder/$file" "Statement" $subfolder) {
            $movedCount++
        }
    }
}

# Transaction DTOs
Write-Host "Moving Transaction DTOs..." -ForegroundColor Cyan
$transactionPath = "$dtoBasePath/Transaction"
$transactionFiles = @{
    "Core" = @("TransactionDTOs.cs", "CreateTransactionRequest.cs")
    "Search" = @("TransactionSearchCriteria.cs", "TransactionSearchRequest.cs")
    "Analytics" = @("TransactionStatistics.cs")
    "Fraud" = @("FraudAnalysisResult.cs", "FraudRiskScore.cs", "FraudRule.cs", "SuspiciousActivityReport.cs")
}

foreach ($subfolder in $transactionFiles.Keys) {
    foreach ($file in $transactionFiles[$subfolder]) {
        if (Move-DtoFile "$transactionPath/$file" "$transactionPath/$subfolder/$file" "Transaction" $subfolder) {
            $movedCount++
        }
    }
}

# Shared DTOs
Write-Host "Moving Shared DTOs..." -ForegroundColor Cyan
$sharedPath = "$dtoBasePath/Shared"
$sharedFiles = @{
    "Notification" = @("NotificationDetailsDto.cs", "SendNotificationRequestDto.cs", "TransactionAlertRequestDto.cs", "SecurityAlertRequestDto.cs", "NotificationPreferencesRequestDto.cs", "NotificationResponseDto.cs", "NotificationHistoryItemDto.cs", "BulkNotificationRequestDto.cs")
    "Audit" = @("AuditDTOs.cs")
    "RateLimit" = @("RateLimitPolicy.cs", "RateLimitResult.cs", "RateLimitStatus.cs")
}

foreach ($subfolder in $sharedFiles.Keys) {
    foreach ($file in $sharedFiles[$subfolder]) {
        if (Move-DtoFile "$sharedPath/$file" "$sharedPath/$subfolder/$file" "Shared" $subfolder) {
            $movedCount++
        }
    }
}

Write-Host ""
Write-Host "Summary:" -ForegroundColor Cyan
Write-Host "  Total Files Moved: $movedCount" -ForegroundColor Green
Write-Host "  Namespaces Updated: $movedCount" -ForegroundColor Green
Write-Host ""
Write-Host "All DTOs organized!" -ForegroundColor Green
