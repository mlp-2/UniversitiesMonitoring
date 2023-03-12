namespace UniversitiesMonitoring.Api;

public class HandlingExceptionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HandlingExceptionsMiddleware> _logger;

    public HandlingExceptionsMiddleware(RequestDelegate next, ILogger<HandlingExceptionsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Request. Route: {Route}. Method: {HttpMethod}. From: {Host}:{HostPort}",
            context.Request.Path.Value, context.Request.Method, context.Request.Host.Host, context.Request.Host.Port);
        try
        {
            await _next(context);
        }
        catch (InvalidOperationException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Некорректные данные");
        }
    }
}