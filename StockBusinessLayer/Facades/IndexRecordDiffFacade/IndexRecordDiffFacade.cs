using AutoMapper;
using DiffCalculator.IndexRecordDiffCalculator;
using DiffCalculator.Model;
using StockBusinessLayer.Services.IndexRecordService;
using IndexRecordDtoCalc = DiffCalculator.Model.IndexRecordDto;

namespace StockBusinessLayer.Facades.IndexRecordDiffFacade;

public class IndexRecordDiffFacade : IIndexRecordDiffFacade
{
    private readonly IIndexRecordService _indexRecordService;
    private readonly IMapper _mapper;

    public IndexRecordDiffFacade(IIndexRecordService indexRecordService, IMapper mapper)
    {
        _indexRecordService = indexRecordService;
        _mapper = mapper;
    }

    public async Task<RecordDiffs> GetIndexRecordDifferenceAsync(string fundName, DateOnly date)
    {
        var current = await _indexRecordService.FetchByDateAndFundNameAsync(fundName, date);

        var comparableDate = await _indexRecordService.FetchComparableOlderDateAsync(fundName, date);
        var last = 
            comparableDate is not null 
                ? await _indexRecordService.FetchByDateAndFundNameAsync(fundName, comparableDate.Value) 
                : [];

        var diffCalc = new IndexRecordListDiffCalculator(
            _mapper.Map<List<IndexRecordDtoCalc>>(last),
            _mapper.Map<List<IndexRecordDtoCalc>>(current));

        return diffCalc.GetIndexRecordListDiff();
    }
}
