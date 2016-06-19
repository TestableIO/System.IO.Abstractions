using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockUnixSupportTests
    {
        [Test]
        public void Should_Convert_Backslashes_To_Slashes_On_Unix()
        {
            Assert.AreEqual("/test/", MockUnixSupport.Path(@"\test\", () => true));
        }

        [Test]
        public void Should_Remove_Drive_Letter_On_Unix()
        {
            Assert.AreEqual("/test/", MockUnixSupport.Path(@"c:\test\", () => true));
        }
    }
}
