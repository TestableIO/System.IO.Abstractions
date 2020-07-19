using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions
{
    [Serializable]
    internal sealed class FileStreamFactory : IFileStreamFactory
    {
        public Stream Create(string path, FileMode mode)
            => new FileStream(path, mode);

        public Stream Create(string path, FileMode mode, FileAccess access)
            => new FileStream(path, mode, access);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share)
            => new FileStream(path, mode, access, share);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            => new FileStream(path, mode, access, share, bufferSize);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            => new FileStream(path, mode, access, share, bufferSize, options);

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            => new FileStream(path, mode, access, share, bufferSize, useAsync);

        public Stream Create(SafeFileHandle handle, FileAccess access)
            => new FileStream(handle, access);

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
            => new FileStream(handle, access, bufferSize);

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
            => new FileStream(handle, access, bufferSize, isAsync);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access)
            => new FileStream(handle, access);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle)
            => new FileStream(handle, access, ownsHandle);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
            => new FileStream(handle, access, ownsHandle, bufferSize);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
            => new FileStream(handle, access, ownsHandle, bufferSize, isAsync);
    }
}