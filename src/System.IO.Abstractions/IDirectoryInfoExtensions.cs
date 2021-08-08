namespace System.IO.Abstractions
{
    public static class IDirectoryInfoExtensions
    {
        /// <summary>
        /// Get an <see cref="IDirectoryInfo"/> for the specified sub-directory <paramref name="name"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name">Sub-directory name (ex. "test")</param>
        /// <returns>An <see cref="IDirectoryInfo"/> for the specified sub-directory</returns>
        public static IDirectoryInfo SubDirectory(this IDirectoryInfo info, string name)
        {
            return info.FileSystem.DirectoryInfo.FromDirectoryName(info.FileSystem.Path.Combine(info.FullName, name));
        }

        /// <summary>
        /// Get an <see cref="IFileInfo"/> for the specified file <paramref name="name"/>
        /// </summary>
        /// <param name="info"></param>
        /// <param name="name">File name (ex. "test.txt")</param>
        /// <returns>An <see cref="IFileInfo"/> for the specified file</returns>
        public static IFileInfo File(this IDirectoryInfo info, string name)
        {
            return info.FileSystem.FileInfo.FromFileName(info.FileSystem.Path.Combine(info.FullName, name));
        }
    }
}
