namespace Company.BuildingBlocks.Contracts.Abstractions;

/// <summary>
/// Contrato para detalle de errores granulares por propiedad/campo.
/// </summary>
public interface IErrorDetail
{
    /// <summary>
    /// Código de error tipado (ej: VALIDATION_ERROR, NOT_FOUND).
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Mensaje descriptivo del error.
    /// </summary>
    string Message { get; }

    /// <summary>
    /// Campo o propiedad relacionada al error (opcional).
    /// </summary>
    string? Field { get; }

    /// <summary>
    /// Valor recibido que causó el error (opcional).
    /// </summary>
    object? AttemptedValue { get; }

    /// <summary>
    /// Nivel de severidad del error.
    /// </summary>
    string Severity { get; }
}

