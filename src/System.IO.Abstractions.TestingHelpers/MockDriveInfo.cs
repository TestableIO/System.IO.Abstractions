namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
    [Serializable]
    public class MockDriveInfo : DriveInfoBase
    {
        private readonly IMockFileDataAccessor mockFileDataAccessor;

        /// <inheritdoc />
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
                || (name.Length == 3 && mockFileDataAccessor.StringOperations.EndsWith(name, DRIVE_SEPARATOR)))
            {
                name = name[0] + DRIVE_SEPARATOR;
            }
            else
            {
                mockFileDataAccessor.PathVerifier.CheckInvalidPathChars(name);
                name = mockFileDataAccessor.Path.GetPathRoot(name);

                if (string.IsNullOrEmpty(name) || mockFileDataAccessor.StringOperations.StartsWith(name, @"\\"))
                {
                    throw new ArgumentException(
                        @"Object must be a root directory (""C:\"") or a drive letter (""C"").");
                }
            }

            Name = name;
            IsReady = true;
        }

        /// <inheritdoc />
        public new long AvailableFreeSpace { get; set; }
        /// <inheritdoc />
        public new string DriveFormat { get; set; }
        /// <inheritdoc />
        public new DriveType DriveType { get; set; }
        /// <inheritdoc />
        public new bool IsReady { get; protected set; }
        /// <inheritdoc />
        public override string Name { get; protected set; }

        /// <inheritdoc />
        public override IDirectoryInfo RootDirectory
        {
            get
            {
                return mockFileDataAccessor.DirectoryInfo.FromDirectoryName(Name);
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Name;
        }

        /// <inheritdoc />
        public new long TotalFreeSpace { get; protected set; }
        /// <inheritdoc />
        public new long TotalSize { get; protected set; }
        /// <inheritdoc />
        public override string VolumeLabel { get; set; }
    }
}
