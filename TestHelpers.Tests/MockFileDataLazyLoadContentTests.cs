using System.Linq;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileDataLazyLoadContentTests
    {
        [Test]
        public void MockFileData_AcceptsFuncAndReturnsContents()
        {
            var contents = Enumerable.Range(0, 100).Select(_ => (byte)_).ToArray();
            var x = new MockFileData(() => contents);
            Assert.AreEqual(x.Contents, contents);
        }

        [Test]
        public void MockFileData_LazyLoadFuncIsCalledOnDemand()
        {
            var contents = Enumerable.Range(19, 42).Select(_ => (byte)_).ToArray();
            var lazyLoadFuncHasBeenCalled = false;
            var x = new MockFileData(() =>
            {
                lazyLoadFuncHasBeenCalled = true;
                return contents;
            });
            Assert.IsFalse(lazyLoadFuncHasBeenCalled);
            Assert.AreEqual(x.Contents, contents);
            Assert.IsTrue(lazyLoadFuncHasBeenCalled);
        }

        [Test]
        public void MockFileData_FuncConstructorThrowsOnNullArgument()
        {
            Assert.Throws<ArgumentNullException>(() => new MockFileData((Func<byte[]>)null));
        }

    }

    [TestFixture]
    public class MockFileDataInsertFileInfoObjectTests
    {
        private IFileSystem destinationFileSystem;
        private FileInfoBase destinationFileInfo;

        private const string sourceFileName = @"a:\c.bin";
        private const string destinationFileName = @"b:\c\d.bin";
        private readonly byte[] fileContent = Enumerable.Range(3, 14).Select(_ => (byte)_).ToArray();
        private const FileAttributes attributes = FileAttributes.Hidden;
        private readonly DateTime dateCreated = DateTime.Now.AddHours(-3);
        private readonly DateTime dateLastWritten = DateTime.Now.AddHours(-2);
        private readonly DateTime dateLastAccessed = DateTime.Now.AddHours(-1);

        [TestFixtureSetUp]
        public void MockFileData_AcceptsFuncAndReturnsContents()
        {
            // create a file in a source file system
            var sourceFileSystem = new MockFileSystem();
            sourceFileSystem.File.WriteAllBytes(sourceFileName, fileContent);
            sourceFileSystem.File.SetAttributes(sourceFileName, attributes);
            sourceFileSystem.File.SetCreationTime(sourceFileName, dateCreated);
            sourceFileSystem.File.SetLastWriteTime(sourceFileName, dateLastWritten);
            sourceFileSystem.File.SetLastAccessTime(sourceFileName, dateLastAccessed);
            var sourceFile = sourceFileSystem.DirectoryInfo.FromDirectoryName(Path.GetDirectoryName(sourceFileName)).GetFiles().Single();

            // create a new destination file system and copy the source file tehre
            var dfs = new MockFileSystem();
            dfs.AddFile(destinationFileName, new MockFileData(sourceFileSystem, sourceFile));
            destinationFileInfo = dfs.DirectoryInfo.FromDirectoryName(Path.GetDirectoryName(destinationFileName)).GetFiles().Single();
            destinationFileSystem = dfs;
        }

        [Test]
        public void TestThatFileHasRightName()
        {
            Assert.AreEqual(destinationFileName, destinationFileInfo.FullName);
        }

        [Test]
        public void TestThatFileHasRightContent()
        {
            Assert.AreEqual(fileContent, destinationFileSystem.File.ReadAllBytes(destinationFileInfo.FullName));
        }

        [Test]
        public void TestThatFileHasRightAttributes()
        {
            Assert.AreEqual(attributes, destinationFileInfo.Attributes);
        }

        [Test]
        public void TestThatFileHasRightCreationTime()
        {
            Assert.AreEqual(dateCreated, destinationFileInfo.CreationTime);
        }

        [Test]
        public void TestThatFileHasRightWriteTime()
        {
            Assert.AreEqual(dateLastWritten, destinationFileInfo.LastWriteTime);
        }

        [Test]
        public void TestThatFileHasRightAccessTime()
        {
            Assert.AreEqual(dateLastAccessed, destinationFileInfo.LastAccessTime);
        }

    }

}
