using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using XFS = MockUnixSupport;

    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void SplitLines_InputWithOneLine_ShouldReturnOnlyOneLine()
        {
            var input = "This is row one";
            var expected = new[] { "This is row one" };

            var result = input.SplitLines();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void SplitLines_InputWithTwoLinesSeparatedWithLf_ShouldReturnBothLines() {
            var input = "This is row one\nThis is row two";
            var expected = new[] { "This is row one", "This is row two" };

            var result = input.SplitLines();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void SplitLines_InputWithTwoLinesSeparatedWithCr_ShouldReturnBothLines() {
            var input = "This is row one\rThis is row two";
            var expected = new[] { "This is row one", "This is row two" };

            var result = input.SplitLines();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void SplitLines_InputWithTwoLinesSeparatedWithCrLf_ShouldReturnBothLines() {
            var input = "This is row one\r\nThis is row two";
            var expected = new[] { "This is row one", "This is row two" };

            var result = input.SplitLines();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void SplitLines_InputWithTwoLinesSeparatedWithAllLineEndings_ShouldReturnAllLines() {
            var input = "one\r\ntwo\rthree\nfour";
            var expected = new[] { "one", "two", "three", "four" };

            var result = input.SplitLines();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_DriveRoot_PreserveTrailingSlash()
        {
            Assert.AreEqual(@"c:\", @"c:\".TrimSlashes());
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_DriveRoot_AppendsTrailingSlash()
        {
            Assert.AreEqual(@"c:\", @"c:".TrimSlashes());
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_DriveRoot_TrimsExcessTrailingSlash()
        {
            Assert.AreEqual(@"c:\", @"c:\\".TrimSlashes());
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_DriveRoot_NormalizeAlternateSlash()
        {
            Assert.AreEqual(@"c:\", @"c:/".TrimSlashes());
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_RootedPath_TrimsAllTrailingSlashes()
        {
            Assert.AreEqual(@"c:\x", @"c:\x\".TrimSlashes());
        }

        [Test]
        public void TrimSlashes_RootedPath_DontAlterPathWithoutTrailingSlashes()
        {
            Assert.AreEqual(XFS.Path(@"c:\x"), XFS.Path(@"c:\x").TrimSlashes());
        }

        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public void TrimSlashes_SlashRoot_TrimsExcessTrailingSlash()
        {
            Assert.AreEqual("/", "//".TrimSlashes());
        }

        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public void TrimSlashes_SlashRoot_PreserveSlashRoot()
        {
            Assert.AreEqual("/", "/".TrimSlashes());
        }
    }
}
