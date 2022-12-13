using System.Collections.Generic;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="File"/>
    [Serializable]
    public abstract partial class FileBase : IFile
    {
        ///
        protected FileBase(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        [Obsolete("This constructor only exists to support mocking libraries.", error: true)]
        internal FileBase() { }

        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        public IFileSystem FileSystem { get; }

        /// <inheritdoc cref="IFile.AppendAllLines(string,IEnumerable{string})"/>
        public abstract void AppendAllLines(string path, IEnumerable<string> contents);

        /// <inheritdoc cref="IFile.AppendAllLines(string,IEnumerable{string},Encoding)"/>
        public abstract void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        /// <inheritdoc cref="IFile.AppendAllText(string,string)"/>
        public abstract void AppendAllText(string path, string contents);

        /// <inheritdoc cref="IFile.AppendAllText(string,string,Encoding)"/>
        public abstract void AppendAllText(string path, string contents, Encoding encoding);

        /// <inheritdoc cref="IFile.AppendText"/>
        public abstract StreamWriter AppendText(string path);

        /// <inheritdoc cref="IFile.Copy(string,string)"/>
        public abstract void Copy(string sourceFileName, string destFileName);

        /// <inheritdoc cref="IFile.Copy(string,string,bool)"/>
        public abstract void Copy(string sourceFileName, string destFileName, bool overwrite);

        /// <inheritdoc cref="IFile.Create(string)"/>
        public abstract FileSystemStream Create(string path);

        /// <inheritdoc cref="IFile.Create(string,int)"/>
        public abstract FileSystemStream Create(string path, int bufferSize);

        /// <inheritdoc cref="IFile.Create(string,int,FileOptions)"/>
        public abstract FileSystemStream Create(string path, int bufferSize, FileOptions options);

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc cref="IFile.CreateSymbolicLink(string, string)"/>
        public abstract IFileSystemInfo CreateSymbolicLink(string path, string pathToTarget);
#endif
        /// <inheritdoc cref="IFile.CreateText"/>
        public abstract StreamWriter CreateText(string path);

        /// <inheritdoc cref="IFile.Decrypt"/>
        public abstract void Decrypt(string path);

        /// <inheritdoc cref="IFile.Delete"/>
        public abstract void Delete(string path);

        /// <inheritdoc cref="IFile.Encrypt"/>
        public abstract void Encrypt(string path);

        /// <inheritdoc cref="IFile.Exists"/>
        public abstract bool Exists(string path);
        
        /// <inheritdoc cref="IFile.GetAttributes(string)"/>
        public abstract FileAttributes GetAttributes(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.GetCreationTime(SafeFileHandle)"/>
        public abstract FileAttributes GetAttributes(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="IFile.GetCreationTime(string)"/>
        public abstract DateTime GetCreationTime(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.GetCreationTime(SafeFileHandle)"/>
        public abstract DateTime GetCreationTime(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="IFile.GetCreationTimeUtc(string)"/>
        public abstract DateTime GetCreationTimeUtc(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.GetCreationTimeUtc(SafeFileHandle)"/>
        public abstract DateTime GetCreationTimeUtc(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="IFile.GetLastAccessTime(string)"/>
        public abstract DateTime GetLastAccessTime(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.GetLastAccessTime(SafeFileHandle)"/>
        public abstract DateTime GetLastAccessTime(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="IFile.GetLastAccessTimeUtc(string)"/>
        public abstract DateTime GetLastAccessTimeUtc(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.GetLastAccessTimeUtc(SafeFileHandle)"/>
        public abstract DateTime GetLastAccessTimeUtc(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="IFile.GetLastWriteTime(string)"/>
        public abstract DateTime GetLastWriteTime(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.GetLastWriteTime(SafeFileHandle)"/>
        public abstract DateTime GetLastWriteTime(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="IFile.GetLastWriteTimeUtc(string)"/>
        public abstract DateTime GetLastWriteTimeUtc(string path);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.GetLastWriteTimeUtc(SafeFileHandle)"/>
        public abstract DateTime GetLastWriteTimeUtc(SafeFileHandle fileHandle);
#endif

#if FEATURE_UNIX_FILE_MODE
        /// <inheritdoc cref="IFile.GetUnixFileMode(string)"/>
        public abstract UnixFileMode GetUnixFileMode(string path);
#endif

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.GetUnixFileMode(SafeFileHandle)"/>
        public abstract UnixFileMode GetUnixFileMode(SafeFileHandle fileHandle);
#endif

        /// <inheritdoc cref="IFile.Move(string,string)"/>
        public abstract void Move(string sourceFileName, string destFileName);

#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc cref="IFile.Move(string,string,bool)"/>
        public abstract void Move(string sourceFileName, string destFileName, bool overwrite);
#endif

        /// <inheritdoc cref="IFile.Open(string,FileMode)"/>
        public abstract FileSystemStream Open(string path, FileMode mode);

        /// <inheritdoc cref="IFile.Open(string,FileMode,FileAccess)"/>
        public abstract FileSystemStream Open(string path, FileMode mode, FileAccess access);

        /// <inheritdoc cref="IFile.Open(string,FileMode,FileAccess,FileShare)"/>
        public abstract FileSystemStream Open(string path, FileMode mode, FileAccess access, FileShare share);

#if FEATURE_FILESTREAM_OPTIONS
        /// <inheritdoc cref="IFile.Open(string,FileStreamOptions)"/>
        public abstract FileSystemStream Open(string path, FileStreamOptions options);
#endif

        /// <inheritdoc cref="IFile.OpenRead"/>
        public abstract FileSystemStream OpenRead(string path);

        /// <inheritdoc cref="IFile.OpenText"/>
        public abstract StreamReader OpenText(string path);

        /// <inheritdoc cref="IFile.OpenWrite"/>
        public abstract FileSystemStream OpenWrite(string path);

        /// <inheritdoc cref="IFile.ReadAllBytes"/>
        public abstract byte[] ReadAllBytes(string path);
        
        /// <inheritdoc cref="IFile.ReadAllLines(string)"/>
        public abstract string[] ReadAllLines(string path);

        /// <inheritdoc cref="IFile.ReadAllLines(string,Encoding)"/>
        public abstract string[] ReadAllLines(string path, Encoding encoding);
        
        /// <inheritdoc cref="IFile.ReadAllText(string)"/>
        public abstract string ReadAllText(string path);

        /// <inheritdoc cref="IFile.ReadAllText(string,Encoding)"/>
        public abstract string ReadAllText(string path, Encoding encoding);
        
        /// <inheritdoc cref="IFile.ReadLines(string)"/>
        public abstract IEnumerable<string> ReadLines(string path);

        /// <inheritdoc cref="IFile.ReadLines(string,Encoding)"/>
        public abstract IEnumerable<string> ReadLines(string path, Encoding encoding);

        /// <inheritdoc cref="IFile.Replace(string,string,string)"/>
        public abstract void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName);

        /// <inheritdoc cref="IFile.Replace(string,string,string,bool)"/>
        public abstract void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc cref="IFile.ResolveLinkTarget(string,bool)"/>
        public abstract IFileSystemInfo ResolveLinkTarget(string linkPath, bool returnFinalTarget);
#endif
        
        /// <inheritdoc cref="IFile.SetAttributes(string, FileAttributes)"/>
        public abstract void SetAttributes(string path, FileAttributes fileAttributes);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.SetAttributes(SafeFileHandle, FileAttributes)"/>
        public abstract void SetAttributes(SafeFileHandle fileHandle, FileAttributes fileAttributes);
#endif

        /// <inheritdoc cref="IFile.SetCreationTime(string, DateTime)"/>
        public abstract void SetCreationTime(string path, DateTime creationTime);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.SetCreationTime(SafeFileHandle, DateTime)"/>
        public abstract void SetCreationTime(SafeFileHandle fileHandle, DateTime creationTime);
#endif

        /// <inheritdoc cref="IFile.SetCreationTimeUtc(string, DateTime)"/>
        public abstract void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.SetCreationTimeUtc(SafeFileHandle, DateTime)"/>
        public abstract void SetCreationTimeUtc(SafeFileHandle fileHandle, DateTime creationTimeUtc);
#endif

        /// <inheritdoc cref="IFile.SetLastAccessTime(string, DateTime)"/>
        public abstract void SetLastAccessTime(string path, DateTime lastAccessTime);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.SetLastAccessTime(SafeFileHandle, DateTime)"/>
        public abstract void SetLastAccessTime(SafeFileHandle fileHandle, DateTime lastAccessTime);
#endif

        /// <inheritdoc cref="IFile.SetLastAccessTimeUtc(string, DateTime)"/>
        public abstract void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.SetLastAccessTimeUtc(SafeFileHandle, DateTime)"/>
        public abstract void SetLastAccessTimeUtc(SafeFileHandle fileHandle, DateTime lastAccessTimeUtc);
#endif

        /// <inheritdoc cref="IFile.SetLastWriteTime(string, DateTime)"/>
        public abstract void SetLastWriteTime(string path, DateTime lastWriteTime);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.SetLastWriteTime(SafeFileHandle, DateTime)"/>
        public abstract void SetLastWriteTime(SafeFileHandle fileHandle, DateTime lastWriteTime);
#endif

        /// <inheritdoc cref="IFile.SetLastWriteTimeUtc(string, DateTime)"/>
        public abstract void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.SetLastWriteTimeUtc(SafeFileHandle, DateTime)"/>
        public abstract void SetLastWriteTimeUtc(SafeFileHandle fileHandle, DateTime lastWriteTimeUtc);
#endif

#if FEATURE_UNIX_FILE_MODE
        /// <inheritdoc cref="IFile.SetUnixFileMode(string, UnixFileMode)"/>
        public abstract void SetUnixFileMode(string path, UnixFileMode mode);
#endif

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc cref="IFile.SetUnixFileMode(SafeFileHandle, UnixFileMode)"/>
        public abstract void SetUnixFileMode(SafeFileHandle fileHandle, UnixFileMode mode);
#endif

        /// <inheritdoc cref="IFile.WriteAllBytes(string, byte[])"/>
        public abstract void WriteAllBytes(string path, byte[] bytes);

        /// <inheritdoc cref="IFile.WriteAllLines(string,IEnumerable{string})"/>
        public abstract void WriteAllLines(string path, IEnumerable<string> contents);

        /// <inheritdoc cref="IFile.WriteAllLines(string,IEnumerable{string},Encoding)"/>
        public abstract void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        /// <inheritdoc cref="IFile.WriteAllLines(string,string[])"/>
        public abstract void WriteAllLines(string path, string[] contents);

        /// <inheritdoc cref="IFile.WriteAllLines(string,string[],Encoding)"/>
        public abstract void WriteAllLines(string path, string[] contents, Encoding encoding);

        /// <inheritdoc cref="IFile.WriteAllText(string,string)"/>
        public abstract void WriteAllText(string path, string contents);

        /// <inheritdoc cref="IFile.WriteAllText(string,string,Encoding)"/>
        public abstract void WriteAllText(string path, string contents, Encoding encoding);
    }
}
