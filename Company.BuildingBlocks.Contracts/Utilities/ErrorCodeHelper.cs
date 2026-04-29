namespace Company.BuildingBlocks.Contracts.Utilities;

using Company.BuildingBlocks.Contracts.ErrorHandling.Enums;

/// <summary>
/// Utilidades para mapeo de excepciones a códigos de error.
/// </summary>
public static class ErrorCodeHelper
{
    private static readonly Dictionary<Type, string> ExceptionTypeToErrorCode = new()
    {
        { typeof(ArgumentNullException), ErrorCode.REQUIRED_FIELD_MISSING },
        { typeof(ArgumentException), ErrorCode.INVALID_INPUT },
        { typeof(InvalidOperationException), ErrorCode.INVALID_INPUT },
        { typeof(FormatException), ErrorCode.INVALID_FORMAT },
        { typeof(OverflowException), ErrorCode.INVALID_INPUT },
        { typeof(UnauthorizedAccessException), ErrorCode.UNAUTHORIZED },
    };

    /// <summary>
    /// Obtiene el código de error correspondiente a un tipo de excepción.
    /// </summary>
    /// <param name="exceptionType">Tipo de excepción.</param>
    /// <returns>Código de error estándar.</returns>
    public static string GetErrorCodeForException(Type exceptionType)
    {
        return ExceptionTypeToErrorCode.TryGetValue(exceptionType, out var errorCode)
            ? errorCode
            : ErrorCode.INTERNAL_SERVER_ERROR;
    }

    /// <summary>
    /// Obtiene el código de error correspondiente a una excepción.
    /// </summary>
    /// <param name="exception">Excepción.</param>
    /// <returns>Código de error estándar.</returns>
    public static string GetErrorCodeForException(Exception exception)
    {
        return GetErrorCodeForException(exception.GetType());
    }
}


