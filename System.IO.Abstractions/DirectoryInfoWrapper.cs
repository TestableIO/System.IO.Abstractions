using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    [Serializable]
    public class DirectoryInfoWrapper : DirectoryInfoBase
    {
        readonly DirectoryInfo instance;

        public DirectoryInfoWrapper(DirectoryInfo instance)
        {
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
            return instance.GetDirectories().Wrap();
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern)
        {
            return instance.GetDirectories(searchPattern).Wrap();
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return instance.GetDirectories(searchPattern, searchOption).Wrap();
        }

        public override FileInfoBase[] GetFiles()
        {
            return instance.GetFiles().Wrap();
        }

        public override FileInfoBase[] GetFiles(string searchPattern)
        {
            return instance.GetFiles(searchPattern).Wrap();
        }

        public override FileInfoBase[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return instance.GetFiles(searchPattern, searchOption).Wrap();
        }

        public override FileSystemInfoBase[] GetFileSystemInfos()
        {
            return instance.GetFileSystemInfos().Wrap();
        }

        public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern)
        {
            return instance.GetFileSystemInfos(searchPattern).Wrap();
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
    }
}