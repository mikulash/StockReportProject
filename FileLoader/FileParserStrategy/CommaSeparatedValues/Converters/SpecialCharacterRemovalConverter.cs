using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using FileLoader.Extensions;

namespace FileLoader.FileParserStrategy.CommaSeparatedValues.Converters;

public class SpecialCharacterRemovalConverter : DefaultTypeConverter
{
    private readonly char[] _charactersToExclude = [',', '$', '%'];
    
    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        string clean = (text ?? string.Empty).ExcludeCharacters(_charactersToExclude);

        return double.TryParse(clean, CultureInfo.InvariantCulture, out double result) ? result : null;
    }
}