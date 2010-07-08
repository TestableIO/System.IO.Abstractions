using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    public abstract class DirectoryInfoBase : FileSystemInfoBase
    {
        public abstract void Create();
        public abstract void Create(DirectorySecurity directorySecurity);
        public abstract DirectoryInfoBase CreateSubdirectory(string path);
        public abstract DirectoryInfoBase CreateSubdirectory(string path, DirectorySecurity directorySecurity);
        public abstract void Delete(bool recursive);
        public abstract DirectorySecurity GetAccessControl();
        public abstract DirectorySecurity GetAccessControl(AccessControlSections includeSections);
        public abstract DirectoryInfoBase[] GetDirectories();
        public abstract DirectoryInfoBase[] GetDirectories(string searchPattern);
        public abstract DirectoryInfoBase[] GetDirectories(string searchPattern, SearchOption searchOption);
        public abstract FileInfoBase[] GetFiles();
        public abstract FileInfoBase[] GetFiles(string searchPattern);
        public abstract FileInfoBase[] GetFiles(string searchPattern, SearchOption searchOption);
        public abstract FileSystemInfoBase[] GetFileSystemInfos();
        public abstract FileSystemInfoBase[] GetFileSystemInfos(string searchPattern);
        public abstract void MoveTo(string destDirName);
        public abstract void SetAccessControl(DirectorySecurity directorySecurity);
        public abstract DirectoryInfoBase Parent { get; }
        public abstract DirectoryInfoBase Root { get; }
        
        public static implicit operator DirectoryInfoBase(DirectoryInfo directoryInfo)
        {
            return new DirectoryInfoWrapper(directoryInfo);
        }
    }
}