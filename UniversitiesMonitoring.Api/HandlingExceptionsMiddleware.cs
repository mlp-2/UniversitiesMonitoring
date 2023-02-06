namespace UniversitiesMonitoring.Api;

public class HandlingExceptionsMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (InvalidOperationException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Некорректные данные");
        }
    }
}