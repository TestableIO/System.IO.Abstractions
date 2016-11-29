using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockUnixSupportTests
    {
        [Fact]
        public void Should_Convert_Backslashes_To_Slashes_On_Unix()
        {
            Assert.Equal("/test/", MockUnixSupport.Path(@"\test\", () => true));
        }

        [Fact]
        public void Should_Remove_Drive_Letter_On_Unix()
        {
            Assert.Equal("/test/", MockUnixSupport.Path(@"c:\test\", () => true));
        }
    }
}
