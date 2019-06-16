using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

#if NETCOREAPP2_0
using System.Threading.Tasks;
using System.Threading;
#endif

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="File"/>
    [Serializable]
    public abstract class FileBase : IFile
    {
        protected FileBase(IFileSystem fileSystem)
        {
            FileSystem = fileSystem;
        }

        [Obsolete("This constructor only exists to support mocking libraries.", error: true)]
        internal FileBase() { }

        /// <summary>
        /// Exposes the underlying filesystem implementation. This is useful for implementing extension methods.
        /// </summary>
        public IFileSystem FileSystem { get; }

        /// <inheritdoc cref="File.AppendAllLines(string,IEnumerable{string})"/>
        public abstract void AppendAllLines(string path, IEnumerable<string> contents);

        /// <inheritdoc cref="File.AppendAllLines(string,IEnumerable{string},Encoding)"/>
        public abstract void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding);

#if NETCOREAPP2_0
        /// <inheritdoc cref="File.AppendAllLinesAsync(string,IEnumerable{string},CancellationToken)"/>
        public abstract Task AppendAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.AppendAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        public abstract Task AppendAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken);
#endif

        /// <inheritdoc cref="File.AppendAllText(string,string)"/>
        public abstract void AppendAllText(string path, string contents);

        /// <inheritdoc cref="File.AppendAllText(string,string,Encoding)"/>
        public abstract void AppendAllText(string path, string contents, Encoding encoding);

#if NETCOREAPP2_0
        /// <inheritdoc cref="File.AppendAllTextAsync(string,string,CancellationToken)"/>
        public abstract Task AppendAllTextAsync(String path, String contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.AppendAllTextAsync(string,string,Encoding,CancellationToken)"/>
        public abstract Task AppendAllTextAsync(String path, String contents, Encoding encoding, CancellationToken cancellationToken);
#endif

        /// <inheritdoc cref="File.AppendText"/>
        public abstract StreamWriter AppendText(string path);

        /// <inheritdoc cref="File.Copy(string,string)"/>
        public abstract void Copy(string sourceFileName, string destFileName);

        /// <inheritdoc cref="File.Copy(string,string,bool)"/>
        public abstract void Copy(string sourceFileName, string destFileName, bool overwrite);

        /// <inheritdoc cref="File.Create(string)"/>
        public abstract Stream Create(string path);

        /// <inheritdoc cref="File.Create(string,int)"/>
        public abstract Stream Create(string path, int bufferSize);

        /// <inheritdoc cref="File.Create(string,int,FileOptions)"/>
        public abstract Stream Create(string path, int bufferSize, FileOptions options);

#if NET40
        /// <inheritdoc cref="File.Create(string,int,FileOptions,FileSecurity)"/>
        public abstract Stream Create(string path, int bufferSize, FileOptions options, FileSecurity fileSecurity);
#endif
        /// <inheritdoc cref="File.CreateText"/>
        public abstract StreamWriter CreateText(string path);
#if NET40
        /// <inheritdoc cref="File.Decrypt"/>
        public abstract void Decrypt(string path);
#endif
        /// <inheritdoc cref="File.Delete"/>
        public abstract void Delete(string path);
#if NET40
        /// <inheritdoc cref="File.Encrypt"/>
        public abstract void Encrypt(string path);
#endif

        /// <inheritdoc cref="File.Exists"/>
        /// <summary>
        /// Determines whether the specified file exists.
        /// </summary>
        /// <param name="path">The file to check.</param>
        /// <returns><see langword="true"/> if the caller has the required permissions and path contains the name of an existing file; otherwise, <see langword="false"/>. This method also returns <see langword="false"/> if <paramref name="path"/> is <see langword="null"/>, an invalid path, or a zero-length string. If the caller does not have sufficient permissions to read the specified file, no exception is thrown and the method returns <see langword="false"/> regardless of the existence of <paramref name="path"/>.</returns>
        /// <remarks>
        /// <para>
        /// The Exists method should not be used for path validation, this method merely checks if the file specified in <paramref name="path"/> exists.
        /// Passing an invalid path to Exists returns <see langword="false"/>.
        /// </para>
        /// <para>
        /// Be aware that another process can potentially do something with the file in between the time you call the Exists method and perform another operation on the file, such as <see cref="Delete"/>.
        /// </para>
        /// <para>
        /// The <paramref name="path"/> parameter is permitted to specify relative or absolute path information.
        /// Relative path information is interpreted as relative to the current working directory.
        /// To obtain the current working directory, see <see cref="DirectoryBase.GetCurrentDirectory"/>.
        /// </para>
        /// <para>
        /// If <paramref name="path"/> describes a directory, this method returns <see langword="false"/>. Trailing spaces are removed from the <paramref name="path"/> parameter before determining if the file exists.
        /// </para>
        /// <para>
        /// The Exists method returns <see langword="false"/> if any error occurs while trying to determine if the specified file exists.
        /// This can occur in situations that raise exceptions such as passing a file name with invalid characters or too many characters
        ///  a failing or missing disk, or if the caller does not have permission to read the file.
        /// </para>
        /// </remarks>
        public abstract bool Exists(string path);

        /// <inheritdoc cref="File.GetAccessControl(string)"/>
        public abstract FileSecurity GetAccessControl(string path);

        /// <inheritdoc cref="File.GetAccessControl(string,AccessControlSections)"/>
        public abstract FileSecurity GetAccessControl(string path, AccessControlSections includeSections);

        /// <inheritdoc cref="File.GetAttributes"/>
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

        /// <inheritdoc cref="File.GetCreationTime"/>
        /// <summary>
        /// Returns the creation date and time of the specified file or directory.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in local time.</returns>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <remarks>
        /// <para>
        /// The <paramref name="path"/> parameter is permitted to specify relative or absolute path information. Relative path information is interpreted as relative to the current working directory. To obtain the current working directory, see <see cref="DirectoryBase.GetCurrentDirectory"/>.
        /// </para>
        /// <para>
        /// If the file described in the <paramref name="path"/> parameter does not exist, this method returns 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time.
        /// </para>
        /// <para>
        /// NTFS-formatted drives may cache file meta-info, such as file creation time, for a short period of time, which is known as "file tunneling." As a result, it may be necessary to explicitly set the creation time of a file if you are overwriting or replacing an existing file.
        /// </para>
        /// </remarks>
        public abstract DateTime GetCreationTime(string path);

        /// <inheritdoc cref="File.GetCreationTimeUtc"/>
        /// <summary>
        /// Returns the creation date and time, in coordinated universal time (UTC), of the specified file or directory.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the creation date and time for the specified file or directory. This value is expressed in UTC time.</returns>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <remarks>
        /// <para>
        /// The <paramref name="path"/> parameter is permitted to specify relative or absolute path information. Relative path information is interpreted as relative to the current working directory. To obtain the current working directory, see <see cref="DirectoryBase.GetCurrentDirectory"/>.
        /// </para>
        /// <para>
        /// If the file described in the <paramref name="path"/> parameter does not exist, this method returns 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC).
        /// </para>
        /// <para>
        /// NTFS-formatted drives may cache file meta-info, such as file creation time, for a short period of time, which is known as "file tunneling." As a result, it may be necessary to explicitly set the creation time of a file if you are overwriting or replacing an existing file.
        /// </para>
        /// </remarks>
        public abstract DateTime GetCreationTimeUtc(string path);

        /// <inheritdoc cref="File.GetLastAccessTime"/>
        /// <summary>
        /// Returns the date and time the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain access date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in local time.</returns>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <remarks>
        /// <para>
        /// The <paramref name="path"/> parameter is permitted to specify relative or absolute path information. Relative path information is interpreted as relative to the current working directory. To obtain the current working directory, see <see cref="DirectoryBase.GetCurrentDirectory"/>.
        /// </para>
        /// <para>
        /// If the file described in the <paramref name="path"/> parameter does not exist, this method returns 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time.
        /// </para>
        /// <para>
        /// NTFS-formatted drives may cache file meta-info, such as file creation time, for a short period of time, which is known as "file tunneling." As a result, it may be necessary to explicitly set the creation time of a file if you are overwriting or replacing an existing file.
        /// </para>
        /// </remarks>
        public abstract DateTime GetLastAccessTime(string path);

        /// <inheritdoc cref="File.GetLastAccessTimeUtc"/>
        /// <summary>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last accessed.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last accessed. This value is expressed in UTC time.</returns>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <remarks>
        /// <para>
        /// The <paramref name="path"/> parameter is permitted to specify relative or absolute path information. Relative path information is interpreted as relative to the current working directory. To obtain the current working directory, see <see cref="DirectoryBase.GetCurrentDirectory"/>.
        /// </para>
        /// <para>
        /// If the file described in the <paramref name="path"/> parameter does not exist, this method returns 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC).
        /// </para>
        /// <para>
        /// NTFS-formatted drives may cache file meta-info, such as file creation time, for a short period of time, which is known as "file tunneling." As a result, it may be necessary to explicitly set the creation time of a file if you are overwriting or replacing an existing file.
        /// </para>
        /// </remarks>
        public abstract DateTime GetLastAccessTimeUtc(string path);

        /// <inheritdoc cref="File.GetLastWriteTime"/>
        /// <summary>
        /// Returns the date and time the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <remarks>
        /// <para>
        /// If the file described in the path parameter does not exist, this method returns 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC), adjusted to local time.
        /// </para>
        /// <para>
        /// The <paramref name="path"/> parameter is permitted to specify relative or absolute path information. Relative path information is interpreted as relative to the current working directory. To obtain the current working directory, see <see cref="DirectoryBase.GetCurrentDirectory"/>.
        /// </para>
        /// <para>
        /// NTFS-formatted drives may cache file meta-info, such as file creation time, for a short period of time, which is known as "file tunneling." As a result, it may be necessary to explicitly set the creation time of a file if you are overwriting or replacing an existing file.
        /// </para>
        /// </remarks>
        public abstract DateTime GetLastWriteTime(string path);

        /// <inheritdoc cref="File.GetLastWriteTimeUtc"/>
        /// <summary>
        /// Returns the date and time, in coordinated universal time (UTC), that the specified file or directory was last written to.
        /// </summary>
        /// <param name="path">The file or directory for which to obtain creation date and time information. </param>
        /// <returns>A <see cref="DateTime"/> structure set to the date and time that the specified file or directory was last written to. This value is expressed in local time.</returns>
        /// <exception cref="UnauthorizedAccessException">The caller does not have the required permission.</exception>
        /// <exception cref="ArgumentException"><paramref name="path"/> is empty, contains only white spaces, or contains invalid characters.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="PathTooLongException">The specified path, file name, or both exceed the system-defined maximum length. For example, on Windows-based platforms, paths must be less than 248 characters, and file names must be less than 260 characters.</exception>
        /// <exception cref="NotSupportedException"><paramref name="path"/> is in an invalid format.</exception>
        /// <remarks>
        /// <para>
        /// If the file described in the path parameter does not exist, this method returns 12:00 midnight, January 1, 1601 A.D. (C.E.) Coordinated Universal Time (UTC).
        /// </para>
        /// <para>
        /// The <paramref name="path"/> parameter is permitted to specify relative or absolute path information. Relative path information is interpreted as relative to the current working directory. To obtain the current working directory, see <see cref="DirectoryBase.GetCurrentDirectory"/>.
        /// </para>
        /// <para>
        /// NTFS-formatted drives may cache file meta-info, such as file creation time, for a short period of time, which is known as "file tunneling." As a result, it may be necessary to explicitly set the creation time of a file if you are overwriting or replacing an existing file.
        /// </para>
        /// </remarks>
        public abstract DateTime GetLastWriteTimeUtc(string path);

        /// <inheritdoc cref="File.Move"/>
        public abstract void Move(string sourceFileName, string destFileName);

        /// <inheritdoc cref="File.Open(string,FileMode)"/>
        public abstract Stream Open(string path, FileMode mode);

        /// <inheritdoc cref="File.Open(string,FileMode,FileAccess)"/>
        public abstract Stream Open(string path, FileMode mode, FileAccess access);

        /// <inheritdoc cref="File.Open(string,FileMode,FileAccess,FileShare)"/>
        public abstract Stream Open(string path, FileMode mode, FileAccess access, FileShare share);

        /// <inheritdoc cref="File.OpenRead"/>
        public abstract Stream OpenRead(string path);

        /// <inheritdoc cref="File.OpenText"/>
        public abstract StreamReader OpenText(string path);

        /// <inheritdoc cref="File.OpenWrite"/>
        public abstract Stream OpenWrite(string path);

        /// <inheritdoc cref="File.ReadAllBytes"/>
        public abstract byte[] ReadAllBytes(string path);

#if NETCOREAPP2_0
        /// <inheritdoc cref="File.ReadAllBytesAsync"/>
        public abstract Task<byte[]> ReadAllBytesAsync(string path, CancellationToken cancellationToken);
#endif

        /// <inheritdoc cref="File.ReadAllLines(string)"/>
        public abstract string[] ReadAllLines(string path);

        /// <inheritdoc cref="File.ReadAllLines(string,Encoding)"/>
        public abstract string[] ReadAllLines(string path, Encoding encoding);

#if NETCOREAPP2_0
        /// <inheritdoc cref="File.ReadAllLinesAsync(string,CancellationToken)"/>
        public abstract Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.ReadAllLinesAsync(string,Encoding,CancellationToken)"/>
        public abstract Task<string[]> ReadAllLinesAsync(string path, Encoding encoding, CancellationToken cancellationToken);
#endif

        /// <inheritdoc cref="File.ReadAllText(string)"/>
        public abstract string ReadAllText(string path);

        /// <inheritdoc cref="File.ReadAllText(string,Encoding)"/>
        public abstract string ReadAllText(string path, Encoding encoding);

#if NETCOREAPP2_0
        ///<inheritdoc cref="File.ReadAllTextAsync(string,CancellationToken)"/>
        public abstract Task<string> ReadAllTextAsync(string path, CancellationToken cancellationToken);

        ///<inheritdoc cref="File.ReadAllTextAsync(string,Encoding,CancellationToken)"/>
        public abstract Task<string> ReadAllTextAsync(string path, Encoding encoding, CancellationToken cancellationToken);
#endif

        /// <inheritdoc cref="File.ReadLines(string)"/>
        public abstract IEnumerable<string> ReadLines(string path);

        /// <inheritdoc cref="File.ReadLines(string,Encoding)"/>
        public abstract IEnumerable<string> ReadLines(string path, Encoding encoding);

#if NET40
        /// <inheritdoc cref="File.Replace(string,string,string)"/>
        public abstract void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName);

        /// <inheritdoc cref="File.Replace(string,string,string,bool)"/>
        public abstract void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors);
#endif

        /// <inheritdoc cref="File.SetAccessControl(string,FileSecurity)"/>
        public abstract void SetAccessControl(string path, FileSecurity fileSecurity);

        /// <inheritdoc cref="File.SetAttributes"/>
        public abstract void SetAttributes(string path, FileAttributes fileAttributes);

        /// <inheritdoc cref="File.SetCreationTime"/>
        public abstract void SetCreationTime(string path, DateTime creationTime);

        /// <inheritdoc cref="File.SetCreationTimeUtc"/>
        public abstract void SetCreationTimeUtc(string path, DateTime creationTimeUtc);

        /// <inheritdoc cref="File.SetLastAccessTime"/>
        public abstract void SetLastAccessTime(string path, DateTime lastAccessTime);

        /// <inheritdoc cref="File.SetLastAccessTimeUtc"/>
        public abstract void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc);

        /// <inheritdoc cref="File.SetLastWriteTime"/>
        public abstract void SetLastWriteTime(string path, DateTime lastWriteTime);

        /// <inheritdoc cref="File.SetLastWriteTimeUtc"/>
        public abstract void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc);

        /// <inheritdoc cref="File.WriteAllBytes"/>
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

        /// <inheritdoc cref="File.WriteAllLines(string,IEnumerable{string})"/>
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

#if NETCOREAPP2_0
        /// <inheritdoc cref="File.WriteAllBytesAsync"/>
        public abstract Task WriteAllBytesAsync(string path, byte[] bytes, CancellationToken cancellationToken);
#endif

        public abstract void WriteAllLines(string path, IEnumerable<string> contents);

        /// <inheritdoc cref="File.WriteAllLines(string,IEnumerable{string},Encoding)"/>
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
        public abstract void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding);

        /// <inheritdoc cref="File.WriteAllLines(string,string[])"/>
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
        ///     The default behavior of the WriteAllLines method is to write out data using UTF-8 encoding without a byte order mark (BOM). If it is necessary to include a UTF-8 identifier, such as a byte order mark, at the beginning of a file, use the <see cref="WriteAllLines(string, string[], Encoding)"/> method overload with <see cref="UTF8Encoding"/> encoding.
        /// </para>
        /// <para>
        ///     Given a string array and a file path, this method opens the specified file, writes the string array to the file using the specified encoding,
        ///     and then closes the file.
        /// </para>
        /// </remarks>
        public abstract void WriteAllLines(string path, string[] contents);

        /// <inheritdoc cref="File.WriteAllLines(string,string[],Encoding)"/>
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
        public abstract void WriteAllLines(string path, string[] contents, Encoding encoding);

        /// <inheritdoc cref="File.WriteAllText(string,string)"/>
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

#if NETCOREAPP2_0
        /// <inheritdoc cref="File.WriteAllLinesAsync(string,IEnumerable{string},CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, IEnumerable<string> contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.WriteAllLinesAsync(string,IEnumerable{string},Encoding,CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, IEnumerable<string> contents, Encoding encoding, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.WriteAllLinesAsync(string,string[],CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, string[] contents, CancellationToken cancellationToken);

        /// <inheritdoc cref="File.WriteAllLinesAsync(string,string[],Encoding,CancellationToken)"/>
        public abstract Task WriteAllLinesAsync(string path, string[] contents, Encoding encoding, CancellationToken cancellationToken);
#endif
        public abstract void WriteAllText(string path, string contents);

        /// <inheritdoc cref="File.WriteAllText(string,string,Encoding)"/>
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
        
#if NETCOREAPP2_0
        /// <inheritdoc cref="File.WriteAllTextAsync(string,string,CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, string contents, CancellationToken cancellationToken);
        
        /// <inheritdoc cref="File.WriteAllTextAsync(string,string,Encoding,CancellationToken)"/>
        public abstract Task WriteAllTextAsync(string path, string contents, Encoding encoding, CancellationToken cancellationToken);
#endif
    }
}
