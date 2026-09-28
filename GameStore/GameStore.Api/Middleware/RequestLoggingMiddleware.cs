using System.Diagnostics;

namespace GameStore.Api.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<RequestLoggingMiddleware> logger;
    public RequestLoggingMiddleware(RequestDelegate next,ILogger<RequestLoggingMiddleware> logger)
    {
        this.next=next;
        this.logger=logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch=Stopwatch.StartNew();

        logger.LogInformation("Request Started:{Method}{Path}",context.Request.Method,context.Request.Path);
        await next(context);
        logger.LogInformation("{Method}{Path} responded {StatusCode} in {ElapsedTime} ms",context.Request.Method,context.Request.Path,context.Response.StatusCode,stopwatch.ElapsedMilliseconds);
    }
}