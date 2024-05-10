using FileLoader.Model;
using FileLoader.Reader;

namespace FileLoader.FileParserStrategy;

public interface IFileParserStrategy
{
    List<NullableIndexRecordDto> ParseFileToList(IReader reader);
}