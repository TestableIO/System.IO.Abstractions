using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    [Serializable]
    public class DirectoryInfoWrapper : DirectoryInfoBase
    {
        private readonly DirectoryInfo instance;

        public DirectoryInfoWrapper(DirectoryInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            this.instance = instance;
        }

        public override IFileSystem FileSystem
        {
            get { return Abstractions.FileSystem.Instance; }
        }

        public override void Delete()
        {
            instance.Delete();
        }

        public override void Refresh()
        {
            instance.Refresh();
        }

        public override FileAttributes Attributes
        {
            get { return instance.Attributes; }
            set { instance.Attributes = value; }
        }

        public override DateTime CreationTime
        {
            get { return instance.CreationTime; }
            set { instance.CreationTime = value; }
        }

        public override DateTime CreationTimeUtc
        {
            get { return instance.CreationTimeUtc; }
            set { instance.CreationTimeUtc = value; }
        }

        public override bool Exists
        {
            get { return instance.Exists; }
        }

        public override string Extension
        {
            get { return instance.Extension; }
        }

        public override string FullName
        {
            get { return instance.FullName; }
        }

        public override DateTime LastAccessTime
        {
            get { return instance.LastAccessTime; }
            set { instance.LastAccessTime = value; }
        }

        public override DateTime LastAccessTimeUtc
        {
            get { return instance.LastAccessTimeUtc; }
            set { instance.LastAccessTimeUtc = value; }
        }

        public override DateTime LastWriteTime
        {
            get { return instance.LastWriteTime; }
            set { instance.LastWriteTime = value; }
        }

        public override DateTime LastWriteTimeUtc
        {
            get { return instance.LastWriteTimeUtc; }
            set { instance.LastWriteTimeUtc = value; }
        }

        public override string Name
        {
            get { return instance.Name; }
        }

        public override void Create()
        {
            instance.Create();
        }

        public override void Create(DirectorySecurity directorySecurity)
        {
            instance.Create(directorySecurity);
        }

        public override IDirectory CreateSubdirectory(string path)
        {
            return new DirectoryInfoWrapper(instance.CreateSubdirectory(path));
        }

        public override IDirectory CreateSubdirectory(string path, DirectorySecurity directorySecurity)
        {
            return new DirectoryInfoWrapper(instance.CreateSubdirectory(path, directorySecurity));
        }

        public override void Delete(bool recursive)
        {
            instance.Delete(recursive);
        }

        public override IEnumerable<IDirectory> EnumerateDirectories()
        {
            return instance.EnumerateDirectories().Select(directoryInfo => new DirectoryInfoWrapper(directoryInfo));
        }

        public override IEnumerable<IDirectory> EnumerateDirectories(string searchPattern)
        {
            return instance.EnumerateDirectories(searchPattern).Select(directoryInfo => new DirectoryInfoWrapper(directoryInfo));
        }

        public override IEnumerable<IDirectory> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return instance.EnumerateDirectories(searchPattern, searchOption).Select(directoryInfo => new DirectoryInfoWrapper(directoryInfo));
        }

        public override IEnumerable<IFile> EnumerateFiles()
        {
            return instance.EnumerateFiles().Select(fileInfo => new FileInfoWrapper(fileInfo));
        }

        public override IEnumerable<IFile> EnumerateFiles(string searchPattern)
        {
            return instance.EnumerateFiles(searchPattern).Select(fileInfo => new FileInfoWrapper(fileInfo));
        }

        public override IEnumerable<IFile> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return instance.EnumerateFiles(searchPattern, searchOption).Select(fileInfo => new FileInfoWrapper(fileInfo));
        }

        public override IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos()
        {
            return instance.EnumerateFileSystemInfos().WrapFileSystemInfos();
        }

        public override IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos(string searchPattern)
        {
            return instance.EnumerateFileSystemInfos(searchPattern).WrapFileSystemInfos();
        }

        public override IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return instance.EnumerateFileSystemInfos(searchPattern, searchOption).WrapFileSystemInfos();
        }

        public override DirectorySecurity GetAccessControl()
        {
            return instance.GetAccessControl();
        }

        public override DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return instance.GetAccessControl(includeSections);
        }

        public override IDirectory[] GetDirectories()
        {
            return instance.GetDirectories().WrapDirectories();
        }

        public override IDirectory[] GetDirectories(string searchPattern)
        {
            return instance.GetDirectories(searchPattern).WrapDirectories();
        }

        public override IDirectory[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return instance.GetDirectories(searchPattern, searchOption).WrapDirectories();
        }

        public override IFile[] GetFiles()
        {
            return instance.GetFiles().WrapFiles();
        }

        public override IFile[] GetFiles(string searchPattern)
        {
            return instance.GetFiles(searchPattern).WrapFiles();
        }

        public override IFile[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return instance.GetFiles(searchPattern, searchOption).WrapFiles();
        }

        public override IFileSystemEntry[] GetFileSystemInfos()
        {
            return instance.GetFileSystemInfos().WrapFileSystemInfos();
        }

        public override IFileSystemEntry[] GetFileSystemInfos(string searchPattern)
        {
            return instance.GetFileSystemInfos(searchPattern).WrapFileSystemInfos();
        }

        public override IFileSystemEntry[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return instance.GetFileSystemInfos(searchPattern, searchOption).WrapFileSystemInfos();
        }

        public override void MoveTo(string destDirName)
        {
            instance.MoveTo(destDirName);
        }

        public override void SetAccessControl(DirectorySecurity directorySecurity)
        {
            instance.SetAccessControl(directorySecurity);
        }

        public override IDirectory Parent
        {
            get { return new DirectoryInfoWrapper(instance.Parent); }
        }

        public override IDirectory Root
        {
            get { return new DirectoryInfoWrapper(instance.Root); }
        }

        public override string ToString()
        {
            return instance.ToString();
        }
    }
}