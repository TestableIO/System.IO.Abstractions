using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.IO.Abstractions
{
    internal static class Converters
    {
        internal static FileSystemInfoBase[] WrapFileSystemInfos(this IEnumerable<FileSystemInfo> input)
        {
            return input
                .Select<FileSystemInfo, FileSystemInfoBase>(item =>
                {
                    if (item is FileInfo)
                        return (FileInfoBase) item;

                    if (item is DirectoryInfo)
                        return (DirectoryInfoBase) item;

                    throw new NotImplementedException(string.Format(
                        CultureInfo.InvariantCulture,
                        "The type {0} is not recognized by the System.IO.Abstractions library.",
                        item.GetType().AssemblyQualifiedName
                    ));
                })
                .ToArray();
        }

        internal static DirectoryInfoBase[] WrapDirectories(this IEnumerable<DirectoryInfo> input)
        {
            return input.Select(f => (DirectoryInfoBase)f).ToArray();
        }

        internal static FileInfoBase[] WrapFiles(this IEnumerable<FileInfo> input)
        {
            return input.Select(f => (FileInfoBase)f).ToArray();
        }
    }
}