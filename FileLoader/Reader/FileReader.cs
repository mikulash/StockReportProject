namespace FileLoader.Reader;

public class FileReader : DisposableBaseReader
{
    private const int ReaderNotOpenIndicator = -1;
    
    public FileReader(string pathToFile)
    {
        Reader = new StreamReader(pathToFile);
        PathToFile = pathToFile;
    }

    public string PathToFile { get; private set; }
    public override TextReader Reader { get; init; }

    public override bool Validate() => Reader.Peek() != ReaderNotOpenIndicator;
}