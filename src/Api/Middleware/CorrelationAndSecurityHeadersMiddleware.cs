namespace NetAiOps.Api.Middleware;

public sealed class CorrelationAndSecurityHeadersMiddleware(RequestDelegate next, ILogger<CorrelationAndSecurityHeadersMiddleware> logger)
{
    public const string CorrelationHeader = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers.TryGetValue(CorrelationHeader, out var existing)
            ? existing.ToString()
            : Guid.NewGuid().ToString("n");

        context.Items[CorrelationHeader] = correlationId;
        context.Response.Headers[CorrelationHeader] = correlationId;
        context.Response.Headers.XContentTypeOptions = "nosniff";
        context.Response.Headers.XFrameOptions = "DENY";
        context.Response.Headers["Referrer-Policy"] = "no-referrer";

        using (logger.BeginScope(new Dictionary<string, object> { ["CorrelationId"] = correlationId }))
        {
            await next(context);
        }
    }
}

