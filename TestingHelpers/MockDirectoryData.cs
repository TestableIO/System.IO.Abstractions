#if NET40
using System.Security.AccessControl;
#endif

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDirectoryData : MockFileData
    {
#if NET40
        [NonSerialized]

        private DirectorySecurity accessControl = new DirectorySecurity();
#endif
        public override bool IsDirectory { get { return true; } }

        public MockDirectoryData() : base(string.Empty)
        {
            Attributes = FileAttributes.Directory;
        }
#if NET40
        public new DirectorySecurity AccessControl
        {
            get { return accessControl; }
            set { accessControl = value; }
        }
#endif
    }
}