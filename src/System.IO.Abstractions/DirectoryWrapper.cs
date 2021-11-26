using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <inheritdoc />
    [Serializable]
    public class DirectoryWrapper : DirectoryBase
    {
        /// <inheritdoc />
        public DirectoryWrapper(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        /// <inheritdoc />
        public override IDirectoryInfo CreateDirectory(string path)
        {
            var directoryInfo = new DirectoryInfo(path);
            directoryInfo.Create();
            return new DirectoryInfoWrapper(FileSystem, directoryInfo);
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override IDirectoryInfo CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            var directoryInfo = new DirectoryInfo(path);
            directoryInfo.Create(directorySecurity);
            return new DirectoryInfoWrapper(FileSystem, directoryInfo);
        }

        /// <inheritdoc />
        public override void Delete(string path)
        {
            Directory.Delete(path);
        }

        /// <inheritdoc />
        public override void Delete(string path, bool recursive)
        {
            Directory.Delete(path, recursive);
        }

        /// <inheritdoc />
        public override bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override DirectorySecurity GetAccessControl(string path)
        {
            return new DirectoryInfo(path).GetAccessControl();
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return new DirectoryInfo(path).GetAccessControl(includeSections);
        }

        /// <inheritdoc />
        public override DateTime GetCreationTime(string path)
        {
            return Directory.GetCreationTime(path);
        }

        /// <inheritdoc />
        public override DateTime GetCreationTimeUtc(string path)
        {
            return Directory.GetCreationTimeUtc(path);
        }

        /// <inheritdoc />
        public override string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        /// <inheritdoc />
        public override string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        /// <inheritdoc />
        public override string[] GetDirectories(string path, string searchPattern)
        {
            return Directory.GetDirectories(path, searchPattern);
        }

        /// <inheritdoc />
        public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetDirectories(path, searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override string[] GetDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.GetDirectories(path, searchPattern, enumerationOptions);
        }
#endif

        /// <inheritdoc />
        public override string GetDirectoryRoot(string path)
        {
            return Directory.GetDirectoryRoot(path);
        }

        /// <inheritdoc />
        public override string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        /// <inheritdoc />
        public override string[] GetFiles(string path, string searchPattern)
        {
            return Directory.GetFiles(path, searchPattern);
        }

        /// <inheritdoc />
        public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.GetFiles(path, searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.GetFiles(path, searchPattern, enumerationOptions);
        }
#endif

        /// <inheritdoc />
        public override string[] GetFileSystemEntries(string path)
        {
            return Directory.GetFileSystemEntries(path);
        }

        /// <inheritdoc />
        public override string[] GetFileSystemEntries(string path, string searchPattern)
        {
            return Directory.GetFileSystemEntries(path, searchPattern);
        }

        /// <inheritdoc />
        public override DateTime GetLastAccessTime(string path)
        {
            return Directory.GetLastAccessTime(path);
        }

        /// <inheritdoc />
        public override DateTime GetLastAccessTimeUtc(string path)
        {
            return Directory.GetLastAccessTimeUtc(path);
        }

        /// <inheritdoc />
        public override DateTime GetLastWriteTime(string path)
        {
            return Directory.GetLastWriteTime(path);
        }

        /// <inheritdoc />
        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return Directory.GetLastWriteTimeUtc(path);
        }

        /// <inheritdoc />
        public override string[] GetLogicalDrives()
        {
            return Directory.GetLogicalDrives();
        }

        /// <inheritdoc />
        public override IDirectoryInfo GetParent(string path)
        {
            var parent = Directory.GetParent(path);

            if (parent == null)
            {
                return null;
            }

            return new DirectoryInfoWrapper(FileSystem, parent);
        }

        /// <inheritdoc />
        public override void Move(string sourceDirName, string destDirName)
        {
            Directory.Move(sourceDirName, destDirName);
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            new DirectoryInfo(path).SetAccessControl(directorySecurity);
        }

        /// <inheritdoc />
        public override void SetCreationTime(string path, DateTime creationTime)
        {
            Directory.SetCreationTime(path, creationTime);
        }

        /// <inheritdoc />
        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            Directory.SetCreationTimeUtc(path, creationTimeUtc);
        }

        /// <inheritdoc />
        public override void SetCurrentDirectory(string path)
        {
            Directory.SetCurrentDirectory(path);
        }

        /// <inheritdoc />
        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            Directory.SetLastAccessTime(path, lastAccessTime);
        }

        /// <inheritdoc />
        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            Directory.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
        }

        /// <inheritdoc />
        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            Directory.SetLastWriteTime(path, lastWriteTime);
        }

        /// <inheritdoc />
        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            Directory.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateDirectories(string path)
        {
            return Directory.EnumerateDirectories(path);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            return Directory.EnumerateDirectories(path, searchPattern);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateDirectories(path, searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.EnumerateDirectories(path, searchPattern, enumerationOptions);
        }
#endif

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFiles(string path)
        {
            return Directory.EnumerateFiles(path);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return Directory.EnumerateFiles(path, searchPattern);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFiles(path, searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.EnumerateFiles(path, searchPattern, enumerationOptions);
        }
#endif

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return Directory.EnumerateFileSystemEntries(path);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            return Directory.EnumerateFileSystemEntries(path, searchPattern);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            return Directory.EnumerateFileSystemEntries(path, searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return Directory.EnumerateFileSystemEntries(path, searchPattern, enumerationOptions);
        }
#endif
    }
}
