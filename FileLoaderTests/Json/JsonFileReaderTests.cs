using FileLoader.FileParserStrategy;
using FileLoader.FileParserStrategy.JavaScriptObjectNotation;
using FileLoaderTests.TestUtilities;

namespace FileLoaderTests.Json;

public class JsonFileReaderTests
{
    private IFileParserStrategy _parser;
    private const string SampleFileDirectory = "JsonTestSamples";

    [SetUp]
    public void SetUp()
    {
        _parser = new JsonParserStrategy();
    }

    [Test]
    public void ParseFileToList_CorrectCsvFile_ReturnsParsedFileAsList()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateFileMockedFileReader("ARK_2024_04_05.json", SampleFileDirectory);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(39));

        Assert.That(result.Last().Date, Is.Null);
        var last = result.Last();
        result.Remove(last);
        
        ListUtilities.CheckAllNonNullPropertiesInList(result);
    }

    [Test]
    public void ParseFileToList_EmptyCsvFile_ReturnsEmptyList()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateFileMockedFileReader("ARK_EMPTY.json", SampleFileDirectory);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(0));
    }

    [Test]
    public void ParseFileToList_TwoFilesDifferentColumnOrdering_EqualParsedFiles()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateFileMockedFileReader("ARK_2024_04_05.json", SampleFileDirectory);
        var readerMockMixed = ReaderUtilities.CreateFileMockedFileReader("ARK_2024_04_05_MIXED.json", SampleFileDirectory);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object)
            .OrderBy(item => item.Shares)
            .ToList();
        var resultMixed = _parser.ParseFileToList(readerMockMixed.Object)
            .OrderBy(item => item.Shares)
            .ToList();
        
        // assert
        Assert.That(result.Count, Is.EqualTo(resultMixed.Count));
        for (int index = 0; index < result.Count; index++)
        {
            Assert.That(result.ElementAt(index).CUSIP, Is.EqualTo(resultMixed.ElementAt(index).CUSIP));
            Assert.That(result.ElementAt(index).Company, Is.EqualTo(resultMixed.ElementAt(index).Company));
            Assert.That(result.ElementAt(index).Shares, Is.EqualTo(resultMixed.ElementAt(index).Shares));
        }
    }
}