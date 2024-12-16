using NUnit.Framework;

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

            string expected = "File:             c:\\b.txt\r\n" +
                "InternalName:     b.txt\r\n" +
                "OriginalFilename: b.txt\r\n" +
                "FileVersion:      1.0.0.0\r\n" +
                "FileDescription:  b\r\n" +
                "Product:          b\r\n" +
                "ProductVersion:   1.0.0.0\r\n" +
                "Debug:            True\r\n" +
                "Patched:          True\r\n" +
                "PreRelease:       True\r\n" +
                "PrivateBuild:     True\r\n" +
                "SpecialBuild:     True\r\n" +
                "Language:         English\r\n";

            // Act & Assert
            Assert.That(mockFileVersionInfo.ToString(), Is.EqualTo(expected));
        }
    }
}
