namespace Bank.Application.Helpers.Account;

/// <summary>
/// Helper for account limit operations
/// </summary>
public static class AccountLimitHelper
{
    /// <summary>
    /// Checks if transaction amount exceeds daily limit
    /// </summary>
    /// <param name="transactionAmount">Transaction amount</param>
    /// <param name="dailyLimit">Daily limit</param>
    /// <param name="dailyUsed">Amount already used today</param>
    /// <returns>True if exceeds limit</returns>
    public static bool ExceedsDailyLimit(decimal transactionAmount, decimal dailyLimit, decimal dailyUsed)
    {
        if (dailyLimit <= 0)
            return false; // No limit

        return (dailyUsed + transactionAmount) > dailyLimit;
    }

    /// <summary>
    /// Checks if transaction amount exceeds monthly limit
    /// </summary>
    /// <param name="transactionAmount">Transaction amount</param>
    /// <param name="monthlyLimit">Monthly limit</param>
    /// <param name="monthlyUsed">Amount already used this month</param>
    /// <returns>True if exceeds limit</returns>
    public static bool ExceedsMonthlyLimit(decimal transactionAmount, decimal monthlyLimit, decimal monthlyUsed)
    {
        if (monthlyLimit <= 0)
            return false; // No limit

        return (monthlyUsed + transactionAmount) > monthlyLimit;
    }

    /// <summary>
    /// Calculates remaining daily limit
    /// </summary>
    /// <param name="dailyLimit">Daily limit</param>
    /// <param name="dailyUsed">Amount already used today</param>
    /// <returns>Remaining amount</returns>
    public static decimal GetRemainingDailyLimit(decimal dailyLimit, decimal dailyUsed)
    {
        if (dailyLimit <= 0)
            return decimal.MaxValue; // No limit

        return Math.Max(0, dailyLimit - dailyUsed);
    }

    /// <summary>
    /// Calculates remaining monthly limit
    /// </summary>
    /// <param name="monthlyLimit">Monthly limit</param>
    /// <param name="monthlyUsed">Amount already used this month</param>
    /// <returns>Remaining amount</returns>
    public static decimal GetRemainingMonthlyLimit(decimal monthlyLimit, decimal monthlyUsed)
    {
        if (monthlyLimit <= 0)
            return decimal.MaxValue; // No limit

        return Math.Max(0, monthlyLimit - monthlyUsed);
    }

    /// <summary>
    /// Calculates percentage of daily limit used
    /// </summary>
    /// <param name="dailyLimit">Daily limit</param>
    /// <param name="dailyUsed">Amount already used today</param>
    /// <returns>Percentage (0-100)</returns>
    public static decimal GetDailyLimitPercentageUsed(decimal dailyLimit, decimal dailyUsed)
    {
        if (dailyLimit <= 0)
            return 0; // No limit

        return (dailyUsed / dailyLimit) * 100;
    }

    /// <summary>
    /// Calculates percentage of monthly limit used
    /// </summary>
    /// <param name="monthlyLimit">Monthly limit</param>
    /// <param name="monthlyUsed">Amount already used this month</param>
    /// <returns>Percentage (0-100)</returns>
    public static decimal GetMonthlyLimitPercentageUsed(decimal monthlyLimit, decimal monthlyUsed)
    {
        if (monthlyLimit <= 0)
            return 0; // No limit

        return (monthlyUsed / monthlyLimit) * 100;
    }

    /// <summary>
    /// Checks if daily limit is nearly exhausted
    /// </summary>
    /// <param name="dailyLimit">Daily limit</param>
    /// <param name="dailyUsed">Amount already used today</param>
    /// <param name="warningThreshold">Warning threshold percentage (default: 80)</param>
    /// <returns>True if nearly exhausted</returns>
    public static bool IsDailyLimitNearlyExhausted(decimal dailyLimit, decimal dailyUsed, decimal warningThreshold = 80)
    {
        if (dailyLimit <= 0)
            return false; // No limit

        var percentageUsed = GetDailyLimitPercentageUsed(dailyLimit, dailyUsed);
        return percentageUsed >= warningThreshold;
    }

    /// <summary>
    /// Checks if monthly limit is nearly exhausted
    /// </summary>
    /// <param name="monthlyLimit">Monthly limit</param>
    /// <param name="monthlyUsed">Amount already used this month</param>
    /// <param name="warningThreshold">Warning threshold percentage (default: 80)</param>
    /// <returns>True if nearly exhausted</returns>
    public static bool IsMonthlyLimitNearlyExhausted(decimal monthlyLimit, decimal monthlyUsed, decimal warningThreshold = 80)
    {
        if (monthlyLimit <= 0)
            return false; // No limit

        var percentageUsed = GetMonthlyLimitPercentageUsed(monthlyLimit, monthlyUsed);
        return percentageUsed >= warningThreshold;
    }

    /// <summary>
    /// Formats limit information for display
    /// </summary>
    /// <param name="limit">Limit amount</param>
    /// <param name="used">Amount used</param>
    /// <returns>Formatted string (e.g., "$500 of $1,000")</returns>
    public static string FormatLimitInfo(decimal limit, decimal used)
    {
        if (limit <= 0)
            return "Unlimited";

        return $"${used:N2} of ${limit:N2}";
    }

    /// <summary>
    /// Gets limit status message
    /// </summary>
    /// <param name="limit">Limit amount</param>
    /// <param name="used">Amount used</param>
    /// <param name="limitName">Name of limit (e.g., "Daily", "Monthly")</param>
    /// <returns>Status message</returns>
    public static string GetLimitStatusMessage(decimal limit, decimal used, string limitName = "Limit")
    {
        if (limit <= 0)
            return $"{limitName}: Unlimited";

        var remaining = GetRemainingDailyLimit(limit, used);
        var percentage = GetDailyLimitPercentageUsed(limit, used);

        if (percentage >= 100)
            return $"{limitName}: Exhausted";
        else if (percentage >= 80)
            return $"{limitName}: {percentage:F0}% used (${remaining:N2} remaining)";
        else
            return $"{limitName}: ${remaining:N2} remaining";
    }

    /// <summary>
    /// Validates limit configuration
    /// </summary>
    /// <param name="dailyLimit">Daily limit</param>
    /// <param name="monthlyLimit">Monthly limit</param>
    /// <returns>Tuple of (isValid, errorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidateLimitConfiguration(decimal dailyLimit, decimal monthlyLimit)
    {
        if (dailyLimit < 0)
            return (false, "Daily limit cannot be negative");

        if (monthlyLimit < 0)
            return (false, "Monthly limit cannot be negative");

        if (dailyLimit > 0 && monthlyLimit > 0 && dailyLimit > monthlyLimit)
            return (false, "Daily limit cannot exceed monthly limit");

        return (true, string.Empty);
    }
}
