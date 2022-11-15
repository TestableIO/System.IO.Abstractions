using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions
{
    /// <summary>
    ///     ACL (access control list) extension methods for <see cref="IDirectory" />.
    /// </summary>
    public static class DirectoryAclExtensions
    {
#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="System.IO.FileSystemAclExtensions.CreateDirectory(DirectorySecurity, string)" />
#else
        /// <inheritdoc cref="Directory.CreateDirectory(string, DirectorySecurity)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static void CreateDirectory(this IDirectory directory,
            string path,
            DirectorySecurity directorySecurity)
        {
            IDirectoryInfo directoryInfo =
                directory.FileSystem.DirectoryInfo.New(path);
            IFileSystemExtensibility extensibility =
                directoryInfo.Extensibility;
            if (extensibility.TryGetWrappedInstance(out DirectoryInfo di))
            {
                di.Create(directorySecurity);
            }
            else
            {
                extensibility.StoreMetadata("AccessControl:Directory",
                    directorySecurity);
                directoryInfo.Create();
            }
        }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(DirectoryInfo)"/>
#else
        /// <inheritdoc cref="Directory.GetAccessControl(string)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static DirectorySecurity GetAccessControl(
            this IDirectory directory, string path)
        {
            IDirectoryInfo directoryInfo =
                directory.FileSystem.DirectoryInfo.New(path);
            IFileSystemExtensibility extensibility =
                directoryInfo.Extensibility;
            return extensibility.TryGetWrappedInstance(out DirectoryInfo di)
                ? di.GetAccessControl()
                : extensibility.RetrieveMetadata<DirectorySecurity>(
                    "AccessControl:Directory") ?? new DirectorySecurity();
        }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(DirectoryInfo)"/>
#else
        /// <inheritdoc cref="Directory.GetAccessControl(string, AccessControlSections)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static DirectorySecurity GetAccessControl(
            this IDirectory directory,
            string path,
            AccessControlSections includeSections)
        {
            IDirectoryInfo directoryInfo =
                directory.FileSystem.DirectoryInfo.New(path);
            IFileSystemExtensibility extensibility =
                directoryInfo.Extensibility;
            return extensibility.TryGetWrappedInstance(out DirectoryInfo di)
                ? di.GetAccessControl(includeSections)
                : extensibility.RetrieveMetadata<DirectorySecurity>(
                    "AccessControl:Directory") ?? new DirectorySecurity();
        }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.SetAccessControl(DirectoryInfo, DirectorySecurity)"/>
#else
        /// <inheritdoc cref="Directory.SetAccessControl(string, DirectorySecurity)"/>
#endif
        [SupportedOSPlatform("windows")]
        public static void SetAccessControl(this IDirectory directory,
            string path,
            DirectorySecurity directorySecurity)
        {
            IDirectoryInfo directoryInfo =
                directory.FileSystem.DirectoryInfo.New(path);
            IFileSystemExtensibility extensibility =
                directoryInfo.Extensibility;
            if (extensibility.TryGetWrappedInstance(out DirectoryInfo di))
            {
                di.SetAccessControl(directorySecurity);
            }
            else
            {
                extensibility.StoreMetadata("AccessControl:Directory",
                    directorySecurity);
            }
        }
    }
}