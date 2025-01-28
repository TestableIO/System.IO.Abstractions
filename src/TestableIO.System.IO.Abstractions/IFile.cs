using Microsoft.Win32.SafeHandles;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using System.Text;

namespace System.IO.Abstractions
{
    /// <summary>
    /// Abstractions for <see cref="File" />.
    /// </summary>
#if FEATURE_ASYNC_FILE
    public partial interface IFile : IFileSystemEntity
#else
    public interface IFile : IFileSystemEntity
#endif
    {
#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="File.AppendAllBytes(string, byte[])" />
        void AppendAllBytes(string path,
            byte[] bytes);
        
        /// <inheritdoc cref="File.AppendAllBytes(string, ReadOnlySpan{byte})" />
        void AppendAllBytes(string path,
            ReadOnlySpan<byte> bytes);
#endif
        
        /// <inheritdoc cref="File.AppendAllLines(string, IEnumerable{string})" />
        void AppendAllLines(string path, IEnumerable<string> contents);

        /// <inheritdoc cref="File.AppendAllLines(string, IEnumerable{string}, Encoding)" />
        void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        /// <inheritdoc cref="File.AppendAllText(string, string?)" />
        void AppendAllText(string path, string? contents);

        /// <inheritdoc cref="File.AppendAllText(string, string?, Encoding)" />
        void AppendAllText(string path, string? contents, Encoding encoding);

#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="File.AppendAllText(string, ReadOnlySpan{char})" />
        void AppendAllText(string path,
            ReadOnlySpan<char> contents);
        
        /// <inheritdoc cref="File.AppendAllText(string, ReadOnlySpan{char}, Encoding)" />
        void AppendAllText(string path,
            ReadOnlySpan<char> contents,
            Encoding encoding);
#endif

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

        /// <inheritdoc cref="File.GetAttributes(string)" />
        FileAttributes GetAttributes(string path);
#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.GetAttributes(SafeFileHandle)" />
        FileAttributes GetAttributes(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="File.GetCreationTime(string)" />
        DateTime GetCreationTime(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.GetCreationTime(SafeFileHandle)" />
        DateTime GetCreationTime(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="File.GetCreationTimeUtc(string)" />
        DateTime GetCreationTimeUtc(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.GetCreationTimeUtc(SafeFileHandle)" />
        DateTime GetCreationTimeUtc(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="File.GetLastAccessTime(string)" />
        DateTime GetLastAccessTime(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.GetLastAccessTime(SafeFileHandle)" />
        DateTime GetLastAccessTime(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="File.GetLastAccessTimeUtc(string)" />
        DateTime GetLastAccessTimeUtc(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.GetLastAccessTimeUtc(SafeFileHandle)" />
        DateTime GetLastAccessTimeUtc(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="File.GetLastWriteTime(string)" />
        DateTime GetLastWriteTime(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.GetLastWriteTime(SafeFileHandle)" />
        DateTime GetLastWriteTime(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="File.GetLastWriteTimeUtc(string)" />
        DateTime GetLastWriteTimeUtc(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.GetLastWriteTimeUtc(SafeFileHandle)" />
        DateTime GetLastWriteTimeUtc(SafeFileHandle fileHandle);
#endif

#if FEATURE_UNIX_FILE_MODE
        /// <inheritdoc cref="File.GetUnixFileMode(string)" />
        [UnsupportedOSPlatform("windows")]
        UnixFileMode GetUnixFileMode(string path);
#endif

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.GetUnixFileMode(SafeFileHandle)" />
        [UnsupportedOSPlatform("windows")]
        UnixFileMode GetUnixFileMode(SafeFileHandle fileHandle);
#endif

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
        
        /// <inheritdoc cref="File.SetAttributes(string, FileAttributes)" />
        void SetAttributes(string path, FileAttributes fileAttributes);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.SetAttributes(SafeFileHandle, FileAttributes)" />
        void SetAttributes(SafeFileHandle fileHandle, FileAttributes fileAttributes);
#endif

        /// <inheritdoc cref="File.SetCreationTime(string, DateTime)" />
        void SetCreationTime(string path, DateTime creationTime);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.SetCreationTime(SafeFileHandle, DateTime)" />
        void SetCreationTime(SafeFileHandle fileHandle, DateTime creationTime);
#endif

        /// <inheritdoc cref="File.SetCreationTimeUtc(string, DateTime)" />
        void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.SetCreationTimeUtc(SafeFileHandle, DateTime)" />
        void SetCreationTimeUtc(SafeFileHandle fileHandle, DateTime creationTimeUtc);
#endif

        /// <inheritdoc cref="File.SetLastAccessTime(string, DateTime)" />
        void SetLastAccessTime(string path, DateTime lastAccessTime);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.SetLastAccessTime(SafeFileHandle, DateTime)" />
        void SetLastAccessTime(SafeFileHandle fileHandle, DateTime lastAccessTime);
#endif

        /// <inheritdoc cref="File.SetLastAccessTimeUtc(string, DateTime)" />
        void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.SetLastAccessTimeUtc(SafeFileHandle, DateTime)" />
        void SetLastAccessTimeUtc(SafeFileHandle fileHandle, DateTime lastAccessTimeUtc);
#endif

        /// <inheritdoc cref="File.SetLastWriteTime(string, DateTime)" />
        void SetLastWriteTime(string path, DateTime lastWriteTime);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.SetLastWriteTime(SafeFileHandle, DateTime)" />
        void SetLastWriteTime(SafeFileHandle fileHandle, DateTime lastWriteTime);
#endif

        /// <inheritdoc cref="File.SetLastWriteTimeUtc(string, DateTime)" />
        void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.SetLastWriteTimeUtc(SafeFileHandle, DateTime)" />
        void SetLastWriteTimeUtc(SafeFileHandle fileHandle, DateTime lastWriteTimeUtc);
#endif

#if FEATURE_UNIX_FILE_MODE
        /// <inheritdoc cref="File.SetUnixFileMode(string, UnixFileMode)" />
        [UnsupportedOSPlatform("windows")]
        void SetUnixFileMode(string path, UnixFileMode mode);
#endif

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="File.SetUnixFileMode(SafeFileHandle, UnixFileMode)" />
        [UnsupportedOSPlatform("windows")]
        void SetUnixFileMode(SafeFileHandle fileHandle, UnixFileMode mode);
#endif

        /// <inheritdoc cref="File.WriteAllBytes(string, byte[])" />
        void WriteAllBytes(string path, byte[] bytes);

#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="File.WriteAllBytes(string, ReadOnlySpan{byte})" />
        void WriteAllBytes(string path,
            ReadOnlySpan<byte> bytes);
#endif

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

#if FEATURE_FILE_SPAN
        /// <inheritdoc cref="File.WriteAllText(string, ReadOnlySpan{char})" />
        void WriteAllText(string path,
            ReadOnlySpan<char> contents);
        
        /// <inheritdoc cref="File.WriteAllText(string, ReadOnlySpan{char}, Encoding)" />
        void WriteAllText(string path,
            ReadOnlySpan<char> contents,
            Encoding encoding);
#endif
    }
}