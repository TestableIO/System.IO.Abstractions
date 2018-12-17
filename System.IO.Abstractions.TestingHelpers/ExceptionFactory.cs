using System.Globalization;

namespace System.IO.Abstractions.TestingHelpers
{
    internal static class CommonExceptions
    {
        public static Exception FileNotFound(string path) =>
            new FileNotFoundException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.Manager.GetString("COULD_NOT_FIND_FILE_EXCEPTION"),
                    path
                ),
                path
            );
    }
}