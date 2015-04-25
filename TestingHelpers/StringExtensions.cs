using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    internal static class StringExtensions
    {
        internal static string[] SplitLines(this string input)
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

        // from http://www.codeproject.com/Articles/11556/Converting-Wildcards-to-Regexes
        // with some adaptions to match the rules on
        // https://msdn.microsoft.com/en-us/library/ms143327%28v=vs.110%29.aspx
        internal static string WildcardToRegex(this string pattern)
        {
            var appendExtensionWildcard = false;

            if (pattern.Contains("*"))
            {
                // bla.txt => .txt
                var extension = Path.GetExtension(pattern);
                if (extension.Length == 4)
                {
                    appendExtensionWildcard = true;
                }
            }

            return "^" + Regex.Escape(pattern)
                              .Replace(@"\*", ".*")
                              .Replace(@"\?", ".")
                       + (appendExtensionWildcard ? ".*" : "") 
                       + "$";
        }

        [Pure]
        public static string Replace(this string source, string oldValue, string newValue, StringComparison comparisonType)
        {
            // from http://stackoverflow.com/a/22565605 with some adaptions
            if (string.IsNullOrEmpty(oldValue))
            {
                throw new ArgumentNullException("oldValue");
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
    }
}