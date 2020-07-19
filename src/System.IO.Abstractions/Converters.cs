using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.IO.Abstractions
{
    internal static class Converters
    {
        internal static IEnumerable<FileSystemInfoBase> WrapFileSystemInfos(this IEnumerable<FileSystemInfo> input, IFileSystem fileSystem)
            => input.Select(info => WrapFileSystemInfo(fileSystem, info));

        internal static FileSystemInfoBase[] WrapFileSystemInfos(this FileSystemInfo[] input, IFileSystem fileSystem)
            => input.Select(info => WrapFileSystemInfo(fileSystem, info)).ToArray();

        internal static IEnumerable<DirectoryInfoBase> WrapDirectories(this IEnumerable<DirectoryInfo> input, IFileSystem fileSystem)
            => input.Select(info => WrapDirectoryInfo(fileSystem, info));

        internal static DirectoryInfoBase[] WrapDirectories(this DirectoryInfo[] input, IFileSystem fileSystem)
            => input.Select(info => WrapDirectoryInfo(fileSystem, info)).ToArray();

        internal static IEnumerable<FileInfoBase> WrapFiles(this IEnumerable<FileInfo> input, IFileSystem fileSystem)
            => input.Select(info => WrapFileInfo(fileSystem, info));

        internal static FileInfoBase[] WrapFiles(this FileInfo[] input, IFileSystem fileSystem)
            => input.Select(info => WrapFileInfo(fileSystem, info)).ToArray();
        
        private static FileSystemInfoBase WrapFileSystemInfo(IFileSystem fileSystem, FileSystemInfo item)
        {
            if (item is FileInfo)
            {
                return WrapFileInfo(fileSystem, (FileInfo)item);
            }
            else if (item is DirectoryInfo)
            {
                return WrapDirectoryInfo(fileSystem, (DirectoryInfo)item);
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

        private static FileInfoBase WrapFileInfo(IFileSystem fileSystem, FileInfo f) => new FileInfoWrapper(fileSystem, f);

        private static DirectoryInfoBase WrapDirectoryInfo(IFileSystem fileSystem, DirectoryInfo d) => new DirectoryInfoWrapper(fileSystem, d);
    }
}
