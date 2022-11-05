using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;

namespace System.IO.Abstractions
{
    /// <summary>
    /// Abstractions for <see cref="File" />.
    /// </summary>
    public partial interface IFile : IFileSystemExtensionPoint
    {
        /// <inheritdoc cref="File.AppendAllLines(string, IEnumerable{string})" />
        void AppendAllLines(string path, IEnumerable<string> contents);

        /// <inheritdoc cref="File.AppendAllLines(string, IEnumerable{string}, Encoding)" />
        void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        /// <inheritdoc cref="File.AppendAllText(string, string?)" />
        void AppendAllText(string path, string? contents);

        /// <inheritdoc cref="File.AppendAllText(string, string?, Encoding)" />
        void AppendAllText(string path, string? contents, Encoding encoding);

        /// <inheritdoc cref="File.AppendText(string)" />
        StreamWriter AppendText(string path);

        /// <inheritdoc cref="File.Copy(string, string)" />
        void Copy(string sourceFileName, string destFileName);

        /// <inheritdoc cref="File.Copy(string, string, bool)" />
        void Copy(string sourceFileName, string destFileName, bool overwrite);

        /// <inheritdoc cref="File.Create(string)" />
        FileSystemStream Create(string path);

        /// <inheritdoc cref="File.Create(string, int)" />
        FileSystemStream Create(string path, int bufferSize);

        /// <inheritdoc cref="File.Create(string, int, FileOptions)" />
        FileSystemStream Create(string path, int bufferSize, FileOptions options);

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc cref="File.CreateSymbolicLink(string, string)" />
        IFileSystemInfo CreateSymbolicLink(string path, string pathToTarget);
#endif
        /// <inheritdoc cref="File.CreateText(string)" />
        StreamWriter CreateText(string path);

        /// <inheritdoc cref="File.Decrypt(string)" />
        [SupportedOSPlatform("windows")]
        void Decrypt(string path);

        /// <inheritdoc cref="File.Delete(string)" />
        void Delete(string path);

        /// <inheritdoc cref="File.Encrypt(string)" />
        [SupportedOSPlatform("windows")]
        void Encrypt(string path);
        
        /// <inheritdoc cref="File.Exists(string?)" />
        bool Exists([NotNullWhen(true)] string? path);

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(FileInfo)"/>
#else
        /// <inheritdoc cref="File.GetAccessControl(string)"/>
#endif
        [SupportedOSPlatform("windows")]
        FileSecurity GetAccessControl(string path);

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.GetAccessControl(FileInfo,AccessControlSections)"/>
#else
        /// <inheritdoc cref="File.GetAccessControl(string,AccessControlSections)"/>
#endif
        [SupportedOSPlatform("windows")]
        FileSecurity GetAccessControl(string path, AccessControlSections includeSections);

        /// <inheritdoc cref="File.GetAttributes(string)" />
        FileAttributes GetAttributes(string path);

        /// <inheritdoc cref="File.GetCreationTime(string)" />
        DateTime GetCreationTime(string path);

        /// <inheritdoc cref="File.GetCreationTimeUtc(string)" />
        DateTime GetCreationTimeUtc(string path);

        /// <inheritdoc cref="File.GetLastAccessTime(string)" />
        DateTime GetLastAccessTime(string path);

        /// <inheritdoc cref="File.GetLastAccessTimeUtc(string)" />
        DateTime GetLastAccessTimeUtc(string path);

        /// <inheritdoc cref="File.GetLastWriteTime(string)" />
        DateTime GetLastWriteTime(string path);

        /// <inheritdoc cref="File.GetLastWriteTimeUtc(string)" />
        DateTime GetLastWriteTimeUtc(string path);

        /// <inheritdoc cref="File.Move(string, string)" />
        void Move(string sourceFileName, string destFileName);

#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc cref="File.Move(string, string, bool)" />
        void Move(string sourceFileName, string destFileName, bool overwrite);
#endif

        /// <inheritdoc cref="File.Open(string, FileMode)" />
        FileSystemStream Open(string path, FileMode mode);

        /// <inheritdoc cref="File.Open(string, FileMode, FileAccess)" />
        FileSystemStream Open(string path, FileMode mode, FileAccess access);

        /// <inheritdoc cref="File.Open(string, FileMode, FileAccess, FileShare)" />
        FileSystemStream Open(string path, FileMode mode, FileAccess access,
            FileShare share);

#if FEATURE_FILESTREAM_OPTIONS
        /// <inheritdoc cref="File.Open(string, FileStreamOptions)" />
        FileSystemStream Open(string path, FileStreamOptions options);
#endif

        /// <inheritdoc cref="File.OpenRead(string)" />
        FileSystemStream OpenRead(string path);

        /// <inheritdoc cref="File.OpenText(string)" />
        StreamReader OpenText(string path);

        /// <inheritdoc cref="File.OpenWrite(string)" />
        FileSystemStream OpenWrite(string path);

        /// <inheritdoc cref="File.ReadAllBytes(string)" />
        byte[] ReadAllBytes(string path);
        
        /// <inheritdoc cref="File.ReadAllLines(string)" />
        string[] ReadAllLines(string path);

        /// <inheritdoc cref="File.ReadAllLines(string, Encoding)" />
        string[] ReadAllLines(string path, Encoding encoding);

        /// <inheritdoc cref="File.ReadAllText(string)" />
        string ReadAllText(string path);

        /// <inheritdoc cref="File.ReadAllText(string, Encoding)" />
        string ReadAllText(string path, Encoding encoding);


        /// <inheritdoc cref="File.ReadLines(string)" />
        IEnumerable<string> ReadLines(string path);

        /// <inheritdoc cref="File.ReadLines(string, Encoding)" />
        IEnumerable<string> ReadLines(string path, Encoding encoding);

        /// <inheritdoc cref="File.Replace(string, string, string?)" />
        void Replace(string sourceFileName,
            string destinationFileName,
            string? destinationBackupFileName);

        /// <inheritdoc cref="File.Replace(string, string, string?, bool)" />
        void Replace(string sourceFileName,
            string destinationFileName,
            string? destinationBackupFileName,
            bool ignoreMetadataErrors);

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc cref="File.ResolveLinkTarget(string, bool)" />
        IFileSystemInfo? ResolveLinkTarget(string linkPath, bool returnFinalTarget);
#endif

#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.SetAccessControl(FileInfo,FileSecurity)"/>
#else
        /// <inheritdoc cref="File.SetAccessControl(string,FileSecurity)"/>
#endif
        [SupportedOSPlatform("windows")]
        void SetAccessControl(string path, FileSecurity fileSecurity);
        
        /// <inheritdoc cref="File.SetAttributes(string, FileAttributes)" />
        void SetAttributes(string path, FileAttributes fileAttributes);

        /// <inheritdoc cref="File.SetCreationTime(string, DateTime)" />
        void SetCreationTime(string path, DateTime creationTime);

        /// <inheritdoc cref="File.SetCreationTimeUtc(string, DateTime)" />
        void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <inheritdoc cref="File.SetLastAccessTime(string, DateTime)" />
        void SetLastAccessTime(string path, DateTime lastAccessTime);

        /// <inheritdoc cref="File.SetLastAccessTimeUtc(string, DateTime)" />
        void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <inheritdoc cref="File.SetLastWriteTime(string, DateTime)" />
        void SetLastWriteTime(string path, DateTime lastWriteTime);

        /// <inheritdoc cref="File.SetLastWriteTimeUtc(string, DateTime)" />
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <inheritdoc cref="File.WriteAllBytes(string, byte[])" />
        void WriteAllBytes(string path, byte[] bytes);

        /// <inheritdoc cref="File.WriteAllLines(string, string[])" />
        void WriteAllLines(string path, string[] contents);

        /// <inheritdoc cref="File.WriteAllLines(string, IEnumerable{string})" />
        void WriteAllLines(string path, IEnumerable<string> contents);

        /// <inheritdoc cref="File.WriteAllLines(string, string[], Encoding)" />
        void WriteAllLines(string path, string[] contents, Encoding encoding);

        /// <inheritdoc cref="File.WriteAllLines(string, IEnumerable{string}, Encoding)" />
        void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        /// <inheritdoc cref="File.WriteAllText(string, string)" />
        void WriteAllText(string path, string? contents);

        /// <inheritdoc cref="File.WriteAllText(string, string, Encoding)" />
        void WriteAllText(string path, string? contents, Encoding encoding);
    }
}