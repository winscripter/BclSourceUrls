namespace BclSourceFetcher;

internal static class ThrowHelper
{
    private static readonly Lazy<ObjectDisposedException> s_objectDisposed = new Lazy<ObjectDisposedException>(() => new ObjectDisposedException("Object has been disposed"));

    public static void FileNotFound(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }
    }

    public static void ObjectDisposed<T>(T? obj)
    {
        if (obj == null) return;
        throw s_objectDisposed.Value;
    }
}
