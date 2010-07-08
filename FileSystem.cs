namespace System.IO.Abstractions
{
    public class FileSystem : IFileSystem
    {
        FileBase file;
        public FileBase File
        {
            get { return file ?? (file = new FileWrapper()); }
        }
    }
}