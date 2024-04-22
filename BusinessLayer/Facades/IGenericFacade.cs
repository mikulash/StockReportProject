﻿using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.Services;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters;

namespace BusinessLayer.Facades;

public interface IGenericFacade<TEntity, TKey, TService, TCreateDto, TUpdateDto, TViewDto, TViewAllDto, TFilter> 
    where TEntity : BaseEntity<TKey>
    where TService : IGenericService<TEntity, TKey>
    where TFilter : IFilter<TEntity>
{
    TService Service { get; init; }
    Task<TViewDto> CreateAsync(TCreateDto create);
    Task<TViewDto> UpdateAsync(TKey key, TUpdateDto update);
    Task<TViewDto> FindByIdAsync(TKey key);
    Task DeleteByIdAsync(TKey key);
    Task<IEnumerable<TViewAllDto>> FetchAllAsync();
    Task<FilterResultDto<TViewAllDto>> FetchAllFilteredAsync(FilterDto filter);
}
