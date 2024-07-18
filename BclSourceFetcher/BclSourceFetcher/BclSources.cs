using System.Text.Json;

namespace BclSourceFetcher;

/// <summary>
/// Represents a collection of the .NET BCL and URLs to their
/// source code hosted on GitHub.
/// </summary>
public class BclSources : IDisposable, IAsyncDisposable
{
    private BclObject[]? _bclSourceUrls; // Keep this nullable
    private bool disposedValue;

    internal BclSources(BclObject[] sources)
    {
        _bclSourceUrls = sources;
    }

    /// <summary>
    /// Loads a new instance of <see cref="BclSources"/> class from an existing JSON file
    /// on disk.
    /// </summary>
    /// <param name="jsonFile">The file path to the JSON file that contains BCL sources.</param>
    /// <returns>A new instance of <see cref="BclSources"/>.</returns>
    public static BclSources LoadFrom(string jsonFile)
    {
        ThrowHelper.FileNotFound(jsonFile);

        string content = File.ReadAllText(jsonFile);
        return Load(content);
    }

    /// <summary>
    /// Loads a new instance of <see cref="BclSources"/> class from the JSON string that contains BCL sources.
    /// </summary>
    /// <param name="jsonContents">String contents of the JSON file that specifies BCL sources.</param>
    /// <returns>A new instance of <see cref="BclSources"/></returns>
    public static BclSources Load(string jsonContents)
    {
        return new BclSources(JsonSerializer.Deserialize<BclObject[]>(jsonContents)!);
    }

    /// <summary>
    /// Returns the source code URL for the given type name (f.e. <c>System.Console</c>)
    /// </summary>
    /// <param name="typeName">The name of the type to get source code for.</param>
    /// <returns>A string that represents the URL which contains the source code of the given object name.</returns>
    public string? GetUrlOfTypeName(string typeName)
    {
        ThrowHelper.ObjectDisposed(_bclSourceUrls);

        if (_bclSourceUrls!.Any(src => src.Name.Equals(typeName)) == false)
        {
            return null;
        }

        return _bclSourceUrls!.First(obj => obj.Name.Equals(typeName)).Url;
    }

    /// <summary>
    /// Gets the URL for the given type.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <returns>A string that represents the URL which contains the source code of the given object name.</returns>
    public string? GetUrlOfType(Type type) => GetUrlOfTypeName(type.FullName ?? type.Name);

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
            }

            _bclSourceUrls ??= null;
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Run(() =>
        {
            _bclSourceUrls ??= null;
        });
        GC.SuppressFinalize(this);
    }
}
