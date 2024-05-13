using System.Net;
using System.Text.Json;
using StockBusinessLayer.Exceptions;
using GenericBusinessLayer.Exceptions;

namespace StockWebAPI.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    private (HttpStatusCode code, string message) GetResponse(Exception exception)
    {
        HttpStatusCode code;
        switch (exception)
        {
            case NoSuchEntityException<long>
                or NoSuchEntityException<IEnumerable<long>>:
                code = HttpStatusCode.NotFound;
                break;
            case InvalidRecordsException:
                code = HttpStatusCode.BadRequest;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
                break;
        }
        return (code, JsonSerializer.Serialize(exception.Message));
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var (status, message) = GetResponse(exception);
            response.StatusCode = (int)status;
            await response.WriteAsync(message);
        }
    }
}
