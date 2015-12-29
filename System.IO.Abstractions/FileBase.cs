using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace System.IO.Abstractions
{
    [Serializable]
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
        public abstract Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity);
        public abstract StreamWriter CreateText(string path);
        public abstract void Decrypt(string path);
        public abstract void Delete(string path);
        public abstract void Encrypt(string path);
        public abstract bool Exists(string path);
        public abstract FileSecurity GetAccessControl(string path);
        public abstract FileSecurity GetAccessControl(string path, AccessControlSections includeSections);

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
        public abstract void WriteAllBytes(string path, byte[] bytes);
        public abstract void WriteAllLines(String path, IEnumerable<String> contents);
        public abstract void WriteAllLines(String path, IEnumerable<String> contents, Encoding encoding);
        public abstract void WriteAllLines(string path, string[] contents);
        public abstract void WriteAllLines(string path, string[] contents, Encoding encoding);

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
        /// If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file, use the <see cref="WriteAllText(string,string, Encoding)"/> method overload with <see cref="UTF8Encoding"/> encoding.
        /// <para>
        /// Given a string and a file path, this method opens the specified file, writes the string to the file, and then closes the file.
        /// </para>
        /// </remarks>
        public abstract void WriteAllText(string path, string contents);

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
        public abstract void WriteAllText(string path, string contents, Encoding encoding);
    }
}