namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using NUnit.Framework;
    using Text;
    using XFS = MockUnixSupport;
    [TestFixture]
    class MockFileSystemSerializationTests
    {
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
            var memoryStream = new MemoryStream();
            var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            
#pragma warning disable SYSLIB0011
            serializer.Serialize(memoryStream, fileSystem);
            memoryStream.Flush();
            memoryStream.Position = 0; 
            fileSystem = (MockFileSystem)serializer.Deserialize(memoryStream);
#pragma warning restore SYSLIB0011
            memoryStream.Dispose();

            // Assert
            Assert.AreEqual(
                expected,
                fileSystem.GetFile(path).Contents);
            Assert.AreEqual(
                content,
                fileSystem.File.ReadAllBytes(path));
        }
    }
}