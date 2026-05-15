using System.Text.Json;
using Messaging.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Messaging.Api.Middleware;

public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            await WriteProblemDetailsAsync(context, exception);
        }
    }

    private async Task WriteProblemDetailsAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception is AppException appException
            ? appException.StatusCode
            : StatusCodes.Status500InternalServerError;

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception occurred.");
        }

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = statusCode == StatusCodes.Status500InternalServerError
                ? "Unexpected server error"
                : "Request failed",
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(json);
    }
}
