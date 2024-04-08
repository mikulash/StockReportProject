using DiffCalculator.Model;

namespace DiffCalculator.FileDiffCalculator;

public interface IFileDiffCalculator
{
    List<RecordDiffDto> GetFileDiff(string filePathA, string filePathB);
}