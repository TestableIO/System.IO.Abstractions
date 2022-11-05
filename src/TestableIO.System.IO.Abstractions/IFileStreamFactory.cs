using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory for the creation of wrappers for <see cref="FileStream" /> in a <see cref="IFileSystem" />.
    /// </summary>
    public interface IFileStreamFactory : IFileSystemExtensionPoint
    {
        /// <inheritdoc cref="FileStream(string,FileMode)" />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode)` instead.")]
        Stream Create(string path, FileMode mode);

        /// <inheritdoc cref="FileStream(string,FileMode,FileAccess)" />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess)` instead.")]
        Stream Create(string path, FileMode mode, FileAccess access);

        /// <inheritdoc cref="FileStream(string,FileMode,FileAccess,FileShare)" />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess, FileShare)` instead.")]
        Stream Create(string path, FileMode mode, FileAccess access, FileShare share);

        /// <inheritdoc cref="FileStream(string,FileMode,FileAccess,FileShare,int)" />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess, FileShare, int)` instead.")]
        Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize);

        /// <inheritdoc cref="FileStream(string,FileMode,FileAccess,FileShare,int,FileOptions)" />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess, FileShare, int, FileOptions)` instead.")]
        Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options);

        /// <inheritdoc cref="FileStream(string,FileMode,FileAccess,FileShare,int,bool)" />
        [Obsolete("Use `IFileStreamFactory.New(string, FileMode, FileAccess, FileShare, int, bool)` instead.")]
        Stream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync);

        /// <inheritdoc cref="FileStream(SafeFileHandle,FileAccess)" />
        [Obsolete("Use `IFileStreamFactory.New(SafeFileHandle, FileAccess)` instead.")]
        Stream Create(SafeFileHandle handle, FileAccess access);

        /// <inheritdoc cref="FileStream(SafeFileHandle,FileAccess,int)" />
        [Obsolete("Use `IFileStreamFactory.New(SafeFileHandle, FileAccess, int)` instead.")]
        Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize);

        /// <inheritdoc cref="FileStream(SafeFileHandle,FileAccess,int,bool)" />
        [Obsolete("Use `IFileStreamFactory.New(SafeFileHandle, FileAccess, int, bool)` instead.")]
        Stream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync);

        /// <inheritdoc cref="FileStream(IntPtr,FileAccess)" />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access);

        /// <inheritdoc cref="FileStream(IntPtr,FileAccess,bool)" />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access, bool ownsHandle);

        /// <inheritdoc cref="FileStream(IntPtr,FileAccess,bool,int)" />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize);

        /// <inheritdoc cref="FileStream(IntPtr,FileAccess,bool,int,bool)" />
        [Obsolete("This method has been deprecated. Please use new Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed. http://go.microsoft.com/fwlink/?linkid=14202")]
        Stream Create(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync);

        /// <inheritdoc cref="FileStream(SafeFileHandle, FileAccess)" />
        FileSystemStream New(SafeFileHandle handle, FileAccess access);

        /// <inheritdoc cref="FileStream(SafeFileHandle ,FileAccess, int)" />
        FileSystemStream New(SafeFileHandle handle, FileAccess access, int bufferSize);

        /// <inheritdoc cref="FileStream(SafeFileHandle, FileAccess, int, bool)" />
        FileSystemStream New(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync);
        
        /// <inheritdoc cref="FileStream(string, FileMode)" />
        FileSystemStream New(string path, FileMode mode);

        /// <inheritdoc cref="FileStream(string, FileMode, FileAccess)" />
        FileSystemStream New(string path, FileMode mode, FileAccess access);

        /// <inheritdoc cref="FileStream(string, FileMode, FileAccess, FileShare)" />
        FileSystemStream New(string path, FileMode mode, FileAccess access,
            FileShare share);

        /// <inheritdoc cref="FileStream(string, FileMode, FileAccess, FileShare, int)" />
        FileSystemStream New(string path, FileMode mode, FileAccess access,
            FileShare share, int bufferSize);

        /// <inheritdoc cref="FileStream(string, FileMode, FileAccess, FileShare, int, bool)" />
        FileSystemStream New(string path, FileMode mode, FileAccess access,
            FileShare share, int bufferSize, bool useAsync);

        /// <inheritdoc cref="FileStream(string, FileMode, FileAccess, FileShare, int, FileOptions)" />
        FileSystemStream New(string path, FileMode mode, FileAccess access,
            FileShare share, int bufferSize, FileOptions options);

#if FEATURE_FILESTREAM_OPTIONS
	    /// <inheritdoc cref="FileStream(string, FileStreamOptions)" />
	    FileSystemStream New(string path, FileStreamOptions options);
#endif

        /// <summary>
        ///     Wraps the <paramref name="fileStream" /> to the testable <see cref="FileSystemStream" />.
        /// </summary>
        FileSystemStream Wrap(FileStream fileStream);
    }
}