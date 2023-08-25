using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// PathWrapper calls direct to Path but all this does is string manipulation so we can inherit directly from PathWrapper as no IO is done
    /// </summary>
#if !NET8_0_OR_GREATER
    [Serializable]
#endif
    public class MockPath : PathWrapper
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;
        private readonly string defaultTempDirectory;

        /// <inheritdoc />
        public MockPath(IMockFileDataAccessor mockFileDataAccessor) : this(mockFileDataAccessor, string.Empty) { }

        /// <inheritdoc />
        public MockPath(IMockFileDataAccessor mockFileDataAccessor, string defaultTempDirectory) : base(mockFileDataAccessor?.FileSystem)
        {
            this.mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));
            this.defaultTempDirectory = !string.IsNullOrEmpty(defaultTempDirectory) ? defaultTempDirectory : base.GetTempPath();
        }

#if FEATURE_PATH_EXISTS
        /// <inheritdoc />
        public override bool Exists(string path)
        {
            return mockFileDataAccessor.FileExists(path);
        }
#endif

        /// <inheritdoc />
        public override string GetFullPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path), StringResources.Manager.GetString("VALUE_CANNOT_BE_NULL"));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw CommonExceptions.PathIsNotOfALegalForm(nameof(path));
            }

            path = path.Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);

            bool isUnc =
                mockFileDataAccessor.StringOperations.StartsWith(path, @"\\") ||
                mockFileDataAccessor.StringOperations.StartsWith(path, @"//");

            string root = GetPathRoot(path);

            bool hasTrailingSlash = path.Length > 1 && path[path.Length - 1] == DirectorySeparatorChar;

            string[] pathSegments;

            if (root.Length == 0)
            {
                // relative path on the current drive or volume
                path = mockFileDataAccessor.Directory.GetCurrentDirectory() + DirectorySeparatorChar + path;
                pathSegments = GetSegments(path);
            }
            else if (isUnc)
            {
                // unc path
                pathSegments = GetSegments(path);
                if (pathSegments.Length < 2)
                {
                    throw CommonExceptions.InvalidUncPath(nameof(path));
                }
            }
            else if (mockFileDataAccessor.StringOperations.Equals(@"\", root) ||
                     mockFileDataAccessor.StringOperations.Equals(@"/", root))
            {
                // absolute path on the current drive or volume
                pathSegments = GetSegments(GetPathRoot(mockFileDataAccessor.Directory.GetCurrentDirectory()), path);
            }
            else
            {
                pathSegments = GetSegments(path);
            }

            // unc paths need at least two segments, the others need one segment
            var isUnixRooted = mockFileDataAccessor.StringOperations.StartsWith(
                mockFileDataAccessor.Directory.GetCurrentDirectory(),
                "/");

            var minPathSegments = isUnc
                ? 2
                : isUnixRooted ? 0 : 1;

            var stack = new Stack<string>();
            foreach (var segment in pathSegments)
            {
                if (mockFileDataAccessor.StringOperations.Equals("..", segment))
                {
                    // only pop, if afterwards are at least the minimal amount of path segments
                    if (stack.Count > minPathSegments)
                    {
                        stack.Pop();
                    }
                }
                else if (mockFileDataAccessor.StringOperations.Equals(".", segment))
                {
                    // ignore .
                }
                else
                {
                    stack.Push(segment);
                }
            }

            var fullPath = string.Join(string.Format(CultureInfo.InvariantCulture, "{0}", DirectorySeparatorChar), stack.Reverse().ToArray());

            if (hasTrailingSlash)
            {
                fullPath += DirectorySeparatorChar;
            }

            if (isUnixRooted && !isUnc)
            {
                fullPath = "/" + fullPath;
            }
            else if (isUnixRooted)
            {
                fullPath = @"//" + fullPath;
            }
            else if (isUnc)
            {
                fullPath = @"\\" + fullPath;
            }

            return fullPath;
        }

        private string[] GetSegments(params string[] paths)
        {
            return paths.SelectMany(path => path.Split(new[] { DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
        }

        /// <inheritdoc />
        public override string GetTempFileName()
        {
            string fileName = mockFileDataAccessor.Path.GetRandomFileName();
            string tempDir = this.GetTempPath();

            string fullPath = mockFileDataAccessor.Path.Combine(tempDir, fileName);

            mockFileDataAccessor.AddFile(fullPath, new MockFileData(string.Empty));

            return fullPath;
        }

        /// <inheritdoc />
        public override string GetTempPath() => defaultTempDirectory;

#if FEATURE_ADVANCED_PATH_OPERATIONS
        /// <inheritdoc />
        public override string GetRelativePath(string relativeTo, string path)
        {
            if (relativeTo == null)
            {
                throw new ArgumentNullException(nameof(relativeTo), StringResources.Manager.GetString("VALUE_CANNOT_BE_NULL"));
            }

            if (relativeTo.Length == 0)
            {
                throw CommonExceptions.PathIsNotOfALegalForm(nameof(relativeTo));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path), StringResources.Manager.GetString("VALUE_CANNOT_BE_NULL"));
            }

            if (path.Length == 0)
            {
                throw CommonExceptions.PathIsNotOfALegalForm(nameof(path));
            }

            relativeTo = GetFullPath(relativeTo);
            path = GetFullPath(path);

            return Path.GetRelativePath(relativeTo, path);
        }
#endif
    }
}
