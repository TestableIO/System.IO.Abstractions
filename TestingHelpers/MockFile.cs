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
            AppendAllLines(path, contents, MockFileData.DefaultEncoding);
        }

        public override void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            var concatContents = contents.Aggregate("", (a, b) => a + b + Environment.NewLine);
            AppendAllText(path, concatContents, encoding);
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
                    throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find a part of the path '{0}'.", path));
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

            return new StreamWriter(Create(path));
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
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override Stream Create(string path, int bufferSize, FileOptions options)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
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
            return mockFileDataAccessor.FileExists(path) && !mockFileDataAccessor.AllDirectories.Any(d => d.Equals(path, StringComparison.OrdinalIgnoreCase));
        }

        public override FileSecurity GetAccessControl(string path)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override FileSecurity GetAccessControl(string path, AccessControlSections includeSections)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
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
                    var parentDirectoryInfo = directoryInfo.Parent;
                    if (!parentDirectoryInfo.Exists)
                    {
                        throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture,
                            Properties.Resources.COULD_NOT_FIND_PART_OF_PATH_EXCEPTION, path));
                    }

                    throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "Could not find file '{0}'.", path));
                }
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

        public override void Move(string sourceFileName, string destFileName)
        {
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
        private void ValidateParameter(string value, string paramName)
        {
            if (value == null)
                throw new ArgumentNullException(paramName, "File name cannot be null.");
            if (value == string.Empty)
                throw new ArgumentException("Empty file name is not legal.", paramName);
            if (value.Trim() == "")
                throw new ArgumentException("The path is not of a legal form.");
            if (ExtractFileName(value).IndexOfAny(mockPath.GetInvalidFileNameChars()) > -1)
                throw new NotSupportedException("The given path's format is not supported.");
            if (ExtractFilePath(value).IndexOfAny(mockPath.GetInvalidPathChars()) > -1)
                throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
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
            return ReadAllText(path, MockFileData.DefaultEncoding);
        }

        public override string ReadAllText(string path, Encoding encoding)
        {
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }

            return ReadAllTextInternal(path, encoding);
        }

        public override IEnumerable<string> ReadLines(string path)
        {
            return ReadAllLines(path);
        }

        public override IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return ReadAllLines(path, encoding);
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
        }

        public override void SetAccessControl(string path, FileSecurity fileSecurity)
        {
            throw new NotImplementedException(Properties.Resources.NOT_IMPLEMENTED_EXCEPTION);
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
            if (path == null)
            {
                throw new ArgumentNullException("path", "Path cannot be null.");
            }

            if (bytes == null)
            {
                throw new ArgumentNullException("bytes", Properties.Resources.VALUE_CANNOT_BE_NULL);
            }

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
            if (contents == null)
            {
                throw new ArgumentNullException("contents", Properties.Resources.VALUE_CANNOT_BE_NULL);
            }

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
            if (contents == null)
            {
                throw new ArgumentNullException("contents", Properties.Resources.VALUE_CANNOT_BE_NULL);
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding", Properties.Resources.VALUE_CANNOT_BE_NULL);
            }

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
            if (contents == null)
            {
                throw new ArgumentNullException("contents", Properties.Resources.VALUE_CANNOT_BE_NULL);
            }

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
            if (contents == null)
            {
                throw new ArgumentNullException("contents", Properties.Resources.VALUE_CANNOT_BE_NULL);
            }

            if (encoding == null)
            {
                throw new ArgumentNullException("encoding", Properties.Resources.VALUE_CANNOT_BE_NULL);
            }

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
            if (path == null)
            {
                throw new ArgumentNullException("path", Properties.Resources.VALUE_CANNOT_BE_NULL);
            }

            if (mockFileDataAccessor.Directory.Exists(path))
            {
                throw new UnauthorizedAccessException(string.Format(CultureInfo.InvariantCulture, Properties.Resources.ACCESS_TO_THE_PATH_IS_DENIED, path));
            }

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
    }
}