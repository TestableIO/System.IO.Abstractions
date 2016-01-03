using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
#if NET40
    [Serializable]
#endif
    public class MockFileInfo : FileInfoBase
    {
        readonly IMockFileDataAccessor mockFileSystem;
        readonly string path;

        public MockFileInfo(IMockFileDataAccessor mockFileSystem, string path)
        {
            this.mockFileSystem = mockFileSystem;
            this.path = path;
        }

        MockFileData MockFileData
        {
            get { return mockFileSystem.GetFile(path); }
        }

        public override void Delete()
        {
            mockFileSystem.RemoveFile(path);
        }

        public override void Refresh()
        {
        }

        public override FileAttributes Attributes
        {
            get
            {
                if (MockFileData == null)
                    throw new FileNotFoundException("File not found", path);
                return MockFileData.Attributes;
            }
            set { MockFileData.Attributes = value; }
        }

        public override DateTime CreationTime
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.CreationTime.DateTime;
            }
            set
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                MockFileData.CreationTime = value;
            }
        }

        public override DateTime CreationTimeUtc
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.CreationTime.UtcDateTime;
            }
            set
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                MockFileData.CreationTime = value.ToLocalTime();
            }
        }

        public override bool Exists
        {
            get { return MockFileData != null; }
        }

        public override string Extension
        {
            get
            {
                // System.IO.Path.GetExtension does only string manipulation,
                // so it's safe to delegate.
                return Path.GetExtension(this.path);
            }
        }

        public override string FullName
        {
            get { return path; }
        }

        public override DateTime LastAccessTime
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.LastAccessTime.DateTime;
            }
            set
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                MockFileData.LastAccessTime = value;
            }
        }

        public override DateTime LastAccessTimeUtc
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.LastAccessTime.UtcDateTime;
            }
            set
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                MockFileData.LastAccessTime = value;
            }
        }

        public override DateTime LastWriteTime
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.LastWriteTime.DateTime;
            }
            set
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                MockFileData.LastWriteTime = value;
            }
        }

        public override DateTime LastWriteTimeUtc
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return MockFileData.LastWriteTime.UtcDateTime;    
            }
            set
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                MockFileData.LastWriteTime = value.ToLocalTime();
            }
        }

        public override string Name {
            get { return new MockPath(mockFileSystem).GetFileName(path); }
        }

        public override StreamWriter AppendText()
        {
            if (MockFileData == null) throw new FileNotFoundException("File not found", path);
            return new StreamWriter(new MockFileStream(mockFileSystem, FullName, true));
            //return ((MockFileDataModifier) MockFileData).AppendText();
        }

        public override FileInfoBase CopyTo(string destFileName)
        {
            new MockFile(mockFileSystem).Copy(FullName, destFileName);
            return mockFileSystem.FileInfo.FromFileName(destFileName);
        }
        
        public override FileInfoBase CopyTo(string destFileName, bool overwrite)
        {
            new MockFile(mockFileSystem).Copy(FullName, destFileName, overwrite);
            return mockFileSystem.FileInfo.FromFileName(destFileName);
        }

        public override Stream Create()
        {
            return new MockFile(mockFileSystem).Create(FullName);
        }

        public override StreamWriter CreateText()
        {
            return new MockFile(mockFileSystem).CreateText(FullName);
        }

#if NET40
        public override void Decrypt()
        {
            if (MockFileData == null) throw new FileNotFoundException("File not found", path);
            var contents = MockFileData.Contents;
            for (var i = 0; i < contents.Length; i++)
                contents[i] ^= (byte)(i % 256);
        }

        public override void Encrypt()
        {
            if (MockFileData == null) throw new FileNotFoundException("File not found", path);
            var contents = MockFileData.Contents;
            for(var i = 0; i < contents.Length; i++)
                contents[i] ^= (byte) (i % 256);
        }
#endif

        public override FileSecurity GetAccessControl()
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override FileSecurity GetAccessControl(AccessControlSections includeSections)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void MoveTo(string destFileName)
        {
            CopyTo(destFileName);
            Delete();
        }

        public override Stream Open(FileMode mode)
        {
            return new MockFile(mockFileSystem).Open(FullName, mode);
        }

        public override Stream Open(FileMode mode, FileAccess access)
        {
            return new MockFile(mockFileSystem).Open(FullName, mode, access);
        }

        public override Stream Open(FileMode mode, FileAccess access, FileShare share)
        {
            return new MockFile(mockFileSystem).Open(FullName, mode, access, share);
        }

        public override Stream OpenRead()
        {
            return new MockFileStream(mockFileSystem, path);
        }

        public override StreamReader OpenText()
        {
          return new StreamReader(OpenRead());
        }

        public override Stream OpenWrite()
        {
            return new MockFileStream(mockFileSystem, path);
        }

        public override FileInfoBase Replace(string destinationFileName, string destinationBackupFileName)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override FileInfoBase Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override void SetAccessControl(FileSecurity fileSecurity)
        {
            throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
        }

        public override DirectoryInfoBase Directory
        {
            get
            {
                return mockFileSystem.DirectoryInfo.FromDirectoryName(this.DirectoryName);
            }
        }

        public override string DirectoryName
        {
            get
            {
                // System.IO.Path.GetDirectoryName does only string manipulation,
                // so it's safe to delegate.
                return Path.GetDirectoryName(this.path);
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                return (MockFileData.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            }
            set
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
                if(value)
                    MockFileData.Attributes |= FileAttributes.ReadOnly;
                else 
                    MockFileData.Attributes &= ~FileAttributes.ReadOnly;
            }
        }

        public override long Length
        {
            get
            {
                if (MockFileData == null) throw new FileNotFoundException("File not found", path);
#if NET40
                return MockFileData.Contents.LongLength;
#elif DOTNET5_4
                return (long) MockFileData.Contents.Length;
#endif
            }
        }

    }
}