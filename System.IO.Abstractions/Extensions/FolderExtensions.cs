namespace System.IO.Abstractions.Extensions
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Abstractions;
    using System.Text.RegularExpressions;

    using JetBrains.Annotations;

    public static class FolderExtensions
    {
        [NotNull]
        public static IFile GetChildFile([NotNull] this IDirectory folder, [NotNull] string fileName)
        {
            return folder.FileSystem.ParseFile(folder.FileSystem.Internals.Path.Combine(folder.FullName, fileName));
        }

        [NotNull]
        public static IDirectory GetChildDirectory([NotNull] this IDirectory folder, [NotNull] string folderName)
        {
            return folder.FileSystem.ParseDirectory(folder.FileSystem.Internals.Path.Combine(folder.FullName, folderName));
        }

        [NotNull]
        public static IDirectory CreateSubfolder([NotNull] this IDirectory folder, [NotNull] string folderName)
        {
            var subfolder = GetChildDirectory(folder, folderName);
            subfolder.Create();

            return subfolder;
        }

        public static void ReplaceLine(this IFile file, string pattern, string replacement)
        {
            var regex = new Regex(pattern, RegexOptions.Compiled);
            var lines = new List<string>();
            using (var reader = new StreamReader(file.Open(FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                while (reader.Peek() >= 0)
                {
                    var line = reader.ReadLine();

                    lines.Add(regex.Replace(line, replacement));
                }
            }

            using (var writer = new StreamWriter(file.Open(FileMode.Open, FileAccess.Write, FileShare.None)))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}