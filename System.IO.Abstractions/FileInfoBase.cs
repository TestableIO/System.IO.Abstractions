using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    [Serializable]
    public abstract class FileInfoBase : FileSystemInfoBase, IFile
    {
        public abstract StreamWriter AppendText();
        public abstract IFile CopyTo(string destFileName);
        public abstract IFile CopyTo(string destFileName, bool overwrite);
        public abstract Stream Create();
        public abstract StreamWriter CreateText();
        public abstract void Decrypt();
        public abstract void Encrypt();
        public abstract FileSecurity GetAccessControl();
        public abstract FileSecurity GetAccessControl(AccessControlSections includeSections);
        public abstract void MoveTo(string destFileName);
        public abstract Stream Open(FileMode mode);
        public abstract Stream Open(FileMode mode, FileAccess access);
        public abstract Stream Open(FileMode mode, FileAccess access, FileShare share);
        public abstract Stream OpenRead();
        public abstract StreamReader OpenText();
        public abstract Stream OpenWrite();
        public abstract IFile Replace(string destinationFileName, string destinationBackupFileName);
        public abstract IFile Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);
        public abstract void SetAccessControl(FileSecurity fileSecurity);
        public abstract IDirectory Directory { get; }
        public abstract string DirectoryName { get; }
        public abstract bool IsReadOnly { get; set; }
        public abstract long Length { get; }

        public static implicit operator FileInfoBase(FileInfo fileInfo)
        {
            return new FileInfoWrapper(fileInfo);
        }
    }
}