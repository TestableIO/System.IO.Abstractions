using System;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    public static class XFS
    {
        public static string Path(string path, Func<bool> isUnixF = null)
        {
            if (isUnixF == null)
            {
                isUnixF = () =>
                {
                    int p = (int)Environment.OSVersion.Platform;
                    return (p == 4) || (p == 6) || (p == 128);
                };
            }
                
            if (isUnixF())
            {
                path = Regex.Replace(path, @"^[a-zA-Z]:(?<path>.*)$", "${path}");
                path = path.Replace(@"\", "/");
            }

            return path;
        }
    }
}
