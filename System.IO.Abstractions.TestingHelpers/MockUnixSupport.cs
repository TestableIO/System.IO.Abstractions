using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    public static class MockUnixSupport
    {
        private static readonly Regex pathTransform = new Regex(@"^[a-zA-Z]:(?<path>.*)$");

        public static string Path(string path) => IsUnixPlatform()
            ? pathTransform.Replace(path, "${path}").Replace(@"\", "/")
            : path;

        public static bool IsUnixPlatform() => IO.Path.DirectorySeparatorChar == '/';
        public static bool IsWindowsPlatform() => IO.Path.DirectorySeparatorChar == '\\';
    }
}
