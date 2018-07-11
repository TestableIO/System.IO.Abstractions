using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.IO.Abstractions
{
    internal static class Converters
    {
        internal static IEnumerable<FileSystemInfoBase> WrapFileSystemInfos(this IEnumerable<FileSystemInfo> input)
            => input.Select(WrapFileSystemInfo);

        internal static FileSystemInfoBase[] WrapFileSystemInfos(this FileSystemInfo[] input)
            => input.Select(WrapFileSystemInfo).ToArray();

        internal static IEnumerable<DirectoryInfoBase> WrapDirectories(this IEnumerable<DirectoryInfo> input) 
            => input.Select(WrapDirectoryInfo);

        internal static DirectoryInfoBase[] WrapDirectories(this DirectoryInfo[] input)
            => input.Select(WrapDirectoryInfo).ToArray();

        internal static IEnumerable<FileInfoBase> WrapFiles(this IEnumerable<FileInfo> input) 
            => input.Select(WrapFileInfo);

        internal static FileInfoBase[] WrapFiles(this FileInfo[] input) 
            => input.Select(WrapFileInfo).ToArray();
        
        private static FileSystemInfoBase WrapFileSystemInfo(FileSystemInfo item)
        {
            if (item is FileInfo)
            {
                return WrapFileInfo((FileInfo)item);
            }
            else if (item is DirectoryInfo)
            {
                return WrapDirectoryInfo((DirectoryInfo)item);
            }
            else
            {
                throw new NotImplementedException(string.Format(
                    CultureInfo.InvariantCulture,
                    "The type {0} is not recognized by the System.IO.Abstractions library.",
                    item.GetType().AssemblyQualifiedName
                ));
            }
        }

        private static FileInfoBase WrapFileInfo(FileInfo f) => (FileInfoBase)f;
    
        private static DirectoryInfoBase WrapDirectoryInfo(DirectoryInfo d) => (DirectoryInfoBase)d;
    }
}