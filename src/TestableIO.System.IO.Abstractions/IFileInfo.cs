﻿using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="FileInfo" />
    public interface IFileInfo : IFileSystemInfo
    {
        /// <inheritdoc cref="FileInfo.AppendText"/>
        StreamWriter AppendText();
        /// <inheritdoc cref="FileInfo.CopyTo(string)"/>
        IFileInfo CopyTo(string destFileName);
        /// <inheritdoc cref="FileInfo.CopyTo(string,bool)"/>
        IFileInfo CopyTo(string destFileName, bool overwrite);
        /// <inheritdoc cref="FileInfo.Create"/>
        Stream Create();
        /// <inheritdoc cref="FileInfo.CreateText"/>
        StreamWriter CreateText();
        /// <inheritdoc cref="FileInfo.Decrypt"/>
        void Decrypt();
        /// <inheritdoc cref="FileInfo.Encrypt"/>
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
        /// <inheritdoc cref="FileInfo.MoveTo(string)"/>
        void MoveTo(string destFileName);
#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc cref="FileInfo.MoveTo(string,bool)"/>
        void MoveTo(string destFileName, bool overwrite);
#endif
        /// <inheritdoc cref="FileInfo.Open(FileMode)"/>
        Stream Open(FileMode mode);
        /// <inheritdoc cref="FileInfo.Open(FileMode,FileAccess)"/>
        Stream Open(FileMode mode, FileAccess access);
        /// <inheritdoc cref="FileInfo.Open(FileMode,FileAccess,FileShare)"/>
        Stream Open(FileMode mode, FileAccess access, FileShare share);
        /// <inheritdoc cref="FileInfo.OpenRead"/>
        Stream OpenRead();
        /// <inheritdoc cref="FileInfo.OpenText"/>
        StreamReader OpenText();
        /// <inheritdoc cref="FileInfo.OpenWrite"/>
        Stream OpenWrite();
        /// <inheritdoc cref="FileInfo.Replace(string,string)"/>
        IFileInfo Replace(string destinationFileName, string destinationBackupFileName);
        /// <inheritdoc cref="FileInfo.Replace(string,string,bool)"/>
        IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);
        /// <inheritdoc cref="M:FileInfo.SetAccessControl(FileSecurity)"/>
        void SetAccessControl(FileSecurity fileSecurity);
        /// <inheritdoc cref="FileInfo.Directory"/>
        IDirectoryInfo Directory { get; }
        /// <inheritdoc cref="FileInfo.DirectoryName"/>
        string DirectoryName { get; }
        /// <inheritdoc cref="FileInfo.IsReadOnly"/>
        bool IsReadOnly { get; set; }
        /// <inheritdoc cref="FileInfo.Length"/>
        long Length { get; }
    }
}