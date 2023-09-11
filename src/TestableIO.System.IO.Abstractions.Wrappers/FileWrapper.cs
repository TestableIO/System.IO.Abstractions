﻿using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace System.IO.Abstractions
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public partial class FileWrapper : FileBase
    {
        /// <inheritdoc />
        public FileWrapper(IFileSystem fileSystem) : base(fileSystem)
        {
        }

        /// <inheritdoc />
        public override void AppendAllLines(string path, IEnumerable<string> contents)
        {
            File.AppendAllLines(path, contents);
        }

        /// <inheritdoc />
        public override void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding)
        {
            File.AppendAllLines(path, contents, encoding);
        }


        /// <inheritdoc />
        public override void AppendAllText(string path, string contents)
        {
            File.AppendAllText(path, contents);
        }

        /// <inheritdoc />
        public override void AppendAllText(string path, string contents, Encoding encoding)
        {
            File.AppendAllText(path, contents, encoding);
        }

        /// <inheritdoc />
        public override StreamWriter AppendText(string path)
        {
            return File.AppendText(path);
        }

        /// <inheritdoc />
        public override void Copy(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
        }

        /// <inheritdoc />
        public override void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            File.Copy(sourceFileName, destFileName, overwrite);
        }

        /// <inheritdoc />
        public override FileSystemStream Create(string path)
        {
            return new FileStreamWrapper(File.Create(path));
        }

        /// <inheritdoc />
        public override FileSystemStream Create(string path, int bufferSize)
        {
            return new FileStreamWrapper(File.Create(path, bufferSize));
        }

        /// <inheritdoc />
        public override FileSystemStream Create(string path, int bufferSize, FileOptions options)
        {
            return new FileStreamWrapper(File.Create(path, bufferSize, options));
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override IFileSystemInfo CreateSymbolicLink(string path, string pathToTarget)
        {
            return File.CreateSymbolicLink(path, pathToTarget)
                .WrapFileSystemInfo(FileSystem);
        }
#endif
        /// <inheritdoc />
        public override StreamWriter CreateText(string path)
        {
            return File.CreateText(path);
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override void Decrypt(string path)
        {
            File.Decrypt(path);
        }

        /// <inheritdoc />
        public override void Delete(string path)
        {
            File.Delete(path);
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public override void Encrypt(string path)
        {
            File.Encrypt(path);
        }

        /// <inheritdoc />
        public override bool Exists(string path)
        {
            return File.Exists(path);
        }

        /// <inheritdoc />
        public override FileAttributes GetAttributes(string path)
        {
            return File.GetAttributes(path);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override FileAttributes GetAttributes(SafeFileHandle fileHandle)
        {
            return File.GetAttributes(fileHandle);
        }
#endif

        /// <inheritdoc />
        public override DateTime GetCreationTime(string path)
        {
            return File.GetCreationTime(path);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override DateTime GetCreationTime(SafeFileHandle fileHandle)
        {
            return File.GetCreationTime(fileHandle);
        }
#endif

        /// <inheritdoc />
        public override DateTime GetCreationTimeUtc(string path)
        {
            return File.GetCreationTimeUtc(path);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override DateTime GetCreationTimeUtc(SafeFileHandle fileHandle)
        {
            return File.GetCreationTimeUtc(fileHandle);
        }
#endif

        /// <inheritdoc />
        public override DateTime GetLastAccessTime(string path)
        {
            return File.GetLastAccessTime(path);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override DateTime GetLastAccessTime(SafeFileHandle fileHandle)
        {
            return File.GetLastAccessTime(fileHandle);
        }
#endif

        /// <inheritdoc />
        public override DateTime GetLastAccessTimeUtc(string path)
        {
            return File.GetLastAccessTimeUtc(path);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override DateTime GetLastAccessTimeUtc(SafeFileHandle fileHandle)
        {
            return File.GetLastAccessTimeUtc(fileHandle);
        }
#endif

        /// <inheritdoc />
        public override DateTime GetLastWriteTime(string path)
        {
            return File.GetLastWriteTime(path);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override DateTime GetLastWriteTime(SafeFileHandle fileHandle)
        {
            return File.GetLastWriteTime(fileHandle);
        }
#endif

        /// <inheritdoc />
        public override DateTime GetLastWriteTimeUtc(string path)
        {
            return File.GetLastWriteTimeUtc(path);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override DateTime GetLastWriteTimeUtc(SafeFileHandle fileHandle)
        {
            return File.GetLastWriteTimeUtc(fileHandle);
        }
#endif

#if FEATURE_UNIX_FILE_MODE
        /// <inheritdoc />
        [UnsupportedOSPlatform("windows")]
        public override UnixFileMode GetUnixFileMode(string path)
        {
            return File.GetUnixFileMode(path);
        }
#endif

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        [UnsupportedOSPlatform("windows")]
        public override UnixFileMode GetUnixFileMode(SafeFileHandle fileHandle)
        {
            return File.GetUnixFileMode(fileHandle);
        }
#endif

        /// <inheritdoc />
        public override void Move(string sourceFileName, string destFileName)
        {
            File.Move(sourceFileName, destFileName);
        }

#if FEATURE_FILE_MOVE_WITH_OVERWRITE
        /// <inheritdoc />
        public override void Move(string sourceFileName, string destFileName, bool overwrite)
        {
            File.Move(sourceFileName, destFileName, overwrite);
        }
#endif


        /// <inheritdoc />
        public override FileSystemStream Open(string path, FileMode mode)
        {
            return new FileStreamWrapper(File.Open(path, mode));
        }

        /// <inheritdoc />
        public override FileSystemStream Open(string path, FileMode mode, FileAccess access)
        {
            return new FileStreamWrapper(File.Open(path, mode, access));
        }

        /// <inheritdoc />
        public override FileSystemStream Open(string path, FileMode mode, FileAccess access, FileShare share)
        {
            return new FileStreamWrapper(File.Open(path, mode, access, share));
        }

#if FEATURE_FILESTREAM_OPTIONS
        /// <inheritdoc />
        public override FileSystemStream Open(string path, FileStreamOptions options)
        {
            return new FileStreamWrapper(File.Open(path, options));
        }
#endif

        /// <inheritdoc />
        public override FileSystemStream OpenRead(string path)
        {
            return new FileStreamWrapper(File.OpenRead(path));
        }

        /// <inheritdoc />
        public override StreamReader OpenText(string path)
        {
            return File.OpenText(path);
        }

        /// <inheritdoc />
        public override FileSystemStream OpenWrite(string path)
        {
            return new FileStreamWrapper(File.OpenWrite(path));
        }

        /// <inheritdoc />
        public override byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <inheritdoc />
        public override string[] ReadAllLines(string path)
        {
            return File.ReadAllLines(path);
        }

        /// <inheritdoc />
        public override string[] ReadAllLines(string path, Encoding encoding)
        {
            return File.ReadAllLines(path, encoding);
        }

        /// <inheritdoc />
        public override string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        /// <inheritdoc />
        public override string ReadAllText(string path, Encoding encoding)
        {
            return File.ReadAllText(path, encoding);
        }

        /// <inheritdoc />
        public override IEnumerable<string> ReadLines(string path)
        {
            return File.ReadLines(path);
        }

        /// <inheritdoc />
        public override IEnumerable<string> ReadLines(string path, Encoding encoding)
        {
            return File.ReadLines(path, encoding);
        }

        /// <inheritdoc />
        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName)
        {
            File.Replace(sourceFileName, destinationFileName, destinationBackupFileName);
        }

        /// <inheritdoc />
        public override void Replace(string sourceFileName, string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
        {
            File.Replace(sourceFileName, destinationFileName, destinationBackupFileName, ignoreMetadataErrors);
        }

#if FEATURE_CREATE_SYMBOLIC_LINK
        /// <inheritdoc />
        public override IFileSystemInfo ResolveLinkTarget(string linkPath, bool returnFinalTarget)
        {
            return File.ResolveLinkTarget(linkPath, returnFinalTarget)
                .WrapFileSystemInfo(FileSystem);
        }
#endif

        /// <inheritdoc />
        public override void SetAttributes(string path, FileAttributes fileAttributes)
        {
            File.SetAttributes(path, fileAttributes);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override void SetAttributes(SafeFileHandle fileHandle, FileAttributes fileAttributes)
        {
            File.SetAttributes(fileHandle, fileAttributes);
        }
#endif

        /// <inheritdoc />
        public override void SetCreationTime(string path, DateTime creationTime)
        {
            File.SetCreationTime(path, creationTime);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override void SetCreationTime(SafeFileHandle fileHandle, DateTime creationTime)
        {
            File.SetCreationTime(fileHandle, creationTime);
        }
#endif

        /// <inheritdoc />
        public override void SetCreationTimeUtc(string path, DateTime creationTimeUtc)
        {
            File.SetCreationTimeUtc(path, creationTimeUtc);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override void SetCreationTimeUtc(SafeFileHandle fileHandle, DateTime creationTimeUtc)
        {
            File.SetCreationTimeUtc(fileHandle, creationTimeUtc);
        }
#endif

        /// <inheritdoc />
        public override void SetLastAccessTime(string path, DateTime lastAccessTime)
        {
            File.SetLastAccessTime(path, lastAccessTime);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override void SetLastAccessTime(SafeFileHandle fileHandle, DateTime lastAccessTime)
        {
            File.SetLastAccessTime(fileHandle, lastAccessTime);
        }
#endif

        /// <inheritdoc />
        public override void SetLastAccessTimeUtc(string path, DateTime lastAccessTimeUtc)
        {
            File.SetLastAccessTimeUtc(path, lastAccessTimeUtc);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override void SetLastAccessTimeUtc(SafeFileHandle fileHandle, DateTime lastAccessTimeUtc)
        {
            File.SetLastAccessTimeUtc(fileHandle, lastAccessTimeUtc);
        }
#endif

        /// <inheritdoc />
        public override void SetLastWriteTime(string path, DateTime lastWriteTime)
        {
            File.SetLastWriteTime(path, lastWriteTime);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override void SetLastWriteTime(SafeFileHandle fileHandle, DateTime lastWriteTime)
        {
            File.SetLastWriteTime(fileHandle, lastWriteTime);
        }
#endif

        /// <inheritdoc />
        public override void SetLastWriteTimeUtc(string path, DateTime lastWriteTimeUtc)
        {
            File.SetLastWriteTimeUtc(path, lastWriteTimeUtc);
        }

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        public override void SetLastWriteTimeUtc(SafeFileHandle fileHandle, DateTime lastWriteTimeUtc)
        {
            File.SetLastWriteTimeUtc(fileHandle, lastWriteTimeUtc);
        }
#endif

#if FEATURE_UNIX_FILE_MODE
        /// <inheritdoc />
        [UnsupportedOSPlatform("windows")]
        public override void SetUnixFileMode(string path, UnixFileMode mode)
        {
            File.SetUnixFileMode(path, mode);
        }
#endif

#if FEATURE_FILE_ATTRIBUTES_VIA_HANDLE
        /// <inheritdoc />
        [UnsupportedOSPlatform("windows")]
        public override void SetUnixFileMode(SafeFileHandle fileHandle, UnixFileMode mode)
        {
            File.SetUnixFileMode(fileHandle, mode);
        }
#endif

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
            File.WriteAllBytes(path, bytes);
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
            File.WriteAllLines(path, contents);
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
            File.WriteAllLines(path, contents, encoding);
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
            File.WriteAllLines(path, contents);
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
            File.WriteAllLines(path, contents, encoding);
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
            File.WriteAllText(path, contents);
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
            File.WriteAllText(path, contents, encoding);
        }
    }
}
