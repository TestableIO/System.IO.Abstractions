using System.Reflection;
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
            IFileSystemAclSupport aclSupport = fileStream as IFileSystemAclSupport;
            var value = aclSupport?.GetAccessControl();
            if (aclSupport == null || value is not FileSecurity fileSecurity)
            {
                throw new NotSupportedException("The file stream does not support ACL extensions");
            }

            return fileSecurity;
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
            IFileSystemAclSupport aclSupport = fileStream as IFileSystemAclSupport;
            if (aclSupport == null)
            {
                throw new NotSupportedException("The file info does not support ACL extensions");
            }

            aclSupport.SetAccessControl(fileSecurity);
        }
    }
}
