namespace Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

/// <summary>
/// Excepción para errores de autenticación (401 Unauthorized).
/// </summary>
public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message, Exception? innerException = null)
        : base(message, Enums.ErrorCode.UNAUTHORIZED, Enums.ErrorSeverity.Warning, innerException)
    {
    }

    public UnauthorizedException(Exception? innerException = null)
        : base("Usuario no autenticado.", Enums.ErrorCode.UNAUTHORIZED, Enums.ErrorSeverity.Warning, innerException)
    {
    }
}

