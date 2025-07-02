using CompanyTaskManager.Application.Exceptions;
using CompanyTaskManager.Web.Models;
using System.Net;
using System.Text.Json;

namespace CompanyTaskManager.Web.Middleware;

/// <summary>
/// Global exception handling middleware that catches all unhandled exceptions
/// and converts them to standardized error responses.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IWebHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred during request processing");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            TraceId = context.TraceIdentifier,
            Timestamp = DateTime.UtcNow
        };

        // Map different exception types to appropriate HTTP status codes and messages
        switch (exception)
        {
            case NotFoundException notFoundEx:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = notFoundEx.Message;
                _logger.LogWarning("Resource not found: {Message}", notFoundEx.Message);
                break;

            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = validationEx.Message;
                _logger.LogWarning("Validation error: {Message}", validationEx.Message);
                break;

            case Application.Exceptions.UnauthorizedAccessException unauthorizedEx:
                response.StatusCode = (int)HttpStatusCode.Forbidden;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = unauthorizedEx.Message;
                _logger.LogWarning("Unauthorized access attempt: {Message}", unauthorizedEx.Message);
                break;

            case DomainException domainEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = domainEx.Message;
                _logger.LogWarning("Domain exception: {Message}", domainEx.Message);
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.StatusCode = response.StatusCode;
                errorResponse.Message = "An internal server error occurred. Please try again later.";
                
                _logger.LogError(exception, "Unexpected error occurred: {Message}", exception.Message);
                break;
        }

        // Include detailed error information only in Development environment
        if (_environment.IsDevelopment())
        {
            errorResponse.Details = new
            {
                exception.Message,
                exception.StackTrace,
                InnerException = exception.InnerException?.Message
            };
        }

        var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await response.WriteAsync(jsonResponse);
    }
}