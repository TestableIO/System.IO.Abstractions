using System.Security.AccessControl;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    public class MockFile : FileBase
    {
        readonly IMockFileDataAccessor mockFileDataAccessor;

        public MockFile(IMockFileDataAccessor mockFileDataAccessor)
        {
            this.mockFileDataAccessor = mockFileDataAccessor;
        }

        public override void AppendAllText(string path, string contents)
        {
            throw new NotImplementedException();
        }

        public override void AppendAllText(string path, string contents, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public override StreamWriter AppendText(string path)
        {
            throw new NotImplementedException();
        }

        public override void Copy(string sourceFileName, string destFileName)
        {
            throw new NotImplementedException();
        }

        public override void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            throw new NotImplementedException();
        }

        public override FileStream Create(string path)
        {
            throw new NotImplementedException();
        }

        public override FileStream Create(string path, int bufferSize)
        {
            throw new NotImplementedException();
        }

        public override FileStream Create(string path, int bufferSize, FileOptions options)
        {
            throw new NotImplementedException();
        }

        public override FileStream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            throw new NotImplementedException();
        }

        public override StreamWriter CreateText(string path)
        {
            throw new NotImplementedException();
        }

        public override void Decrypt(string path)
        {
            throw new NotImplementedException();
        }

        public override void Delete(string path)
        {
            throw new NotImplementedException();
        }

        public override void Encrypt(string path)
        {
            throw new NotImplementedException();
        }

        public override bool Exists(string path)
        {
            throw new NotImplementedException();
        }

        public override FileSecurity GetAccessControl(string path)
        {
            throw new NotImplementedException();
        }

        public override FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            throw new NotImplementedException();
        }

        public override FileAttributes GetAttributes(string path)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetCreationTime(string path)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetCreationTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastAccessTime(string path)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastAccessTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastWriteTime(string path)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            throw new NotImplementedException();
        }

        public override void Move(string sourceFileName, string destFileName)
        {
            throw new NotImplementedException();
        }

        public override FileStream Open(string path, FileMode mode)
        {
            throw new NotImplementedException();
        }

        public override FileStream Open(string path, FileMode mode, FileAccess access)
        {
            throw new NotImplementedException();
        }

        public override FileStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            throw new NotImplementedException();
        }

        public override FileStream OpenRead(string path)
        {
            throw new NotImplementedException();
        }

        public override StreamReader OpenText(string path)
        {
            throw new NotImplementedException();
        }

        public override FileStream OpenWrite(string path)
        {
            throw new NotImplementedException();
        }

        public override byte[] ReadAllBytes(string path)
        {
            return mockFileDataAccessor.GetFile(path).Contents;
        }

        public override string[] ReadAllLines(string path)
        {
            return mockFileDataAccessor
                .GetFile(path)
                .TextContents
                .SplitLines();
        }

        public override string[] ReadAllLines(string path, Encoding encoding)
        {
            return encoding
                .GetString(mockFileDataAccessor.GetFile(path).Contents)
                .SplitLines();
        }

        public override string ReadAllText(string path)
        {
            return mockFileDataAccessor.GetFile(path).TextContents;
        }

        public override string ReadAllText(string path, Encoding encoding)
        {
            return encoding.GetString(mockFileDataAccessor.GetFile(path).Contents);
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            throw new NotImplementedException();
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException();
        }

        public override void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            throw new NotImplementedException();
        }

        public override void SetAttributes(string path, FileAttributes fileAttributes)
        {
            throw new NotImplementedException();
        }

        public override void SetCreationTime(string path, DateTime creationTime)
        {
            throw new NotImplementedException();
        }

        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            throw new NotImplementedException();
        }

        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            throw new NotImplementedException();
        }

        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            throw new NotImplementedException();
        }

        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            throw new NotImplementedException();
        }

        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            throw new NotImplementedException();
        }

        public override void WriteAllBytes(string path, byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public override void WriteAllLines(string path, string[] contents)
        {
            throw new NotImplementedException();
        }

        public override void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            throw new NotImplementedException();
        }

        public override void WriteAllText(string path, string contents)
        {
            throw new NotImplementedException();
        }

        public override void WriteAllText(string path, string contents, Encoding encoding)
        {
            throw new NotImplementedException();
        }
    }
}