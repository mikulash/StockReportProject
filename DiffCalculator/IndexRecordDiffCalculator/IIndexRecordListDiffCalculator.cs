using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculator.IndexRecordDiffCalculator;

public interface IIndexRecordListDiffCalculator
{
    List<IndexRecordDiffDto> GetIndexRecordListDiff(List<IndexRecordDto> listA, List<IndexRecordDto> listB);
}