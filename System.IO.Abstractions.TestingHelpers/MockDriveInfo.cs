﻿namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDriveInfo : DriveInfoBase
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;

        public MockDriveInfo(IMockFileDataAccessor mockFileDataAccessor, string name) : base(mockFileDataAccessor?.FileSystem)
        {
            this.mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            const string DRIVE_SEPARATOR = @":\";

            if (name.Length == 1
                || (name.Length == 2 && name[1] == ':')
                || (name.Length == 3 && name.EndsWith(DRIVE_SEPARATOR, mockFileDataAccessor.Comparison)))
            {
                name = ToUpper(name[0]) + DRIVE_SEPARATOR;
            }
            else
            {
                mockFileDataAccessor.PathVerifier.CheckInvalidPathChars(name);
                name = mockFileDataAccessor.Path.GetPathRoot(name);

                if (string.IsNullOrEmpty(name) || name.StartsWith(@"\\", mockFileDataAccessor.Comparison))
                {
                    throw new ArgumentException(
                        @"Object must be a root directory (""C:\"") or a drive letter (""C"").");
                }
            }

            Name = name;
            IsReady = true;
        }

        private char ToUpper(char c)
        {
            return mockFileDataAccessor.CaseSensitive ? c : char.ToUpperInvariant(c);
        }

        public new long AvailableFreeSpace { get; set; }
        public new string DriveFormat { get; set; }
        public new DriveType DriveType { get; set; }
        public new bool IsReady { get; protected set; }
        public override string Name { get; protected set; }

        public override DirectoryInfoBase RootDirectory
        {
            get
            {
                return mockFileDataAccessor.DirectoryInfo.FromDirectoryName(Name);
            }
        }

        public new long TotalFreeSpace { get; protected set; }
        public new long TotalSize { get; protected set; }
        public override string VolumeLabel { get; set; }
    }
}
