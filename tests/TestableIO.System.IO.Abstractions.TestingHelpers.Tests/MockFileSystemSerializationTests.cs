#if !NET9_0_OR_GREATER
namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using NUnit.Framework;
    using Text;
    using XFS = MockUnixSupport;
    [TestFixture]
    class MockFileSystemSerializationTests
    {
        [Test]
        public async Task SerializationBytes()
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
#pragma warning disable SYSLIB0011
            var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();

            serializer.Serialize(memoryStream, fileSystem);
            memoryStream.Flush();
            memoryStream.Position = 0;
            fileSystem = (MockFileSystem)serializer.Deserialize(memoryStream);
#pragma warning restore SYSLIB0011
            memoryStream.Dispose();

            // Assert
            await That(fileSystem.GetFile(path).Contents).IsEqualTo(expected);
            await That(fileSystem.File.ReadAllBytes(path)).IsEqualTo(expected);
        }
    }
}
#endif
