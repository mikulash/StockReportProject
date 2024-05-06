using Microsoft.AspNetCore.Mvc;
using StockAPI.DTOs.FundDTO.Create;
using StockAPI.DTOs.FundDTO.Filter;
using StockAPI.DTOs.FundDTO.Update;
using FundFacade = GenericBusinessLayer.Facades.IGenericFacade<DataAccessLayer.Models.Fund, long, 
    GenericBusinessLayer.Services.IGenericService<DataAccessLayer.Models.Fund, long>, 
    StockAPI.DTOs.FundDTO.Create.CreateFundDto, StockAPI.DTOs.FundDTO.Update.UpdateFundDto, 
    StockAPI.DTOs.FundDTO.View.ViewFundDto, StockAPI.DTOs.FundDTO.View.ViewFundDto>;

namespace StockWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FundController : ControllerBase
{
    private readonly FundFacade _fundFacade;

    public FundController(FundFacade fundFacade)
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
