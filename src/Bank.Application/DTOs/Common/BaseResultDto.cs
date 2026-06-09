namespace Bank.Application.DTOs.Common;

/// <summary>
/// Base class for operation result DTOs
/// Eliminates duplication of Success/IsSuccess, Message, and Errors properties
/// </summary>
public abstract class BaseResultDto
{
    /// <summary>
    /// Whether the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Human-readable message about the operation result
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// List of errors that occurred, if any
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Convenience method to check if any errors exist
    /// </summary>
    public bool HasErrors => Errors?.Count > 0;
}
