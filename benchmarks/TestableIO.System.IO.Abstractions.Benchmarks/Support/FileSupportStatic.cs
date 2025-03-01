using System.IO;

namespace System.IO.Abstractions.Benchmarks.Support;

public static class FileSupportStatic
{
    public static string GetRandomTempFile()
    {
        return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    }

    public static bool IsFile(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// Checks and deletes given file if it does exists.
    /// </summary>
    /// <param name="filePath">Path of the file</param>
    public static void DeleteIfExists(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}