using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="FileStream" />
    public interface IFileStreamFactory
    {
        /// <inheritdoc cref="FileStream.FileStream(string,FileMode)" />
        Stream Create(string path, FileMode mode);

        /// <inheritdoc cref="FileStream.FileStream(string,FileMode,FileAccess)" />
        Stream Create(string path, FileMode mode, FileAccess access);

        /// <inheritdoc cref="FileStream.FileStream(string,FileMode,FileAccess,FileShare)" />
        Stream Create(string path, FileMode mode, FileAccess access, FileShare share);

        /// <inheritdoc cref="FileStream.FileStream(string,FileMode,FileAccess,FileShare,int)" />
        Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize);

        /// <inheritdoc cref="FileStream.FileStream(string,FileMode,FileAccess,FileShare,int,FileOptions)" />
        Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options);

        /// <inheritdoc cref="FileStream.FileStream(string,FileMode,FileAccess,FileShare,int,bool)" />
        Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync);

        /// <inheritdoc cref="FileStream.FileStream(SafeFileHandle,FileAccess)" />
        Stream Create(SafeFileHandle handle, FileAccess access);

        /// <inheritdoc cref="FileStream.FileStream(SafeFileHandle,FileAccess,int)" />
        Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize);

        /// <inheritdoc cref="FileStream.FileStream(SafeFileHandle,FileAccess,int,bool)" />
        Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync);

        /// <inheritdoc cref="FileStream.FileStream(IntPtr,FileAccess)" />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access);

        /// <inheritdoc cref="FileStream.FileStream(IntPtr,FileAccess,bool)" />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access, bool ownsHandle);

        /// <inheritdoc cref="FileStream.FileStream(IntPtr,FileAccess,bool,int)" />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize);

        /// <inheritdoc cref="FileStream.FileStream(IntPtr,FileAccess,bool,int,bool)" />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync);
    }
}