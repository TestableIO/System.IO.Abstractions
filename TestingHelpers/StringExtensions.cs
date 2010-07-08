using System.Collections.Generic;

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
    }
}