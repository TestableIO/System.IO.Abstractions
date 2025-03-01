namespace System.IO.Abstractions;

/// <inheritdoc cref="Path"/>
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
public abstract class PathBase : IPath
{
    /// <summary>
    /// Base class for calling static methods of <see cref="Path"/>
    /// </summary>
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

    /// <inheritdoc cref="Path.InvalidPathChars"/>
    [Obsolete("Please use GetInvalidPathChars or GetInvalidFileNameChars instead.")]
    public abstract char[] InvalidPathChars { get; }

    /// <inheritdoc cref="Path.PathSeparator"/>
    public abstract char PathSeparator { get; }

    /// <inheritdoc cref="Path.VolumeSeparatorChar"/>
    public abstract char VolumeSeparatorChar { get; }

    /// <inheritdoc cref="Path.ChangeExtension"/>
    public abstract string ChangeExtension(string path, string extension);

    /// <inheritdoc cref="Path.Combine(string[])"/>
    public abstract string Combine(params string[] paths);
        
#if FEATURE_PATH_SPAN
        /// <inheritdoc cref="Path.Combine(ReadOnlySpan{string})"/>
        public abstract string Combine(params ReadOnlySpan<string> paths);
#endif

    /// <inheritdoc cref="Path.Combine(string,string)"/>
    public abstract string Combine(string path1, string path2);

    /// <inheritdoc cref="Path.Combine(string,string,string)"/>
    public abstract string Combine(string path1, string path2, string path3);

    /// <inheritdoc cref="Path.Combine(string,string,string,string)"/>
    public abstract string Combine(string path1, string path2, string path3, string path4);

#if FEATURE_PATH_EXISTS
    /// <inheritdoc cref="Path.Exists(string)" />
    public abstract bool Exists(string path);
#endif

    /// <inheritdoc cref="Path.GetDirectoryName(string)"/>
    public abstract string GetDirectoryName(string path);

    /// <inheritdoc cref="Path.GetExtension(string)"/>
    public abstract string GetExtension(string path);

    /// <inheritdoc cref="Path.GetFileName(string)"/>
    public abstract string GetFileName(string path);

    /// <inheritdoc cref="Path.GetFileNameWithoutExtension(string)"/>
    public abstract string GetFileNameWithoutExtension(string path);

    /// <inheritdoc cref="Path.GetFullPath(string)"/>
    public abstract string GetFullPath(string path);

#if FEATURE_ADVANCED_PATH_OPERATIONS
    /// <inheritdoc cref="Path.GetFullPath(string, string)"/>
    public abstract string GetFullPath(string path, string basePath);
#endif

    /// <inheritdoc cref="Path.GetInvalidFileNameChars"/>
    public abstract char[] GetInvalidFileNameChars();

    /// <inheritdoc cref="Path.GetInvalidPathChars"/>
    public abstract char[] GetInvalidPathChars();

    /// <inheritdoc cref="Path.GetPathRoot(string)"/>
    public abstract string GetPathRoot(string path);

    /// <inheritdoc cref="Path.GetRandomFileName"/>
    public abstract string GetRandomFileName();

    /// <inheritdoc cref="Path.GetTempFileName"/>
    public abstract string GetTempFileName();

    /// <inheritdoc cref="Path.GetTempPath"/>
    public abstract string GetTempPath();

    /// <inheritdoc cref="Path.HasExtension(string)"/>
    public abstract bool HasExtension(string path);

    /// <inheritdoc cref="Path.IsPathRooted(string)"/>
    public abstract bool IsPathRooted(string path);

#if FEATURE_ADVANCED_PATH_OPERATIONS
    /// <inheritdoc />
    public abstract bool IsPathFullyQualified(string path);

    /// <inheritdoc />
    public abstract string GetRelativePath(string relativeTo, string path);
#endif

#if FEATURE_PATH_JOIN_WITH_SPAN
    /// <inheritdoc />
    public abstract string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2);

    /// <inheritdoc />
    public abstract string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3);

#if FEATURE_PATH_SPAN
        /// <inheritdoc cref="Path.Join(ReadOnlySpan{string})"/>
        public abstract string Join(params ReadOnlySpan<string> paths);
#endif

    /// <inheritdoc />
    public abstract bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, Span<char> destination, out int charsWritten);

    /// <inheritdoc />
    public abstract bool TryJoin(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, Span<char> destination, out int charsWritten);
#endif

#if FEATURE_ADVANCED_PATH_OPERATIONS
    /// <inheritdoc />
    public abstract bool HasExtension(ReadOnlySpan<char> path);
    /// <inheritdoc />
    public abstract bool IsPathFullyQualified(ReadOnlySpan<char> path);
    /// <inheritdoc />
    public abstract bool IsPathRooted(ReadOnlySpan<char> path);
    /// <inheritdoc />
    public abstract ReadOnlySpan<char> GetDirectoryName(ReadOnlySpan<char> path);
    /// <inheritdoc />
    public abstract ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> path);
    /// <inheritdoc />
    public abstract ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path);
    /// <inheritdoc />
    public abstract ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path);
    /// <inheritdoc />
    public abstract ReadOnlySpan<char> GetPathRoot(ReadOnlySpan<char> path);

#endif
#if FEATURE_PATH_JOIN_WITH_PARAMS
    /// <inheritdoc />
    public abstract string Join(params string[] paths);

    /// <inheritdoc />
    public abstract string Join(string path1, string path2);
    /// <inheritdoc />

    public abstract string Join(string path1, string path2, string path3);

#endif

#if FEATURE_ENDS_IN_DIRECTORY_SEPARATOR
    /// <inheritdoc />
    public abstract bool EndsInDirectorySeparator(ReadOnlySpan<char> path);
    /// <inheritdoc />
    public abstract bool EndsInDirectorySeparator(string path);
    /// <inheritdoc />
    public abstract ReadOnlySpan<char> TrimEndingDirectorySeparator(ReadOnlySpan<char> path);

    /// <inheritdoc />
    public abstract string TrimEndingDirectorySeparator(string path);
#endif

#if FEATURE_PATH_JOIN_WITH_FOUR_PATHS

    /// <inheritdoc />
    public abstract string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2, ReadOnlySpan<char> path3, ReadOnlySpan<char> path4);
    /// <inheritdoc />
    public abstract string Join(string path1, string path2, string path3, string path4);
#endif
}