
using Serilog;
using System.Diagnostics;
namespace Project.Middlewares;

public class LogMiddleware
{
    private readonly RequestDelegate _next;

    public LogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // התחלת מדידת זמן
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // רישום פרטי הבקשה
            Log.Information("Incoming request: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);

            // המשך לצינור הבקשות
            await _next(context);

            // רישום סטטוס התגובה
            Log.Information("Response: {StatusCode} (Elapsed: {ElapsedMilliseconds}ms)",
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            // רישום שגיאות
            Log.Error(ex, "An unhandled exception occurred while processing the request.");
            throw;
        }
        finally
        {
            stopwatch.Stop();
        }
    }
}

public static partial class MiddlewareExtensions
{
    public static IApplicationBuilder UseLog(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<LogMiddleware>();
    }
}
