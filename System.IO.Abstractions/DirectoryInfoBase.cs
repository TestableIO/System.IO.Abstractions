using System.Collections.Generic;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="DirectoryInfo"/>
    [Serializable]
    public abstract class DirectoryInfoBase : FileSystemInfoBase, IDirectoryInfo
    {
        protected DirectoryInfoBase(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        [Obsolete("This constructor only exists to support mocking libraries.", error: true)]
        internal DirectoryInfoBase() { }

        /// <inheritdoc cref="DirectoryInfo.Create()"/>
        public abstract void Create();
#if NET40
        /// <inheritdoc cref="DirectoryInfo.Create(DirectorySecurity)"/>
        public abstract void Create(DirectorySecurity directorySecurity);
#endif

        /// <inheritdoc cref="DirectoryInfo.CreateSubdirectory(string)"/>
        public abstract IDirectoryInfo CreateSubdirectory(string path);
#if NET40
        /// <inheritdoc cref="DirectoryInfo.CreateSubdirectory(string,DirectorySecurity)"/>
        public abstract IDirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity);
#endif

        /// <inheritdoc cref="DirectoryInfo.Delete(bool)"/>
        public abstract void Delete(bool recursive);

        /// <inheritdoc cref="DirectoryInfo.EnumerateDirectories()"/>
        public abstract IEnumerable<IDirectoryInfo> EnumerateDirectories();

        /// <inheritdoc cref="DirectoryInfo.EnumerateDirectories(string)"/>
        public abstract IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern);

        /// <inheritdoc cref="DirectoryInfo.EnumerateDirectories(string,SearchOption)"/>
        public abstract IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="DirectoryInfo.EnumerateFiles()"/>
        public abstract IEnumerable<IFileInfo> EnumerateFiles();

        /// <inheritdoc cref="DirectoryInfo.EnumerateFiles(string)"/>
        public abstract IEnumerable<IFileInfo> EnumerateFiles(string searchPattern);

        /// <inheritdoc cref="DirectoryInfo.EnumerateFiles(string,SearchOption)"/>
        public abstract IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="DirectoryInfo.EnumerateFileSystemInfos()"/>
        public abstract IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos();

        /// <inheritdoc cref="DirectoryInfo.EnumerateFileSystemInfos(string)"/>
        public abstract IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern);

        /// <inheritdoc cref="DirectoryInfo.EnumerateFileSystemInfos(string,SearchOption)"/>
        public abstract IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="DirectoryInfo.GetAccessControl()"/>
        public abstract DirectorySecurity GetAccessControl();

        /// <inheritdoc cref="DirectoryInfo.GetAccessControl(AccessControlSections)"/>
        public abstract DirectorySecurity GetAccessControl(AccessControlSections includeSections);

        /// <inheritdoc cref="DirectoryInfo.GetDirectories()"/>
        public abstract IDirectoryInfo[] GetDirectories();

        /// <inheritdoc cref="DirectoryInfo.GetDirectories(string)"/>
        public abstract IDirectoryInfo[] GetDirectories(string searchPattern);

        /// <inheritdoc cref="DirectoryInfo.GetDirectories(string,SearchOption)"/>
        public abstract IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="DirectoryInfo.GetFiles(string)"/>
        public abstract IFileInfo[] GetFiles();

        /// <inheritdoc cref="DirectoryInfo.GetFiles(string)"/>
        public abstract IFileInfo[] GetFiles(string searchPattern);

        /// <inheritdoc cref="DirectoryInfo.GetFiles(string,SearchOption)"/>
        public abstract IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="DirectoryInfo.GetFileSystemInfos()"/>
        public abstract IFileSystemInfo[] GetFileSystemInfos();

        /// <inheritdoc cref="DirectoryInfo.GetFileSystemInfos(string)"/>
        public abstract IFileSystemInfo[] GetFileSystemInfos(string searchPattern);

        /// <inheritdoc cref="DirectoryInfo.GetFileSystemInfos(string,SearchOption)"/>
        public abstract IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="DirectoryInfo.MoveTo"/>
        public abstract void MoveTo(string destDirName);

        /// <inheritdoc cref="DirectoryInfo.SetAccessControl"/>
        public abstract void SetAccessControl(DirectorySecurity directorySecurity);

        /// <inheritdoc cref="DirectoryInfo.Parent"/>
        public abstract IDirectoryInfo Parent { get; }

        /// <inheritdoc cref="DirectoryInfo.Root"/>
        public abstract IDirectoryInfo Root { get; }

        public static implicit operator DirectoryInfoBase(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
                return null;
            return new DirectoryInfoWrapper(new FileSystem(), directoryInfo);
        }
    }
}
