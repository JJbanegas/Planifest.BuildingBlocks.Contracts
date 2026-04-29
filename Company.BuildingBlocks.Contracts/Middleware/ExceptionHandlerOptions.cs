namespace Company.BuildingBlocks.Contracts.Middleware;

/// <summary>
/// Opciones de configuración para el middleware de manejo global de excepciones.
/// </summary>
public class ExceptionHandlerOptions
{
    /// <summary>
    /// Indica si se deben incluir detalles de traza completa en respuestas de error.
    /// Por defecto: false (solo en desarrollo).
    /// </summary>
    public bool IncludeExceptionDetails { get; set; } = false;

    /// <summary>
    /// Indica si se debe incluir el stack trace en respuestas de error.
    /// Por defecto: false.
    /// </summary>
    public bool IncludeStackTrace { get; set; } = false;

    /// <summary>
    /// Mensaje genérico para errores internos no controlados.
    /// </summary>
    public string? GenericErrorMessage { get; set; } = "Ocurrió un error interno del servidor. Por favor, intente más tarde.";

    /// <summary>
    /// Función personalizada para registrar excepciones.
    /// </summary>
    public Func<Exception, Task>? OnExceptionAsync { get; set; }
}

