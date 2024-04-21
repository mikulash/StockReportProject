using AutoMapper;
using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.Services;
using DataAccessLayer.Models;
using Infrastructure.Query;
using Infrastructure.Query.Filters;

namespace BusinessLayer.Facades;

public class GenericFacade<TEntity, TKey, TCreateDto, TUpdateDto, TViewDto, TViewAllDto, TFilter> 
    : IGenericFacade<TEntity, TKey, TCreateDto, TUpdateDto, TViewDto, TViewAllDto, TFilter> 
    where TEntity : BaseEntity<TKey> 
    where TFilter : IFilter<TEntity>
{
    protected IMapper Mapper { get; init; }
    public IGenericService<TEntity, TKey> Service { get; init; }

    public GenericFacade(IGenericService<TEntity, TKey> service, IMapper mapper)
    {
        Service = service;
        Mapper = mapper;
    }

    public async Task<TViewDto> CreateAsync(TCreateDto create)
        => Mapper.Map<TViewDto>(await Service.CreateAsync(Mapper.Map<TEntity>(create)));

    public async Task<TViewDto> UpdateAsync(TKey key, TUpdateDto update)
    {
        var entity = await Service.FindByIdAsync(key);
        entity.SelfUpdate(Mapper.Map<TEntity>(update));
        return Mapper.Map<TViewDto>(entity);
    }

    public async Task<TViewDto> FindByIdAsync(TKey key) => Mapper.Map<TViewDto>(await Service.FindByIdAsync(key));

    public async Task DeleteByIdAsync(TKey key)
    {
        var entity = await Service.FindByIdAsync(key);
        await Service.DeleteAsync(entity);
    }

    public async Task<IEnumerable<TViewAllDto>> FetchAllAsync() 
        => Mapper.Map<List<TViewAllDto>>(await Service.FetchAllAsync());

    public async Task<FilterResultDto<TViewAllDto>> FetchAllFilteredAsync(FilterDto filter)
    {
        var queryResult = await Service.FetchFilteredAsync(Mapper.Map<TFilter>(filter), 
            Mapper.Map<QueryParams>(filter));
        
        return Mapper.Map<FilterResultDto<TViewAllDto>>(queryResult);
    }
}