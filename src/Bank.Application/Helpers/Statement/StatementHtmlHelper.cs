using System.Text;

namespace Bank.Application.Helpers.Statement;

/// <summary>
/// Centralized HTML generation helper for statement reports.
/// Extracted from StatementGenerator to promote Single Responsibility Principle.
/// Provides reusable CSS styles and HTML generation utilities.
/// </summary>
public static class StatementHtmlHelper
{
    /// <summary>
    /// Gets the default CSS styles for statements.
    /// Includes styling for tables, headers, transactions, and disclosures.
    /// Can be combined with custom template-specific styles.
    /// </summary>
    /// <returns>CSS stylesheet as string</returns>
    public static string GetDefaultCssStyles()
    {
        return @"
            body { font-family: Arial, sans-serif; margin: 20px; line-height: 1.4; }
            .bank-header { text-align: center; border-bottom: 2px solid #333; padding-bottom: 10px; margin-bottom: 20px; }
            .statement-header { margin-bottom: 20px; }
            .account-info, .balance-summary, .monthly-statistics, .fee-summary { margin-bottom: 20px; }
            table { width: 100%; border-collapse: collapse; margin-bottom: 10px; }
            th, td { padding: 8px; text-align: left; border-bottom: 1px solid #ddd; }
            th { background-color: #f2f2f2; font-weight: bold; }
            .transaction-table { margin-top: 10px; }
            .credit { color: green; font-weight: bold; }
            .debit { color: red; font-weight: bold; }
            .total-row { border-top: 2px solid #333; font-weight: bold; }
            .disclosures { margin-top: 30px; padding-top: 20px; border-top: 1px solid #ccc; font-size: 12px; }
            .disclosure-section { margin-bottom: 15px; }
            .disclosure-section h4 { margin-bottom: 5px; color: #555; font-size: 13px; }
            .monthly-statistics table, .fee-summary table { background-color: #f9f9f9; }
            .monthly-statistics h3, .fee-summary h3 { color: #2c5aa0; }
            h1, h2, h3 { color: #333; }
            h4 { color: #555; margin-top: 15px; margin-bottom: 5px; }
        ";
    }

    /// <summary>
    /// Generates HTML header section with DOCTYPE, meta tags, and style references.
    /// Used for both single and consolidated statements.
    /// </summary>
    /// <param name="title">HTML page title</param>
    /// <param name="defaultCss">Default CSS styles</param>
    /// <param name="customCss">Optional custom CSS to append</param>
    /// <returns>HTML head section as string</returns>
    public static string GenerateHtmlHeader(string title, string? defaultCss = null, string? customCss = null)
    {
        var html = new StringBuilder();
        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html>");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset='utf-8'>");
        html.AppendLine($"<title>{title}</title>");
        html.AppendLine("<style>");
        
        if (string.IsNullOrEmpty(defaultCss))
        {
            html.AppendLine(GetDefaultCssStyles());
        }
        else
        {
            html.AppendLine(defaultCss);
        }
        
        if (!string.IsNullOrEmpty(customCss))
        {
            html.AppendLine(customCss);
        }
        
        html.AppendLine("</style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        
        return html.ToString();
    }

    /// <summary>
    /// Generates HTML footer (closing tags).
    /// </summary>
    /// <returns>HTML closing tags</returns>
    public static string GenerateHtmlFooter()
    {
        return @"
</body>
</html>";
    }

    /// <summary>
    /// Generates bank branding header section.
    /// Includes bank name and tagline if enabled.
    /// </summary>
    /// <param name="bankName">Name of the bank</param>
    /// <param name="tagline">Optional tagline/motto</param>
    /// <returns>HTML bank header section</returns>
    public static string GenerateBankHeader(string bankName = "SecureBank", string tagline = "Your Trusted Financial Partner")
    {
        var html = new StringBuilder();
        html.AppendLine("<div class='bank-header'>");
        html.AppendLine($"<h1>{bankName}</h1>");
        html.AppendLine($"<p>{tagline}</p>");
        html.AppendLine("</div>");
        return html.ToString();
    }

    /// <summary>
    /// Determines fee type category from transaction description.
    /// Used to group and summarize fees for reporting.
    /// </summary>
    /// <param name="description">Transaction description containing fee indicator</param>
    /// <returns>Categorized fee type (e.g., "Monthly Maintenance Fee")</returns>
    public static string DetermineFeeType(string description)
    {
        if (string.IsNullOrEmpty(description))
            return "Other Fee";

        var desc = description.ToLower();
        
        if (desc.Contains("maintenance") || desc.Contains("monthly"))
            return "Monthly Maintenance Fee";
        if (desc.Contains("overdraft") || desc.Contains("nsf"))
            return "Overdraft Fee";
        if (desc.Contains("atm") || desc.Contains("withdrawal"))
            return "ATM/Withdrawal Fee";
        if (desc.Contains("transfer") || desc.Contains("wire"))
            return "Transfer Fee";
        if (desc.Contains("foreign") || desc.Contains("international"))
            return "Foreign Transaction Fee";
        if (desc.Contains("minimum") || desc.Contains("balance"))
            return "Minimum Balance Fee";
        if (desc.Contains("dormancy") || desc.Contains("inactive"))
            return "Dormancy Fee";
        if (desc.Contains("stop") || desc.Contains("payment"))
            return "Stop Payment Fee";
        if (desc.Contains("check") || desc.Contains("returned"))
            return "Returned Check Fee";
        if (desc.Contains("late") || desc.Contains("penalty"))
            return "Late Payment Penalty";
        if (desc.Contains("excess") || desc.Contains("limit"))
            return "Excess Limit Fee";
        
        return "Other Fee";
    }

    /// <summary>
    /// Generates regulatory disclosures section for statements.
    /// Includes FDIC insurance information, privacy notice, and fee disclosures.
    /// </summary>
    /// <param name="hasFees">Whether to include fee-specific disclosures</param>
    /// <returns>HTML disclosures section</returns>
    public static string GenerateRegulatoryDisclosures(bool hasFees = false)
    {
        var html = new StringBuilder();
        html.AppendLine("<div class='disclosures'>");
        html.AppendLine("<h3>Important Disclosures</h3>");
        
        html.AppendLine("<div class='disclosure-section'>");
        html.AppendLine("<h4>Account Information</h4>");
        html.AppendLine("<p><small>This statement is provided for informational purposes. Please review all transactions carefully and report any discrepancies within 30 days of the statement date.</small></p>");
        html.AppendLine("<p><small>For questions about your account, please contact customer service at 1-800-BANK-123 or visit our website.</small></p>");
        html.AppendLine("</div>");
        
        html.AppendLine("<div class='disclosure-section'>");
        html.AppendLine("<h4>Regulatory Information</h4>");
        html.AppendLine("<p><small>FDIC Insured - Equal Housing Lender - Member FDIC</small></p>");
        html.AppendLine("<p><small>Your deposits are insured up to $250,000 per depositor, per insured bank, for each account ownership category.</small></p>");
        html.AppendLine("<p><small>This institution is an Equal Opportunity Lender and complies with all applicable federal civil rights laws.</small></p>");
        html.AppendLine("</div>");
        
        html.AppendLine("<div class='disclosure-section'>");
        html.AppendLine("<h4>Privacy Notice</h4>");
        html.AppendLine("<p><small>We are committed to protecting your privacy. We do not sell customer information to third parties.</small></p>");
        html.AppendLine("<p><small>For our complete privacy policy, visit our website or request a copy by calling customer service.</small></p>");
        html.AppendLine("</div>");
        
        html.AppendLine("<div class='disclosure-section'>");
        html.AppendLine("<h4>Electronic Statements</h4>");
        html.AppendLine("<p><small>Electronic statements are available through online banking and mobile app.</small></p>");
        html.AppendLine("<p><small>You may opt out of electronic statements at any time by contacting customer service.</small></p>");
        html.AppendLine("</div>");
        
        if (hasFees)
        {
            html.AppendLine("<div class='disclosure-section'>");
            html.AppendLine("<h4>Fee Information</h4>");
            html.AppendLine("<p><small>Fees charged during this statement period are detailed in the Fee Summary section above.</small></p>");
            html.AppendLine("<p><small>For a complete fee schedule, please refer to your account agreement or visit our website.</small></p>");
            html.AppendLine("</div>");
        }
        
        html.AppendLine("</div>");
        return html.ToString();
    }
}
