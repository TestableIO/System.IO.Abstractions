namespace System.IO.Abstractions
{
    using System.Collections.Generic;
    using System.IO.Abstractions.Extensions;
    using System.Security.AccessControl;

    public class UniqueTempDirectory : IUniqueTempDirectory
    {
        public IFileSystem FileSystem { get; }

        private IDirectory Inner { get; }

        public UniqueTempDirectory(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
            var temp = Environment.ExpandEnvironmentVariables("%TEMP%");
            var stamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
            var fullName = $"{temp}\\{stamp}";
            var directory = FileSystemExtensions.ParseDirectory(fileSystem, fullName);
            if (directory.Exists)
            {
                fullName = $"{temp}\\{Guid.NewGuid()}";
                directory = FileSystemExtensions.ParseDirectory(fileSystem, fullName);
            }

            directory.Create();

            Inner = directory;
        }

        public void Dispose()
        {
            Delete();
        }

        public FileAttributes Attributes
        {
            get => Inner.Attributes;
            set => Inner.Attributes = value;
        }

        public DateTime CreationTime
        {
            get => Inner.CreationTime;
            set => Inner.CreationTime = value;
        }

        public DateTime CreationTimeUtc
        {
            get => Inner.CreationTimeUtc;
            set => Inner.CreationTimeUtc = value;
        }

        public bool Exists => Inner.Exists;

        public string Extension => Inner.Extension;

        public string FullName => Inner.FullName;

        public DateTime LastAccessTime
        {
            get => Inner.LastAccessTime;
            set => Inner.LastAccessTime = value;
        }

        public DateTime LastAccessTimeUtc
        {
            get => Inner.LastAccessTimeUtc;
            set => Inner.LastAccessTimeUtc = value;
        }

        public DateTime LastWriteTime
        {
            get => Inner.LastWriteTime;
            set => Inner.LastWriteTime = value;
        }

        public DateTime LastWriteTimeUtc
        {
            get => Inner.LastWriteTimeUtc;
            set => Inner.LastWriteTimeUtc = value;
        }

        public string Name => Inner.Name;

        public void Delete()
        {
            Inner.Delete();
        }

        public void Refresh()
        {
            Inner.Refresh();
        }

        public IDirectory Parent => Inner.Parent;

        public IDirectory Root => Inner.Root;

        public void Create()
        {
            Inner.Create();
        }

        public void Create(DirectorySecurity directorySecurity)
        {
            Inner.Create(directorySecurity);
        }

        public IDirectory CreateSubdirectory(string path)
        {
            return Inner.CreateSubdirectory(path);
        }

        public IDirectory CreateSubdirectory(string path, DirectorySecurity directorySecurity)
        {
            return Inner.CreateSubdirectory(path, directorySecurity);
        }

        public void Delete(bool recursive)
        {
            Inner.Delete(recursive);
        }

        public IEnumerable<IDirectory> EnumerateDirectories()
        {
            return Inner.EnumerateDirectories();
        }

        public IEnumerable<IDirectory> EnumerateDirectories(string searchPattern)
        {
            return Inner.EnumerateDirectories(searchPattern);
        }

        public IEnumerable<IDirectory> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            return Inner.EnumerateDirectories(searchPattern, searchOption);
        }

        public IEnumerable<IFile> EnumerateFiles()
        {
            return Inner.EnumerateFiles();
        }

        public IEnumerable<IFile> EnumerateFiles(string searchPattern)
        {
            return Inner.EnumerateFiles(searchPattern);
        }

        public IEnumerable<IFile> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            return Inner.EnumerateFiles(searchPattern, searchOption);
        }

        public IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos()
        {
            return Inner.EnumerateFileSystemInfos();
        }

        public IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos(string searchPattern)
        {
            return Inner.EnumerateFileSystemInfos(searchPattern);
        }

        public IEnumerable<IFileSystemEntry> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return Inner.EnumerateFileSystemInfos(searchPattern, searchOption);
        }

        public DirectorySecurity GetAccessControl()
        {
            return Inner.GetAccessControl();
        }

        public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return Inner.GetAccessControl(includeSections);
        }

        public IDirectory[] GetDirectories()
        {
            return Inner.GetDirectories();
        }

        public IDirectory[] GetDirectories(string searchPattern)
        {
            return Inner.GetDirectories(searchPattern);
        }

        public IDirectory[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return Inner.GetDirectories(searchPattern, searchOption);
        }

        public IFile[] GetFiles()
        {
            return Inner.GetFiles();
        }

        public IFile[] GetFiles(string searchPattern)
        {
            return Inner.GetFiles(searchPattern);
        }

        public IFile[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return Inner.GetFiles(searchPattern, searchOption);
        }

        public IFileSystemEntry[] GetFileSystemInfos()
        {
            return Inner.GetFileSystemInfos();
        }

        public IFileSystemEntry[] GetFileSystemInfos(string searchPattern)
        {
            return Inner.GetFileSystemInfos(searchPattern);
        }

        public IFileSystemEntry[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return Inner.GetFileSystemInfos(searchPattern, searchOption);
        }

        public void MoveTo(string destDirName)
        {
            Inner.MoveTo(destDirName);
        }

        public void SetAccessControl(DirectorySecurity directorySecurity)
        {
            Inner.SetAccessControl(directorySecurity);
        }
    }
}