namespace System.IO.Abstractions
{
    /// <inheritdoc cref="Path"/>
    [Serializable]
    public abstract class PathBase : IPath
    {
        protected PathBase(IFileSystem fileSystem)
        {
            this.FileSystem = fileSystem;
        }

        [Obsolete("This constructor only exists to support mocking libraries.", error: true)]
        internal PathBase() { }

        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        public IFileSystem FileSystem { get; }

        /// <inheritdoc cref="Path.AltDirectorySeparatorChar"/>
        public abstract char AltDirectorySeparatorChar { get; }

        /// <inheritdoc cref="Path.DirectorySeparatorChar"/>
        public abstract char DirectorySeparatorChar { get; }

#if NET40
        /// <inheritdoc cref="Path.InvalidPathChars"/>
        [Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
        public abstract char[] InvalidPathChars { get; }
#endif

        /// <inheritdoc cref="Path.PathSeparator"/>
        public abstract char PathSeparator { get; }

        /// <inheritdoc cref="Path.VolumeSeparatorChar"/>
        public abstract char VolumeSeparatorChar { get; }

        /// <inheritdoc cref="Path.ChangeExtension"/>
        public abstract string ChangeExtension(string path, string extension);

        /// <inheritdoc cref="Path.Combine(string[])"/>
        public abstract string Combine(params string[] paths);

        /// <inheritdoc cref="Path.Combine(string,string)"/>
        public abstract string Combine(string path1, string path2);

        /// <inheritdoc cref="Path.Combine(string,string,string)"/>
        public abstract string Combine(string path1, string path2, string path3);

        /// <inheritdoc cref="Path.Combine(string,string,string,string)"/>
        public abstract string Combine(string path1, string path2, string path3, string path4);

        /// <inheritdoc cref="Path.GetDirectoryName"/>
        public abstract string GetDirectoryName(string path);

        /// <inheritdoc cref="Path.GetExtension"/>
        public abstract string GetExtension(string path);

        /// <inheritdoc cref="Path.GetFileName"/>
        public abstract string GetFileName(string path);

        /// <inheritdoc cref="Path.GetFileNameWithoutExtension"/>
        public abstract string GetFileNameWithoutExtension(string path);

        /// <inheritdoc cref="Path.GetFullPath"/>
        public abstract string GetFullPath(string path);

        /// <inheritdoc cref="Path.GetInvalidFileNameChars"/>
        public abstract char[] GetInvalidFileNameChars();

        /// <inheritdoc cref="Path.GetInvalidPathChars"/>
        public abstract char[] GetInvalidPathChars();

        /// <inheritdoc cref="Path.GetPathRoot"/>
        public abstract string GetPathRoot(string path);

        /// <inheritdoc cref="Path.GetRandomFileName"/>
        public abstract string GetRandomFileName();

        /// <inheritdoc cref="Path.GetTempFileName"/>
        public abstract string GetTempFileName();

        /// <inheritdoc cref="Path.GetTempPath"/>
        public abstract string GetTempPath();

        /// <inheritdoc cref="Path.HasExtension"/>
        public abstract bool HasExtension(string path);

        /// <inheritdoc cref="Path.IsPathRooted"/>
        public abstract bool IsPathRooted(string path);
    }
}
