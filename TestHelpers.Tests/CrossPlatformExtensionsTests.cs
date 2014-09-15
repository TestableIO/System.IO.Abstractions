using System.Collections.Generic;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class CrossPlatformExtensionsTests
    {
        [Test]
        public void Should_Convert_Backslashes_To_Slashes_On_Unix()
        {
            Assert.AreEqual("/test/", XFS.Path(@"\test\", () => true));
        }

        [Test]
        public void Should_Remove_Drive_Letter_On_Unix()
        {
            Assert.AreEqual("/test/", XFS.Path(@"c:\test\", () => true));
        }
    }
}
