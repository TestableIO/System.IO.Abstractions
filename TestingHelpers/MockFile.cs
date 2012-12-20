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
            mockFileDataAccessor
                .GetFile(path)
                .TextContents += contents;
        }

        public override void AppendAllText(string path, string contents, Encoding encoding)
        {
            var file = mockFileDataAccessor.GetFile(path);
            var originalText = encoding.GetString(file.Contents);
            var newText = originalText + contents;
            file.Contents = encoding.GetBytes(newText);
        }

        public override StreamWriter AppendText(string path)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void Copy(string sourceFileName, string destFileName)
        {
            if (mockFileDataAccessor.FileExists(destFileName))
				throw new IOException(string.Format("The file {0} already exists.", destFileName));

			mockFileDataAccessor.AddFile(destFileName, mockFileDataAccessor.GetFile(sourceFileName));
        }

        public override void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if(overwrite)
			{
				if (mockFileDataAccessor.FileExists(destFileName))
				{
					var sourceFile = mockFileDataAccessor.GetFile(sourceFileName);
					mockFileDataAccessor.RemoveFile(destFileName);
					mockFileDataAccessor.AddFile(destFileName, sourceFile);
					return;
				}
			}

            Copy(sourceFileName, destFileName);
        }

        public override Stream Create(string path)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override Stream Create(string path, int bufferSize)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override Stream Create(string path, int bufferSize, FileOptions options)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override StreamWriter CreateText(string path)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void Decrypt(string path)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

		public override void Delete(string path)
		{
			mockFileDataAccessor.RemoveFile(path);
		}

        public override void Encrypt(string path)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override bool Exists(string path)
        {
            return mockFileDataAccessor.FileExists(path);
        }

        public override FileSecurity GetAccessControl(string path)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override FileAttributes GetAttributes(string path)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override DateTime GetCreationTime(string path)
        {
            return mockFileDataAccessor.GetFile(path).CreationTime.LocalDateTime;
        }

        public override DateTime GetCreationTimeUtc(string path)
        {
            return mockFileDataAccessor.GetFile(path).CreationTime.UtcDateTime;
        }

        public override DateTime GetLastAccessTime(string path)
        {
            return mockFileDataAccessor.GetFile(path).LastAccessTime.LocalDateTime;
        }

        public override DateTime GetLastAccessTimeUtc(string path)
        {
            return mockFileDataAccessor.GetFile(path).LastAccessTime.UtcDateTime;
        }

        public override DateTime GetLastWriteTime(string path)
        {
            return mockFileDataAccessor.GetFile(path).LastWriteTime.LocalDateTime;
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return mockFileDataAccessor.GetFile(path).LastWriteTime.UtcDateTime;
        }

        public override void Move(string sourceFileName, string destFileName)
        {
            var sourceFile = mockFileDataAccessor.GetFile(sourceFileName);

            mockFileDataAccessor.AddFile(destFileName, new MockFileData(sourceFile.Contents));
            mockFileDataAccessor.RemoveFile(sourceFileName);
        }

        public override Stream Open(string path, FileMode mode)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override Stream Open(string path, FileMode mode, FileAccess access)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override Stream OpenRead(string path)
        {
            return new MemoryStream(
                mockFileDataAccessor
                    .GetFile(path)
                    .Contents);
        }

        public override StreamReader OpenText(string path)
        {
            return new StreamReader(
                OpenRead(path));
        }

        public override Stream OpenWrite(string path)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
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
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void SetAttributes(string path, FileAttributes fileAttributes)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void SetCreationTime(string path, DateTime creationTime)
        {
            mockFileDataAccessor.GetFile(path).CreationTime = new DateTimeOffset(creationTime);
        }

        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            mockFileDataAccessor.GetFile(path).CreationTime = new DateTimeOffset(creationTimeUtc, TimeSpan.Zero);
        }

        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            mockFileDataAccessor.GetFile(path).LastAccessTime = new DateTimeOffset(lastAccessTime);
        }

        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            mockFileDataAccessor.GetFile(path).LastAccessTime = new DateTimeOffset(lastAccessTimeUtc, TimeSpan.Zero);
        }

        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            mockFileDataAccessor.GetFile(path).LastWriteTime = new DateTimeOffset(lastWriteTime);
        }

        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            mockFileDataAccessor.GetFile(path).LastWriteTime = new DateTimeOffset(lastWriteTimeUtc, TimeSpan.Zero);
        }

        public override void WriteAllBytes(string path, byte[] bytes)
        {
            mockFileDataAccessor.AddFile(path, new MockFileData(bytes));
        }

        public override void WriteAllLines(string path, string[] contents)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void WriteAllText(string path, string contents)
        {
            if (mockFileDataAccessor.FileExists(path))
                mockFileDataAccessor.RemoveFile(path);
            mockFileDataAccessor.AddFile(path, new MockFileData(contents));
        }

        public override void WriteAllText(string path, string contents, Encoding encoding)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }
    }
}