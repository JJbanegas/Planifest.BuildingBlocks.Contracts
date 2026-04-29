using Microsoft.AspNetCore.Builder;

namespace Company.BuildingBlocks.Contracts.Middleware;

/// <summary>
/// Extensiones para registrar el middleware de manejo global de excepciones.
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Agrega el middleware global de manejo de excepciones a la canalización de solicitud HTTP.
    /// Debe agregarse al inicio de la canalización para capturar todas las excepciones.
    /// </summary>
    /// <param name="app">IApplicationBuilder.</param>
    /// <param name="options">Opciones de configuración (opcional).</param>
    /// <returns>IApplicationBuilder para encadenamiento.</returns>
    public static IApplicationBuilder UseGlobalExceptionHandler(
        this IApplicationBuilder app,
        ExceptionHandlerOptions? options = null)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>(options ?? new ExceptionHandlerOptions());
    }
}

