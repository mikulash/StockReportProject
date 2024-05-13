using FileLoader.FileParserStrategy;
using FileLoader.FileParserStrategy.CommaSeparatedValues;
using FileLoader.Reader;
using FileLoaderTests.TestUtilities;

namespace FileLoaderTests.Csv;

public class CsvMemoryReaderTests 
{
    private IFileParserStrategy _parser;

    [SetUp]
    public void SetUp()
    {
        _parser = new CsvParserStrategy();
    }

    private void ActAndAssertEmpty(IReader reader)
    {
        var result = _parser.ParseFileToList(reader);
        
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    private static IEnumerable<string> IncorrectOrEmptyTestCases()
    {
        return
        [
            string.Empty, 
            StringTestCases.OnlyCsvHeader, 
            StringTestCases.RandomString, 
            StringTestCases.RandomCsvHeader,
            StringTestCases.AlmostCorrectCsvHeader,
            StringTestCases.IncorrectHeaderCorrectData,
            StringTestCases.RandomCsv
        ];
    }
    
    [TestCaseSource(nameof(IncorrectOrEmptyTestCases))]
    public void ParseFileToList_IncorrectOrEmptyInputFormat_EmptyResult(string input)
    {
        // arrange
        var readerMock = ReaderUtilities.CreateMemoryMockedReader(input);

        // act & assert
        ActAndAssertEmpty(readerMock.Object);
    }

    [Test]
    public void ParseFileToList_CorrectCsvFileAsString_ReturnsParsedFile()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateMemoryMockedReader(StringTestCases.CorrectHeaderCorrectData);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Select(item => item.Date), Has.All.EqualTo(new DateOnly(2024, 4, 3)));
        Assert.That(result.Select(item => item.Company), Has.All.Not.Null);
        Assert.That(result.Select(item => item.MarketValue), Has.All.GreaterThan(0));
        Assert.That(result.Select(item => item.Fund), Has.All.EqualTo("ARKK"));
    }

    private static IEnumerable<Tuple<string, string>> CorruptedDataFieldsTestCases()
    {
        return 
            [
                new Tuple<string, string>("Date", StringTestCases.CorrectHeaderDateColumnCorrupted),
                new Tuple<string, string>("Shares", StringTestCases.CorrectHeaderSharesColumnCorrupted),
                new Tuple<string, string>("MarketValue", StringTestCases.CorrectHeaderMarketValueColumnCorrupted),
                new Tuple<string, string>("Weight", StringTestCases.CorrectHeaderWeightColumnCorrupted)
            ];
    }

    [TestCaseSource(nameof(CorruptedDataFieldsTestCases))]
    public void ParseFileToList_CorruptedFieldsData_ParsedFileWithNullFields(Tuple<string, string> input)
    {
        // arrange
        var readerMock = ReaderUtilities.CreateMemoryMockedReader(input.Item2);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        //assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Select(item => item.GetType().GetProperty(input.Item1).GetValue(item)), 
            Contains.Item(null));
    }

    [Test]
    public void ParseFileToList_OneInvalidRecord_NullPropertyValuesForInvalidRecord()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateMemoryMockedReader(StringTestCases.CorrectHeaderOneInvalidRecord);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Has.Count.EqualTo(2));

        var item = result.Last();
        
        Assert.That(item, Is.Not.Null);
        Assert.That(item.Date, Is.Null);
        Assert.That(item.MarketValue, Is.Null);
    }

    [Test]
    public void ParseFileToList_TwoFilesDifferentColumnOrdering_EqualParsedFiles()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateMemoryMockedReader(StringTestCases.CorrectHeaderCorrectData);
        var readerMockSecond = ReaderUtilities.CreateMemoryMockedReader(StringTestCases.CorrectHeaderCorrectDataMixed);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object)
            .OrderBy(x => x.Shares)
            .ToList();
        var resultSecond = _parser.ParseFileToList(readerMockSecond.Object)
            .OrderBy(x => x.Shares)
            .ToList();
        
        // assert
        Assert.That(result.Count(), Is.EqualTo(resultSecond.Count()));
        for (int index = 0; index < result.Count(); index++)
        {
            Assert.That(result.ElementAt(index).CUSIP, Is.EqualTo(resultSecond.ElementAt(index).CUSIP));
            Assert.That(result.ElementAt(index).Company, Is.EqualTo(resultSecond.ElementAt(index).Company));
        }
    }

    [Test]
    public void ParseFileToList_RecordColumnIsRotated_RecordWithNullValues()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateMemoryMockedReader(StringTestCases.CorrectHeaderRotateColumnValue);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));

        var item = result.First();
        
        Assert.That(item.Date, Is.Null);
        Assert.That(item.Company, Is.EqualTo("COIN")); // rotated
        Assert.That(item.Weight, Is.Null);
    }

    [Test]
    public void ParseFileToList_RecordHasNewColumn_NewColumnsIgnored()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateMemoryMockedReader(StringTestCases.CorrectHeaderNewColumnInRecord);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));

        ListUtilities.CheckAllNonNullPropertiesInList(result);
    }
}
