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
            ProductVersionParser.Parse(
                productVersion,
                out int productMajor,
                out int productMinor,
                out int productBuild,
                out int productPrivate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(productMajor, Is.Zero);
                Assert.That(productMinor, Is.Zero);
                Assert.That(productBuild, Is.Zero);
                Assert.That(productPrivate, Is.Zero);
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
            ProductVersionParser.Parse(
                productVersion,
                out int productMajor,
                out int productMinor,
                out int productBuild,
                out int productPrivate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(productMajor, Is.EqualTo(expectedMajor));
                Assert.That(productMinor, Is.EqualTo(expectedMinor));
                Assert.That(productBuild, Is.EqualTo(expectedBuild));
                Assert.That(productPrivate, Is.EqualTo(expectedRevision));
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
            ProductVersionParser.Parse(
                productVersion,
                out int productMajor,
                out int productMinor,
                out int productBuild,
                out int productPrivate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(productMajor, Is.EqualTo(expectedMajor));
                Assert.That(productMinor, Is.EqualTo(expectedMinor));
                Assert.That(productBuild, Is.EqualTo(expectedBuild));
                Assert.That(productPrivate, Is.EqualTo(expectedRevision));
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
            ProductVersionParser.Parse(
                productVersion,
                out int productMajor,
                out int productMinor,
                out int productBuild,
                out int productPrivate);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(productMajor, Is.EqualTo(expectedMajor));
                Assert.That(productMinor, Is.EqualTo(expectedMinor));
                Assert.That(productBuild, Is.EqualTo(expectedBuild));
                Assert.That(productPrivate, Is.EqualTo(expectedRevision));
            });
        }
    }
}
