namespace Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

/// <summary>
/// Excepción para errores de validación (400 Bad Request).
/// </summary>
public class ValidationException : AppException
{
    public ValidationException(string message, List<Models.ApiError>? errorDetails = null, Exception? innerException = null)
        : base(message, errorDetails, Enums.ErrorCode.VALIDATION_ERROR, Enums.ErrorSeverity.Warning, innerException)
    {
    }
}

