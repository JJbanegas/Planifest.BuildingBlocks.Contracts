namespace Company.BuildingBlocks.Contracts.Abstractions;

/// <summary>
/// Contrato base para todas las respuestas de API.
/// </summary>
public interface IApiResponse
{
    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    bool Success { get; }

    /// <summary>
    /// Mensaje general descriptivo de la respuesta.
    /// </summary>
    string? Message { get; }

    /// <summary>
    /// Lista de errores detallados (si aplica).
    /// </summary>
    IEnumerable<IErrorDetail>? Errors { get; }

    /// <summary>
    /// Código de error tipado si la respuesta contiene un error.
    /// </summary>
    string? ErrorCode { get; }

    /// <summary>
    /// Timestamp (RFC3339) de cuando se generó la respuesta.
    /// </summary>
    DateTime Timestamp { get; }

    /// <summary>
    /// ID de rastreo para debugging distribuido (trace ID).
    /// </summary>
    string? TraceId { get; }
}

/// <summary>
/// Extensión de IApiResponse que incluye datos genéricos.
/// </summary>
/// <typeparam name="T">Tipo de datos de la respuesta.</typeparam>
public interface IApiResponse<T> : IApiResponse
{
    /// <summary>
    /// Datos de la respuesta cuando Success es verdadero.
    /// </summary>
    T? Data { get; }
}

