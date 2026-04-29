# Changelog

Todos los cambios notables de este proyecto se documentarán en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/),
y este proyecto sigue [Semantic Versioning](https://semver.org/lang/es/).

## [1.0.0] - 2024-04-29

### Added

#### Abstractions (Interfaces)
- `IApiResponse` - Contrato base para respuestas API
- `IApiResponse<T>` - Respuesta API genérica con datos
- `IErrorDetail` - Detalle granular de errores
- `IPaginationInfo` - Información de paginación

#### Models
- `ApiResponse` - Respuesta sin datos
- `ApiResponse<T>` - Respuesta genérica tipada
- `ApiPaginatedResponse<T>` - Respuesta paginada
- `ApiError` - Detalle de error individual
- `PaginationInfo` - Metadata de paginación
- `PagedRequest` - Solicitud de paginación

#### Error Handling
- `ErrorCode` - Enumeración de códigos de error estándar
- `ErrorSeverity` - Niveles de severidad de errores
- `HttpStatusMapping` - Mapeo automático de códigos a HTTP status codes

#### Exception Hierarchy
- `AppException` - Excepción base personalizada
- `ValidationException` - Errores de validación (400)
- `NotFoundException` - Recurso no encontrado (404)
- `UnauthorizedException` - No autenticado (401)
- `ForbiddenException` - Acceso denegado (403)
- `ConflictException` - Conflicto (409)
- `BadRequestException` - Solicitud inválida (400)
- `InternalServerException` - Error interno (500)

#### Middleware
- `GlobalExceptionHandlerMiddleware` - Manejo centralizado de excepciones
- `ExceptionHandlerOptions` - Configuración del middleware

#### Extensions
- `ServiceCollectionExtensions` - Registro de servicios DI
  - `AddBuildingBlocks()` - Registro estándar
  - `AddBuildingBlocksForDevelopment()` - Configuración de desarrollo
  - `AddBuildingBlocksForProduction()` - Configuración de producción
- `ApplicationBuilderExtensions` - Configuración de middleware
  - `UseBuildingBlocks()` - Registra el middleware
- `ExceptionExtensions` - Helpers para manejo de excepciones

#### Attributes
- `RequiredIfAttribute` - Validación condicional
- `ValidateModelAttribute` - Marcador de validación
- `ErrorResponseAttribute` - Documentación de errores para Swagger

#### Utilities
- `ErrorCodeHelper` - Mapeo de excepciones a códigos de error
- `ValidationHelper` - Validadores comunes
  - `ValidateRequired()`
  - `ValidateMinLength()`
  - `ValidateMaxLength()`
  - `ValidateRange()`
  - `ValidateEmail()`

#### Documentation
- Archivo README.md completo con guía de uso
- Ejemplos de implementación
- Estructura de respuestas
- Códigos de error documentados
- Mejores prácticas

### Features

✅ Respuestas API estandarizadas y consistentes
✅ Manejo automático de excepciones global
✅ Jerarquía tipada de excepciones
✅ Códigos de error con mapeo HTTP automático
✅ Paginación integrada
✅ Errores granulares por campo
✅ Metadata con timestamp y trace ID
✅ Validadores de entrada
✅ Configuración fluida para DI
✅ Soporte para desarrollo y producción

### Dependencies

- Microsoft.AspNetCore.Http.Abstractions >= 2.2.0
- Microsoft.Extensions.DependencyInjection.Abstractions >= 9.0.0
- Microsoft.Extensions.Logging.Abstractions >= 9.0.0

### Target Framework

- .NET 9.0

---

## Notas Futuras

### v1.1.0 (Planeado)
- Integración con FluentValidation
- Localización de mensajes de error
- Decoradores para validación automática
- Middleware para validación de modelos

### v1.2.0 (Planeado)
- Soporte para rate limiting
- Middleware de correlación de requests
- Métricas y observabilidad integrada
- Caché de respuestas

### v2.0.0 (Planeado)
- Event sourcing helpers
- CQRS patterns
- Repository pattern interfaces
- Unit of work implementation

