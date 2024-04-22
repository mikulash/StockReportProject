using BusinessLayer.DTOs.FundDTO;
using BusinessLayer.DTOs.FundDTO.Create;
using BusinessLayer.DTOs.FundDTO.Filter;
using BusinessLayer.DTOs.FundDTO.Update;
using BusinessLayer.DTOs.FundDTO.View;
using BusinessLayer.Facades;
using DataAccessLayer.Models;
using Infrastructure.Query.Filters.EntityFilters;
using Microsoft.AspNetCore.Mvc;

namespace StockWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FundController : ControllerBase
{
    private IGenericFacade<Fund, long, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto, FundFilter> _fundFacade;

    public FundController(IGenericFacade<Fund, long, CreateFundDto, UpdateFundDto, ViewFundDto, ViewFundDto, FundFilter> fundFacade)
    {
        _fundFacade = fundFacade;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFund(CreateFundDto createFundDto)
    {
        var fund = await _fundFacade.CreateAsync(createFundDto);
        return Created(
            new Uri($"{Request.Path}/{fund.Id}", UriKind.Relative),
            fund
        );
    }

    [HttpGet]
    public async Task<IActionResult> FetchAll() => Ok(await _fundFacade.FetchAllAsync());
    
    [HttpGet]
    [Route("filter")]
    public async Task<IActionResult> FetchAllFiltered([FromQuery] FundFilterDto filter) 
        => Ok(await _fundFacade.FetchAllFilteredAsync(filter));

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateFund(long id, UpdateFundDto updateFundDto) 
        => Ok(await _fundFacade.UpdateAsync(id, updateFundDto));

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> FindById(long id) => Ok(await _fundFacade.FindByIdAsync(id));

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(long id)
    {
        await _fundFacade.DeleteByIdAsync(id);
        return NoContent();
    }
}
