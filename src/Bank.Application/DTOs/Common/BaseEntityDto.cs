namespace Bank.Application.DTOs.Common;

/// <summary>
/// Base class for entity DTOs with identity and audit information
/// </summary>
public abstract class BaseEntityDto : BaseAuditDto
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; set; }
}
