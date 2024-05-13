using StockBusinessLayer.Facades.IndexRecordDiffFacade;
using Microsoft.AspNetCore.Mvc;

namespace StockWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndexRecordDiffController : ControllerBase
{
    private readonly IIndexRecordDiffFacade _indexRecordDiffFacade;

    public IndexRecordDiffController(IIndexRecordDiffFacade indexRecordDiffFacade)
    {
        _indexRecordDiffFacade = indexRecordDiffFacade;
    }

    [HttpGet]
    [Route("raw")]
    public async Task<IActionResult> GetDifference([FromQuery] string fundName, [FromQuery] DateOnly date)
    {
        return Ok(await _indexRecordDiffFacade.GetIndexRecordDifferenceAsync(fundName, date));
    }
}
