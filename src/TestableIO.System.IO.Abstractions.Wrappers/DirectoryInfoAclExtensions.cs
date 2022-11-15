using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <summary>
    ///     ACL (access control list) extension methods for <see cref="IDirectoryInfo" />.
    /// </summary>
    public static class DirectoryInfoAclExtensions
    {
#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.Create(DirectoryInfo,DirectorySecurity)"/>
#else
        /// <inheritdoc cref="DirectoryInfo.Create(DirectorySecurity)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static void Create(this IDirectoryInfo directoryInfo,
            DirectorySecurity directorySecurity)
        {
            IFileSystemExtensibility extensibility =
                directoryInfo.Extensibility;
            if (extensibility.TryGetWrappedInstance(out DirectoryInfo di))
            {
                di.Create(directorySecurity);
            }
            else
            {
                directoryInfo.Create();
                directoryInfo.Extensibility.StoreMetadata("AccessControl:DirectorySecurity",
                    directorySecurity);
            }
        }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(DirectoryInfo)"/>
#else
        /// <inheritdoc cref="DirectoryInfo.GetAccessControl()"/>
#endif
        [SupportedOSPlatform("windows")]
        public static DirectorySecurity GetAccessControl(
            this IDirectoryInfo directoryInfo)
        {
            IFileSystemExtensibility extensibility =
                directoryInfo.Extensibility;
            return extensibility.TryGetWrappedInstance(out DirectoryInfo di)
                ? di.GetAccessControl()
                : extensibility.RetrieveMetadata<DirectorySecurity>(
                    "AccessControl:DirectorySecurity") ?? new DirectorySecurity();
        }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(DirectoryInfo,AccessControlSections)"/>
#else
        /// <inheritdoc cref="DirectoryInfo.GetAccessControl(AccessControlSections)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static DirectorySecurity GetAccessControl(
            this IDirectoryInfo directoryInfo,
            AccessControlSections includeSections)
        {
            IFileSystemExtensibility extensibility =
                directoryInfo.Extensibility;
            return extensibility.TryGetWrappedInstance(out DirectoryInfo di)
                ? di.GetAccessControl(includeSections)
                : extensibility.RetrieveMetadata<DirectorySecurity>(
                    "AccessControl:DirectorySecurity") ?? new DirectorySecurity();
        }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.SetAccessControl(DirectoryInfo,DirectorySecurity)"/>
#else
        /// <inheritdoc cref="DirectoryInfo.SetAccessControl(DirectorySecurity)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static void SetAccessControl(this IDirectoryInfo directoryInfo,
            DirectorySecurity directorySecurity)
        {
            IFileSystemExtensibility extensibility =
                directoryInfo.Extensibility;
            if (extensibility.TryGetWrappedInstance(out DirectoryInfo di))
            {
                di.SetAccessControl(directorySecurity);
            }
            else
            {
                extensibility.StoreMetadata("AccessControl:DirectorySecurity",
                    directorySecurity);
            }
        }
    }
}
