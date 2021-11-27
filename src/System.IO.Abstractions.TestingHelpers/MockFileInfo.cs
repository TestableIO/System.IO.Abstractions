using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
    [Serializable]
    public class MockFileInfo : FileInfoBase
    {
        private readonly IMockFileDataAccessor mockFileSystem;
        private string path;
        private string originalPath;

        /// <inheritdoc />
        public MockFileInfo(IMockFileDataAccessor mockFileSystem, string path) : base(mockFileSystem?.FileSystem)
        {
            this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
            this.originalPath = path ?? throw new ArgumentNullException(nameof(path));
            this.path = mockFileSystem.Path.GetFullPath(path);

        }

        MockFileData MockFileData
        {
            get { return mockFileSystem.GetFile(path); }
        }

        /// <inheritdoc />
        public override void Delete()
        {
            mockFileSystem.RemoveFile(path);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            // Nothing to do here. Mock file system is always up-to-date.
        }

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get
            {
                if (MockFileData == null)
                {
                    throw CommonExceptions.FileNotFound(path);
                }
                return MockFileData.Attributes;
            }
            set
            {
                if (MockFileData == null)
                {
                    throw CommonExceptions.FileNotFound(path);
                }
                MockFileData.Attributes = value;
            }
        }

        /// <inheritdoc />
        public override DateTime CreationTime
        {
            get
            {
                if (MockFileData == null)
                {
                    throw CommonExceptions.FileNotFound(path);
                }
                return MockFileData.CreationTime.DateTime;
            }
            set
            {
                if (MockFileData == null)
                {
                    throw CommonExceptions.FileNotFound(path);
                }
                MockFileData.CreationTime = value;
            }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override bool Exists
        {
            get { return MockFileData != null && !MockFileData.IsDirectory; }
        }

        /// <inheritdoc />
        public override string Extension
        {
            get
            {
                // System.IO.Path.GetExtension does only string manipulation,
                // so it's safe to delegate.
                return Path.GetExtension(path);
            }
        }

        /// <inheritdoc />
        public override string FullName
        {
            get { return path; }
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override string Name
        {
            get { return new MockPath(mockFileSystem).GetFileName(path); }
        }

        /// <inheritdoc />
        public override StreamWriter AppendText()
        {
            return new StreamWriter(new MockFileStream(mockFileSystem, FullName, FileMode.Append));
        }

        /// <inheritdoc />
        public override IFileInfo CopyTo(string destFileName)
        {
            return CopyTo(destFileName, false);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override Stream Create()
        {
            return new MockFile(mockFileSystem).Create(FullName);
        }

        /// <inheritdoc />
        public override StreamWriter CreateText()
        {
            return new MockFile(mockFileSystem).CreateText(FullName);
        }

        /// <inheritdoc />
        public override void Decrypt()
        {
            if (MockFileData == null) throw CommonExceptions.FileNotFound(path);

            MockFileData.Attributes &= ~FileAttributes.Encrypted;
        }

        /// <inheritdoc />
        public override void Encrypt()
        {
            if (MockFileData == null) throw CommonExceptions.FileNotFound(path);

            MockFileData.Attributes |= FileAttributes.Encrypted;
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override FileSecurity GetAccessControl()
        {
            return mockFileSystem.File.GetAccessControl(this.path);
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            return mockFileSystem.File.GetAccessControl(this.path, includeSections);
        }

        /// <inheritdoc />
        public override void MoveTo(string destFileName)
        {
            mockFileSystem.File.Move(path, destFileName);
            path = mockFileSystem.Path.GetFullPath(destFileName);
        }

#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc />
        public override void MoveTo(string destFileName, bool overwrite)
        {
            mockFileSystem.File.Move(path, destFileName, overwrite);
            path = mockFileSystem.Path.GetFullPath(destFileName);
        }
#endif

        /// <inheritdoc />
        public override Stream Open(FileMode mode)
        {
            return new MockFile(mockFileSystem).Open(FullName, mode);
        }

        /// <inheritdoc />
        public override Stream Open(FileMode mode, FileAccess access)
        {
            return new MockFile(mockFileSystem).Open(FullName, mode, access);
        }

        /// <inheritdoc />
        public override Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return new MockFile(mockFileSystem).Open(FullName, mode, access, share);
        }

        /// <inheritdoc />
        public override Stream OpenRead() => mockFileSystem.File.OpenRead(path);

        /// <inheritdoc />
        public override StreamReader OpenText() => mockFileSystem.File.OpenText(path);

        /// <inheritdoc />
        public override Stream OpenWrite() => mockFileSystem.File.OpenWrite(path);

        /// <inheritdoc />
        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName)
        {
            return Replace(destinationFileName, destinationBackupFileName, false);
        }

        /// <inheritdoc />
        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            mockFileSystem.File.Replace(path, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
            return mockFileSystem.FileInfo.FromFileName(destinationFileName);
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override void SetAccessControl(FileSecurity fileSecurity)
        {
            mockFileSystem.File.SetAccessControl(this.path, fileSecurity);
        }

        /// <inheritdoc />
        public override IDirectoryInfo Directory
        {
            get
            {
                return mockFileSystem.DirectoryInfo.FromDirectoryName(DirectoryName);
            }
        }

        /// <inheritdoc />
        public override string DirectoryName
        {
            get
            {
                // System.IO.Path.GetDirectoryName does only string manipulation,
                // so it's safe to delegate.
                return Path.GetDirectoryName(path);
            }
        }

        /// <inheritdoc />
        public override bool IsReadOnly
        {
            get
            {
                if (MockFileData == null)
                {
                    throw CommonExceptions.FileNotFound(path);
                }
                return (MockFileData.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            }
            set
            {
                if (MockFileData == null)
                {
                    throw CommonExceptions.FileNotFound(path);
                }
                if (value)
                {
                    MockFileData.Attributes |= FileAttributes.ReadOnly;
                }
                else
                {
                    MockFileData.Attributes &= ~FileAttributes.ReadOnly;
                }
            }
        }

        /// <inheritdoc />
        public override long Length
        {
            get
            {
                if (MockFileData == null || MockFileData.IsDirectory)
                {
                    throw CommonExceptions.FileNotFound(path);
                }
                return MockFileData.Contents.Length;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return originalPath;
        }
    }
}
