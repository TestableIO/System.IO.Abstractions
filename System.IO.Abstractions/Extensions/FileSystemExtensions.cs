namespace System.IO.Abstractions.Extensions
{
    using System.IO;
    using System.IO.Abstractions;
    using System.Linq;

    using JetBrains.Annotations;

    public static class FileSystemExtensions
    {
        [NotNull]
        public static IDirectory ParseDirectory(this IFileSystem fileSystem, DirectoryInfo dir)
        {
            return ParseDirectory(fileSystem, dir.FullName);
        }

        [NotNull]
        public static IFile ParseFile(this IFileSystem fileSystem, FileInfo file)
        {
            return fileSystem.ParseFile(file.FullName);
        }

        [NotNull]
        public static IDirectory ParseDirectory([NotNull] this IFileSystem fileSystem, [NotNull] params string[] pathSegments)
        {
            return ParseDirectory(fileSystem, Path.Combine(pathSegments));
        }

        [NotNull]
        public static IDirectory ParseDirectory([NotNull] this IFileSystem fileSystem, [NotNull] IDirectory folder, [NotNull] params string[] pathSegments)
        {
            return ParseDirectory(fileSystem, Path.Combine(pathSegments.Concat(new [] { folder.FullName }).ToArray()));
        }

        [NotNull]
        public static IFile ParseFile([NotNull] this IFileSystem fileSystem, [NotNull] params string[] pathSegments)
        {
            return fileSystem.ParseFile(Path.Combine(pathSegments));
        }

        [NotNull]
        public static IFile ParseFile([NotNull] this IFileSystem fileSystem, [NotNull] IDirectory folder, [NotNull] params string[] pathSegments)
        {
            return fileSystem.ParseFile(Path.Combine(pathSegments.Concat(new[] { folder.FullName }).ToArray()));
        }

        public static IUniqueTempDirectory CreateTempFolder([NotNull] this IFileSystem fileSystem)
        {
            var folder = new UniqueTempDirectory(fileSystem);

            return folder;
        }
    }
}
