namespace DiffCalculator.Model;

public class Positions
{
    public List<IndexRecordDiffDto>? NewPositions { get; set; }
    public List<IndexRecordDiffDto>? IncreasedPositions { get; set; }
    public List<IndexRecordDiffDto>? DecreasedPositions { get; set; }

}