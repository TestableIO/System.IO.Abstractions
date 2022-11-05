using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="FileInfo" />
    public interface IFileInfo : IFileSystemInfo
    {
        /// <inheritdoc cref="FileInfo.Directory" />
        IDirectoryInfo? Directory { get; }

        /// <inheritdoc cref="FileInfo.DirectoryName" />
        string? DirectoryName { get; }

        /// <inheritdoc cref="FileInfo.IsReadOnly" />
        bool IsReadOnly { get; set; }

        /// <inheritdoc cref="FileInfo.Length" />
        long Length { get; }

        /// <inheritdoc cref="FileInfo.AppendText()" />
        public StreamWriter AppendText();

        /// <inheritdoc cref="FileInfo.CopyTo(string)" />
        IFileInfo CopyTo(string destFileName);

        /// <inheritdoc cref="FileInfo.CopyTo(string, bool)" />
        IFileInfo CopyTo(string destFileName, bool overwrite);

        /// <inheritdoc cref="FileInfo.Create()" />
        FileSystemStream Create();

        /// <inheritdoc cref="FileInfo.CreateText()" />
        public StreamWriter CreateText();

        /// <inheritdoc cref="FileInfo.Decrypt()" />
        [SupportedOSPlatform("windows")]
        void Decrypt();

        /// <inheritdoc cref="FileInfo.Encrypt()" />
        [SupportedOSPlatform("windows")]
        void Encrypt();

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(FileInfo)"/>
#else
        /// <inheritdoc cref="FileInfo.GetAccessControl()"/>
#endif
        FileSecurity GetAccessControl();
#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(FileInfo,AccessControlSections)"/>
#else
        /// <inheritdoc cref="File.GetAccessControl(string,AccessControlSections)"/>
#endif
        FileSecurity GetAccessControl(AccessControlSections includeSections);

        /// <inheritdoc cref="FileInfo.MoveTo(string)" />
        void MoveTo(string destFileName);

#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc cref="FileInfo.MoveTo(string, bool)" />
        void MoveTo(string destFileName, bool overwrite);
#endif

        /// <inheritdoc cref="FileInfo.Open(FileMode)" />
        FileSystemStream Open(FileMode mode);

        /// <inheritdoc cref="FileInfo.Open(FileMode, FileAccess)" />
        FileSystemStream Open(FileMode mode, FileAccess access);

        /// <inheritdoc cref="FileInfo.Open(FileMode, FileAccess, FileShare)" />
        FileSystemStream Open(FileMode mode, FileAccess access, FileShare share);

#if FEATURE_FILESTREAM_OPTIONS
        /// <inheritdoc cref="FileInfo.Open(FileStreamOptions)" />
        FileSystemStream Open(FileStreamOptions options);
#endif

        /// <inheritdoc cref="FileInfo.OpenRead()" />
        FileSystemStream OpenRead();

        /// <inheritdoc cref="FileInfo.OpenText()" />
        public StreamReader OpenText();

        /// <inheritdoc cref="FileInfo.OpenWrite()" />
        FileSystemStream OpenWrite();

        /// <inheritdoc cref="FileInfo.Replace(string, string?)" />
        IFileInfo Replace(string destinationFileName,
            string? destinationBackupFileName);

        /// <inheritdoc cref="FileInfo.Replace(string, string?, bool)" />
        IFileInfo Replace(string destinationFileName,
            string? destinationBackupFileName,
            bool ignoreMetadataErrors);

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
#endif
        /// <inheritdoc cref="M:FileInfo.SetAccessControl(FileSecurity)"/>
        void SetAccessControl(FileSecurity fileSecurity);
    }
}