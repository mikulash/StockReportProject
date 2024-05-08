namespace FileLoader.Reader;

public abstract class DisposableBaseReader : IReader, IDisposable
{
    protected readonly int ReaderNotOpenIndicator = -1;
    protected bool Disposed = false;
    
    public virtual TextReader Reader { get; init; }
    
    public virtual bool Validate() => Reader.Peek() != ReaderNotOpenIndicator;

    protected virtual void Dispose(bool disposing)
    {
        if (Disposed)
        {
            return;
        }

        if (disposing)
        {
            Reader.Dispose();
        }

        Disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}