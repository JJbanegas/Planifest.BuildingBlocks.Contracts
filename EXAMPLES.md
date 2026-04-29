# Ejemplos de Uso - Company Building Blocks Contracts

Este archivo contiene ejemplos prácticos de cómo usar la librería en tus microservicios.

## 1. Configuración Inicial en Program.cs

```csharp
using Company.BuildingBlocks.Contracts.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Registrar servicios de Building Blocks
// Opción 1: Configuración automática según el ambiente
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddBuildingBlocksForDevelopment();
}
else
{
    builder.Services.AddBuildingBlocksForProduction("Error interno. Por favor intenta más tarde.");
}

// Opción 2: Configuración personalizada
// builder.Services.AddBuildingBlocks(options =>
// {
//     options.IncludeExceptionDetails = false;
//     options.IncludeStackTrace = false;
//     options.OnExceptionAsync = async (ex) =>
//     {
//         // Registrar en un servicio externo
//         await logger.LogCriticalAsync(ex);
//     };
// });

var app = builder.Build();

// Agregar middleware de Building Blocks (DEBE IR AL INICIO)
app.UseBuildingBlocks();

// Otros middleware...
app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
```

## 2. Controlador con Respuestas API

```csharp
using Company.BuildingBlocks.Contracts.Models;
using Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;
using Company.BuildingBlocks.Contracts.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace YourMicroservice.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Obtener un usuario por ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public IActionResult GetUser(int id)
    {
        try
        {
            // Validar entrada
            ValidationHelper.ValidateRange(id, 1, int.MaxValue, "id");

            var user = _userService.GetUser(id);
            
            if (user == null)
                throw new NotFoundException(nameof(User), id);

            var userDto = new UserDto 
            { 
                Id = user.Id, 
                Name = user.Name, 
                Email = user.Email 
            };

            return Ok(ApiResponse<UserDto>.SuccessResponse(
                userDto,
                "Usuario obtenido exitosamente"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario");
            throw; // El middleware lo capturará
        }
    }

    /// <summary>
    /// Crear un nuevo usuario
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public IActionResult CreateUser([FromBody] CreateUserRequest request)
    {
        // Validar entrada
        var validationErrors = new List<ApiError>();

        if (string.IsNullOrWhiteSpace(request.Name))
            validationErrors.Add(new("REQUIRED_FIELD", "El nombre es requerido", "name"));

        if (string.IsNullOrWhiteSpace(request.Email))
            validationErrors.Add(new("REQUIRED_FIELD", "El email es requerido", "email"));
        else if (!request.Email.Contains("@"))
            validationErrors.Add(new("INVALID_EMAIL", "El email debe ser válido", "email", request.Email));

        if (validationErrors.Any())
            throw new ValidationException("Validación fallida", validationErrors);

        // Verificar si el email ya existe
        if (_userService.EmailExists(request.Email))
            throw new ConflictException("El email ya está registrado");

        // Crear usuario
        var user = new User 
        { 
            Name = request.Name, 
            Email = request.Email 
        };

        var createdUser = _userService.CreateUser(user);

        var userDto = new UserDto 
        { 
            Id = createdUser.Id, 
            Name = createdUser.Name, 
            Email = createdUser.Email 
        };

        return CreatedAtAction(
            nameof(GetUser),
            new { id = createdUser.Id },
            ApiResponse<UserDto>.SuccessResponse(
                userDto,
                "Usuario creado exitosamente"));
    }

    /// <summary>
    /// Actualizar un usuario
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public IActionResult UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var user = _userService.GetUser(id);
        
        if (user == null)
            throw new NotFoundException(nameof(User), id);

        // Validar que el nombre no esté vacío
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ValidationException(
                "El nombre no puede estar vacío",
                new List<ApiError>
                {
                    new("EMPTY_NAME", "El nombre es requerido", "name")
                });

        user.Name = request.Name;
        
        if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
        {
            if (_userService.EmailExists(request.Email))
                throw new ConflictException("El email ya está en uso por otro usuario");
            
            user.Email = request.Email;
        }

        _userService.UpdateUser(user);

        var userDto = new UserDto 
        { 
            Id = user.Id, 
            Name = user.Name, 
            Email = user.Email 
        };

        return Ok(ApiResponse<UserDto>.SuccessResponse(
            userDto,
            "Usuario actualizado exitosamente"));
    }

    /// <summary>
    /// Eliminar un usuario
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public IActionResult DeleteUser(int id)
    {
        var user = _userService.GetUser(id);
        
        if (user == null)
            throw new NotFoundException(nameof(User), id);

        _userService.DeleteUser(id);

        return Ok(ApiResponse.SuccessResponse("Usuario eliminado exitosamente"));
    }

    /// <summary>
    /// Obtener usuarios con paginación
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiPaginatedResponse<UserDto>), StatusCodes.Status200OK)]
    public IActionResult GetUsers([FromQuery] PagedRequest request)
    {
        request.IsValid(); // Valida y normaliza

        var (users, total) = _userService.GetUsersPaged(
            request.PageNumber,
            request.PageSize,
            request.SortBy ?? "name",
            request.SortOrder);

        var userDtos = users.Select(u => new UserDto 
        { 
            Id = u.Id, 
            Name = u.Name, 
            Email = u.Email 
        }).ToList();

        return Ok(ApiPaginatedResponse<UserDto>.SuccessResponse(
            userDtos,
            request.PageNumber,
            request.PageSize,
            total,
            "Usuarios obtenidos exitosamente"));
    }
}
```

## 3. Servicio con Manejo de Excepciones

```csharp
using Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

namespace YourMicroservice.Business.Services;

public interface IOrderService
{
    Order GetOrder(int orderId);
    void CancelOrder(int orderId);
    void UpdateOrderStatus(int orderId, OrderStatus status);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IAuthService _authService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository repository,
        IAuthService authService,
        ILogger<OrderService> logger)
    {
        _repository = repository;
        _authService = authService;
        _logger = logger;
    }

    public Order GetOrder(int orderId)
    {
        var order = _repository.GetById(orderId);
        
        if (order == null)
        {
            _logger.LogWarning("Orden no encontrada: {OrderId}", orderId);
            throw new NotFoundException(nameof(Order), orderId);
        }

        return order;
    }

    public void CancelOrder(int orderId)
    {
        // Verificar autenticación
        var user = _authService.GetCurrentUser();
        if (user == null)
            throw new UnauthorizedException("Usuario no autenticado");

        // Verificar permisos
        if (!user.CanCancelOrders)
            throw new ForbiddenException("No tienes permisos para cancelar pedidos");

        var order = GetOrder(orderId);

        // Verificar estado
        if (order.Status != OrderStatus.Pending)
            throw new ConflictException(
                "El pedido ya fue procesado y no puede ser cancelado",
                $"Estado actual: {order.Status}");

        order.Status = OrderStatus.Cancelled;
        _repository.Update(order);

        _logger.LogInformation("Pedido cancelado: {OrderId}", orderId);
    }

    public void UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        var order = GetOrder(orderId);

        // Validar transición de estado válida
        if (!IsValidStatusTransition(order.Status, newStatus))
        {
            throw new BadRequestException(
                $"No se puede cambiar de {order.Status} a {newStatus}");
        }

        order.Status = newStatus;
        _repository.Update(order);

        _logger.LogInformation(
            "Estado del pedido actualizado: {OrderId} -> {NewStatus}",
            orderId, newStatus);
    }

    private bool IsValidStatusTransition(OrderStatus from, OrderStatus to)
    {
        return (from, to) switch
        {
            (OrderStatus.Pending, OrderStatus.Processing) => true,
            (OrderStatus.Processing, OrderStatus.Shipped) => true,
            (OrderStatus.Shipped, OrderStatus.Delivered) => true,
            (OrderStatus.Pending, OrderStatus.Cancelled) => true,
            _ => false
        };
    }
}
```

## 4. DTOs y Modelos de Solicitud

```csharp
namespace YourMicroservice.Api.Models;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class CreateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class UpdateUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
}
```

## 5. Ejemplo de Respuesta Exitosa (200 OK)

```json
{
  "success": true,
  "message": "Usuario obtenido exitosamente",
  "data": {
    "id": 1,
    "name": "Juan Pérez",
    "email": "juan@example.com"
  },
  "errors": null,
  "errorCode": null,
  "timestamp": "2024-04-29T10:30:15.234Z",
  "traceId": "0HN1GJ7IV923K:00000001"
}
```

## 6. Ejemplo de Respuesta con Error (400 Bad Request)

```json
{
  "success": false,
  "message": "Validación fallida",
  "data": null,
  "errors": [
    {
      "code": "REQUIRED_FIELD",
      "message": "El nombre es requerido",
      "field": "name",
      "attemptedValue": null,
      "severity": "Error"
    },
    {
      "code": "INVALID_EMAIL",
      "message": "El email debe ser válido",
      "field": "email",
      "attemptedValue": "invalid-email",
      "severity": "Error"
    }
  ],
  "errorCode": "VALIDATION_ERROR",
  "timestamp": "2024-04-29T10:30:15.234Z",
  "traceId": "0HN1GJ7IV923K:00000001"
}
```

## 7. Ejemplo de Respuesta Paginada (200 OK)

```json
{
  "success": true,
  "message": "Usuarios obtenidos exitosamente",
  "data": [
    {
      "id": 1,
      "name": "Juan Pérez",
      "email": "juan@example.com"
    },
    {
      "id": 2,
      "name": "María García",
      "email": "maria@example.com"
    }
  ],
  "pagination": {
    "pageNumber": 1,
    "pageSize": 10,
    "totalRecords": 52,
    "totalPages": 6,
    "hasNextPage": true,
    "hasPreviousPage": false
  },
  "errors": null,
  "errorCode": null,
  "timestamp": "2024-04-29T10:30:15.234Z",
  "traceId": "0HN1GJ7IV923K:00000001"
}
```

## 8. Validadores Personalizados

```csharp
using Company.BuildingBlocks.Contracts.Utilities;
using Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;

namespace YourMicroservice.Business.Validators;

public class OrderValidator
{
    public static void ValidateCreateOrderRequest(CreateOrderRequest request)
    {
        var errors = new List<ApiError>();

        try
        {
            ValidationHelper.ValidateRequired(request.CustomerId, "customerId");
        }
        catch (ValidationException ex)
        {
            errors.AddRange(ex.ErrorDetails ?? new());
        }

        try
        {
            ValidationHelper.ValidateRequired(request.Items, "items");
        }
        catch (ValidationException ex)
        {
            errors.AddRange(ex.ErrorDetails ?? new());
        }

        if (request.Items?.Count == 0)
            errors.Add(new("EMPTY_ITEMS", "Debe incluir al menos un producto", "items"));

        if (errors.Any())
            throw new ValidationException("Validación del pedido fallida", errors);
    }
}
```

## 9. Instalación en Microservicios

Para instalar la librería en tus microservicios:

```bash
dotnet add package Company.BuildingBlocks.Contracts
```

O actualizar en tu `.csproj`:

```xml
<ItemGroup>
  <PackageReference Include="Company.BuildingBlocks.Contracts" Version="1.0.0" />
</ItemGroup>
```

Luego ejecuta:

```bash
dotnet restore
```

## 10. Testing Unitario

```csharp
using Xunit;
using Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;
using Company.BuildingBlocks.Contracts.Models;

namespace YourMicroservice.Tests;

public class UserControllerTests
{
    [Fact]
    public void CreateUser_WithInvalidEmail_ThrowsValidationException()
    {
        // Arrange
        var controller = new UsersController(userService, logger);
        var request = new CreateUserRequest 
        { 
            Name = "Test", 
            Email = "invalid-email" 
        };

        // Act & Assert
        var exception = Assert.Throws<ValidationException>(() =>
            controller.CreateUser(request));

        Assert.NotEmpty(exception.ErrorDetails);
        Assert.Equal("VALIDATION_ERROR", exception.ErrorCode);
    }

    [Fact]
    public void GetUser_WithNonExistentId_ThrowsNotFoundException()
    {
        // Arrange
        var controller = new UsersController(userService, logger);

        // Act & Assert
        var exception = Assert.Throws<NotFoundException>(() =>
            controller.GetUser(999));

        Assert.Equal("NOT_FOUND", exception.ErrorCode);
    }
}
```

---

## Notas Importantes

- ✅ **El middleware debe registrarse al inicio** de la canalización en `Program.cs`
- ✅ **Usa excepciones específicas** en lugar de excepciones genéricas
- ✅ **Incluye detalles granulares** en ErrorDetails para validaciones complejas
- ✅ **Proporciona trace IDs** para debugging distribuido
- ✅ **Configura según el ambiente** (desarrollo vs producción)
- ✅ **Valida siempre las entradas** antes de procesar

Para más información, consulta el archivo README.md

