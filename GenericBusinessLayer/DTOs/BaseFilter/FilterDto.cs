using GenericInfrastructure.Query;

namespace GenericBusinessLayer.DTOs.BaseFilter;

public class FilterDto
{
    public int? PageNumber { get; set; } = PagingParameters.defaultPageNumber;
    public int? PageSize { get; set; } = PagingParameters.defaultPageSize;
    public string? SortParameter { get; set; }
    public bool? SortAscending { get; set; }
}
