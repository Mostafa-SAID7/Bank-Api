using Bank.Domain.Enums;

namespace Bank.Application.Helpers.Account;

/// <summary>
/// Helper for account status operations
/// </summary>
public static class AccountStatusHelper
{
    /// <summary>
    /// Determines if account status transition is valid
    /// </summary>
    /// <param name="currentStatus">Current account status</param>
    /// <param name="newStatus">New account status</param>
    /// <returns>True if transition is valid</returns>
    public static bool IsValidStatusTransition(AccountStatus currentStatus, AccountStatus newStatus)
    {
        // Same status is always valid (no-op)
        if (currentStatus == newStatus)
            return true;

        // Define valid transitions
        return (currentStatus, newStatus) switch
        {
            // From Active
            (AccountStatus.Active, AccountStatus.Suspended) => true,
            (AccountStatus.Active, AccountStatus.Closed) => true,
            (AccountStatus.Active, AccountStatus.Frozen) => true,

            // From Suspended
            (AccountStatus.Suspended, AccountStatus.Active) => true,
            (AccountStatus.Suspended, AccountStatus.Closed) => true,
            (AccountStatus.Suspended, AccountStatus.Frozen) => true,

            // From Frozen
            (AccountStatus.Frozen, AccountStatus.Active) => true,
            (AccountStatus.Frozen, AccountStatus.Suspended) => true,
            (AccountStatus.Frozen, AccountStatus.Closed) => true,

            // From Closed - no transitions allowed
            (AccountStatus.Closed, _) => false,

            // From Pending
            (AccountStatus.Pending, AccountStatus.Active) => true,
            (AccountStatus.Pending, AccountStatus.Closed) => true,

            // Default - invalid
            _ => false
        };
    }

    /// <summary>
    /// Gets status display name
    /// </summary>
    /// <param name="status">Account status</param>
    /// <returns>Display name</returns>
    public static string GetStatusDisplayName(AccountStatus status)
    {
        return status switch
        {
            AccountStatus.Active => "Active",
            AccountStatus.Suspended => "Suspended",
            AccountStatus.Closed => "Closed",
            AccountStatus.Frozen => "Frozen",
            AccountStatus.Pending => "Pending",
            _ => "Unknown"
        };
    }

    /// <summary>
    /// Checks if account is in active state
    /// </summary>
    /// <param name="status">Account status</param>
    /// <returns>True if active</returns>
    public static bool IsActive(AccountStatus status)
    {
        return status == AccountStatus.Active;
    }

    /// <summary>
    /// Checks if account is in final state (cannot be changed)
    /// </summary>
    /// <param name="status">Account status</param>
    /// <returns>True if in final state</returns>
    public static bool IsInFinalState(AccountStatus status)
    {
        return status == AccountStatus.Closed;
    }

    /// <summary>
    /// Checks if account can perform transactions
    /// </summary>
    /// <param name="status">Account status</param>
    /// <returns>True if can perform transactions</returns>
    public static bool CanPerformTransactions(AccountStatus status)
    {
        return status is AccountStatus.Active or AccountStatus.Pending;
    }

    /// <summary>
    /// Checks if account can receive deposits
    /// </summary>
    /// <param name="status">Account status</param>
    /// <returns>True if can receive deposits</returns>
    public static bool CanReceiveDeposits(AccountStatus status)
    {
        return status is AccountStatus.Active or AccountStatus.Suspended or AccountStatus.Pending;
    }

    /// <summary>
    /// Checks if account can make withdrawals
    /// </summary>
    /// <param name="status">Account status</param>
    /// <returns>True if can make withdrawals</returns>
    public static bool CanMakeWithdrawals(AccountStatus status)
    {
        return status == AccountStatus.Active;
    }

    /// <summary>
    /// Gets reason why account cannot perform transaction
    /// </summary>
    /// <param name="status">Account status</param>
    /// <returns>Reason message</returns>
    public static string GetTransactionBlockReason(AccountStatus status)
    {
        return status switch
        {
            AccountStatus.Suspended => "Account is suspended. Please contact support.",
            AccountStatus.Closed => "Account is closed. No transactions allowed.",
            AccountStatus.Frozen => "Account is frozen. Please contact support.",
            AccountStatus.Pending => "Account is pending activation. Please complete verification.",
            _ => "Account cannot perform transactions."
        };
    }

    /// <summary>
    /// Gets status color for UI display
    /// </summary>
    /// <param name="status">Account status</param>
    /// <returns>Color code</returns>
    public static string GetStatusColor(AccountStatus status)
    {
        return status switch
        {
            AccountStatus.Active => "#28a745",      // Green
            AccountStatus.Suspended => "#ffc107",   // Yellow
            AccountStatus.Closed => "#dc3545",      // Red
            AccountStatus.Frozen => "#6c757d",      // Gray
            AccountStatus.Pending => "#17a2b8",     // Blue
            _ => "#6c757d"                          // Gray
        };
    }

    /// <summary>
    /// Gets status icon for UI display
    /// </summary>
    /// <param name="status">Account status</param>
    /// <returns>Icon symbol</returns>
    public static string GetStatusIcon(AccountStatus status)
    {
        return status switch
        {
            AccountStatus.Active => "✓",
            AccountStatus.Suspended => "⚠",
            AccountStatus.Closed => "✕",
            AccountStatus.Frozen => "❄",
            AccountStatus.Pending => "⏳",
            _ => "?"
        };
    }
}
