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

        public override DirectoryInfoBase CreateSubdirectory(string path)
        {
            return new DirectoryInfoWrapper(instance.CreateSubdirectory(path));
        }

        public override DirectoryInfoBase CreateSubdirectory(string path, DirectorySecurity directorySecurity)
        {
            return new DirectoryInfoWrapper(instance.CreateSubdirectory(path, directorySecurity));
        }

        public override void Delete(bool recursive)
        {
            instance.Delete(recursive);
        }

        public override IEnumerable<DirectoryInfoBase> EnumerateDirectories()
        {
#if !NET40
            return GetDirectories();
#else
            return instance.EnumerateDirectories().Select(directoryInfo => new DirectoryInfoWrapper(directoryInfo));
#endif
        }

        public override IEnumerable<DirectoryInfoBase> EnumerateDirectories(string searchPattern)
        {
#if !NET40
            return GetDirectories(searchPattern);
#else
            return instance.EnumerateDirectories(searchPattern).Select(directoryInfo => new DirectoryInfoWrapper(directoryInfo));
#endif
        }

        public override IEnumerable<DirectoryInfoBase> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
#if !NET40
            return GetDirectories(searchPattern, searchOption);
#else
            return instance.EnumerateDirectories(searchPattern, searchOption).Select(directoryInfo => new DirectoryInfoWrapper(directoryInfo));
#endif
        }

        public override IEnumerable<FileInfoBase> EnumerateFiles()
        {
#if !NET40
            return GetFiles();
#else
            return instance.EnumerateFiles().Select(fileInfo => new FileInfoWrapper(fileInfo));
#endif
        }

        public override IEnumerable<FileInfoBase> EnumerateFiles(string searchPattern)
        {
#if !NET40
            return GetFiles(searchPattern);
#else
            return instance.EnumerateFiles(searchPattern).Select(fileInfo => new FileInfoWrapper(fileInfo));
#endif
        }

        public override IEnumerable<FileInfoBase> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
#if !NET40
            return GetFiles(searchPattern, searchOption);
#else
            return instance.EnumerateFiles(searchPattern, searchOption).Select(fileInfo => new FileInfoWrapper(fileInfo));
#endif
        }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos()
        {
#if !NET40
            return GetFileSystemInfos();
#else
            return instance.EnumerateFileSystemInfos().WrapFileSystemInfos();
#endif
        }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos(string searchPattern)
        {
#if !NET40
            return GetFileSystemInfos(searchPattern);
#else
            return instance.EnumerateFileSystemInfos(searchPattern).WrapFileSystemInfos();
#endif
        }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
#if !NET40
            return GetFileSystemInfos(searchPattern, searchOption);
#else
            return instance.EnumerateFileSystemInfos(searchPattern, searchOption).WrapFileSystemInfos();
#endif
        }

        public override DirectorySecurity GetAccessControl()
        {
            return instance.GetAccessControl();
        }

        public override DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return instance.GetAccessControl(includeSections);
        }

        public override DirectoryInfoBase[] GetDirectories()
        {
            return instance.GetDirectories().WrapDirectories();
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern)
        {
            return instance.GetDirectories(searchPattern).WrapDirectories();
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return instance.GetDirectories(searchPattern, searchOption).WrapDirectories();
        }

        public override FileInfoBase[] GetFiles()
        {
            return instance.GetFiles().WrapFiles();
        }

        public override FileInfoBase[] GetFiles(string searchPattern)
        {
            return instance.GetFiles(searchPattern).WrapFiles();
        }

        public override FileInfoBase[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return instance.GetFiles(searchPattern, searchOption).WrapFiles();
        }

        public override FileSystemInfoBase[] GetFileSystemInfos()
        {
            return instance.GetFileSystemInfos().WrapFileSystemInfos();
        }

        public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern)
        {
            return instance.GetFileSystemInfos(searchPattern).WrapFileSystemInfos();
        }

        public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
#if !NET40
            if (searchOption == SearchOption.TopDirectoryOnly)
            {
                return instance.GetFileSystemInfos(searchPattern).WrapFileSystemInfos();
            }
            else
            {
                var fis = instance.GetFiles(searchPattern, searchOption).WrapFileSystemInfos();
                var dis = instance.GetDirectories(searchPattern, searchOption).WrapFileSystemInfos();
                return fis.Union(dis).ToArray();
            }
#else
            return instance.GetFileSystemInfos(searchPattern, searchOption).WrapFileSystemInfos();
#endif
        }

        public override void MoveTo(string destDirName)
        {
            instance.MoveTo(destDirName);
        }

        public override void SetAccessControl(DirectorySecurity directorySecurity)
        {
            instance.SetAccessControl(directorySecurity);
        }

        public override DirectoryInfoBase Parent
        {
            get { return instance.Parent; }
        }

        public override DirectoryInfoBase Root
        {
            get { return instance.Root; }
        }

        public override string ToString()
        {
            return instance.ToString();
        }
    }
}