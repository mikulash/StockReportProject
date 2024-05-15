using System.Net;

namespace GenericBusinessLayer.DTOs.Exception;

public class ApiErrorDto(HttpStatusCode code, System.Exception exception)
{
    public int StatusCode { get; set; } = (int)code;
    public string StatusCodeAsString { get; set; } = code.ToString();
    public string ExceptionName { get; set; } = exception.GetType().Name;
    public string ExceptionMessage { get; set; } = exception.Message;
}
