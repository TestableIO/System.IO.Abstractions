using System.Diagnostics;

namespace System.IO.Abstractions
{
    /// <summary>
    /// A factory for the creation of wrappers for <see cref="FileVersionInfo" /> in a <see cref="IFileSystem" />.
    /// </summary>
    public interface IFileVersionInfoFactory : IFileSystemEntity
    {
        /// <inheritdoc cref="FileVersionInfo.GetVersionInfo(string)" />
        IFileVersionInfo GetVersionInfo(string fileName);
    }
}
