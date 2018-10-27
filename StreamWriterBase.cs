using System;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="StreamWriter"/>
    public class StreamWriterBase
    {
        protected StreamWriterBase(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }
    }
}
