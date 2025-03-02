
namespace Project.Middlewares;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate _next)
    {
        next = _next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context); // מעביר את הבקשה הלאה בצינור
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);   // מטפל בשגיאה
        }
    }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}