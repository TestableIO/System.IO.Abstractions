using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Snapshooter;
using Snapshooter.NUnit;
using static System.Reflection.BindingFlags;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class ApiCompletenessTests
    {
        [Test]
        public void File() =>
            AssertTypeParity(
                typeof(System.IO.File),
                typeof(System.IO.Abstractions.FileBase)
            );

        [Test]
        public void FileInfo() =>
            AssertTypeParity(
                typeof(System.IO.FileInfo),
                typeof(System.IO.Abstractions.FileInfoBase)
            );

        [Test]
        public void Directory() =>
            AssertTypeParity(
                typeof(System.IO.Directory),
                typeof(System.IO.Abstractions.DirectoryBase)
            );

        [Test]
        public void DirectoryInfo() =>
            AssertTypeParity(
                typeof(System.IO.DirectoryInfo),
                typeof(System.IO.Abstractions.DirectoryInfoBase)
            );

        [Test]
        public void DriveInfo() =>
            AssertTypeParity(
                typeof(System.IO.DriveInfo),
                typeof(System.IO.Abstractions.DriveInfoBase)
            );

        [Test]
        public void Path() =>
            AssertTypeParity(
                typeof(System.IO.Path),
                typeof(System.IO.Abstractions.PathBase)
            );

        private void AssertTypeParity(Type referenceType, Type abstractionType)
        {
            const BindingFlags bindingFlags = Instance | Static | Public | FlattenHierarchy;
            var expectedMembers = referenceType
                .GetMembers(bindingFlags)
                .Select(x => x.ToString())
                .OrderBy(x => x)
                .Select(x => x.Replace("System.IO.FileStream", "System.IO.Stream"))
                .Select(x => x.Replace("System.IO.FileSystemInfo", "System.IO.Abstractions.IFileSystemInfo"))
                .Select(x => x.Replace("System.IO.FileInfo", "System.IO.Abstractions.IFileInfo"))
                .Select(x => x.Replace("System.IO.DirectoryInfo", "System.IO.Abstractions.IDirectoryInfo"))
                .Select(x => x.Replace("System.IO.DriveInfo", "System.IO.Abstractions.IDriveInfo"));
            var implementedMembers = abstractionType
                .GetMembers(bindingFlags)
                .OrderBy(x => x).Select(x => x.ToString())
                .Where(x => !x.Contains("op_Implicit"))
                .Where(x => x != "System.IO.Abstractions.IFileSystem get_FileSystem()")
                .Where(x => x != "System.IO.Abstractions.IFileSystem FileSystem")
                .Where(x => x != "");

            var diff = new ApiDiff(
                implementedMembers.Except(expectedMembers),
                expectedMembers.Except(implementedMembers)
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

#if NETCOREAPP3_1
        private const string snapshotSuffix = ".NET Core 3.1";
#elif NETCOREAPP2_1
            private const string snapshotSuffix = ".NET Core 2.1";
#else
#error Unknown target framework.
#endif
    }
}
