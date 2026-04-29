namespace Company.BuildingBlocks.Contracts.Abstractions;

/// <summary>
/// Contrato para información de paginación.
/// </summary>
public interface IPaginationInfo
{
    /// <summary>
    /// Número de página actual (basado en 1).
    /// </summary>
    int PageNumber { get; }

    /// <summary>
    /// Cantidad de registros por página.
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// Total de registros disponibles.
    /// </summary>
    long TotalRecords { get; }

    /// <summary>
    /// Total de páginas disponibles.
    /// </summary>
    int TotalPages { get; }

    /// <summary>
    /// Indica si hay página siguiente.
    /// </summary>
    bool HasNextPage { get; }

    /// <summary>
    /// Indica si hay página anterior.
    /// </summary>
    bool HasPreviousPage { get; }
}

