using BusinessLayer.DTOs.BaseFilter;
using BusinessLayer.DTOs.CompanyDTOs.Create;
using BusinessLayer.DTOs.CompanyDTOs.Filter;
using BusinessLayer.DTOs.CompanyDTOs.Update;
using BusinessLayer.Facades.CompanyFacade;
using Microsoft.AspNetCore.Mvc;

namespace StockWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly ICompanyFacade _companyFacade;

    public CompanyController(ICompanyFacade companyFacade)
    {
        _companyFacade = companyFacade;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateFund(CreateCompanyDto createCompanyDto)
    {
        var company = await _companyFacade.CreateAsync(createCompanyDto);
        return Created(
            new Uri($"{Request.Path}/{company.Id}", UriKind.Relative),
            company
        );
    }

    [HttpGet]
    public async Task<IActionResult> FetchAll() => Ok(await _companyFacade.FetchAllAsync());
    
    [HttpGet]
    [Route("filter")]
    public async Task<IActionResult> FetchAllFiltered([FromQuery] CompanyFilterDto filter) 
        => Ok(await _companyFacade.FetchAllFilteredAsync(filter));

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateFund(long id, UpdateCompanyDto updateCompanyDto) 
        => Ok(await _companyFacade.UpdateAsync(id, updateCompanyDto));

    [HttpGet]
    [Route("all/{id}")]
    public async Task<IActionResult> FindById(long id) => Ok(await _companyFacade.FindByIdAsync(id));

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> FindByIdFilteredIndexRecords(long id, [FromQuery] FilterDto filter)
        => Ok(await _companyFacade.FindByIdFilteredIndexRecords(id, filter));

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(long id)
    {
        await _companyFacade.DeleteByIdAsync(id);
        return NoContent();
    }
}
