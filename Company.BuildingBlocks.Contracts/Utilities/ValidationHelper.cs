namespace Company.BuildingBlocks.Contracts.Utilities;

using Company.BuildingBlocks.Contracts.ErrorHandling.Enums;
using Company.BuildingBlocks.Contracts.ErrorHandling.Exceptions;
using Company.BuildingBlocks.Contracts.Models;

/// <summary>
/// Utilidades para validación de modelos y entrada de usuario.
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Valida que un campo requerido no sea nulo o vacío.
    /// </summary>
    /// <param name="value">Valor a validar.</param>
    /// <param name="fieldName">Nombre del campo (para error).</param>
    /// <exception cref="ValidationException">Si la validación falla.</exception>
    public static void ValidateRequired(object? value, string fieldName)
    {
        if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
        {
            throw new ValidationException(
                $"El campo {fieldName} es requerido.",
                new List<ApiError>
                {
                    new(ErrorCode.REQUIRED_FIELD_MISSING, $"El campo {fieldName} es requerido.", fieldName)
                });
        }
    }

    /// <summary>
    /// Valida que una cadena tenga una longitud mínima.
    /// </summary>
    /// <param name="value">Valor a validar.</param>
    /// <param name="minLength">Longitud mínima.</param>
    /// <param name="fieldName">Nombre del campo (para error).</param>
    /// <exception cref="ValidationException">Si la validación falla.</exception>
    public static void ValidateMinLength(string? value, int minLength, string fieldName)
    {
        if (string.IsNullOrEmpty(value) || value.Length < minLength)
        {
            throw new ValidationException(
                $"El campo {fieldName} debe tener al menos {minLength} caracteres.",
                new List<ApiError>
                {
                    new(ErrorCode.INVALID_INPUT, 
                        $"El campo {fieldName} debe tener al menos {minLength} caracteres.", 
                        fieldName, value)
                });
        }
    }

    /// <summary>
    /// Valida que una cadena no exceda una longitud máxima.
    /// </summary>
    /// <param name="value">Valor a validar.</param>
    /// <param name="maxLength">Longitud máxima.</param>
    /// <param name="fieldName">Nombre del campo (para error).</param>
    /// <exception cref="ValidationException">Si la validación falla.</exception>
    public static void ValidateMaxLength(string? value, int maxLength, string fieldName)
    {
        if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
        {
            throw new ValidationException(
                $"El campo {fieldName} no debe exceder {maxLength} caracteres.",
                new List<ApiError>
                {
                    new(ErrorCode.INVALID_INPUT,
                        $"El campo {fieldName} no debe exceder {maxLength} caracteres.",
                        fieldName, value)
                });
        }
    }

    /// <summary>
    /// Valida que un entero esté dentro de un rango.
    /// </summary>
    /// <param name="value">Valor a validar.</param>
    /// <param name="minValue">Valor mínimo.</param>
    /// <param name="maxValue">Valor máximo.</param>
    /// <param name="fieldName">Nombre del campo (para error).</param>
    /// <exception cref="ValidationException">Si la validación falla.</exception>
    public static void ValidateRange(int value, int minValue, int maxValue, string fieldName)
    {
        if (value < minValue || value > maxValue)
        {
            throw new ValidationException(
                $"El campo {fieldName} debe estar entre {minValue} y {maxValue}.",
                new List<ApiError>
                {
                    new(ErrorCode.INVALID_INPUT,
                        $"El campo {fieldName} debe estar entre {minValue} y {maxValue}.",
                        fieldName, value)
                });
        }
    }

    /// <summary>
    /// Valida que un email tenga formato válido (validación básica).
    /// </summary>
    /// <param name="email">Email a validar.</param>
    /// <param name="fieldName">Nombre del campo (para error).</param>
    /// <exception cref="ValidationException">Si la validación falla.</exception>
    public static void ValidateEmail(string? email, string fieldName = "Email")
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@") || !email.Contains("."))
        {
            throw new ValidationException(
                $"El campo {fieldName} debe ser un email válido.",
                new List<ApiError>
                {
                    new(ErrorCode.INVALID_FORMAT,
                        $"El campo {fieldName} debe ser un email válido.",
                        fieldName, email)
                });
        }
    }
}


