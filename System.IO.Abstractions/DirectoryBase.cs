using System.Collections.Generic;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="Directory"/>
    [Serializable]
    public abstract class DirectoryBase : IDirectory
    {
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

        /// <inheritdoc cref="Directory.CreateDirectory(string)"/>
        public abstract IDirectoryInfo CreateDirectory(string path);

#if NET40
        /// <inheritdoc cref="Directory.CreateDirectory(string,DirectorySecurity)"/>
        public abstract IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity);
#endif

        /// <inheritdoc cref="Directory.Delete(string)"/>
        public abstract void Delete(string path);

        /// <inheritdoc cref="Directory.Delete(string,bool)"/>
        public abstract void Delete(string path, bool recursive);

        /// <inheritdoc cref="Directory.Exists"/>
        public abstract bool Exists(string path);

        /// <inheritdoc cref="Directory.GetAccessControl(string)"/>
        public abstract DirectorySecurity GetAccessControl(string path);

        /// <inheritdoc cref="Directory.GetAccessControl(string,AccessControlSections)"/>
        public abstract DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections);

        /// <inheritdoc cref="Directory.GetCreationTime"/>
        public abstract DateTime GetCreationTime(string path);

        /// <inheritdoc cref="Directory.GetCreationTimeUtc"/>
        public abstract DateTime GetCreationTimeUtc(string path);

        /// <inheritdoc cref="Directory.GetCurrentDirectory"/>
        public abstract string GetCurrentDirectory();

        /// <inheritdoc cref="Directory.GetDirectories(string)"/>
        public abstract string[] GetDirectories(string path);

        /// <inheritdoc cref="Directory.GetDirectories(string,string)"/>
        public abstract string[] GetDirectories(string path, string searchPattern);

        /// <inheritdoc cref="Directory.GetDirectories(string,string,SearchOption)"/>
        public abstract string[] GetDirectories(string path, string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="Directory.GetDirectoryRoot"/>
        public abstract string GetDirectoryRoot(string path);

        /// <inheritdoc cref="Directory.GetFiles(string)"/>
        public abstract string[] GetFiles(string path);

        /// <inheritdoc cref="Directory.GetFiles(string,string)"/>
        public abstract string[] GetFiles(string path, string searchPattern);

        /// <inheritdoc cref="Directory.GetFiles(string,string,SearchOption)"/>
        public abstract string[] GetFiles(string path, string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="Directory.GetFileSystemEntries(string)"/>
        public abstract string[] GetFileSystemEntries(string path);

        /// <inheritdoc cref="Directory.GetFileSystemEntries(string,string)"/>
        public abstract string[] GetFileSystemEntries(string path, string searchPattern);

        /// <inheritdoc cref="Directory.GetLastAccessTime"/>
        public abstract DateTime GetLastAccessTime(string path);

        /// <inheritdoc cref="Directory.GetLastAccessTimeUtc"/>
        public abstract DateTime GetLastAccessTimeUtc(string path);

        /// <inheritdoc cref="Directory.GetLastWriteTime"/>
        public abstract DateTime GetLastWriteTime(string path);

        /// <inheritdoc cref="Directory.GetLastWriteTimeUtc"/>
        public abstract DateTime GetLastWriteTimeUtc(string path);

#if NET40
        /// <inheritdoc cref="Directory.GetLogicalDrives"/>
        public abstract string[] GetLogicalDrives();
#endif

        /// <inheritdoc cref="Directory.GetParent"/>
        public abstract IDirectoryInfo GetParent(string path);

        /// <inheritdoc cref="Directory.Move"/>
        public abstract void Move(string sourceDirName, string destDirName);

        /// <inheritdoc cref="Directory.SetAccessControl"/>
        public abstract void SetAccessControl(string path, DirectorySecurity directorySecurity);

        /// <inheritdoc cref="Directory.SetCreationTime"/>
        public abstract void SetCreationTime(string path, DateTime creationTime);

        /// <inheritdoc cref="Directory.SetCreationTimeUtc"/>
        public abstract void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <inheritdoc cref="Directory.SetCurrentDirectory"/>
        public abstract void SetCurrentDirectory(string path);

        /// <inheritdoc cref="Directory.SetLastAccessTime"/>
        public abstract void SetLastAccessTime(string path, DateTime lastAccessTime);

        /// <inheritdoc cref="Directory.SetLastAccessTimeUtc"/>
        public abstract void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <inheritdoc cref="Directory.SetLastWriteTime"/>
        public abstract void SetLastWriteTime(string path, DateTime lastWriteTime);

        /// <inheritdoc cref="Directory.SetLastWriteTimeUtc"/>
        public abstract void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <inheritdoc cref="Directory.EnumerateDirectories(string)"/>
        public abstract IEnumerable<string> EnumerateDirectories(string path);

        /// <inheritdoc cref="Directory.EnumerateDirectories(string,string)"/>
        public abstract IEnumerable<string> EnumerateDirectories(string path, string searchPattern);

        /// <inheritdoc cref="Directory.EnumerateDirectories(string,string,SearchOption)"/>
        public abstract IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="Directory.EnumerateFiles(string)"/>
        public abstract IEnumerable<string> EnumerateFiles(string path);

        /// <inheritdoc cref="Directory.EnumerateFiles(string,string)"/>
        public abstract IEnumerable<string> EnumerateFiles(string path, string searchPattern);

        /// <inheritdoc cref="Directory.EnumerateFiles(string,string,SearchOption)"/>
        public abstract IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);

        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string)"/>
        public abstract IEnumerable<string> EnumerateFileSystemEntries(string path);

        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string,string)"/>
        public abstract IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern);

        /// <inheritdoc cref="Directory.EnumerateFileSystemEntries(string,string,SearchOption)"/>
        public abstract IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption);
    }
}
