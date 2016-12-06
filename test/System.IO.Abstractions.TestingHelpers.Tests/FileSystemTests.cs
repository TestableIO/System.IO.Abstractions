using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class FileSystemTests
    {
#if NET45
        [Fact]
        public void Is_Serializable()
        {
            var fileSystem = new FileSystem();
            var memoryStream = new MemoryStream();

            var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            serializer.Serialize(memoryStream, fileSystem);

            Assert.True(memoryStream.Length > 0, "Length didn't increase after serialization task.");
        }
#endif
    }
}
