using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace FileLoader.FileParserStrategy.CommaSeparatedValues.Converters;

public class NullableFormatLongConverter : DefaultTypeConverter
{
    public override object? ConvertFromString(string text, IReaderRow row, 
        MemberMapData memberMapData) => 
        long.TryParse(text, NumberStyles.AllowThousands, 
            CultureInfo.InvariantCulture, out long value) ? 
            value : 
            null;
}