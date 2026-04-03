namespace Bank.Application.Validators.Shared;

/// <summary>
/// Validator for sanctions check simulation
/// </summary>
public static class SanctionsValidator
{
    /// <summary>
    /// Performs basic sanctions check simulation
    /// </summary>
    /// <param name="name">Name to check</param>
    /// <param name="country">Country code</param>
    /// <returns>True if passed sanctions check</returns>
    public static bool PerformCheck(string name, string? country = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // Simulate sanctions list check
        var sanctionedNames = new[]
        {
            "BLOCKED PERSON",
            "SANCTIONED ENTITY",
            "PROHIBITED INDIVIDUAL"
        };

        var sanctionedCountries = new[]
        {
            "XX", // Placeholder for sanctioned countries
            "YY"
        };

        var normalizedName = name.ToUpperInvariant().Trim();
        
        // Check against sanctioned names
        if (sanctionedNames.Any(sanctioned => normalizedName.Contains(sanctioned)))
            return false;

        // Check against sanctioned countries
        if (!string.IsNullOrEmpty(country) && sanctionedCountries.Contains(country.ToUpperInvariant()))
            return false;

        return true;
    }
}
