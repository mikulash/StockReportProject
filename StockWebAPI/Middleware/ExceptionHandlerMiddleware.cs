using System.Net;
using System.Text.Json;
using GenericBusinessLayer.DTOs.Exception;
using StockBusinessLayer.Exceptions;
using GenericBusinessLayer.Exceptions;

namespace StockWebAPI.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private const string ContentType = "application/json";
    
    private ApiErrorDto GetResponse(Exception exception)
    {
        HttpStatusCode code;
        switch (exception)
        {
            case NoSuchEntityException<long>
                or NoSuchEntityException<IEnumerable<long>>:
                code = HttpStatusCode.NotFound;
                break;
            case InvalidRecordsException
                or RecordExistenceCheckException:
                code = HttpStatusCode.BadRequest;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
                break;
        }
        return new(code, exception);
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var response = context.Response;
            response.ContentType = ContentType;

            var apiErrorDto = GetResponse(exception);
            response.StatusCode = apiErrorDto.StatusCode;
            await response.WriteAsync(JsonSerializer.Serialize(apiErrorDto));
        }
    }
}
