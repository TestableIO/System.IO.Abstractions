namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    ///     Constructor options for <see cref="MockFileSystem" />
    /// </summary>
    public class MockFileSystemOptions
    {
        /// <summary>
        ///     The <see cref="Directory.GetCurrentDirectory()" /> with which the <see cref="MockFileSystem" /> is initialized.
        /// </summary>
        public string CurrentDirectory { get; set; } = "";

        /// <summary>
        /// Flag indicating, if a temporary directory should be created.
        /// </summary>
        public bool CreateDefaultTempDir { get; set; } = true;
    }
}
