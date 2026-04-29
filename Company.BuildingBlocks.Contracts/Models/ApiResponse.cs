namespace Company.BuildingBlocks.Contracts.Models;

/// <summary>
/// Respuesta API estándar sin datos (se usa para operaciones sin retorno, como DELETE, POST sin datos).
/// </summary>
public class ApiResponse : Abstractions.IApiResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public IEnumerable<Abstractions.IErrorDetail>? Errors { get; set; }
    public string? ErrorCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? TraceId { get; set; }

    public ApiResponse() { }

    public ApiResponse(bool success, string? message = null, string? errorCode = null)
    {
        Success = success;
        Message = message;
        ErrorCode = errorCode;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Crea una respuesta exitosa.
    /// </summary>
    public static ApiResponse SuccessResponse(string? message = "Operación exitosa")
    {
        return new ApiResponse
        {
            Success = true,
            Message = message,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta con error.
    /// </summary>
    public static ApiResponse ErrorResponse(string errorCode, string message, IEnumerable<ApiError>? errors = null)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode,
            Errors = errors,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta con error y detalles granulares.
    /// </summary>
    public static ApiResponse ErrorResponse(string errorCode, string message, List<ApiError> errors)
    {
        return new ApiResponse
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode,
            Errors = errors.Cast<Abstractions.IErrorDetail>(),
            Timestamp = DateTime.UtcNow
        };
    }
}

/// <summary>
/// Respuesta API genérica tipada con datos.
/// </summary>
public class ApiResponse<T> : Abstractions.IApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public IEnumerable<Abstractions.IErrorDetail>? Errors { get; set; }
    public string? ErrorCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? TraceId { get; set; }

    public ApiResponse() { }

    public ApiResponse(bool success, T? data = default, string? message = null, string? errorCode = null)
    {
        Success = success;
        Data = data;
        Message = message;
        ErrorCode = errorCode;
        Timestamp = DateTime.UtcNow;
    }

    /// <summary>
    /// Crea una respuesta exitosa con datos.
    /// </summary>
    public static ApiResponse<T> SuccessResponse(T data, string? message = "Operación exitosa")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta con error.
    /// </summary>
    public static ApiResponse<T> ErrorResponse(string errorCode, string message, IEnumerable<ApiError>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode,
            Errors = errors,
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta con error y detailles granulares.
    /// </summary>
    public static ApiResponse<T> ErrorResponse(string errorCode, string message, List<ApiError> errors)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode,
            Errors = errors.Cast<Abstractions.IErrorDetail>(),
            Timestamp = DateTime.UtcNow
        };
    }
}

/// <summary>
/// Respuesta API paginada para listados grandes.
/// </summary>
public class ApiPaginatedResponse<T> : Abstractions.IApiResponse<IEnumerable<T>>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public IEnumerable<T>? Data { get; set; }
    public PaginationInfo? Pagination { get; set; }
    public IEnumerable<Abstractions.IErrorDetail>? Errors { get; set; }
    public string? ErrorCode { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? TraceId { get; set; }

    public ApiPaginatedResponse() { }

    /// <summary>
    /// Crea una respuesta paginada exitosa.
    /// </summary>
    public static ApiPaginatedResponse<T> SuccessResponse(
        IEnumerable<T> data,
        int pageNumber,
        int pageSize,
        long totalRecords,
        string? message = "Operación exitosa")
    {
        return new ApiPaginatedResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Pagination = new PaginationInfo(pageNumber, pageSize, totalRecords),
            Timestamp = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Crea una respuesta paginada con error.
    /// </summary>
    public static ApiPaginatedResponse<T> ErrorResponse(string errorCode, string message)
    {
        return new ApiPaginatedResponse<T>
        {
            Success = false,
            Message = message,
            ErrorCode = errorCode,
            Timestamp = DateTime.UtcNow
        };
    }
}

