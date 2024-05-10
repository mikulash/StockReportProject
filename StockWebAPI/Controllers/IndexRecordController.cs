using StockBusinessLayer.Facades.IndexRecordFacade;
using StockBusinessLayer.Facades.ProcessFileFacade;
using Microsoft.AspNetCore.Mvc;
using StockAPI.DTOs.IndexRecordDTOs.Create;
using StockAPI.DTOs.IndexRecordDTOs.Filter;
using StockAPI.DTOs.IndexRecordDTOs.Update;

namespace StockWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndexRecordController : ControllerBase
{
    private readonly IIndexRecordFacade _indexRecordFacade;
    private readonly IProcessFileFacade _processFileFacade;

    public IndexRecordController(IIndexRecordFacade indexRecordFacade, IProcessFileFacade processFileFacade)
    {
        _indexRecordFacade = indexRecordFacade;
        _processFileFacade = processFileFacade;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateIndexRecord(CreateIndexRecordDto createIndexRecord)
    {
        var indexRecord = await _indexRecordFacade.CreateAsync(createIndexRecord);
        return Created(
            new Uri($"{Request.Path}/{indexRecord.Id}", UriKind.Relative),
            indexRecord
        );
    }

    [HttpGet]
    public async Task<IActionResult> FetchAll() => Ok(await _indexRecordFacade.FetchAllAsync());
    
    [HttpGet]
    [Route("filter")]
    public async Task<IActionResult> FetchAllFiltered([FromQuery] IndexRecordFilterDto filter) 
        => Ok(await _indexRecordFacade.FetchAllFilteredAsync(filter));

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateIndexRecord(long id, UpdateIndexRecordDto updateIndexRecord) 
        => Ok(await _indexRecordFacade.UpdateAsync(id, updateIndexRecord));

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> FindById(long id) => Ok(await _indexRecordFacade.FindByIdAsync(id));

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteById(long id)
    {
        await _indexRecordFacade.DeleteByIdAsync(id);
        return NoContent();
    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
        await using (var fileStream = file.OpenReadStream())
        {
            await _processFileFacade.ProcessAndSaveFileAsync(fileStream, file.ContentType);
        }

        return Created();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteByDateAndFund([FromQuery] string fundName, [FromQuery] DateOnly date)
    {
        await _indexRecordFacade.DeleteByDateAndFundAsync(fundName, date);
        
        return NoContent();
    }
}
