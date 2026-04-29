using Microsoft.AspNetCore.Builder;
using Company.BuildingBlocks.Contracts.Middleware;

namespace Company.BuildingBlocks.Contracts.Extensions;

/// <summary>
/// Extensiones de IApplicationBuilder para configurar Building Blocks en la canalización de solicitud.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Agrega el middleware de Building Blocks a la canalización de solicitud.
    /// NOTA: Debe invocarse al INICIO de Program.cs, ANTES de otros middleware.
    /// </summary>
    /// <param name="app">IApplicationBuilder.</param>
    /// <returns>IApplicationBuilder para encadenamiento.</returns>
    public static IApplicationBuilder UseBuildingBlocks(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }

    /// <summary>
    /// Agrega el middleware de Building Blocks con opciones personalizadas.
    /// NOTA: Debe invocarse al INICIO de Program.cs, ANTES de otros middleware.
    /// </summary>
    /// <param name="app">IApplicationBuilder.</param>
    /// <param name="optionsAction">Acción para configurar opciones.</param>
    /// <returns>IApplicationBuilder para encadenamiento.</returns>
    public static IApplicationBuilder UseBuildingBlocks(
        this IApplicationBuilder app,
        Action<ExceptionHandlerOptions> optionsAction)
    {
        var options = new ExceptionHandlerOptions();
        optionsAction.Invoke(options);

        return app.UseMiddleware<GlobalExceptionHandlerMiddleware>(options);
    }
}

