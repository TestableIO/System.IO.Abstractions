
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers
{
    public class PathVerifier
    {
        private readonly IMockFileDataAccessor _mockFileDataAccessor;

        internal PathVerifier(IMockFileDataAccessor mockFileDataAccessor)
        {
            if (mockFileDataAccessor == null)
            {
                throw new ArgumentNullException("mockFileDataAccessor");
            }

            _mockFileDataAccessor = mockFileDataAccessor;
        }

        public void IsLegalAbsoluteOrRelative(string path, string paramName)
        {
            if (path == null)
            {
                throw new ArgumentNullException(paramName, "File name cannot be null.");
            }

            if (path == string.Empty)
            {
                throw new ArgumentException("Empty file name is not legal.", paramName);
            }

            if (path.Trim() == string.Empty)
            {
                throw new ArgumentException("The path is not of a legal form.", paramName);
            }

            if (ExtractFileName(path).IndexOfAny(_mockFileDataAccessor.Path.GetInvalidFileNameChars()) > -1)
            {
                throw new NotSupportedException("The given path's format is not supported.");
            }

            var filePath = ExtractFilePath(path);
            if (MockPath.HasIllegalCharacters(filePath, false))
            {
                throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
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
