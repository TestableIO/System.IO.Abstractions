using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFile : FileBase
    {
        readonly IMockFileDataAccessor mockFileDataAccessor;
        readonly MockPath mockPath;

        public MockFile(IMockFileDataAccessor mockFileDataAccessor)
        {
            this.mockFileDataAccessor = mockFileDataAccessor;
            mockPath = new MockPath(mockFileDataAccessor);
        }

        public override void AppendAllLines(string path, IEnumerable<string> contents)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void AppendAllText(string path, string contents)
        {
            AppendAllText(path, contents, MockFileData.DefaultEncoding);
        }

        public override void AppendAllText(string path, string contents, Encoding encoding)
        {
            ValidateParameter(path, "path");

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (!mockFileDataAccessor.FileExists(path))
            {
                var dir = mockFileDataAccessor.Path.GetDirectoryName(path);
                if (!mockFileDataAccessor.Directory.Exists(dir))
                {
                    throw new DirectoryNotFoundException(String.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path));
                }

                mockFileDataAccessor.AddFile(path, new MockFileData(contents, encoding));
            }
            else
            {
                var file = mockFileDataAccessor.GetFile(path);
                var bytesToAppend = encoding.GetBytes(contents);
                file.Contents = file.Contents.Concat(bytesToAppend).ToArray();
            }
        }

        public override StreamWriter AppendText(string path)
        {
            if (mockFileDataAccessor.FileExists(path))
            {
                StreamWriter sw = new StreamWriter(OpenWrite(path));
                sw.BaseStream.Seek(0, SeekOrigin.End); //push the stream pointer at the end for append.
                return sw;
            }

            return new StreamWriter(this.Create(path));
        }

        public override void Copy(string sourceFileName, string destFileName)
        {
            Copy(sourceFileName, destFileName, false);
        }

        public override void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            ValidateParameter(sourceFileName, "sourceFileName");
            ValidateParameter(destFileName, "destFileName");

            var directoryNameOfDestination = mockPath.GetDirectoryName(destFileName);
            if (!mockFileDataAccessor.Directory.Exists(directoryNameOfDestination))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", destFileName));
            }

            var fileExists = mockFileDataAccessor.FileExists(destFileName);
            if (fileExists)
            {
                if (!overwrite)
                {
                    throw new IOException(string.Format(CultureInfo.InvariantCulture, "The file {0} already exists.", destFileName));
                }

                mockFileDataAccessor.RemoveFile(destFileName);
            }

            var sourceFile = mockFileDataAccessor.GetFile(sourceFileName);
            mockFileDataAccessor.AddFile(destFileName, sourceFile);
        }

        public override Stream Create(string path)
        {
            mockFileDataAccessor.AddFile(path, new MockFileData(new byte[0]));
            var stream = OpenWrite(path);
            return stream;
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
            return new StreamWriter(Create(path));
        }

        public override void Decrypt(string path)
        {
            new MockFileInfo(mockFileDataAccessor, path).Decrypt();
        }

        public override void Delete(string path)
        {
            mockFileDataAccessor.RemoveFile(path);
        }

        public override void Encrypt(string path)
        {
            new MockFileInfo(mockFileDataAccessor, path).Encrypt();
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
            var possibleFileData = mockFileDataAccessor.GetFile(path);
            FileAttributes result = FileAttributes.Normal;
            if (possibleFileData == null)
            {
                var directoryInfo = mockFileDataAccessor.DirectoryInfo.FromDirectoryName(path);
                if (directoryInfo.Exists)
                {
                    result = directoryInfo.Attributes;
                }
            }
            else
            {
                result = possibleFileData.Attributes;
            }

            return result;
        }

        public override DateTime GetCreationTime(string path)
        {
            return mockFileDataAccessor.GetFile(path, true).CreationTime.LocalDateTime;
        }

        public override DateTime GetCreationTimeUtc(string path)
        {
            return mockFileDataAccessor.GetFile(path, true).CreationTime.UtcDateTime;
        }

        public override DateTime GetLastAccessTime(string path)
        {
            return mockFileDataAccessor.GetFile(path, true).LastAccessTime.LocalDateTime;
        }

        public override DateTime GetLastAccessTimeUtc(string path)
        {
            return mockFileDataAccessor.GetFile(path, true).LastAccessTime.UtcDateTime;
        }

        public override DateTime GetLastWriteTime(string path) {
            return mockFileDataAccessor.GetFile(path, true).LastWriteTime.LocalDateTime;
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return mockFileDataAccessor.GetFile(path, true).LastWriteTime.UtcDateTime;
        }

        public override void Move(string sourceFileName, string destFileName) {
            ValidateParameter(sourceFileName, "sourceFileName");
            ValidateParameter(destFileName, "destFileName");

            if (mockFileDataAccessor.GetFile(destFileName) != null)
                throw new IOException("A file can not be created if it already exists.");

            var sourceFile = mockFileDataAccessor.GetFile(sourceFileName);

            if (sourceFile == null)
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "The file \"{0}\" could not be found.", sourceFileName), sourceFileName);

            var destDir = mockFileDataAccessor.Directory.GetParent(destFileName);
            if (!destDir.Exists)
            {
                throw new DirectoryNotFoundException("Could not find a part of the path.");
            }

            mockFileDataAccessor.AddFile(destFileName, new MockFileData(sourceFile.Contents));
            mockFileDataAccessor.RemoveFile(sourceFileName);
        }
        
        [DebuggerNonUserCode]
        private void ValidateParameter(string value, string paramName) {
            if (value == null)
                throw new ArgumentNullException(paramName, "File name cannot be null.");
            if (value == string.Empty)
                throw new ArgumentException("Empty file name is not legal.", paramName);
            if (value.Trim() == "")
                throw new ArgumentException("The path is not of a legal form.");
            if (ExtractFileName(value).IndexOfAny(mockPath.GetInvalidFileNameChars()) > -1)
                throw new NotSupportedException("The given path's format is not supported.");
            if (ExtractFilePath(value).IndexOfAny(mockPath.GetInvalidPathChars()) > -1)
                throw new ArgumentException("Illegal characters in path.");
        }

        private string ExtractFilePath(string fullFileName)
        {
            var extractFilePath = fullFileName.Split(mockPath.DirectorySeparatorChar);
            return string.Join(mockPath.DirectorySeparatorChar.ToString(), extractFilePath.Take(extractFilePath.Length - 1));
        }

        private string ExtractFileName(string fullFileName)
        {
            return fullFileName.Split(mockPath.DirectorySeparatorChar).Last();
        }

        public override Stream Open(string path, FileMode mode)
        {
            return Open(path, mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite), FileShare.None);
        }

        public override Stream Open(string path, FileMode mode, FileAccess access)
        {
            return Open(path, mode, access, FileShare.None);
        }

        public override Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            bool exists = mockFileDataAccessor.FileExists(path);

            if (mode == FileMode.CreateNew && exists)
                throw new IOException(string.Format(CultureInfo.InvariantCulture, "The file '{0}' already exists.", path));

            if ((mode == FileMode.Open || mode == FileMode.Truncate) && !exists)
                throw new FileNotFoundException(path);

            if (!exists || mode == FileMode.CreateNew)
                return Create(path);

            if (mode == FileMode.Create || mode == FileMode.Truncate)
            {
                Delete(path);
                return Create(path);
            }

            var length = mockFileDataAccessor.GetFile(path).Contents.Length;
            var stream = OpenWrite(path);

            if (mode == FileMode.Append)
                stream.Seek(length, SeekOrigin.Begin);

            return stream;
        }

        public override Stream OpenRead(string path)
        {
            return Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public override StreamReader OpenText(string path)
        {
            return new StreamReader(
                OpenRead(path));
        }

        public override Stream OpenWrite(string path)
        {
            return new MockFileStream(mockFileDataAccessor, path);
        }

        public override byte[] ReadAllBytes(string path)
        {
            return mockFileDataAccessor.GetFile(path).Contents;
        }

        public override string[] ReadAllLines(string path)
        {
            if (!mockFileDataAccessor.FileExists(path))
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
            return mockFileDataAccessor
                .GetFile(path)
                .TextContents
                .SplitLines();
        }

        public override string[] ReadAllLines(string path, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            if (!mockFileDataAccessor.FileExists(path))
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
            return encoding
                .GetString(mockFileDataAccessor.GetFile(path).Contents)
                .SplitLines();
        }

        public override string ReadAllText(string path)
        {
            if (!mockFileDataAccessor.FileExists(path))
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
            return mockFileDataAccessor.GetFile(path).TextContents;
        }

        public override string ReadAllText(string path, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            return encoding.GetString(mockFileDataAccessor.GetFile(path).Contents);
        }

        public override IEnumerable<string> ReadLines(string path)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            throw new NotImplementedException();
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
            mockFileDataAccessor.GetFile(path).Attributes = fileAttributes;
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

        public override void WriteAllLines(string path, IEnumerable<string> contents)
        {
            var sb = new StringBuilder();
            foreach (var line in contents)
            {
                sb.AppendLine(line);
            }
            WriteAllText(path, sb.ToString()); 
        }

        public override void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void WriteAllLines(string path, string[] contents)
        {
            WriteAllText(path, string.Join("\n", contents));
        }

        public override void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            WriteAllText(path, string.Join("\n", contents), encoding);
        }

        public override void WriteAllText(string path, string contents)
        {
            WriteAllText(path, new MockFileData(contents));
        }

        public override void WriteAllText(string path, string contents, Encoding encoding)
        {
            WriteAllText(path, new MockFileData(contents, encoding));
        }

        private void WriteAllText(string path, MockFileData mockFileData)
        {
            mockFileDataAccessor.AddFile(path, mockFileData);
        }
    }
}