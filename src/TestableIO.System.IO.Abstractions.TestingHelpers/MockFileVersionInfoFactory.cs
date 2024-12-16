using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class MockFileVersionInfoFactory : IFileVersionInfoFactory
    {
        private static IMockFileDataAccessor mockFileSystem;

        /// <inheritdoc />
        public MockFileVersionInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            MockFileVersionInfoFactory.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
        }

        /// <inheritdoc />
        public IFileSystem FileSystem => mockFileSystem;

        IFileVersionInfo IFileVersionInfoFactory.GetVersionInfo(string fileName)
        {
            return GetVersionInfo(fileName);
        }

        /// <inheritdoc cref="Diagnostics.FileVersionInfo.GetVersionInfo(string)" />
        public static IFileVersionInfo GetVersionInfo(string fileName)
        {
            MockFileData mockFileData = mockFileSystem.GetFile(fileName);

            if (mockFileData != null)
            {
                return mockFileData.FileVersionInfo;
            }

            throw CommonExceptions.FileNotFound(fileName);
        }
    }
}
