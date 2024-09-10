namespace PlannerCRM.Server.Middlewares;

public class CustomExceptionHandlerMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";

        var responseMessage = new
        {
            Message = "Something went wrong. Please try again later."
        };

        await httpContext.Response.WriteAsJsonAsync(responseMessage, cancellationToken);

        return true;
    }
}