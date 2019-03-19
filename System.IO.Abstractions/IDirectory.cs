using System.Collections.Generic;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    public interface IDirectory
    {
        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        IFileSystem FileSystem { get; }

        /// <inheritdoc cref="Directory.CreateDirectory(string)"/>
        IDirectoryInfo CreateDirectory(string path);

#if NET40
        /// <inheritdoc cref="Directory.CreateDirectory(string,DirectorySecurity)"/>
        IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity);
#endif
        /// <inheritdoc cref="Directory.Delete(string)"/>
        void Delete(string path);
        /// <inheritdoc cref="Directory.Delete(string,bool)"/>
        void Delete(string path, bool recursive);
        /// <inheritdoc cref="Directory.Exists"/>
        bool Exists(string path);
        /// <inheritdoc cref="Directory.GetAccessControl(string)"/>
        DirectorySecurity GetAccessControl(string path);
        /// <inheritdoc cref="Directory.GetAccessControl(string,AccessControlSections)"/>
        DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections);
        /// <inheritdoc cref="Directory.GetCreationTime"/>
        DateTime GetCreationTime(string path);
        /// <inheritdoc cref="Directory.GetCreationTimeUtc"/>
        DateTime GetCreationTimeUtc(string path);
        /// <inheritdoc cref="Directory.GetCurrentDirectory"/>
        string GetCurrentDirectory();
        /// <inheritdoc cref="Directory.GetDirectories(string)"/>
        string[] GetDirectories(string path);
        /// <inheritdoc cref="Directory.GetDirectories(string,string)"/>
        string[] GetDirectories(string path, string searchPattern);
        /// <inheritdoc cref="Directory.GetDirectories(string,string,SearchOption)"/>
        string[] GetDirectories(string path, string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="Directory.GetDirectoryRoot"/>
        string GetDirectoryRoot(string path);
        /// <inheritdoc cref="Directory.GetFiles(string)"/>
        string[] GetFiles(string path);
        /// <inheritdoc cref="Directory.GetFiles(string,string)"/>
        string[] GetFiles(string path, string searchPattern);
        /// <inheritdoc cref="Directory.GetFiles(string,string,SearchOption)"/>
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="Directory.GetFileSystemEntries(string)"/>
        string[] GetFileSystemEntries(string path);
        /// <inheritdoc cref="Directory.GetFileSystemEntries(string,string)"/>
        string[] GetFileSystemEntries(string path, string searchPattern);
        /// <inheritdoc cref="Directory.GetLastAccessTime"/>
        DateTime GetLastAccessTime(string path);
        /// <inheritdoc cref="Directory.GetLastAccessTimeUtc"/>
        DateTime GetLastAccessTimeUtc(string path);
        /// <inheritdoc cref="Directory.GetLastWriteTime"/>
        DateTime GetLastWriteTime(string path);
        /// <inheritdoc cref="Directory.GetLastWriteTimeUtc"/>
        DateTime GetLastWriteTimeUtc(string path);
#if NET40
        /// <inheritdoc cref="Directory.GetLogicalDrives"/>
        string[] GetLogicalDrives();
#endif
        /// <inheritdoc cref="Directory.GetParent"/>
        IDirectoryInfo GetParent(string path);
        /// <inheritdoc cref="Directory.Move"/>
        void Move(string sourceDirName, string destDirName);
        /// <inheritdoc cref="Directory.SetAccessControl"/>
        void SetAccessControl(string path, DirectorySecurity directorySecurity);
        /// <inheritdoc cref="Directory.SetCreationTime"/>
        void SetCreationTime(string path, DateTime creationTime);
        /// <inheritdoc cref="Directory.SetCreationTimeUtc"/>
        void SetCreationTimeUtc(string path, DateTime creationTimeUtc);
        /// <inheritdoc cref="Directory.SetCurrentDirectory"/>
        void SetCurrentDirectory(string path);
        /// <inheritdoc cref="Directory.SetLastAccessTime"/>
        void SetLastAccessTime(string path, DateTime lastAccessTime);
        /// <inheritdoc cref="Directory.SetLastAccessTimeUtc"/>
        void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);
        /// <inheritdoc cref="Directory.SetLastWriteTime"/>
        void SetLastWriteTime(string path, DateTime lastWriteTime);
        /// <inheritdoc cref="Directory.SetLastWriteTimeUtc"/>
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);
        /// <inheritdoc cref="Directory.EnumerateDirectories(string)"/>
        IEnumerable<string> EnumerateDirectories(string path);
        /// <inheritdoc cref="Directory.EnumerateDirectories(string,string)"/>
        IEnumerable<string> EnumerateDirectories(string path, string searchPattern);
        /// <inheritdoc cref="Directory.EnumerateDirectories(string,string,SearchOption)"/>
        IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="Directory.EnumerateFiles(string)"/>
        IEnumerable<string> EnumerateFiles(string path);
        /// <inheritdoc cref="Directory.EnumerateFiles(string,string)"/>
        IEnumerable<string> EnumerateFiles(string path, string searchPattern);
        /// <inheritdoc cref="Directory.EnumerateFiles(string,string,SearchOption)"/>
        IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string)"/>
        IEnumerable<string> EnumerateFileSystemEntries(string path);
        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string,string)"/>
        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);
        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string,string,SearchOption)"/>
        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);
    }
}