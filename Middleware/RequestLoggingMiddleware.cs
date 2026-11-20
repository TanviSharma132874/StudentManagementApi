using System.Diagnostics;

namespace StudentManagementApi.Middleware;

/// <summary>Logs every request (method, path, status code, time taken) and adds an X-Response-Time-Ms header.</summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        // Headers must be added before the response starts, so use OnStarting.
        context.Response.OnStarting(() =>
        {
            context.Response.Headers["X-Response-Time-Ms"] = stopwatch.ElapsedMilliseconds.ToString();
            return Task.CompletedTask;
        });

        await _next(context);   // call the next middleware in the pipeline

        stopwatch.Stop();
        _logger.LogInformation(
            "{Method} {Path} responded {StatusCode} in {Elapsed} ms",
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds);
    }
}
