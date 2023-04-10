using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.AccessControl;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockDirectoryInfoSymlinkTests
    {

#if FEATURE_CREATE_SYMBOLIC_LINK

        [Test]
        public void MockDirectoryInfo_ResolveLinkTarget_ShouldReturnPathOfTargetLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("bar");
            fileSystem.Directory.CreateSymbolicLink("foo", "bar");

            var result = fileSystem.DirectoryInfo.New("foo").ResolveLinkTarget(false);

            Assert.AreEqual("bar", result.Name);
        }

        [Test]
        public void MockDirectoryInfo_ResolveLinkTarget_WithFinalTarget_ShouldReturnPathOfTargetLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("bar");
            fileSystem.Directory.CreateSymbolicLink("foo", "bar");
            fileSystem.Directory.CreateSymbolicLink("foo1", "foo");

            var result = fileSystem.DirectoryInfo.New("foo1").ResolveLinkTarget(true);

            Assert.AreEqual("bar", result.Name);
        }

        [Test]
        public void MockDirectoryInfo_ResolveLinkTarget_WithoutFinalTarget_ShouldReturnFirstLink()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("bar");
            fileSystem.Directory.CreateSymbolicLink("foo", "bar");
            fileSystem.Directory.CreateSymbolicLink("foo1", "foo");

            var result = fileSystem.DirectoryInfo.New("foo1").ResolveLinkTarget(false);

            Assert.AreEqual("foo", result.Name);
        }

        [Test]
        public void MockDirectoryInfo_ResolveLinkTarget_WithoutTargetLink_ShouldThrowIOException()
        {
            var fileSystem = new MockFileSystem();
            fileSystem.Directory.CreateDirectory("bar");
            fileSystem.Directory.CreateSymbolicLink("foo", "bar");

            Assert.Throws<IOException>(() =>
            {
                fileSystem.DirectoryInfo.New("bar").ResolveLinkTarget(false);
            });
        }
#endif
    }
}