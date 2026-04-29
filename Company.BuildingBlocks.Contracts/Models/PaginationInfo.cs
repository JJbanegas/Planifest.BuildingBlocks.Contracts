namespace Company.BuildingBlocks.Contracts.Models;

/// <summary>
/// Información de paginación para respuestas paginadas.
/// </summary>
public class PaginationInfo : Abstractions.IPaginationInfo
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public long TotalRecords { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;

    public PaginationInfo() { }

    public PaginationInfo(int pageNumber, int pageSize, long totalRecords)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
    }
}

