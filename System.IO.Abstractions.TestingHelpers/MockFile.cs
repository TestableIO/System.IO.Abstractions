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
        private readonly IMockFileDataAccessor mockFileDataAccessor;
        private readonly MockPath mockPath;

        public MockFile(IMockFileDataAccessor mockFileDataAccessor) : base(mockFileDataAccessor?.FileSystem)
        {
            this.mockFileDataAccessor = mockFileDataAccessor ?? throw new ArgumentNullException(nameof(mockFileDataAccessor));
            mockPath = new MockPath(mockFileDataAccessor);
        }

        public override void AppendAllLines(string path, IEnumerable<string> contents)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");

            AppendAllLines(path, contents, MockFileData.DefaultEncoding);
        }

        public override void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            var concatContents = contents.Aggregate("", (a, b) => a + b + Environment.NewLine);
            AppendAllText(path, concatContents, encoding);
        }

        public override void AppendAllText(string path, string contents)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            AppendAllText(path, contents, MockFileData.DefaultEncoding);
        }

        public override void AppendAllText(string path, string contents, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!mockFileDataAccessor.FileExists(path))
            {
                var dir = mockFileDataAccessor.Path.GetDirectoryName(path);
                if (!mockFileDataAccessor.Directory.Exists(dir))
                {
                    throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), path));
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
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (mockFileDataAccessor.FileExists(path))
            {
                StreamWriter sw = new StreamWriter(OpenWrite(path));
                sw.BaseStream.Seek(0, SeekOrigin.End); //push the stream pointer at the end for append.
                return sw;
            }

            return new StreamWriter(Create(path));
        }

        public override void Copy(string sourceFileName, string destFileName)
        {
            Copy(sourceFileName, destFileName, false);
        }

        public override void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException(nameof(sourceFileName), StringResources.Manager.GetString("FILENAME_CANNOT_BE_NULL"));
            }

            if (destFileName == null)
            {
                throw new ArgumentNullException(nameof(destFileName), StringResources.Manager.GetString("FILENAME_CANNOT_BE_NULL"));
            }

            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(sourceFileName, "sourceFileName");
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(destFileName, "destFileName");

            if (!Exists(sourceFileName))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_FILE_EXCEPTION"), sourceFileName));
            }

            var directoryNameOfDestination = mockPath.GetDirectoryName(destFileName);
            if (!mockFileDataAccessor.Directory.Exists(directoryNameOfDestination))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), destFileName));
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

            var sourceFileData = mockFileDataAccessor.GetFile(sourceFileName);
            mockFileDataAccessor.AddFile(destFileName, new MockFileData(sourceFileData));
        }

        public override Stream Create(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path), "Path cannot be null.");
            }
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            var directoryPath = mockPath.GetDirectoryName(path);
            if (!mockFileDataAccessor.Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), path));
            }

            mockFileDataAccessor.AddFile(path, new MockFileData(new byte[0]));
            var stream = OpenWrite(path);
            return stream;
        }

        public override Stream Create(string path, int bufferSize)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

        public override Stream Create(string path, int bufferSize, FileOptions options)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }

#if NET40
        public override Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            throw new NotImplementedException(StringResources.Manager.GetString("NOT_IMPLEMENTED_EXCEPTION"));
        }
#endif

        public override StreamWriter CreateText(string path)
        {
            return new StreamWriter(Create(path));
        }

#if NET40
        public override void Decrypt(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            new MockFileInfo(mockFileDataAccessor, path).Decrypt();
        }
#endif
        public override void Delete(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            mockFileDataAccessor.RemoveFile(path);
        }

#if NET40
        public override void Encrypt(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            new MockFileInfo(mockFileDataAccessor, path).Encrypt();
        }
#endif

        public override bool Exists(string path)
        {
            if (path == null)
            {
                return false;
            }

            var file = mockFileDataAccessor.GetFile(path);
            return file != null && !file.IsDirectory;
        }

        public override FileSecurity GetAccessControl(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!mockFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_FILE_EXCEPTION"), path));
            }

            var fileData = mockFileDataAccessor.GetFile(path);
            return fileData.AccessControl;
        }

        public override FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            return GetAccessControl(path);
        }

        /// <summary>
        /// Gets the <see cref="FileAttributes"/> of the file on the path.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <returns>The <see cref="FileAttributes"/> of the file on the path.</returns>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="FileNotFoundException"><paramref name="path"/> represents a file and is invalid, such as being on an unmapped drive, or the file cannot be found.</exception>
        /// <exception cref="DirectoryNotFoundException"><paramref name="path"/> represents a directory and is invalid, such as being on an unmapped drive, or the directory cannot be found.</exception>
        /// <exception cref="IOException">This file is being used by another process.</exception>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        public override FileAttributes GetAttributes(string path)
        {
            if (path != null && path.Length == 0)
            {
                throw new ArgumentException(StringResources.Manager.GetString("THE_PATH_IS_NOT_OF_A_LEGAL_FORM"), "path");
            }

            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            var possibleFileData = mockFileDataAccessor.GetFile(path);
            FileAttributes result;
            if (possibleFileData != null)
            {
                result = possibleFileData.Attributes;
            }
            else
            {
                var directoryInfo = mockFileDataAccessor.DirectoryInfo.FromDirectoryName(path);
                if (directoryInfo.Exists)
                {
                    result = directoryInfo.Attributes;
                }
                else
                {
                    VerifyDirectoryExists(path);

                    throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find file '{0}'.", path));
                }
            }

            return result;
        }

        public override DateTime GetCreationTime(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.CreationTime.LocalDateTime, () => MockFileData.DefaultDateTimeOffset.LocalDateTime);
        }

        public override DateTime GetCreationTimeUtc(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.CreationTime.UtcDateTime, () => MockFileData.DefaultDateTimeOffset.UtcDateTime);
        }

        public override DateTime GetLastAccessTime(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.LastAccessTime.LocalDateTime, () => MockFileData.DefaultDateTimeOffset.LocalDateTime);
        }

        public override DateTime GetLastAccessTimeUtc(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.LastAccessTime.UtcDateTime, () => MockFileData.DefaultDateTimeOffset.UtcDateTime);
        }

        public override DateTime GetLastWriteTime(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.LastWriteTime.LocalDateTime, () => MockFileData.DefaultDateTimeOffset.LocalDateTime);
        }

        public override DateTime GetLastWriteTimeUtc(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return GetTimeFromFile(path, data => data.LastWriteTime.UtcDateTime, () => MockFileData.DefaultDateTimeOffset.UtcDateTime);
        }

        private DateTime GetTimeFromFile(string path, Func<MockFileData, DateTime> existingFileFunction, Func<DateTime> nonExistingFileFunction)
        {
            DateTime result;
            MockFileData file = mockFileDataAccessor.GetFile(path);
            if (file != null)
            {
                result = existingFileFunction(file);
            }
            else
            {
                result = nonExistingFileFunction();
            }

            return result;
        }

        public override void Move(string sourceFileName, string destFileName)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException(nameof(sourceFileName), StringResources.Manager.GetString("FILENAME_CANNOT_BE_NULL"));
            }

            if (destFileName == null)
            {
                throw new ArgumentNullException(nameof(destFileName), StringResources.Manager.GetString("FILENAME_CANNOT_BE_NULL"));
            }

            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(sourceFileName, "sourceFileName");
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(destFileName, "destFileName");

            if (mockFileDataAccessor.GetFile(destFileName) != null)
            {
                if (destFileName.Equals(sourceFileName))
                {
                    return;
                }
                else
                {
                    throw new IOException("A file can not be created if it already exists.");
                }
            }


            var sourceFile = mockFileDataAccessor.GetFile(sourceFileName);

            if (sourceFile == null)
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "The file \"{0}\" could not be found.", sourceFileName), sourceFileName);

            VerifyDirectoryExists(destFileName);

            mockFileDataAccessor.AddFile(destFileName, new MockFileData(sourceFile.Contents));
            mockFileDataAccessor.RemoveFile(sourceFileName);
        }

        public override Stream Open(string path, FileMode mode)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return Open(path, mode, (mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite), FileShare.None);
        }

        public override Stream Open(string path, FileMode mode, FileAccess access)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return Open(path, mode, access, FileShare.None);
        }

        public override Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

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

            MockFileStream.StreamType streamType = MockFileStream.StreamType.WRITE;
            if (access == FileAccess.Read)
                streamType = MockFileStream.StreamType.READ;
            else if (mode == FileMode.Append)
                streamType = MockFileStream.StreamType.APPEND;

            return new MockFileStream(mockFileDataAccessor, path, streamType);
        }

        public override Stream OpenRead(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public override StreamReader OpenText(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return new StreamReader(
                OpenRead(path));
        }

        public override Stream OpenWrite(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return Open(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
        }

        public override byte[] ReadAllBytes(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!mockFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_FILE_EXCEPTION"), path));
            }

            return mockFileDataAccessor.GetFile(path).Contents;
        }

        public override string[] ReadAllLines(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!mockFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_FILE_EXCEPTION"), path));
            }

            return mockFileDataAccessor
                .GetFile(path)
                .TextContents
                .SplitLines();
        }

        public override string[] ReadAllLines(string path, Encoding encoding)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            if (!mockFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
            }

            return encoding
                .GetString(mockFileDataAccessor.GetFile(path).Contents)
                .SplitLines();
        }

        public override string ReadAllText(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!mockFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path));
            }

            return ReadAllText(path, MockFileData.DefaultEncoding);
        }

        public override string ReadAllText(string path, Encoding encoding)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return ReadAllTextInternal(path, encoding);
        }

        public override IEnumerable<string> ReadLines(string path)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            return ReadAllLines(path);
        }

        public override IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(encoding, "encoding");

            return ReadAllLines(path, encoding);
        }

#if NET40
        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            Replace(sourceFileName, destinationFileName, destinationBackupFileName, false);
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            if (sourceFileName == null)
            {
                throw new ArgumentNullException(nameof(sourceFileName));
            }

            if (destinationFileName == null)
            {
                throw new ArgumentNullException(nameof(destinationFileName));
            }

            if (!mockFileDataAccessor.FileExists(sourceFileName))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_FILE_EXCEPTION"), sourceFileName));
            }

            if (!mockFileDataAccessor.FileExists(destinationFileName))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_FILE_EXCEPTION"), destinationFileName));
            }

            var mockFile = new MockFile(mockFileDataAccessor);

            if (destinationBackupFileName != null)
            {
                mockFile.Copy(destinationFileName, destinationBackupFileName, true);
            }

            mockFile.Delete(destinationFileName);
            mockFile.Move(sourceFileName, destinationFileName);
        }
#endif

        public override void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            if (!mockFileDataAccessor.FileExists(path))
            {
                throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Can't find {0}", path), path);
            }

            var fileData = mockFileDataAccessor.GetFile(path);
            fileData.AccessControl = fileSecurity;
        }

        public override void SetAttributes(string path, FileAttributes fileAttributes)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            var possibleFileData = mockFileDataAccessor.GetFile(path);
            if (possibleFileData == null)
            {
                var directoryInfo = mockFileDataAccessor.DirectoryInfo.FromDirectoryName(path);
                if (directoryInfo.Exists)
                {
                    directoryInfo.Attributes = fileAttributes;
                }
                else
                {
                    throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_FILE_EXCEPTION"), path), path);
                }
            }
            else
            {
                possibleFileData.Attributes = fileAttributes;
            }
        }

        public override void SetCreationTime(string path, DateTime creationTime)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            mockFileDataAccessor.GetFile(path).CreationTime = new DateTimeOffset(creationTime);
        }

        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            mockFileDataAccessor.GetFile(path).CreationTime = new DateTimeOffset(creationTimeUtc, TimeSpan.Zero);
        }

        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            mockFileDataAccessor.GetFile(path).LastAccessTime = new DateTimeOffset(lastAccessTime);
        }

        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            mockFileDataAccessor.GetFile(path).LastAccessTime = new DateTimeOffset(lastAccessTimeUtc, TimeSpan.Zero);
        }

        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            mockFileDataAccessor.GetFile(path).LastWriteTime = new DateTimeOffset(lastWriteTime);
        }

        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");

            mockFileDataAccessor.GetFile(path).LastWriteTime = new DateTimeOffset(lastWriteTimeUtc, TimeSpan.Zero);
        }

        /// <summary>
        /// Creates a new file, writes the specified byte array to the file, and then closes the file.
        /// If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="bytes">The bytes to write to the file. </param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/> or contents is empty.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// path specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// path specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// Given a byte array and a file path, this method opens the specified file, writes the contents of the byte array to the file, and then closes the file.
        /// </remarks>
        public override void WriteAllBytes(string path, byte[] bytes)
        {
            VerifyValueIsNotNull(bytes, "bytes");

            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyDirectoryExists(path);

            mockFileDataAccessor.AddFile(path, new MockFileData(bytes));
        }

       /// <summary>
        /// Creates a new file, writes a collection of strings to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The lines to write to the file.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <remarks>
        /// <para>
        ///     If the target file already exists, it is overwritten.
        /// </para>
        /// <para>
        ///     You can use this method to create the contents for a collection class that takes an <see cref="IEnumerable{T}"/> in its constructor, such as a <see cref="List{T}"/>, <see cref="HashSet{T}"/>, or a <see cref="SortedSet{T}"/> class.
        /// </para>
        /// </remarks>
        public override void WriteAllLines(string path, IEnumerable<string> contents)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");

            WriteAllLines(path, contents, MockFileData.DefaultEncoding);
        }

        /// <summary>
        /// Creates a new file by using the specified encoding, writes a collection of strings to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The lines to write to the file.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException">Either <paramref name="path"/>, <paramref name="contents"/>, or <paramref name="encoding"/> is <see langword="null"/>.</exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <remarks>
        /// <para>
        ///     If the target file already exists, it is overwritten.
        /// </para>
        /// <para>
        ///     You can use this method to create a file that contains the following:
        /// <list type="bullet">
        /// <item>
        /// <description>The results of a LINQ to Objects query on the lines of a file, as obtained by using the ReadLines method.</description>
        /// </item>
        /// <item>
        /// <description>The contents of a collection that implements an <see cref="IEnumerable{T}"/> of strings.</description>
        /// </item>
        /// </list>
        /// </para>
        /// </remarks>
        public override void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");
            VerifyValueIsNotNull(encoding, "encoding");

            var sb = new StringBuilder();
            foreach (var line in contents)
            {
                sb.AppendLine(line);
            }

            WriteAllText(path, sb.ToString(), encoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// <para>
        ///     If the target file already exists, it is overwritten.
        /// </para>
        /// <para>
        ///     The default behavior of the WriteAllLines method is to write out data using UTF-8 encoding without a byte order mark (BOM). If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file, use the <see cref="FileBase.WriteAllLines(string,string[],System.Text.Encoding)"/> method overload with <see cref="UTF8Encoding"/> encoding.
        /// </para>
        /// <para>
        ///     Given a string array and a file path, this method opens the specified file, writes the string array to the file using the specified encoding,
        ///     and then closes the file.
        /// </para>
        /// </remarks>
        public override void WriteAllLines(string path, string[] contents)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");

            WriteAllLines(path, contents, MockFileData.DefaultEncoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string array to the file by using the specified encoding, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="contents">The string array to write to the file.</param>
        /// <param name="encoding">An <see cref="Encoding"/> object that represents the character encoding applied to the string array.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException">Either <paramref name="path"/> or <paramref name="contents"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// <paramref name="path"/> specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// <paramref name="path"/> specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// <para>
        ///     If the target file already exists, it is overwritten.
        /// </para>
        /// <para>
        ///     Given a string array and a file path, this method opens the specified file, writes the string array to the file using the specified encoding,
        ///     and then closes the file.
        /// </para>
        /// </remarks>
        public override void WriteAllLines(string path, string[] contents, Encoding encoding)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(contents, "contents");
            VerifyValueIsNotNull(encoding, "encoding");

            WriteAllLines(path, new List<string>(contents), encoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to. </param>
        /// <param name="contents">The string to write to the file. </param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/> or contents is empty.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// path specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// path specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// This method uses UTF-8 encoding without a Byte-Order Mark (BOM), so using the <see cref="M:Encoding.GetPreamble"/> method will return an empty byte array.
        /// If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file, use the <see cref="FileBase.WriteAllText(string,string,System.Text.Encoding)"/> method overload with <see cref="UTF8Encoding"/> encoding.
        /// <para>
        /// Given a string and a file path, this method opens the specified file, writes the string to the file, and then closes the file.
        /// </para>
        /// </remarks>
        public override void WriteAllText(string path, string contents)
        {
            WriteAllText(path, contents, MockFileData.DefaultEncoding);
        }

        /// <summary>
        /// Creates a new file, writes the specified string to the file using the specified encoding, and then closes the file. If the target file already exists, it is overwritten.
        /// </summary>
        /// <param name="path">The file to write to. </param>
        /// <param name="contents">The string to write to the file. </param>
        /// <param name="encoding">The encoding to apply to the string.</param>
        /// <exception cref="ArgumentException"><paramref name="path"/> is a zero-length string, contains only white space, or contains one or more invalid characters as defined by <see cref="Path.GetInvalidPathChars"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/> or contents is empty.</exception>
        /// <exception cref="PathTooLongException">
        /// The specified path, file name, or both exceed the system-defined maximum length.
        /// For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.
        /// </exception>
        /// <exception cref="DirectoryNotFoundException">The specified path is invalid (for example, it is on an unmapped drive).</exception>
        /// <exception cref="IOException">An I/O error occurred while opening the file.</exception>
        /// <exception cref="UnauthorizedAccessException">
        /// path specified a file that is read-only.
        /// -or-
        /// This operation is not supported on the current platform.
        /// -or-
        /// path specified a directory.
        /// -or-
        /// The caller does not have the required permission.
        /// </exception>
        /// <exception cref="FileNotFoundException">The file specified in <paramref name="path"/> was not found.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <exception cref="System.Security.SecurityException">The caller does not have the required permission.</exception>
        /// <remarks>
        /// Given a string and a file path, this method opens the specified file, writes the string to the file using the specified encoding, and then closes the file.
        /// The file handle is guaranteed to be closed by this method, even if exceptions are raised.
        /// </remarks>
        public override void WriteAllText(string path, string contents, Encoding encoding)
        {
            mockFileDataAccessor.PathVerifier.IsLegalAbsoluteOrRelative(path, "path");
            VerifyValueIsNotNull(path, "path");

            if (mockFileDataAccessor.Directory.Exists(path))
            {
                throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("ACCESS_TO_THE_PATH_IS_DENIED"), path));
            }

            VerifyDirectoryExists(path);

            MockFileData data = contents == null ? new MockFileData(new byte[0]) : new MockFileData(contents, encoding);
            mockFileDataAccessor.AddFile(path, data);
        }

        internal static string ReadAllBytes(byte[] contents, Encoding encoding)
        {
            using (var ms = new MemoryStream(contents))
            using (var sr = new StreamReader(ms, encoding))
            {
                return sr.ReadToEnd();
            }
        }

        private string ReadAllTextInternal(string path, Encoding encoding)
        {
            var mockFileData = mockFileDataAccessor.GetFile(path);
            return ReadAllBytes(mockFileData.Contents, encoding);
        }

        private void VerifyValueIsNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName, StringResources.Manager.GetString("VALUE_CANNOT_BE_NULL"));
            }
        }

        private void VerifyDirectoryExists(string path)
        {
            DirectoryInfoBase dir = mockFileDataAccessor.Directory.GetParent(path);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, StringResources.Manager.GetString("COULD_NOT_FIND_PART_OF_PATH_EXCEPTION"), dir));
            }
        }
    }
}
