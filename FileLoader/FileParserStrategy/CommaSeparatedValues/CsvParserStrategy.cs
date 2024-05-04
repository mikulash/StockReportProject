using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using FileLoader.Extensions;
using FileLoader.Model;
using IReader = FileLoader.Reader.IReader;

namespace FileLoader.FileParserStrategy.CommaSeparatedValues;


public class CsvParserStrategy : IFileParserStrategy
{
    private static readonly CultureInfo DefaultCultureInfo = CultureInfo.InvariantCulture;

    private static void RegisterCsvMaps(CsvReader csvReader)
    {
        csvReader.Context.RegisterClassMap<IndexRecordMap>();
    }

    private static CsvConfiguration CreateCsvConfiguration()
    {
        return new CsvConfiguration(DefaultCultureInfo)
        {
            Delimiter = ",",
            MissingFieldFound = null,
            HasHeaderRecord = true
        };
    }
    
    public List<NullableIndexRecordDto> ParseFileToList(IReader reader)
    {
        using var csvReader = new CsvReader(reader.Reader, CreateCsvConfiguration());
        RegisterCsvMaps(csvReader);
        return csvReader.TryValidateHeader<NullableIndexRecordDto>() ? csvReader.GetRecords<NullableIndexRecordDto>().ToList() : [];
    }
}
