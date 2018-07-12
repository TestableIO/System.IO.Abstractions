using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
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
        public static string CleanPath(this string path)
        {
            //TODO: remove redundant slashes, but preserve Unix root slash and unc double slash
            if (MockUnixSupport.IsUnixPlatform() && path.StartsWith("/"))
            {
                return "/" + path.Substring(1).TrimEnd(Path.DirectorySeparatorChar);
            }

            if (!MockUnixSupport.IsUnixPlatform())
            {
                if (path.StartsWith("\\\\"))
                {
                    return "\\\\" + path.Substring(2).TrimEnd(Path.DirectorySeparatorChar);
                }

                var trimmed = path.TrimEnd(Path.DirectorySeparatorChar);

                if (Regex.IsMatch(trimmed, "^[a-zA-Z]:$"))
                {
                    return trimmed + Path.DirectorySeparatorChar;
                }
            }

            return path.TrimEnd(Path.DirectorySeparatorChar);
        }
    }
}