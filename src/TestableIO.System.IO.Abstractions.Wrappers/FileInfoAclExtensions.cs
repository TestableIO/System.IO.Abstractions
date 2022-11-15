using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <summary>
    ///     ACL (access control list) extension methods for <see cref="IFileInfo" />.
    /// </summary>
    public static class FileInfoAclExtensions
    {
#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(FileInfo)"/>
#else
        /// <inheritdoc cref="FileInfo.GetAccessControl()"/>
#endif
        [SupportedOSPlatform("windows")]
        public static FileSecurity GetAccessControl(
            this IFileInfo fileInfo)
        {
            IFileSystemExtensibility extensibility =
                fileInfo.Extensibility;
            return extensibility.TryGetWrappedInstance(out FileInfo fi)
                ? fi.GetAccessControl()
                : extensibility.RetrieveMetadata<FileSecurity>(
                    "AccessControl:FileSecurity") ?? new FileSecurity();
        }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(FileInfo,AccessControlSections)"/>
#else
        /// <inheritdoc cref="File.GetAccessControl(string,AccessControlSections)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static FileSecurity GetAccessControl(
            this IFileInfo fileInfo,
            AccessControlSections includeSections)
        {
            IFileSystemExtensibility extensibility =
                fileInfo.Extensibility;
            return extensibility.TryGetWrappedInstance(out FileInfo fi)
                ? fi.GetAccessControl(includeSections)
                : extensibility.RetrieveMetadata<FileSecurity>(
                    "AccessControl:FileSecurity") ?? new FileSecurity();
        }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.SetAccessControl(FileInfo, FileSecurity)" />
#else
        /// <inheritdoc cref="FileInfo.SetAccessControl(FileSecurity)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static void SetAccessControl(this IFileInfo fileInfo,
            FileSecurity fileSecurity)
        {
            IFileSystemExtensibility extensibility =
                fileInfo.Extensibility;
            if (extensibility.TryGetWrappedInstance(out FileInfo fi))
            {
                fi.SetAccessControl(fileSecurity);
            }
            else
            {
                extensibility.StoreMetadata("AccessControl:FileSecurity",
                    fileSecurity);
            }
        }
    }
}
