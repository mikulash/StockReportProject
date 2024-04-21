using BusinessLayer.Services;

namespace BusinessLayer.Facades;

public interface IGenericFacade<TEntity, TKey, TCreateDto, TUpdateDto, TViewDto, TViewAllDto> where TEntity : class
{
    IGenericService<TEntity, TKey> Service { get; init; }

    Task<TViewDto> CreateAsync(TCreateDto create);

    Task<TViewDto> UpdateAsync(TKey key, TUpdateDto update);

    Task<TViewDto> FindByIdAsync(TKey key);

    Task DeleteByIdAsync(TKey key);

    Task<IEnumerable<TViewAllDto>> FetchAllAsync();
}