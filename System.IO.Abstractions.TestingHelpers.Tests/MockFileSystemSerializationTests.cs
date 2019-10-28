namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using NUnit.Framework;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Text;
    using XFS = MockUnixSupport;
    [TestFixture]
    class MockFileSystemSerializationTests
    {

        const string FS_FILENAME = @"test.fs";

        [Test]
        public void SerializationTexts()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");

            var content = "Hello there!" + Environment.NewLine + "Second line!" + Environment.NewLine;

            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            fileSystem.File.WriteAllText(path, content);

            //Act
            SaveFileSystem(fileSystem);
            fileSystem = (MockFileSystem) LoadFileSystem();

            //Clear
            ClearFileSystem();

            // Assert
            Assert.AreEqual(
                content,
                fileSystem.GetFile(path).Contents);
            Assert.AreEqual(
                content,
                fileSystem.File.ReadAllText(path));

        }
        [Test]
        public void SerializationBytes()
        {
            // Arrange
            string path = XFS.Path(@"c:\something\demo.txt");

            var content = "Hello there!" + Environment.NewLine + "Second line!" + Environment.NewLine;
            var expected = Encoding.ASCII.GetBytes(content); //Convert a C# string to a byte array

            var fileSystem = new MockFileSystem();
            fileSystem.AddDirectory(XFS.Path(@"c:\something"));

            fileSystem.File.WriteAllBytes(path, expected);

            //Act
            SaveFileSystem(fileSystem);
            fileSystem = (MockFileSystem)LoadFileSystem();

            //Clear
            ClearFileSystem();

            // Assert
            Assert.AreEqual(
                expected,
                fileSystem.GetFile(path).Contents);
            Assert.AreEqual(
                content,
                fileSystem.File.ReadAllBytes(path));
        }
        private void SaveFileSystem(MockFileSystem fileSystem)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(Path.GetTempPath() + FS_FILENAME, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, fileSystem);
            stream.Close();
        }
        private IFileSystem LoadFileSystem()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(Path.GetTempPath() + FS_FILENAME, FileMode.Open, FileAccess.Read);
            IFileSystem fileSystem = (MockFileSystem)formatter.Deserialize(stream);
            stream.Close();
            return fileSystem;
        }
        private void ClearFileSystem()
        {
            string temp = Path.GetTempPath() + FS_FILENAME;
            if (File.Exists(temp))
                File.Delete(temp);
        }
    }
}
