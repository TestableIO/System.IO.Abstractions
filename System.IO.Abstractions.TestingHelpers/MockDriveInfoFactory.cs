using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDriveInfoFactory : IDriveInfoFactory
    {
        private readonly IMockFileDataAccessor mockFileSystem;

        public MockDriveInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
        }

        public DriveInfoBase[] GetDrives()
        {
            var driveLetters = new HashSet<string>(new DriveEqualityComparer(mockFileSystem));
            foreach (var path in mockFileSystem.AllPaths)
            {
                var pathRoot = mockFileSystem.Path.GetPathRoot(path);
                driveLetters.Add(pathRoot);
            }

            var result = new List<DriveInfoBase>();
            foreach (string driveLetter in driveLetters)
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

        public DriveInfoBase FromDriveName(string driveName)
        {
            var drive = mockFileSystem.Path.GetPathRoot(driveName);

            return new MockDriveInfo(mockFileSystem, drive);
        }

        private string NormalizeDriveName(string driveName)
        {
            if (driveName.Length == 3 && driveName.EndsWith(@":\", mockFileSystem.Comparison))
            {
                return (mockFileSystem.CaseSensitive ? driveName[0] : char.ToUpperInvariant(driveName[0])) + @":\";
            }

            if (driveName.StartsWith(@"\\", mockFileSystem.Comparison))
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
                if (ReferenceEquals(x, y))
                {
                    return true;
                }

                if (ReferenceEquals(x, null))
                {
                    return false;
                }

                if (ReferenceEquals(y, null))
                {
                    return false;
                }

                if (x[1] == ':' && y[1] == ':')
                {
                    return mockFileSystem.Comparer.Compare(x.Substring(0, 1), y.Substring(0, 1)) == 0;
                }

                return false;
            }

            public int GetHashCode(string obj)
            {
                return (mockFileSystem.CaseSensitive ? obj : obj.ToUpperInvariant()).GetHashCode();
            }
        }
    }
}
