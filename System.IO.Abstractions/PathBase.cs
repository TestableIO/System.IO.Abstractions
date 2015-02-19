namespace System.IO.Abstractions
{
    [Serializable]
    public abstract class PathBase
    {
        public abstract char AltDirectorySeparatorChar { get; }
        public abstract char DirectorySeparatorChar { get; }
        [Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
        public abstract char[] InvalidPathChars { get; }
        public abstract char PathSeparator { get; }
        public abstract char VolumeSeparatorChar { get; }

        public abstract string ChangeExtension(string path, string extension);
        public abstract string Combine(params string[] paths);
        public abstract string Combine(string path1, string path2);
        public abstract string Combine(string path1, string path2, string path3);
        public abstract string Combine(string path1, string path2, string path3, string path4);
        public abstract string GetDirectoryName(string path);
        public abstract string GetExtension(string path);
        public abstract string GetFileName(string path);
        public abstract string GetFileNameWithoutExtension(string path);
        public abstract string GetFullPath(string path);
        public abstract char[] GetInvalidFileNameChars();
        public abstract char[] GetInvalidPathChars();
        public abstract string GetPathRoot(string path);
        public abstract string GetRandomFileName();
        public abstract string GetTempFileName();
        public abstract string GetTempPath();
        public abstract bool HasExtension(string path);
        public abstract bool IsPathRooted(string path);
    }
}