using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.Services;
using Infrastructure.Query.Filters;

namespace BusinessLayer.Facades;

public interface IGenericFacade<TEntity, TKey, TCreateDto, TUpdateDto, TViewDto, TViewAllDto, TFilter> 
    where TEntity : class
    where TFilter : IFilter<TEntity>
{
    IGenericService<TEntity, TKey> Service { get; init; }
    Task<TViewDto> CreateAsync(TCreateDto create);
    Task<TViewDto> UpdateAsync(TKey key, TUpdateDto update);
    Task<TViewDto> FindByIdAsync(TKey key);
    Task DeleteByIdAsync(TKey key);
    Task<IEnumerable<TViewAllDto>> FetchAllAsync();
    Task<FilterResultDto<TViewAllDto>> FetchAllFilteredAsync(FilterDto filter);
}
