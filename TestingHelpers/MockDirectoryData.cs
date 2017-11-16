using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDirectoryData : MockFileData
    {
        [NonSerialized]
        private DirectorySecurity accessControl = new DirectorySecurity();
        
        public override bool IsDirectory { get { return true; } }

        public MockDirectoryData() : base(string.Empty)
        {
            Attributes = FileAttributes.Directory;
        }
        
        public new DirectorySecurity AccessControl
        {
            get { return accessControl; }
            set { accessControl = value; }
        }
    }
}