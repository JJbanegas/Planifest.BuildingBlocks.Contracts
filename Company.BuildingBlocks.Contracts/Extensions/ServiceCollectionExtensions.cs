using Microsoft.Extensions.DependencyInjection;
using Company.BuildingBlocks.Contracts.Middleware;

namespace Company.BuildingBlocks.Contracts.Extensions;

/// <summary>
/// Extensiones de IServiceCollection para registrar servicios de Building Blocks.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registra los servicios de Building Blocks en la inyección de dependencias.
    /// </summary>
    /// <param name="services">IServiceCollection.</param>
    /// <param name="optionsAction">Acción para configurar opciones (opcional).</param>
    /// <returns>IServiceCollection para encadenamiento.</returns>
    public static IServiceCollection AddBuildingBlocks(
        this IServiceCollection services,
        Action<ExceptionHandlerOptions>? optionsAction = null)
    {
        var options = new ExceptionHandlerOptions();
        optionsAction?.Invoke(options);

        services.AddSingleton(options);

        return services;
    }

    /// <summary>
    /// Registra los servicios de Building Blocks con opciones de desarrollo.
    /// Incluye detalles de excepciones y stack traces para debugging.
    /// </summary>
    /// <param name="services">IServiceCollection.</param>
    /// <returns>IServiceCollection para encadenamiento.</returns>
    public static IServiceCollection AddBuildingBlocksForDevelopment(
        this IServiceCollection services)
    {
        var options = new ExceptionHandlerOptions
        {
            IncludeExceptionDetails = true,
            IncludeStackTrace = true
        };

        services.AddSingleton(options);

        return services;
    }

    /// <summary>
    /// Registra los servicios de Building Blocks con opciones de producción.
    /// Oculta detalles técnicos en respuestas de error.
    /// </summary>
    /// <param name="services">IServiceCollection.</param>
    /// <param name="genericErrorMessage">Mensaje genérico para errores (opcional).</param>
    /// <returns>IServiceCollection para encadenamiento.</returns>
    public static IServiceCollection AddBuildingBlocksForProduction(
        this IServiceCollection services,
        string? genericErrorMessage = null)
    {
        var options = new ExceptionHandlerOptions
        {
            IncludeExceptionDetails = false,
            IncludeStackTrace = false,
            GenericErrorMessage = genericErrorMessage ?? "Ocurrió un error interno del servidor. Por favor, intente más tarde."
        };

        services.AddSingleton(options);

        return services;
    }
}

