using System;
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers;

using XFS = MockUnixSupport;

/// <summary>
/// Provides helper methods for verifying paths.
/// </summary>
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
public class PathVerifier
{
    private static readonly char[] AdditionalInvalidPathChars = { '*', '?' };
    private readonly IMockFileDataAccessor _mockFileDataAccessor;

    // Windows supports extended-length paths with a `\\?\` prefix, to work around low path length limits.
    // Ref: https://learn.microsoft.com/en-us/windows/win32/fileio/maximum-file-path-limitation?tabs=registry
    private const string WINDOWS_EXTENDED_LENGTH_PATH_PREFIX = @"\\?\";

    /// <summary>
    /// Creates a new verifier instance.
    /// </summary>
    public PathVerifier(IMockFileDataAccessor mockFileDataAccessor)
    {
        _mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));
    }

    /// <summary>
    /// Determines whether the given path is legal.
    /// </summary>
    public void IsLegalAbsoluteOrRelative(string path, string paramName)
    {
        if (path == null)
        {
            throw new ArgumentNullException(paramName, StringResources.Manager.GetString("VALUE_CANNOT_BE_NULL"));
        }

        if (path == string.Empty)
        {
            throw new ArgumentException("Empty file name is not legal.", paramName);
        }

        if (path.Trim() == string.Empty)
        {
            throw CommonExceptions.PathIsNotOfALegalForm(paramName);
        }

        if (XFS.IsWindowsPlatform() && !IsValidUseOfVolumeSeparatorChar(path))
        {

            throw CommonExceptions.InvalidUseOfVolumeSeparator();
        }

        if (ExtractFileName(path).IndexOfAny(_mockFileDataAccessor.Path.GetInvalidFileNameChars()) > -1)
        {
            throw CommonExceptions.IllegalCharactersInPath();
        }

        var filePath = ExtractFilePath(path);

        if (HasIllegalCharacters(filePath, checkAdditional: false))
        {
            throw CommonExceptions.IllegalCharactersInPath();
        }
    }

    private static bool IsValidUseOfVolumeSeparatorChar(string path)
    {
        if (XFS.IsWindowsPlatform() && path.StartsWith(WINDOWS_EXTENDED_LENGTH_PATH_PREFIX))
        {
            // Skip over the `\\?\` prefix if there is one.
            path = path.Substring(WINDOWS_EXTENDED_LENGTH_PATH_PREFIX.Length);
        }

        var lastVolSepIndex = path.LastIndexOf(Path.VolumeSeparatorChar);
        return lastVolSepIndex == -1 || lastVolSepIndex == 1 && char.IsLetter(path[0]);
    }

    private string ExtractFileName(string fullFileName)
    {
        return fullFileName.Split(
            _mockFileDataAccessor.Path.DirectorySeparatorChar,
            _mockFileDataAccessor.Path.AltDirectorySeparatorChar).Last();
    }

    private string ExtractFilePath(string fullFileName)
    {
        var extractFilePath = fullFileName.Split(
            _mockFileDataAccessor.Path.DirectorySeparatorChar,
            _mockFileDataAccessor.Path.AltDirectorySeparatorChar);
        return string.Join(_mockFileDataAccessor.Path.DirectorySeparatorChar.ToString(), extractFilePath.Take(extractFilePath.Length - 1));
    }

    /// <summary>
    /// Determines whether the given path contains illegal characters.
    /// </summary>
    public bool HasIllegalCharacters(string path, bool checkAdditional)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        var invalidPathChars = _mockFileDataAccessor.Path.GetInvalidPathChars();

        if (checkAdditional)
        {
            // AdditionalInvalidPathChars includes '?', but this character is allowed in extended-length
            // windows path prefixes (`\\?\`). If we're dealing with such a path, check for invalid
            // characters after the prefix.
            if (XFS.IsWindowsPlatform() && path.StartsWith(WINDOWS_EXTENDED_LENGTH_PATH_PREFIX))
            {
                path = path.Substring(WINDOWS_EXTENDED_LENGTH_PATH_PREFIX.Length);
            }

            return path.IndexOfAny(invalidPathChars.Concat(AdditionalInvalidPathChars).ToArray()) >= 0;
        }

        return path.IndexOfAny(invalidPathChars) >= 0;
    }

    /// <summary>
    /// Throws an excpetion if the given path contains invalid characters.
    /// </summary>
    public void CheckInvalidPathChars(string path, bool checkAdditional = false)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path));
        }

        if (HasIllegalCharacters(path, checkAdditional))
        {
            throw CommonExceptions.IllegalCharactersInPath();
        }
    }

    /// <summary>
    /// Determines the normalized drive name used for drive identification.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="name"/> is not a valid drive name.</exception>
    public string NormalizeDriveName(string name)
    {
        return TryNormalizeDriveName(name, out var result)
            ? result
            : throw new ArgumentException(
                @"Object must be a root directory (""C:\"") or a drive letter (""C"").");
    }

    /// <summary>
    /// Tries to determine the normalized drive name used for drive identification.
    /// </summary>
    public bool TryNormalizeDriveName(string name, out string result)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        const string DRIVE_SEPARATOR = @":\";

        if (name.Length == 1
            || (name.Length == 2 && name[1] == ':')
            || (name.Length == 3 && _mockFileDataAccessor.StringOperations.EndsWith(name, DRIVE_SEPARATOR)))
        {
            name = name[0] + DRIVE_SEPARATOR;
        }
        else
        {
            CheckInvalidPathChars(name);
            name = _mockFileDataAccessor.Path.GetPathRoot(name);

            if (string.IsNullOrEmpty(name) || _mockFileDataAccessor.StringOperations.StartsWith(name, @"\\"))
            {
                result = null;
                return false;
            }
        }

        result = name;
        return true;
    }

    /// <summary>
    /// Resolves and normalizes a path.
    /// </summary>
    public string FixPath(string path)
    {
        if (path == null)
        {
            throw new ArgumentNullException(nameof(path), StringResources.Manager.GetString("VALUE_CANNOT_BE_NULL"));
        }

        var pathSeparatorFixed = path.Replace(
            _mockFileDataAccessor.Path.AltDirectorySeparatorChar,
            _mockFileDataAccessor.Path.DirectorySeparatorChar
        );
        var fullPath = _mockFileDataAccessor.Path.GetFullPath(pathSeparatorFixed);

        return fullPath;
    }
}
