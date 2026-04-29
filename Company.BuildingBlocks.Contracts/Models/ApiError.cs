namespace Company.BuildingBlocks.Contracts.Models;

/// <summary>
/// Detalle granular de un error dentro de una respuesta API.
/// </summary>
public class ApiError : Abstractions.IErrorDetail
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Field { get; set; }
    public object? AttemptedValue { get; set; }
    public string Severity { get; set; } = "Error";

    public ApiError() { }

    public ApiError(string code, string message, string? field = null, object? attemptedValue = null, string severity = "Error")
    {
        Code = code;
        Message = message;
        Field = field;
        AttemptedValue = attemptedValue;
        Severity = severity;
    }
}

