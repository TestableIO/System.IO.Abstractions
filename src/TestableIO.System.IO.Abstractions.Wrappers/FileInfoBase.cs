namespace System.IO.Abstractions
{
    /// <inheritdoc cref="FileInfo"/>
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public abstract class FileInfoBase : FileSystemInfoBase, IFileInfo
    {
        /// <summary>
        /// Base class for calling methods of <see cref="FileInfo"/>
        /// </summary>
        protected FileInfoBase(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        [Obsolete("This constructor only exists to support mocking libraries.", error: true)]
        internal FileInfoBase() { }

        /// <inheritdoc cref="IFileInfo.AppendText"/>
        public abstract StreamWriter AppendText();

        /// <inheritdoc cref="IFileInfo.CopyTo(string)"/>
        public abstract IFileInfo CopyTo(string destFileName);

        /// <inheritdoc cref="IFileInfo.CopyTo(string,bool)"/>
        public abstract IFileInfo CopyTo(string destFileName, bool overwrite);

        /// <inheritdoc cref="IFileInfo.Create"/>
        public abstract FileSystemStream Create();

        /// <inheritdoc cref="IFileInfo.CreateText"/>
        public abstract StreamWriter CreateText();

        /// <inheritdoc cref="IFileInfo.Decrypt"/>
        public abstract void Decrypt();

        /// <inheritdoc cref="IFileInfo.Encrypt"/>
        public abstract void Encrypt();

        /// <inheritdoc cref="IFileInfo.MoveTo(string)"/>
        public abstract void MoveTo(string destFileName);

#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc cref="IFileInfo.MoveTo(string,bool)"/>
        public abstract void MoveTo(string destFileName, bool overwrite);
#endif

        /// <inheritdoc cref="IFileInfo.Open(FileMode)"/>
        public abstract FileSystemStream Open(FileMode mode);

        /// <inheritdoc cref="IFileInfo.Open(FileMode,FileAccess)"/>
        public abstract FileSystemStream Open(FileMode mode, FileAccess access);

        /// <inheritdoc cref="IFileInfo.Open(FileMode,FileAccess,FileShare)"/>
        public abstract FileSystemStream Open(FileMode mode, FileAccess access, FileShare share);

#if FEATURE_FILESTREAM_OPTIONS
        /// <inheritdoc cref="IFileInfo.Open(FileStreamOptions)"/>
        public abstract FileSystemStream Open(FileStreamOptions options);
#endif

        /// <inheritdoc cref="IFileInfo.OpenRead"/>
        public abstract FileSystemStream OpenRead();

        /// <inheritdoc cref="IFileInfo.OpenText"/>
        public abstract StreamReader OpenText();

        /// <inheritdoc cref="IFileInfo.OpenWrite"/>
        public abstract FileSystemStream OpenWrite();

        /// <inheritdoc cref="IFileInfo.Replace(string,string)"/>
        public abstract IFileInfo Replace(string destinationFileName, string destinationBackupFileName);

        /// <inheritdoc cref="IFileInfo.Replace(string,string,bool)"/>
        public abstract IFileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);

        /// <inheritdoc cref="IFileInfo.Directory"/>
        public abstract IDirectoryInfo Directory { get; }

        /// <inheritdoc cref="IFileInfo.DirectoryName"/>
        public abstract string DirectoryName { get; }

        /// <inheritdoc cref="IFileInfo.IsReadOnly"/>
        public abstract bool IsReadOnly { get; set; }

        /// <inheritdoc cref="IFileInfo.Length"/>
        public abstract long Length { get; }

        /// <summary>
        /// Implicitly converts a <see cref="FileInfo"/> to a <see cref="FileInfoBase"/>.
        /// </summary>
        public static implicit operator FileInfoBase(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return null;
            }

            return new FileInfoWrapper(new FileSystem(), fileInfo);
        }
    }
}
