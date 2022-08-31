namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// Notifies about a pending file event.
    /// </summary>
    public class MockFileEvent
    {
        /// <summary>
        /// The path of the file.
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// The type of the file event.
        /// </summary>
        public FileEventType EventType { get; }

        internal MockFileEvent(string path, FileEventType changeType)
        {
            Path = path;
            EventType = changeType;
        }

        /// <summary>
        /// The type of the file event.
        /// </summary>
        public enum FileEventType
        {
            /// <summary>
            /// The file is created.
            /// </summary>
            Created,
            /// <summary>
            /// The file is updated.
            /// </summary>
            Updated,
            /// <summary>
            /// The file is deleted.
            /// </summary>
            Deleted
        }
    }
}
