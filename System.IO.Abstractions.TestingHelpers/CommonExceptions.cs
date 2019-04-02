using System.Globalization;

namespace System.IO.Abstractions.TestingHelpers
{
    internal static class CommonExceptions
    {
        private const int _fileLockHResult = unchecked((int) 0x80070020);

        public static FileNotFoundException FileNotFound(string path) =>
            new FileNotFoundException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.Manager.GetString("COULD_NOT_FIND_FILE_EXCEPTION"),
                    path
                ),
                path
            );

        public static DirectoryNotFoundException CouldNotFindPartOfPath(string path) =>
            new DirectoryNotFoundException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"),
                    path
                )
            );

        public static UnauthorizedAccessException AccessDenied(string path) =>
            new UnauthorizedAccessException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.Manager.GetString("ACCESS_TO_THE_PATH_IS_DENIED"),
                    path
                )
            );

        public static Exception InvalidUseOfVolumeSeparator() =>
            new NotSupportedException(StringResources.Manager.GetString("THE_PATH_IS_NOT_OF_A_LEGAL_FORM"));

        public static Exception PathIsNotOfALegalForm(string paramName) => 
            new ArgumentException(
                StringResources.Manager.GetString("THE_PATH_IS_NOT_OF_A_LEGAL_FORM"), 
                paramName
            );

        public static ArgumentNullException FilenameCannotBeNull(string paramName) =>
            new ArgumentNullException(
                paramName,
                StringResources.Manager.GetString("FILENAME_CANNOT_BE_NULL")
            );

        public static ArgumentException IllegalCharactersInPath(string paramName = null) =>
            paramName != null 
                ? new ArgumentException(StringResources.Manager.GetString("ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION"), paramName)
                : new ArgumentException(StringResources.Manager.GetString("ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION"));

        public static Exception InvalidUncPath(string paramName) => 
            new ArgumentException(@"The UNC path should be of the form \\server\share.", paramName);

        public static IOException ProcessCannotAccessFileInUse(string paramName = null) =>
            paramName != null
            ? new IOException(string.Format(StringResources.Manager.GetString("PROCESS_CANNOT_ACCESS_FILE_IN_USE_WITH_FILENAME"), paramName), _fileLockHResult)
            : new IOException(StringResources.Manager.GetString("PROCESS_CANNOT_ACCESS_FILE_IN_USE"), _fileLockHResult);
    }
}