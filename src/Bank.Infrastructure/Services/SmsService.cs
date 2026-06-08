using System.Text.RegularExpressions;
using Bank.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bank.Infrastructure.Services;

/// <summary>
/// SMS service implementation - mock implementation for development
/// In production, integrate with services like Twilio, AWS SNS, etc.
/// </summary>
public class SmsService : ISmsService
{
    private readonly ILogger<SmsService> _logger;
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(500);

    public SmsService(ILogger<SmsService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            if (!IsValidPhoneNumber(phoneNumber))
            {
                _logger.LogWarning("Invalid phone number format provided for SMS (masked: {PhoneNumberMasked})", MaskPhoneNumber(phoneNumber));
                return false;
            }

            // Mock implementation - log the SMS instead of actually sending
            // In production, integrate with SMS provider (Twilio, AWS SNS, etc.)
            // Never log the full phone number or message content (may contain OTP codes)
            _logger.LogInformation("SMS dispatched to {PhoneNumberMasked}", MaskPhoneNumber(phoneNumber));

            // Simulate API call delay
            await Task.Delay(100);

            // For development, always return success
            // In production, handle actual SMS provider responses
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS to {PhoneNumberMasked}", MaskPhoneNumber(phoneNumber));
            return false;
        }
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string templateId, Dictionary<string, string> parameters)
    {
        try
        {
            var template = GetSmsTemplate(templateId);
            if (string.IsNullOrEmpty(template))
            {
                _logger.LogWarning("SMS template not found: {TemplateId}", templateId);
                return false;
            }

            var message = ReplaceTemplateParameters(template, parameters);
            return await SendSmsAsync(phoneNumber, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send templated SMS to {PhoneNumberMasked} with template {TemplateId}", MaskPhoneNumber(phoneNumber), templateId);
            return false;
        }
    }

    public bool IsValidPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return false;

        try
        {
            // Basic phone number validation - supports international format
            var phoneRegex = new Regex(@"^\+?[1-9]\d{1,14}$", RegexOptions.None, RegexTimeout);
            var cleanedNumber = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            return phoneRegex.IsMatch(cleanedNumber);
        }
        catch
        {
            return false;
        }
    }

    private string GetSmsTemplate(string templateId)
    {
        // Simple template system - in production, load from database or configuration
        return templateId switch
        {
            "2fa_token" => "Your Bank verification code is: {Token}. Valid for {ExpiryMinutes} minutes.",
            
            "password_reset" => "Your password reset code is: {ResetCode}. Valid for {ExpiryMinutes} minutes. Never share this code.",
            
            "welcome" => "Welcome to SecureBank, {UserName}! Your account is now active. Log in to get started.",
            
            "transaction_alert" => "Transaction alert: ${Amount} {TransactionType} from account {AccountLast4} on {TransactionDate}. If not you, contact support.",
            
            "payment_confirmation" => "Bill payment confirmed: ${Amount} to {BillerName} on {PaymentDate}. Reference: {PaymentReference}",
            
            "account_locked" => "Your account has been locked due to security concerns. Contact support at 1-800-BANK-123 to unlock your account.",
            
            "suspicious_activity" => "Suspicious activity detected on your account. Please verify your recent transactions or reset your password immediately.",
            
            "deposit_maturity_notice" => "Your fixed deposit (Account: {DepositNumber}) matures on {MaturityDate}. Action required: Choose renewal or withdrawal.",
            
            "low_balance_alert" => "Balance alert: Your account balance is now ${CurrentBalance}. Minimum balance is ${MinimumBalance}.",
            
            "loan_payment_due" => "Loan payment due: ${Amount} for loan {LoanNumber} is due on {DueDate}. Pay now to avoid late fees.",
            
            "card_blocked" => "Your card ending in {CardLast4} has been blocked for security. Call {SupportNumber} to unblock it.",
            
            "card_activated" => "Your new {CardType} card ending in {CardLast4} has been activated and is ready to use.",
            
            _ => string.Empty
        };
    }

    private static string ReplaceTemplateParameters(string template, Dictionary<string, string> parameters)
    {
        var result = template;
        foreach (var parameter in parameters)
        {
            result = result.Replace($"{{{parameter.Key}}}", parameter.Value);
        }
        return result;
    }

    /// <summary>
    /// Masks a phone number for safe logging: keeps country code prefix and last 4 digits.
    /// e.g. "+12025551234" → "+1******1234"
    /// </summary>
    private static string MaskPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrEmpty(phoneNumber)) return "[empty]";
        var cleaned = phoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        if (cleaned.Length <= 4) return "[redacted]";
        var prefix = cleaned.StartsWith('+') ? cleaned[..2] : cleaned[..1];
        var suffix = cleaned[^4..];
        var masked = new string('*', Math.Max(0, cleaned.Length - prefix.Length - 4));
        return $"{prefix}{masked}{suffix}";
    }
}