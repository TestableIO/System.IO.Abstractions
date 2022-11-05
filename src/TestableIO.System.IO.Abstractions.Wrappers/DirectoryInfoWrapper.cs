using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class DirectoryInfoWrapper : DirectoryInfoBase
    {
        private readonly DirectoryInfo instance;

        /// <inheritdoc />
        public DirectoryInfoWrapper(IFileSystem fileSystem, DirectoryInfo instance) : base(fileSystem)
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
        public override void Create()
        {
            instance.Create();
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override void Create(DirectorySecurity directorySecurity)
        {
            instance.Create(directorySecurity);
        }

        /// <inheritdoc />
        public override IDirectoryInfo CreateSubdirectory(string path)
        {
            return new DirectoryInfoWrapper(FileSystem, instance.CreateSubdirectory(path));
        }

        /// <inheritdoc />
        public override void Delete(bool recursive)
        {
            instance.Delete(recursive);
        }

        /// <inheritdoc />
        public override IEnumerable<IDirectoryInfo> EnumerateDirectories()
        {
            return instance.EnumerateDirectories().Select(directoryInfo => new DirectoryInfoWrapper(FileSystem, directoryInfo));
        }

        /// <inheritdoc />
        public override IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern)
        {
            return instance.EnumerateDirectories(searchPattern).Select(directoryInfo => new DirectoryInfoWrapper(FileSystem, directoryInfo));
        }

        /// <inheritdoc />
        public override IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return instance.EnumerateDirectories(searchPattern, searchOption).Select(directoryInfo => new DirectoryInfoWrapper(FileSystem, directoryInfo));
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return instance.EnumerateDirectories(searchPattern, enumerationOptions).Select(directoryInfo => new DirectoryInfoWrapper(FileSystem, directoryInfo));
        }
#endif

        /// <inheritdoc />
        public override IEnumerable<IFileInfo> EnumerateFiles()
        {
            return instance.EnumerateFiles().Select(fileInfo => new FileInfoWrapper(FileSystem, fileInfo));
        }

        /// <inheritdoc />
        public override IEnumerable<IFileInfo> EnumerateFiles(string searchPattern)
        {
            return instance.EnumerateFiles(searchPattern).Select(fileInfo => new FileInfoWrapper(FileSystem, fileInfo));
        }

        /// <inheritdoc />
        public override IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return instance.EnumerateFiles(searchPattern, searchOption).Select(fileInfo => new FileInfoWrapper(FileSystem, fileInfo));
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return instance.EnumerateFiles(searchPattern, enumerationOptions).Select(fileInfo => new FileInfoWrapper(FileSystem, fileInfo));
        }
#endif

        /// <inheritdoc />
        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos()
        {
            return instance.EnumerateFileSystemInfos().WrapFileSystemInfos(FileSystem);
        }

        /// <inheritdoc />
        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
        {
            return instance.EnumerateFileSystemInfos(searchPattern).WrapFileSystemInfos(FileSystem);
        }

        /// <inheritdoc />
        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return instance.EnumerateFileSystemInfos(searchPattern, searchOption).WrapFileSystemInfos(FileSystem);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return instance.EnumerateFileSystemInfos(searchPattern, enumerationOptions).WrapFileSystemInfos(FileSystem);
        }
#endif
        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override DirectorySecurity GetAccessControl()
        {
            return instance.GetAccessControl();
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return instance.GetAccessControl(includeSections);
        }

        /// <inheritdoc />
        public override IDirectoryInfo[] GetDirectories()
        {
            return instance.GetDirectories().WrapDirectories(FileSystem);
        }

        /// <inheritdoc />
        public override IDirectoryInfo[] GetDirectories(string searchPattern)
        {
            return instance.GetDirectories(searchPattern).WrapDirectories(FileSystem);
        }

        /// <inheritdoc />
        public override IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return instance.GetDirectories(searchPattern, searchOption).WrapDirectories(FileSystem);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return instance.GetDirectories(searchPattern, enumerationOptions).WrapDirectories(FileSystem);
        }
#endif

        /// <inheritdoc />
        public override IFileInfo[] GetFiles()
        {
            return instance.GetFiles().WrapFiles(FileSystem);
        }

        /// <inheritdoc />
        public override IFileInfo[] GetFiles(string searchPattern)
        {
            return instance.GetFiles(searchPattern).WrapFiles(FileSystem);
        }

        /// <inheritdoc />
        public override IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return instance.GetFiles(searchPattern, searchOption).WrapFiles(FileSystem);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return instance.GetFiles(searchPattern, enumerationOptions).WrapFiles(FileSystem);
        }
#endif

        /// <inheritdoc />
        public override IFileSystemInfo[] GetFileSystemInfos()
        {
            return instance.GetFileSystemInfos().WrapFileSystemInfos(FileSystem);
        }

        /// <inheritdoc />
        public override IFileSystemInfo[] GetFileSystemInfos(string searchPattern)
        {
            return instance.GetFileSystemInfos(searchPattern).WrapFileSystemInfos(FileSystem);
        }

        /// <inheritdoc />
        public override IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return instance.GetFileSystemInfos(searchPattern, searchOption).WrapFileSystemInfos(FileSystem);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions)
        {
            return instance.GetFileSystemInfos(searchPattern, enumerationOptions).WrapFileSystemInfos(FileSystem);
        }
#endif

        /// <inheritdoc />
        public override void MoveTo(string destDirName)
        {
            instance.MoveTo(destDirName);
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override void SetAccessControl(DirectorySecurity directorySecurity)
        {
            instance.SetAccessControl(directorySecurity);
        }

        /// <inheritdoc />
        public override IDirectoryInfo Parent
        {
            get
            {
                if (instance.Parent == null)
                {
                    return null;
                }
                else
                {
                    return new DirectoryInfoWrapper(FileSystem, instance.Parent);
                }
            }
        }

        /// <inheritdoc />
        public override IDirectoryInfo Root
            => new DirectoryInfoWrapper(FileSystem, instance.Root);

        /// <inheritdoc />
        public override string ToString()
        {
            return instance.ToString();
        }
    }
}
