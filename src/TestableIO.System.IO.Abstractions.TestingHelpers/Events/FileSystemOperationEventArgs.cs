namespace System.IO.Abstractions.TestingHelpers.Events;

/// <summary>
/// Provides data for file system operation events.
/// </summary>
public class FileSystemOperationEventArgs : EventArgs
{
    private OperationResponse response;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FileSystemOperationEventArgs"/> class.
    /// </summary>
    /// <param name="path">The path of the resource being operated on.</param>
    /// <param name="operation">The type of operation.</param>
    /// <param name="resourceType">The type of resource.</param>
    /// <param name="phase">The phase of the operation.</param>
    public FileSystemOperationEventArgs(
        string path, 
        FileOperation operation, 
        ResourceType resourceType, 
        OperationPhase phase)
    {
        Path = path ?? throw new ArgumentNullException(nameof(path));
        Operation = operation;
        ResourceType = resourceType;
        Phase = phase;
    }
    
    /// <summary>
    /// Gets the path of the resource being operated on.
    /// </summary>
    public string Path { get; }
    
    /// <summary>
    /// Gets the type of operation being performed.
    /// </summary>
    public FileOperation Operation { get; }
    
    /// <summary>
    /// Gets the type of resource being operated on.
    /// </summary>
    public ResourceType ResourceType { get; }
    
    /// <summary>
    /// Gets the phase of the operation.
    /// </summary>
    public OperationPhase Phase { get; }
    
    /// <summary>
    /// Sets a response for the operation. Only valid for Before phase events.
    /// </summary>
    /// <param name="response">The response to set.</param>
    /// <exception cref="InvalidOperationException">Thrown when called on an After phase event.</exception>
    public void SetResponse(OperationResponse response)
    {
        if (Phase != OperationPhase.Before)
        {
            throw new InvalidOperationException("Response can only be set for Before phase events.");
        }
        
        this.response = response ?? throw new ArgumentNullException(nameof(response));
    }
    
    /// <summary>
    /// Gets the response set for this operation, if any.
    /// </summary>
    internal OperationResponse GetResponse() => response;
}