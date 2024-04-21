using BusinessLayer.DTOs.FundDTO;
using BusinessLayer.Facades;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;
using Microsoft.AspNetCore.Mvc;

namespace StockWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FundController : ControllerBase
{
    private IGenericFacade<Fund, long, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto, FundFilter> _facade;

    public FundController(IGenericFacade<Fund, long, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto, FundFilter> facade)
    {
        _facade = facade;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFund(CreateFundDto createFundDto)
    {
        var fund = await _facade.CreateAsync(createFundDto);
        return Created(
            new Uri($"{Request.Path}/{fund.Id}", UriKind.Relative),
            fund
        );
    }

    [HttpGet]
    public async Task<IActionResult> FetchAll() => Ok(await _facade.FetchAllAsync());
    
    [HttpGet]
    [Route("filter")]
    public async Task<IActionResult> FetchAllFiltered([FromQuery] FundFilterDto filter) 
        => Ok(await _facade.FetchAllFilteredAsync(filter));

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateFund(long id, UpdateFundDto updateFundDto) 
        => Ok(await _facade.UpdateAsync(id, updateFundDto));

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> FindById(long id) => Ok(await _facade.FindByIdAsync(id));

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(long id)
    {
        await _facade.DeleteByIdAsync(id);
        return NoContent();
    }
}
