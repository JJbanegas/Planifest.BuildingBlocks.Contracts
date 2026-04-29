namespace Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

/// <summary>
/// Excepción base para todas las excepciones personalizadas de la aplicación.
/// </summary>
public class AppException : Exception
{
    /// <summary>
    /// Código de error tipado asociado a la excepción.
    /// </summary>
    public string ErrorCode { get; set; } = Enums.ErrorCode.INTERNAL_SERVER_ERROR;

    /// <summary>
    /// Nivel de severidad de la excepción.
    /// </summary>
    public string Severity { get; set; } = Enums.ErrorSeverity.Error;

    /// <summary>
    /// Detalles granulares de errores (ej: errores de validación por campo).
    /// </summary>
    public List<Models.ApiError>? ErrorDetails { get; set; }

    /// <summary>
    /// HTTP status code recomendado para esta excepción.
    /// </summary>
    public int HttpStatusCode { get; }

    public AppException(string message, string errorCode = Enums.ErrorCode.INTERNAL_SERVER_ERROR, 
        string severity = Enums.ErrorSeverity.Error, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Severity = severity;
        HttpStatusCode = Enums.HttpStatusMapping.GetStatusCode(errorCode);
    }

    public AppException(string message, List<Models.ApiError>? errorDetails = null,
        string errorCode = Enums.ErrorCode.INTERNAL_SERVER_ERROR,
        string severity = Enums.ErrorSeverity.Error, Exception? innerException = null)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
        Severity = severity;
        ErrorDetails = errorDetails;
        HttpStatusCode = Enums.HttpStatusMapping.GetStatusCode(errorCode);
    }
}

