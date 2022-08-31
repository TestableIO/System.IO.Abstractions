namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// Base class for mock file system events.
    /// </summary>
    public abstract class MockFileSystemEvent
    {
        /// <summary>
        /// By setting the <see cref="ExceptionToThrow"/> the corresponding exception will be thrown instead of changing the mock file system.
        /// </summary>
        public Exception ExceptionToThrow { get; set; }
    }
}
