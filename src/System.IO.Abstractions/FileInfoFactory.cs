namespace System.IO.Abstractions
{
    [Serializable]
    internal class FileInfoFactory : IFileInfoFactory
    {
        public FileInfoBase FromFileName(string fileName)
        {
            var realFileInfo = new FileInfo(fileName);
            return new FileInfoWrapper(realFileInfo);
        }
    }
}