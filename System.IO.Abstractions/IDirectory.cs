using System.Collections.Generic;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    public interface IDirectory
    {
        IFileSystem FileSystem { get; }

        DirectoryInfoBase CreateDirectory(string path);
        void Delete(string path);
        void Delete(string path, bool recursive);
        IEnumerable<string> EnumerateDirectories(string path);
        IEnumerable<string> EnumerateDirectories(string path, string searchPattern);
        IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);
        IEnumerable<string> EnumerateFiles(string path);
        IEnumerable<string> EnumerateFiles(string path, string searchPattern);
        IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);
        IEnumerable<string> EnumerateFileSystemEntries(string path);
        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);
        IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);
        bool Exists(string path);
        DirectorySecurity GetAccessControl(string path);
        DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections);
        DateTime GetCreationTime(string path);
        DateTime GetCreationTimeUtc(string path);
        string GetCurrentDirectory();
        string[] GetDirectories(string path);
        string[] GetDirectories(string path, string searchPattern);
        string[] GetDirectories(string path, string searchPattern, SearchOption searchOption);
        string GetDirectoryRoot(string path);
        string[] GetFiles(string path);
        string[] GetFiles(string path, string searchPattern);
        string[] GetFiles(string path, string searchPattern, SearchOption searchOption);
        string[] GetFileSystemEntries(string path);
        string[] GetFileSystemEntries(string path, string searchPattern);
        DateTime GetLastAccessTime(string path);
        DateTime GetLastAccessTimeUtc(string path);
        DateTime GetLastWriteTime(string path);
        DateTime GetLastWriteTimeUtc(string path);
        DirectoryInfoBase GetParent(string path);
        void Move(string sourceDirName, string destDirName);
        void SetAccessControl(string path, DirectorySecurity directorySecurity);
        void SetCreationTime(string path, DateTime creationTime);
        void SetCreationTimeUtc(string path, DateTime creationTimeUtc);
        void SetCurrentDirectory(string path);
        void SetLastAccessTime(string path, DateTime lastAccessTime);
        void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);
        void SetLastWriteTime(string path, DateTime lastWriteTime);
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

#if NET40
        DirectoryInfoBase CreateDirectory(string path, DirectorySecurity directorySecurity);
        string[] GetLogicalDrives();
#endif

    }
}