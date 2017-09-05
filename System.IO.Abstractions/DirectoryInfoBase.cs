using System.Collections.Generic;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    [Serializable]
    public abstract class DirectoryInfoBase : FileSystemInfoBase, IDirectory
    {
        public abstract void Create();
        public abstract void Create(DirectorySecurity directorySecurity);
        public abstract IDirectory CreateSubdirectory(string path);
        public abstract IDirectory CreateSubdirectory(string path, DirectorySecurity directorySecurity);
        public abstract void Delete(bool recursive);
        public abstract IEnumerable<IDirectory> EnumerateDirectories();
        public abstract IEnumerable<IDirectory> EnumerateDirectories(string searchPattern);
        public abstract IEnumerable<IDirectory> EnumerateDirectories(string searchPattern, SearchOption searchOption);
        public abstract IEnumerable<IFile> EnumerateFiles();
        public abstract IEnumerable<IFile> EnumerateFiles(string searchPattern);
        public abstract IEnumerable<IFile> EnumerateFiles(string searchPattern, SearchOption searchOption);
        public abstract IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos();
        public abstract IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos(string searchPattern);
        public abstract IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption);
        public abstract DirectorySecurity GetAccessControl();
        public abstract DirectorySecurity GetAccessControl(AccessControlSections includeSections);
        public abstract IDirectory[] GetDirectories();
        public abstract IDirectory[] GetDirectories(string searchPattern);
        public abstract IDirectory[] GetDirectories(string searchPattern, SearchOption searchOption);
        public abstract IFile[] GetFiles();
        public abstract IFile[] GetFiles(string searchPattern);
        public abstract IFile[] GetFiles(string searchPattern, SearchOption searchOption);
        public abstract IFileSystemEntry[] GetFileSystemInfos();
        public abstract IFileSystemEntry[] GetFileSystemInfos(string searchPattern);
        public abstract IFileSystemEntry[] GetFileSystemInfos(string searchPattern, SearchOption searchOption);
        public abstract void MoveTo(string destDirName);
        public abstract void SetAccessControl(DirectorySecurity directorySecurity);
        public abstract IDirectory Parent { get; }
        public abstract IDirectory Root { get; }

        public static implicit operator DirectoryInfoBase(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                return null;
            return new DirectoryInfoWrapper(directoryInfo);
        }
    }
}