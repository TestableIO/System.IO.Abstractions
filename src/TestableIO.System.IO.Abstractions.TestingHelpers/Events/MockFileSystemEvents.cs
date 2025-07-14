using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace System.IO.Abstractions.TestingHelpers.Events;

/// <summary>
/// Provides event functionality for MockFileSystem operations.
/// </summary>
/// <example>
/// <code>
/// // Subscribe to all operations
/// var fs = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
/// var subscription = fs.Events.Subscribe(args => 
/// {
///     Console.WriteLine($"{args.Operation} {args.Path}");
/// });
/// 
/// // Subscribe to specific operations
/// var writeSub = fs.Events.Subscribe(FileOperation.Write, args =>
/// {
///     if (args.Phase == OperationPhase.Before)
///     {
///         args.SetResponse(new OperationResponse 
///         { 
///             Exception = new IOException("Disk full") 
///         });
///     }
/// });
/// 
/// // Unsubscribe when done
/// subscription.Dispose();
/// </code>
/// </example>
#if FEATURE_SERIALIZABLE
[Serializable]
#endif
public class MockFileSystemEvents
{
#if FEATURE_SERIALIZABLE
    /// <summary>
    /// Represents the collection of active event subscriptions for handling file system operations.
    /// </summary>
    [NonSerialized]
#endif
    private readonly List<Subscription> subscriptions = [];

    /// <summary>
    /// Indicates whether event handling is currently enabled for file system operations.
    /// </summary>
    private volatile bool isEnabled;

    /// <summary>
    /// Tracks the current version of active subscriptions, allowing for the detection of changes
    /// such as additions or removals of subscription handlers.
    /// </summary>
    private volatile int subscriptionVersion;
    
    /// <summary>
    /// Gets the synchronization object used to manage thread safety for event handling operations.
    /// </summary>
    private object LockObject { get; } = new object();

    /// <summary>
    /// Gets a value indicating whether events are enabled.
    /// </summary>
    public bool IsEnabled => isEnabled;
    
    /// <summary>
    /// Subscribes to all file system operations.
    /// </summary>
    /// <param name="handler">The handler to invoke for each operation.</param>
    /// <returns>A disposable that removes the subscription when disposed.</returns>
    public IDisposable Subscribe(Action<FileSystemOperationEventArgs> handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }
            
        lock (LockObject)
        {
            var subscription = new Subscription(handler, null, this);
            subscriptions.Add(subscription);
            Interlocked.Increment(ref subscriptionVersion);
            return subscription;
        }
    }
    
    /// <summary>
    /// Subscribes to a specific file system operation.
    /// </summary>
    /// <param name="operation">The operation to subscribe to.</param>
    /// <param name="handler">The handler to invoke for the operation.</param>
    /// <returns>A disposable that removes the subscription when disposed.</returns>
    public IDisposable Subscribe(FileOperation operation, Action<FileSystemOperationEventArgs> handler)
    {
        if (handler == null) {
            throw new ArgumentNullException(nameof(handler));
        }
            
        lock (LockObject)
        {
            var subscription = new Subscription(handler, new HashSet<FileOperation> { operation }, this);
            subscriptions.Add(subscription);
            Interlocked.Increment(ref subscriptionVersion);
            return subscription;
        }
    }
    
    /// <summary>
    /// Subscribes to multiple specific file system operations.
    /// </summary>
    /// <param name="operations">The operations to subscribe to.</param>
    /// <param name="handler">The handler to invoke for the operations.</param>
    /// <returns>A disposable that removes the subscription when disposed.</returns>
    public IDisposable Subscribe(FileOperation[] operations, Action<FileSystemOperationEventArgs> handler)
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler));
        if (operations == null)
        {
            throw new ArgumentNullException(nameof(operations));
        }
            
        lock (LockObject)
        {
            var subscription = new Subscription(handler, new HashSet<FileOperation>(operations), this);
            subscriptions.Add(subscription);
            Interlocked.Increment(ref subscriptionVersion);
            return subscription;
        }
    }

    /// <summary>
    /// Enables event handling for file system operations in the mock file system.
    /// </summary>
    internal void Enable() => isEnabled = true;
    
    /// <summary>
    /// Wraps an operation with Before/After events, handling exceptions properly.
    /// </summary>
    /// <typeparam name="T">The return type of the operation</typeparam>
    /// <param name="path">The file path</param>
    /// <param name="operation">The type of operation</param>
    /// <param name="resourceType">The type of resource</param>
    /// <param name="func">The operation to execute</param>
    /// <returns>The result of the operation</returns>
    public T WithEvents<T>(string path, FileOperation operation, ResourceType resourceType, Func<T> func)
    {
        if (!isEnabled)
        {
            return func();
        }
            
        RaiseOperation(path, operation, resourceType, OperationPhase.Before);
        
        var operationCompleted = false;
        try
        {
            var result = func();
            operationCompleted = true;
            return result;
        }
        finally
        {
            if (operationCompleted)
            {
                try
                {
                    RaiseOperation(path, operation, resourceType, OperationPhase.After);
                }
                catch
                {
                    // Don't let After event exceptions mask the main operation
                }
            }
        }
    }
    
    /// <summary>
    /// Wraps a void operation with Before/After events, handling exceptions properly.
    /// </summary>
    /// <param name="path">The file path</param>
    /// <param name="operation">The type of operation</param>
    /// <param name="resourceType">The type of resource</param>
    /// <param name="action">The operation to execute</param>
    public void WithEvents(string path, FileOperation operation, ResourceType resourceType, Action action)
    {
        if (!isEnabled)
        {
            action();
            return;
        }
            
        RaiseOperation(path, operation, resourceType, OperationPhase.Before);
        
        var operationCompleted = false;
        try
        {
            action();
            operationCompleted = true;
        }
        finally
        {
            if (operationCompleted)
            {
                try
                {
                    RaiseOperation(path, operation, resourceType, OperationPhase.After);
                }
                catch
                {
                    // Don't let After event exceptions mask the main operation
                }
            }
        }
    }
    
    /// <summary>
    /// Raises an operation event if events are enabled.
    /// </summary>
    internal void RaiseOperation(
        string path, 
        FileOperation operation, 
        ResourceType resourceType, 
        OperationPhase phase)
    {
        if (!isEnabled) {
            return;
        }
            
        var args = new FileSystemOperationEventArgs(path, operation, resourceType, phase);
        Subscription[] currentSubscriptions;
        
        lock (LockObject)
        {
            if (subscriptions.Count == 0)
            {
                return;
            }
            currentSubscriptions = subscriptions.ToArray();
        }
        
        var exceptions = new List<Exception>();
        
        foreach (var sub in currentSubscriptions.Where(sub => !sub.IsDisposed && sub.ShouldHandle(operation)))
        {
            try
            {
                sub.Handler(args);
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
        
        if (exceptions.Count > 0)
        {
            if (exceptions.Count == 1)
            {
                throw exceptions[0];
            }
            throw new AggregateException(
                $"One or more event handlers failed for {operation} operation on {path}", 
                exceptions);
        }
        
        // Handle responses only for Before phase
        if (phase != OperationPhase.Before)
        {
            return;
        }
        var response = args.GetResponse();
        if (response == null)
        {
            return;
        }

        if (response.Exception != null)
        {
            throw response.Exception;
        }

        if (response.Cancel)
        {
            throw new OperationCanceledException(
                $"Operation {operation} on {path} was cancelled.");
        }
    }

    /// <summary>
    /// Removes the specified subscription from the list of active subscriptions.
    /// </summary>
    /// <param name="subscription">The subscription to be removed.</param>
    private void RemoveSubscription(Subscription subscription)
    {
        lock (LockObject)
        {
            if (!subscriptions.Remove(subscription))
            {
                return;
            }
            Interlocked.Increment(ref subscriptionVersion);
        }
    }
    
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    private class Subscription : IDisposable
    {
#if FEATURE_SERIALIZABLE
        /// <summary>
        /// Represents a reference to the parent <see cref="MockFileSystemEvents"/> instance
        /// that manages the subscriptions and events for the file system operations.
        /// </summary>
        [NonSerialized]
#endif
        private readonly MockFileSystemEvents parent;
        /// <summary>
        /// Specifies the set of file system operations that this subscription filters on.
        /// </summary>
        /// <remarks>
        /// When set, only the specified file operations will trigger the event handler.
        /// If null, all file system operations are considered for this subscription.
        /// </remarks>
        private readonly HashSet<FileOperation> filterOperations;

        /// <summary>
        /// Indicates whether the subscription has been disposed.
        /// </summary>
        private volatile bool isDisposed;
        
#if FEATURE_SERIALIZABLE
        [NonSerialized]
#endif
        public readonly Action<FileSystemOperationEventArgs> Handler;
        /// <summary>
        /// Gets a value indicating whether this subscription has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get
            {
                return isDisposed;
            }
        }

        /// <summary>
        /// Represents a subscription to file system events within a mock file system.
        /// </summary>
        /// <remarks>
        /// A subscription tracks a handler and optional filters for specific file operations and
        /// connects to a parent <see cref="MockFileSystemEvents"/> instance.
        /// </remarks>
        public Subscription(Action<FileSystemOperationEventArgs> handler,
            HashSet<FileOperation> filterOperations, MockFileSystemEvents parent)
        {
            Handler = handler ?? throw new ArgumentNullException(nameof(handler));
            this.filterOperations = filterOperations;
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        /// <summary>
        /// Determines whether the specified file operation should be handled based on the defined filters.
        /// </summary>
        /// <param name="operation">The file operation to evaluate.</param>
        /// <returns><c>true</c> if the operation should be handled; otherwise, <c>false</c>.</returns>
        public bool ShouldHandle(FileOperation operation)
        {
            return filterOperations == null || filterOperations.Contains(operation);
        }

        /// <summary>
        /// Disposes of this subscription, unregistering it from the parent <see cref="MockFileSystemEvents"/> instance.
        /// </summary>
        /// <remarks>
        /// Once disposed, the subscription is no longer valid and will be removed from the list of active subscriptions.
        /// </remarks>
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }
            isDisposed = true;
            parent.RemoveSubscription(this);
        }
    }
}
