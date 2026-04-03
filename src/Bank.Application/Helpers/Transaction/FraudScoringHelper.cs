using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Transaction;

/// <summary>
/// Helper for fraud detection and risk scoring
/// </summary>
public static class FraudScoringHelper
{
    /// <summary>
    /// Calculates fraud risk score (0-100)
    /// </summary>
    /// <param name="amount">Transaction amount</param>
    /// <param name="averageAmount">Average transaction amount for account</param>
    /// <param name="isUnusualLocation">Whether transaction is from unusual location</param>
    /// <param name="isUnusualTime">Whether transaction is at unusual time</param>
    /// <param name="isNewMerchant">Whether merchant is new to account</param>
    /// <returns>Risk score (0-100)</returns>
    public static int CalculateFraudRiskScore(
        decimal amount,
        decimal averageAmount,
        bool isUnusualLocation = false,
        bool isUnusualTime = false,
        bool isNewMerchant = false)
    {
        int score = 0;

        // Amount anomaly (0-30 points)
        if (averageAmount > 0)
        {
            var amountRatio = amount / averageAmount;
            if (amountRatio > 5)
                score += 30;
            else if (amountRatio > 3)
                score += 20;
            else if (amountRatio > 2)
                score += 10;
        }

        // Location anomaly (0-25 points)
        if (isUnusualLocation)
            score += 25;

        // Time anomaly (0-20 points)
        if (isUnusualTime)
            score += 20;

        // New merchant (0-25 points)
        if (isNewMerchant)
            score += 25;

        return Math.Min(score, 100);
    }

    /// <summary>
    /// Determines fraud risk level based on score
    /// </summary>
    /// <param name="riskScore">Risk score (0-100)</param>
    /// <returns>Risk level</returns>
    public static FraudRiskLevel GetRiskLevel(int riskScore)
    {
        return riskScore switch
        {
            < 20 => FraudRiskLevel.Low,
            < 50 => FraudRiskLevel.Medium,
            < 75 => FraudRiskLevel.High,
            _ => FraudRiskLevel.Critical
        };
    }

    /// <summary>
    /// Checks if transaction should be flagged for review
    /// </summary>
    /// <param name="riskScore">Risk score (0-100)</param>
    /// <param name="threshold">Threshold for flagging (default: 50)</param>
    /// <returns>True if should be flagged</returns>
    public static bool ShouldFlagForReview(int riskScore, int threshold = 50)
    {
        return riskScore >= threshold;
    }

    /// <summary>
    /// Checks if transaction should be blocked
    /// </summary>
    /// <param name="riskScore">Risk score (0-100)</param>
    /// <param name="blockThreshold">Threshold for blocking (default: 80)</param>
    /// <returns>True if should be blocked</returns>
    public static bool ShouldBlock(int riskScore, int blockThreshold = 80)
    {
        return riskScore >= blockThreshold;
    }

    /// <summary>
    /// Evaluates multiple fraud indicators
    /// </summary>
    /// <param name="indicators">Dictionary of indicator names and their values</param>
    /// <returns>Overall fraud risk score</returns>
    public static int EvaluateFraudIndicators(Dictionary<string, bool> indicators)
    {
        int score = 0;
        int totalIndicators = indicators.Count;

        if (totalIndicators == 0)
            return 0;

        var activeIndicators = indicators.Values.Count(v => v);
        var indicatorPercentage = (decimal)activeIndicators / totalIndicators;

        // Convert percentage to score (0-100)
        score = (int)(indicatorPercentage * 100);

        return Math.Min(score, 100);
    }

    /// <summary>
    /// Gets fraud risk description
    /// </summary>
    /// <param name="riskLevel">Risk level</param>
    /// <returns>Description of risk level</returns>
    public static string GetRiskDescription(FraudRiskLevel riskLevel)
    {
        return riskLevel switch
        {
            FraudRiskLevel.Low => "Low fraud risk - Transaction appears normal",
            FraudRiskLevel.Medium => "Medium fraud risk - Some unusual patterns detected",
            FraudRiskLevel.High => "High fraud risk - Multiple suspicious indicators",
            FraudRiskLevel.Critical => "Critical fraud risk - Transaction should be blocked",
            _ => "Unknown risk level"
        };
    }
}

/// <summary>
/// Fraud risk levels
/// </summary>
public enum FraudRiskLevel
{
    Low = 0,
    Medium = 1,
    High = 2,
    Critical = 3
}
