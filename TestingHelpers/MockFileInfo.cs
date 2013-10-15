using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
	[Serializable]
	internal class MockFileInfo : FileInfoBase
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
			get { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
			set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
		}

		public override DateTime CreationTimeUtc
		{
			get
			{
				if (MockFileData == null) throw new FileNotFoundException("File not found", path);
				return MockFileData.CreationTime.UtcDateTime;
			}
			set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
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
			get { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
			set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
		}

		public override DateTime LastAccessTimeUtc
		{
			get
			{
				if (MockFileData == null) throw new FileNotFoundException("File not found", path);
				return MockFileData.LastAccessTime.UtcDateTime;
			}
			set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
		}

		public override DateTime LastWriteTime
		{
			get
			{
							if (MockFileData == null) throw new FileNotFoundException("File not found", path);
							return MockFileData.LastWriteTime.DateTime;
						}
			set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
		}

		public override DateTime LastWriteTimeUtc
		{
			get
			{
				if (MockFileData == null) throw new FileNotFoundException("File not found", path);
				return MockFileData.LastWriteTime.UtcDateTime;    
			}
			set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
		}

		public override string Name {
			get { return new MockPath(mockFileSystem).GetFileName(path); }
		}

		public override StreamWriter AppendText()
		{
			throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
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
			throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
		}

	    public override StreamWriter CreateText()
	    {
	        if (!string.IsNullOrEmpty(path))
	            return new StreamWriter(path);

	        throw new InvalidOperationException("Path must not be empty");
	    }

	    public override void Decrypt()
		{
			throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
		}

		public override void Encrypt()
		{
			throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
		}

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
			throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
		}

		public override Stream Open(FileMode mode, FileAccess access)
		{
			throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
		}

		public override Stream Open(FileMode mode, FileAccess access, FileShare share)
		{
			throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
		}

		public override Stream OpenRead()
		{
			return new MockFileStream(mockFileSystem, path);
		}

		public override StreamReader OpenText()
		{
			throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
		}

		public override Stream OpenWrite()
		{
			throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all.");
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
			get { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
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
			get { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
			set { throw new NotImplementedException("This test helper hasn't been implemented yet. They are implemented on an as-needed basis. As it seems like you need it, now would be a great time to send us a pull request over at https://github.com/tathamoddie/System.IO.Abstractions. You know, because it's open source and all."); }
		}

		public override long Length
		{
			get
			{
				if (MockFileData == null) throw new FileNotFoundException("File not found", path);
				return MockFileData.Contents.LongLength;
			}
		}
	}
}