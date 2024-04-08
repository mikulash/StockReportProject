using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace FileLoader.FileParserStrategy.CommaSeparatedValues.Converters;

public class NullableDateOnlyConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string text, IReaderRow row, 
        MemberMapData memberMapData) => DateOnly.TryParse(text, out DateOnly date) ? date : null;
}