using System.Globalization;

namespace System.IO.Abstractions.TestingHelpers
{
    internal static class CommonExceptions
    {
        private const int _fileLockHResult = unchecked((int)0x80070020);
        
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

        public static NotSupportedException InvalidUseOfVolumeSeparator() =>
            new NotSupportedException(StringResources.Manager.GetString("THE_PATH_IS_NOT_OF_A_LEGAL_FORM"));

        public static ArgumentException PathIsNotOfALegalForm(string paramName) =>
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

        public static ArgumentException InvalidUncPath(string paramName) =>
            new ArgumentException(@"The UNC path should be of the form \\server\share.", paramName);

        public static IOException ProcessCannotAccessFileInUse(string paramName = null) =>
            paramName != null
            ? new IOException(string.Format(StringResources.Manager.GetString("PROCESS_CANNOT_ACCESS_FILE_IN_USE_WITH_FILENAME"), paramName), _fileLockHResult)
            : new IOException(StringResources.Manager.GetString("PROCESS_CANNOT_ACCESS_FILE_IN_USE"), _fileLockHResult);

        public static IOException FileAlreadyExists(string paramName) =>
            new IOException(string.Format(StringResources.Manager.GetString("FILE_ALREADY_EXISTS"), paramName));

        public static ArgumentException InvalidAccessCombination(FileMode mode, FileAccess access)
            => new ArgumentException(string.Format(StringResources.Manager.GetString("INVALID_ACCESS_COMBINATION"), mode, access), nameof(access));

        public static ArgumentException AppendAccessOnlyInWriteOnlyMode()
            => new ArgumentException(string.Format(StringResources.Manager.GetString("APPEND_ACCESS_ONLY_IN_WRITE_ONLY_MODE")), "access");

        public static NotImplementedException NotImplemented() =>
            new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));

        public static IOException CannotCreateBecauseSameNameAlreadyExists(string path) =>
            new IOException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.Manager.GetString("CANNOT_CREATE_BECAUSE_SAME_NAME_ALREADY_EXISTS"),
                    path
                )
            );

        public static IOException NameCannotBeResolvedByTheSystem(string path) =>
            new IOException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.Manager.GetString("NAME_CANNOT_BE_RESOLVED_BY_THE_SYSTEM"),
                    path
                )
            );

        public static DirectoryNotFoundException PathDoesNotExistOrCouldNotBeFound(string path) =>
            new DirectoryNotFoundException(
                string.Format(
                    CultureInfo.InvariantCulture,
                    StringResources.Manager.GetString("PATH_DOES_NOT_EXIST_OR_COULD_NOT_BE_FOUND"),
                    path
                )
            );
    }
}