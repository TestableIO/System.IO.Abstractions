namespace System.IO.Abstractions.TestingHelpers.Events;

/// <summary>
/// Represents the type of file system resource.
/// </summary>
public enum ResourceType
{
    /// <summary>
    /// The resource is a file.
    /// </summary>
    File,
    
    /// <summary>
    /// The resource is a directory.
    /// </summary>
    Directory
}