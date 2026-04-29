namespace Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

/// <summary>
/// Excepción para conflictos (409 Conflict).
/// </summary>
public class ConflictException : AppException
{
    public ConflictException(string message, Exception? innerException = null)
        : base(message, Enums.ErrorCode.CONFLICT, Enums.ErrorSeverity.Warning, innerException)
    {
    }

    public ConflictException(string resourceName, string reason, Exception? innerException = null)
        : base($"Conflicto en {resourceName}: {reason}", Enums.ErrorCode.CONFLICT, Enums.ErrorSeverity.Warning, innerException)
    {
    }
}

