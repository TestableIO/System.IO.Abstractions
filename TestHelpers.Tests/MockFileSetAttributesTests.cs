using System.Collections.Generic;
using NUnit.Framework;
using XFS = System.IO.Abstractions.TestingHelpers.MockUnixSupport;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileSetAttributesTests
    {
        [Test]
        public void MockFile_SetAttributes_ShouldSetAttributesOnFile()
        {
            var path = XFS.Path(@"c:\something\demo.txt");
            var filedata = new MockFileData("test");
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {path, filedata},
            });

            fileSystem.File.SetAttributes(path, FileAttributes.Hidden);

            var attributes = fileSystem.File.GetAttributes(path);
            Assert.That(attributes, Is.EqualTo(FileAttributes.Hidden));
        }

        [Test]
        public void MockFile_SetAttributes_ShouldSetAttributesOnDirectory()
        {
            var fileSystem = new MockFileSystem();
            var path = XFS.Path(@"c:\something");
            fileSystem.AddDirectory(path);
            var directoryInfo = fileSystem.DirectoryInfo.FromDirectoryName(path);
            directoryInfo.Attributes = FileAttributes.Directory | FileAttributes.Normal;

            fileSystem.File.SetAttributes(path, FileAttributes.Directory | FileAttributes.Hidden);

            var attributes = fileSystem.File.GetAttributes(path);
            Assert.That(attributes, Is.EqualTo(FileAttributes.Directory | FileAttributes.Hidden));
        }

        [Test]
        [TestCase("", FileAttributes.Normal)]
        [TestCase("   ", FileAttributes.Normal)]
        public void MockFile_SetAttributes_ShouldThrowArgumentExceptionIfPathContainsOnlyWhitespaces(string path, FileAttributes attributes)
        {
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => fileSystem.File.SetAttributes(path, attributes);

            Assert.Throws<ArgumentException>(action);
        }

        [Test]
        public void MockFile_SetAttributes_ShouldThrowFileNotFoundExceptionIfMissingDirectory()
        {
            var path = XFS.Path(@"C:\something");
            var attributes = FileAttributes.Normal;
            var fileSystem = new MockFileSystem();

            TestDelegate action = () => fileSystem.File.SetAttributes(path, attributes);

            var exception = Assert.Throws<FileNotFoundException>(action);
            Assert.That(exception.FileName, Is.EqualTo(path));
        }
    }
}
