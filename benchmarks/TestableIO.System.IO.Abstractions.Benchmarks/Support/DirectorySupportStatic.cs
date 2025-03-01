namespace System.IO.Abstractions.Benchmarks.Support;

public static class DirectorySupportStatic
{
    public static bool IsDirectory(string path)
    {
        return Directory.Exists(path);
    }

    private static string GetRandomTempDirectory()
    {
        return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
    }

    public static string CreateDirectory()
    {
        var randomPath = GetRandomTempDirectory();
        Directory.CreateDirectory(randomPath);
        return randomPath;
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true, bool overwrite = true)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, overwrite);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }

    public static void CreateIfNotExists(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public static bool Exists(string directory) => Directory.Exists(directory);
}