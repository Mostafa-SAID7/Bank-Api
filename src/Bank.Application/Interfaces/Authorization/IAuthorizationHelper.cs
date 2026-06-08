namespace Bank.Application.Interfaces.Authorization;

/// <summary>
/// Interface for authorization and ownership verification operations.
/// Centralizes repeated authorization checks from multiple controllers.
/// </summary>
public interface IAuthorizationHelper
{
    /// <summary>
    /// Verifies that a user owns a specific resource.
    /// Automatically handles admin access (admins can access any resource).
    /// </summary>
    /// <typeparam name="T">The resource type</typeparam>
    /// <param name="resourceId">The ID of the resource to check</param>
    /// <param name="userId">The user ID to verify ownership</param>
    /// <param name="resourceGetter">Async function to fetch the resource</param>
    /// <param name="getOwnerId">Function to extract the owner ID from the resource</param>
    /// <param name="userRole">The user's role (if Admin, returns true)</param>
    /// <param name="resourceName">Friendly name of the resource for error messages</param>
    /// <returns>Tuple of (IsAuthorized, ErrorMessage). ErrorMessage is null if authorized.</returns>
    Task<(bool IsAuthorized, string? ErrorMessage)> VerifyResourceOwnershipAsync<T>(
        Guid resourceId,
        Guid userId,
        Func<Guid, Task<T?>> resourceGetter,
        Func<T, Guid> getOwnerId,
        string? userRole = null,
        string resourceName = "Resource") where T : class;

    /// <summary>
    /// Checks if a user has any of the specified roles.
    /// </summary>
    /// <param name="userRole">The user's current role</param>
    /// <param name="roles">The roles to check against</param>
    /// <returns>True if user has any of the specified roles</returns>
    bool HasRole(string? userRole, params string[] roles);

    /// <summary>
    /// Checks if a user is an admin.
    /// </summary>
    /// <param name="userRole">The user's current role</param>
    /// <returns>True if the user is an admin</returns>
    bool IsAdmin(string? userRole);
}
