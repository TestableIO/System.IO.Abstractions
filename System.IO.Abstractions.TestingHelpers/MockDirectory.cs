using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    using XFS = MockUnixSupport;

    [Serializable]
    public class MockDirectory : DirectoryBase
    {
        private readonly FileBase fileBase;

        private readonly IMockFileDataAccessor mockFileDataAccessor;

        private string currentDirectory;

        public MockDirectory(IMockFileDataAccessor mockFileDataAccessor, FileBase fileBase, string currentDirectory) : base(mockFileDataAccessor?.FileSystem)
        {
            this.currentDirectory = currentDirectory;
            this.mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));
            this.fileBase = fileBase;
        }

        public override DirectoryInfoBase CreateDirectory(string path)
        {
            return CreateDirectoryInternal(path, null);
        }

#if NET40
        public override DirectoryInfoBase CreateDirectory(string path, DirectorySecurity directorySecurity)
        {
            return CreateDirectoryInternal(path, directorySecurity);
        }
#endif
        private DirectoryInfoBase CreateDirectoryInternal(string path, DirectorySecurity directorySecurity)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(StringResources.Manager.GetString("PATH_CANNOT_BE_THE_EMPTY_STRING_OR_ALL_WHITESPACE"), "path");
            }

            path = EnsurePathEndsWithDirectorySeparator(mockFileDataAccessor.Path.GetFullPath(path));

            if (!Exists(path))
            {
                mockFileDataAccessor.AddDirectory(path);
            }

            var created = new MockDirectoryInfo(mockFileDataAccessor, path);

            if (directorySecurity != null)
            {
                created.SetAccessControl(directorySecurity);
            }

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
                affectedPaths.Count > 1)
                throw new IOException("The directory specified by " + path + " is read-only, or recursive is false and " + path + " is not an empty directory.");

            foreach (var affectedPath in affectedPaths)
                mockFileDataAccessor.RemoveFile(affectedPath);
        }

        public override bool Exists(string path)
        {
            if (path == "/" && XFS.IsUnixPlatform()) 
            {
                return true;
            }

            try
            {

                path = EnsurePathEndsWithDirectorySeparator(path);
                path = mockFileDataAccessor.Path.GetFullPath(path);
                return mockFileDataAccessor.AllDirectories.Any(p => p.Equals(path, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override DirectorySecurity GetAccessControl(string path)
        {           
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            path = EnsurePathEndsWithDirectorySeparator(path);
            
            if (!mockFileDataAccessor.Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), path));
            }

            var directoryData = (MockDirectoryData) mockFileDataAccessor.GetFile(path);
            return directoryData.AccessControl;
        }

        public override DirectorySecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return GetAccessControl(path);
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
            return EnumerateDirectories(path, searchPattern, searchOption).ToArray();
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
            if(path == null)
                throw new ArgumentNullException();

            if (!Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), path));
            }

            return GetFilesInternal(mockFileDataAccessor.AllFiles, path, searchPattern, searchOption);
        }

        private string[] GetFilesInternal(IEnumerable<string> files, string path, string searchPattern, SearchOption searchOption)
        {
            CheckSearchPattern(searchPattern);
            path = EnsurePathEndsWithDirectorySeparator(path);
            path = EnsureAbsolutePath(path);

            bool isUnix = XFS.IsUnixPlatform();

            string allDirectoriesPattern = isUnix
                ? @"([^<>:""/|?*]*/)*"
                : @"([^<>:""/\\|?*]*\\)*";

            string fileNamePattern;
            string pathPatternSpecial = null;
            if (searchPattern == "*")
            {
                fileNamePattern = isUnix ? @"[^/]*?/?" : @"[^\\]*?\\?";
            }
            else
            {
                fileNamePattern = Regex.Escape(searchPattern)
                    .Replace(@"\*", isUnix ? @"[^<>:""/|?*]*?" : @"[^<>:""/\\|?*]*?")
                    .Replace(@"\?", isUnix ? @"[^<>:""/|?*]?" : @"[^<>:""/\\|?*]?");

                var extension = Path.GetExtension(searchPattern);
                bool hasExtensionLengthOfThree = extension != null && extension.Length == 4 && !extension.Contains("*") && !extension.Contains("?");
                if (hasExtensionLengthOfThree)
                {
                    var fileNamePatternSpecial = string.Format(CultureInfo.InvariantCulture, "{0}[^.]", fileNamePattern);
                    pathPatternSpecial = string.Format(
                        CultureInfo.InvariantCulture,
                        isUnix ? @"(?i:^{0}{1}{2}(?:/?)$)" : @"(?i:^{0}{1}{2}(?:\\?)$)",
                        Regex.Escape(path),
                        searchOption == SearchOption.AllDirectories ? allDirectoriesPattern : string.Empty,
                        fileNamePatternSpecial);
                }
            }

            var pathPattern = string.Format(
                CultureInfo.InvariantCulture,
                isUnix ? @"(?i:^{0}{1}{2}(?:/?)$)" : @"(?i:^{0}{1}{2}(?:\\?)$)",
                Regex.Escape(path),
                searchOption == SearchOption.AllDirectories ? allDirectoriesPattern : string.Empty,
                fileNamePattern);


            return files
                .Where(p =>
                    {
                        if (Regex.IsMatch(p, pathPattern))
                        {
                            return true;
                        }

                        if (pathPatternSpecial != null && Regex.IsMatch(p, pathPatternSpecial))
                        {
                            return true;
                        }

                        return false;
                    })
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

#if NET40
        public override string[] GetLogicalDrives()
        {
            return mockFileDataAccessor
                .AllDirectories
                .Select(d => new MockDirectoryInfo(mockFileDataAccessor, d).Root.FullName)
                .Select(r => r.ToLowerInvariant())
                .Distinct()
                .ToArray();
        }
#endif

        public override DirectoryInfoBase GetParent(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(StringResources.Manager.GetString("PATH_CANNOT_BE_THE_EMPTY_STRING_OR_ALL_WHITESPACE"), "path");
            }

            if (MockPath.HasIllegalCharacters(path, false))
            {
                throw new ArgumentException("Path contains invalid path characters.", "path");
            }

            var absolutePath = mockFileDataAccessor.Path.GetFullPath(path);
            var sepAsString = string.Format(CultureInfo.InvariantCulture, "{0}", mockFileDataAccessor.Path.DirectorySeparatorChar);

            var lastIndex = 0;
            if (absolutePath != sepAsString)
            {
                var startIndex = absolutePath.EndsWith(sepAsString, StringComparison.OrdinalIgnoreCase) ? absolutePath.Length - 1 : absolutePath.Length;
                lastIndex = absolutePath.LastIndexOf(mockFileDataAccessor.Path.DirectorySeparatorChar, startIndex - 1);
                if (lastIndex < 0)
                {
                    return null;
                }
            }

            var parentPath = absolutePath.Substring(0, lastIndex);
            if (string.IsNullOrEmpty(parentPath))
            {
                return null;
            }

            var parent = new MockDirectoryInfo(mockFileDataAccessor, parentPath);
            return parent;
        }

        public override void Move(string sourceDirName, string destDirName)
        {
            var fullSourcePath = EnsurePathEndsWithDirectorySeparator(mockFileDataAccessor.Path.GetFullPath(sourceDirName));
            var fullDestPath = EnsurePathEndsWithDirectorySeparator(mockFileDataAccessor.Path.GetFullPath(destDirName));

            if (string.Equals(fullSourcePath, fullDestPath, StringComparison.OrdinalIgnoreCase))
            {
                throw new IOException("Source and destination path must be different.");
            }

            var sourceRoot = mockFileDataAccessor.Path.GetPathRoot(fullSourcePath);
            var destinationRoot = mockFileDataAccessor.Path.GetPathRoot(fullDestPath);
            if (!string.Equals(sourceRoot, destinationRoot, StringComparison.OrdinalIgnoreCase))
            {
                throw new IOException("Source and destination path must have identical roots. Move will not work across volumes.");
            }

            if (!mockFileDataAccessor.Directory.Exists(fullSourcePath))
            {
                throw new DirectoryNotFoundException($"Could not find a part of the path '{sourceDirName}'.");
            }

            if (mockFileDataAccessor.Directory.Exists(fullDestPath) || mockFileDataAccessor.File.Exists(fullDestPath))
            {
                throw new IOException($"Cannot create '{fullDestPath}' because a file or directory with the same name already exists.");
            }

            mockFileDataAccessor.MoveDirectory(fullSourcePath, fullDestPath);
        }

        public override void SetAccessControl(string path, DirectorySecurity directorySecurity)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            path = EnsurePathEndsWithDirectorySeparator(path);

            if (!mockFileDataAccessor.Directory.Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), path));
            }

            var directoryData = (MockDirectoryData)mockFileDataAccessor.GetFile(path);
            directoryData.AccessControl = directorySecurity;
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

        public override IEnumerable<string> EnumerateDirectories(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return EnumerateDirectories(path, "*");
        }

        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            path = EnsurePathEndsWithDirectorySeparator(path);
            path = mockFileDataAccessor.Path.GetFullPath(path);

            if (!Exists(path))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), path));
            }

            var dirs = GetFilesInternal(mockFileDataAccessor.AllDirectories, path, searchPattern, searchOption);
            return dirs.Where(p => string.Compare(p, path, StringComparison.OrdinalIgnoreCase) != 0);
        }

        public override IEnumerable<string> EnumerateFiles(string path)
        {
            return GetFiles(path);
        }

        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return GetFiles(path, searchPattern);
        }

        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return GetFiles(path, searchPattern, searchOption);
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            var fileSystemEntries = new List<string>(GetFiles(path));
            fileSystemEntries.AddRange(GetDirectories(path));
            return fileSystemEntries;
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            var fileSystemEntries = new List<string>(GetFiles(path, searchPattern));
            fileSystemEntries.AddRange(GetDirectories(path, searchPattern));
            return fileSystemEntries;
        }

        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            var fileSystemEntries = new List<string>(GetFiles(path, searchPattern, searchOption));
            fileSystemEntries.AddRange(GetDirectories(path, searchPattern, searchOption));
            return fileSystemEntries;
        }

        static string EnsurePathEndsWithDirectorySeparator(string path)
        {
            if (!path.EndsWith(string.Format(CultureInfo.InvariantCulture, "{0}", Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase))
                path += Path.DirectorySeparatorChar;
            return path;
        }

        private string EnsureAbsolutePath(string path)
        {
            return Path.IsPathRooted(path)
                ? path
                : Path.Combine(GetCurrentDirectory(), path);
        }

        static void CheckSearchPattern(string searchPattern)
        {
            if (searchPattern == null)
            {
                throw new ArgumentNullException(nameof(searchPattern));
            }

            const string TWO_DOTS = "..";
            Func<ArgumentException> createException = () => new ArgumentException(@"Search pattern cannot contain "".."" to move up directories and can be contained only internally in file/directory names, as in ""a..b"".", searchPattern);

            if (searchPattern.EndsWith(TWO_DOTS, StringComparison.OrdinalIgnoreCase))
            {
                throw createException();
            }

            int position;
            if ((position = searchPattern.IndexOf(TWO_DOTS, StringComparison.OrdinalIgnoreCase)) >= 0)
            {
                var characterAfterTwoDots = searchPattern[position + 2];
                if (characterAfterTwoDots == Path.DirectorySeparatorChar || characterAfterTwoDots == Path.AltDirectorySeparatorChar)
                {
                    throw createException();
                }
            }

            var invalidPathChars = Path.GetInvalidPathChars();
            if (searchPattern.IndexOfAny(invalidPathChars) > -1)
            {
                throw new ArgumentException(StringResources.Manager.GetString("ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION"), "searchPattern");
            }
        }
    }
}
