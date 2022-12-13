using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace System.IO.Abstractions
{
    /// <summary>
    /// Abstractions for <see cref="Directory" />.
    /// </summary>
    public interface IDirectory : IFileSystemEntity
    {
        /// <inheritdoc cref="Directory.CreateDirectory(string)" />
        IDirectoryInfo CreateDirectory(string path);

#if FEATURE_UNIX_FILE_MODE
	/// <inheritdoc cref="Directory.CreateDirectory(string, UnixFileMode)" />
	IDirectoryInfo CreateDirectory(string path, UnixFileMode unixCreateMode);
#endif

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc cref="Directory.CreateSymbolicLink(string, string)" />
        IFileSystemInfo CreateSymbolicLink(string path, string pathToTarget);
#endif

#if FEATURE_CREATE_TEMP_SUBDIRECTORY
	/// <inheritdoc cref="Directory.CreateTempSubdirectory(string)" />
	IDirectoryInfo CreateTempSubdirectory(string? prefix = null);
#endif

        /// <inheritdoc cref="Directory.Delete(string)" />
        void Delete(string path);

        /// <inheritdoc cref="Directory.Delete(string, bool)" />
        void Delete(string path, bool recursive);

        /// <inheritdoc cref="Directory.EnumerateDirectories(string)" />
        IEnumerable<string> EnumerateDirectories(string path);

        /// <inheritdoc cref="Directory.EnumerateDirectories(string, string)" />
        IEnumerable<string> EnumerateDirectories(string path, string searchPattern);

        /// <inheritdoc cref="Directory.EnumerateDirectories(string, string, SearchOption)" />
        IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="Directory.EnumerateDirectories(string, string, EnumerationOptions)" />
        IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif
        
        /// <inheritdoc cref="Directory.EnumerateFiles(string)" />
        IEnumerable<string> EnumerateFiles(string path);

        /// <inheritdoc cref="Directory.EnumerateFiles(string, string)" />
        IEnumerable<string> EnumerateFiles(string path, string searchPattern);

        /// <inheritdoc cref="Directory.EnumerateFiles(string, string, SearchOption)" />
        IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="Directory.EnumerateFiles(string, string, EnumerationOptions)"/>
        IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string)" />
        IEnumerable<string> EnumerateFileSystemEntries(string path);

        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string, string)" />
        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);

        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string, string, SearchOption)" />
        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string,string,EnumerationOptions)"/>
        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="Directory.Exists(string)" />
        bool Exists([NotNullWhen(true)] string? path);
        
        /// <inheritdoc cref="Directory.GetCreationTime(string)" />
        DateTime GetCreationTime(string path);

        /// <inheritdoc cref="Directory.GetCreationTimeUtc(string)" />
        DateTime GetCreationTimeUtc(string path);

        /// <inheritdoc cref="Directory.GetCurrentDirectory()" />
        string GetCurrentDirectory();

        /// <inheritdoc cref="Directory.GetDirectories(string)" />
        string[] GetDirectories(string path);

        /// <inheritdoc cref="Directory.GetDirectories(string, string)" />
        string[] GetDirectories(string path, string searchPattern);

        /// <inheritdoc cref="Directory.GetDirectories(string, string, SearchOption)" />
        string[] GetDirectories(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="Directory.GetDirectories(string, string, EnumerationOptions)" />
        string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="Directory.GetDirectoryRoot(string)" />
        string GetDirectoryRoot(string path);

        /// <inheritdoc cref="Directory.GetFiles(string)" />
        string[] GetFiles(string path);

        /// <inheritdoc cref="Directory.GetFiles(string, string)" />
        string[] GetFiles(string path, string searchPattern);

        /// <inheritdoc cref="Directory.GetFiles(string, string, SearchOption)" />
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="Directory.GetFiles(string, string, EnumerationOptions)" />
        string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="Directory.GetFileSystemEntries(string)" />
        string[] GetFileSystemEntries(string path);

        /// <inheritdoc cref="Directory.GetFileSystemEntries(string, string)" />
        string[] GetFileSystemEntries(string path, string searchPattern);

        /// <inheritdoc cref="Directory.GetFileSystemEntries(string, string, SearchOption)" />
        string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="Directory.GetFileSystemEntries(string, string, EnumerationOptions)" />
        string[] GetFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="Directory.GetLastAccessTime(string)" />
        DateTime GetLastAccessTime(string path);

        /// <inheritdoc cref="Directory.GetLastAccessTimeUtc(string)" />
        DateTime GetLastAccessTimeUtc(string path);

        /// <inheritdoc cref="Directory.GetLastWriteTime(string)" />
        DateTime GetLastWriteTime(string path);

        /// <inheritdoc cref="Directory.GetLastWriteTimeUtc(string)" />
        DateTime GetLastWriteTimeUtc(string path);

        /// <inheritdoc cref="Directory.GetLogicalDrives()" />
        string[] GetLogicalDrives();

        /// <inheritdoc cref="Directory.GetParent(string)" />
        IDirectoryInfo? GetParent(string path);

        /// <inheritdoc cref="Directory.Move(string, string)" />
        void Move(string sourceDirName, string destDirName);

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc cref="Directory.ResolveLinkTarget(string, bool)" />
        IFileSystemInfo? ResolveLinkTarget(string linkPath, bool returnFinalTarget);
#endif

        /// <inheritdoc cref="Directory.SetCreationTime(string, DateTime)" />
        void SetCreationTime(string path, DateTime creationTime);

        /// <inheritdoc cref="Directory.SetCreationTimeUtc(string, DateTime)" />
        void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <inheritdoc cref="Directory.SetCurrentDirectory(string)" />
        void SetCurrentDirectory(string path);

        /// <inheritdoc cref="Directory.SetLastAccessTime(string, DateTime)" />
        void SetLastAccessTime(string path, DateTime lastAccessTime);

        /// <inheritdoc cref="Directory.SetLastAccessTimeUtc(string, DateTime)" />
        void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <inheritdoc cref="Directory.SetLastWriteTime(string, DateTime)" />
        void SetLastWriteTime(string path, DateTime lastWriteTime);

        /// <inheritdoc cref="Directory.SetLastWriteTimeUtc(string, DateTime)" />
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);
    }
}