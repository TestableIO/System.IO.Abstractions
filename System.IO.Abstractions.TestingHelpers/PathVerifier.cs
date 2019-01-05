using System.Linq;

namespace System.IO.Abstractions.TestingHelpers
{
    using XFS = MockUnixSupport;

    public class PathVerifier
    {
        private static readonly char[] AdditionalInvalidPathChars = { '*', '?' };
        private readonly IMockFileDataAccessor _mockFileDataAccessor;

        internal PathVerifier(IMockFileDataAccessor mockFileDataAccessor)
        {
            _mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));
        }

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

        public bool HasIllegalCharacters(string path, bool checkAdditional)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            var invalidPathChars = _mockFileDataAccessor.Path.GetInvalidPathChars();

            if (checkAdditional)
            {
                return path.IndexOfAny(invalidPathChars.Concat(AdditionalInvalidPathChars).ToArray()) >= 0;
            }

            return path.IndexOfAny(invalidPathChars) >= 0;
        }

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
    }
}
