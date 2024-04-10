using FileLoader.Model;

namespace FileLoader.Filter;

public interface IDecoratorFilter
{
    IDecoratorFilter? DecoratorFilter { get; set; }
    List<IndexRecordDto> ApplyFilter(List<IndexRecordDto> input);
}