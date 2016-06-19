using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    public interface IMockFileDataAccessor {
        MockFileData GetFile(string path, bool returnNullObject = false);

        /// <summary>
        /// Gets a file specified by <paramref name="path"/>.
        /// </summary>
        /// <param name="path">The path of the file to get.</param>
        /// <param name="result">If the method returns <see langword="true"/> then the value contains the <see cref="MockFileData"/>. If the method returns <see langword="true"/> then the value is not set.</param>
        /// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</returns>
        bool TryGetFile(string path, out MockFileData result);

        void AddFile(string path, MockFileData mockFile);
        void AddDirectory(string path);
        void RemoveFile(string path);
        bool FileExists(string path);
        IEnumerable<string> AllPaths { get; }
        IEnumerable<string> AllFiles { get; }
        IEnumerable<string> AllDirectories { get; }
        DirectoryBase Directory { get; }
        IFileInfoFactory FileInfo {get; }
        PathBase Path { get; }
        IDirectoryInfoFactory DirectoryInfo { get; }
        IDriveInfoFactory DriveInfo { get; }

        PathVerifier PathVerifier { get; }
    }
}