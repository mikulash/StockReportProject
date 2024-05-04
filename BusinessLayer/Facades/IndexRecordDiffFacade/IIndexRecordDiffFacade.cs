using DiffCalculator.Model;

namespace BusinessLayer.Facades.IndexRecordDiffFacade;

public interface IIndexRecordDiffFacade
{
    Task<RecordDiffs> GetIndexRecordDifferenceAsync(string fundName, DateOnly date);
}
