using System.Security.AccessControl;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions
{
    [Serializable]
    internal sealed class FileStreamFactory : IFileStreamFactory
    {
        public IFileSystem FileSystem { get; }

        public FileStreamFactory(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <inheritdoc />
        [Obsolete("Use `FileStreamFactory.New` instead")]
        public FileSystemStream Create(string path, FileMode mode)
            => New(path, mode);

        /// <inheritdoc />
        [Obsolete("Use `FileStreamFactory.New` instead")]
        public FileSystemStream Create(string path, FileMode mode, FileAccess access)
            => New(path, mode, access);

        /// <inheritdoc />
        [Obsolete("Use `FileStreamFactory.New` instead")]
        public FileSystemStream Create(string path, FileMode mode, FileAccess access, FileShare share)
            => New(path, mode, access, share);

        /// <inheritdoc />
        [Obsolete("Use `FileStreamFactory.New` instead")]
        public FileSystemStream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            => New(path, mode, access, share, bufferSize);

        /// <inheritdoc />
        [Obsolete("Use `FileStreamFactory.New` instead")]
        public FileSystemStream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            => New(path, mode, access, share, bufferSize, options);

        /// <inheritdoc />
        [Obsolete("Use `FileStreamFactory.New` instead")]
        public FileSystemStream Create(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            => New(path, mode, access, share, bufferSize, useAsync);

        /// <inheritdoc />
        public FileSystemStream Create(SafeFileHandle handle, FileAccess access)
            => Wrap(new FileStream(handle, access));

        /// <inheritdoc />
        public FileSystemStream Create(SafeFileHandle handle, FileAccess access, int bufferSize)
            => Wrap(new FileStream(handle, access, bufferSize));

        /// <inheritdoc />
        public FileSystemStream Create(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
            => Wrap(new FileStream(handle, access, bufferSize, isAsync));

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

        public FileSystemStream New(string path, FileMode mode)
        {
            return new FileStreamWrapper(new FileStream(path, mode));
        }

        public FileSystemStream New(string path, FileMode mode, FileAccess access)
        {
            return new FileStreamWrapper(new FileStream(path, mode, access));
        }

        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new FileStreamWrapper(new FileStream(path, mode, access, share));
        }

        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
        {
            return new FileStreamWrapper(new FileStream(path, mode, access, share, bufferSize));
        }

        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
        {
            return new FileStreamWrapper(new FileStream(path, mode, access, share, bufferSize, useAsync));
        }

        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize,
            FileOptions options)
        {
            return new FileStreamWrapper(new FileStream(path, mode, access, share, bufferSize, options));
        }

#if FEATURE_FILESTREAMOPTIONS
        public FileSystemStream New(string path, FileStreamOptions options)
        {
            return new FileStreamWrapper(new FileStream(path, options));
        }
#endif

        public FileSystemStream Wrap(FileStream fileStream)
        {
            return new FileStreamWrapper(fileStream);
        }
    }
}