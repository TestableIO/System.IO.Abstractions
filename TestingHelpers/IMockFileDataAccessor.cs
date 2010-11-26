using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    public interface IMockFileDataAccessor
    {
        MockFileData GetFile(string path);
        void AddFile(string path, MockFileData mockFile);
        bool FileExists(string path);
        IEnumerable<string> AllPaths { get; }
    }
}