using Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;
using Company.BuildingBlocks.Contracts.Models;

namespace Company.BuildingBlocks.Contracts.Extensions;

/// <summary>
/// Extensiones para AppException y manejo de excepciones.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Convierte una excepción a una respuesta ApiResponse.
    /// </summary>
    /// <param name="exception">Excepción a convertir.</param>
    /// <returns>ApiResponse con los detalles de la excepción.</returns>
    public static ApiResponse ToApiResponse(this Exception exception)
    {
        if (exception is AppException appEx)
        {
            return ApiResponse.ErrorResponse(appEx.ErrorCode, appEx.Message, appEx.ErrorDetails ?? new List<ApiError>());
        }

        return ApiResponse.ErrorResponse(
            ErrorCode.INTERNAL_SERVER_ERROR,
            exception.Message);
    }

    /// <summary>
    /// Convierte una excepción a una respuesta ApiResponse genérica.
    /// </summary>
    /// <typeparam name="T">Tipo genérico de la respuesta.</typeparam>
    /// <param name="exception">Excepción a convertir.</param>
    /// <returns>ApiResponse{T} con los detalles de la excepción.</returns>
    public static ApiResponse<T> ToApiResponse<T>(this Exception exception)
    {
        if (exception is AppException appEx)
        {
            return ApiResponse<T>.ErrorResponse(appEx.ErrorCode, appEx.Message, appEx.ErrorDetails ?? new List<ApiError>());
        }

        return ApiResponse<T>.ErrorResponse(
            ErrorCode.INTERNAL_SERVER_ERROR,
            exception.Message);
    }

    /// <summary>
    /// Obtiene el HTTP status code asociado a la excepción.
    /// </summary>
    /// <param name="exception">Excepción.</param>
    /// <returns>HTTP status code (por defecto 500).</returns>
    public static int GetHttpStatusCode(this Exception exception)
    {
        if (exception is AppException appEx)
        {
            return appEx.HttpStatusCode;
        }

        return 500;
    }
}




