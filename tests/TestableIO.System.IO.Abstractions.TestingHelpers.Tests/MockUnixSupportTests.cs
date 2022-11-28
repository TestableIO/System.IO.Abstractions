using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockUnixSupportTests
    {
        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public void Should_Convert_Backslashes_To_Slashes_On_Unix()
        {
            Assert.AreEqual("/test/", XFS.Path(@"\test\"));
        }

        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public void Should_Remove_Drive_Letter_On_Unix()
        {
            Assert.AreEqual("/test/", XFS.Path(@"c:\test\"));
        }
    }
}
