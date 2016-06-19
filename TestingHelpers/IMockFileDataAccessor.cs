using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    public interface IMockFileDataAccessor
    {
        /// <summary>
        /// Gets a file.
        /// </summary>
        /// <param name="path">The path of the file to get.</param>
        /// <returns>The file. <see langword="null"/> if the file does not exist.</returns>
        MockFileData GetFile(string path);

        void AddFile(string path, MockFileData mockFile);
        void AddDirectory(string path);

        /// <summary>
        /// Removes the file.
        /// </summary>
        /// <param name="path">The file to remove.</param>
        /// <remarks>
        /// The file must not exist.
        /// </remarks>
        void RemoveFile(string path);

        /// <summary>
        /// Determines whether the file exists.
        /// </summary>
        /// <param name="path">The file to check. </param>
        /// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</returns>
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