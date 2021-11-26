using System.Collections.Generic;
using System.Reflection;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// Provides access to the file system storage.
    /// </summary>
    public interface IMockFileDataAccessor
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
        /// </summary>
        
        StringOperations StringOperations { get; }

        /// <summary>
        /// </summary>
        IFile File { get; }
        
        /// <summary>
        /// </summary>
        IDirectory Directory { get; }
        
        /// <summary>
        /// </summary>
        IFileInfoFactory FileInfo {get; }
        
        /// <summary>
        /// </summary>
        IPath Path { get; }
        
        /// <summary>
        /// </summary>
        IDirectoryInfoFactory DirectoryInfo { get; }
        
        /// <summary>
        /// </summary>
        IDriveInfoFactory DriveInfo { get; }

        /// <summary>
        /// </summary>
        PathVerifier PathVerifier { get; }

        
        /// <summary>
        /// </summary>
        IFileSystem FileSystem { get; }
    }
}
