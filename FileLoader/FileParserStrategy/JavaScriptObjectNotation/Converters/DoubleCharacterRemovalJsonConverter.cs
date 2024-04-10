using System.Globalization;
using FileLoader.Extensions;
using Newtonsoft.Json;

namespace FileLoader.FileParserStrategy.JavaScriptObjectNotation.Converters;

public class DoubleCharacterRemovalJsonConverter : JsonConverter<double?>
{
    private readonly char[] _charactersToExclude = [',', '$', '%'];
    
    public override void WriteJson(JsonWriter writer, double? value, JsonSerializer serializer) 
        => writer.WriteValue(value?.ToString() ?? string.Empty);

    public override double? ReadJson(JsonReader reader, Type objectType, double? existingValue, bool hasExistingValue, 
        JsonSerializer serializer)
    {
        string clean = (reader.Value?.ToString() ?? string.Empty)
            .ExcludeCharacters(_charactersToExclude);

        return double.TryParse(clean, CultureInfo.InvariantCulture, out double value) ? value : null;
    }
}
