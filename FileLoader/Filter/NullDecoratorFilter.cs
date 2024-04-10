using FileLoader.Model;

namespace FileLoader.Filter;

public class NullDecoratorFilter : BaseDecoratorFilter
{
    private static bool HasSomeNullProperty(IndexRecordDto item) 
        => item.GetType().GetProperties().Any(property => property.GetValue(item) is null);

    public override List<IndexRecordDto> ApplyFilter(List<IndexRecordDto> input)
    {
        var result = input.Where(HasSomeNullProperty).ToList();
        return DecoratorFilter is null ? result : DecoratorFilter.ApplyFilter(result);
    }
}
