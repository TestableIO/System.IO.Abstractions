namespace System.IO.Abstractions.TestingHelpers.Events;

/// <summary>
/// Represents the phase of an operation.
/// </summary>
public enum OperationPhase
{
    /// <summary>
    /// Before the operation is executed. Allows cancellation or throwing exceptions.
    /// </summary>
    Before,
    
    /// <summary>
    /// After the operation has been executed. For notification only.
    /// </summary>
    After
}