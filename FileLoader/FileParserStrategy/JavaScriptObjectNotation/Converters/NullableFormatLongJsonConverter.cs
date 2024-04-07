using System.Globalization;
using Newtonsoft.Json;

namespace FileLoader.FileParserStrategy.JavaScriptObjectNotation.Converters;

public class NullableFormatLongJsonConverter : JsonConverter<long?>
{
    public override void WriteJson(JsonWriter writer, long? value, JsonSerializer serializer) 
        => writer.WriteValue(value?.ToString() ?? string.Empty);

    public override long? ReadJson(JsonReader reader, Type objectType, long? existingValue, bool hasExistingValue,
        JsonSerializer serializer)
        => long.TryParse(reader?.Value?.ToString() ?? string.Empty, NumberStyles.AllowThousands,
            CultureInfo.InvariantCulture, out long value)
            ? value
            : null;
}
