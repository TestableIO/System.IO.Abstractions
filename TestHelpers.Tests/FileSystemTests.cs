#if NET40
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class FileSystemTests
    {
        [Test]
        public void Is_Serializable()
        {
            var fileSystem = new FileSystem();
            var memoryStream = new MemoryStream();

            var serializer = new Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            serializer.Serialize(memoryStream, fileSystem);

            Assert.That(memoryStream.Length > 0, "Length didn't increase after serialization task.");
        }
    }
}
#endif
