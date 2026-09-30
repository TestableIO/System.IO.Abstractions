using System.Globalization;
using System.Runtime.InteropServices;

namespace System.IO.Abstractions.TestingHelpers;

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
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new ArgumentException(@"The UNC path should be of the form \\server\share.", paramName)
            : new ArgumentException(@"The UNC path should be of the form //server/share.", paramName);

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

    public static NotSupportedException RandomAccessNotSupported() =>
        new NotSupportedException(StringResources.Manager.GetString("RANDOM_ACCESS_NOT_SUPPORTED_EXCEPTION"));

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

    public static ArgumentException PathIsEmpty(string paramName) =>
        new ArgumentException(StringResources.Manager.GetString("EMPTY_STRING_NOT_ALLOWED"), paramName);

    public static ArgumentOutOfRangeException EnumValueOutOfRange(string paramName) =>
        new ArgumentOutOfRangeException(paramName, StringResources.Manager.GetString("ENUM_VALUE_OUT_OF_RANGE"));

    public static ArgumentOutOfRangeException NonNegativeNumberRequired(string paramName) =>
        new ArgumentOutOfRangeException(paramName, StringResources.Manager.GetString("NON_NEGATIVE_NUMBER_REQUIRED"));

    public static ArgumentException PreallocationRequiresNewFile(FileMode mode) =>
        new ArgumentException(
            string.Format(CultureInfo.InvariantCulture,
                StringResources.Manager.GetString("PREALLOCATION_REQUIRES_NEW_FILE"), mode),
            "mode");

    public static ArgumentException PreallocationRequiresWriteAccess(FileAccess access) =>
        new ArgumentException(
            string.Format(CultureInfo.InvariantCulture,
                StringResources.Manager.GetString("PREALLOCATION_REQUIRES_WRITE_ACCESS"), access),
            "access");

    public static ArgumentException InvalidHandle(string paramName) =>
        new ArgumentException(StringResources.Manager.GetString("INVALID_HANDLE"), paramName);

    public static ObjectDisposedException HandleIsClosed() =>
        new ObjectDisposedException(null, StringResources.Manager.GetString("HANDLE_IS_CLOSED"));

    public static ArgumentException HandleNotFromThisFileSystem(string paramName) =>
        new ArgumentException(StringResources.Manager.GetString("HANDLE_NOT_FROM_THIS_FILE_SYSTEM"), paramName);

    public static IOException InvalidArgument(string path) =>
        new IOException(
            string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("INVALID_ARGUMENT"), path));

    public static IOException FileTooLarge(string path) =>
        new IOException(
            string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("FILE_TOO_LARGE"), path));
}