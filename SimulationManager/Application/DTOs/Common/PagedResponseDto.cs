namespace SimulationManager.Application.DTOs.Common;

public record PagedResponseDto<T>
{
    public IEnumerable<T> Data { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalRecords { get; init; }
    public int TotalPages { get; init; }
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;

    public PagedResponseDto(
        IEnumerable<T> data,
        int pageNumber,
        int pageSize,
        int totalRecords)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
    }
}
