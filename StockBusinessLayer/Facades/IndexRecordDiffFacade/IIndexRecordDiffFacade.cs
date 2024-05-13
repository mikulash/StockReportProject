using DiffCalculator.Model;

namespace StockBusinessLayer.Facades.IndexRecordDiffFacade;

public interface IIndexRecordDiffFacade
{
    Task<RecordDiffs> GetIndexRecordDifferenceAsync(string fundName, DateOnly date);
}
