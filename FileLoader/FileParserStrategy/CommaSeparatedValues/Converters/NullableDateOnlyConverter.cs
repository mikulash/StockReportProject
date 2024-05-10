using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace FileLoader.FileParserStrategy.CommaSeparatedValues.Converters;

public class NullableDateOnlyConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string text, IReaderRow row, 
        MemberMapData memberMapData) => DateOnly.TryParseExact(text, "MM/dd/yyyy", out DateOnly date) ? date : null;
}