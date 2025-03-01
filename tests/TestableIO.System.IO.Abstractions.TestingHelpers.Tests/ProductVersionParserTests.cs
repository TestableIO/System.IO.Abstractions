using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class ProductVersionParserTests
    {
        [Test]
        public async Task ProductVersionParser_Parse_ShouldIgnoreTheSegmentsWhenThereAreMoreThanFiveOfThem()
        {
            // Arrange
            string productVersion = "1.2.3.4.5";

            // Act
            var versionInfo = new MockFileVersionInfo("foo", productVersion: productVersion);

            // Assert
            await ThatAll(
                That(versionInfo.ProductMajorPart).IsEqualTo(0),
                That(versionInfo.ProductMinorPart).IsEqualTo(0),
                That(versionInfo.ProductBuildPart).IsEqualTo(0),
                That(versionInfo.ProductPrivatePart).IsEqualTo(0)
            );
        }

        [Test]
        [TestCase("test.2.3.4", 0, 0, 0, 0)]
        [TestCase("1.test.3.4", 1, 0, 0, 0)]
        [TestCase("1.2.test.4", 1, 2, 0, 0)]
        [TestCase("1.2.3.test", 1, 2, 3, 0)]
        public async Task ProductVersionParser_Parse_ShouldSkipTheRestOfTheSegmentsWhenOneIsNotValidNumber(
            string productVersion,
            int expectedMajor,
            int expectedMinor,
            int expectedBuild,
            int expectedRevision)
        {
            // Act
            var versionInfo = new MockFileVersionInfo("foo", productVersion: productVersion);

            // Assert
            await ThatAll(
                That(versionInfo.ProductMajorPart).IsEqualTo(expectedMajor),
                That(versionInfo.ProductMinorPart).IsEqualTo(expectedMinor),
                That(versionInfo.ProductBuildPart).IsEqualTo(expectedBuild),
                That(versionInfo.ProductPrivatePart).IsEqualTo(expectedRevision)
            );
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
        public async Task ProductVersionParser_Parse_ShouldSkipTheRestOfTheSegmentsWhenOneContainsMoreThanJustOneNumber(
            string productVersion,
            int expectedMajor,
            int expectedMinor,
            int expectedBuild,
            int expectedRevision)
        {
            // Act
            var versionInfo = new MockFileVersionInfo("foo", productVersion: productVersion);

            // Assert
            
            await ThatAll(
                That(versionInfo.ProductMajorPart).IsEqualTo(expectedMajor),
                That(versionInfo.ProductMinorPart).IsEqualTo(expectedMinor),
                That(versionInfo.ProductBuildPart).IsEqualTo(expectedBuild),
                That(versionInfo.ProductPrivatePart).IsEqualTo(expectedRevision)
            );
        }

        [Test]
        [TestCase("", 0, 0, 0, 0)]
        [TestCase("1", 1, 0, 0, 0)]
        [TestCase("1.2", 1, 2, 0, 0)]
        [TestCase("1.2.3", 1, 2, 3, 0)]
        [TestCase("1.2.3.4", 1, 2, 3, 4)]
        public async Task ProductVersionParser_Parse_ShouldParseEachProvidedSegment(
            string productVersion,
            int expectedMajor,
            int expectedMinor,
            int expectedBuild,
            int expectedRevision)
        {
            // Act
            var versionInfo = new MockFileVersionInfo("foo", productVersion: productVersion);

            // Assert
            await ThatAll(
                That(versionInfo.ProductMajorPart).IsEqualTo(expectedMajor),
                That(versionInfo.ProductMinorPart).IsEqualTo(expectedMinor),
                That(versionInfo.ProductBuildPart).IsEqualTo(expectedBuild),
                That(versionInfo.ProductPrivatePart).IsEqualTo(expectedRevision)
            );
        }
    }
}
