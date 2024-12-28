﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Snapshooter;
using Snapshooter.NUnit;
using static System.Reflection.BindingFlags;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class ApiParityTests
    {
        [Test]
        public void File() =>
            AssertParity(
                typeof(System.IO.File),
                typeof(System.IO.Abstractions.FileBase)
            );

        [Test]
        public void FileInfo() =>
            AssertParity(
                typeof(System.IO.FileInfo),
                typeof(System.IO.Abstractions.FileInfoBase)
            );

        [Test]
        public void FileVersionInfo() =>
            AssertParity(
                typeof(System.Diagnostics.FileVersionInfo),
                typeof(System.IO.Abstractions.FileVersionInfoBase)
            );

        [Test]
        public void Directory() =>
            AssertParity(
                typeof(System.IO.Directory),
                typeof(System.IO.Abstractions.DirectoryBase)
            );

        [Test]
        public void DirectoryInfo() =>
            AssertParity(
                typeof(System.IO.DirectoryInfo),
                typeof(System.IO.Abstractions.DirectoryInfoBase)
            );

        [Test]
        public void DriveInfo() =>
            AssertParity(
                typeof(System.IO.DriveInfo),
                typeof(System.IO.Abstractions.DriveInfoBase)
            );

        [Test]
        public void Path() =>
            AssertParity(
                typeof(System.IO.Path),
                typeof(System.IO.Abstractions.PathBase)
            );

        [Test]
        public void FileSystemWatcher() =>
            AssertParity(
                typeof(System.IO.FileSystemWatcher),
                typeof(System.IO.Abstractions.FileSystemWatcherBase)
            );

        private void AssertParity(Type referenceType, Type abstractionType)
        {
            static IEnumerable<string> GetMembers(Type type) => type
                .GetMembers(bindingAttr: Instance | Static | Public | FlattenHierarchy)
                .Select(x => x.ToString())
                .OrderBy(x => x, StringComparer.Ordinal);
            var referenceMembers = GetMembers(referenceType)
                .Select(x => x.Replace("System.IO.FileStream", "System.IO.Abstractions.FileSystemStream"))
                .Select(x => x.Replace("System.IO.Abstractions.FileSystemStreamOptions", "System.IO.FileStreamOptions"))
                .Select(x => x.Replace("System.IO.FileSystemInfo", "System.IO.Abstractions.IFileSystemInfo"))
                .Select(x => x.Replace("System.IO.FileInfo", "System.IO.Abstractions.IFileInfo"))
                .Select(x => x.Replace("System.IO.DirectoryInfo", "System.IO.Abstractions.IDirectoryInfo"))
                .Select(x => x.Replace("System.IO.DriveInfo", "System.IO.Abstractions.IDriveInfo"))
                .Select(x => x.Replace("System.IO.WaitForChangedResult", "System.IO.Abstractions.IWaitForChangedResult"))
                .Where(x => x != "System.Diagnostics.FileVersionInfo GetVersionInfo(System.String)");
            var abstractionMembers = GetMembers(abstractionType)
                .Where(x => !x.Contains("op_Implicit"))
                .Where(x => x != "System.IO.Abstractions.IFileSystem get_FileSystem()")
                .Where(x => x != "System.IO.Abstractions.IFileSystem FileSystem");
            var diff = new ApiDiff(
                extraMembers: abstractionMembers.Except(referenceMembers),
                missingMembers: referenceMembers.Except(abstractionMembers)
            );
            Snapshot.Match(diff, SnapshotNameExtension.Create(snapshotSuffix));
        }

        private readonly struct ApiDiff
        {
            public ApiDiff(IEnumerable<string> extraMembers, IEnumerable<string> missingMembers)
            {
                ExtraMembers = extraMembers.ToArray();
                MissingMembers = missingMembers.ToArray();

            }

            public string[] ExtraMembers { get; }
            public string[] MissingMembers { get; }
        }

#if NET462
        private const string snapshotSuffix = ".NET Framework 4.6.2";
#elif NET6_0
        private const string snapshotSuffix = ".NET 6.0";
#elif NET7_0
        private const string snapshotSuffix = ".NET 7.0";
#elif NET8_0
        private const string snapshotSuffix = ".NET 8.0";
#else
#error Unknown target framework.
#endif
    }
}
