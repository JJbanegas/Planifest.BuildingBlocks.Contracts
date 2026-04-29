namespace Company.BuildingBlocks.Contracts.Models;

/// <summary>
/// Solicitud de paginación de lista.
/// </summary>
public class PagedRequest
{
    /// <summary>
    /// Número de página (basado en 1). Por defecto: 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Cantidad de registros por página. Por defecto: 10. Máximo: 100.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Campo por el que ordenar (opcional).
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Dirección del ordenamiento: "asc" o "desc". Por defecto: "asc".
    /// </summary>
    public string SortOrder { get; set; } = "asc";

    /// <summary>
    /// Valida que los parámetros de paginación sean válidos.
    /// </summary>
    public bool IsValid()
    {
        if (PageNumber < 1) PageNumber = 1;
        if (PageSize < 1) PageSize = 10;
        if (PageSize > 100) PageSize = 100;
        if (SortOrder != "asc" && SortOrder != "desc") SortOrder = "asc";

        return PageNumber >= 1 && PageSize >= 1 && PageSize <= 100;
    }
}

