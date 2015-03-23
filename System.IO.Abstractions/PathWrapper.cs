namespace System.IO.Abstractions
{
    [Serializable]
    public class PathWrapper : PathBase
    {
        public override char AltDirectorySeparatorChar
        {
            get { return Path.AltDirectorySeparatorChar; }
        }

        public override char DirectorySeparatorChar
        {
            get { return Path.DirectorySeparatorChar; }
        }

        [Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
        public override char[] InvalidPathChars
        {
            get { return Path.InvalidPathChars; }
        }

        public override char PathSeparator
        {
            get { return Path.PathSeparator; }
        }

        public override char VolumeSeparatorChar
        {
            get { return Path.VolumeSeparatorChar; }
        }

        public override string ChangeExtension(string path, string extension)
        {
            return Path.ChangeExtension(path, extension);
        }

        public override string Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        public override string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public override string Combine(string path1, string path2, string path3)
        {
            return Path.Combine(path1, path2, path3);
        }

        public override string Combine(string path1, string path2, string path3, string path4)
        {
            return Path.Combine(path1, path2, path3, path4);
        }

        public override string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public override string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        public override string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        public override string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        public override string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

        public override char[] GetInvalidFileNameChars()
        {
            return Path.GetInvalidFileNameChars();
        }

        public override char[] GetInvalidPathChars()
        {
            return Path.GetInvalidPathChars();
        }

        public override string GetPathRoot(string path)
        {
            return Path.GetPathRoot(path);
        }

        public override string GetRandomFileName()
        {
            return Path.GetRandomFileName();
        }

        public override string GetTempFileName()
        {
            return Path.GetTempFileName();
        }

        public override string GetTempPath()
        {
            return Path.GetTempPath();
        }

        public override bool HasExtension(string path)
        {
            return Path.HasExtension(path);
        }

        public override bool IsPathRooted(string path)
        {
            return Path.IsPathRooted(path);
        }
    }
}
