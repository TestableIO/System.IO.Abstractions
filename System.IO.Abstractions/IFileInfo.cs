using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    public interface IFileInfo: IFileSystemInfo
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
#if NET40
        /// <inheritdoc cref="FileInfo.Decrypt"/>
        void Decrypt();
        /// <inheritdoc cref="FileInfo.Encrypt"/>
        void Encrypt();
#endif
        /// <inheritdoc cref="FileInfo.GetAccessControl()"/>
        FileSecurity GetAccessControl();
        /// <inheritdoc cref="FileInfo.GetAccessControl(AccessControlSections)"/>
        FileSecurity GetAccessControl(AccessControlSections includeSections);
        /// <inheritdoc cref="FileInfo.MoveTo"/>
        void MoveTo(string destFileName);
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
#if NET40
        /// <inheritdoc cref="FileInfo.Replace(string,string)"/>
        IFileInfo Replace(string destinationFileName, string destinationBackupFileName);
        /// <inheritdoc cref="FileInfo.Replace(string,string,bool)"/>
        IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);
#endif
        /// <inheritdoc cref="FileInfo.SetAccessControl(FileSecurity)"/>
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