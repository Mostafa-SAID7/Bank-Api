namespace Bank.Application.Interfaces;

/// <summary>
/// Service for sending email messages
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send email message
    /// </summary>
    Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    
    /// <summary>
    /// Send email to multiple recipients
    /// </summary>
    Task<bool> SendEmailAsync(List<string> to, string subject, string body, bool isHtml = false);
    
    /// <summary>
    /// Send email with template
    /// </summary>
    Task<bool> SendEmailAsync(string to, string templateId, Dictionary<string, string> parameters);
    
    /// <summary>
    /// Send email with attachment
    /// </summary>
    Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string attachmentName, bool isHtml = false);
    
    /// <summary>
    /// Validate email address format
    /// </summary>
    bool IsValidEmail(string email);
}