﻿using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class MockFileInfo : FileInfoBase, IFileSystemAclSupport
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
            mockFileSystem.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            this.originalPath = path;
            this.path = mockFileSystem.Path.GetFullPath(path);
            this.mockFile = new MockFile(mockFileSystem);
            Refresh();
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override void CreateAsSymbolicLink(string pathToTarget)
        {
            FileSystem.File.CreateSymbolicLink(FullName, pathToTarget);
        }
#endif

        /// <inheritdoc />
        public override void Delete()
        {
            refreshOnNextRead = true;
            mockFile.Delete(path);
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            var mockFileData = mockFileSystem.GetFile(path)?.Clone();
            cachedMockFileData = mockFileData ?? MockFileData.NullObject.Clone();
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override IFileSystemInfo ResolveLinkTarget(bool returnFinalTarget)
        {
            return FileSystem.File.ResolveLinkTarget(FullName, returnFinalTarget);
        }
#endif

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
                mockFileData.Attributes = value & ~FileAttributes.Directory;
            }
        }

        /// <inheritdoc />
        public override DateTime CreationTime
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.CreationTime.LocalDateTime;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.CreationTime = AdjustUnspecifiedKind(value, DateTimeKind.Local);
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
                mockFileData.CreationTime = AdjustUnspecifiedKind(value, DateTimeKind.Utc);
            }
        }

        /// <inheritdoc />
        public override bool Exists
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return (int)mockFileData.Attributes != -1 && !mockFileData.IsDirectory;
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
                return mockFileData.LastAccessTime.LocalDateTime;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.LastAccessTime = AdjustUnspecifiedKind(value, DateTimeKind.Local);
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
                mockFileData.LastAccessTime = AdjustUnspecifiedKind(value, DateTimeKind.Utc);
            }
        }

        /// <inheritdoc />
        public override DateTime LastWriteTime
        {
            get
            {
                var mockFileData = GetMockFileDataForRead();
                return mockFileData.LastWriteTime.LocalDateTime;
            }
            set
            {
                var mockFileData = GetMockFileDataForWrite();
                mockFileData.LastWriteTime = AdjustUnspecifiedKind(value, DateTimeKind.Local);
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
                mockFileData.LastWriteTime = AdjustUnspecifiedKind(value, DateTimeKind.Utc);
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
            return new StreamWriter(new MockFileStream(mockFileSystem, FullName, FileMode.Append, FileAccess.Write));
        }

        /// <inheritdoc />
        public override IFileInfo CopyTo(string destFileName)
        {
            return CopyTo(destFileName, false);
        }

        /// <inheritdoc />
        public override IFileInfo CopyTo(string destFileName, bool overwrite)
        {
            if (destFileName == FullName)
            {
                return this;
            }
            mockFile.Copy(FullName, destFileName, overwrite);
            return mockFileSystem.FileInfo.New(destFileName);
        }

        /// <inheritdoc />
        public override FileSystemStream Create()
        {
            var result = mockFile.Create(FullName);
            refreshOnNextRead = true;
            return result;
        }

        /// <inheritdoc />
        public override StreamWriter CreateText()
        {
            var result = mockFile.CreateText(FullName);
            refreshOnNextRead = true;
            return result;
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
        public override void MoveTo(string destFileName)
        {
            mockFile.Move(path, destFileName);
            path = mockFileSystem.Path.GetFullPath(destFileName);
        }

#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc />
        public override void MoveTo(string destFileName, bool overwrite)
        {
            mockFile.Move(path, destFileName, overwrite);
            path = mockFileSystem.Path.GetFullPath(destFileName);
        }
#endif

        /// <inheritdoc />
        public override FileSystemStream Open(FileMode mode)
        {
            return mockFile.Open(FullName, mode);
        }

        /// <inheritdoc />
        public override FileSystemStream Open(FileMode mode, FileAccess access)
        {
            return mockFile.Open(FullName, mode, access);
        }

        /// <inheritdoc />
        public override FileSystemStream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return mockFile.Open(FullName, mode, access, share);
        }

#if FEATURE_FILESTREAM_OPTIONS
        /// <inheritdoc />
        public override FileSystemStream Open(FileStreamOptions options)
        {
            return mockFile.Open(FullName, options.Mode, options.Access, options.Share);
        }
#endif

        /// <inheritdoc />
        public override FileSystemStream OpenRead() => mockFile.OpenRead(path);

        /// <inheritdoc />
        public override StreamReader OpenText() => mockFile.OpenText(path);

        /// <inheritdoc />
        public override FileSystemStream OpenWrite() => mockFile.OpenWrite(path);

        /// <inheritdoc />
        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName)
        {
            return Replace(destinationFileName, destinationBackupFileName, false);
        }

        /// <inheritdoc />
        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            mockFile.Replace(path, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
            return mockFileSystem.FileInfo.New(destinationFileName);
        }

        /// <inheritdoc />
        public override IDirectoryInfo Directory
        {
            get
            {
                return mockFileSystem.DirectoryInfo.New(DirectoryName);
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
                var mockFileData = GetMockFileDataForRead();
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

        /// <inheritdoc cref="IFileSystemAclSupport.GetAccessControl()" />
        [SupportedOSPlatform("windows")]
        public object GetAccessControl()
        {
            return GetMockFileData().AccessControl;
        }

        /// <inheritdoc cref="IFileSystemAclSupport.GetAccessControl(IFileSystemAclSupport.AccessControlSections)" />
        [SupportedOSPlatform("windows")]
        public object GetAccessControl(IFileSystemAclSupport.AccessControlSections includeSections)
        {
            return GetMockFileData().AccessControl;
        }

        /// <inheritdoc cref="IFileSystemAclSupport.SetAccessControl(object)" />
        [SupportedOSPlatform("windows")]
        public void SetAccessControl(object value)
        {
            GetMockFileData().AccessControl = value as FileSecurity;
        }

        private MockFileData GetMockFileData()
        {
            return mockFileSystem.GetFile(path)
                   ?? throw CommonExceptions.FileNotFound(path);
        }

        private static DateTime AdjustUnspecifiedKind(DateTime time, DateTimeKind fallbackKind)
        {
            if (time.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(time, fallbackKind);
            }

            return time;
        }

        private MockFileData GetMockFileDataForRead()
        {
            if (refreshOnNextRead)
            {
                Refresh();
                refreshOnNextRead = false;
            }
            return cachedMockFileData;
        }

        private MockFileData GetMockFileDataForWrite()
        {
            refreshOnNextRead = true;
            return mockFileSystem.GetFile(path)
                ?? throw CommonExceptions.FileNotFound(path);
        }
    }
}
