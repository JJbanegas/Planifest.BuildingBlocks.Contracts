namespace Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

/// <summary>
/// Excepción para errores de autorización (403 Forbidden).
/// </summary>
public class ForbiddenException : AppException
{
    public ForbiddenException(string message, Exception? innerException = null)
        : base(message, Enums.ErrorCode.FORBIDDEN, Enums.ErrorSeverity.Warning, innerException)
    {
    }

    public ForbiddenException(Exception? innerException = null)
        : base("No tiene permisos para acceder al recurso solicitado.", Enums.ErrorCode.FORBIDDEN, Enums.ErrorSeverity.Warning, innerException)
    {
    }
}

