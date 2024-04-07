using FileLoader.Model;
using FileLoader.Reader;
using Newtonsoft.Json;

namespace FileLoader.FileParserStrategy.JavaScriptObjectNotation;

public class JsonParserStrategy : IFileParserStrategy
{
    public List<IndexRecordDto> ParseFileToList(IReader reader)
    {
        try
        {
            return JsonConvert.DeserializeObject<List<IndexRecordDto>>(reader.Reader.ReadToEnd()) ?? [];
        }
        catch (JsonException)
        {
            return [];
        }
    }
}