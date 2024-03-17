using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class MockDriveInfoFactory : IDriveInfoFactory
    {
        private readonly IMockFileDataAccessor mockFileSystem;

        /// <inheritdoc />
        public MockDriveInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
        }

        /// <inheritdoc />
        public IFileSystem FileSystem
            => mockFileSystem;

        /// <inheritdoc />
        public IDriveInfo[] GetDrives()
        {
            var result = new List<DriveInfoBase>();
            foreach (string driveLetter in mockFileSystem.AllDrives)
            {
                try
                {
                    var mockDriveInfo = new MockDriveInfo(mockFileSystem, driveLetter);
                    result.Add(mockDriveInfo);
                }
                catch (ArgumentException)
                {
                    // invalid drives should be ignored
                }
            }

            return result.ToArray();
        }

        /// <inheritdoc />
        public IDriveInfo New(string driveName)
        {
            var drive = mockFileSystem.Path.GetPathRoot(driveName);

            return new MockDriveInfo(mockFileSystem, drive);
        }

        /// <inheritdoc />
        public IDriveInfo Wrap(DriveInfo driveInfo)
        {
            if (driveInfo == null)
            {
                return null;
            }

            return New(driveInfo.Name);
        }

        private string NormalizeDriveName(string driveName)
        {
            if (driveName.Length == 3 && mockFileSystem.StringOperations.EndsWith(driveName, @":\"))
            {
                return mockFileSystem.StringOperations.ToUpper(driveName[0]) + @":\";
            }

            if (mockFileSystem.StringOperations.StartsWith(driveName, @"\\"))
            {
                return null;
            }

            return driveName;
        }

        private class DriveEqualityComparer : IEqualityComparer<string>
        {
            private readonly IMockFileDataAccessor mockFileSystem;

            public DriveEqualityComparer(IMockFileDataAccessor mockFileSystem)
            {
                this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
            }

            public bool Equals(string x, string y)
            {
                return ReferenceEquals(x, y) ||
                       (HasDrivePrefix(x) && HasDrivePrefix(y) && mockFileSystem.StringOperations.Equals(x[0], y[0]));
            }

            private static bool HasDrivePrefix(string x)
            {
                return x != null && x.Length >= 2 && x[1] == ':';
            }

            public int GetHashCode(string obj)
            {
                return mockFileSystem.StringOperations.ToUpper(obj).GetHashCode();
            }
        }
    }
}
