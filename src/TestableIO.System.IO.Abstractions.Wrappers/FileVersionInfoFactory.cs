using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.IO.Abstractions
{
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    internal class FileVersionInfoFactory : IFileVersionInfoFactory
    {
        private readonly IFileSystem fileSystem;

        /// <inheritdoc />
        public FileVersionInfoFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <inheritdoc />
        public IFileSystem FileSystem => fileSystem;

        IFileVersionInfo IFileVersionInfoFactory.GetVersionInfo(string fileName)
        {
            return GetVersionInfo(fileName);
        }

        /// <inheritdoc cref="Diagnostics.FileVersionInfo.GetVersionInfo(string)" />
        public static IFileVersionInfo GetVersionInfo(string fileName)
        {
            Diagnostics.FileVersionInfo fileVersionInfo = Diagnostics.FileVersionInfo.GetVersionInfo(fileName);
            
            return new FileVersionInfoWrapper(fileVersionInfo);
        }
    }
}
