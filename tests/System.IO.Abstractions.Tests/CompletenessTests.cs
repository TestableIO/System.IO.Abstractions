
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Snapshooter.NUnit;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class ApiCompletenessTests
    {
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

        [Test]
        public void File() =>
            BaseTest(
                typeof(System.IO.File),
                typeof(System.IO.Abstractions.FileBase)
            );

        [Test]
        public void FileInfo() =>
            BaseTest(
                typeof(System.IO.FileInfo),
                typeof(System.IO.Abstractions.FileInfoBase)
            );

        [Test]
        public void Directory() => 
            BaseTest(
                typeof(System.IO.Directory),
                typeof(System.IO.Abstractions.DirectoryBase)
            );

        [Test]
        public void DirectoryInfo() =>
            BaseTest(
                typeof(System.IO.DirectoryInfo),
                typeof(System.IO.Abstractions.DirectoryInfoBase)
            );

        private void BaseTest(Type referenceType, Type abstractionType)
        {

            var expectedMembers = referenceType
                .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Select(x => x.ToString())
                .Select(x => x.Replace("System.IO.FileStream", "System.IO.Stream"))
                .Select(x => x.Replace("System.IO.FileSystemInfo", "System.IO.Abstractions.IFileSystemInfo"))
                .Select(x => x.Replace("System.IO.FileInfo", "System.IO.Abstractions.IFileInfo"))
                .Select(x => x.Replace("System.IO.DirectoryInfo", "System.IO.Abstractions.IDirectoryInfo"));
            var implementedMembers = abstractionType
                .GetMembers(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .Select(x => x.ToString())
                .Where(x => !x.Contains("op_Implicit"))
                .Where(x => x != "System.IO.Abstractions.IFileSystem get_FileSystem()")
                .Where(x => x != "System.IO.Abstractions.IFileSystem FileSystem")
                .Where(x => x != "");

            var diff = new ApiDiff(
                implementedMembers.Except(expectedMembers),
                expectedMembers.Except(implementedMembers)
            );
            Snapshot.Match(diff);
        }
    }
}
