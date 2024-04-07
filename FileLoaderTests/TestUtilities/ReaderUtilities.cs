using FileLoader.Reader;
using Moq;

namespace FileLoaderTests.TestUtilities;

public static class ReaderUtilities
{
    public static TextReader CreateStringReader(string source) => new StringReader(source);

    public static Mock<IReader> CreateMemoryMockedReader(string source)
    {
        var readerMock = new Mock<IReader>();
        readerMock.Setup(mock => mock.Validate()).Returns(true);
        readerMock.Setup(mock => mock.Reader).Returns(CreateStringReader(source));

        return readerMock;
    }

    public static TextReader CreateFileReader(string fileName, string sampleFileDirectory)
    {
        var path = Path.Combine(Environment.CurrentDirectory, sampleFileDirectory,fileName);

        return new StreamReader(path);
    }

    public static Mock<IReader> CreateFileMockedFileReader(string fileName, string sampleFileDirectory)
    {
        var readerMock = new Mock<IReader>();
        readerMock.Setup(mock => mock.Validate()).Returns(true);
        readerMock.Setup(mock => mock.Reader).Returns(CreateFileReader(fileName, sampleFileDirectory));
        
        return readerMock;
    }
    
    
}