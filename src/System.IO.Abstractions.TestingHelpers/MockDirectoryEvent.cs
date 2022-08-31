namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// Notifies about a pending directory event.
    /// </summary>
    public class MockDirectoryEvent : MockFileSystemEvent
    {
        /// <summary>
        /// The path of the directory.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// The type of the directory event.
        /// </summary>
        public DirectoryEventType EventType { get; }

        internal MockDirectoryEvent(string path, DirectoryEventType eventType)
        {
            Path = path;
            EventType = eventType;
        }

        /// <summary>
        /// The type of the directory event.
        /// </summary>
        public enum DirectoryEventType
        {
            /// <summary>
            /// The directory is created.
            /// </summary>
            Created,
            /// <summary>
            /// The directory is deleted.
            /// </summary>
            Deleted
        }
    }
}
