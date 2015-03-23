using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDirectoryInfo : DirectoryInfoBase
    {
        readonly IMockFileDataAccessor mockFileDataAccessor;
        readonly string directoryPath;

        private static string EnsurePathEndsWithDirectorySeparator(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                path += Path.DirectorySeparatorChar;
            return path;
        }

        public MockDirectoryInfo(IMockFileDataAccessor mockFileDataAccessor, string directoryPath)
        {
            this.mockFileDataAccessor = mockFileDataAccessor;
            this.directoryPath = EnsurePathEndsWithDirectorySeparator(directoryPath);
        }

        MockFileData MockFileData
        {
            get { return mockFileDataAccessor.GetFile(directoryPath); }
        }

        public override void Delete()
        {
            mockFileDataAccessor.Directory.Delete(directoryPath);
        }

        public override void Refresh()
        {
        }

        public override FileAttributes Attributes
        {
            get { return MockFileData.Attributes; }
            set { MockFileData.Attributes = value; }
        }

        public override DateTime CreationTime
        {
            get { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
            set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
        }

        public override DateTime CreationTimeUtc
        {
            get { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
            set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
        }

        public override bool Exists
        {
            get { return mockFileDataAccessor.Directory.Exists(FullName); }
        }

        public override string Extension
        {
            get
            {
                // System.IO.Path.GetExtension does only string manipulation,
                // so it's safe to delegate.
                return Path.GetExtension(this.directoryPath);
            }
        }

        public override string FullName
        {
            get
            {
                var root = mockFileDataAccessor.Path.GetPathRoot(directoryPath);
                if (string.Equals(directoryPath, root, StringComparison.OrdinalIgnoreCase))
                {
                    // drives have the trailing slash
                    return directoryPath;
                }

                // directories do not have a trailing slash
                return directoryPath.TrimEnd('\\').TrimEnd('/');
            }
        }

        public override DateTime LastAccessTime
        {
            get { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
            set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
        }

        public override DateTime LastAccessTimeUtc
        {
            get {
                if (MockFileData == null) throw new FileNotFoundException("File not found", directoryPath);
                return MockFileData.LastAccessTime.UtcDateTime;
            }
            set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
        }

        public override DateTime LastWriteTime
        {
            get { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
            set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
        }

        public override DateTime LastWriteTimeUtc
        {
            get { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
            set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
        }

        public override string Name
        {
            get { return new MockPath(mockFileDataAccessor).GetFileName(directoryPath.TrimEnd('\\')); }
        }

        public override void Create()
        {
            mockFileDataAccessor.Directory.CreateDirectory(FullName);
        }

        public override void Create(DirectorySecurity directorySecurity)
        {
            mockFileDataAccessor.Directory.CreateDirectory(FullName, directorySecurity);
        }

        public override DirectoryInfoBase CreateSubdirectory(string path)
        {
            return mockFileDataAccessor.Directory.CreateDirectory(Path.Combine(FullName, path));
        }

        public override DirectoryInfoBase CreateSubdirectory(string path, DirectorySecurity directorySecurity)
        {
            return mockFileDataAccessor.Directory.CreateDirectory(Path.Combine(FullName, path), directorySecurity);
        }

        public override void Delete(bool recursive)
        {
            mockFileDataAccessor.Directory.Delete(directoryPath, recursive);
        }

        public override IEnumerable<DirectoryInfoBase> EnumerateDirectories()
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override IEnumerable<DirectoryInfoBase> EnumerateDirectories(string searchPattern)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override IEnumerable<DirectoryInfoBase> EnumerateDirectories(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override IEnumerable<FileInfoBase> EnumerateFiles()
        {
            return mockFileDataAccessor.AllFiles.Select(x => new MockFileInfo(mockFileDataAccessor, x));
        }

        public override IEnumerable<FileInfoBase> EnumerateFiles(string searchPattern)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override IEnumerable<FileInfoBase> EnumerateFiles(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos()
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos(string searchPattern)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override IEnumerable<FileSystemInfoBase> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override DirectorySecurity GetAccessControl()
        {
            return mockFileDataAccessor.Directory.GetAccessControl(directoryPath);
        }

        public override DirectorySecurity GetAccessControl(AccessControlSections includeSections)
        {
            return mockFileDataAccessor.Directory.GetAccessControl(directoryPath, includeSections);
        }

        public override DirectoryInfoBase[] GetDirectories()
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath));
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern)
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath, searchPattern));
        }

        public override DirectoryInfoBase[] GetDirectories(string searchPattern, SearchOption searchOption)
        {
            return ConvertStringsToDirectories(mockFileDataAccessor.Directory.GetDirectories(directoryPath, searchPattern, searchOption));
        }

        private DirectoryInfoBase[] ConvertStringsToDirectories(IEnumerable<string> paths)
        {
            return paths
                .Select(path => new MockDirectoryInfo(mockFileDataAccessor, path))
                .Cast<DirectoryInfoBase>()
                .ToArray();
        }

        public override FileInfoBase[] GetFiles()
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName));
        }

        public override FileInfoBase[] GetFiles(string searchPattern)
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName, searchPattern));
        }

        public override FileInfoBase[] GetFiles(string searchPattern, SearchOption searchOption)
        {
            return ConvertStringsToFiles(mockFileDataAccessor.Directory.GetFiles(FullName, searchPattern, searchOption));
        }

        FileInfoBase[] ConvertStringsToFiles(IEnumerable<string> paths)
        {
            return paths
                  .Select(mockFileDataAccessor.FileInfo.FromFileName)
                  .ToArray();
        }

        public override FileSystemInfoBase[] GetFileSystemInfos()
        {
            return GetFileSystemInfos("*");
        }

        public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern)
        {
            return GetFileSystemInfos(searchPattern, SearchOption.TopDirectoryOnly);
        }

        public override FileSystemInfoBase[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
        {
            return GetDirectories(searchPattern, searchOption).OfType<FileSystemInfoBase>().Concat(this.GetFiles(searchPattern, searchOption)).ToArray();
        }

        public override void MoveTo(string destDirName)
        {
            mockFileDataAccessor.Directory.Move(directoryPath, destDirName);
        }

        public override void SetAccessControl(DirectorySecurity directorySecurity)
        {
            mockFileDataAccessor.Directory.SetAccessControl(directoryPath, directorySecurity);
        }

        public override DirectoryInfoBase Parent
        {
            get
            {
                return mockFileDataAccessor.Directory.GetParent(directoryPath);
            }
        }

        public override DirectoryInfoBase Root
        {
            get
            {
                return new MockDirectoryInfo(mockFileDataAccessor, mockFileDataAccessor.Directory.GetDirectoryRoot(FullName));
            }
        }
    }
}
