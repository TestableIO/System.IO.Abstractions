using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDirectoryData : MockFileData
    {
        [NonSerialized]
        private DirectorySecurity accessControl;
        
        public override bool IsDirectory { get { return true; } }

        public MockDirectoryData() : base(string.Empty)
        {
            Attributes = FileAttributes.Directory;
        }

        public new DirectorySecurity AccessControl
        {
            get
            {
                // DirectorySecurity's constructor will throw PlatformNotSupportedException on non-Windows platform, so we initialize it in lazy way.
                // This let's us use this class as long as we don't use AccessControl property.
                return accessControl ?? (accessControl = new DirectorySecurity());
            }
            set { accessControl = value; }
        }
    }
}