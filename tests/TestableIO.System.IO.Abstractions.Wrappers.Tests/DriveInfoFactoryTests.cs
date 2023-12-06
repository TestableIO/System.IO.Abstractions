using NUnit.Framework;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class DriveInfoFactoryTests
    {
        [Test]
        public void Wrap_WithNull_ShouldReturnNull()
        {
            var fileSystem = new FileSystem();

            var result = fileSystem.DriveInfo.Wrap(null);

            Assert.That(result, Is.Null);
        }
    }
}
