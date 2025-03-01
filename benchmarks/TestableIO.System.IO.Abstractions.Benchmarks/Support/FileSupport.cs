namespace System.IO.Abstractions.Benchmarks.Support;

public class FileSupport
{
    private readonly IFileSystem _fileSystem;

    public FileSupport(IFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
    }

    public FileSupport() : this(new FileSystem())
    {
        // Default implementation for FileSystem
    }

    private static string GetRandomTempFile()
    {
        return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    }

    public bool IsFile(string path)
    {
        return _fileSystem.File.Exists(path);
    }

    /// <summary>
    /// Checks and deletes given file if it does exists.
    /// </summary>
    /// <param name="filePath">Path of the file</param>
    public void DeleteIfExists(string filePath)
    {
        if (_fileSystem.File.Exists(filePath))
        {
            _fileSystem.File.Delete(filePath);
        }
    }
}