using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions
{
    [Serializable]
    internal class FileStreamFactory : IFileStreamFactory
    {
        public Stream Create(string path, FileMode mode)
        {
            return new FileStream(path, mode);
        }

        public Stream Create(string path, FileMode mode, FileAccess access)
        {
            return new FileStream(path, mode, access);
        }

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new FileStream(path, mode, access, share);
        }

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
        {
            return new FileStream(path, mode, access, share, bufferSize);
        }

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
        {
            return new FileStream(path, mode, access, share, bufferSize, options);
        }

        public Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
        {
            return new FileStream(path, mode, access, share, bufferSize, useAsync);
        }

        public Stream Create(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            return new FileStream(path, mode, rights, share, bufferSize, options, fileSecurity);
        }

        public Stream Create(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options)
        {
            return new FileStream(path, mode, rights, share, bufferSize, options);
        }

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access)
        {
            return new FileStream(handle, access);
        }

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle)
        {
            return new FileStream(handle, access, ownsHandle);
        }

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize)
        {
            return new FileStream(handle, access, ownsHandle, bufferSize);
        }

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        public Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync)
        {
            return new FileStream(handle, access, ownsHandle, bufferSize, isAsync);
        }

        public Stream Create(SafeFileHandle handle, FileAccess access)
        {
            return new FileStream(handle, access);
        }

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
        {
            return new FileStream(handle, access, bufferSize);
        }

        public Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
        {
            return new FileStream(handle, access, bufferSize, isAsync);
        }
    }
}