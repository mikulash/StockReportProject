using Newtonsoft.Json;

namespace FileLoader.FileParserStrategy.JavaScriptObjectNotation.Converters;

public class DateOnlyJsonConverter : JsonConverter<DateOnly?>
{
    private const string DateOnlyFormat = "MM/DD/YYYY";
    
    public override void WriteJson(JsonWriter writer, DateOnly? value, JsonSerializer serializer) 
        => writer.WriteValue(value?.ToString(DateOnlyFormat) ?? string.Empty);

    public override DateOnly? ReadJson(JsonReader reader, Type objectType, DateOnly? existingValue,
        bool hasExistingValue, JsonSerializer serializer) 
        => DateOnly.TryParseExact(reader?.Value?.ToString() ?? string.Empty, "MM/dd/yyyy", out DateOnly date)
            ? date
            : null;
}
