using System;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    public static class XFS
    {
        public static string ForWin(string path)
        {
            return path;
        }

        public static string ForUnix(this string path, string unixPath)
        {
            int p = (int) Environment.OSVersion.Platform;
            bool isUnix = (p == 4) || (p == 6) || (p == 128);

            return isUnix ? unixPath : path;
        }

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
