# Company Building Blocks - Contracts

[![NuGet](https://img.shields.io/nuget/v/Company.BuildingBlocks.Contracts)](https://www.nuget.org/packages/Company.BuildingBlocks.Contracts/)

Librería NuGet compartida para microservicios que proporciona configuraciones estándar de excepciones globales, modelos de respuesta API genéricos y contratos reutilizables siguiendo los estándares de la industria.

## Características

✅ **Respuestas API Estandarizadas**
- `ApiResponse` - Respuesta sin datos
- `ApiResponse<T>` - Respuesta genérica tipada
- `ApiPaginatedResponse<T>` - Respuesta con paginación
- Incluye metadata: timestamp, trace ID, errores granulares

✅ **Manejo de Excepciones Global**
- Middleware automático que captura todas las excepciones
- Conversión automática a respuestas API estandarizadas
- Jerarquía de excepciones personalizadas:
  - `ValidationException` (400)
  - `NotFoundException` (404)
  - `UnauthorizedException` (401)
  - `ForbiddenException` (403)
  - `ConflictException` (409)
  - `BadRequestException` (400)
  - `InternalServerException` (500)

✅ **Códigos de Error Tipados**
- Enumeraciones estándar de códigos de error
- Mapeo automático a HTTP status codes
- Mensajes descriptivos por idioma

✅ **Utilidades**
- Validadores de entrada
- Helpers para mapeo de errores
- Atributos personalizados para validación

## Instalación

```
dotnet add package Company.BuildingBlocks.Contracts
```

O en Visual Studio Package Manager:

```
Install-Package Company.BuildingBlocks.Contracts
```

## Uso Rápido

### 1. Configurar en Program.cs

```csharp
using Company.BuildingBlocks.Contracts.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de Building Blocks
builder.Services.AddBuildingBlocks();
// o para desarrollo:
// builder.Services.AddBuildingBlocksForDevelopment();
// o para producción:
// builder.Services.AddBuildingBlocksForProduction();

var app = builder.Build();

// Usar middleware de Building Blocks (DEBE IR AL INICIO)
app.UseBuildingBlocks();

// Resto de middleware...
app.UseRouting();
app.MapControllers();

app.Run();
```

### 2. Usar Respuestas API en Controladores

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        if (id <= 0)
        {
            throw new ValidationException(
                "ID debe ser mayor que cero",
                new List<ApiError>
                {
                    new("INVALID_ID", "El ID debe ser mayor que cero", "id", id)
                });
        }

        var user = new { Id = id, Name = "Juan" };
        
        return Ok(ApiResponse<dynamic>.SuccessResponse(user, "Usuario encontrado"));
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        if (string.IsNullOrEmpty(request.Name))
        {
            throw new ValidationException(
                "El nombre es requerido",
                new List<ApiError>
                {
                    new("NAME_REQUIRED", "El nombre es requerido", "name")
                });
        }

        if (!request.Email.Contains("@"))
        {
            throw new BadRequestException("Email debe contener @");
        }

        // Crear usuario...
        
        return CreatedAtAction(nameof(GetUser), 
            ApiResponse<dynamic>.SuccessResponse(
                new { id = 1 }, 
                "Usuario creado exitosamente"));
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        var user = _userService.GetUser(id);
        
        if (user == null)
        {
            throw new NotFoundException("Usuario", id);
        }

        _userService.Delete(user);
        
        return Ok(ApiResponse.SuccessResponse("Usuario eliminado exitosamente"));
    }
}
```

### 3. Usar Excepciones Personalizadas

```csharp
public class OrderService
{
    public Order GetOrder(int orderId)
    {
        var order = _repository.Find(orderId);
        
        if (order == null)
            throw new NotFoundException(nameof(Order), orderId);
            
        return order;
    }

    public void UpdateOrder(int orderId, UpdateOrderRequest request)
    {
        var order = GetOrder(orderId);
        
        if (order.Status == OrderStatus.Completed)
            throw new ConflictException(
                "El pedido ya fue completado y no puede modificarse",
                "Ya está completado");
        
        order.Update(request);
        _repository.Save(order);
    }

    public void CancelOrder(int orderId)
    {
        var user = _authService.GetCurrentUser();
        
        if (user == null)
            throw new UnauthorizedException();
        
        if (!user.CanCancelOrders)
            throw new ForbiddenException("No tienes permisos para cancelar pedidos");
        
        // Cancelar pedido...
    }
}
```

### 4. Respuestas Paginadas

```csharp
[HttpGet]
public IActionResult GetUsers([FromQuery] PagedRequest request)
{
    request.IsValid(); // Valida y normaliza

    var (users, total) = _userService.GetUsersPaged(
        request.PageNumber, 
        request.PageSize,
        request.SortBy,
        request.SortOrder);

    return Ok(ApiPaginatedResponse<UserDto>.SuccessResponse(
        users,
        request.PageNumber,
        request.PageSize,
        total,
        "Usuarios obtenidos exitosamente"));
}
```

### 5. Manejo de Errores Personalizados

```csharp
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // El middleware de Building Blocks lo manejará automáticamente
            throw;
        }
    }
}
```

## Estructura de Respuesta

### Respuesta Exitosa (200 OK)

```json
{
  "success": true,
  "message": "Operación exitosa",
  "data": {
    "id": 1,
    "name": "Producto A",
    "price": 99.99
  },
  "errors": null,
  "errorCode": null,
  "timestamp": "2024-04-29T10:30:00Z",
  "traceId": "0HN1GJ7IV923K:00000001"
}
```

### Respuesta con Error (400 Bad Request)

```json
{
  "success": false,
  "message": "Errores de validación",
  "data": null,
  "errors": [
    {
      "code": "REQUIRED_FIELD_MISSING",
      "message": "El campo nombre es requerido",
      "field": "name",
      "attemptedValue": null,
      "severity": "Error"
    },
    {
      "code": "INVALID_FORMAT",
      "message": "El email debe ser válido",
      "field": "email",
      "attemptedValue": "invalid-email",
      "severity": "Error"
    }
  ],
  "errorCode": "VALIDATION_ERROR",
  "timestamp": "2024-04-29T10:30:00Z",
  "traceId": "0HN1GJ7IV923K:00000001"
}
```

### Respuesta con Error (404 Not Found)

```json
{
  "success": false,
  "message": "Usuario con ID 999 no fue encontrado.",
  "data": null,
  "errors": null,
  "errorCode": "NOT_FOUND",
  "timestamp": "2024-04-29T10:30:00Z",
  "traceId": "0HN1GJ7IV923K:00000001"
}
```

## Códigos de Error Estándar

| Código | HTTP | Descripción |
|--------|------|-------------|
| `VALIDATION_ERROR` | 400 | Error de validación |
| `INVALID_INPUT` | 400 | Entrada inválida |
| `REQUIRED_FIELD_MISSING` | 400 | Campo requerido faltante |
| `INVALID_FORMAT` | 400 | Formato inválido |
| `UNAUTHORIZED` | 401 | No autenticado |
| `INVALID_CREDENTIALS` | 401 | Credenciales inválidas |
| `TOKEN_EXPIRED` | 401 | Token expirado |
| `INVALID_TOKEN` | 401 | Token inválido |
| `FORBIDDEN` | 403 | Acceso denegado |
| `INSUFFICIENT_PERMISSIONS` | 403 | Permisos insuficientes |
| `NOT_FOUND` | 404 | Recurso no encontrado |
| `CONFLICT` | 409 | Conflicto |
| `DUPLICATE_ENTRY` | 409 | Duplicado |
| `RESOURCE_ALREADY_EXISTS` | 409 | Ya existe |
| `INTERNAL_SERVER_ERROR` | 500 | Error del servidor |
| `DATABASE_ERROR` | 500 | Error de base de datos |
| `EXTERNAL_SERVICE_ERROR` | 502 | Error de servicio externo |
| `SERVICE_UNAVAILABLE` | 503 | Servicio no disponible |

## Configuración

### Para Desarrollo

```csharp
builder.Services.AddBuildingBlocksForDevelopment();
```

Muestra:
- Detalles completos de excepciones
- Stack traces
- Excepciones internas

### Para Producción

```csharp
builder.Services.AddBuildingBlocksForProduction(
    genericErrorMessage: "Error interno. Intente más tarde.");
```

Oculta:
- Detalles técnicos
- Stack traces
- Información sensible

### Personalizado

```csharp
builder.Services.AddBuildingBlocks(options =>
{
    options.IncludeExceptionDetails = false;
    options.IncludeStackTrace = false;
    options.GenericErrorMessage = "Algo salió mal";
    options.OnExceptionAsync = async (ex) =>
    {
        // Registrar en servicio externo
        await _loggingService.LogException(ex);
    };
});
```

## Validadores

```csharp
// Validar campo requerido
ValidationHelper.ValidateRequired(request.Name, "nombre");

// Validar longitud
ValidationHelper.ValidateMinLength(request.Name, 3, "nombre");
ValidationHelper.ValidateMaxLength(request.Name, 100, "nombre");

// Validar rango
ValidationHelper.ValidateRange(request.Age, 0, 150, "edad");

// Validar email
ValidationHelper.ValidateEmail(request.Email);
```

## Respuestas Paginadas

```csharp
public class PagedRequest
{
    public int PageNumber { get; set; } = 1;      // Número de página (basado en 1)
    public int PageSize { get; set; } = 10;       // Registros por página
    public string? SortBy { get; set; }           // Campo para ordenar
    public string SortOrder { get; set; } = "asc"; // asc o desc
}

public class PaginationInfo
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public long TotalRecords { get; set; }
    public int TotalPages { get; }
    public bool HasNextPage { get; }
    public bool HasPreviousPage { get; }
}
```

## Jerarquía de Excepciones

```
Exception
  └── AppException
      ├── ValidationException (400)
      ├── NotFoundException (404)
      ├── UnauthorizedException (401)
      ├── ForbiddenException (403)
      ├── ConflictException (409)
      ├── BadRequestException (400)
      └── InternalServerException (500)
```

## Mejores Prácticas

1. **Siempre registra el middleware primero**
   ```csharp
   app.UseBuildingBlocks(); // Debe ir antes que otros middleware
   app.UseRouting();
   ```

2. **Usa excepciones específicas**
   ```csharp
   // ✅ Correcto
   throw new NotFoundException(nameof(User), userId);
   
   // ❌ Evita excepciones genéricas
   throw new Exception("Usuario no encontrado");
   ```

3. **Incluye detalles de errores granulares**
   ```csharp
   var errors = new List<ApiError>
   {
       new(ErrorCode.INVALID_FORMAT, "Email debe ser válido", "email", email),
       new(ErrorCode.REQUIRED_FIELD_MISSING, "Nombre es requerido", "name")
   };
   throw new ValidationException("Validación fallida", errors);
   ```

4. **Usa trace IDs para debugging**
   ```csharp
   // El middleware incluye automáticamente el trace ID
   // Úsalo en logs para correlacionar errores
   ```

5. **Configura según el ambiente**
   ```csharp
   if (app.Environment.IsDevelopment())
       builder.Services.AddBuildingBlocksForDevelopment();
   else
       builder.Services.AddBuildingBlocksForProduction();
   ```

## Changelog

### v1.0.0 (2024-04-29)
- Release inicial
- Respuestas API genéricas
- Middleware de manejo de excepciones
- Excepciones personalizadas
- Códigos de error tipados
- Validadores y utilidades

## Licencia

MIT

## Autor

Company Development Team

## Soporte

Para reportar problemas o sugerencias, contacta al equipo de desarrollo.

