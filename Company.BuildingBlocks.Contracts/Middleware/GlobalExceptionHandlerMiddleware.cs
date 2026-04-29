using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;
using Company.BuildingBlocks.Contracts.Models;

namespace Company.BuildingBlocks.Contracts.Middleware;

/// <summary>
/// Middleware para manejo global de excepciones.
/// Captura todas las excepciones no controladas y las convierte en respuestas API estandarizadas.
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
    private readonly ExceptionHandlerOptions _options;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger, ExceptionHandlerOptions? options = null)
    {
        _next = next;
        _logger = logger;
        _options = options ?? new ExceptionHandlerOptions();
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada capturada por GlobalExceptionHandlerMiddleware");
            
            // Registrar excepción si hay un callback
            if (_options.OnExceptionAsync != null)
            {
                try
                {
                    await _options.OnExceptionAsync(ex);
                }
                catch (Exception logEx)
                {
                    _logger.LogError(logEx, "Error al ejecutar callback de logging de excepciones");
                }
            }

            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        ApiResponse response;
        int statusCode;

        // Manejar AppException y sus derivadas
        if (exception is AppException appException)
        {
            statusCode = appException.HttpStatusCode;
            
            response = new ApiResponse
            {
                Success = false,
                Message = appException.Message,
                ErrorCode = appException.ErrorCode,
                Errors = appException.ErrorDetails?.Cast<IErrorDetail>(),
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier
            };

            _logger.LogWarning(
                "AppException capturada - ErrorCode: {ErrorCode}, StatusCode: {StatusCode}, Message: {Message}",
                appException.ErrorCode,
                statusCode,
                appException.Message);
        }
        else
        {
            // Errores no controlados
            statusCode = StatusCodes.Status500InternalServerError;
            
            var errorMessage = _options.IncludeExceptionDetails
                ? exception.Message
                : _options.GenericErrorMessage ?? "Ocurrió un error interno del servidor.";

            var errorDetails = new List<ApiError>();
            
            if (_options.IncludeExceptionDetails || _options.IncludeStackTrace)
            {
                if (_options.IncludeStackTrace && !string.IsNullOrEmpty(exception.StackTrace))
                {
                    errorDetails.Add(new ApiError(
                        "STACK_TRACE",
                        exception.StackTrace,
                        "StackTrace",
                        null,
                        ErrorSeverity.Critical));
                }

                if (exception.InnerException != null)
                {
                    errorDetails.Add(new ApiError(
                        "INNER_EXCEPTION",
                        exception.InnerException.Message,
                        "InnerException",
                        null,
                        ErrorSeverity.Warning));
                }
            }

            response = new ApiResponse
            {
                Success = false,
                Message = errorMessage,
                ErrorCode = ErrorCode.INTERNAL_SERVER_ERROR,
                Errors = errorDetails.Any() ? errorDetails.Cast<IErrorDetail>() : null,
                Timestamp = DateTime.UtcNow,
                TraceId = context.TraceIdentifier
            };

            _logger.LogError(
                "Excepción no controlada - Type: {ExceptionType}, Message: {Message}, TraceId: {TraceId}",
                exception.GetType().Name,
                exception.Message,
                context.TraceIdentifier);
        }

        context.Response.StatusCode = statusCode;

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        var json = JsonSerializer.Serialize(response, jsonOptions);
        return context.Response.WriteAsync(json);
    }
}




