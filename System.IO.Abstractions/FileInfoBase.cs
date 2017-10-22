using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    [Serializable]
    public abstract class FileInfoBase : FileSystemInfoBase
    {
        public abstract StreamWriter AppendText();
        public abstract FileInfoBase CopyTo(string destFileName);
        public abstract FileInfoBase CopyTo(string destFileName, bool overwrite);
        public abstract Stream Create();
        public abstract StreamWriter CreateText();
#if NET40
        public abstract void Decrypt();
        public abstract void Encrypt();
#endif
        public abstract FileSecurity GetAccessControl();
        public abstract FileSecurity GetAccessControl(AccessControlSections includeSections);
        public abstract void MoveTo(string destFileName);
        public abstract Stream Open(FileMode mode);
        public abstract Stream Open(FileMode mode, FileAccess access);
        public abstract Stream Open(FileMode mode, FileAccess access, FileShare share);
        public abstract Stream OpenRead();
        public abstract StreamReader OpenText();
        public abstract Stream OpenWrite();
#if NET40
        public abstract FileInfoBase Replace(string destinationFileName, string destinationBackupFileName);
        public abstract FileInfoBase Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);
#endif
        public abstract void SetAccessControl(FileSecurity fileSecurity);
        public abstract DirectoryInfoBase Directory { get; }
        public abstract string DirectoryName { get; }
        public abstract bool IsReadOnly { get; set; }
        public abstract long Length { get; }

        public static implicit operator FileInfoBase(FileInfo fileInfo)
        {
            return new FileInfoWrapper(fileInfo);
        }
    }
}