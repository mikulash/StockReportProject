namespace FileLoader.Reader;

public class FileReader : DisposableBaseReader
{
    public string PathToFile { get; private set; }
    public FileReader(string pathToFile)
    {
        Reader = new StreamReader(pathToFile);
        PathToFile = pathToFile;
    }
}
