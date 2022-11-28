using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
    [Serializable]
    public class MockDirectoryData : MockFileData
    {
        /// <inheritdoc />
        public MockDirectoryData() : base(string.Empty)
        {
            Attributes = FileAttributes.Directory;
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public new DirectorySecurity AccessControl
        {
            get
            {
                // DirectorySecurity's constructor will throw PlatformNotSupportedException on non-Windows platform, so we initialize it in lazy way.
                // This let's us use this class as long as we don't use AccessControl property.
                var directorySecurity = Extensibility.RetrieveMetadata("AccessControl:DirectorySecurity") as DirectorySecurity;
                if (directorySecurity == null)
                {
                    directorySecurity = new DirectorySecurity();
                    Extensibility.StoreMetadata("AccessControl:DirectorySecurity", directorySecurity);
                }

                return directorySecurity;
            }
            set { Extensibility.StoreMetadata("AccessControl:DirectorySecurity", value); }
        }
    }
}