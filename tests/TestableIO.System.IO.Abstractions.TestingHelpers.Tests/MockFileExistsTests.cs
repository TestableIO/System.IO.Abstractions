namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using NUnit.Framework;

    using XFS = MockUnixSupport;

    public class MockFileExistsTests
    {
        [Test]
        public async Task MockFile_Exists_ShouldReturnTrueForSamePath()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"C:\something\other.gif"), new MockFileData("gif content") }
            });

            // Act
            var result = fileSystem.File.Exists(XFS.Path(@"C:\something\other.gif"));

            // Assert
            await That(result).IsTrue();
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
        public async Task MockFile_Exists_ShouldReturnTrueForPathVaryingByCase()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\something\demo.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.File.Exists(@"C:\SomeThing\DEMO.txt");

            // Assert
            await That(result).IsTrue();
        }

        [Test]
        [UnixOnly(UnixSpecifics.CaseSensitivity)]
        public async Task MockFile_Exists_ShouldReturnFalseForPathVaryingByCase()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "/something/demo.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.File.Exists("/SomeThing/DEMO.txt");

            // Assert
            await That(result).IsFalse();
        }

        [Test]
        public async Task MockFile_Exists_ShouldReturnFalseForEntirelyDifferentPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"C:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"C:\something\other.gif"), new MockFileData("gif content") }
            });

            // Act
            var result = fileSystem.File.Exists(XFS.Path(@"C:\SomeThing\DoesNotExist.gif"));

            // Assert
            await That(result).IsFalse();
        }

        [Test]
        public async Task MockFile_Exists_ShouldReturnFalseForNullPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.File.Exists(null);

            // Assert
            await That(result).IsFalse();
        }

        [Test]
        public async Task MockFile_Exists_ShouldReturnFalseForEmptyStringPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.File.Exists(string.Empty);

            // Assert
            await That(result).IsFalse();
        }

        [Test]
        public async Task MockFile_Exists_ShouldReturnFalseForInvalidCharactersInPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.File.Exists(@"C:""*/:<>?|abc");

            // Assert
            await That(result).IsFalse();
        }

        [Test]
        public async Task MockFile_Exists_ShouldReturnFalseForDirectories()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"C:\something\demo.txt"), new MockFileData("Demo text content") },
                { XFS.Path(@"C:\something\other.gif"), new MockFileData("gif content") }
            });

            // Act
            var result = fileSystem.File.Exists(XFS.Path(@"C:\something\"));

            // Assert
            await That(result).IsFalse();
        }
    }
}