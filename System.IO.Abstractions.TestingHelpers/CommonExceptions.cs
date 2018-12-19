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

        public static Exception CouldNotFindPartOfPath(string path) =>
            new DirectoryNotFoundException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"),
                    path
                )
            );

        public static Exception AccessDenied(string path) =>
            new UnauthorizedAccessException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.Manager.GetString("ACCESS_TO_THE_PATH_IS_DENIED"),
                    path
                )
            );
    }
}