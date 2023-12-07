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
        public void SplitLines_InputWithTwoLinesSeparatedWithLf_ShouldReturnBothLines()
        {
            var input = "This is row one\nThis is row two";
            var expected = new[] { "This is row one", "This is row two" };

            var result = input.SplitLines();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void SplitLines_InputWithTwoLinesSeparatedWithCr_ShouldReturnBothLines()
        {
            var input = "This is row one\rThis is row two";
            var expected = new[] { "This is row one", "This is row two" };

            var result = input.SplitLines();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void SplitLines_InputWithTwoLinesSeparatedWithCrLf_ShouldReturnBothLines()
        {
            var input = "This is row one\r\nThis is row two";
            var expected = new[] { "This is row one", "This is row two" };

            var result = input.SplitLines();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void SplitLines_InputWithTwoLinesSeparatedWithAllLineEndings_ShouldReturnAllLines()
        {
            var input = "one\r\ntwo\rthree\nfour";
            var expected = new[] { "one", "two", "three", "four" };

            var result = input.SplitLines();

            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_DriveRoot_PreserveTrailingSlash()
        {
            Assert.That(@"c:\".TrimSlashes(), Is.EqualTo(@"c:\"));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_DriveRoot_AppendsTrailingSlash()
        {
            Assert.That(@"c:".TrimSlashes(), Is.EqualTo(@"c:\"));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_DriveRoot_TrimsExcessTrailingSlash()
        {
            Assert.That(@"c:\\".TrimSlashes(), Is.EqualTo(@"c:\"));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_DriveRoot_NormalizeAlternateSlash()
        {
            Assert.That(@"c:/".TrimSlashes(), Is.EqualTo(@"c:\"));
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.Drives)]
        public void TrimSlashes_RootedPath_TrimsAllTrailingSlashes()
        {
            Assert.That(@"c:\x\".TrimSlashes(), Is.EqualTo(@"c:\x"));
        }

        [Test]
        public void TrimSlashes_RootedPath_DoNotAlterPathWithoutTrailingSlashes()
        {
            Assert.That(XFS.Path(@"c:\x").TrimSlashes(), Is.EqualTo(XFS.Path(@"c:\x")));
        }

        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public void TrimSlashes_SlashRoot_TrimsExcessTrailingSlash()
        {
            Assert.That("//".TrimSlashes(), Is.EqualTo("/"));
        }

        [Test]
        [UnixOnly(UnixSpecifics.SlashRoot)]
        public void TrimSlashes_SlashRoot_PreserveSlashRoot()
        {
            Assert.That("/".TrimSlashes(), Is.EqualTo("/"));
        }

        [TestCase(@"\\unc\folder\file.txt", @"\\unc\folder\file.txt")]
        [TestCase(@"//unc/folder/file.txt", @"\\unc\folder\file.txt")]
        [WindowsOnly(WindowsSpecifics.UNCPaths)]
        public void NormalizeSlashes_KeepsUNCPathPrefix(string path, string expectedValue)
        {
            Assert.That(path.NormalizeSlashes(), Is.EqualTo(expectedValue));
        }
    }
}
