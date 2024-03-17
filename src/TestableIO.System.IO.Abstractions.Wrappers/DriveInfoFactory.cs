﻿namespace System.IO.Abstractions
{
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    internal class DriveInfoFactory : IDriveInfoFactory
    {
        private readonly IFileSystem fileSystem;

        public DriveInfoFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <inheritdoc />
        public IFileSystem FileSystem
            => fileSystem;

        /// <inheritdoc />
        public IDriveInfo[] GetDrives()
        {
            var driveInfos = DriveInfo.GetDrives();
            var driveInfoWrappers = new DriveInfoBase[driveInfos.Length];
            for (int index = 0; index < driveInfos.Length; index++)
            {
                var driveInfo = driveInfos[index];
                driveInfoWrappers[index] = new DriveInfoWrapper(fileSystem, driveInfo);
            }

            return driveInfoWrappers;
        }

        /// <inheritdoc />
        public IDriveInfo New(string driveName)
        {
            var realDriveInfo = new DriveInfo(driveName);
            return new DriveInfoWrapper(fileSystem, realDriveInfo);
        }

        /// <inheritdoc />
        public IDriveInfo Wrap(DriveInfo driveInfo)
        {
            if (driveInfo == null)
            {
                return null;
            }

            return new DriveInfoWrapper(fileSystem, driveInfo);
        }
    }
}
