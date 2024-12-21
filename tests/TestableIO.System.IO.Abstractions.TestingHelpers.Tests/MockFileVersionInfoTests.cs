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
                fileVersion: "1.0.0.0",
                productVersion: "1.0.0.0",
                fileDescription: "b",
                productName: "b",
                companyName: null,
                comments: null,
                internalName: "b.txt",
                isDebug: true,
                isPatched: true,
                isPrivateBuild: true,
                isPreRelease: true,
                isSpecialBuild: true,
                language: "English",
                legalCopyright: null,
                legalTrademarks: null,
                originalFilename: "b.txt",
                privateBuild: null,
                specialBuild: null);

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
        public void MockFileVersionInfo_Constructor_ShouldSetFileAndProductVersionNumbersIfFileAndProductVersionAreNotNull()
        {
            // Arrange
            var mockFileVersionInfo = new MockFileVersionInfo(@"c:\file.txt", fileVersion: "1.2.3.4", productVersion: "5.6.7.8");

            // Assert
            Assert.That(mockFileVersionInfo.FileMajorPart, Is.EqualTo(1));
            Assert.That(mockFileVersionInfo.FileMinorPart, Is.EqualTo(2));
            Assert.That(mockFileVersionInfo.FileBuildPart, Is.EqualTo(3));
            Assert.That(mockFileVersionInfo.FilePrivatePart, Is.EqualTo(4));
            Assert.That(mockFileVersionInfo.ProductMajorPart, Is.EqualTo(5));
            Assert.That(mockFileVersionInfo.ProductMinorPart, Is.EqualTo(6));
            Assert.That(mockFileVersionInfo.ProductBuildPart, Is.EqualTo(7));
            Assert.That(mockFileVersionInfo.ProductPrivatePart, Is.EqualTo(8));
        }

        [Test]
        public void MockFileVersionInfo_Constructor_ShouldNotSetFileAndProductVersionNumbersIfFileAndProductVersionAreNull()
        {
            // Act
            var mockFileVersionInfo = new MockFileVersionInfo(@"c:\a.txt");

            // Assert
            Assert.That(mockFileVersionInfo.FileMajorPart, Is.EqualTo(0));
            Assert.That(mockFileVersionInfo.FileMinorPart, Is.EqualTo(0));
            Assert.That(mockFileVersionInfo.FileBuildPart, Is.EqualTo(0));
            Assert.That(mockFileVersionInfo.FilePrivatePart, Is.EqualTo(0));
            Assert.That(mockFileVersionInfo.ProductMajorPart, Is.EqualTo(0));
            Assert.That(mockFileVersionInfo.ProductMinorPart, Is.EqualTo(0));
            Assert.That(mockFileVersionInfo.ProductBuildPart, Is.EqualTo(0));
            Assert.That(mockFileVersionInfo.ProductPrivatePart, Is.EqualTo(0));
        }
    }
}
