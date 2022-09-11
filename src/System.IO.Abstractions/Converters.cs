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

        internal static FileSystemInfoBase WrapFileSystemInfo(this FileSystemInfo input, IFileSystem fileSystem)
            => WrapFileSystemInfo(fileSystem, input);

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
            if (item is FileInfo fileInfo)
            {
                return WrapFileInfo(fileSystem, fileInfo);
            }
            else if (item is DirectoryInfo directoryInfo)
            {
                return WrapDirectoryInfo(fileSystem, directoryInfo);
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
