using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class MockFileStreamFactory : IFileStreamFactory
    {
        private readonly IMockFileDataAccessor mockFileSystem;

        /// <inheritdoc />
        public MockFileStreamFactory(IMockFileDataAccessor mockFileSystem)
            => this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
        
        /// <inheritdoc />
        public IFileSystem FileSystem
            => mockFileSystem;

        /// <inheritdoc />
        public FileSystemStream New(SafeFileHandle handle, FileAccess access)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        /// <inheritdoc />
        public FileSystemStream New(SafeFileHandle handle, FileAccess access, int bufferSize)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        /// <inheritdoc />
        public FileSystemStream New(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode)
            => new MockFileStream(mockFileSystem, path, mode);

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access)
            => new MockFileStream(mockFileSystem, path, mode, access);

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share)
            => new MockFileStream(mockFileSystem, path, mode, access);

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            => new MockFileStream(mockFileSystem, path, mode, access);

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            => new MockFileStream(mockFileSystem, path, mode, access);

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize,
            FileOptions options)
            => new MockFileStream(mockFileSystem, path, mode, access, options);

#if FEATURE_FILESTREAM_OPTIONS
        /// <inheritdoc />
        public FileSystemStream New(string path, FileStreamOptions options)
            => new MockFileStream(mockFileSystem, path, options.Mode, options.Access, options.Options);
#endif

        /// <inheritdoc />
        public FileSystemStream Wrap(FileStream fileStream)
            => throw new NotSupportedException("You cannot wrap an existing FileStream in the MockFileSystem instance!");
    }
}