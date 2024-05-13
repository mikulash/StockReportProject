namespace FileLoader.Reader;

public class BinaryStreamReader : DisposableBaseReader
{
    public BinaryStreamReader(Stream stream)
    {
        Reader = new StreamReader(stream);
    }
}