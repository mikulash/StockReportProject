namespace DiffCalculator.Model;

public class IndexRecordDto
{
    public DateOnly Date { get; set; }
    public string Fund { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string CUSIP { get; set; } = string.Empty;
    public long Shares { get; set; }
    public double MarketValue { get; set; }
    public double Weight { get; set; }
}
