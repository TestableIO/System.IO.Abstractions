using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    internal class MockFileInfo : FileInfoBase
    {
        readonly IMockFileDataAccessor mockFileSystem;
        readonly string path;

        public MockFileInfo(IMockFileDataAccessor mockFileSystem, string path)
        {
            this.mockFileSystem = mockFileSystem;
            this.path = path;
        }

        MockFileData MockFileData
        {
            get { return mockFileSystem.GetFile(path); }
        }

        public override void Delete()
        {
            throw new NotImplementedException();
        }

        public override void Refresh()
        {
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
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.CreationTime.UtcDateTime;
            }
            set { throw new NotImplementedException(); }
        }

        public override bool Exists
        {
            get { return MockFileData != null; }
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
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.LastAccessTime.UtcDateTime;
            }
            set { throw new NotImplementedException(); }
        }

        public override DateTime LastWriteTime
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override DateTime LastWriteTimeUtc
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.LastWriteTime.UtcDateTime;    
            }
            set { throw new NotImplementedException(); }
        }

        public override string Name
        {
            get { throw new NotImplementedException(); }
        }

        public override StreamWriter AppendText()
        {
            throw new NotImplementedException();
        }

        public override FileInfoBase CopyTo(string destFileName)
        {
            throw new NotImplementedException();
        }

        public override FileInfoBase CopyTo(string destFileName, bool overwrite)
        {
            throw new NotImplementedException();
        }

        public override Stream Create()
        {
            throw new NotImplementedException();
        }

        public override StreamWriter CreateText()
        {
            throw new NotImplementedException();
        }

        public override void Decrypt()
        {
            throw new NotImplementedException();
        }

        public override void Encrypt()
        {
            throw new NotImplementedException();
        }

        public override FileSecurity GetAccessControl()
        {
            throw new NotImplementedException();
        }

        public override FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        public override void MoveTo(string destFileName)
        {
            throw new NotImplementedException();
        }

        public override Stream Open(FileMode mode)
        {
            throw new NotImplementedException();
        }

        public override Stream Open(FileMode mode, FileAccess access)
        {
            throw new NotImplementedException();
        }

        public override Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            throw new NotImplementedException();
        }

        public override Stream OpenRead()
        {
            throw new NotImplementedException();
        }

        public override StreamReader OpenText()
        {
            throw new NotImplementedException();
        }

        public override Stream OpenWrite()
        {
            throw new NotImplementedException();
        }

        public override FileInfoBase Replace(string destinationFileName, string destinationBackupFileName)
        {
            throw new NotImplementedException();
        }

        public override FileInfoBase Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException();
        }

        public override void SetAccessControl(FileSecurity fileSecurity)
        {
            throw new NotImplementedException();
        }

        public override DirectoryInfoBase Directory
        {
            get { throw new NotImplementedException(); }
        }

        public override string DirectoryName
        {
            get { throw new NotImplementedException(); }
        }

        public override bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public override long Length
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.Contents.LongLength;
            }
        }
    }
}