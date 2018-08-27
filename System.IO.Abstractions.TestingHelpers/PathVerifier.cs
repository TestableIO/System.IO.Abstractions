using System.Linq;

namespace System.IO.Abstractions.TestingHelpers
{
    public class PathVerifier
    {
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
                throw new ArgumentException(StringResources.Manager.GetString("THE_PATH_IS_NOT_OF_A_LEGAL_FORM"), paramName);
            }

            if (!MockUnixSupport.IsUnixPlatform())
            {
                if (!IsValidUseOfVolumeSeparatorChar(path))
                {
                    throw new NotSupportedException(StringResources.Manager.GetString("THE_PATH_IS_NOT_OF_A_LEGAL_FORM"));
                }
            }

            if (ExtractFileName(path).IndexOfAny(_mockFileDataAccessor.Path.GetInvalidFileNameChars()) > -1)
            {
                throw new ArgumentException(StringResources.Manager.GetString("ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION"));
            }

            var filePath = ExtractFilePath(path);
            if (MockPath.HasIllegalCharacters(filePath, false))
            {
                throw new ArgumentException(StringResources.Manager.GetString("ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION"));
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
    }
}
