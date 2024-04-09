namespace FileLoader.Reader;

public abstract class DisposableBaseReader : IReader, IDisposable
{
    protected bool Disposed = false;
    public abstract TextReader Reader { get; init; }
    
    public abstract bool Validate();

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