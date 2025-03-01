using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class MockUnixSupportTests
    {
        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public async Task Should_Convert_Backslashes_To_Slashes_On_Unix()
        {
            await That(XFS.Path(@"\test\")).IsEqualTo("/test/");
        }

        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public async Task Should_Remove_Drive_Letter_On_Unix()
        {
            await That(XFS.Path(@"c:\test\")).IsEqualTo("/test/");
        }
    }
}
