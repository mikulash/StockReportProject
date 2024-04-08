namespace FileLoader.Reader;

public interface IReader
{
    TextReader Reader { get; init; }
    bool Validate();
}