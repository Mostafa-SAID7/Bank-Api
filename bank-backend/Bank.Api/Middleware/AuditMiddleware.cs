using Bank.Application.Interfaces;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Bank.Api.Middleware;

/// <summary>
/// Middleware for automatic audit logging of HTTP requests and responses.
/// Captures user actions, IP addresses, and request/response data for compliance.
/// </summary>
public class AuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditMiddleware> _logger;
    private readonly HashSet<string> _excludedPaths;

    public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
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

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip audit logging for excluded paths
        if (ShouldSkipAuditLogging(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var originalBodyStream = context.Response.Body;

        try
        {
            // Capture request details
            var requestDetails = await CaptureRequestDetailsAsync(context.Request);
            
            // Create a memory stream to capture response
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            // Execute the request
            await _next(context);

            stopwatch.Stop();

            // Capture response details
            var responseDetails = await CaptureResponseDetailsAsync(context.Response, responseBodyStream);

            // Copy response back to original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);

            // Log the audit entry asynchronously
            _ = Task.Run(async () =>
            {
                try
                {
                    await LogAuditEntryAsync(context, requestDetails, responseDetails, stopwatch.ElapsedMilliseconds, requestId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to log audit entry for request {RequestId}", requestId);
                }
            });
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(ex, "Error in audit middleware for request {RequestId}", requestId);
            
            // Log the error as a security event
            _ = Task.Run(async () =>
            {
                try
                {
                    await LogSecurityEventAsync(context, "MIDDLEWARE_ERROR", ex.Message, requestId);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Failed to log security event for request {RequestId}", requestId);
                }
            });
            
            throw;
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }

    private bool ShouldSkipAuditLogging(PathString path)
    {
        return _excludedPaths.Any(excludedPath => 
            path.StartsWithSegments(excludedPath, StringComparison.OrdinalIgnoreCase));
    }

    private async Task<string> CaptureRequestDetailsAsync(HttpRequest request)
    {
        var requestDetails = new
        {
            Method = request.Method,
            Path = request.Path.Value,
            QueryString = request.QueryString.Value,
            Headers = request.Headers.Where(h => !IsSecuritySensitiveHeader(h.Key))
                .ToDictionary(h => h.Key, h => h.Value.ToString()),
            ContentType = request.ContentType,
            ContentLength = request.ContentLength,
            Body = await GetRequestBodyAsync(request)
        };

        return JsonSerializer.Serialize(requestDetails, new JsonSerializerOptions 
        { 
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private async Task<string> GetRequestBodyAsync(HttpRequest request)
    {
        if (request.ContentLength == 0 || request.ContentLength > 10240) // Skip large bodies > 10KB
            return string.Empty;

        if (!IsLoggableContentType(request.ContentType))
            return "[Binary Content]";

        request.EnableBuffering();
        var buffer = new byte[Convert.ToInt32(request.ContentLength ?? 0)];
        await request.Body.ReadAsync(buffer, 0, buffer.Length);
        request.Body.Position = 0;

        var body = Encoding.UTF8.GetString(buffer);
        
        // Sanitize sensitive data
        return SanitizeSensitiveData(body);
    }

    private async Task<string> CaptureResponseDetailsAsync(HttpResponse response, MemoryStream responseBodyStream)
    {
        var responseBody = string.Empty;
        
        if (responseBodyStream.Length > 0 && responseBodyStream.Length <= 10240) // Skip large responses > 10KB
        {
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var buffer = new byte[responseBodyStream.Length];
            await responseBodyStream.ReadAsync(buffer, 0, buffer.Length);
            
            if (IsLoggableContentType(response.ContentType))
            {
                responseBody = Encoding.UTF8.GetString(buffer);
                responseBody = SanitizeSensitiveData(responseBody);
            }
            else
            {
                responseBody = "[Binary Content]";
            }
        }

        var responseDetails = new
        {
            StatusCode = response.StatusCode,
            Headers = response.Headers.Where(h => !IsSecuritySensitiveHeader(h.Key))
                .ToDictionary(h => h.Key, h => h.Value.ToString()),
            ContentType = response.ContentType,
            ContentLength = responseBodyStream.Length,
            Body = responseBody
        };

        return JsonSerializer.Serialize(responseDetails, new JsonSerializerOptions 
        { 
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private async Task LogAuditEntryAsync(
        HttpContext context, 
        string requestDetails, 
        string responseDetails, 
        long durationMs,
        string requestId)
    {
        var auditService = context.RequestServices.GetService<IAuditLogService>();
        if (auditService == null) return;

        var userId = GetUserId(context);
        var ipAddress = GetClientIpAddress(context);
        var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
        var sessionId = context.Session?.Id;

        var action = $"{context.Request.Method} {context.Request.Path}";
        var additionalData = JsonSerializer.Serialize(new
        {
            Request = requestDetails,
            Response = responseDetails,
            DurationMs = durationMs,
            StatusCode = context.Response.StatusCode
        });

        if (context.Response.StatusCode >= 400)
        {
            // Log as security event for error responses
            await auditService.LogSecurityEventAsync(
                userId,
                action,
                "HTTP_REQUEST",
                requestId,
                ipAddress,
                userAgent,
                additionalData,
                sessionId,
                requestId);
        }
        else
        {
            // Log as user action for successful requests
            await auditService.LogUserActionAsync(
                userId ?? Guid.Empty,
                action,
                "HTTP_REQUEST",
                requestId,
                requestDetails,
                responseDetails,
                ipAddress,
                userAgent,
                sessionId,
                requestId);
        }
    }

    private async Task LogSecurityEventAsync(HttpContext context, string action, string additionalData, string requestId)
    {
        var auditService = context.RequestServices.GetService<IAuditLogService>();
        if (auditService == null) return;

        var userId = GetUserId(context);
        var ipAddress = GetClientIpAddress(context);
        var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault();
        var sessionId = context.Session?.Id;

        await auditService.LogSecurityEventAsync(
            userId,
            action,
            "SECURITY_EVENT",
            requestId,
            ipAddress,
            userAgent,
            additionalData,
            sessionId,
            requestId);
    }

    private Guid? GetUserId(HttpContext context)
    {
        var userIdClaim = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }

    private string? GetClientIpAddress(HttpContext context)
    {
        // Check for forwarded IP first (load balancer/proxy scenarios)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
        {
            return realIp;
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }

    private bool IsSecuritySensitiveHeader(string headerName)
    {
        var sensitiveHeaders = new[]
        {
            "Authorization",
            "Cookie",
            "Set-Cookie",
            "X-API-Key",
            "X-Auth-Token"
        };

        return sensitiveHeaders.Contains(headerName, StringComparer.OrdinalIgnoreCase);
    }

    private bool IsLoggableContentType(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType))
            return false;

        var loggableTypes = new[]
        {
            "application/json",
            "application/xml",
            "text/plain",
            "text/xml",
            "application/x-www-form-urlencoded"
        };

        return loggableTypes.Any(type => 
            contentType.StartsWith(type, StringComparison.OrdinalIgnoreCase));
    }

    private string SanitizeSensitiveData(string content)
    {
        if (string.IsNullOrEmpty(content))
            return content;

        // List of sensitive field patterns to sanitize
        var sensitivePatterns = new[]
        {
            @"""password""\s*:\s*""[^""]*""",
            @"""Password""\s*:\s*""[^""]*""",
            @"""pin""\s*:\s*""[^""]*""",
            @"""Pin""\s*:\s*""[^""]*""",
            @"""ssn""\s*:\s*""[^""]*""",
            @"""socialSecurityNumber""\s*:\s*""[^""]*""",
            @"""creditCardNumber""\s*:\s*""[^""]*""",
            @"""accountNumber""\s*:\s*""[^""]*"""
        };

        foreach (var pattern in sensitivePatterns)
        {
            content = System.Text.RegularExpressions.Regex.Replace(
                content, 
                pattern, 
                match => match.Value.Substring(0, match.Value.IndexOf(':') + 1) + " \"[REDACTED]\"",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        }

        return content;
    }
}