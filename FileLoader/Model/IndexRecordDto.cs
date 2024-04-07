using FileLoader.FileParserStrategy.JavaScriptObjectNotation.Converters;
using Newtonsoft.Json;

namespace FileLoader.Model;

public class IndexRecordDto
{
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly? Date { get; set; }
    public string? Fund { get; set; }
    public string? Company { get; set; }
    public string? Ticker { get; set; }
    public string? CUSIP { get; set; }
    [JsonConverter(typeof(NullableFormatLongJsonConverter))]
    public long? Shares { get; set; }
    [JsonProperty("market value ($)")]
    [JsonConverter(typeof(DoubleCharacterRemovalJsonConverter))]
    public double? MarketValue { get; set; }
    [JsonProperty("weight (%)")]
    [JsonConverter(typeof(DoubleCharacterRemovalJsonConverter))]
    public double? Weight { get; set; }
}