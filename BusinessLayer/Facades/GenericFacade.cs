using AutoMapper;
using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.Services;
using DataAccessLayer.Models;
using Infrastructure.Query;
using Infrastructure.Query.Filters;

namespace BusinessLayer.Facades;

public class GenericFacade<TEntity, TKey, TService, TCreateDto, TUpdateDto, TViewDto, TViewAllDto, TFilter> 
    : IGenericFacade<TEntity, TKey, TService, TCreateDto, TUpdateDto, TViewDto, TViewAllDto, TFilter> 
    where TEntity : BaseEntity<TKey> 
    where TService : IGenericService<TEntity, TKey>
    where TFilter : IFilter<TEntity>
{
    protected IMapper Mapper { get; init; }
    public TService Service { get; init; }

    public GenericFacade(TService service, IMapper mapper)
    {
        Service = service;
        Mapper = mapper;
    }

    public virtual async Task<TViewDto> CreateAsync(TCreateDto create)
        => Mapper.Map<TViewDto>(await Service.CreateAsync(Mapper.Map<TEntity>(create)));

    public virtual async Task<TViewDto> UpdateAsync(TKey key, TUpdateDto update)
    {
        var entity = await Service.FindByIdAsync(key);
        entity.SelfUpdate(Mapper.Map<TEntity>(update));
        
        entity = await Service.UpdateAsync(entity);
        return Mapper.Map<TViewDto>(entity);
    }

    public virtual async Task<TViewDto> FindByIdAsync(TKey key) => Mapper.Map<TViewDto>(await Service.FindByIdAsync(key));

    public virtual async Task DeleteByIdAsync(TKey key)
    {
        var entity = await Service.FindByIdAsync(key);
        await Service.DeleteAsync(entity);
    }

    public virtual async Task<IEnumerable<TViewAllDto>> FetchAllAsync() 
        => Mapper.Map<List<TViewAllDto>>(await Service.FetchAllAsync());

    public virtual async Task<FilterResultDto<TViewAllDto>> FetchAllFilteredAsync(FilterDto filter)
    {
        var queryResult = await Service.FetchFilteredAsync(Mapper.Map<TFilter>(filter), 
            Mapper.Map<QueryParams>(filter));
        
        return Mapper.Map<FilterResultDto<TViewAllDto>>(queryResult);
    }
}
