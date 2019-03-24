using System.Collections.Generic;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    public interface IDirectoryInfo: IFileSystemInfo
    {
        /// <inheritdoc cref="DirectoryInfo.Create()"/>
        void Create();
#if NET40
        /// <inheritdoc cref="DirectoryInfo.Create(DirectorySecurity)"/>
        void Create(DirectorySecurity directorySecurity);
#endif
        /// <inheritdoc cref="DirectoryInfo.CreateSubdirectory(string)"/>
        IDirectoryInfo CreateSubdirectory(string path);
#if NET40
        /// <inheritdoc cref="DirectoryInfo.CreateSubdirectory(string,DirectorySecurity)"/>
        IDirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity);
#endif
        /// <inheritdoc cref="DirectoryInfo.Delete(bool)"/>
        void Delete(bool recursive);
        /// <inheritdoc cref="DirectoryInfo.EnumerateDirectories()"/>
        IEnumerable<IDirectoryInfo> EnumerateDirectories();
        /// <inheritdoc cref="DirectoryInfo.EnumerateDirectories(string)"/>
        IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern);
        /// <inheritdoc cref="DirectoryInfo.EnumerateDirectories(string,SearchOption)"/>
        IEnumerable<IDirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="DirectoryInfo.EnumerateFiles()"/>
        IEnumerable<IFileInfo> EnumerateFiles();
        /// <inheritdoc cref="DirectoryInfo.EnumerateFiles(string)"/>
        IEnumerable<IFileInfo> EnumerateFiles(string searchPattern);
        /// <inheritdoc cref="DirectoryInfo.EnumerateFiles(string,SearchOption)"/>
        IEnumerable<IFileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="DirectoryInfo.EnumerateFileSystemInfos()"/>
        IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos();
        /// <inheritdoc cref="DirectoryInfo.EnumerateFileSystemInfos(string)"/>
        IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern);
        /// <inheritdoc cref="DirectoryInfo.EnumerateFileSystemInfos(string,SearchOption)"/>
        IEnumerable<IFileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="DirectoryInfo.GetAccessControl()"/>
        DirectorySecurity GetAccessControl();
        /// <inheritdoc cref="DirectoryInfo.GetAccessControl(AccessControlSections)"/>
        DirectorySecurity GetAccessControl(AccessControlSections includeSections);
        /// <inheritdoc cref="DirectoryInfo.GetDirectories()"/>
        IDirectoryInfo[] GetDirectories();
        /// <inheritdoc cref="DirectoryInfo.GetDirectories(string)"/>
        IDirectoryInfo[] GetDirectories(string searchPattern);
        /// <inheritdoc cref="DirectoryInfo.GetDirectories(string,SearchOption)"/>
        IDirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="DirectoryInfo.GetFiles(string)"/>
        IFileInfo[] GetFiles();
        /// <inheritdoc cref="DirectoryInfo.GetFiles(string)"/>
        IFileInfo[] GetFiles(string searchPattern);
        /// <inheritdoc cref="DirectoryInfo.GetFiles(string,SearchOption)"/>
        IFileInfo[] GetFiles(string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="DirectoryInfo.GetFileSystemInfos()"/>
        IFileSystemInfo[] GetFileSystemInfos();
        /// <inheritdoc cref="DirectoryInfo.GetFileSystemInfos(string)"/>
        IFileSystemInfo[] GetFileSystemInfos(string searchPattern);
        /// <inheritdoc cref="DirectoryInfo.GetFileSystemInfos(string,SearchOption)"/>
        IFileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption);
        /// <inheritdoc cref="DirectoryInfo.MoveTo"/>
        void MoveTo(string destDirName);
        /// <inheritdoc cref="DirectoryInfo.SetAccessControl"/>
        void SetAccessControl(DirectorySecurity directorySecurity);
        /// <inheritdoc cref="DirectoryInfo.Parent"/>
        IDirectoryInfo Parent { get; }
        /// <inheritdoc cref="DirectoryInfo.Root"/>
        IDirectoryInfo Root { get; }
    }
}