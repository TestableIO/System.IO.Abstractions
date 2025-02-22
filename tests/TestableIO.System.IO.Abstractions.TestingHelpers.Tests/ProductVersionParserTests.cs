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
            var versionInfo = new MockFileVersionInfo("foo", productVersion: productVersion);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(versionInfo.ProductMajorPart, Is.Zero);
                Assert.That(versionInfo.ProductMinorPart, Is.Zero);
                Assert.That(versionInfo.ProductBuildPart, Is.Zero);
                Assert.That(versionInfo.ProductPrivatePart, Is.Zero);
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
            var versionInfo = new MockFileVersionInfo("foo", productVersion: productVersion);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(versionInfo.ProductMajorPart, Is.EqualTo(expectedMajor));
                Assert.That(versionInfo.ProductMinorPart, Is.EqualTo(expectedMinor));
                Assert.That(versionInfo.ProductBuildPart, Is.EqualTo(expectedBuild));
                Assert.That(versionInfo.ProductPrivatePart, Is.EqualTo(expectedRevision));
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
            var versionInfo = new MockFileVersionInfo("foo", productVersion: productVersion);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(versionInfo.ProductMajorPart, Is.EqualTo(expectedMajor));
                Assert.That(versionInfo.ProductMinorPart, Is.EqualTo(expectedMinor));
                Assert.That(versionInfo.ProductBuildPart, Is.EqualTo(expectedBuild));
                Assert.That(versionInfo.ProductPrivatePart, Is.EqualTo(expectedRevision));
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
            var versionInfo = new MockFileVersionInfo("foo", productVersion: productVersion);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(versionInfo.ProductMajorPart, Is.EqualTo(expectedMajor));
                Assert.That(versionInfo.ProductMinorPart, Is.EqualTo(expectedMinor));
                Assert.That(versionInfo.ProductBuildPart, Is.EqualTo(expectedBuild));
                Assert.That(versionInfo.ProductPrivatePart, Is.EqualTo(expectedRevision));
            });
        }
    }
}
