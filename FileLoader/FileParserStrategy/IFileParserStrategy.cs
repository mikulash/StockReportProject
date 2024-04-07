using FileLoader.Model;
using FileLoader.Reader;

namespace FileLoader.FileParserStrategy;

public interface IFileParserStrategy
{
    List<IndexRecordDto> ParseFileToList(IReader reader);
}