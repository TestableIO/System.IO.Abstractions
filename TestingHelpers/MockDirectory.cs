using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDirectory : DirectoryBase
    {
        readonly FileBase fileBase;
        readonly IMockFileDataAccessor mockFileDataAccessor;

        private string currentDirectory;

        public MockDirectory(IMockFileDataAccessor mockFileDataAccessor, FileBase fileBase, string currentDirectory) 
        {
            this.currentDirectory = currentDirectory;
            this.mockFileDataAccessor = mockFileDataAccessor;
            this.fileBase = fileBase;
        }

        public override DirectoryInfoBase CreateDirectory(string path)
        {
            return CreateDirectory(path, null);
        }

        public override DirectoryInfoBase CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            path = EnsurePathEndsWithDirectorySeparator(mockFileDataAccessor.Path.GetFullPath(path));
            if (!Exists(path))
                mockFileDataAccessor.AddDirectory(path);
            var created = new MockDirectoryInfo(mockFileDataAccessor, path);

            var parent = GetParent(path);
            if (parent != null)
                CreateDirectory(GetParent(path).FullName, directorySecurity);

            return created;
        }

        public override void Delete(string path)
        {
            Delete(path, false);
        }

        public override void Delete(string path, bool recursive)
        {
            path = EnsurePathEndsWithDirectorySeparator(mockFileDataAccessor.Path.GetFullPath(path));
            var affectedPaths = mockFileDataAccessor
                .AllPaths
                .Where(p => p.StartsWith(path, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!affectedPaths.Any())
                throw new DirectoryNotFoundException(path + " does not exist or could not be found.");

            if (!recursive &&
                affectedPaths.Count() > 1)
                throw new IOException("The directory specified by " + path + " is read-only, or recursive is false and " + path + " is not an empty directory.");

            foreach (var affectedPath in affectedPaths)
                mockFileDataAccessor.RemoveFile(affectedPath);
        }

        public override bool Exists(string path)
        {
            path = EnsurePathEndsWithDirectorySeparator(path);

            path = mockFileDataAccessor.Path.GetFullPath(path);
            return mockFileDataAccessor.AllDirectories.Any(p => p.Equals(path, StringComparison.OrdinalIgnoreCase));
        }

        public override DirectorySecurity GetAccessControl(string path)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
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
            return currentDirectory;
        }

        public override string[] GetDirectories(string path)
        {
            return GetDirectories(path, "*");
        }

        public override string[] GetDirectories(string path, string searchPattern)
        {
            return GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            path = EnsurePathEndsWithDirectorySeparator(path);

            if (!Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path));
            }

            var dirs = GetFilesInternal(mockFileDataAccessor.AllDirectories, path, searchPattern, searchOption);
            return dirs.Where(p => p != path).ToArray();
        }

        public override string GetDirectoryRoot(string path)
        {
            return Path.GetPathRoot(path);
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
            if (!Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path));
            }

            return GetFilesInternal(mockFileDataAccessor.AllFiles, path, searchPattern, searchOption);
        }

        private string[] GetFilesInternal(IEnumerable<string> files, string path, string searchPattern, SearchOption searchOption)
        {
            path = EnsurePathEndsWithDirectorySeparator(path);

            path = mockFileDataAccessor.Path.GetFullPath(path);

            const string allDirectoriesPattern = @"([^<>:""/\\|?*]*\\)*";
            
            var fileNamePattern = searchPattern == "*"
                ? @"[^\\]*?\\?"
                : Regex.Escape(searchPattern)
                    .Replace(@"\*", @"[^<>:""/\\|?*]*?")
                    .Replace(@"\?", @"[^<>:""/\\|?*]?");

            var pathPattern = string.Format(
                CultureInfo.InvariantCulture,
                @"(?i:^{0}{1}{2}$)",
                Regex.Escape(path),
                searchOption == SearchOption.AllDirectories ? allDirectoriesPattern : string.Empty,
                fileNamePattern);

            return files
                .Where(p => Regex.IsMatch(p, pathPattern))
                .ToArray();
        }

        public override string[] GetFileSystemEntries(string path)
        {
            return GetFileSystemEntries(path, "*");
        }

        public override string[] GetFileSystemEntries(string path, string searchPattern)
        {
            var dirs = GetDirectories(path, searchPattern);
            var files = GetFiles(path, searchPattern);

            return dirs.Union(files).ToArray();
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
            return mockFileDataAccessor
                .AllDirectories
                .Select(d => new MockDirectoryInfo(mockFileDataAccessor, d).Root.FullName)
                .Select(r => r.ToLowerInvariant())
                .Distinct()
                .ToArray();
        }

        public override DirectoryInfoBase GetParent(string path)
        {
            var parent = new DirectoryInfo(path).Parent;
            if (parent == null)
                return null;

            return new MockDirectoryInfo(mockFileDataAccessor, parent.FullName);
        }

        public override void Move(string sourceDirName, string destDirName) {
            //Make sure that the destination exists
            mockFileDataAccessor.Directory.CreateDirectory(destDirName);

            //Recursively move all the subdirectories
            var subdirectories = GetDirectories(sourceDirName);
            foreach (var subdirectory in subdirectories)
            {
                var newSubdirPath = subdirectory.Replace(sourceDirName, destDirName);
                Move(subdirectory, newSubdirPath);
            }

            //Move the files in this directory
            var files = GetFiles(sourceDirName);
            foreach (var file in files)
            {
                var newFilePath = file.Replace(sourceDirName, destDirName);
                mockFileDataAccessor.FileInfo.FromFileName(file).MoveTo(newFilePath);
            }

            //Delete this directory
            Delete(sourceDirName);
        }

        public override void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
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
          currentDirectory = path;
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
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase))
                path += Path.DirectorySeparatorChar;
            return path;
        }
    }
}