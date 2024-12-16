using NUnit.Framework;
using System.Collections;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class MockFileVersionInfoTests
    {
        [Test]
        public void MockFileVersionInfo_ToString_ShouldReturnTheDefaultFormat()
        {
            // Arrange
            var mockFileVersionInfo = new MockFileVersionInfo(
                fileName: @"c:\b.txt",
                internalName: "b.txt",
                originalFilename: "b.txt",
                fileVersion: "1.0.0.0",
                fileDescription: "b",
                productName: "b",
                productVersion: "1.0.0.0",
                isDebug: true,
                isPatched: true,
                isPreRelease: true,
                isPrivateBuild: true,
                isSpecialBuild: true,
                language: "English");

            string expected = @"File:             c:\b.txt
InternalName:     b.txt
OriginalFilename: b.txt
FileVersion:      1.0.0.0
FileDescription:  b
Product:          b
ProductVersion:   1.0.0.0
Debug:            True
Patched:          True
PreRelease:       True
PrivateBuild:     True
SpecialBuild:     True
Language:         English
";

            // Act & Assert
            Assert.That(mockFileVersionInfo.ToString(), Is.EqualTo(expected));
        }

        [Test]
        public void MockFileVersionInfo_Constructor_ShouldSetFileVersionNumbersIfFileVersionIsNotNull()
        {
            // Arrange
            var mockFileVersionInfo = new MockFileVersionInfo(fileVersion: "1.2.3.4");

            // Assert
            Assert.That(mockFileVersionInfo.FileMajorPart, Is.EqualTo(1));
            Assert.That(mockFileVersionInfo.FileMinorPart, Is.EqualTo(2));
            Assert.That(mockFileVersionInfo.FileBuildPart, Is.EqualTo(3));
            Assert.That(mockFileVersionInfo.FilePrivatePart, Is.EqualTo(4));
        }

        [Test]
        public void MockFileVersionInfo_Constructor_ShouldSetProductVersionNumbersIfProductVersionIsNotNull()
        {
            // Act
            var mockFileVersionInfo = new MockFileVersionInfo(productVersion: "1.2.3.4");

            // Assert
            Assert.That(mockFileVersionInfo.ProductMajorPart, Is.EqualTo(1));
            Assert.That(mockFileVersionInfo.ProductMinorPart, Is.EqualTo(2));
            Assert.That(mockFileVersionInfo.ProductBuildPart, Is.EqualTo(3));
            Assert.That(mockFileVersionInfo.ProductPrivatePart, Is.EqualTo(4));
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidVersionStrings))]
        public void MockFileVersionInfo_Constructor_ShouldThrowFormatExceptionIfFileVersionFormatIsInvalid(string version)
        {
            // Assert
            Assert.Throws<FormatException>(() => new MockFileVersionInfo(fileVersion: version));
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidVersionStrings))]
        public void MockFileVersionInfo_Constructor_ShouldThrowFormatExceptionIfProductVersionFormatIsInvalid(string version)
        {
            // Assert
            Assert.Throws<FormatException>(() => new MockFileVersionInfo(productVersion: version));
        }

        private static IEnumerable GetInvalidVersionStrings()
        {
            yield return "";
            yield return "1,2,3,4";
            yield return "1.2.34";
            yield return "1.2.build.4";
        }
    }
}
