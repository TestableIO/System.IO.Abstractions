﻿using System.Runtime.Versioning;

namespace System.IO.Abstractions
{
    /// <summary>
    /// The wrapper for a <see cref="DriveInfo"/>.
    /// </summary>
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class DriveInfoWrapper : DriveInfoBase
    {
        /// <summary>
        /// The instance of the real <see cref="FileSystem"/>.
        /// </summary>
        private readonly DriveInfo instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="DriveInfoWrapper"/> class, which acts as a wrapper for a drive info.
        /// </summary>
        /// <param name="fileSystem">The underlying IFileSystem.</param>
        /// <param name="instance">The drive info.</param>
        public DriveInfoWrapper(IFileSystem fileSystem, DriveInfo instance) : base(fileSystem)
        {
            this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        /// <summary>
        /// Gets or sets the name of a drive, such as C:\.
        /// </summary>
        /// <value>The name of the drive.</value>
        /// <remarks>
        /// This property is the name assigned to the drive, such as C:\ or E:\.
        /// </remarks>
        public override string Name
        {
            get { return instance.Name; }
        }

        /// <summary>
        /// Gets or sets the drive type, such as CD-ROM, removable, network, or fixed.
        /// </summary>
        /// <value>One of the enumeration values that specifies a drive type.</value>
        /// <remarks>
        /// The DriveType property indicates whether a drive is one of the following: CDRom, Fixed, Network, NoRootDirectory, Ram, Removable, or Unknown.
        /// These values are described in the DriveType enumeration.
        /// </remarks>
        public override DriveType DriveType
        {
            get { return instance.DriveType; }
        }

        /// <summary>
        /// Gets or sets the name of the file system, such as NTFS or FAT32.
        /// </summary>
        /// <remarks>
        /// Use DriveFormat to determine what formatting a drive uses.
        /// </remarks>
        /// <value>The name of the file system on the specified drive.</value>
        /// <exception cref="UnauthorizedAccessException">Thrown if the access to the drive information is denied.</exception>
        /// <exception cref="DriveNotFoundException">Thrown if the drive does not exist or is not mapped.</exception>
        /// <exception cref="IOException">Thrown if an I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        public override string DriveFormat
        {
            get { return instance.DriveFormat; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a drive is ready.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the drive is ready; <see langword="false"/> if the drive is not ready.
        /// </value>
        /// <remarks>
        /// IsReady indicates whether a drive is ready.
        /// For example, it indicates whether a CD is in a CD drive or whether a removable storage device is ready for read/write operations.
        /// If you do not test whether a drive is ready, and it is not ready, querying the drive using <see cref="DriveInfoBase"/> will raise an IOException.
        /// Do not rely on IsReady to avoid catching exceptions from other members such as TotalSize, TotalFreeSpace, and <see cref="DriveInfoBase.DriveFormat"/>.
        /// Between the time that your code checks IsReady and then accesses one of the other properties (even if the access occurs immediately after the check),
        ///  a drive may have been disconnected or a disk may have been removed.
        /// </remarks>
        public override bool IsReady
        {
            get { return instance.IsReady; }
        }

        /// <summary>
        /// Gets or sets the amount of available free space on a drive, in bytes.
        /// </summary>
        /// <value>The amount of free space available on the drive, in bytes.</value>
        /// <remarks>
        /// This property indicates the amount of free space available on the drive.
        /// Note that this number may be different from the TotalFreeSpace number because this property takes into account disk quotas.
        /// </remarks>
        /// <exception cref="UnauthorizedAccessException">Thrown if the access to the drive information is denied.</exception>
        /// <exception cref="IOException">Thrown if an I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        public override long AvailableFreeSpace
        {
            get { return instance.AvailableFreeSpace; }
        }

        /// <summary>
        /// Gets or sets the total amount of free space available on a drive, in bytes.
        /// </summary>
        /// <value>The total free space available on a drive, in bytes.</value>
        /// <remarks>This property indicates the total amount of free space available on the drive, not just what is available to the current user.</remarks>
        /// <exception cref="UnauthorizedAccessException">Thrown if the access to the drive information is denied.</exception>
        /// <exception cref="DriveNotFoundException">Thrown if the drive does not exist or is not mapped.</exception>
        /// <exception cref="IOException">Thrown if an I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        public override long TotalFreeSpace
        {
            get { return instance.TotalFreeSpace; }
        }

        /// <summary>
        /// Gets or sets the total size of storage space on a drive, in bytes.
        /// </summary>
        /// <value>The total size of the drive, in bytes.</value>
        /// <remarks>
        /// This property indicates the total size of the drive in bytes, not just what is available to the current user.
        /// </remarks>
        /// <exception cref="UnauthorizedAccessException">Thrown if the access to the drive information is denied.</exception>
        /// <exception cref="DriveNotFoundException">Thrown if the drive does not exist or is not mapped.</exception>
        /// <exception cref="IOException">Thrown if an I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        public override long TotalSize
        {
            get { return instance.TotalSize; }
        }

        /// <summary>
        /// Gets or sets the root directory of a drive.
        /// </summary>
        /// <value>An object that contains the root directory of the drive.</value>
        public override IDirectoryInfo RootDirectory
        {
            get { return new DirectoryInfoWrapper(FileSystem, instance.RootDirectory); }
        }

        /// <summary>
        /// Gets or sets the volume label of a drive.
        /// </summary>
        /// <value>The volume label.</value>
        /// <remarks>
        /// The label length is determined by the operating system. For example, NTFS allows a volume label to be up to 32 characters long. Note that <see langword="null"/> is a valid VolumeLabel.
        /// </remarks>
        /// <exception cref="IOException">Thrown if an I/O error occurred (for example, a disk error or a drive was not ready).</exception>
        /// <exception cref="DriveNotFoundException">Thrown if the drive does not exist or is not mapped.</exception>
        /// <exception cref="System.Security.SecurityException">Thrown if the caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// Thrown if the volume label is being set on a network or CD-ROM drive
        /// -or-
        /// Access to the drive information is denied.
        /// </exception>
        public override string VolumeLabel
        {
            get { return instance.VolumeLabel; }

            [SupportedOSPlatform("windows")]
#pragma warning disable CA1416
            set { instance.VolumeLabel = value; }
#pragma warning restore CA1416
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return instance.ToString();
        }
    }
}
