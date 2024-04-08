namespace FileLoader.Reader;

public class FileReader : IReader, IDisposable
{
    private const int ReaderNotOpenIndicator = -1;
    
    public FileReader(string pathToFile)
    {
        Reader = new StreamReader(pathToFile);
        PathToFile = pathToFile;
    }

    public string PathToFile { get; private set; }
    public TextReader Reader { get; init; }

    public bool Validate() => Reader.Peek() != ReaderNotOpenIndicator;

    public void Dispose()
    {
        Reader.Close();
        Reader.Dispose();
    }
}