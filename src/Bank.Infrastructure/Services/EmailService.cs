using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Bank.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bank.Infrastructure.Services;

/// <summary>
/// Email service implementation using SMTP with mandatory SSL/TLS encryption.
/// Uses centralized template service for template management.
/// 
/// SECURITY: EnableSsl is ALWAYS set to true. SMTP connections MUST use encrypted channels.
/// This protects email credentials and message content from interception (OWASP A2, CWE-319, STIG V-222596).
/// </summary>
public sealed class EmailService : IEmailService, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly ITemplateService _templateService;
    private readonly SmtpClient _smtpClient;
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(500);

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger, ITemplateService templateService)
    {
        _configuration = configuration;
        _logger = logger;
        _templateService = templateService;
        
        // Configure SMTP client with MANDATORY SSL/TLS encryption
        var smtpHost = _configuration["Email:SmtpHost"] ?? "localhost";
        var smtpPortStr = _configuration["Email:SmtpPort"] ?? "587";
        var smtpPort = int.TryParse(smtpPortStr, out var parsedPort) ? parsedPort : 587;
        var username = _configuration["Email:Username"] ?? "noreply@bankapp.com";
        var password = _configuration["Email:Password"] ?? "password";

        _smtpClient = new SmtpClient(smtpHost, smtpPort)
        {
            // SECURITY: EnableSsl is ALWAYS true - no exceptions
            // Port 587 (submission) and 465 (SMTPS) both require encrypted connections
            EnableSsl = true,
            UseDefaultCredentials = string.IsNullOrEmpty(username),
            Credentials = !string.IsNullOrEmpty(username) ? new NetworkCredential(username, password) : null,
            // Ensure credentials are not exposed
            Timeout = 10000
        };

        _logger.LogInformation("SMTP client configured with SSL/TLS encryption enabled (secure channel)");
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        try
        {
            if (!IsValidEmail(to))
            {
                _logger.LogWarning("Invalid email address: {Email}", to);
                return false;
            }

            var fromEmail = _configuration["Email:FromAddress"] ?? "noreply@bankapp.com";
            var fromName = _configuration["Email:FromName"] ?? "Bank Management System";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            mailMessage.To.Add(to);

            await _smtpClient.SendMailAsync(mailMessage);
            
            _logger.LogInformation("Email sent successfully to {Email}", to);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", to);
            return false;
        }
    }

    public async Task<bool> SendEmailAsync(List<string> to, string subject, string body, bool isHtml = false)
    {
        try
        {
            var validEmails = to.Where(IsValidEmail).ToList();
            if (!validEmails.Any())
            {
                _logger.LogWarning("No valid email addresses provided");
                return false;
            }

            var fromEmail = _configuration["Email:FromAddress"] ?? "noreply@bankapp.com";
            var fromName = _configuration["Email:FromName"] ?? "Bank Management System";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            foreach (var email in validEmails)
            {
                mailMessage.To.Add(email);
            }

            await _smtpClient.SendMailAsync(mailMessage);
            
            _logger.LogInformation("Email sent successfully to {Count} recipients", validEmails.Count);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to multiple recipients");
            return false;
        }
    }

    public async Task<bool> SendEmailAsync(string to, string templateId, Dictionary<string, string> parameters)
    {
        try
        {
            var template = _templateService.GetEmailTemplate(templateId);
            if (string.IsNullOrEmpty(template))
            {
                _logger.LogWarning("Email template not found: {TemplateId}", templateId);
                return false;
            }

            var subject = _templateService.GetEmailSubject(templateId);
            var body = _templateService.ReplaceTemplateVariables(template, parameters);

            return await SendEmailAsync(to, subject, body, true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send templated email to {Email} with template {TemplateId}", to, templateId);
            return false;
        }
    }

    public async Task<bool> SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string attachmentName, bool isHtml = false)
    {
        try
        {
            if (!IsValidEmail(to))
            {
                _logger.LogWarning("Invalid email address: {Email}", to);
                return false;
            }

            var fromEmail = _configuration["Email:FromAddress"] ?? "noreply@bankapp.com";
            var fromName = _configuration["Email:FromName"] ?? "Bank Management System";

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            mailMessage.To.Add(to);

            // Add attachment
            if (attachment != null && attachment.Length > 0)
            {
                var attachmentStream = new MemoryStream(attachment);
                var mailAttachment = new Attachment(attachmentStream, attachmentName);
                mailMessage.Attachments.Add(mailAttachment);
            }

            await _smtpClient.SendMailAsync(mailMessage);
            
            _logger.LogInformation("Email with attachment sent successfully to {Email}", to);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email with attachment to {Email}", to);
            return false;
        }
    }

    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            // Use regex for basic email validation
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, RegexTimeout);
            return emailRegex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _smtpClient?.Dispose();
        }
    }

    ~EmailService()
    {
        Dispose(false);
    }
}