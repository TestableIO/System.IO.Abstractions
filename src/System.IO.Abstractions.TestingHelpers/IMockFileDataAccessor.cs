using System.Collections.Generic;
using System.Reflection;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// Provides access to the file system storage.
    /// </summary>
    public interface IMockFileDataAccessor : IFileSystem
    {
        /// <summary>
        /// Gets a file.
        /// </summary>
        /// <param name="path">The path of the file to get.</param>
        /// <returns>The file. <see langword="null"/> if the file does not exist.</returns>
        MockFileData GetFile(string path);

        /// <summary>
        /// </summary>
        void AddFile(string path, MockFileData mockFile);

        /// <summary>
        /// </summary>
        void AddDirectory(string path);

        /// <summary>
        /// </summary>
        void AddFileFromEmbeddedResource(string path, Assembly resourceAssembly, string embeddedResourcePath);

        /// <summary>
        /// </summary>
        void AddFilesFromEmbeddedNamespace(string path, Assembly resourceAssembly, string embeddedResourcePath);

        /// <summary>
        /// </summary>
        void MoveDirectory(string sourcePath, string destPath);

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

        /// <summary>
        /// Gets all unique paths of all files and directories.
        /// </summary>
        IEnumerable<string> AllPaths { get; }

        /// <summary>
        /// Gets the paths of all files.
        /// </summary>
        IEnumerable<string> AllFiles { get; }

        /// <summary>
        /// Gets the paths of all directories.
        /// </summary>
        IEnumerable<string> AllDirectories { get; }

        /// <summary>
        /// Gets a helper for string operations.
        /// </summary>

        StringOperations StringOperations { get; }

        /// <summary>
        /// Gets a helper for verifying file system paths.
        /// </summary>
        PathVerifier PathVerifier { get; }

        /// <summary>
        /// Gets a reference to the underlying file system. 
        /// </summary>
        IFileSystem FileSystem { get; }
    }
}
