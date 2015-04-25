using System;
using System.Linq;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class StringExtensions
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

        [TestCase("*", Result = @"^.*$")]
        [TestCase("nowildcard.txt", Result = @"^nowildcard\.txt$")]
        [TestCase("*.txt", Result = @"^.*\.txt.*$")]
        [TestCase("test?.txt", Result = @"^test.\.txt$")]
        [TestCase("*.so", Result = @"^.*\.so$")]
        [TestCase("*.abcd", Result = @"^.*\.abcd$")]
        public string WildcardToRegexForFilenames(string wildcard)
        {
            return wildcard.WildcardToRegex(forFilename: true);
        }

        [TestCase("*", Result = @"^.*$")]
        [TestCase("nowildcard.txt", Result = @"^nowildcard\.txt$")]
        [TestCase("*.txt", Result = @"^.*\.txt$")]
        [TestCase("test?.txt", Result = @"^test.\.txt$")]
        [TestCase("*.so", Result = @"^.*\.so$")]
        [TestCase("*.abcd", Result = @"^.*\.abcd$")]
        public string WildcardToRegex(string wildcard)
        {
            return wildcard.WildcardToRegex();
        }

    }
}
