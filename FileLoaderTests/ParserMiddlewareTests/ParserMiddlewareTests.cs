using FileLoader.FileParserStrategy;
using FileLoader.FileParserStrategy.CommaSeparatedValues;
using FileLoader.FileParserStrategy.JavaScriptObjectNotation;
using FileLoader.Model;
using FileLoader.Reader;
using FileLoaderTests.TestUtilities;
using Moq;

namespace FileLoaderTests.ParserMiddlewareTests;

public class ParserMiddlewareTests
{
    private static List<IndexRecordDto> _dummyList = 
        [
            new IndexRecordDto {Company = "TestComp"}, 
            new IndexRecordDto {Company = "TestComp2"}
        ];
    
    [Test]
    public void SetNewParserStrategy_NewStrategy_ReturnsBoundedStrategy()
    {
        // arrange
        var parser = new ParserMiddleware();
        
        // act
        parser.SetNewParserStrategy(FileType.Csv);
        var strategy = parser.ParserStrategy;

        // assert
        Assert.That(strategy, Is.Not.Null);
        Assert.That(strategy, Is.TypeOf<CsvParserStrategy>());
        
        // act
        parser.SetNewParserStrategy(FileType.Json);
        strategy = parser.ParserStrategy;
        
        // assert
        Assert.That(strategy, Is.Not.Null);
        Assert.That(strategy, Is.TypeOf<JsonParserStrategy>());
    }

    [Test]
    public void ParseFileToList_InvalidReader_ReturnsEmptyList()
    {
        // arrange
        var reader = new Mock<IReader>();
        var strategy = new Mock<IFileParserStrategy>();

        reader.Setup(mock => mock.Validate()).Returns(false);
        strategy.Setup(mock => mock.ParseFileToList(reader.Object)).Returns(_dummyList);

        var parser = new ParserMiddleware()
        {
            ParserStrategy = strategy.Object
        };
        
        // act
        var result = parser.ParseFileToList(reader.Object);
        
        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.Empty);
    }

    [Test]
    public void ParseFileToList_ValidReader_ReturnsPredefinedList()
    {
        // arrange
        var reader = ReaderUtilities.CreateMemoryMockedReader(string.Empty);
        var strategy = new Mock<IFileParserStrategy>();
        strategy.Setup(mock => mock.ParseFileToList(reader.Object)).Returns(_dummyList);

        var parser = new ParserMiddleware()
        {
            ParserStrategy = strategy.Object
        };
        
        // act
        var result = parser.ParseFileToList(reader.Object);

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(_dummyList.Count));
        Assert.That(result.Select(item => item.Company), 
            Is.EquivalentTo(_dummyList.Select(item => item.Company)));
    }
}
