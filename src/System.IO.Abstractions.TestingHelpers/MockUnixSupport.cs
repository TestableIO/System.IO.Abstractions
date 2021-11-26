using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// </summary>
    public static class MockUnixSupport
    {
        private static readonly Regex pathTransform = new Regex(@"^[a-zA-Z]:(?<path>.*)$");

        /// <summary>
        /// </summary>
        public static string Path(string path) => path != null && IsUnixPlatform()
               ? pathTransform.Replace(path, "${path}").Replace(@"\", "/")
               : path;

        /// <summary>
        /// </summary>
        public static bool IsUnixPlatform() => IO.Path.DirectorySeparatorChar == '/';

        /// <summary>
        /// </summary>
        public static bool IsWindowsPlatform() => IO.Path.DirectorySeparatorChar == '\\';
    }
}
