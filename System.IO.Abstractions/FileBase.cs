using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace System.IO.Abstractions
{
#if NET40
    [Serializable]
#endif
    public abstract class FileBase
    {
        public abstract void AppendAllLines(String path, IEnumerable<String> contents);
        public abstract void AppendAllLines(String path, IEnumerable<String> contents, Encoding encoding);
        public abstract void AppendAllText(string path, string contents);
        public abstract void AppendAllText(string path, string contents, Encoding encoding);
        public abstract StreamWriter AppendText(string path);
        public abstract void Copy(string sourceFileName, string destFileName);
        public abstract void Copy(string sourceFileName, string destFileName, bool overwrite);
        public abstract Stream Create(string path);
        public abstract Stream Create(string path, int bufferSize);
        public abstract Stream Create(string path, int bufferSize, FileOptions options);

#if NET40
        public abstract Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity);
#endif

        public abstract StreamWriter CreateText(string path);

#if NET40
        public abstract void Decrypt(string path);
#endif

        public abstract void Delete(string path);

#if NET40
        public abstract void Encrypt(string path);
#endif

        public abstract bool Exists(string path);

#if NET40
        public abstract FileSecurity GetAccessControl(string path);
        public abstract FileSecurity GetAccessControl(string path, AccessControlSections includeSections);
#endif

        public abstract FileAttributes GetAttributes(string path);
        public abstract DateTime GetCreationTime(string path);
        public abstract DateTime GetCreationTimeUtc(string path);
        public abstract DateTime GetLastAccessTime(string path);
        public abstract DateTime GetLastAccessTimeUtc(string path);
        public abstract DateTime GetLastWriteTime(string path);
        public abstract DateTime GetLastWriteTimeUtc(string path);
        public abstract void Move(string sourceFileName, string destFileName);
        public abstract Stream Open(string path, FileMode mode);
        public abstract Stream Open(string path, FileMode mode, FileAccess access);
        public abstract Stream Open(string path, FileMode mode, FileAccess access, FileShare share);
        public abstract Stream OpenRead(string path);
        public abstract StreamReader OpenText(string path);
        public abstract Stream OpenWrite(string path);
        public abstract byte[] ReadAllBytes(string path);
        public abstract string[] ReadAllLines(string path);
        public abstract string[] ReadAllLines(string path, Encoding encoding);
        public abstract string ReadAllText(string path);
        public abstract string ReadAllText(string path, Encoding encoding);
        public abstract IEnumerable<String> ReadLines(String path);
        public abstract IEnumerable<String> ReadLines(String path, Encoding encoding);

#if NET40
        public abstract void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName);
        public abstract void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);
        public abstract void SetAccessControl(string path, FileSecurity fileSecurity);
#endif

        public abstract void SetAttributes(string path, FileAttributes fileAttributes);
        public abstract void SetCreationTime(string path, DateTime creationTime);
        public abstract void SetCreationTimeUtc(string path, DateTime creationTimeUtc);
        public abstract void SetLastAccessTime(string path, DateTime lastAccessTime);
        public abstract void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);
        public abstract void SetLastWriteTime(string path, DateTime lastWriteTime);
        public abstract void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);
        public abstract void WriteAllBytes(string path, byte[] bytes);
        public abstract void WriteAllLines(String path, IEnumerable<String> contents);
        public abstract void WriteAllLines(String path, IEnumerable<String> contents, Encoding encoding);
        public abstract void WriteAllLines(string path, string[] contents);
        public abstract void WriteAllLines(string path, string[] contents, Encoding encoding);
        public abstract void WriteAllText(string path, string contents);
        public abstract void WriteAllText(string path, string contents, Encoding encoding);
    }
}