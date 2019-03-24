using System.Collections.Generic;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    [Serializable]
    public class DirectoryWrapper : DirectoryBase
    {
        public DirectoryWrapper(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        public override IDirectoryInfo CreateDirectory(string path)
        {
            return new DirectoryInfoWrapper(FileSystem, Directory.CreateDirectory(path));
        }

#if NET40
        public override IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            return new DirectoryInfoWrapper(FileSystem, Directory.CreateDirectory(path, directorySecurity));
        }
#endif
        public override void Delete(string path)
        {
            Directory.Delete(path);
        }

        public override void Delete(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }

        public override bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public override DirectorySecurity GetAccessControl(string path)
        {
            return new DirectoryInfo(path).GetAccessControl();
        }

        public override DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return new DirectoryInfo(path).GetAccessControl(includeSections);
        }

        public override DateTime GetCreationTime(string path)
        {
            return Directory.GetCreationTime(path);
        }

        public override DateTime GetCreationTimeUtc(string path)
        {
            return Directory.GetCreationTimeUtc(path);
        }

        public override string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        public override string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public override string[] GetDirectories(string path, string searchPattern)
        {
            return Directory.GetDirectories(path, searchPattern);
        }

        public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetDirectories(path, searchPattern, searchOption);
        }

        public override string GetDirectoryRoot(string path)
        {
            return Directory.GetDirectoryRoot(path);
        }

        public override string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public override string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }

        public override string[] GetFileSystemEntries(string path)
        {
            return Directory.GetFileSystemEntries(path);
        }

        public override string[] GetFileSystemEntries(string path, string searchPattern)
        {
            return Directory.GetFileSystemEntries(path, searchPattern);
        }

        public override DateTime GetLastAccessTime(string path)
        {
            return Directory.GetLastAccessTime(path);
        }

        public override DateTime GetLastAccessTimeUtc(string path)
        {
            return Directory.GetLastAccessTimeUtc(path);
        }

        public override DateTime GetLastWriteTime(string path)
        {
            return Directory.GetLastWriteTime(path);
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return Directory.GetLastWriteTimeUtc(path);
        }

#if NET40
        public override string[] GetLogicalDrives()
        {
            return Directory.GetLogicalDrives();
        }
#endif

        public override IDirectoryInfo GetParent(string path)
        {
            return new DirectoryInfoWrapper(FileSystem, Directory.GetParent(path));
        }

        public override void Move(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        public override void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            new DirectoryInfo(path).SetAccessControl(directorySecurity);
        }

        public override void SetCreationTime(string path, DateTime creationTime)
        {
            Directory.SetCreationTime(path, creationTime);
        }

        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            Directory.SetCreationTimeUtc(path, creationTimeUtc);
        }

        public override void SetCurrentDirectory(string path)
        {
            Directory.SetCurrentDirectory(path);
        }

        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            Directory.SetLastAccessTime(path, lastAccessTime);
        }

        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            Directory.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
        }

        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            Directory.SetLastAccessTime(path, lastWriteTime);
        }

        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            Directory.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }

        public override IEnumerable<string> EnumerateDirectories(string path)
        {
            return Directory.EnumerateDirectories(path);
        }

        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            return Directory.EnumerateDirectories(path, searchPattern);
        }

        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(path, searchPattern, searchOption);
        }

        public override IEnumerable<string> EnumerateFiles(string path)
        {
           return Directory.EnumerateFiles(path);
        }
 
        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern);
        }

        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(path, searchPattern, searchOption);
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return Directory.EnumerateFileSystemEntries(path);
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            return Directory.EnumerateFileSystemEntries(path, searchPattern);
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);
        }
    }
}
