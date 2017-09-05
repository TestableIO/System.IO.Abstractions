using System.Collections.Generic;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    public interface IDirectory : IFileSystemEntry
    {
        IDirectory Parent { get; }
        IDirectory Root { get; }

        void Create();
        void Create(DirectorySecurity directorySecurity);
        IDirectory CreateSubdirectory(string path);
        IDirectory CreateSubdirectory(string path, DirectorySecurity directorySecurity);
        void Delete(bool recursive);
        IEnumerable<IDirectory> EnumerateDirectories();
        IEnumerable<IDirectory> EnumerateDirectories(string searchPattern);
        IEnumerable<IDirectory> EnumerateDirectories(string searchPattern, SearchOption searchOption);
        IEnumerable<IFile> EnumerateFiles();
        IEnumerable<IFile> EnumerateFiles(string searchPattern);
        IEnumerable<IFile> EnumerateFiles(string searchPattern, SearchOption searchOption);
        IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos();
        IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos(string searchPattern);
        IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption);
        DirectorySecurity GetAccessControl();
        DirectorySecurity GetAccessControl(AccessControlSections includeSections);
        IDirectory[] GetDirectories();
        IDirectory[] GetDirectories(string searchPattern);
        IDirectory[] GetDirectories(string searchPattern, SearchOption searchOption);
        IFile[] GetFiles();
        IFile[] GetFiles(string searchPattern);
        IFile[] GetFiles(string searchPattern, SearchOption searchOption);
        IFileSystemEntry[] GetFileSystemInfos();
        IFileSystemEntry[] GetFileSystemInfos(string searchPattern);
        IFileSystemEntry[] GetFileSystemInfos(string searchPattern, SearchOption searchOption);
        [Obsolete]
        void MoveTo(string destDirName);
        void SetAccessControl(DirectorySecurity directorySecurity);
    }
}