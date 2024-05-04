using FileLoader.Model;
using FileLoader.Reader;

namespace FileLoader.FileParserStrategy;

public interface IParserMiddleware
{
    IFileParserStrategy ParserStrategy { get; set; }
    void SetNewParserStrategy(FileType fileType);
    List<NullableIndexRecordDto> ParseFileToList(IReader reader);
}