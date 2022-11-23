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
            IFileSystemAclSupport aclSupport = directoryInfo as IFileSystemAclSupport;
            if (aclSupport == null)
            {
                throw new NotSupportedException("The directory info does not support ACL extensions");
            }

            directoryInfo.Create();
            aclSupport.SetAccessControl(directorySecurity);
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
            IFileSystemAclSupport aclSupport = directoryInfo as IFileSystemAclSupport;
            var value = aclSupport?.GetAccessControl();
            if (aclSupport == null || value is not DirectorySecurity directorySecurity)
            {
                throw new NotSupportedException("The directory info does not support ACL extensions");
            }

            return directorySecurity;
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
            IFileSystemAclSupport aclSupport = directoryInfo as IFileSystemAclSupport;
            var value = aclSupport?.GetAccessControl((IFileSystemAclSupport.AccessControlSections) includeSections);
            if (aclSupport == null || value is not DirectorySecurity directorySecurity)
            {
                throw new NotSupportedException("The directory info does not support ACL extensions");
            }

            return directorySecurity;
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
            IFileSystemAclSupport aclSupport = directoryInfo as IFileSystemAclSupport;
            if (aclSupport == null)
            {
                throw new NotSupportedException("The directory info does not support ACL extensions");
            }
            
            aclSupport.SetAccessControl(directorySecurity);
        }
    }
}
