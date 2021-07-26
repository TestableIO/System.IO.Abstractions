using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;

namespace System.IO.Abstractions
{
    public partial interface IFile
    {
        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        IFileSystem FileSystem { get; }

        /// <inheritdoc cref="File.AppendAllLines(string,IEnumerable{string})"/>
        void AppendAllLines(string path, IEnumerable<string> contents);
        /// <inheritdoc cref="File.AppendAllLines(string,IEnumerable{string},Encoding)"/>
        void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);
        /// <inheritdoc cref="File.AppendAllText(string,string)"/>
        void AppendAllText(string path, string contents);
        /// <inheritdoc cref="File.AppendAllText(string,string,Encoding)"/>
        void AppendAllText(string path, string contents, Encoding encoding);
        /// <inheritdoc cref="File.AppendText"/>
        StreamWriter AppendText(string path);
        /// <inheritdoc cref="File.Copy(string,string)"/>
        void Copy(string sourceFileName, string destFileName);
        /// <inheritdoc cref="File.Copy(string,string,bool)"/>
        void Copy(string sourceFileName, string destFileName, bool overwrite);
        /// <inheritdoc cref="File.Create(string)"/>
        Stream Create(string path);
        /// <inheritdoc cref="File.Create(string,int)"/>
        Stream Create(string path, int bufferSize);
        /// <inheritdoc cref="File.Create(string,int,FileOptions)"/>
        Stream Create(string path, int bufferSize, FileOptions options);
        /// <inheritdoc cref="File.CreateText"/>
        StreamWriter CreateText(string path);
        /// <inheritdoc cref="File.Decrypt"/>
        void Decrypt(string path);
        /// <inheritdoc cref="File.Delete"/>
        void Delete(string path);
        /// <inheritdoc cref="File.Encrypt"/>
        void Encrypt(string path);
        /// <inheritdoc cref="File.Exists"/>
        bool Exists(string path);
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

        /// <inheritdoc cref="File.GetAttributes"/>
        FileAttributes GetAttributes(string path);
        /// <inheritdoc cref="File.GetCreationTime"/>
        DateTime GetCreationTime(string path);
        /// <inheritdoc cref="File.GetCreationTimeUtc"/>
        DateTime GetCreationTimeUtc(string path);
        /// <inheritdoc cref="File.GetLastAccessTime"/>
        DateTime GetLastAccessTime(string path);
        /// <inheritdoc cref="File.GetLastAccessTimeUtc"/>
        DateTime GetLastAccessTimeUtc(string path);
        /// <inheritdoc cref="File.GetLastWriteTime"/>
        DateTime GetLastWriteTime(string path);
        /// <inheritdoc cref="File.GetLastWriteTimeUtc"/>
        DateTime GetLastWriteTimeUtc(string path);
        /// <inheritdoc cref="File.Move(string,string)"/>
        void Move(string sourceFileName, string destFileName);
#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc cref="File.Move(string,string,bool)"/>
        void Move(string sourceFileName, string destFileName, bool overwrite);
#endif
        /// <inheritdoc cref="File.Open(string,FileMode)"/>
        Stream Open(string path, FileMode mode);
        /// <inheritdoc cref="File.Open(string,FileMode,FileAccess)"/>
        Stream Open(string path, FileMode mode, FileAccess access);
        /// <inheritdoc cref="File.Open(string,FileMode,FileAccess,FileShare)"/>
        Stream Open(string path, FileMode mode, FileAccess access, FileShare share);
        /// <inheritdoc cref="File.OpenRead"/>
        Stream OpenRead(string path);
        /// <inheritdoc cref="File.OpenText"/>
        StreamReader OpenText(string path);
        /// <inheritdoc cref="File.OpenWrite"/>
        Stream OpenWrite(string path);
        /// <inheritdoc cref="File.ReadAllBytes"/>
        byte[] ReadAllBytes(string path);
        /// <inheritdoc cref="File.ReadAllLines(string)"/>
        string[] ReadAllLines(string path);
        /// <inheritdoc cref="File.ReadAllLines(string,Encoding)"/>
        string[] ReadAllLines(string path, Encoding encoding);
        /// <inheritdoc cref="File.ReadAllText(string)"/>
        string ReadAllText(string path);
        /// <inheritdoc cref="File.ReadAllText(string,Encoding)"/>
        string ReadAllText(string path, Encoding encoding);
        /// <inheritdoc cref="File.ReadLines(string)"/>
        IEnumerable<string> ReadLines(string path);
        /// <inheritdoc cref="File.ReadLines(string,Encoding)"/>
        IEnumerable<string> ReadLines(string path, Encoding encoding);
        /// <inheritdoc cref="File.Replace(string,string,string)"/>
        void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName);
        /// <inheritdoc cref="File.Replace(string,string,string,bool)"/>
        void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);
#if FEATURE_FILE_SYSTEM_ACL_EXTENSIONS
        /// <inheritdoc cref="FileSystemAclExtensions.SetAccessControl(FileInfo,FileSecurity)"/>
#else
        /// <inheritdoc cref="File.SetAccessControl(FileSecurity)"/>
#endif
        [SupportedOSPlatform("windows")]
        void SetAccessControl(string path, FileSecurity fileSecurity);
        /// <inheritdoc cref="File.SetAttributes"/>
        void SetAttributes(string path, FileAttributes fileAttributes);
        /// <inheritdoc cref="File.SetCreationTime"/>
        void SetCreationTime(string path, DateTime creationTime);
        /// <inheritdoc cref="File.SetCreationTimeUtc"/>
        void SetCreationTimeUtc(string path, DateTime creationTimeUtc);
        /// <inheritdoc cref="File.SetLastAccessTime"/>
        void SetLastAccessTime(string path, DateTime lastAccessTime);
        /// <inheritdoc cref="File.SetLastAccessTimeUtc"/>
        void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);
        /// <inheritdoc cref="File.SetLastWriteTime"/>
        void SetLastWriteTime(string path, DateTime lastWriteTime);
        /// <inheritdoc cref="File.SetLastWriteTimeUtc"/>
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);
        /// <inheritdoc cref="File.WriteAllBytes"/>
        void WriteAllBytes(string path, byte[] bytes);
        /// <inheritdoc cref="File.WriteAllLines(string,IEnumerable{string})"/>
        void WriteAllLines(string path, IEnumerable<string> contents);
        /// <inheritdoc cref="File.WriteAllLines(string,IEnumerable{string},Encoding)"/>
        void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);
        /// <inheritdoc cref="File.WriteAllLines(string,string[])"/>
        void WriteAllLines(string path, string[] contents);
        /// <inheritdoc cref="File.WriteAllLines(string,string[],Encoding)"/>
        void WriteAllLines(string path, string[] contents, Encoding encoding);
        /// <inheritdoc cref="File.WriteAllText(string,string)"/>
        void WriteAllText(string path, string contents);
        /// <inheritdoc cref="File.WriteAllText(string,string,Encoding)"/>
        void WriteAllText(string path, string contents, Encoding encoding);
    }
}