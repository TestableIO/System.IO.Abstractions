namespace System.IO.Abstractions.TestingHelpers;

/// <summary>
///     Constructor options for <see cref="MockFileSystem" />
/// </summary>
public class MockFileSystemOptions
{
    /// <summary>
    ///     The <see cref="Directory.GetCurrentDirectory()" /> with which the <see cref="MockFileSystem" /> is initialized.
    /// </summary>
    public string CurrentDirectory { get; init; } = "";

    /// <summary>
    /// Flag indicating, if a temporary directory should be created.
    /// </summary>
    public bool CreateDefaultTempDir { get; init; } = true;

    /// <summary>
    /// Flag indicating whether file system events should be enabled.
    /// When false (default), the event system has zero overhead.
    /// </summary>
    public bool EnableEvents { get; init; }
}