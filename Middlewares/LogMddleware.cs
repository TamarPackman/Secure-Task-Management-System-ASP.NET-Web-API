using Serilog;
using System.Diagnostics;

namespace project.Middlewares
{
     public class LogMddleware
    {
        private   RequestDelegate  next;
         private readonly ILogger<LogMddleware> logger;
 
        public LogMddleware(RequestDelegate next,ILogger<LogMddleware> logger)
        {
            this.next = next;
            this.logger = logger;   
        }

        public async Task InvokeAsync(HttpContext context)
    {
         // שלב לפני ביצוע הפעולה (לפני Controller או Middleware הבא)
         
          logger.LogInformation($"Handling request: {context.Request.Method}.{context.Request.Path}");

        await next(context);
            logger.LogInformation($"Finished handling request: {context.Request.Method}.{context.Request.Path}, Response Status: {context.Response.StatusCode}");
      
    }
    }

    public   static partial class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLog(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogMddleware>();
        }
    }


}