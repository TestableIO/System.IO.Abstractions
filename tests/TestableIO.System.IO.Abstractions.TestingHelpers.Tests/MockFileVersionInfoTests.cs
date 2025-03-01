using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests;

[TestFixture]
public class MockFileVersionInfoTests
{
    [Test]
    public async Task MockFileVersionInfo_ToString_ShouldReturnTheDefaultFormat()
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
        await That(mockFileVersionInfo.ToString()).IsEqualTo(expected);
    }

    [Test]
    public async Task MockFileVersionInfo_Constructor_ShouldSetFileAndProductVersionNumbersIfFileAndProductVersionAreNotNull()
    {
        // Arrange
        var mockFileVersionInfo = new MockFileVersionInfo(@"c:\file.txt", fileVersion: "1.2.3.4", productVersion: "5.6.7.8");

        // Assert
        await That(mockFileVersionInfo.FileMajorPart).IsEqualTo(1);
        await That(mockFileVersionInfo.FileMinorPart).IsEqualTo(2);
        await That(mockFileVersionInfo.FileBuildPart).IsEqualTo(3);
        await That(mockFileVersionInfo.FilePrivatePart).IsEqualTo(4);
        await That(mockFileVersionInfo.ProductMajorPart).IsEqualTo(5);
        await That(mockFileVersionInfo.ProductMinorPart).IsEqualTo(6);
        await That(mockFileVersionInfo.ProductBuildPart).IsEqualTo(7);
        await That(mockFileVersionInfo.ProductPrivatePart).IsEqualTo(8);
    }

    [Test]
    public async Task MockFileVersionInfo_Constructor_ShouldNotSetFileAndProductVersionNumbersIfFileAndProductVersionAreNull()
    {
        // Act
        var mockFileVersionInfo = new MockFileVersionInfo(@"c:\a.txt");

        // Assert
        await That(mockFileVersionInfo.FileMajorPart).IsEqualTo(0);
        await That(mockFileVersionInfo.FileMinorPart).IsEqualTo(0);
        await That(mockFileVersionInfo.FileBuildPart).IsEqualTo(0);
        await That(mockFileVersionInfo.FilePrivatePart).IsEqualTo(0);
        await That(mockFileVersionInfo.ProductMajorPart).IsEqualTo(0);
        await That(mockFileVersionInfo.ProductMinorPart).IsEqualTo(0);
        await That(mockFileVersionInfo.ProductBuildPart).IsEqualTo(0);
        await That(mockFileVersionInfo.ProductPrivatePart).IsEqualTo(0);
    }
}