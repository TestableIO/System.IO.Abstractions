using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileStreamFactory : IFileStreamFactory
    {
        private readonly IMockFileDataAccessor mockFileSystem;

        public MockFileStreamFactory(IMockFileDataAccessor mockFileSystem)
            => this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));

        public Stream Create(string path, FileMode mode)
            => new MockFileStream(mockFileSystem, path, mode);

        public Stream Create(string path, FileMode mode, FileAccess access)
            => new MockFileStream(mockFileSystem, path, mode, access);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share)
            => new MockFileStream(mockFileSystem, path, mode, access);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            => new MockFileStream(mockFileSystem, path, mode, access);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            => new MockFileStream(mockFileSystem, path, mode, access, options);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            => new MockFileStream(mockFileSystem, path, mode, access);

        public Stream Create(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity fileSecurity)
            => new MockFileStream(mockFileSystem, path, mode, options: options);

        public Stream Create(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options)
            => new MockFileStream(mockFileSystem, path, mode, options: options);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        public Stream Create(SafeFileHandle handle, FileAccess access)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
            => new MockFileStream(mockFileSystem, handle.ToString(), FileMode.Open, access: access);
    }
}