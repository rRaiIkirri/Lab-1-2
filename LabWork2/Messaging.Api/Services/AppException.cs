namespace Messaging.Api.Services;

public sealed class AppException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;

    public static AppException BadRequest(string message) =>
        new(message, StatusCodes.Status400BadRequest);

    public static AppException NotFound(string message) =>
        new(message, StatusCodes.Status404NotFound);

    public static AppException Conflict(string message) =>
        new(message, StatusCodes.Status409Conflict);
}
