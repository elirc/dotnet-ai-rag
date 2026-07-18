using System.Net;
using Microsoft.EntityFrameworkCore;

namespace NetAiOps.Api.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var status = ex switch
            {
                UnauthorizedAccessException => HttpStatusCode.Forbidden,
                InvalidOperationException => HttpStatusCode.BadRequest,
                DbUpdateConcurrencyException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };

            if (status == HttpStatusCode.InternalServerError)
            {
                logger.LogError(ex, "Unhandled request failure");
            }
            else
            {
                logger.LogWarning(ex, "Handled request failure");
            }

            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsJsonAsync(new
            {
                title = status.ToString(),
                status = (int)status,
                detail = status == HttpStatusCode.InternalServerError ? "An unexpected error occurred." : ex.Message,
                correlationId = context.Items.TryGetValue(CorrelationAndSecurityHeadersMiddleware.CorrelationHeader, out var value) ? value : null
            });
        }
    }
}
