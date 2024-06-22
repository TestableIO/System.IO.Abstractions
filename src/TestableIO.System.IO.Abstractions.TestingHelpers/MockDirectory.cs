using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    using XFS = MockUnixSupport;



    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class MockDirectory : DirectoryBase
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;
        private string currentDirectory;

        /// <inheritdoc />
        public MockDirectory(IMockFileDataAccessor mockFileDataAccessor, FileBase fileBase, string currentDirectory) :
            this(mockFileDataAccessor, currentDirectory)
        {
        }

        /// <inheritdoc />
        public MockDirectory(IMockFileDataAccessor mockFileDataAccessor, string currentDirectory) : base(
            mockFileDataAccessor?.FileSystem)
        {
            this.currentDirectory = currentDirectory;
            this.mockFileDataAccessor =
                mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));
        }


        /// <inheritdoc />
        public override IDirectoryInfo CreateDirectory(string path)
        {
            return CreateDirectoryInternal(path);
        }

#if FEATURE_UNIX_FILE_MODE
        /// <inheritdoc />
        public override IDirectoryInfo CreateDirectory(string path, UnixFileMode unixCreateMode)
        {
            throw CommonExceptions.NotImplemented();
        }
#endif

        private IDirectoryInfo CreateDirectoryInternal(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(
                    StringResources.Manager.GetString("PATH_CANNOT_BE_THE_EMPTY_STRING_OR_ALL_WHITESPACE"), "path");
            }

            if (mockFileDataAccessor.PathVerifier.HasIllegalCharacters(path, true))
            {
                throw CommonExceptions.IllegalCharactersInPath(nameof(path));
            }

            path = mockFileDataAccessor.Path.GetFullPath(path).TrimSlashes();
            if (XFS.IsWindowsPlatform())
            {
                path = path.TrimEnd(' ');
            }

            var existingFile = mockFileDataAccessor.GetFile(path);
            if (existingFile == null)
            {
                mockFileDataAccessor.AddDirectory(path);
            }
            else if (!existingFile.IsDirectory)
            {
                throw CommonExceptions.FileAlreadyExists("path");
            }

            var created = new MockDirectoryInfo(mockFileDataAccessor, path);

            return created;
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override IFileSystemInfo CreateSymbolicLink(string path, string pathToTarget)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, nameof(path));
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(pathToTarget, nameof(pathToTarget));

            if (Exists(path))
            {
                throw CommonExceptions.FileAlreadyExists(nameof(path));
            }

            mockFileDataAccessor.AddDirectory(path);
            mockFileDataAccessor.GetFile(path).LinkTarget = pathToTarget;

            var directoryInfo = new MockDirectoryInfo(mockFileDataAccessor, path);
            directoryInfo.Attributes |= FileAttributes.ReparsePoint;
            return directoryInfo;
        }
#endif

#if FEATURE_CREATE_TEMP_SUBDIRECTORY
        /// <inheritdoc />
        public override IDirectoryInfo CreateTempSubdirectory(string prefix = null)
        {
            prefix ??= "";
            string potentialTempDirectory;

            // Perform directory name generation in a loop, just in case the randomly generated name already exists.
            do
            {
                var randomDir = $"{prefix}{FileSystem.Path.GetRandomFileName()}";
                potentialTempDirectory = Path.Combine(FileSystem.Path.GetTempPath(), randomDir);
            } while (Exists(potentialTempDirectory));

            return CreateDirectoryInternal(potentialTempDirectory);
        }
#endif

        /// <inheritdoc />
        public override void Delete(string path)
        {
            Delete(path, false);
        }


        /// <inheritdoc />
        public override void Delete(string path, bool recursive)
        {
            path = mockFileDataAccessor.Path.GetFullPath(path).TrimSlashes();

            var stringOps = mockFileDataAccessor.StringOperations;
            var pathWithDirectorySeparatorChar = $"{path}{Path.DirectorySeparatorChar}";

            var affectedPaths = mockFileDataAccessor
                .AllPaths
                .Where(p => stringOps.Equals(p, path) || stringOps.StartsWith(p, pathWithDirectorySeparatorChar))
                .ToList();

            if (!affectedPaths.Any())
            {
                throw CommonExceptions.PathDoesNotExistOrCouldNotBeFound(path);
            }

            if (!recursive && affectedPaths.Count > 1)
            {
                throw new IOException("The directory specified by " + path +
                                      " is read-only, or recursive is false and " + path +
                                      " is not an empty directory.");
            }

            foreach (var affectedPath in affectedPaths)
            {
                mockFileDataAccessor.RemoveFile(affectedPath);
            }
        }


        /// <inheritdoc />
        public override bool Exists(string path)
        {
            if (path == "/" && XFS.IsUnixPlatform())
            {
                return true;
            }

            try
            {
                path = path.TrimSlashes();
                path = mockFileDataAccessor.Path.GetFullPath(path);
                return mockFileDataAccessor.GetFile(path)?.IsDirectory ?? false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        /// <inheritdoc />
        public override DateTime GetCreationTime(string path)
        {
            return mockFileDataAccessor.File.GetCreationTime(path);
        }


        /// <inheritdoc />
        public override DateTime GetCreationTimeUtc(string path)
        {
            return mockFileDataAccessor.File.GetCreationTimeUtc(path);
        }

        /// <inheritdoc />
        public override string GetCurrentDirectory()
        {
            return currentDirectory;
        }

        /// <inheritdoc />
        public override string[] GetDirectories(string path)
        {
            return GetDirectories(path, "*");
        }

        /// <inheritdoc />
        public override string[] GetDirectories(string path, string searchPattern)
        {
            return GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <inheritdoc />
        public override string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            return EnumerateDirectories(path, searchPattern, searchOption).ToArray();
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override string[] GetDirectories(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return GetDirectories(path, "*", EnumerationOptionsToSearchOption(enumerationOptions));
        }
#endif

        /// <inheritdoc />
        public override string GetDirectoryRoot(string path)
        {
            return Path.GetPathRoot(path);
        }

        /// <inheritdoc />
        public override string[] GetFiles(string path)
        {
            // Same as what the real framework does
            return GetFiles(path, "*");
        }

        /// <inheritdoc />
        public override string[] GetFiles(string path, string searchPattern)
        {
            // Same as what the real framework does
            return GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <inheritdoc />
        public override string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return GetFilesInternal(mockFileDataAccessor.AllFiles, path, searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override string[] GetFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            return GetFiles(path, searchPattern, EnumerationOptionsToSearchOption(enumerationOptions));
        }
#endif

        private string[] GetFilesInternal(
            IEnumerable<string> files,
            string path,
            string searchPattern,
            SearchOption searchOption)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Any(c => Path.GetInvalidPathChars().Contains(c)))
            {
                throw new ArgumentException("Invalid character(s) in path", nameof(path));
            }

            CheckSearchPattern(searchPattern);
            if (searchPattern.Equals(string.Empty, StringComparison.OrdinalIgnoreCase))
            {
                searchPattern = "*";
            }

            path = path.TrimSlashes();
            path = path.NormalizeSlashes();
            path = mockFileDataAccessor.Path.GetFullPath(path);

            if (!Exists(path))
            {
                throw CommonExceptions.CouldNotFindPartOfPath(path);
            }

            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                path += Path.DirectorySeparatorChar;
            }

            var isUnix = XFS.IsUnixPlatform();

            var allDirectoriesPattern = isUnix
                ? @"([^<>:""/|?*]*/)*"
                : @"([^<>:""/\\|?*]*\\)*";

            var searchEndInStarDot = searchPattern.EndsWith(@"*.");

            string fileNamePattern;
            string pathPatternNoExtension = string.Empty;
            string pathPatternEndsInDot = string.Empty;
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
                bool hasExtensionLengthOfThree = extension != null && extension.Length == 4 &&
                                                 !extension.Contains("*") && !extension.Contains("?");
                if (hasExtensionLengthOfThree)
                {
                    var fileNamePatternSpecial =
                        string.Format(CultureInfo.InvariantCulture, "{0}[^.]", fileNamePattern);
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

            if (searchEndInStarDot)
            {
                pathPatternNoExtension = ReplaceLastOccurrence(pathPattern, @"]*?\.", @"\.]*?[.]*");
                pathPatternEndsInDot = ReplaceLastOccurrence(pathPattern, @"]*?\.", @"]*?[.]{1,}");
            }

            return files.Where(p =>
                    !searchEndInStarDot
                        ? (Regex.IsMatch(p, pathPattern) ||
                           (pathPatternSpecial != null && Regex.IsMatch(p, pathPatternSpecial)))
                        : (Regex.IsMatch(p, pathPatternNoExtension) || Regex.IsMatch(p, pathPatternEndsInDot))
                )
                .ToArray();
        }

        /// <inheritdoc />
        public override string[] GetFileSystemEntries(string path)
        {
            return GetFileSystemEntries(path, "*");
        }

        /// <inheritdoc />
        public override string[] GetFileSystemEntries(string path, string searchPattern)
        {
            var dirs = GetDirectories(path, searchPattern);
            var files = GetFiles(path, searchPattern);

            return dirs.Union(files).ToArray();
        }

        /// <inheritdoc />
        public override string[] GetFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            var dirs = GetDirectories(path, searchPattern, searchOption);
            var files = GetFiles(path, searchPattern, searchOption);

            return dirs.Union(files).ToArray();
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override string[] GetFileSystemEntries(string path, string searchPattern,
            EnumerationOptions enumerationOptions)
        {
            return GetFileSystemEntries(path, "*", EnumerationOptionsToSearchOption(enumerationOptions));
        }
#endif

        /// <inheritdoc />
        public override DateTime GetLastAccessTime(string path)
        {
            return mockFileDataAccessor.File.GetLastAccessTime(path);
        }

        /// <inheritdoc />
        public override DateTime GetLastAccessTimeUtc(string path)
        {
            return mockFileDataAccessor.File.GetLastAccessTimeUtc(path);
        }

        /// <inheritdoc />
        public override DateTime GetLastWriteTime(string path)
        {
            return mockFileDataAccessor.File.GetLastWriteTime(path);
        }

        /// <inheritdoc />
        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return mockFileDataAccessor.File.GetLastWriteTimeUtc(path);
        }

        /// <inheritdoc />
        public override string[] GetLogicalDrives()
        {
            return mockFileDataAccessor
                .AllDirectories
                .Select(d => new MockDirectoryInfo(mockFileDataAccessor, d).Root.FullName)
                .Select(r => mockFileDataAccessor.StringOperations.ToUpper(r))
                .Distinct()
                .ToArray();
        }

        /// <inheritdoc />
        public override IDirectoryInfo GetParent(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                throw new ArgumentException(
                    StringResources.Manager.GetString("PATH_CANNOT_BE_THE_EMPTY_STRING_OR_ALL_WHITESPACE"), "path");
            }

            if (mockFileDataAccessor.PathVerifier.HasIllegalCharacters(path, false))
            {
                throw new ArgumentException("Path contains invalid path characters.", "path");
            }

            var absolutePath = mockFileDataAccessor.Path.GetFullPath(path);
            var sepAsString = mockFileDataAccessor.Path.DirectorySeparatorChar.ToString();
            var lastIndex = 0;

            if (absolutePath != sepAsString)
            {
                var startIndex = mockFileDataAccessor.StringOperations.EndsWith(absolutePath, sepAsString)
                    ? absolutePath.Length - 1
                    : absolutePath.Length;
                lastIndex = absolutePath.LastIndexOf(mockFileDataAccessor.Path.DirectorySeparatorChar, startIndex - 1);

                if (lastIndex < 0)
                {
                    return null;
                }
            }

            var parentPath = absolutePath.Substring(0, lastIndex);

            if (string.IsNullOrEmpty(parentPath))
            {
                // On the Unix platform, the parent of a path consisting of a slash followed by
                // non-slashes is the root, '/'.
                if (XFS.IsUnixPlatform())
                {
                    absolutePath = absolutePath.TrimSlashes();

                    if (absolutePath.Length > 1 &&
                        absolutePath.LastIndexOf(mockFileDataAccessor.Path.DirectorySeparatorChar) == 0)
                    {
                        return new MockDirectoryInfo(mockFileDataAccessor,
                            mockFileDataAccessor.Path.DirectorySeparatorChar.ToString());
                    }
                }

                return null;
            }

            return new MockDirectoryInfo(mockFileDataAccessor, parentPath);
        }

        /// <inheritdoc />
        public override void Move(string sourceDirName, string destDirName)
        {
            var fullSourcePath = mockFileDataAccessor.Path.GetFullPath(sourceDirName).TrimSlashes();
            var fullDestPath = mockFileDataAccessor.Path.GetFullPath(destDirName).TrimSlashes();

            if (mockFileDataAccessor.StringOperations.Equals(fullSourcePath, fullDestPath))
            {
                throw new IOException("Source and destination path must be different.");
            }

            //if we're moving a file, not a directory, call the appropriate file moving function.
            var fileData = mockFileDataAccessor.GetFile(fullSourcePath);
            if (fileData?.Attributes.HasFlag(FileAttributes.Directory) == false)
            {
                mockFileDataAccessor.File.Move(fullSourcePath, fullDestPath);
                return;
            }

            var sourceRoot = mockFileDataAccessor.Path.GetPathRoot(fullSourcePath);
            var destinationRoot = mockFileDataAccessor.Path.GetPathRoot(fullDestPath);

            if (!mockFileDataAccessor.StringOperations.Equals(sourceRoot, destinationRoot))
            {
                throw new IOException(
                    "Source and destination path must have identical roots. Move will not work across volumes.");
            }

            if (!mockFileDataAccessor.Directory.Exists(fullSourcePath))
            {
                throw CommonExceptions.CouldNotFindPartOfPath(sourceDirName);
            }

            if (!mockFileDataAccessor.Directory.GetParent(fullDestPath).Exists)
            {
                throw CommonExceptions.CouldNotFindPartOfPath(destDirName);
            }

            if (mockFileDataAccessor.Directory.Exists(fullDestPath) || mockFileDataAccessor.File.Exists(fullDestPath))
            {
                throw CommonExceptions.CannotCreateBecauseSameNameAlreadyExists(fullDestPath);
            }

            mockFileDataAccessor.MoveDirectory(fullSourcePath, fullDestPath);
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override IFileSystemInfo ResolveLinkTarget(string linkPath, bool returnFinalTarget)
        {
            var initialContainer = mockFileDataAccessor.GetFile(linkPath);
            if (initialContainer.LinkTarget != null)
            {
                var nextLocation = initialContainer.LinkTarget;
                var nextContainer = mockFileDataAccessor.GetFile(nextLocation);

                if (returnFinalTarget)
                {
                    // The maximum number of symbolic links that are followed:
                    // https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.resolvelinktarget?view=net-6.0#remarks
                    int maxResolveLinks = XFS.IsWindowsPlatform() ? 63 : 40;
                    for (int i = 1; i < maxResolveLinks; i++)
                    {
                        if (nextContainer.LinkTarget == null)
                        {
                            break;
                        }
                        nextLocation = nextContainer.LinkTarget;
                        nextContainer = mockFileDataAccessor.GetFile(nextLocation);
                    }

                    if (nextContainer.LinkTarget != null)
                    {
                        throw CommonExceptions.NameCannotBeResolvedByTheSystem(linkPath);
                    }
                }

                if (nextContainer.IsDirectory)
                {
                    return new MockDirectoryInfo(mockFileDataAccessor, nextLocation);
                }
                else
                {
                    return new MockFileInfo(mockFileDataAccessor, nextLocation);
                }
            }
            throw CommonExceptions.NameCannotBeResolvedByTheSystem(linkPath);
        }
    
#endif

        /// <inheritdoc />
        public override void SetCreationTime(string path, DateTime creationTime)
        {
            mockFileDataAccessor.File.SetCreationTime(path, creationTime);
        }

        /// <inheritdoc />
        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            mockFileDataAccessor.File.SetCreationTimeUtc(path, creationTimeUtc);
        }

        /// <inheritdoc />
        public override void SetCurrentDirectory(string path)
        {
            currentDirectory = mockFileDataAccessor.Path.GetFullPath(path);
        }

        /// <inheritdoc />
        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            mockFileDataAccessor.File.SetLastAccessTime(path, lastAccessTime);
        }

        /// <inheritdoc />
        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            mockFileDataAccessor.File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
        }

        /// <inheritdoc />
        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            mockFileDataAccessor.File.SetLastWriteTime(path, lastWriteTime);
        }

        /// <inheritdoc />
        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            mockFileDataAccessor.File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateDirectories(string path)
        {
            return EnumerateDirectories(path, "*");
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
        {
            return EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            var originalPath = path;
            path = path.TrimSlashes();
            path = mockFileDataAccessor.Path.GetFullPath(path);
            return GetFilesInternal(mockFileDataAccessor.AllDirectories, path, searchPattern, searchOption)
                .Where(p => !mockFileDataAccessor.StringOperations.Equals(p, path))
                .Select(p => FixPrefix(p, originalPath));
        }
        
        private string FixPrefix(string path, string originalPath)
        {
            var normalizedOriginalPath = mockFileDataAccessor.Path.GetFullPath(originalPath);
            var pathWithoutOriginalPath = path.Substring(normalizedOriginalPath.Length)
                .TrimStart(mockFileDataAccessor.Path.DirectorySeparatorChar);
            return mockFileDataAccessor.Path.Combine(originalPath, pathWithoutOriginalPath);
        }
        
#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<string> EnumerateDirectories(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            var searchOption = enumerationOptions.RecurseSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return EnumerateDirectories(path, searchPattern, searchOption);
        }
#endif

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFiles(string path)
        {
            return GetFiles(path);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return GetFiles(path, searchPattern);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return GetFiles(path, searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            var searchOption = enumerationOptions.RecurseSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return GetFiles(path, searchPattern, searchOption);
        }
#endif

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFileSystemEntries(string path)
        {
            return GetFileSystemEntries(path);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern)
        {
            return GetFileSystemEntries(path, searchPattern);
        }

        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, SearchOption searchOption)
        {
            return GetFileSystemEntries(path, searchPattern, searchOption);
        }

#if FEATURE_ENUMERATION_OPTIONS
        /// <inheritdoc />
        public override IEnumerable<string> EnumerateFileSystemEntries(string path, string searchPattern, EnumerationOptions enumerationOptions)
        {
            var searchOption = enumerationOptions.RecurseSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            var fileSystemEntries = new List<string>(GetFiles(path, searchPattern, searchOption));
            fileSystemEntries.AddRange(GetDirectories(path, searchPattern, searchOption));
            return fileSystemEntries;
        }
#endif

        private string EnsureAbsolutePath(string path)
        {
            return Path.IsPathRooted(path)
                ? path
                : Path.Combine(GetCurrentDirectory(), path);
        }

        private void CheckSearchPattern(string searchPattern)
        {
            if (searchPattern == null)
            {
                throw new ArgumentNullException(nameof(searchPattern));
            }

            const string TWO_DOTS = "..";
            Func<ArgumentException> createException = () => new ArgumentException(@"Search pattern cannot contain "".."" to move up directories and can be contained only internally in file/directory names, as in ""a..b"".", searchPattern);

            if (mockFileDataAccessor.StringOperations.EndsWith(searchPattern, TWO_DOTS))
            {
                throw createException();
            }

            var position = mockFileDataAccessor.StringOperations.IndexOf(searchPattern, TWO_DOTS);

            if (position >= 0)
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
                throw CommonExceptions.IllegalCharactersInPath(nameof(searchPattern));
            }
        }

        private string ReplaceLastOccurrence(string source, string find, string replace)
        {
            if (source == null)
            {
                return source;
            }

            var place = source.LastIndexOf(find);

            if (place == -1)
            {
                return source;
            }

            var result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

#if FEATURE_ENUMERATION_OPTIONS
        private SearchOption EnumerationOptionsToSearchOption(EnumerationOptions enumerationOptions)
        {
            static Exception CreateExceptionForUnsupportedProperty(string propertyName)
            {
                return new NotSupportedException(
                    $"Changing EnumerationOptions.{propertyName} is not yet implemented for the mock file system."
                );
            }

            if (enumerationOptions.AttributesToSkip != (FileAttributes.System | FileAttributes.Hidden))
            {
                throw CreateExceptionForUnsupportedProperty("AttributesToSkip");
            }
            if (!enumerationOptions.IgnoreInaccessible)
            {
                throw CreateExceptionForUnsupportedProperty("IgnoreInaccessible");
            }
            if (enumerationOptions.MatchCasing != MatchCasing.PlatformDefault)
            {
                throw CreateExceptionForUnsupportedProperty("MatchCasing");
            }
            if (enumerationOptions.MatchType != MatchType.Simple)
            {
                throw CreateExceptionForUnsupportedProperty("MatchType");
            }
            if (enumerationOptions.ReturnSpecialDirectories)
            {
                throw CreateExceptionForUnsupportedProperty("ReturnSpecialDirectories");
            }

            return enumerationOptions.RecurseSubdirectories
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;
        }
#endif
    }
}
