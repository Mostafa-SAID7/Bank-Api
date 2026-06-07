namespace Bank.Application.DTOs.Common;

/// <summary>
/// Base class for DTOs with audit trail information
/// </summary>
public abstract class BaseAuditDto
{
    /// <summary>
    /// Entity creation timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Entity last modification timestamp
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
