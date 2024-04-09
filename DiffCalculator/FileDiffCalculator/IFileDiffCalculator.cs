using DiffCalculator.Model;
using FileLoader.Model;

namespace DiffCalculator.FileDiffCalculator;

public interface IFileDiffCalculator
{
    List<RecordDiffDto> GetListDiff(List<IndexRecordDto> listA, List<IndexRecordDto> listB);
}