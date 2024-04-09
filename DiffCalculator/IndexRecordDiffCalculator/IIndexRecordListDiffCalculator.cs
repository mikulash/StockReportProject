using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculator.IndexRecordDiffCalculator;

public interface IIndexRecordListDiffCalculator
{
     RecordDiffs GetIndexRecordListDiff(List<IndexRecordDto> listA, List<IndexRecordDto> listB);
}