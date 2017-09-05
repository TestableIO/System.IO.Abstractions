using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    public interface IFile : IFileSystemEntry
    {
        IDirectory Directory { get; }
        string DirectoryName { get; }
        bool IsReadOnly { get; set; }
        long Length { get; }

        StreamWriter AppendText();
        IFile CopyTo(string destFileName);
        IFile CopyTo(string destFileName, bool overwrite);
        Stream Create();
        StreamWriter CreateText();
        void Decrypt();
        void Encrypt();
        FileSecurity GetAccessControl();
        FileSecurity GetAccessControl(AccessControlSections includeSections);
        void MoveTo(string destFileName);
        Stream Open(FileMode mode);
        Stream Open(FileMode mode, FileAccess access);
        Stream Open(FileMode mode, FileAccess access, FileShare share);
        Stream OpenRead();
        StreamReader OpenText();
        Stream OpenWrite();
        IFile Replace(string destinationFileName, string destinationBackupFileName);
        IFile Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);
        void SetAccessControl(FileSecurity fileSecurity);
    }
}