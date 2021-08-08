using NUnit.Framework;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class DirectoryInfoExtensionsTests
    {
        [Test]
        public void SubDirectory_Extension_Test()
        {
            //arrange
            var fs = new FileSystem();
            var current =  fs.DirectoryInfo.FromDirectoryName(fs.Directory.GetCurrentDirectory());
            var guid = Guid.NewGuid().ToString();
            var expectedPath = fs.Path.Combine(current.FullName, guid);

            //make sure directory doesn't exists
            Assert.IsFalse(fs.Directory.Exists(expectedPath));

            //create directory
            var created = current.SubDirectory(guid);
            created.Create();

            //assert it exists
            Assert.IsTrue(fs.Directory.Exists(expectedPath));
            Assert.AreEqual(expectedPath, created.FullName);

            //delete directory
            created.Delete();
            Assert.IsFalse(fs.Directory.Exists(expectedPath));
        }

        [Test]
        public void File_Extension_Test()
        {
            //arrange
            var fs = new FileSystem();
            var current = fs.DirectoryInfo.FromDirectoryName(fs.Directory.GetCurrentDirectory());
            var guid = Guid.NewGuid().ToString();
            var expectedPath = fs.Path.Combine(current.FullName, guid);

            //make sure file doesn't exists
            Assert.IsFalse(fs.File.Exists(expectedPath));

            //create file
            var created = current.File(guid);
            using (var stream = created.Create())
            {
                stream.Dispose();
            }

            //assert it exists
            Assert.IsTrue(fs.File.Exists(expectedPath));
            Assert.AreEqual(expectedPath, created.FullName);

            //delete file
            created.Delete();
            Assert.IsFalse(fs.File.Exists(expectedPath));
        }
    }
}
