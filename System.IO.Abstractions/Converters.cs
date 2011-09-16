using System.Collections.Generic;
using System.Linq;

namespace System.IO.Abstractions
{
    internal static class Converters
    {
        internal static FileSystemInfoBase[] Wrap(this FileSystemInfo[] input)
        {
            var results = new List<FileSystemInfoBase>();

            foreach (var item in input)
            {
                if (item is FileInfo)
                {
                    results.Add((FileInfoBase)item);
                    continue;
                }

                if (item is DirectoryInfo)
                {
                    results.Add((DirectoryInfoBase)item);
                    continue;
                }

                throw new NotImplementedException(string.Format(
                    "The type {0} is not recognized by the System.IO.Abstractions library.",
                    item.GetType().AssemblyQualifiedName
                ));
            }

            return results.ToArray();
        }

        internal static DirectoryInfoBase[] Wrap(this DirectoryInfo[] input)
        {
            return input.Select(f => (DirectoryInfoBase)f).ToArray();
        }

        internal static FileInfoBase[] Wrap(this FileInfo[] input)
        {
            return input.Select(f => (FileInfoBase)f).ToArray();
        }
    }
}