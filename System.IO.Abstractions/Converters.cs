using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace System.IO.Abstractions
{
    
    internal static class Converters
    {

        /// <summary>
        /// Function that converts System.IO.FileSystemInfo to System.IO.Abstractions.FileSystemInfoBase
        /// </summary>
        internal static Func<FileSystemInfo, FileSystemInfoBase> AsFileSystemInfoBase= new Func<FileSystemInfo,FileSystemInfoBase>(item =>
             {
                 if(item is FileInfo)
                     return (FileInfoBase)item;

                 if(item is DirectoryInfo)
                     return (DirectoryInfoBase)item;

                 throw new NotImplementedException(string.Format(
                     System.Globalization.CultureInfo.InvariantCulture,
                     "The type {0} is not recognized by the System.IO.Abstractions library.",
                     item.GetType().AssemblyQualifiedName
                 ));
             });

    internal static FileSystemInfoBase[] WrapFileSystemInfos(this IEnumerable<FileSystemInfo> input)
        {
            return input
                .Select<FileSystemInfo, FileSystemInfoBase>(AsFileSystemInfoBase)
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