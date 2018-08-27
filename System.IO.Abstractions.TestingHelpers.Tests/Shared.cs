namespace System.IO.Abstractions.TestingHelpers.Tests
{
    internal static class Shared
    {
        /// <summary>
        /// These chars are not valid path chars but do not cause the same
        /// errors that other <code>Path.GetInvalidFileNameChars()</code> will.
        /// </summary>
        public static char[] SpecialInvalidPathChars(IFileSystem fileSystem) => new[]
        {
            // These are not allowed in a file name, but
            // inserting them a path does not make it invalid
            fileSystem.Path.DirectorySeparatorChar,
            fileSystem.Path.AltDirectorySeparatorChar,

            // Raises a different type of exception from other
            // invalid chars and is covered by other tests
            fileSystem.Path.VolumeSeparatorChar
        };
    }
}
