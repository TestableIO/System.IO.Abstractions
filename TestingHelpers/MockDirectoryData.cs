namespace System.IO.Abstractions.TestingHelpers
{
#if NET40
    [Serializable]
#endif
    public class MockDirectoryData : MockFileData {
        public override bool IsDirectory { get { return true; } }

        public MockDirectoryData() : base(string.Empty)
        {
            Attributes = FileAttributes.Directory;
        }
    }
}