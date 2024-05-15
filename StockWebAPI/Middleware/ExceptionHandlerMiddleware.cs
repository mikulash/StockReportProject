using System.Net;
using System.Text;
using System.Text.Json;
using GenericBusinessLayer.DTOs.Exception;
using StockBusinessLayer.Exceptions;
using GenericBusinessLayer.Exceptions;

namespace StockWebAPI.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next, IConfiguration config)
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

    private async Task SendHelpAsync(Exception exception, ApiErrorDto apiErrorDto)
    {
        if (exception.GetType() == typeof(InvalidRecordsException)
            || exception.GetType() == typeof(RecordExistenceCheckException))
        {
            string? url = config.GetSection("WebHookReporting").Value;
            if (url is null)
            {
                return;
            }
            var content = new StringContent(JsonSerializer.Serialize(apiErrorDto), Encoding.UTF8, ContentType);
            await new HttpClient().PostAsync(url, content);
        }
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

            await SendHelpAsync(exception, apiErrorDto);
            
            response.StatusCode = apiErrorDto.StatusCode;
            await response.WriteAsync(JsonSerializer.Serialize(apiErrorDto));
        }
    }
}
