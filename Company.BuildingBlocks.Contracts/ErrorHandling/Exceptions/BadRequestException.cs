namespace Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

/// <summary>
/// Excepción para solicitudes con formato incorrecto (400 Bad Request).
/// </summary>
public class BadRequestException : AppException
{
    public BadRequestException(string message, Exception? innerException = null)
        : base(message, Enums.ErrorCode.INVALID_INPUT, Enums.ErrorSeverity.Warning, innerException)
    {
    }
}

