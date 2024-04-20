namespace Infrastructure.Query;

public class QueryParams
{
    public int PageNumber { get; set; } = PagingParameters.defaultPageNumber;
    public int PageSize { get; set; } = PagingParameters.defaultPageSize;
    public string SortParameter { get; set; } = string.Empty;
    public bool SortAscending { get; set; } = false;
}
