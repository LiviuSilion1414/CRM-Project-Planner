namespace PlannerCRM.Server.Middlewares;

public class CustomExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) {
        try {
            await next(context);
        } catch {
            await context.Response.WriteAsync("An error occurred. Try again later.");
        }
    }
}
