using System.Diagnostics.CodeAnalysis;

namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class PathWrapper : PathBase
    {
        /// <inheritdoc />
        public PathWrapper(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        /// <inheritdoc />
        public override char AltDirectorySeparatorChar
        {
            get { return Path.AltDirectorySeparatorChar; }
        }

        /// <inheritdoc />
        public override char DirectorySeparatorChar
        {
            get { return Path.DirectorySeparatorChar; }
        }

        /// <inheritdoc />
        [Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
        public override char[] InvalidPathChars
        {
            get { return Path.InvalidPathChars; }
        }

        /// <inheritdoc />
        public override char PathSeparator
        {
            get { return Path.PathSeparator; }
        }

        /// <inheritdoc />
        public override char VolumeSeparatorChar
        {
            get { return Path.VolumeSeparatorChar; }
        }

        /// <inheritdoc />
        public override string ChangeExtension(string path, string extension)
        {
            return Path.ChangeExtension(path, extension);
        }

        /// <inheritdoc />
        public override string Combine(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <inheritdoc />
        public override string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <inheritdoc />
        public override string Combine(string path1, string path2, string path3)
        {
            return Path.Combine(path1, path2, path3);
        }

        /// <inheritdoc />
        public override string Combine(string path1, string path2, string path3, string path4)
        {
            return Path.Combine(path1, path2, path3, path4);
        }

#if FEATURE_FILESYSTEM_NET7
        /// <inheritdoc />
        public override bool Exists(string path)
        {
            return Path.Exists(path);
        }
#endif

        /// <inheritdoc />
        public override string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <inheritdoc />
        public override string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }

        /// <inheritdoc />
        public override string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <inheritdoc />
        public override string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <inheritdoc />
        public override string GetFullPath(string path)
        {
            return Path.GetFullPath(path);
        }

# if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc />
        public override string GetFullPath(string path, string basePath)
        {
            return Path.GetFullPath(path, basePath);
        }
#endif

        /// <inheritdoc />
        public override char[] GetInvalidFileNameChars()
        {
            return Path.GetInvalidFileNameChars();
        }

        /// <inheritdoc />
        public override char[] GetInvalidPathChars()
        {
            return Path.GetInvalidPathChars();
        }

        /// <inheritdoc />
        public override string GetPathRoot(string path)
        {
            return Path.GetPathRoot(path);
        }

        /// <inheritdoc />
        public override string GetRandomFileName()
        {
            return Path.GetRandomFileName();
        }

        /// <inheritdoc />
        public override string GetTempFileName()
        {
            return Path.GetTempFileName();
        }

        /// <inheritdoc />
        public override string GetTempPath()
        {
            return Path.GetTempPath();
        }

        /// <inheritdoc />
        public override bool HasExtension(string path)
        {
            return Path.HasExtension(path);
        }

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc />
        public override bool IsPathFullyQualified(string path)
        {
            return Path.IsPathFullyQualified(path);
        }

        /// <inheritdoc />
        public override string GetRelativePath(string relativeTo, string path)
        {
            return Path.GetRelativePath(relativeTo, path);
        }
#endif

#if FEATURE_PATH_JOIN_WITH_SPAN
        /// <inheritdoc />
        public override string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2) =>
            Path.Join(path1, path2);

        /// <inheritdoc />
        public override string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3) =>
            Path.Join(path1, path2, path3);

        /// <inheritdoc />
        public override bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, Span<char> destination, out int charsWritten) =>
            Path.TryJoin(path1, path2, destination, out charsWritten);

        /// <inheritdoc />
        public override bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, Span<char> destination, out int charsWritten) =>
            Path.TryJoin(path1, path2, path3, destination, out charsWritten);
#endif

        /// <inheritdoc />
        public override bool IsPathRooted(string path)
        {
            return Path.IsPathRooted(path);
        }

#if FEATURE_ENDS_IN_DIRECTORY_SEPARATOR
        /// <inheritdoc />
        public override bool EndsInDirectorySeparator(ReadOnlySpan<char> path)
        {
            return Path.EndsInDirectorySeparator(path);
        }

        /// <inheritdoc />
        public override bool EndsInDirectorySeparator(string path)
        {
            return Path.EndsInDirectorySeparator(path);
        }

        /// <inheritdoc />
        public override ReadOnlySpan<char> TrimEndingDirectorySeparator(ReadOnlySpan<char> path)
        {
            return Path.TrimEndingDirectorySeparator(path);
        }

        /// <inheritdoc />
        public override string TrimEndingDirectorySeparator(string path)
        {
            return Path.TrimEndingDirectorySeparator(path);
        }
#endif

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc />
        public override bool HasExtension(ReadOnlySpan<char> path)
        {
            return Path.HasExtension(path);
        }

        /// <inheritdoc />
        public override bool IsPathFullyQualified(ReadOnlySpan<char> path)
        {
            return Path.IsPathFullyQualified(path);
        }

        /// <inheritdoc />
        public override bool IsPathRooted(ReadOnlySpan<char> path)
        {
            return Path.IsPathRooted(path);
        }

        /// <inheritdoc />
        public override ReadOnlySpan<char> GetDirectoryName(ReadOnlySpan<char> path)
        {
            return Path.GetDirectoryName(path);
        }

        /// <inheritdoc />
        public override ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> path)
        {
            return Path.GetExtension(path);
        }

        /// <inheritdoc />
        public override ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path)
        {
            return Path.GetFileName(path);
        }

        /// <inheritdoc />
        public override ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <inheritdoc />
        public override ReadOnlySpan<char> GetPathRoot(ReadOnlySpan<char> path)
        {
            return Path.GetPathRoot(path);
        }
#endif

#if FEATURE_PATH_JOIN_WITH_FOUR_PATHS
        /// <inheritdoc />
        public override string Join(string path1, string path2, string path3, string path4)
        {
            return Path.Join(path1, path2, path3, path4);
        }

        /// <inheritdoc />
        public override string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, ReadOnlySpan<char> path4)
        {
            return Path.Join(path1, path2, path3, path4);
        }
#endif

#if FEATURE_PATH_JOIN_WITH_PARAMS
        /// <inheritdoc />
        public override string Join(string path1, string path2)
        {
            return Path.Join(path1, path2);
        }

        /// <inheritdoc />
        public override string Join(string path1, string path2, string path3)
        {
            return Path.Join(path1, path2, path3);
        }

        /// <inheritdoc />
        public override string Join(params string[] paths)
        {
            return Path.Join(paths);
        }
#endif
    }
}
