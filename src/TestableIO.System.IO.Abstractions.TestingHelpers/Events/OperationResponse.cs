namespace System.IO.Abstractions.TestingHelpers.Events;

/// <summary>
/// Represents a response to a file system operation that can cancel or modify the operation.
/// </summary>
public class OperationResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation should be cancelled.
    /// Only applies to Before phase events.
    /// </summary>
    public bool Cancel { get; set; }
    
    /// <summary>
    /// Gets or sets an exception to throw instead of executing the operation.
    /// Only applies to Before phase events.
    /// </summary>
    public Exception Exception { get; set; }
}