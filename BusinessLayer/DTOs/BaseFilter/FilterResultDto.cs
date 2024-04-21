namespace BusinessLayer.DTOs.BaseFilter;

public class FilterResultDto<TEntityDto>
{
    public long TotalItemsCount { get; set; }
    public int TotalPages { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<TEntityDto> Items { get; set; } = new List<TEntityDto>();
    public bool PagingEnabled { get; set; }
}
