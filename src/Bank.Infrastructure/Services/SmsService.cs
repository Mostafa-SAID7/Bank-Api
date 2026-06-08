using Bank.Application.Helpers.Shared;
using Bank.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bank.Infrastructure.Services;

/// <summary>
/// SMS service implementation - mock implementation for development
/// In production, integrate with services like Twilio, AWS SNS, etc.
/// Uses centralized template service for template management.
/// Uses centralized ValidationHelper for phone number validation.
/// Uses centralized MaskingHelper for safe phone number logging.
/// </summary>
public sealed class SmsService : ISmsService
{
    private readonly ILogger<SmsService> _logger;
    private readonly ITemplateService _templateService;

    public SmsService(ILogger<SmsService> logger, ITemplateService templateService)
    {
        _logger = logger;
        _templateService = templateService;
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message)
    {
        try
        {
            if (!ValidationHelper.IsValidPhoneNumber(phoneNumber))
            {
                _logger.LogWarning("Invalid phone number format provided for SMS (masked: {PhoneNumberMasked})", MaskingHelper.MaskPhoneNumber(phoneNumber));
                return false;
            }

            // Mock implementation - log the SMS instead of actually sending
            // In production, integrate with SMS provider (Twilio, AWS SNS, etc.)
            // Never log the full phone number or message content (may contain OTP codes)
            _logger.LogInformation("SMS dispatched to {PhoneNumberMasked}", MaskingHelper.MaskPhoneNumber(phoneNumber));

            // Simulate API call delay
            await Task.Delay(100);

            // For development, always return success
            // In production, handle actual SMS provider responses
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send SMS to {PhoneNumberMasked}", MaskingHelper.MaskPhoneNumber(phoneNumber));
            return false;
        }
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string templateId, Dictionary<string, string> parameters)
    {
        try
        {
            var template = _templateService.GetSmsTemplate(templateId);
            if (string.IsNullOrEmpty(template))
            {
                _logger.LogWarning("SMS template not found: {TemplateId}", templateId);
                return false;
            }

            var message = _templateService.ReplaceTemplateVariables(template, parameters);
            return await SendSmsAsync(phoneNumber, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send templated SMS to {PhoneNumberMasked} with template {TemplateId}", MaskingHelper.MaskPhoneNumber(phoneNumber), templateId);
            return false;
        }
    }

    public bool IsValidPhoneNumber(string phoneNumber)
    {
        return ValidationHelper.IsValidPhoneNumber(phoneNumber);
    }
}