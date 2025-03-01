using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions;

/// <summary>
///     ACL (access control list) extension methods for <see cref="IFile" />.
/// </summary>
public static class FileAclExtensions
{
#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
    /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(FileInfo)"/>
#else
        /// <inheritdoc cref="File.GetAccessControl(string)"/>
#endif
    [SupportedOSPlatform("windows")]
    public static FileSecurity GetAccessControl(
        this IFile file, string path)
    {
        IFileInfo fileInfo = file.FileSystem.FileInfo.New(path);
        return fileInfo.GetAccessControl();
    }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
    /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(FileInfo,AccessControlSections)"/>
#else
        /// <inheritdoc cref="File.GetAccessControl(string,AccessControlSections)"/>
#endif
    [SupportedOSPlatform("windows")]
    public static FileSecurity GetAccessControl(
        this IFile file,
        string path,
        AccessControlSections includeSections)
    {
        IFileInfo fileInfo = file.FileSystem.FileInfo.New(path);
        return fileInfo.GetAccessControl(includeSections);
    }

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
    /// <inheritdoc cref="FileSystemAclExtensions.SetAccessControl(FileInfo,FileSecurity)"/>
#else
        /// <inheritdoc cref="File.SetAccessControl(string,FileSecurity)"/>
#endif
    [SupportedOSPlatform("windows")]
    public static void SetAccessControl(this IFile file,
        string path,
        FileSecurity fileSecurity)
    {
        IFileInfo fileInfo = file.FileSystem.FileInfo.New(path);
        fileInfo.SetAccessControl(fileSecurity);
    }
}