using Bank.Api.Middleware.Audit;
using System.Diagnostics;

namespace Bank.Api.Middleware;

/// <summary>
/// Middleware for automatic audit logging of HTTP requests and responses.
/// Captures user actions, IP addresses, and request/response data for compliance.
/// Delegates to specialized classes for each responsibility.
/// </summary>
public class AuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditMiddleware> _logger;
    private readonly AuditPathFilter _pathFilter;
    private readonly ContentSanitizer _sanitizer;
    private readonly RequestCapture _requestCapture;
    private readonly ResponseCapture _responseCapture;
    private readonly AuditLogger _auditLogger;

    public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger, ILogger<AuditLogger> auditLoggerLogger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        // Initialize specialized components
        _pathFilter = new AuditPathFilter();
        _sanitizer = new ContentSanitizer();
        _requestCapture = new RequestCapture(_sanitizer);
        _responseCapture = new ResponseCapture(_sanitizer);
        _auditLogger = new AuditLogger(auditLoggerLogger);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip audit logging for excluded paths
        if (_pathFilter.ShouldSkipAuditLogging(context.Request.Path))
        {
            await _next(context);
            return;
        }

        var requestId = Guid.NewGuid().ToString();
        context.Items["RequestId"] = requestId;

        var stopwatch = Stopwatch.StartNew();
        var originalBodyStream = context.Response.Body;

        try
        {
            // Capture request details
            var requestDetails = await _requestCapture.CaptureRequestDetailsAsync(context.Request);
            
            // Create a memory stream to capture response
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            // Execute the request
            await _next(context);

            stopwatch.Stop();

            // Capture response details
            var responseDetails = await _responseCapture.CaptureResponseDetailsAsync(context.Response, responseBodyStream);

            // Copy response back to original stream
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            await responseBodyStream.CopyToAsync(originalBodyStream);

            // Log the audit entry asynchronously
            _ = Task.Run(async () =>
            {
                try
                {
                    await _auditLogger.LogAuditEntryAsync(context, requestDetails, responseDetails, stopwatch.ElapsedMilliseconds, requestId);
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
                    await _auditLogger.LogSecurityEventAsync(context, "MIDDLEWARE_ERROR", ex.Message, requestId);
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
}