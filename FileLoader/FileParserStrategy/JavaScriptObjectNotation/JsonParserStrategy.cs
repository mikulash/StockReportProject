using FileLoader.Model;
using FileLoader.Reader;
using Newtonsoft.Json;

namespace FileLoader.FileParserStrategy.JavaScriptObjectNotation;

public class JsonParserStrategy : IFileParserStrategy
{
    public List<NullableIndexRecordDto> ParseFileToList(IReader reader)
    {
        try
        {
            return JsonConvert.DeserializeObject<List<NullableIndexRecordDto>>(reader.Reader.ReadToEnd()) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}