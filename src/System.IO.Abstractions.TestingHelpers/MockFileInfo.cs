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
        private readonly string originalPath;
        private MockFileData cachedMockFileData;
        private MockFile mockFile;
        private bool refreshOnNextRead;

        /// <inheritdoc />
        public MockFileInfo(IMockFileDataAccessor mockFileSystem, string path) : base(mockFileSystem?.FileSystem)
        {
            this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
            this.originalPath = path ?? throw new ArgumentNullException(nameof(path));
            this.path = mockFileSystem.Path.GetFullPath(path);
            this.mockFile = new MockFile(mockFileSystem);
            Refresh();
        }

        /// <inheritdoc />
        public override void Delete()
        {
            mockFile.Delete(path);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            cachedMockFileData = mockFileSystem.GetFile(path)?.Clone();
        }

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.Attributes;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.Attributes = value;
            }
        }

        /// <inheritdoc />
        public override DateTime CreationTime
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.CreationTime.DateTime;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.CreationTime = value;
            }
        }

        /// <inheritdoc />
        public override DateTime CreationTimeUtc
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.CreationTime.UtcDateTime;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.CreationTime = value.ToLocalTime();
            }
        }

        /// <inheritdoc />
        public override bool Exists
        {
            get
            {
                var mockFileData = GetMockFileDataForRead(throwIfNotExisting: false);
                return mockFileData != null && !mockFileData.IsDirectory;
            }
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
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.LastAccessTime.DateTime;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.LastAccessTime = value;
            }
        }

        /// <inheritdoc />
        public override DateTime LastAccessTimeUtc
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.LastAccessTime.UtcDateTime;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.LastAccessTime = value;
            }
        }

        /// <inheritdoc />
        public override DateTime LastWriteTime
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.LastWriteTime.DateTime;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.LastWriteTime = value;
            }
        }

        /// <inheritdoc />
        public override DateTime LastWriteTimeUtc
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.LastWriteTime.UtcDateTime;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.LastWriteTime = value.ToLocalTime();
            }
        }

#if FEATURE_FILE_SYSTEM_INFO_LINK_TARGET
        /// <inheritdoc />
        public override string LinkTarget
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.LinkTarget;
            }
        }
#endif

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
                var mockFileData = GetMockFileDataForRead(throwIfNotExisting: false);
                if (mockFileData == null)
                {
                    throw CommonExceptions.FileNotFound(FullName);
                }
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
            var mockFileData = GetMockFileDataForWrite();
            mockFileData.Attributes &= ~FileAttributes.Encrypted;
        }

        /// <inheritdoc />
        public override void Encrypt()
        {
            var mockFileData = GetMockFileDataForWrite();
            mockFileData.Attributes |= FileAttributes.Encrypted;
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
                var mockFileData = GetMockFileDataForRead();
                return (mockFileData.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                if (value)
                {
                    mockFileData.Attributes |= FileAttributes.ReadOnly;
                }
                else
                {
                    mockFileData.Attributes &= ~FileAttributes.ReadOnly;
                }
            }
        }

        /// <inheritdoc />
        public override long Length
        {
            get
            {
                var mockFileData = GetMockFileDataForRead(throwIfNotExisting: false);
                if (mockFileData == null || mockFileData.IsDirectory)
                {
                    throw CommonExceptions.FileNotFound(path);
                }
                return mockFileData.Contents.Length;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return originalPath;
        }

        private MockFileData GetMockFileDataForRead(bool throwIfNotExisting = true)
        {
            if (refreshOnNextRead)
            {
                Refresh();
                refreshOnNextRead = false;
            }
            var mockFileData = cachedMockFileData;
            if (mockFileData == null)
            {
                if (throwIfNotExisting)
                {
                    throw CommonExceptions.FileNotFound(path);
                }
                else
                {
                    return null;
                }
            }
            return mockFileData;
        }

        private MockFileData GetMockFileDataForWrite()
        {
            refreshOnNextRead = true;
            return mockFileSystem.GetFile(path)
                ?? throw CommonExceptions.FileNotFound(path);
        }
    }
}
