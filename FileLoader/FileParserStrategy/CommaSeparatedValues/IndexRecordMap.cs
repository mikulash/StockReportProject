using System.Globalization;
using CsvHelper.Configuration;
using FileLoader.FileParserStrategy.CommaSeparatedValues.Converters;
using FileLoader.Model;

namespace FileLoader.FileParserStrategy.CommaSeparatedValues;

public sealed class IndexRecordMap : ClassMap<IndexRecordDto>
{
    public IndexRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(record => record.Date).Name("date")
            .TypeConverter<NullableDateOnlyConverter>();
        Map(record => record.Fund).Name("fund");
        Map(record => record.Company).Name("company");
        Map(record => record.Ticker).Name("ticker");
        Map(record => record.CUSIP).Name("cusip");
        Map(record => record.Shares).Name("shares")
            .TypeConverter<NullableFormatLongConverter>();
        Map(record => record.MarketValue).Name("market value ($)")
            .TypeConverter<SpecialCharacterRemovalConverter>();
        Map(record => record.Weight).Name("weight (%)")
            .TypeConverter<SpecialCharacterRemovalConverter>();
    }
}