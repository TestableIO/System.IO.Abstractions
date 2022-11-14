using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="DriveInfo" />
    public interface IDriveInfo : IFileSystemEntity
    {
        /// <inheritdoc cref="DriveInfo.AvailableFreeSpace" />
        long AvailableFreeSpace { get; }

        /// <inheritdoc cref="DriveInfo.DriveFormat" />
        string DriveFormat { get; }

        /// <inheritdoc cref="DriveInfo.DriveType" />
        DriveType DriveType { get; }

        /// <inheritdoc cref="DriveInfo.IsReady" />
        bool IsReady { get; }

        /// <inheritdoc cref="DriveInfo.Name" />
        string Name { get; }

        /// <inheritdoc cref="DriveInfo.RootDirectory" />
        IDirectoryInfo RootDirectory { get; }

        /// <inheritdoc cref="DriveInfo.TotalFreeSpace" />
        long TotalFreeSpace { get; }

        /// <inheritdoc cref="DriveInfo.TotalSize" />
        long TotalSize { get; }

        /// <inheritdoc cref="DriveInfo.VolumeLabel" />
        [AllowNull]
        string VolumeLabel
        {
            get;
            [SupportedOSPlatform("windows")] set;
        }
    }
}