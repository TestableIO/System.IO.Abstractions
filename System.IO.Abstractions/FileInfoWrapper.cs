using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    [Serializable]
    public class FileInfoWrapper : FileInfoBase
    {
        private readonly FileInfo instance;

        public FileInfoWrapper(IFileSystem fileSystem, FileInfo instance) : base(fileSystem)
        {
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public override void Delete()
        {
            instance.Delete();
        }

        public override void Refresh()
        {
            instance.Refresh();
        }

        public override FileAttributes Attributes
        {
            get { return instance.Attributes; }
            set { instance.Attributes = value; }
        }

        public override DateTime CreationTime
        {
            get { return instance.CreationTime; }
            set { instance.CreationTime = value; }
        }

        public override DateTime CreationTimeUtc
        {
            get { return instance.CreationTimeUtc; }
            set { instance.CreationTimeUtc = value; }
        }

        public override bool Exists
        {
            get { return instance.Exists; }
        }

        public override string Extension
        {
            get { return instance.Extension; }
        }

        public override string FullName
        {
            get { return instance.FullName; }
        }

        public override DateTime LastAccessTime
        {
            get { return instance.LastAccessTime; }
            set { instance.LastAccessTime = value; }
        }

        public override DateTime LastAccessTimeUtc
        {
            get { return instance.LastAccessTimeUtc; }
            set { instance.LastAccessTimeUtc = value; }
        }

        public override DateTime LastWriteTime
        {
            get { return instance.LastWriteTime; }
            set { instance.LastWriteTime = value; }
        }

        public override DateTime LastWriteTimeUtc
        {
            get { return instance.LastWriteTimeUtc; }
            set { instance.LastWriteTimeUtc = value; }
        }

        public override string Name
        {
            get { return instance.Name; }
        }

        public override StreamWriter AppendText()
        {
            return instance.AppendText();
        }

        public override IFileInfo CopyTo(string destFileName)
        {
            return new FileInfoWrapper(FileSystem, instance.CopyTo(destFileName));
        }

        public override IFileInfo CopyTo(string destFileName, bool overwrite)
        {
            return new FileInfoWrapper(FileSystem, instance.CopyTo(destFileName, overwrite));
        }

        public override Stream Create()
        {
            return instance.Create();
        }

        public override StreamWriter CreateText()
        {
            return instance.CreateText();
        }

#if NET40
        public override void Decrypt()
        {
            instance.Decrypt();
        }

        public override void Encrypt()
        {
            instance.Encrypt();
        }
#endif

        public override FileSecurity GetAccessControl()
        {
            return instance.GetAccessControl();
        }

        public override FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            return instance.GetAccessControl(includeSections);
        }

        public override void MoveTo(string destFileName)
        {
            instance.MoveTo(destFileName);
        }

        public override Stream Open(FileMode mode)
        {
            return instance.Open(mode);
        }

        public override Stream Open(FileMode mode, FileAccess access)
        {
            return instance.Open(mode, access);
        }

        public override Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return instance.Open(mode, access, share);
        }

        public override Stream OpenRead()
        {
            return instance.OpenRead();
        }

        public override StreamReader OpenText()
        {
            return instance.OpenText();
        }

        public override Stream OpenWrite()
        {
            return instance.OpenWrite();
        }

#if NET40
        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName)
        {
            return new FileInfoWrapper(FileSystem, instance.Replace(destinationFileName, destinationBackupFileName));
        }

        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            return new FileInfoWrapper(FileSystem, instance.Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors));
        }
#endif

        public override void SetAccessControl(FileSecurity fileSecurity)
        {
            instance.SetAccessControl(fileSecurity);
        }

        public override IDirectoryInfo Directory
        {
            get { return new DirectoryInfoWrapper(FileSystem, instance.Directory); }
        }

        public override string DirectoryName
        {
            get { return instance.DirectoryName; }
        }

        public override bool IsReadOnly
        {
            get { return instance.IsReadOnly; }
            set { instance.IsReadOnly = value; }
        }

        public override long Length
        {
            get { return instance.Length; }
        }
    }
}
