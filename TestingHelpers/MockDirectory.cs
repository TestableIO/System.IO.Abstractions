using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockDirectory : DirectoryBase
    {
        readonly FileBase fileBase;
        readonly IMockFileDataAccessor mockFileDataAccessor;

        public MockDirectory(IMockFileDataAccessor mockFileDataAccessor, FileBase fileBase)
        {
            this.mockFileDataAccessor = mockFileDataAccessor;
            this.fileBase = fileBase;
        }

        public override DirectoryInfoBase CreateDirectory(string path)
        {
            return CreateDirectory(path, null);
        }

        public override DirectoryInfoBase CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            path = EnsurePathEndsWithDirectorySeparator(path);
            mockFileDataAccessor.AddFile(path + "__PLACEHOLDER__.dir", new MockFileData(string.Empty));
            return new MockDirectoryInfo(mockFileDataAccessor, path);
        }

        public override void Delete(string path)
        {
            throw new NotImplementedException();
        }

        public override void Delete(string path, bool recursive)
        {
            throw new NotImplementedException();
        }

        public override bool Exists(string path)
        {
            path = EnsurePathEndsWithDirectorySeparator(path);
            return mockFileDataAccessor.AllPaths.Any(p => p.StartsWith(path));
        }

        public override DirectorySecurity GetAccessControl(string path)
        {
            throw new NotImplementedException();
        }

        public override DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetCreationTime(string path)
        {
            return fileBase.GetCreationTime(path);
        }

        public override DateTime GetCreationTimeUtc(string path)
        {
            return fileBase.GetCreationTimeUtc(path);
        }

        public override string GetCurrentDirectory()
        {
            throw new NotImplementedException();
        }

        public override string[] GetDirectories(string path)
        {
            throw new NotImplementedException();
        }

        public override string[] GetDirectories(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            throw new NotImplementedException();
        }

        public override string GetDirectoryRoot(string path)
        {
            throw new NotImplementedException();
        }

        public override string[] GetFiles(string path)
        {
            // Same as what the real framework does
            return GetFiles(path, "*");
        }

        public override string[] GetFiles(string path, string searchPattern)
        {
            // Same as what the real framework does
            return GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                path += Path.DirectorySeparatorChar;

            const string allDirectoriesPattern = @"([\w\d\s-\.]*\\)*";
            
            var fileNamePattern = searchPattern == "*"
                ? @"[\w\d\s-\.]*?\.[\w\d]+"
                : Regex.Escape(searchPattern).Replace(@"\*", @"[\w\d\s-\.]*?");

            var pathPattern = string.Format(
                @"(?i:^{0}{1}{2}$)",
                Regex.Escape(path),
                searchOption == SearchOption.AllDirectories ? allDirectoriesPattern : string.Empty,
                fileNamePattern);

            return mockFileDataAccessor
                .AllPaths
                .Where(p => Regex.IsMatch(p, pathPattern))
                .ToArray();
        }

        public override string[] GetFileSystemEntries(string path)
        {
            throw new NotImplementedException();
        }

        public override string[] GetFileSystemEntries(string path, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastAccessTime(string path)
        {
            return fileBase.GetLastAccessTime(path);
        }

        public override DateTime GetLastAccessTimeUtc(string path)
        {
            return fileBase.GetLastAccessTimeUtc(path);
        }

        public override DateTime GetLastWriteTime(string path)
        {
            return fileBase.GetLastWriteTime(path);
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return fileBase.GetLastWriteTimeUtc(path);
        }

        public override string[] GetLogicalDrives()
        {
            throw new NotImplementedException();
        }

        public override DirectoryInfoBase GetParent(string path)
        {
            throw new NotImplementedException();
        }

        public override void Move(string sourceDirName, string destDirName)
        {
            throw new NotImplementedException();
        }

        public override void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            throw new NotImplementedException();
        }

        public override void SetCreationTime(string path, DateTime creationTime)
        {
            fileBase.SetCreationTime(path, creationTime);
        }

        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            fileBase.SetCreationTimeUtc(path, creationTimeUtc);
        }

        public override void SetCurrentDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            fileBase.SetLastAccessTime(path, lastAccessTime);
        }

        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            fileBase.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
        }

        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            fileBase.SetLastWriteTime(path, lastWriteTime);
        }

        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            fileBase.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }

        static string EnsurePathEndsWithDirectorySeparator(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                path += Path.DirectorySeparatorChar;
            return path;
        }
    }
}