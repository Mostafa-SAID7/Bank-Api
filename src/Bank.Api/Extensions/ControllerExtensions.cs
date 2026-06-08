using Bank.Api.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Extensions;

/// <summary>
/// Extension methods for ControllerBase to eliminate duplicate utility methods across controllers
/// </summary>
public static class ControllerExtensions
{
    /// <summary>
    /// Gets the current user's ID from the claims principal.
    /// Centralizes duplicate GetCurrentUserId implementations across 7+ controllers
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <returns>The current user's ID</returns>
    public static Guid GetCurrentUserId(this ControllerBase controller)
    {
        return controller.GetCurrentUserIdRequired();
    }
}
