using FileLoader.Reader;

namespace FileLoaderTests.ReaderTests;

public class FileReaderTests
{
    [Test]
    public void Reader_CreateValidReader_ReturnsNewReader()
    {
        // arrange
        var reader = new FileReader(Path.Combine(Environment.CurrentDirectory, "CsvTestSamples", "ARK_EMPTY.csv"));
        
        // act
        var valid = reader.Validate();
        var text = reader.Reader.ReadToEnd();
        
        // assert
        Assert.That(valid, Is.True);
        Assert.That(text, Is.Not.Null);
        Assert.That(text, Is.Not.Empty);
    }

    [Test]
    public void Reader_InvalidFilePath_ThrowsException()
    {
        // act & assert
        Assert.Throws<FileNotFoundException>(() 
            => new FileReader(Path.Combine(Environment.CurrentDirectory, "SomeFile.not.txt")));
    }
}