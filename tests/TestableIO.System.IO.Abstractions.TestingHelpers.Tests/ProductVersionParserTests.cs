using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class ProductVersionParserTests
    {
        [Test]
        public void ProductVersionParser_Parse_ShouldIgnoreTheSegmentsWhenThereAreMoreThanFiveOfThem()
        {
            // Arrange
            string productVersion = "1.2.3.4.5";

            // Act
            var parsedProductVersion = ProductVersionParser.Parse(productVersion);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(parsedProductVersion.Major, Is.Zero);
                Assert.That(parsedProductVersion.Minor, Is.Zero);
                Assert.That(parsedProductVersion.Build, Is.Zero);
                Assert.That(parsedProductVersion.PrivatePart, Is.Zero);
            });
        }

        [Test]
        [TestCase("test.2.3.4", 0, 0, 0, 0)]
        [TestCase("1.test.3.4", 1, 0, 0, 0)]
        [TestCase("1.2.test.4", 1, 2, 0, 0)]
        [TestCase("1.2.3.test", 1, 2, 3, 0)]
        public void ProductVersionParser_Parse_ShouldSkipTheRestOfTheSegmentsWhenOneIsNotValidNumber(
            string productVersion,
            int expectedMajor,
            int expectedMinor,
            int expectedBuild,
            int expectedRevision)
        {
            // Act
            var parsedProductVersion = ProductVersionParser.Parse(productVersion);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(parsedProductVersion.Major, Is.EqualTo(expectedMajor));
                Assert.That(parsedProductVersion.Minor, Is.EqualTo(expectedMinor));
                Assert.That(parsedProductVersion.Build, Is.EqualTo(expectedBuild));
                Assert.That(parsedProductVersion.PrivatePart, Is.EqualTo(expectedRevision));
            });
        }

        [Test]
        [TestCase("1-test.2.3.4", 1, 0, 0, 0)]
        [TestCase("1-test5.2.3.4", 1, 0, 0, 0)]
        [TestCase("1.2-test.3.4", 1, 2, 0, 0)]
        [TestCase("1.2-test5.3.4", 1, 2, 0, 0)]
        [TestCase("1.2.3-test.4", 1, 2, 3, 0)]
        [TestCase("1.2.3-test5.4", 1, 2, 3, 0)]
        [TestCase("1.2.3.4-test", 1, 2, 3, 4)]
        [TestCase("1.2.3.4-test5", 1, 2, 3, 4)]
        public void ProductVersionParser_Parse_ShouldSkipTheRestOfTheSegmentsWhenOneContainsMoreThanJustOneNumber(
            string productVersion,
            int expectedMajor,
            int expectedMinor,
            int expectedBuild,
            int expectedRevision)
        {
            // Act
            var parsedProductVersion = ProductVersionParser.Parse(productVersion);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(parsedProductVersion.Major, Is.EqualTo(expectedMajor));
                Assert.That(parsedProductVersion.Minor, Is.EqualTo(expectedMinor));
                Assert.That(parsedProductVersion.Build, Is.EqualTo(expectedBuild));
                Assert.That(parsedProductVersion.PrivatePart, Is.EqualTo(expectedRevision));
            });
        }

        [Test]
        [TestCase("", 0, 0, 0, 0)]
        [TestCase("1", 1, 0, 0, 0)]
        [TestCase("1.2", 1, 2, 0, 0)]
        [TestCase("1.2.3", 1, 2, 3, 0)]
        [TestCase("1.2.3.4", 1, 2, 3, 4)]
        public void ProductVersionParser_Parse_ShouldParseEachProvidedSegment(
            string productVersion,
            int expectedMajor,
            int expectedMinor,
            int expectedBuild,
            int expectedRevision)
        {
            // Act
            var parsedProductVersion = ProductVersionParser.Parse(productVersion);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(parsedProductVersion.Major, Is.EqualTo(expectedMajor));
                Assert.That(parsedProductVersion.Minor, Is.EqualTo(expectedMinor));
                Assert.That(parsedProductVersion.Build, Is.EqualTo(expectedBuild));
                Assert.That(parsedProductVersion.PrivatePart, Is.EqualTo(expectedRevision));
            });
        }
    }
}
