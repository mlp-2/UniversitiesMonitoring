namespace UniversitiesMonitoring.Module;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (Exception exception)
        {
            _logger.LogWarning("Exception thrown due request. Name: {ExceptionName}. Stacktrace:\n{Stacktrace}",
                exception.GetType().Name, exception.StackTrace);
        }
    }
}