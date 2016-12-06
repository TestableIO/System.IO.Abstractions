namespace System.IO.Abstractions
{
    [Serializable]
    internal class DirectoryInfoFactory : IDirectoryInfoFactory
    {
        public DirectoryInfoBase FromDirectoryName(string directoryName)
        {
            var realDirectoryInfo = new DirectoryInfo(directoryName);
            return new DirectoryInfoWrapper(realDirectoryInfo);
        }
    }
}