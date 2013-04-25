using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    public interface IMockFileDataAccessor {
        MockFileData GetFile(string path, bool returnNullObject = false);
        void AddFile(string path, MockFileData mockFile);
        void RemoveFile(string path);
        bool FileExists(string path);
        IEnumerable<string> AllPaths { get; }
        IEnumerable<string> AllFiles { get; }
        IEnumerable<string> AllDirectories { get; }
        DirectoryBase Directory { get; }
        IFileInfoFactory FileInfo {get; }
    }
}