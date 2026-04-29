namespace Company.BuildingBlocks.Contracts.ErrorHandling.Enums;

/// <summary>
/// Mapeo entre códigos de error y HTTP status codes.
/// </summary>
public static class HttpStatusMapping
{
    private static readonly Dictionary<string, int> ErrorCodeToStatusCode = new()
    {
        // Errores de Validación (400)
        { ErrorCode.VALIDATION_ERROR, 400 },
        { ErrorCode.INVALID_INPUT, 400 },
        { ErrorCode.REQUIRED_FIELD_MISSING, 400 },
        { ErrorCode.INVALID_FORMAT, 400 },

        // Errores de Autenticación (401)
        { ErrorCode.UNAUTHORIZED, 401 },
        { ErrorCode.INVALID_CREDENTIALS, 401 },
        { ErrorCode.TOKEN_EXPIRED, 401 },
        { ErrorCode.INVALID_TOKEN, 401 },

        // Errores de Autorización (403)
        { ErrorCode.FORBIDDEN, 403 },
        { ErrorCode.INSUFFICIENT_PERMISSIONS, 403 },
        { ErrorCode.ACCESS_DENIED, 403 },

        // Errores de Recurso No Encontrado (404)
        { ErrorCode.NOT_FOUND, 404 },
        { ErrorCode.RESOURCE_NOT_FOUND, 404 },
        { ErrorCode.ENTITY_NOT_FOUND, 404 },

        // Errores de Conflicto (409)
        { ErrorCode.CONFLICT, 409 },
        { ErrorCode.DUPLICATE_ENTRY, 409 },
        { ErrorCode.RESOURCE_ALREADY_EXISTS, 409 },
        { ErrorCode.VERSION_MISMATCH, 409 },

        // Errores de Tamaño (413)
        { ErrorCode.PAYLOAD_TOO_LARGE, 413 },
        { ErrorCode.REQUEST_ENTITY_TOO_LARGE, 413 },

        // Errores del Servidor Interno (500)
        { ErrorCode.INTERNAL_SERVER_ERROR, 500 },
        { ErrorCode.UNHANDLED_EXCEPTION, 500 },
        { ErrorCode.DATABASE_ERROR, 500 },
        { ErrorCode.EXTERNAL_SERVICE_ERROR, 502 },

        // Errores de Servicio No Disponible (503)
        { ErrorCode.SERVICE_UNAVAILABLE, 503 },
        { ErrorCode.MAINTENANCE_MODE, 503 },
        { ErrorCode.DEPENDENCY_UNAVAILABLE, 503 }
    };

    /// <summary>
    /// Obtiene el HTTP status code correspondiente a un código de error.
    /// </summary>
    /// <param name="errorCode">Código de error.</param>
    /// <returns>HTTP status code (por defecto 500 si no se encuentra mapa).</returns>
    public static int GetStatusCode(string? errorCode)
    {
        if (string.IsNullOrEmpty(errorCode))
            return 500;

        return ErrorCodeToStatusCode.TryGetValue(errorCode, out var statusCode)
            ? statusCode
            : 500;
    }
}

