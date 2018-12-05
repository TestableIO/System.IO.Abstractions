using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    using XFS = MockUnixSupport;

    public static class StringExtensions
    {
        [Pure]
        public static string[] SplitLines(this string input)
        {
            var list = new List<string>();
            using (var reader = new StringReader(input))
            {
                string str;
                while ((str = reader.ReadLine()) != null)
                {
                    list.Add(str);
                }
            }

            return list.ToArray();
        }

        [Pure]
        public static string Replace(this string source, string oldValue, string newValue, StringComparison comparisonType)
        {
            // from http://stackoverflow.com/a/22565605 with some adaptions
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentNullException(nameof(oldValue));
            }

            if (source.Length == 0)
            {
                return source;
            }

            if (newValue == null)
            {
                newValue = string.Empty;
            }

            var result = new StringBuilder();
            int startingPos = 0;
            int nextMatch;
            while ((nextMatch = source.IndexOf(oldValue, startingPos, comparisonType)) > -1)
            {
                result.Append(source, startingPos, nextMatch - startingPos);
                result.Append(newValue);
                startingPos = nextMatch + oldValue.Length;
            }

            result.Append(source, startingPos, source.Length - startingPos);

            return result.ToString();
        }

        [Pure]
        public static string TrimSlashes(this string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var trimmed = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (XFS.IsUnixPlatform()
                && (path[0] == Path.DirectorySeparatorChar || path[0] == Path.AltDirectorySeparatorChar)
                && trimmed == "")
            {
                return Path.DirectorySeparatorChar.ToString();
            }

            if (XFS.IsWindowsPlatform()
                && trimmed.Length == 2
                && char.IsLetter(trimmed[0])
                && trimmed[1] == ':')
            {
                return trimmed + Path.DirectorySeparatorChar;
            }

            return trimmed;
        }

        [Pure]
        public static string NormalizeSlashes(this string path)
        {
            path = path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            var sep = Path.DirectorySeparatorChar.ToString();
            var doubleSep = sep + sep;

            while (true)
            {
                var newPath = path.Replace(doubleSep, sep);

                if (path == newPath)
                {
                    return path;
                }

                path = newPath;
            }
        }
    }
}