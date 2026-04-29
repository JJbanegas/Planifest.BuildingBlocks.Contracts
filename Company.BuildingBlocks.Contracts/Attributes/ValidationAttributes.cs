namespace Company.BuildingBlocks.Contracts.Attributes;

/// <summary>
/// Atributo para marcar un campo que es requerido cuando una condición se cumple.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class RequiredIfAttribute : Attribute
{
    public string DependentProperty { get; set; }
    public object? DependentValue { get; set; }

    public RequiredIfAttribute(string dependentProperty, object? dependentValue = null)
    {
        DependentProperty = dependentProperty;
        DependentValue = dependentValue;
    }
}

/// <summary>
/// Atributo para marcar propiedades que deben validarse.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ValidateModelAttribute : Attribute
{
    public string? ErrorMessage { get; set; }

    public ValidateModelAttribute(string? errorMessage = null)
    {
        ErrorMessage = errorMessage;
    }
}

/// <summary>
/// Atributo para documentar respuestas de error en un endpoint.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ErrorResponseAttribute : Attribute
{
    public int StatusCode { get; set; }
    public string ErrorCode { get; set; }
    public string Description { get; set; }

    public ErrorResponseAttribute(int statusCode, string errorCode, string description)
    {
        StatusCode = statusCode;
        ErrorCode = errorCode;
        Description = description;
    }
}

