namespace Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

/// <summary>
/// Excepciones para errores internos del servidor (500 Internal Server Error).
/// </summary>
public class InternalServerException : AppException
{
    public InternalServerException(string message, Exception? innerException = null)
        : base(message, Enums.ErrorCode.INTERNAL_SERVER_ERROR, Enums.ErrorSeverity.Critical, innerException)
    {
    }

    public InternalServerException(string message, string errorCode, Exception? innerException = null)
        : base(message, errorCode, Enums.ErrorSeverity.Critical, innerException)
    {
    }
}

