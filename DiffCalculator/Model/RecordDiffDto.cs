namespace DiffCalculator.Model;

public class RecordDiffDto
{
    public string? CUSIP { get; set; }
    public string? Fund { get; set; }
    public string? Company { get; set; }
    public string? Ticker { get; set; }
    public int? DayDiff { get; set; }
    public long? SharesDiff { get; set; }
    public double? MarketValueDiff { get; set; }
    public double? WeightDiff { get; set; }
}