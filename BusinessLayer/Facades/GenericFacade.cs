using AutoMapper;
using BusinessLayer.Services;
using DataAccessLayer.Models;

namespace BusinessLayer.Facades;

public class GenericFacade<TEntity, TKey, TCreateDto, TUpdateDto, TViewDto, TViewAllDto> 
    : IGenericFacade<TEntity, TKey, TCreateDto, TUpdateDto, TViewDto, TViewAllDto> where TEntity : BaseEntity<TKey>
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
}