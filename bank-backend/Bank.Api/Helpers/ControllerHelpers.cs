using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Api.Helpers;

/// <summary>
/// Helper methods for controllers
/// </summary>
public static class ControllerHelpers
{
    /// <summary>
    /// Gets the current user ID from claims as a string
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <returns>The user ID as string or null if not found</returns>
    public static string? GetUserId(this ControllerBase controller)
    {
        return controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    /// <summary>
    /// Gets the current user ID from claims
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <returns>The user ID or Guid.Empty if not found</returns>
    public static Guid GetCurrentUserId(this ControllerBase controller)
    {
        var userIdClaim = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }

    /// <summary>
    /// Gets the current user ID from claims with exception if not found
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <returns>The user ID</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when user ID is not found</exception>
    public static Guid GetCurrentUserIdRequired(this ControllerBase controller)
    {
        var userIdClaim = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }

    /// <summary>
    /// Gets the current user email from claims
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <returns>The user email or null if not found</returns>
    public static string? GetCurrentUserEmail(this ControllerBase controller)
    {
        return controller.User.FindFirst(ClaimTypes.Email)?.Value;
    }

    /// <summary>
    /// Gets the current session token from claims
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <returns>The session token or null if not found</returns>
    public static string? GetCurrentSessionToken(this ControllerBase controller)
    {
        return controller.User.FindFirst("session_token")?.Value;
    }

    /// <summary>
    /// Checks if the current user is in the specified role
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <param name="role">The role to check</param>
    /// <returns>True if user is in role, false otherwise</returns>
    public static bool IsInRole(this ControllerBase controller, string role)
    {
        return controller.User.IsInRole(role);
    }

    /// <summary>
    /// Checks if the current user is an admin
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <returns>True if user is admin, false otherwise</returns>
    public static bool IsAdmin(this ControllerBase controller)
    {
        return controller.User.IsInRole("Admin");
    }

    /// <summary>
    /// Creates a standardized error response
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <param name="message">The error message</param>
    /// <param name="statusCode">The HTTP status code</param>
    /// <returns>An ObjectResult with the error</returns>
    public static ObjectResult CreateErrorResponse(this ControllerBase controller, string message, int statusCode = 500)
    {
        return controller.StatusCode(statusCode, new { Success = false, Message = message });
    }

    /// <summary>
    /// Creates a standardized success response
    /// </summary>
    /// <param name="controller">The controller instance</param>
    /// <param name="message">The success message</param>
    /// <param name="data">Optional data to include</param>
    /// <returns>An OkObjectResult with the success response</returns>
    public static OkObjectResult CreateSuccessResponse(this ControllerBase controller, string message, object? data = null)
    {
        var response = new { Success = true, Message = message };
        if (data != null)
        {
            return controller.Ok(new { Success = true, Message = message, Data = data });
        }
        return controller.Ok(response);
    }
}