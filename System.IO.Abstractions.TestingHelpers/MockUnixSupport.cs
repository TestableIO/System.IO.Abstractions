using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    public static class MockUnixSupport
    {
        public static string Path(string path, Func<bool> isUnixF = null)
        {
            var isUnix = isUnixF ?? IsUnixPlatform;

            if (isUnix())
            {
                path = Regex.Replace(path, @"^[a-zA-Z]:(?<path>.*)$", "${path}");
                path = path.Replace(@"\", "/");
            }

            return path;
        }

        public static string Separator(Func<bool> isUnixF = null)
        {
            var isUnix = isUnixF ?? IsUnixPlatform;
            return isUnix() ? "/" : @"\";
        }

        public static bool IsUnixPlatform()
        {
            return IO.Path.DirectorySeparatorChar == '/';
        }
    }
}
