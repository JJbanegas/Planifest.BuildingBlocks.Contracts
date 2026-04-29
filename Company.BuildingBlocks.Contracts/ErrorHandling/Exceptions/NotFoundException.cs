namespace Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

/// <summary>
/// Excepción para cuando no se encuentra un recurso (404 Not Found).
/// </summary>
public class NotFoundException : AppException
{
    public NotFoundException(string message, Exception? innerException = null)
        : base(message, Enums.ErrorCode.NOT_FOUND, Enums.ErrorSeverity.Warning, innerException)
    {
    }

    public NotFoundException(string resourceName, object resourceId, Exception? innerException = null)
        : base($"{resourceName} con ID {resourceId} no fue encontrado.", Enums.ErrorCode.NOT_FOUND, Enums.ErrorSeverity.Warning, innerException)
    {
    }
}

