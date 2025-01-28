namespace System.IO.Abstractions
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public abstract class DriveInfoBase : IDriveInfo
    {
        /// <summary>
        /// Base class for calling methods of <see cref="DriveInfo"/>
        /// </summary>
        protected DriveInfoBase(IFileSystem fileSystem)
        {
            FileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        [Obsolete("This constructor only exists to support mocking libraries.", error: true)]
        internal DriveInfoBase() { }

        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        public IFileSystem FileSystem { get; }

        /// <inheritdoc cref="IDriveInfo.AvailableFreeSpace"/>
        public abstract long AvailableFreeSpace { get; }

        /// <inheritdoc cref="IDriveInfo.DriveFormat"/>
        public abstract string DriveFormat { get; }

        /// <inheritdoc cref="IDriveInfo.DriveType"/>
        public abstract DriveType DriveType { get; }

        /// <inheritdoc cref="IDriveInfo.IsReady"/>
        public abstract bool IsReady { get; }

        /// <inheritdoc cref="IDriveInfo.Name"/>
        public abstract string Name { get; }

        /// <inheritdoc cref="IDriveInfo.RootDirectory"/>
        public abstract IDirectoryInfo RootDirectory { get; }

        /// <inheritdoc cref="IDriveInfo.TotalFreeSpace"/>
        public abstract long TotalFreeSpace { get; }

        /// <inheritdoc cref="IDriveInfo.TotalSize"/>
        public abstract long TotalSize { get; }

        /// <inheritdoc cref="IDriveInfo.VolumeLabel"/>
        public abstract string VolumeLabel { get; set; }

        /// <summary>
        /// Implicitly converts a <see cref="DriveInfo"/> to a <see cref="DriveInfoBase"/>.
        /// </summary>
        public static implicit operator DriveInfoBase(DriveInfo driveInfo)
        {
            if (driveInfo == null)
            {
                return null;
            }

            return new DriveInfoWrapper(new FileSystem(), driveInfo);
        }
    }
}
