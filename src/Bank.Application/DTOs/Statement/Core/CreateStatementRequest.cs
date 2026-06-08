using Bank.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.DTOs.Statement.Core;

/// <summary>
/// Request DTO for creating a statement
/// </summary>
public class CreateStatementRequest
{
    /// <summary>
    /// Account ID to generate statement for
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }

    /// <summary>
    /// Start date for statement period
    /// </summary>
    [Required]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date for statement period
    /// </summary>
    [Required]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Format for the statement (PDF, Excel, etc.)
    /// </summary>
    public StatementFormat Format { get; set; } = StatementFormat.PDF;
}
