using System.Net;

namespace Workflow.Domain.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; set; } = (int)HttpStatusCode.BadRequest;
    public string? ErrorCode { get; set; }

    public ApiException(string message, int statusCode = 400, string? errorCode = null)
        : base(message)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }

    public ApiException(string message, Exception inner, int statusCode = 400, string? errorCode = null)
        : base(message, inner)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
    }
}
