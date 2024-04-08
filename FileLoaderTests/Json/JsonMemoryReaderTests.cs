using FileLoader.FileParserStrategy;
using FileLoader.FileParserStrategy.JavaScriptObjectNotation;
using FileLoader.Reader;
using FileLoaderTests.TestUtilities;

namespace FileLoaderTests.Json;

public class JsonMemoryReaderTests
{
    private IFileParserStrategy _parser;

    [SetUp]
    public void SetUp()
    {
        _parser = new JsonParserStrategy();
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
            StringTestCases.EmptyJsonArray,
            StringTestCases.RandomString
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
    public void ParseFileToList_NotCorrectJsonFormat_NotEmptyResultWithNullObjects()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateMemoryMockedReader(StringTestCases.RandomJson);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(3));
        ListUtilities.CheckAllNonNullPropertiesInList(result);
    }

    private static IEnumerable<string> ValidJsonStrings()
    {
        return
        [
            StringTestCases.ValidJson,
            StringTestCases.ValidJsonWithNewKeys
        ];
    }
    
    [TestCaseSource(nameof(ValidJsonStrings))]
    public void ParseFileToList_CorrectJsonFileAsString_ReturnsParsedFile(string json)
    {
        // arrange
        var readerMock = ReaderUtilities.CreateMemoryMockedReader(json);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Not.Empty);
        Assert.That(result.Select(item => item.Date), Has.All.EqualTo(DateOnly.Parse("03/08/2024")));
        Assert.That(result.Select(item => item.Company), Has.All.Not.Null);
        Assert.That(result.Select(item => item.MarketValue), Has.All.GreaterThan(0));
        Assert.That(result.Select(item => item.Fund), Has.All.EqualTo("ARKK"));
    }
}
