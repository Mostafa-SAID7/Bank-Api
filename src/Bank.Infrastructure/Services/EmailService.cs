using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Bank.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Bank.Infrastructure.Services;

/// <summary>
/// Email service implementation using SMTP with mandatory SSL/TLS encryption.
/// 
/// SECURITY: EnableSsl is ALWAYS set to true. SMTP connections MUST use encrypted channels.
/// This protects email credentials and message content from interception (OWASP A2, CWE-319, STIG V-222596).
/// </summary>
public class EmailService : IEmailService, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpClient _smtpClient;
    private static readonly TimeSpan RegexTimeout = TimeSpan.FromMilliseconds(500);

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        
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
            // For now, use a simple template system
            // In production, you might use a more sophisticated template engine
            var template = GetEmailTemplate(templateId);
            if (string.IsNullOrEmpty(template))
            {
                _logger.LogWarning("Email template not found: {TemplateId}", templateId);
                return false;
            }

            var subject = GetTemplateSubject(templateId);
            var body = ReplaceTemplateParameters(template, parameters);

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

    private string GetEmailTemplate(string templateId)
    {
        // Simple template system - in production, load from database or files
        return templateId switch
        {
            "2fa_token" => "<h2>Your Verification Code</h2><p>Hello {UserName},</p><p>Your verification code is: <strong>{Token}</strong></p><p>This code will expire in {ExpiryMinutes} minutes.</p><p>If you didn't request this code, please contact support immediately.</p><footer><p style=\"color: #666; font-size: 12px; margin-top: 20px;\">This is an automated message. Please do not reply to this email.</p></footer>",
            
            "password_reset" => "<h2>Password Reset Request</h2><p>Hello {UserName},</p><p>We received a request to reset your password. Click the link below to proceed:</p><p><a href=\"{ResetLink}\" style=\"background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;\">Reset Password</a></p><p>This link will expire in {ExpiryHours} hours.</p><p>If you didn't request this, you can safely ignore this email.</p><footer><p style=\"color: #666; font-size: 12px; margin-top: 20px;\">This is an automated message. Please do not reply to this email.</p></footer>",
            
            "welcome" => "<h2>Welcome to SecureBank</h2><p>Hello {UserName},</p><p>Thank you for creating an account with us. Your account has been successfully activated.</p><h3>Your Account Details</h3><ul><li>Email: {Email}</li><li>Account Created: {CreatedDate}</li><li>Customer ID: {CustomerId}</li></ul><p>You can now log in and start using our banking services. For security, we recommend setting up two-factor authentication in your account settings.</p><p><a href=\"{LoginLink}\" style=\"background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;\">Log In Now</a></p><footer><p style=\"color: #666; font-size: 12px; margin-top: 20px;\">This is an automated message. Please do not reply to this email.</p></footer>",
            
            "transaction_confirmation" => "<h2>Transaction Confirmation</h2><p>Hello {CustomerName},</p><p>Your transaction has been successfully processed.</p><h3>Transaction Details</h3><ul><li>Reference Number: {TransactionReference}</li><li>Type: {TransactionType}</li><li>Amount: ${Amount}</li><li>Date: {TransactionDate}</li><li>From Account: {FromAccount}</li><li>To Account: {ToAccount}</li><li>Status: {Status}</li></ul><p>If you did not authorize this transaction, please contact us immediately.</p><footer><p style=\"color: #666; font-size: 12px; margin-top: 20px;\">This is an automated message. Please do not reply to this email.</p></footer>",
            
            "bill_payment_confirmation" => "<h2>Bill Payment Confirmation</h2><p>Hello {CustomerName},</p><p>Your bill payment has been successfully submitted.</p><h3>Payment Details</h3><ul><li>Biller: {BillerName}</li><li>Account Number: {BillerAccountNumber}</li><li>Amount: ${Amount}</li><li>Payment Date: {PaymentDate}</li><li>Reference Number: {PaymentReference}</li><li>Expected Settlement: {SettlementDate}</li></ul><p>You will receive another confirmation once the payment is cleared.</p><footer><p style=\"color: #666; font-size: 12px; margin-top: 20px;\">This is an automated message. Please do not reply to this email.</p></footer>",
            
            "security_alert" => "<h2>Security Alert</h2><p>Hello {CustomerName},</p><p>We detected unusual activity on your account. Please review the details below:</p><h3>Activity Details</h3><ul><li>Type: {ActivityType}</li><li>Location: {Location}</li><li>Device: {Device}</li><li>Time: {ActivityTime}</li></ul><p>If this was not you, please change your password immediately and contact our security team.</p><p><a href=\"{SecurityLink}\" style=\"background-color: #dc3545; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;\">Review Account Security</a></p><footer><p style=\"color: #666; font-size: 12px; margin-top: 20px;\">This is an automated message. Please do not reply to this email.</p></footer>",
            
            "statement_ready" => "<h2>Your Account Statement</h2><p>Hello {CustomerName},</p><p>Your account statement for the period {PeriodStart} to {PeriodEnd} is now ready.</p><h3>Statement Summary</h3><ul><li>Account Number: {AccountNumber}</li><li>Opening Balance: ${OpeningBalance}</li><li>Closing Balance: ${ClosingBalance}</li><li>Total Transactions: {TransactionCount}</li></ul><p><a href=\"{StatementLink}\" style=\"background-color: #007bff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;\">View Statement</a></p><footer><p style=\"color: #666; font-size: 12px; margin-top: 20px;\">This is an automated message. Please do not reply to this email.</p></footer>",
            
            _ => string.Empty
        };
    }

    private string GetTemplateSubject(string templateId)
    {
        return templateId switch
        {
            "2fa_token" => "Bank Verification Code",
            "password_reset" => "Reset Your Password",
            "welcome" => "Welcome to SecureBank",
            "account_welcome" => "Welcome to SecureBank",
            "transaction_confirmation" => "Transaction Confirmation - {TransactionReference}",
            "bill_payment_confirmation" => "Bill Payment Confirmation - {BillerName}",
            "security_alert" => "Security Alert - Unusual Activity",
            "statement_ready" => "Your Account Statement is Ready",
            "deposit_maturity_notice" => "Your Deposit is Maturing - Action Required",
            "loan_payment_due" => "Loan Payment Due Reminder",
            "low_balance_alert" => "Balance Alert - Low Account Balance",
            _ => "Bank Notification"
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