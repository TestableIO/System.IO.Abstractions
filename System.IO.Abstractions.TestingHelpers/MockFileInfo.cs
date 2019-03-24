using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileInfo : FileInfoBase
    {
        private readonly IMockFileDataAccessor mockFileSystem;
        private string path;

        public MockFileInfo(IMockFileDataAccessor mockFileSystem, string path) : base(mockFileSystem?.FileSystem)
        {
            this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
            this.path = path ?? throw new ArgumentNullException(nameof(path));
        }

        MockFileData MockFileData
        {
            get { return mockFileSystem.GetFile(path); }
        }

        public override void Delete()
        {
            mockFileSystem.RemoveFile(path);
        }

        public override void Refresh()
        {
        }

        public override FileAttributes Attributes
        {
            get
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                return MockFileData.Attributes;
            }
            set { MockFileData.Attributes = value; }
        }

        public override DateTime CreationTime
        {
            get
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                return MockFileData.CreationTime.DateTime;
            }
            set
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                MockFileData.CreationTime = value;
            }
        }

        public override DateTime CreationTimeUtc
        {
            get
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                return MockFileData.CreationTime.UtcDateTime;
            }
            set
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                MockFileData.CreationTime = value.ToLocalTime();
            }
        }

        public override bool Exists
        {
            get { return MockFileData != null && !MockFileData.IsDirectory; }
        }

        public override string Extension
        {
            get
            {
                // System.IO.Path.GetExtension does only string manipulation,
                // so it's safe to delegate.
                return Path.GetExtension(path);
            }
        }

        public override string FullName
        {
            get { return path; }
        }

        public override DateTime LastAccessTime
        {
            get
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                return MockFileData.LastAccessTime.DateTime;
            }
            set
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                MockFileData.LastAccessTime = value;
            }
        }

        public override DateTime LastAccessTimeUtc
        {
            get
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                return MockFileData.LastAccessTime.UtcDateTime;
            }
            set
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                MockFileData.LastAccessTime = value;
            }
        }

        public override DateTime LastWriteTime
        {
            get
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                return MockFileData.LastWriteTime.DateTime;
            }
            set
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                MockFileData.LastWriteTime = value;
            }
        }

        public override DateTime LastWriteTimeUtc
        {
            get
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                return MockFileData.LastWriteTime.UtcDateTime;
            }
            set
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                MockFileData.LastWriteTime = value.ToLocalTime();
            }
        }

        public override string Name
        {
            get { return new MockPath(mockFileSystem).GetFileName(path); }
        }

        public override StreamWriter AppendText()
        {
            if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
            return new StreamWriter(new MockFileStream(mockFileSystem, FullName, MockFileStream.StreamType.APPEND));
        }

        public override IFileInfo CopyTo(string destFileName)
        {
            return CopyTo(destFileName, false);
        }

        public override IFileInfo CopyTo(string destFileName, bool overwrite)
        {
            if (!Exists)
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(FullName);
            }
            if (destFileName == FullName)
            {
                return this;
            }
            mockFileSystem.File.Copy(FullName, destFileName, overwrite);
            return mockFileSystem.FileInfo.FromFileName(destFileName);
        }

        public override Stream Create()
        {
            return new MockFile(mockFileSystem).Create(FullName);
        }

        public override StreamWriter CreateText()
        {
            return new MockFile(mockFileSystem).CreateText(FullName);
        }

#if NET40
        public override void Decrypt()
        {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);

            MockFileData.Attributes &= ~FileAttributes.Encrypted;
        }

        public override void Encrypt()
        {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);

            MockFileData.Attributes |= FileAttributes.Encrypted;
        }
#endif

        public override FileSecurity GetAccessControl()
        {
            return mockFileSystem.File.GetAccessControl(this.path);
        }

        public override FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            return mockFileSystem.File.GetAccessControl(this.path, includeSections);
        }

        public override void MoveTo(string destFileName)
        {
            var movedFileInfo = CopyTo(destFileName);
            if (destFileName == FullName)
            {
                return;
            }
            Delete();
            path = movedFileInfo.FullName;
        }

        public override Stream Open(FileMode mode)
        {
            return new MockFile(mockFileSystem).Open(FullName, mode);
        }

        public override Stream Open(FileMode mode, FileAccess access)
        {
            return new MockFile(mockFileSystem).Open(FullName, mode, access);
        }

        public override Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return new MockFile(mockFileSystem).Open(FullName, mode, access, share);
        }

        public override Stream OpenRead()
        {
            if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
            return new MockFileStream(mockFileSystem, path, MockFileStream.StreamType.READ);
        }

        public override StreamReader OpenText()
        {
            return new StreamReader(OpenRead());
        }

        public override Stream OpenWrite()
        {
            return new MockFileStream(mockFileSystem, path, MockFileStream.StreamType.WRITE);
        }

#if NET40
        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName)
        {
            return Replace(destinationFileName, destinationBackupFileName, false);
        }

        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            mockFileSystem.File.Replace(path, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
            return mockFileSystem.FileInfo.FromFileName(destinationFileName);
        }
#endif

        public override void SetAccessControl(FileSecurity fileSecurity)
        {
            mockFileSystem.File.SetAccessControl(this.path, fileSecurity);
        }

        public override IDirectoryInfo Directory
        {
            get
            {
                return mockFileSystem.DirectoryInfo.FromDirectoryName(DirectoryName);
            }
        }

        public override string DirectoryName
        {
            get
            {
                // System.IO.Path.GetDirectoryName does only string manipulation,
                // so it's safe to delegate.
                return Path.GetDirectoryName(path);
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                return (MockFileData.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            }
            set
            {
                if (MockFileData == null) throw CommonExceptions.FileNotFound(path);
                if (value)
                    MockFileData.Attributes |= FileAttributes.ReadOnly;
                else
                    MockFileData.Attributes &= ~FileAttributes.ReadOnly;
            }
        }

        public override long Length
        {
            get
            {
                if (MockFileData == null || MockFileData.IsDirectory) throw CommonExceptions.FileNotFound(path);
                return MockFileData.Contents.Length;
            }
        }

        public override string ToString()
        {
            return path;
        }
    }
}
