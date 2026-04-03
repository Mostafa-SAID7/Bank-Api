namespace Bank.Api.Middleware.Audit;

/// <summary>
/// Determines which paths should be excluded from audit logging
/// </summary>
public class AuditPathFilter
{
    private readonly HashSet<string> _excludedPaths;

    public AuditPathFilter()
    {
        // Paths to exclude from audit logging (health checks, static files, etc.)
        _excludedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "/health",
            "/healthz",
            "/ready",
            "/live",
            "/metrics",
            "/swagger",
            "/favicon.ico"
        };
    }

    public bool ShouldSkipAuditLogging(PathString path)
    {
        return _excludedPaths.Any(excludedPath => 
            path.StartsWithSegments(excludedPath, StringComparison.OrdinalIgnoreCase));
    }
}
