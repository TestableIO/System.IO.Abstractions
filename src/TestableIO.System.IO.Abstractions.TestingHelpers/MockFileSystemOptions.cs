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
    ///     The temporary directory used by the <see cref="MockFileSystem" />.
    ///     Defaults to <see cref="System.IO.Path.GetTempPath()" /> when <see langword="null" /> or empty.
    /// </summary>
    public string? TemporaryDirectory { get; init; }
}