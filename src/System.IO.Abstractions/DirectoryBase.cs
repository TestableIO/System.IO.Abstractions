using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="Directory"/>
    [Serializable]
    public abstract class DirectoryBase : IDirectory
    {
        /// <inheritdoc />
        protected DirectoryBase(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        [Obsolete("This constructor only exists to support mocking libraries.", error: true)]
        internal DirectoryBase() { }

        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        public IFileSystem FileSystem { get; }

        /// <inheritdoc cref="IDirectory.CreateDirectory(string)"/>
        public abstract IDirectoryInfo CreateDirectory(string path);

        /// <inheritdoc cref="IDirectory.CreateDirectory(string,DirectorySecurity)"/>
        [SupportedOSPlatform("windows")]
        public abstract IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity);
#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc cref="IDirectory.CreateSymbolicLink(string, string)"/>
        public abstract IFileSystemInfo CreateSymbolicLink(string path, string pathToTarget);
#endif
        /// <inheritdoc cref="IDirectory.Delete(string)"/>
        public abstract void Delete(string path);

        /// <inheritdoc cref="IDirectory.Delete(string,bool)"/>
        public abstract void Delete(string path, bool recursive);

        /// <inheritdoc cref="IDirectory.Exists"/>
        public abstract bool Exists(string path);

        /// <inheritdoc cref="IDirectory.GetAccessControl(string)"/>
        [SupportedOSPlatform("windows")]
        public abstract DirectorySecurity GetAccessControl(string path);

        /// <inheritdoc cref="IDirectory.GetAccessControl(string,AccessControlSections)"/>
        [SupportedOSPlatform("windows")]
        public abstract DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections);

        /// <inheritdoc cref="IDirectory.GetCreationTime"/>
        public abstract DateTime GetCreationTime(string path);

        /// <inheritdoc cref="IDirectory.GetCreationTimeUtc"/>
        public abstract DateTime GetCreationTimeUtc(string path);

        /// <inheritdoc cref="IDirectory.GetCurrentDirectory"/>
        public abstract string GetCurrentDirectory();

        /// <inheritdoc cref="IDirectory.GetDirectories(string)"/>
        public abstract string[] GetDirectories(string path);

        /// <inheritdoc cref="IDirectory.GetDirectories(string,string)"/>
        public abstract string[] GetDirectories(string path, string searchPattern);

        /// <inheritdoc cref="IDirectory.GetDirectories(string,string,SearchOption)"/>
        public abstract string[] GetDirectories(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectory.GetDirectories(string,string,EnumerationOptions)"/>
        public abstract string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectory.GetDirectoryRoot"/>
        public abstract string GetDirectoryRoot(string path);

        /// <inheritdoc cref="IDirectory.GetFiles(string)"/>
        public abstract string[] GetFiles(string path);

        /// <inheritdoc cref="IDirectory.GetFiles(string,string)"/>
        public abstract string[] GetFiles(string path, string searchPattern);

        /// <inheritdoc cref="IDirectory.GetFiles(string,string,SearchOption)"/>
        public abstract string[] GetFiles(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectory.GetFiles(string,string,EnumerationOptions)"/>
        public abstract string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectory.GetFileSystemEntries(string)"/>
        public abstract string[] GetFileSystemEntries(string path);

        /// <inheritdoc cref="IDirectory.GetFileSystemEntries(string,string)"/>
        public abstract string[] GetFileSystemEntries(string path, string searchPattern);

        /// <inheritdoc cref="IDirectory.GetLastAccessTime"/>
        public abstract DateTime GetLastAccessTime(string path);

        /// <inheritdoc cref="IDirectory.GetLastAccessTimeUtc"/>
        public abstract DateTime GetLastAccessTimeUtc(string path);

        /// <inheritdoc cref="IDirectory.GetLastWriteTime"/>
        public abstract DateTime GetLastWriteTime(string path);

        /// <inheritdoc cref="IDirectory.GetLastWriteTimeUtc"/>
        public abstract DateTime GetLastWriteTimeUtc(string path);

        /// <inheritdoc cref="IDirectory.GetLogicalDrives"/>
        public abstract string[] GetLogicalDrives();

        /// <inheritdoc cref="IDirectory.GetParent"/>
        public abstract IDirectoryInfo GetParent(string path);

        /// <inheritdoc cref="IDirectory.Move"/>
        public abstract void Move(string sourceDirName, string destDirName);

        /// <inheritdoc cref="IDirectory.SetAccessControl"/>
        public abstract void SetAccessControl(string path, DirectorySecurity directorySecurity);

        /// <inheritdoc cref="IDirectory.SetCreationTime"/>
        public abstract void SetCreationTime(string path, DateTime creationTime);

        /// <inheritdoc cref="IDirectory.SetCreationTimeUtc"/>
        public abstract void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <inheritdoc cref="IDirectory.SetCurrentDirectory"/>
        public abstract void SetCurrentDirectory(string path);

        /// <inheritdoc cref="IDirectory.SetLastAccessTime"/>
        public abstract void SetLastAccessTime(string path, DateTime lastAccessTime);

        /// <inheritdoc cref="IDirectory.SetLastAccessTimeUtc"/>
        public abstract void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <inheritdoc cref="IDirectory.SetLastWriteTime"/>
        public abstract void SetLastWriteTime(string path, DateTime lastWriteTime);

        /// <inheritdoc cref="IDirectory.SetLastWriteTimeUtc"/>
        public abstract void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <inheritdoc cref="IDirectory.EnumerateDirectories(string)"/>
        public abstract IEnumerable<string> EnumerateDirectories(string path);

        /// <inheritdoc cref="IDirectory.EnumerateDirectories(string,string)"/>
        public abstract IEnumerable<string> EnumerateDirectories(string path, string searchPattern);

        /// <inheritdoc cref="IDirectory.EnumerateDirectories(string,string,SearchOption)"/>
        public abstract IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectory.EnumerateDirectories(string,string,EnumerationOptions)"/>
        public abstract IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectory.EnumerateFiles(string)"/>
        public abstract IEnumerable<string> EnumerateFiles(string path);

        /// <inheritdoc cref="IDirectory.EnumerateFiles(string,string)"/>
        public abstract IEnumerable<string> EnumerateFiles(string path, string searchPattern);

        /// <inheritdoc cref="IDirectory.EnumerateFiles(string,string,SearchOption)"/>
        public abstract IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectory.EnumerateFiles(string,string,EnumerationOptions)"/>
        public abstract IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif

        /// <inheritdoc cref="IDirectory.EnumerateFileSystemEntries(string)"/>
        public abstract IEnumerable<string> EnumerateFileSystemEntries(string path);

        /// <inheritdoc cref="IDirectory.EnumerateFileSystemEntries(string,string)"/>
        public abstract IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);

        /// <inheritdoc cref="IDirectory.EnumerateFileSystemEntries(string,string,SearchOption)"/>
        public abstract IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc cref="IDirectory.EnumerateFileSystemEntries(string,string,EnumerationOptions)"/>
        public abstract IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions);
#endif
    }
}
