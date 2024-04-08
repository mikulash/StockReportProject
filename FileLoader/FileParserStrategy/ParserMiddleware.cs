using FileLoader.FileParserStrategy.CommaSeparatedValues;
using FileLoader.FileParserStrategy.JavaScriptObjectNotation;
using FileLoader.Model;
using FileLoader.Reader;

namespace FileLoader.FileParserStrategy;

public class ParserMiddleware
{
    private const FileType DefaultFileType = FileType.Csv;
    public IFileParserStrategy ParserStrategy { get; set; }
    
    public ParserMiddleware()
    {
        SetNewParserStrategy();
    }

    public void SetNewParserStrategy(FileType fileType = DefaultFileType)
    {
        ParserStrategy = fileType switch
        {
            FileType.Csv => new CsvParserStrategy(),
            FileType.Json => new JsonParserStrategy(),
            _ => new CsvParserStrategy()
        };
    }

    public List<IndexRecordDto> ParseFileToList(IReader reader) 
        => reader.Validate() ? ParserStrategy.ParseFileToList(reader) : [];
}