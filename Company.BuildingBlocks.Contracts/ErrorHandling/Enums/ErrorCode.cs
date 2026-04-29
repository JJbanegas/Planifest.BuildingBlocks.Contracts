namespace Company.BuildingBlocks.Contracts.ErrorHandling.Enums;

/// <summary>
/// Códigos de error estándar utilizados en toda la aplicación.
/// Estos códigos se mapean a HTTP Status Codes específicos.
/// </summary>
public static class ErrorCode
{
    // Errores de Validación (400)
    public const string VALIDATION_ERROR = "VALIDATION_ERROR";
    public const string INVALID_INPUT = "INVALID_INPUT";
    public const string REQUIRED_FIELD_MISSING = "REQUIRED_FIELD_MISSING";
    public const string INVALID_FORMAT = "INVALID_FORMAT";

    // Errores de Autenticación (401)
    public const string UNAUTHORIZED = "UNAUTHORIZED";
    public const string INVALID_CREDENTIALS = "INVALID_CREDENTIALS";
    public const string TOKEN_EXPIRED = "TOKEN_EXPIRED";
    public const string INVALID_TOKEN = "INVALID_TOKEN";

    // Errores de Autorización (403)
    public const string FORBIDDEN = "FORBIDDEN";
    public const string INSUFFICIENT_PERMISSIONS = "INSUFFICIENT_PERMISSIONS";
    public const string ACCESS_DENIED = "ACCESS_DENIED";

    // Errores de Recurso No Encontrado (404)
    public const string NOT_FOUND = "NOT_FOUND";
    public const string RESOURCE_NOT_FOUND = "RESOURCE_NOT_FOUND";
    public const string ENTITY_NOT_FOUND = "ENTITY_NOT_FOUND";

    // Errores de Conflicto (409)
    public const string CONFLICT = "CONFLICT";
    public const string DUPLICATE_ENTRY = "DUPLICATE_ENTRY";
    public const string RESOURCE_ALREADY_EXISTS = "RESOURCE_ALREADY_EXISTS";
    public const string VERSION_MISMATCH = "VERSION_MISMATCH";

    // Errores de Descarga de Archivos (413)
    public const string PAYLOAD_TOO_LARGE = "PAYLOAD_TOO_LARGE";
    public const string REQUEST_ENTITY_TOO_LARGE = "REQUEST_ENTITY_TOO_LARGE";

    // Errores del Servidor Interno (500)
    public const string INTERNAL_SERVER_ERROR = "INTERNAL_SERVER_ERROR";
    public const string UNHANDLED_EXCEPTION = "UNHANDLED_EXCEPTION";
    public const string DATABASE_ERROR = "DATABASE_ERROR";
    public const string EXTERNAL_SERVICE_ERROR = "EXTERNAL_SERVICE_ERROR";

    // Errores de Servicio No Disponible (503)
    public const string SERVICE_UNAVAILABLE = "SERVICE_UNAVAILABLE";
    public const string MAINTENANCE_MODE = "MAINTENANCE_MODE";
    public const string DEPENDENCY_UNAVAILABLE = "DEPENDENCY_UNAVAILABLE";
}

