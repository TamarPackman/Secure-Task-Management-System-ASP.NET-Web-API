using project.Exceptions;
namespace project.Middlewares
{
     public class errorHandling
    {
        private   RequestDelegate  next;
 
        public errorHandling(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // המשך ל-Middleware הבא בשרשרת
            await next(context);
        }
        catch (NotFoundIdException ex) // טיפול בשגיאות מותאמות אישית
        {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";
        await  context.Response.WriteAsync(ex.Message);
        }
        catch (IdMismatchException ex) // טיפול בשגיאות כלליות
        {
           context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";
        await  context.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex){
            
           context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain";
        await  context.Response.WriteAsync(ex.Message);

        }
    }
    }

    public   static  class MiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<errorHandling>();
        }
    }


}