namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDriveInfo : DriveInfoBase
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;

        public MockDriveInfo(IMockFileDataAccessor mockFileDataAccessor, string name) : base(mockFileDataAccessor?.FileSystem)
        {
            if (mockFileDataAccessor == null)
            {
                throw new ArgumentNullException(nameof(mockFileDataAccessor));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            const string DRIVE_SEPARATOR = @":\";
            if (name.Length == 1)
            {
                name = char.ToUpperInvariant(name[0]) + DRIVE_SEPARATOR;
            }
            else if (name.Length == 2 && name[1] == ':')
            {
                name = char.ToUpperInvariant(name[0]) + DRIVE_SEPARATOR;
            }
            else if (name.Length == 3 && name.EndsWith(DRIVE_SEPARATOR, StringComparison.Ordinal))
            {
                name = char.ToUpperInvariant(name[0]) + DRIVE_SEPARATOR;
            }
            else
            {
                MockPath.CheckInvalidPathChars(name);
                name = mockFileDataAccessor.Path.GetPathRoot(name);

                if (string.IsNullOrEmpty(name) || name.StartsWith(@"\\", StringComparison.Ordinal))
                {
                    throw new ArgumentException(
                        @"Object must be a root directory (""C:\"") or a drive letter (""C"").");
                }
            }

            this.mockFileDataAccessor = mockFileDataAccessor;

            Name = name;
            IsReady = true;
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
                var directory = mockFileDataAccessor.DirectoryInfo.FromDirectoryName(Name);
                return directory;
            }
        }

        public new long TotalFreeSpace { get; protected set; }
        public new long TotalSize { get; protected set; }
        public override string VolumeLabel { get; set; }
    }
}
