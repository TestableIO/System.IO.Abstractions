using System.Collections.Generic;
using System.Linq;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDriveInfoFactory : IDriveInfoFactory
    {
        private readonly IMockFileDataAccessor mockFileSystem;

        public MockDriveInfoFactory(IMockFileDataAccessor mockFileSystem)
        {
            this.mockFileSystem = mockFileSystem ?? throw new ArgumentNullException(nameof(mockFileSystem));
            _drives = new Dictionary<string, DriveInfoBase>(new DriveEqualityComparer(mockFileSystem));
        }

        private Dictionary<string, DriveInfoBase> _drives;

        public DriveInfoBase[] GetDrives()
        {
            foreach (var path in mockFileSystem.AllPaths)
            {
                var pathRoot = mockFileSystem.Path.GetPathRoot(path);
                if (!_drives.ContainsKey(pathRoot))
                    _drives.Add(pathRoot, new MockDriveInfo(mockFileSystem, pathRoot));
            }

            return _drives.Values.ToArray();
        }

        public DriveInfoBase FromDriveName(string driveName)
        {
            var drive = mockFileSystem.Path.GetPathRoot(driveName);

            return new MockDriveInfo(mockFileSystem, drive);
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
