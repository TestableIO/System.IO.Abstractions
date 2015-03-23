using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    ///     PathWrapper calls direct to Path but all this does is string manipulation so we can inherit directly from PathWrapper as no IO is done
    /// </summary>
    [Serializable]
    public class MockPath : PathWrapper
    {
        readonly IMockFileDataAccessor mockFileDataAccessor;

        public MockPath(IMockFileDataAccessor mockFileDataAccessor)
        {
            this.mockFileDataAccessor = mockFileDataAccessor;
        }

        public override string GetFullPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path", "Value cannot be null.");
            }

            if(path.Length == 0)
            {
                throw new ArgumentException("The path is not of a legal form.", "path");
            }

            path = path.Replace(AltDirectorySeparatorChar, DirectorySeparatorChar);

            bool isUnc =
                path.StartsWith(@"\\", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith(@"//", StringComparison.OrdinalIgnoreCase);

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
                    throw new ArgumentException(@"The UNC path should be of the form \\server\share.", "path");
                }
            }
            else if (@"\".Equals(root, StringComparison.OrdinalIgnoreCase) || @"/".Equals(root, StringComparison.OrdinalIgnoreCase))
            {
                // absolute path on the current drive or volume
                pathSegments = GetSegments(GetPathRoot(mockFileDataAccessor.Directory.GetCurrentDirectory()), path);
            }
                        else
            {
                pathSegments = GetSegments(path);
            }

            // unc paths need at least two segments, the others need one segment
            bool isUnixRooted =
                mockFileDataAccessor.Directory.GetCurrentDirectory()
                    .StartsWith(DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase);

            var minPathSegments = isUnc
                ? 2
                : isUnixRooted ? 0 : 1;

            var stack = new Stack<string>();
            foreach (var segment in pathSegments)
            {
                if ("..".Equals(segment, StringComparison.OrdinalIgnoreCase))
                {
                    // only pop, if afterwards are at least the minimal amount of path segments
                    if (stack.Count > minPathSegments)
                    {
                        stack.Pop();
                    }
                }
                else if (".".Equals(segment, StringComparison.OrdinalIgnoreCase))
                {
                    // ignore .
                }
                else
                {
                    stack.Push(segment);
                }
            }

            var fullPath = string.Join(DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), stack.Reverse().ToArray());

            if (hasTrailingSlash)
            {
                fullPath += DirectorySeparatorChar;
            }

            if (isUnixRooted && !isUnc)
            {
                fullPath = DirectorySeparatorChar + fullPath;
            }
            else if (isUnixRooted && isUnc)
            {
                fullPath = @"//" + fullPath;
            }
            else if (!isUnixRooted && isUnc)
            {
                fullPath = @"\\" + fullPath;
            }

            return fullPath;
        }

        private string[] GetSegments(params string[] paths)
        {
            return paths.SelectMany(path => path.Split(new[] { DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries)).ToArray();
        }

        public override string GetTempFileName()
        {
            string fileName = mockFileDataAccessor.Path.GetRandomFileName();
            string tempDir = mockFileDataAccessor.Path.GetTempPath();

            string fullPath = mockFileDataAccessor.Path.Combine(tempDir, fileName);

            mockFileDataAccessor.AddFile(fullPath, new MockFileData(String.Empty));

            return fullPath;
        }
    }
}
