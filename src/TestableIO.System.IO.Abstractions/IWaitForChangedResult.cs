namespace System.IO.Abstractions
{
    /// <summary>
    ///     Abstractions for <see cref="WaitForChangedResult" />.
    /// </summary>
    public interface IWaitForChangedResult
    {
        /// <inheritdoc cref="WaitForChangedResult.ChangeType" />
        WatcherChangeTypes ChangeType { get; }

        /// <inheritdoc cref="WaitForChangedResult.Name" />
        string? Name { get; }

        /// <inheritdoc cref="WaitForChangedResult.OldName" />
        string? OldName { get; }

        /// <inheritdoc cref="WaitForChangedResult.TimedOut" />
        bool TimedOut { get; }
    }
}