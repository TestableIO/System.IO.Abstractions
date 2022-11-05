using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class FileInfoWrapper : FileInfoBase
    {
        private readonly FileInfo instance;

        /// <inheritdoc />
        public FileInfoWrapper(IFileSystem fileSystem, FileInfo instance) : base(fileSystem)
        {
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.Extensibility = new FileSystemExtensibility(instance);
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override void CreateAsSymbolicLink(string pathToTarget)
        {
            instance.CreateAsSymbolicLink(pathToTarget);
        }
#endif

        /// <inheritdoc />
        public override void Delete()
        {
            instance.Delete();
        }

        /// <inheritdoc />
        public override void Refresh()
        {
            instance.Refresh();
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override IFileSystemInfo ResolveLinkTarget(bool returnFinalTarget)
        {
            return instance.ResolveLinkTarget(returnFinalTarget).WrapFileSystemInfo(FileSystem);
        }
#endif

        /// <inheritdoc />
        public override FileAttributes Attributes
        {
            get { return instance.Attributes; }
            set { instance.Attributes = value; }
        }

        /// <inheritdoc />
        public override DateTime CreationTime
        {
            get { return instance.CreationTime; }
            set { instance.CreationTime = value; }
        }

        /// <inheritdoc />
        public override DateTime CreationTimeUtc
        {
            get { return instance.CreationTimeUtc; }
            set { instance.CreationTimeUtc = value; }
        }

        /// <inheritdoc />
        public override bool Exists
        {
            get { return instance.Exists; }
        }

        /// <inheritdoc />
        public override string Extension
        {
            get { return instance.Extension; }
        }


        /// <inheritdoc />
        public override IFileSystemExtensibility Extensibility { get; }

        /// <inheritdoc />
        public override string FullName
        {
            get { return instance.FullName; }
        }

        /// <inheritdoc />
        public override DateTime LastAccessTime
        {
            get { return instance.LastAccessTime; }
            set { instance.LastAccessTime = value; }
        }

        /// <inheritdoc />
        public override DateTime LastAccessTimeUtc
        {
            get { return instance.LastAccessTimeUtc; }
            set { instance.LastAccessTimeUtc = value; }
        }

        /// <inheritdoc />
        public override DateTime LastWriteTime
        {
            get { return instance.LastWriteTime; }
            set { instance.LastWriteTime = value; }
        }

        /// <inheritdoc />
        public override DateTime LastWriteTimeUtc
        {
            get { return instance.LastWriteTimeUtc; }
            set { instance.LastWriteTimeUtc = value; }
        }

#if FEATURE_FILE_SYSTEM_INFO_LINK_TARGET
        /// <inheritdoc />
        public override string LinkTarget
        {
            get { return instance.LinkTarget; }
        }
#endif

        /// <inheritdoc />
        public override string Name
        {
            get { return instance.Name; }
        }

        /// <inheritdoc />
        public override StreamWriter AppendText()
        {
            return instance.AppendText();
        }

        /// <inheritdoc />
        public override IFileInfo CopyTo(string destFileName)
        {
            return new FileInfoWrapper(FileSystem, instance.CopyTo(destFileName));
        }

        /// <inheritdoc />
        public override IFileInfo CopyTo(string destFileName, bool overwrite)
        {
            return new FileInfoWrapper(FileSystem, instance.CopyTo(destFileName, overwrite));
        }

        /// <inheritdoc />
        public override FileSystemStream Create()
        {
            return new FileStreamWrapper(instance.Create());
        }

        /// <inheritdoc />
        public override StreamWriter CreateText()
        {
            return instance.CreateText();
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override void Decrypt()
        {
            instance.Decrypt();
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override void Encrypt()
        {
            instance.Encrypt();
        }

        /// <inheritdoc />

        [SupportedOSPlatform("windows")]
        public override FileSecurity GetAccessControl()
        {
            return instance.GetAccessControl();
        }

        /// <inheritdoc />

        [SupportedOSPlatform("windows")]
        public override FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            return instance.GetAccessControl(includeSections);
        }

        /// <inheritdoc />
        public override void MoveTo(string destFileName)
        {
            instance.MoveTo(destFileName);
        }

#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc />
        public override void MoveTo(string destFileName, bool overwrite)
        {
            instance.MoveTo(destFileName, overwrite);
        }
#endif

        /// <inheritdoc />
        public override FileSystemStream Open(FileMode mode)
        {
            return new FileStreamWrapper(instance.Open(mode));
        }

        /// <inheritdoc />
        public override FileSystemStream Open(FileMode mode, FileAccess access)
        {
            return new FileStreamWrapper(instance.Open(mode, access));
        }

        /// <inheritdoc />
        public override FileSystemStream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return new FileStreamWrapper(instance.Open(mode, access, share));
        }

#if FEATURE_FILESTREAM_OPTIONS
        /// <inheritdoc />
        public override FileSystemStream Open(FileStreamOptions options)
        {
            return new FileStreamWrapper(instance.Open(options));
        }
#endif

        /// <inheritdoc />
        public override FileSystemStream OpenRead()
        {
            return new FileStreamWrapper(instance.OpenRead());
        }

        /// <inheritdoc />
        public override StreamReader OpenText()
        {
            return instance.OpenText();
        }

        /// <inheritdoc />
        public override FileSystemStream OpenWrite()
        {
            return new FileStreamWrapper(instance.OpenWrite());
        }

        /// <inheritdoc />
        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName)
        {
            return new FileInfoWrapper(FileSystem, instance.Replace(destinationFileName, destinationBackupFileName));
        }

        /// <inheritdoc />
        public override IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            return new FileInfoWrapper(FileSystem, instance.Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors));
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override void SetAccessControl(FileSecurity fileSecurity)
        {
            instance.SetAccessControl(fileSecurity);
        }

        /// <inheritdoc />
        public override IDirectoryInfo Directory
        {
            get { return new DirectoryInfoWrapper(FileSystem, instance.Directory); }
        }

        /// <inheritdoc />
        public override string DirectoryName
        {
            get { return instance.DirectoryName; }
        }

        /// <inheritdoc />
        public override bool IsReadOnly
        {
            get { return instance.IsReadOnly; }
            set { instance.IsReadOnly = value; }
        }

        /// <inheritdoc />
        public override long Length
        {
            get { return instance.Length; }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return instance.ToString();
        }
    }
}
