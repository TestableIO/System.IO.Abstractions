namespace System.IO.Abstractions.TestingHelpers
{
    public interface IMockFileDataAccessor
    {
        MockFileData GetFile(string path);
        bool FileExists(string path);
    }
}