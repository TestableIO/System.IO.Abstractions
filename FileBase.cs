using System.Security.AccessControl;
using System.Text;

namespace System.IO.Abstractions
{
    public abstract class FileBase
    {
        public abstract void AppendAllText(string path, string contents);
        public abstract void AppendAllText(string path, string contents, Encoding encoding);
        public abstract StreamWriter AppendText(string path);
        public abstract void Copy(string sourceFileName, string destFileName);
        public abstract void Copy(string sourceFileName, string destFileName, bool overwrite);
        public abstract FileStream Create(string path);
        public abstract FileStream Create(string path, int bufferSize);
        public abstract FileStream Create(string path, int bufferSize, FileOptions options);
        public abstract FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity);
        public abstract StreamWriter CreateText(string path);
        public abstract void Decrypt(string path);
        public abstract void Delete(string path);
        public abstract void Encrypt(string path);
        public abstract bool Exists(string path);
        public abstract FileSecurity GetAccessControl(string path);
        public abstract FileSecurity GetAccessControl(string path, AccessControlSections includeSections);
        public abstract FileAttributes GetAttributes(string path);
        public abstract DateTime GetCreationTime(string path);
        public abstract DateTime GetCreationTimeUtc(string path);
        public abstract DateTime GetLastAccessTime(string path);
        public abstract DateTime GetLastAccessTimeUtc(string path);
        public abstract DateTime GetLastWriteTime(string path);
        public abstract DateTime GetLastWriteTimeUtc(string path);
        public abstract void Move(string sourceFileName, string destFileName);
        public abstract FileStream Open(string path, FileMode mode);
        public abstract FileStream Open(string path, FileMode mode, FileAccess access);
        public abstract FileStream Open(string path, FileMode mode, FileAccess access, FileShare share);
        public abstract FileStream OpenRead(string path);
        public abstract StreamReader OpenText(string path);
        public abstract FileStream OpenWrite(string path);
        public abstract byte[] ReadAllBytes(string path);
        public abstract string[] ReadAllLines(string path);
        public abstract string[] ReadAllLines(string path, Encoding encoding);
        public abstract string ReadAllText(string path);
        public abstract string ReadAllText(string path, Encoding encoding);
        public abstract void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName);
        public abstract void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);
        public abstract void SetAccessControl(string path, FileSecurity fileSecurity);
        public abstract void SetAttributes(string path, FileAttributes fileAttributes);
        public abstract void SetCreationTime(string path, DateTime creationTime);
        public abstract void SetCreationTimeUtc(string path, DateTime creationTimeUtc);
        public abstract void SetLastAccessTime(string path, DateTime lastAccessTime);
        public abstract void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);
        public abstract void SetLastWriteTime(string path, DateTime lastWriteTime);
        public abstract void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);
        public abstract void WriteAllBytes(string path, byte[] bytes);
        public abstract void WriteAllLines(string path, string[] contents);
        public abstract void WriteAllLines(string path, string[] contents, Encoding encoding);
        public abstract void WriteAllText(string path, string contents);
        public abstract void WriteAllText(string path, string contents, Encoding encoding);
    }
}