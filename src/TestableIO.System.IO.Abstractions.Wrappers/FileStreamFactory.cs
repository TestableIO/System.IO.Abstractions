using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions
{
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    internal sealed class FileStreamFactory : IFileStreamFactory
    {
        public FileStreamFactory(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        /// <inheritdoc />
        public IFileSystem FileSystem { get; }

        /// <inheritdoc />
        public FileSystemStream New(SafeFileHandle handle, FileAccess access)
            => new FileStreamWrapper(new FileStream(handle, access));

        /// <inheritdoc />
        public FileSystemStream New(SafeFileHandle handle, FileAccess access, int bufferSize)
            => new FileStreamWrapper(new FileStream(handle, access, bufferSize));

        /// <inheritdoc />
        public FileSystemStream New(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync)
            => new FileStreamWrapper(new FileStream(handle, access, bufferSize, isAsync));


        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode)
            => new FileStreamWrapper(new FileStream(path, mode));


        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access)
            => new FileStreamWrapper(new FileStream(path, mode, access));

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share)
            => new FileStreamWrapper(new FileStream(path, mode, access, share));

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            => new FileStreamWrapper(new FileStream(path, mode, access, share, bufferSize));

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            => new FileStreamWrapper(new FileStream(path, mode, access, share, bufferSize, useAsync));

        /// <inheritdoc />
        public FileSystemStream New(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize,
            FileOptions options)
            => new FileStreamWrapper(new FileStream(path, mode, access, share, bufferSize, options));

#if FEATURE_FILESTREAM_OPTIONS
        /// <inheritdoc />
        public FileSystemStream New(string path, FileStreamOptions options)
            => new FileStreamWrapper(new FileStream(path, options));
#endif

        /// <inheritdoc />
        public FileSystemStream Wrap(FileStream fileStream)
            => new FileStreamWrapper(fileStream);
    }
}