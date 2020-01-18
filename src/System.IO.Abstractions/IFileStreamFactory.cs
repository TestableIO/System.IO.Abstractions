using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions
{
    public interface IFileStreamFactory
    {
        Stream Create(string path, FileMode mode);

        Stream Create(string path, FileMode mode, FileAccess access);

        Stream Create(string path, FileMode mode, FileAccess access, FileShare share);

        Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize);

        Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync);

        Stream Create(SafeFileHandle handle, FileAccess access);

        Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize);

        Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access, bool ownsHandle);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize);

        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync);
    }
}