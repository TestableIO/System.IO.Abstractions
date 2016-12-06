using FluentAssertions;
using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class StringExtensions
    {
        [Fact]
        public void SplitLines_InputWithOneLine_ShouldReturnOnlyOneLine()
        {
            var input = "This is row one";
            var expected = new[] { "This is row one" };

            var result = input.SplitLines();

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SplitLines_InputWithTwoLinesSeparatedWithLf_ShouldReturnBothLines() {
            var input = "This is row one\nThis is row two";
            var expected = new[] { "This is row one", "This is row two" };

            var result = input.SplitLines();

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SplitLines_InputWithTwoLinesSeparatedWithCr_ShouldReturnBothLines() {
            var input = "This is row one\rThis is row two";
            var expected = new[] { "This is row one", "This is row two" };

            var result = input.SplitLines();

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SplitLines_InputWithTwoLinesSeparatedWithCrLf_ShouldReturnBothLines() {
            var input = "This is row one\r\nThis is row two";
            var expected = new[] { "This is row one", "This is row two" };

            var result = input.SplitLines();

            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public void SplitLines_InputWithTwoLinesSeparatedWithAllLineEndings_ShouldReturnAllLines() {
            var input = "one\r\ntwo\rthree\nfour";
            var expected = new[] { "one", "two", "three", "four" };

            var result = input.SplitLines();

            result.Should().BeEquivalentTo(expected);
        }

    }
}
