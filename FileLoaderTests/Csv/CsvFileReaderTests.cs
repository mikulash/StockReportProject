using FileLoader.FileParserStrategy;
using FileLoader.FileParserStrategy.CommaSeparatedValues;
using FileLoaderTests.TestUtilities;

namespace FileLoaderTests.Csv;

public class CsvFileReaderTests
{
    private IFileParserStrategy _parser;
    private const string SampleFileDirectory = "CsvTestSamples";

    [SetUp]
    public void SetUp()
    {
        _parser = new CsvParserStrategy();
    }

    [Test]
    public void ParseFileToList_CorrectCsvFile_ReturnsParsedFileAsList()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateFileMockedFileReader("ARK_2024_03_08.csv", SampleFileDirectory);
        
        // act
        var result = _parser.ParseFileToList(readerMock.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(38));

        Assert.That(result.Last().Date, Is.Null);
        var last = result.Last();
        result.Remove(last);
        
        ListUtilities.CheckAllNonNullPropertiesInList(result);
    }

    [Test]
    public void ParseFileToList_EmptyCsvFile_ReturnsEmptyList()
    {
        // arrange
        var readerMock = ReaderUtilities.CreateFileMockedFileReader("ARK_EMPTY.csv", SampleFileDirectory);
        
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
        var readerMock = ReaderUtilities.CreateFileMockedFileReader("ARK_2024_03_08.csv", SampleFileDirectory);
        var readerMockMixed = ReaderUtilities.CreateFileMockedFileReader("ARK_2024_03_08_MIXED.csv", SampleFileDirectory);
        
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