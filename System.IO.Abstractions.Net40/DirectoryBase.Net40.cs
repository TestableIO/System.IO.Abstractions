using System.Collections.Generic;

namespace System.IO.Abstractions
{
    public abstract partial class DirectoryBase
    {
        public abstract IEnumerable<string> EnumerateFiles(string path);
        public abstract IEnumerable<string> EnumerateFiles(string path, string searchPattern);
        public abstract IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption);
    }
}
