namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockDirectoryData : MockFileData
    {
        public override bool IsDirectory { get { return true; } }

        public MockDirectoryData() : base(string.Empty)
        {
            Attributes = FileAttributes.Directory;
        }
    }
}