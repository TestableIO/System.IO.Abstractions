using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers
{
    public partial class MockDirectory : DirectoryBase
    {
        public override IEnumerable<string> EnumerateFiles(string path)
        {
            return GetDirectories(path);
        }

        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern)
        {
            return GetDirectories(path, searchPattern);
        }

        public override IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption)
        {
            return GetDirectories(path, searchPattern, searchOption);
        }
    }
}
