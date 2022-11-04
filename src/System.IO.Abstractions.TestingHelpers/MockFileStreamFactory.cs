using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
    [Serializable]
    public class MockFileStreamFactory : IFileStreamFactory
    {
        private readonly IMockFileDataAccessor mockFileSystem;

        /// <inheritdoc />
        public MockFileStreamFactory(IMockFileDataAccessor mockFileSystem)
            => this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));

        /// <inheritdoc />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode)` instead.")]
        public Stream Create(string path, FileMode mode)
            => New(path, mode);

        /// <inheritdoc />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess)` instead.")]
        public Stream Create(string path, FileMode mode, FileAccess access)
            => New(path, mode, access);

        /// <inheritdoc />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess, FileShare)` instead.")]
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share)
            => New(path, mode, access, share);

        /// <inheritdoc />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess, FileShare, int)` instead.")]
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            => New(path, mode, access, share, bufferSize);

        /// <inheritdoc />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess, FileShare, int, FileOptions)` instead.")]
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            => New(path, mode, access, share, bufferSize, options);

        /// <inheritdoc />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess, FileShare, int, bool)` instead.")]
        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            => New(path, mode, access, share, bufferSize, useAsync);
        
        /// <inheritdoc />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        /// <inheritdoc />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        /// <inheritdoc />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        /// <inheritdoc />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        /// <inheritdoc />
        [Obsolete("Use `IFileStreamFactory.New(SafeFileHandle, FileAccess)` instead.")]
        public Stream Create(SafeFileHandle handle, FileAccess access)
            => New(handle, access);

        /// <inheritdoc />
        [Obsolete("Use `IFileStreamFactory.New(SafeFileHandle, FileAccess, int)` instead.")]
        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
            => New(handle, access, bufferSize);

        /// <inheritdoc />
        [Obsolete("Use `IFileStreamFactory.New(SafeFileHandle, FileAccess, int, bool)` instead.")]
        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
            => New(handle, access, bufferSize, isAsync);

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