namespace System.IO.Abstractions.TestingHelpers;

/// <inheritdoc />
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
public class MockFileVersionInfoFactory : IFileVersionInfoFactory
{
    private readonly IMockFileDataAccessor mockFileSystem;

    /// <inheritdoc />
    public MockFileVersionInfoFactory(IMockFileDataAccessor mockFileSystem)
    {
        this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
    }

    /// <inheritdoc />
    public IFileSystem FileSystem => mockFileSystem;

    /// <inheritdoc />
    public IFileVersionInfo GetVersionInfo(string fileName)
    {
        MockFileData mockFileData = mockFileSystem.GetFile(fileName);

        if (mockFileData != null)
        {
            return mockFileData.FileVersionInfo;
        }

        throw CommonExceptions.FileNotFound(fileName);
    }
}