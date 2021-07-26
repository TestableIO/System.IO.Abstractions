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

        /// <inheritdoc cref="IDirectoryInfo.Create()"/>
        public abstract void Create();
        /// <inheritdoc cref="IDirectoryInfo.Create(DirectorySecurity)"/>
        public abstract void Create(DirectorySecurity directorySecurity);

        /// <inheritdoc cref="IDirectoryInfo.CreateSubdirectory(string)"/>
        public abstract IDirectoryInfo CreateSubdirectory(string path);

        /// <inheritdoc cref="IDirectoryInfo.Delete(bool)"/>
        public abstract void Delete(bool recursive);

        /// <inheritdoc cref="IDirectoryInfo.EnumerateDirectories()"/>
        public abstract IEnumerable<IDirectoryInfo> EnumerateDirectories();

        /// <inheritdoc cref="IDirectoryInfo.EnumerateDirectories(string)"/>
        public abstract IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern);

        /// <inheritdoc cref="IDirectoryInfo.EnumerateDirectories(string,SearchOption)"/>
        public abstract IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectoryInfo.EnumerateDirectories(string,EnumerationOptions)"/>
        public abstract IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectoryInfo.EnumerateFiles()"/>
        public abstract IEnumerable<IFileInfo> EnumerateFiles();

        /// <inheritdoc cref="IDirectoryInfo.EnumerateFiles(string)"/>
        public abstract IEnumerable<IFileInfo> EnumerateFiles(string searchPattern);

        /// <inheritdoc cref="IDirectoryInfo.EnumerateFiles(string,SearchOption)"/>
        public abstract IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectoryInfo.EnumerateFiles(string,EnumerationOptions)"/>
        public abstract IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectoryInfo.EnumerateFileSystemInfos()"/>
        public abstract IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos();

        /// <inheritdoc cref="IDirectoryInfo.EnumerateFileSystemInfos(string)"/>
        public abstract IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern);

        /// <inheritdoc cref="IDirectoryInfo.EnumerateFileSystemInfos(string,SearchOption)"/>
        public abstract IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectoryInfo.EnumerateFileSystemInfos(string,EnumerationOptions)"/>
        public abstract IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectoryInfo.GetAccessControl()"/>
        public abstract DirectorySecurity GetAccessControl();

        /// <inheritdoc cref="IDirectoryInfo.GetAccessControl(AccessControlSections)"/>
        public abstract DirectorySecurity GetAccessControl(AccessControlSections includeSections);

        /// <inheritdoc cref="IDirectoryInfo.GetDirectories()"/>
        public abstract IDirectoryInfo[] GetDirectories();

        /// <inheritdoc cref="IDirectoryInfo.GetDirectories(string)"/>
        public abstract IDirectoryInfo[] GetDirectories(string searchPattern);

        /// <inheritdoc cref="IDirectoryInfo.GetDirectories(string,SearchOption)"/>
        public abstract IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectoryInfo.GetDirectories(string,EnumerationOptions)"/>
        public abstract IDirectoryInfo[] GetDirectories(string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectoryInfo.GetFiles(string)"/>
        public abstract IFileInfo[] GetFiles();

        /// <inheritdoc cref="IDirectoryInfo.GetFiles(string)"/>
        public abstract IFileInfo[] GetFiles(string searchPattern);

        /// <inheritdoc cref="IDirectoryInfo.GetFiles(string,SearchOption)"/>
        public abstract IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption);


#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectoryInfo.GetFiles(string,EnumerationOptions)"/>
        public abstract IFileInfo[] GetFiles(string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectoryInfo.GetFileSystemInfos()"/>
        public abstract IFileSystemInfo[] GetFileSystemInfos();

        /// <inheritdoc cref="IDirectoryInfo.GetFileSystemInfos(string)"/>
        public abstract IFileSystemInfo[] GetFileSystemInfos(string searchPattern);

        /// <inheritdoc cref="IDirectoryInfo.GetFileSystemInfos(string,SearchOption)"/>
        public abstract IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectoryInfo.GetFileSystemInfos(string,EnumerationOptions)"/>
        public abstract IFileSystemInfo[] GetFileSystemInfos(string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectoryInfo.MoveTo"/>
        public abstract void MoveTo(string destDirName);

        /// <inheritdoc cref="IDirectoryInfo.SetAccessControl"/>
        public abstract void SetAccessControl(DirectorySecurity directorySecurity);

        /// <inheritdoc cref="IDirectoryInfo.Parent"/>
        public abstract IDirectoryInfo Parent { get; }

        /// <inheritdoc cref="IDirectoryInfo.Root"/>
        public abstract IDirectoryInfo Root { get; }

        public static implicit operator DirectoryInfoBase(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null)
            {
                return null;
            }
            return new DirectoryInfoWrapper(new FileSystem(), directoryInfo);
        }
    }
}
