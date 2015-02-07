using System.Collections.Generic;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class FileSystemInfoBaseEqualityComparer : IEqualityComparer<FileSystemInfoBase>
    {
        public bool Equals(FileSystemInfoBase x, FileSystemInfoBase y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            var typeOfX = x.GetType();
            var typeOfY = y.GetType();
            if (typeOfX != typeOfY)
            {
                return false;
            }

            return string.Equals(x.FullName, y.FullName, StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(FileSystemInfoBase obj)
        {
            return obj.FullName.GetHashCode();
        }
    }
}
