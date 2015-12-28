using System;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace System.IO.Abstractions.TestingHelpers
{

    internal static class MockUnixSupport
    {
        internal static string Path(string path, Func<bool> isUnixF = null)
        {
            var isUnix = isUnixF ?? IsUnixPlatform;

            if (isUnix())
            {
                path = Regex.Replace(path, @"^[a-zA-Z]:(?<path>.*)$", "${path}");
                path = path.Replace(@"\", "/");
            }

            return path;
        }

        internal static string Separator(Func<bool> isUnixF = null)
        {
            var isUnix = isUnixF ?? IsUnixPlatform;
            return isUnix() ? "/" : @"\";
        }

        internal static bool IsUnixPlatform()
        {
#if NET40
            int p = (int)Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
#elif DOTNET5_4
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
        }
    }
}
