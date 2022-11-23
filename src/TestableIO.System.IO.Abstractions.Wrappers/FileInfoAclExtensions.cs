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
            IFileSystemAclSupport aclSupport = fileInfo as IFileSystemAclSupport;
            var value = aclSupport?.GetAccessControl();
            if (aclSupport == null || value is not FileSecurity fileSecurity)
            {
                throw new NotSupportedException("The file info does not support ACL extensions");
            }

            return fileSecurity;
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
            IFileSystemAclSupport aclSupport = fileInfo as IFileSystemAclSupport;
            var value = aclSupport?.GetAccessControl((IFileSystemAclSupport.AccessControlSections)includeSections);
            if (aclSupport == null || value is not FileSecurity fileSecurity)
            {
                throw new NotSupportedException("The file info does not support ACL extensions");
            }

            return fileSecurity;
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
            IFileSystemAclSupport aclSupport = fileInfo as IFileSystemAclSupport;
            if (aclSupport == null)
            {
                throw new NotSupportedException("The file info does not support ACL extensions");
            }

            aclSupport.SetAccessControl(fileSecurity);
        }
    }
}
