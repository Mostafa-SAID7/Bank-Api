namespace Bank.Application.Interfaces;

/// <summary>
/// Service for sending SMS messages
/// </summary>
public interface ISmsService
{
    /// <summary>
    /// Send SMS message to a phone number
    /// </summary>
    Task<bool> SendSmsAsync(string phoneNumber, string message);
    
    /// <summary>
    /// Send SMS with template
    /// </summary>
    Task<bool> SendSmsAsync(string phoneNumber, string templateId, Dictionary<string, string> parameters);
    
    /// <summary>
    /// Validate phone number format
    /// </summary>
    bool IsValidPhoneNumber(string phoneNumber);
}