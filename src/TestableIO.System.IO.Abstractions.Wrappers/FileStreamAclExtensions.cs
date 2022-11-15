using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <summary>
    ///     ACL (access control list) extension methods for <see cref="FileSystemStream" />.
    /// </summary>
    public static class FileStreamAclExtensions
    {
#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(FileStream)" />
#else
        /// <inheritdoc cref="FileStream.GetAccessControl()"/>
#endif
        [SupportedOSPlatform("windows")]
        public static FileSecurity GetAccessControl(this FileSystemStream fileStream)
        {
            IFileSystemExtensibility extensibility =
                fileStream.Extensibility;
            return extensibility.TryGetWrappedInstance(out FileStream fs)
                ? fs.GetAccessControl()
                : extensibility.RetrieveMetadata<FileSecurity>(
                    "AccessControl") ?? new FileSecurity();
        }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.SetAccessControl(FileStream, FileSecurity)" />
#else
        /// <inheritdoc cref="FileStream.SetAccessControl(FileSecurity)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static void SetAccessControl(this FileSystemStream fileStream,
            FileSecurity fileSecurity)
        {
            IFileSystemExtensibility extensibility =
                fileStream.Extensibility;
            if (extensibility.TryGetWrappedInstance(out FileStream fs))
            {
                fs.SetAccessControl(fileSecurity);
            }
            else
            {
                extensibility.StoreMetadata("AccessControl",
                    fileSecurity);
            }
        }
    }
}