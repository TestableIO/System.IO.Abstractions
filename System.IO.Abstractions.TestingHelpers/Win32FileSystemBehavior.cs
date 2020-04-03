namespace System.IO.Abstractions.TestingHelpers
{
    internal static class Win32FileSystemBehavior
    {
        internal static bool MoveFileCanParsePath(string fullPath)
        {
            fullPath = fullPath.Trim(Path.DirectorySeparatorChar);
            var dir = Path.IsPathRooted(fullPath)
                ? fullPath.Replace(Path.GetPathRoot(fullPath), string.Empty)
                : fullPath.Replace($".{Path.DirectorySeparatorChar}", string.Empty);

            const int DirectorySegmentsAllowed = 1;
            var isSupported = dir.Split(Path.DirectorySeparatorChar).Length <= DirectorySegmentsAllowed;
            return isSupported;
        }
    }        
}
