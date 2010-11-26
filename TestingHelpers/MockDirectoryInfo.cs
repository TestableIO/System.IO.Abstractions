using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockDirectoryInfo : DirectoryInfoBase
    {
        readonly IMockFileDataAccessor mockFileDataAccessor;
        readonly string directoryPath;

        public MockDirectoryInfo(IMockFileDataAccessor mockFileDataAccessor, string directoryPath)
        {
            this.mockFileDataAccessor = mockFileDataAccessor;
            this.directoryPath = directoryPath;
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        public override FileAttributes Attributes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DateTime CreationTime
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DateTime CreationTimeUtc
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override bool Exists
        {
            get { throw new NotImplementedException(); }
        }

        public override string Extension
        {
            get { throw new NotImplementedException(); }
        }

        public override string FullName
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime LastAccessTime
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DateTime LastAccessTimeUtc
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DateTime LastWriteTime
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DateTime LastWriteTimeUtc
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }

        public override void Create(DirectorySecurity directorySecurity)
        {
            throw new NotImplementedException();
        }

        public override DirectoryInfoBase CreateSubdirectory(string path)
        {
            throw new NotImplementedException();
        }

        public override DirectoryInfoBase CreateSubdirectory(string path, DirectorySecurity directorySecurity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(bool recursive)
        {
            throw new NotImplementedException();
        }

        public override DirectorySecurity GetAccessControl()
        {
            throw new NotImplementedException();
        }

        public override DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        public override DirectoryInfoBase[] GetDirectories()
        {
            throw new NotImplementedException();
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public override FileInfoBase[] GetFiles()
        {
            throw new NotImplementedException();
        }

        public override FileInfoBase[] GetFiles(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public override FileInfoBase[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public override FileSystemInfoBase[] GetFileSystemInfos()
        {
            throw new NotImplementedException();
        }

        public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern)
        {
            throw new NotImplementedException();
        }

        public override void MoveTo(string destDirName)
        {
            throw new NotImplementedException();
        }

        public override void SetAccessControl(DirectorySecurity directorySecurity)
        {
            throw new NotImplementedException();
        }

        public override DirectoryInfoBase Parent
        {
            get { throw new NotImplementedException(); }
        }

        public override DirectoryInfoBase Root
        {
            get { throw new NotImplementedException(); }
        }
    }
}
