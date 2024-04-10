using FileLoader.Model;

namespace FileLoader.Filter;

public abstract class BaseDecoratorFilter(IDecoratorFilter? filter = null) : IDecoratorFilter
{
    public IDecoratorFilter? DecoratorFilter { get; set; } = filter;

    public abstract List<IndexRecordDto> ApplyFilter(List<IndexRecordDto> input);
}