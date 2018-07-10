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

        private string ExtractFileName(string fullFileName)
        {
            return fullFileName.Split(_mockFileDataAccessor.Path.DirectorySeparatorChar).Last();
        }

        private string ExtractFilePath(string fullFileName)
        {
            var extractFilePath = fullFileName.Split(_mockFileDataAccessor.Path.DirectorySeparatorChar);
            return string.Join(_mockFileDataAccessor.Path.DirectorySeparatorChar.ToString(), extractFilePath.Take(extractFilePath.Length - 1));
        }
    }
}
