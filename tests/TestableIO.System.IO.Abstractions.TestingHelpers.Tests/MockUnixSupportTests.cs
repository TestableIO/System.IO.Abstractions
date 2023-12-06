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
            Assert.That(XFS.Path(@"\test\"), Is.EqualTo("/test/"));
        }

        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public void Should_Remove_Drive_Letter_On_Unix()
        {
            Assert.That(XFS.Path(@"c:\test\"), Is.EqualTo("/test/"));
        }
    }
}
