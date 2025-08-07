namespace System.IO.Abstractions.TestingHelpers.Events;

/// <summary>
/// Represents the type of file system operation.
/// </summary>
public enum FileOperation
{
    /// <summary>
    /// File or directory creation operation.
    /// </summary>
    Create,
    
    /// <summary>
    /// File open operation.
    /// </summary>
    Open,
    
    /// <summary>
    /// File write operation.
    /// </summary>
    Write,
    
    /// <summary>
    /// File read operation.
    /// </summary>
    Read,
    
    /// <summary>
    /// File or directory deletion operation.
    /// </summary>
    Delete,
    
    /// <summary>
    /// File or directory move operation.
    /// </summary>
    Move,
    
    /// <summary>
    /// File or directory copy operation.
    /// </summary>
    Copy,
    
    /// <summary>
    /// Set attributes operation.
    /// </summary>
    SetAttributes,
    
    /// <summary>
    /// Set file times operation.
    /// </summary>
    SetTimes,
    
    /// <summary>
    /// Set permissions operation.
    /// </summary>
    SetPermissions
}