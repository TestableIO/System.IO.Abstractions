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
            => new MockFileStream(mockFileSystem, path, GetStreamType(mode));

        public Stream Create(string path, FileMode mode, FileAccess access)
            => new MockFileStream(mockFileSystem, path, GetStreamType(mode, access));

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share)
            => new MockFileStream(mockFileSystem, path, GetStreamType(mode, access));

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            => new MockFileStream(mockFileSystem, path, GetStreamType(mode, access));

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            => new MockFileStream(mockFileSystem, path, GetStreamType(mode, access), options);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            => new MockFileStream(mockFileSystem, path, GetStreamType(mode, access));

#if NET40
        public Stream Create(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity fileSecurity)
            => new MockFileStream(mockFileSystem, path, GetStreamType(mode), options);

        public Stream Create(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options)
            => new MockFileStream(mockFileSystem, path, GetStreamType(mode), options);
#endif

#if NET40 || NETSTANDARD_20
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access)
            => new MockFileStream(mockFileSystem, handle.ToString(), GetStreamType(FileMode.Append, access));

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle)
            => new MockFileStream(mockFileSystem, handle.ToString(), GetStreamType(FileMode.Append, access));

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
            => new MockFileStream(mockFileSystem, handle.ToString(), GetStreamType(FileMode.Append, access));

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
            => new MockFileStream(mockFileSystem, handle.ToString(), GetStreamType(FileMode.Append, access));
#endif

        public Stream Create(SafeFileHandle handle, FileAccess access)
            => new MockFileStream(mockFileSystem, handle.ToString(), GetStreamType(FileMode.Append, access));

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
            => new MockFileStream(mockFileSystem, handle.ToString(), GetStreamType(FileMode.Append, access));

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
            => new MockFileStream(mockFileSystem, handle.ToString(), GetStreamType(FileMode.Append, access));

        private static MockFileStream.StreamType GetStreamType(FileMode mode, FileAccess access = FileAccess.ReadWrite)
        {
            if (access == FileAccess.Read)
            {
                return MockFileStream.StreamType.READ;
            }
            else if (mode == FileMode.Append) 
            {
                return MockFileStream.StreamType.APPEND;
            }
            else if (mode == FileMode.Truncate)
            {
                return MockFileStream.StreamType.TRUNCATE;
            }
            else if (mode == FileMode.Create)
            {
                return MockFileStream.StreamType.TRUNCATE;
            }
            else if (mode == FileMode.CreateNew)
            {
                return MockFileStream.StreamType.TRUNCATE;
            }
            else
            {
                return MockFileStream.StreamType.WRITE;
            }
        }
    }
}