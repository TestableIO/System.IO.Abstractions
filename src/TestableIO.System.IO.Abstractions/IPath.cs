using System.Diagnostics.CodeAnalysis;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="Path" />
    public interface IPath : IFileSystemEntity
    {
        /// <inheritdoc cref="Path.AltDirectorySeparatorChar" />
        char AltDirectorySeparatorChar { get; }

        /// <inheritdoc cref="Path.DirectorySeparatorChar" />
        char DirectorySeparatorChar { get; }

        /// <inheritdoc cref="Path.PathSeparator" />
        char PathSeparator { get; }

        /// <inheritdoc cref="Path.VolumeSeparatorChar" />
        char VolumeSeparatorChar { get; }

        /// <inheritdoc cref="Path.ChangeExtension(string, string)" />
        [return: NotNullIfNotNull("path")]
        string? ChangeExtension(string? path, string? extension);

        /// <inheritdoc cref="Path.Combine(string, string)" />
        string Combine(string path1, string path2);

        /// <inheritdoc cref="Path.Combine(string, string, string)" />
        string Combine(string path1, string path2, string path3);

        /// <inheritdoc cref="Path.Combine(string, string, string, string)" />
        string Combine(string path1, string path2, string path3, string path4);

        /// <inheritdoc cref="Path.Combine(string[])" />
        string Combine(params string[] paths);

#if FEATURE_PATH_SPAN
        /// <inheritdoc cref="Path.Combine(ReadOnlySpan{string})" />
        string Combine(params ReadOnlySpan<string> paths);
#endif

#if FEATURE_ENDS_IN_DIRECTORY_SEPARATOR
        /// <inheritdoc cref="Path.EndsInDirectorySeparator(ReadOnlySpan{char})" />
        bool EndsInDirectorySeparator(ReadOnlySpan<char> path);

	    /// <inheritdoc cref="Path.EndsInDirectorySeparator(string)" />
	    bool EndsInDirectorySeparator(string path);
#endif

#if FEATURE_PATH_EXISTS
        /// <inheritdoc cref="Path.Exists(string)" />
        bool Exists([NotNullWhen(true)] string? path);
#endif

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc cref="Path.GetDirectoryName(ReadOnlySpan{char})" />
        ReadOnlySpan<char> GetDirectoryName(ReadOnlySpan<char> path);
#endif

        /// <inheritdoc cref="Path.GetDirectoryName(string)" />
        string? GetDirectoryName(string? path);

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc cref="Path.GetExtension(ReadOnlySpan{char})" />
        ReadOnlySpan<char> GetExtension(ReadOnlySpan<char> path);
#endif

        /// <inheritdoc cref="Path.GetExtension(string)" />
        [return: NotNullIfNotNull("path")]
        string? GetExtension(string? path);

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc cref="Path.GetFileName(ReadOnlySpan{char})" />
        ReadOnlySpan<char> GetFileName(ReadOnlySpan<char> path);
#endif

        /// <inheritdoc cref="Path.GetFileName(string)" />
        [return: NotNullIfNotNull("path")]
        string? GetFileName(string? path);

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc cref="Path.GetFileNameWithoutExtension(ReadOnlySpan{char})" />
        ReadOnlySpan<char> GetFileNameWithoutExtension(ReadOnlySpan<char> path);
#endif

        /// <inheritdoc cref="Path.GetFileNameWithoutExtension(string)" />
        [return: NotNullIfNotNull("path")]
        string? GetFileNameWithoutExtension(string? path);

        /// <inheritdoc cref="Path.GetFullPath(string)" />
        string GetFullPath(string path);

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc cref="Path.GetFullPath(string, string)" />
        string GetFullPath(string path, string basePath);
#endif

        /// <inheritdoc cref="Path.GetInvalidFileNameChars()" />
        char[] GetInvalidFileNameChars();

        /// <inheritdoc cref="Path.GetInvalidPathChars()" />
        char[] GetInvalidPathChars();

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc cref="Path.GetPathRoot(ReadOnlySpan{char})" />
        ReadOnlySpan<char> GetPathRoot(ReadOnlySpan<char> path);
#endif

        /// <inheritdoc cref="Path.GetPathRoot(string?)" />
        string? GetPathRoot(string? path);

        /// <inheritdoc cref="Path.GetRandomFileName()" />
        string GetRandomFileName();

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc cref="Path.GetRelativePath(string, string)" />
        string GetRelativePath(string relativeTo, string path);
#endif

        /// <inheritdoc cref="Path.GetTempFileName()" />
        string GetTempFileName();

        /// <inheritdoc cref="Path.GetTempPath()" />
        string GetTempPath();

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc cref="Path.HasExtension(ReadOnlySpan{char})" />
        bool HasExtension(ReadOnlySpan<char> path);
#endif

        /// <inheritdoc cref="Path.HasExtension(string)" />
        bool HasExtension([NotNullWhen(true)] string? path);

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc cref="Path.IsPathFullyQualified(ReadOnlySpan{char})" />
        bool IsPathFullyQualified(ReadOnlySpan<char> path);

        /// <inheritdoc cref="Path.IsPathFullyQualified(string)" />
        bool IsPathFullyQualified(string path);

        /// <inheritdoc cref="Path.IsPathRooted(ReadOnlySpan{char})" />
        bool IsPathRooted(ReadOnlySpan<char> path);
#endif

        /// <inheritdoc cref="Path.IsPathRooted(string?)" />
        bool IsPathRooted([NotNullWhen(true)] string? path);

#if FEATURE_PATH_JOIN_WITH_SPAN
        /// <inheritdoc cref="Path.Join(ReadOnlySpan{char}, ReadOnlySpan{char})" />
        string Join(ReadOnlySpan<char> path1, ReadOnlySpan<char> path2);

        /// <inheritdoc cref="Path.Join(ReadOnlySpan{char}, ReadOnlySpan{char}, ReadOnlySpan{char})" />
        string Join(ReadOnlySpan<char> path1,
            ReadOnlySpan<char> path2,
            ReadOnlySpan<char> path3);

#if FEATURE_PATH_SPAN
        /// <inheritdoc cref="Path.Join(ReadOnlySpan{string?})" />
        string Join(params ReadOnlySpan<string?> paths);
#endif

        /// <inheritdoc cref="Path.TryJoin(ReadOnlySpan{char}, ReadOnlySpan{char}, Span{char}, out int)" />
        bool TryJoin(ReadOnlySpan<char> path1,
            ReadOnlySpan<char> path2,
            Span<char> destination,
            out int charsWritten);

        /// <inheritdoc cref="Path.TryJoin(ReadOnlySpan{char}, ReadOnlySpan{char}, ReadOnlySpan{char}, Span{char}, out int)" />
        bool TryJoin(ReadOnlySpan<char> path1,
            ReadOnlySpan<char> path2,
            ReadOnlySpan<char> path3,
            Span<char> destination,
            out int charsWritten);
#endif

#if FEATURE_PATH_JOIN_WITH_PARAMS
        /// <inheritdoc cref="Path.Join(string, string)" />
        string Join(string? path1, string? path2);

        /// <inheritdoc cref="Path.Join(string, string, string)" />
        string Join(string? path1, string? path2, string? path3);

        /// <inheritdoc cref="Path.Join(string[])" />
        string Join(params string?[] paths);
#endif

#if FEATURE_ENDS_IN_DIRECTORY_SEPARATOR
        /// <inheritdoc cref="Path.TrimEndingDirectorySeparator(ReadOnlySpan{char})" />
        ReadOnlySpan<char> TrimEndingDirectorySeparator(ReadOnlySpan<char> path);

        /// <inheritdoc cref="Path.TrimEndingDirectorySeparator(string)" />
        string TrimEndingDirectorySeparator(string path);
#endif

#if FEATURE_PATH_JOIN_WITH_FOUR_PATHS
        /// <inheritdoc cref="Path.Join(ReadOnlySpan{char}, ReadOnlySpan{char}, ReadOnlySpan{char}, ReadOnlySpan{char})" />
        string Join(ReadOnlySpan<char> path1,
            ReadOnlySpan<char> path2,
            ReadOnlySpan<char> path3,
            ReadOnlySpan<char> path4);

        /// <inheritdoc cref="Path.Join(string, string, string, string)" />
        string Join(string? path1, string? path2, string? path3, string? path4);
#endif
    }
}