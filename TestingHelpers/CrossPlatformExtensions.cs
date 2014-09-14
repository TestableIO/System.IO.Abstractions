using System;

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
    }
}
