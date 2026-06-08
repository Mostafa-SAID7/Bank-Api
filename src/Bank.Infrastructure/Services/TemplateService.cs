using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Bank.Infrastructure.Services;

/// <summary>
/// Centralized template service that loads and manages all application templates
/// from the ~/Templates folder structure
/// </summary>
public sealed class TemplateService : ITemplateService
{
    private readonly ILogger<TemplateService> _logger;
    private readonly string _templateBasePath;
    private readonly ConcurrentDictionary<string, string> _templateCache;
    private const string TEMPLATE_VARIABLE_PATTERN = @"\{\{(\w+)\}\}";

    public TemplateService(ILogger<TemplateService> logger, string basePath = "Templates")
    {
        _logger = logger;
        _templateBasePath = Path.Combine(AppContext.BaseDirectory, basePath);
        _templateCache = new ConcurrentDictionary<string, string>();

        if (!Directory.Exists(_templateBasePath))
        {
            _logger.LogWarning("Templates directory not found at: {TemplateBasePath}", _templateBasePath);
        }
    }

    /// <summary>
    /// Get email template by ID
    /// </summary>
    public string GetEmailTemplate(string templateId)
    {
        var cacheKey = $"email_{templateId}";
        
        if (_templateCache.TryGetValue(cacheKey, out var cachedTemplate))
            return cachedTemplate;

        // Define fallback inline templates if file not found
        var inlineTemplates = new Dictionary<string, string>
        {
            { "2fa_token", "<h2>Your Verification Code</h2><p>Hello {{UserName}},</p><p>Your verification code is: <strong>{{Token}}</strong></p><p>This code will expire in {{ExpiryMinutes}} minutes.</p><p>If you didn't request this code, please contact support immediately.</p>" },
            { "password_reset", "<h2>Password Reset Request</h2><p>Hello {{UserName}},</p><p>We received a request to reset your password. Click the link below to proceed:</p><p><a href=\"{{ResetLink}}\">Reset Password</a></p><p>This link will expire in {{ExpiryHours}} hours.</p>" },
            { "account_welcome", "<h2>Welcome to SecureBank</h2><p>Hello {{UserName}},</p><p>Thank you for creating an account with us. Your account has been successfully activated.</p>" },
            { "transaction_confirmation", "<h2>Transaction Confirmation</h2><p>Hello {{CustomerName}},</p><p>Your transaction has been successfully processed. Reference: {{TransactionReference}}</p>" },
            { "bill_payment_confirmation", "<h2>Bill Payment Confirmation</h2><p>Hello {{CustomerName}},</p><p>Your bill payment has been successfully submitted to {{BillerName}}.</p>" },
            { "security_alert", "<h2>Security Alert</h2><p>Hello {{CustomerName}},</p><p>We detected unusual activity on your account. Please review and take action if needed.</p>" },
            { "statement_ready", "<h2>Your Account Statement</h2><p>Hello {{CustomerName}},</p><p>Your account statement for {{PeriodStart}} to {{PeriodEnd}} is now ready.</p>" }
        };

        if (inlineTemplates.TryGetValue(templateId, out var template))
        {
            _templateCache[cacheKey] = template;
            return template;
        }

        _logger.LogWarning("Email template not found: {TemplateId}", templateId);
        return string.Empty;
    }

    /// <summary>
    /// Get SMS template by ID
    /// </summary>
    public string GetSmsTemplate(string templateId)
    {
        var cacheKey = $"sms_{templateId}";
        
        if (_templateCache.TryGetValue(cacheKey, out var cachedTemplate))
            return cachedTemplate;

        // Define fallback inline templates if file not found
        var inlineTemplates = new Dictionary<string, string>
        {
            { "2fa_token", "Your Bank verification code is: {{Token}}. Valid for {{ExpiryMinutes}} minutes." },
            { "password_reset", "Your password reset code is: {{ResetCode}}. Valid for {{ExpiryMinutes}} minutes. Never share this code." },
            { "welcome", "Welcome to SecureBank, {{UserName}}! Your account is now active. Log in to get started." },
            { "transaction_alert", "Transaction alert: ${{Amount}} {{TransactionType}} from account {{AccountLast4}} on {{TransactionDate}}. If not you, contact support." },
            { "payment_confirmation", "Bill payment confirmed: ${{Amount}} to {{BillerName}} on {{PaymentDate}}. Reference: {{PaymentReference}}" },
            { "account_locked", "Your account has been locked due to security concerns. Contact support at 1-800-BANK-123." },
            { "suspicious_activity", "Suspicious activity detected on your account. Please verify your recent transactions or reset your password immediately." },
            { "deposit_maturity_notice", "Your fixed deposit (Account: {{DepositNumber}}) matures on {{MaturityDate}}. Action required: Choose renewal or withdrawal." },
            { "low_balance_alert", "Balance alert: Your account balance is now ${{CurrentBalance}}. Minimum balance is ${{MinimumBalance}}." }
        };

        if (inlineTemplates.TryGetValue(templateId, out var template))
        {
            _templateCache[cacheKey] = template;
            return template;
        }

        _logger.LogWarning("SMS template not found: {TemplateId}", templateId);
        return string.Empty;
    }

    /// <summary>
    /// Get email subject for template
    /// </summary>
    public string GetEmailSubject(string templateId)
    {
        var subjects = new Dictionary<string, string>
        {
            { "2fa_token", "Bank Verification Code" },
            { "password_reset", "Reset Your Password" },
            { "account_welcome", "Welcome to SecureBank" },
            { "transaction_confirmation", "Transaction Confirmation" },
            { "bill_payment_confirmation", "Bill Payment Confirmation" },
            { "security_alert", "Security Alert - Unusual Activity" },
            { "statement_ready", "Your Account Statement is Ready" },
            { "deposit_maturity_notice", "Your Deposit is Maturing - Action Required" },
            { "loan_payment_due", "Loan Payment Due Reminder" },
            { "low_balance_alert", "Balance Alert - Low Account Balance" }
        };

        return subjects.TryGetValue(templateId, out var subject) ? subject : "Bank Notification";
    }

    /// <summary>
    /// Replace template variables with actual values
    /// Supports both {{variable}} and {variable} syntax
    /// </summary>
    public string ReplaceTemplateVariables(string template, Dictionary<string, string> variables)
    {
        if (string.IsNullOrEmpty(template) || variables == null || !variables.Any())
            return template;

        var result = template;

        foreach (var variable in variables)
        {
            // Replace {{variable}} format
            result = result.Replace($"{{{{{variable.Key}}}}}", variable.Value);
            // Replace {variable} format (legacy support)
            result = result.Replace($"{{{variable.Key}}}", variable.Value);
        }

        return result;
    }

    /// <summary>
    /// Get HTML statement template
    /// </summary>
    public string GetStatementTemplate(string templateType = "AccountStatement")
    {
        var cacheKey = $"statement_{templateType}";
        
        if (_templateCache.TryGetValue(cacheKey, out var cachedTemplate))
            return cachedTemplate;

        try
        {
            var templatePath = Path.Combine(_templateBasePath, "Statement", $"{templateType}.html");
            if (File.Exists(templatePath))
            {
                var content = File.ReadAllText(templatePath);
                _templateCache[cacheKey] = content;
                return content;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading statement template: {TemplateType}", templateType);
        }

        return string.Empty;
    }

    /// <summary>
    /// Get deposit certificate template
    /// </summary>
    public string GetDepositCertificateTemplate()
    {
        var cacheKey = "deposit_certificate";
        
        if (_templateCache.TryGetValue(cacheKey, out var cachedTemplate))
            return cachedTemplate;

        try
        {
            var templatePath = Path.Combine(_templateBasePath, "Deposit", "DepositCertificate.html");
            if (File.Exists(templatePath))
            {
                var content = File.ReadAllText(templatePath);
                _templateCache[cacheKey] = content;
                return content;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading deposit certificate template");
        }

        return string.Empty;
    }

    /// <summary>
    /// Clear template cache (useful for testing or template updates)
    /// </summary>
    public void ClearCache()
    {
        _templateCache.Clear();
        _logger.LogInformation("Template cache cleared");
    }

    /// <summary>
    /// Get all available template IDs for a category
    /// </summary>
    public IEnumerable<string> GetAvailableTemplates(string category)
    {
        var templates = new Dictionary<string, List<string>>
        {
            { "email", new() { "2fa_token", "password_reset", "account_welcome", "transaction_confirmation", "bill_payment_confirmation", "security_alert", "statement_ready" } },
            { "sms", new() { "2fa_token", "password_reset", "welcome", "transaction_alert", "payment_confirmation", "account_locked", "suspicious_activity", "deposit_maturity_notice", "low_balance_alert" } },
            { "statement", new() { "AccountStatement", "ConsolidatedStatement" } },
            { "deposit", new() { "DepositCertificate" } }
        };

        return templates.TryGetValue(category.ToLower(), out var list) ? list : Enumerable.Empty<string>();
    }
}

/// <summary>
/// Interface for template service
/// </summary>
public interface ITemplateService
{
    string GetEmailTemplate(string templateId);
    string GetSmsTemplate(string templateId);
    string GetEmailSubject(string templateId);
    string ReplaceTemplateVariables(string template, Dictionary<string, string> variables);
    string GetStatementTemplate(string templateType = "AccountStatement");
    string GetDepositCertificateTemplate();
    void ClearCache();
    IEnumerable<string> GetAvailableTemplates(string category);
}
