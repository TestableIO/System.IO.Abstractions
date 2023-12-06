namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using NUnit.Framework;

    using XFS = MockUnixSupport;

    public class MockFileExistsTests
    {
        [Test]
        public void MockFile_Exists_ShouldReturnTrueForSamePath()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { XFS.Path(@"C:\something\other.gif"), new MockFileData("gif content") }
            });

            // Act
            var result = fileSystem.File.Exists(XFS.Path(@"C:\something\other.gif"));

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        [WindowsOnly(WindowsSpecifics.CaseInsensitivity)]
        public void MockFile_Exists_ShouldReturnTrueForPathVaryingByCase()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"C:\something\demo.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.File.Exists(@"C:\SomeThing\DEMO.txt");

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        [UnixOnly(UnixSpecifics.CaseSensitivity)]
        public void MockFile_Exists_ShouldReturnFalseForPathVaryingByCase()
        {
            // Arrange
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { "/something/demo.txt", new MockFileData("Demo text content") }
            });

            // Act
            var result = fileSystem.File.Exists("/SomeThing/DEMO.txt");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void MockFile_Exists_ShouldReturnFalseForEntirelyDifferentPath()
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
            Assert.That(result, Is.False);
        }

        [Test]
        public void MockFile_Exists_ShouldReturnFalseForNullPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.File.Exists(null);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void MockFile_Exists_ShouldReturnFalseForEmptyStringPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.File.Exists(string.Empty);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void MockFile_Exists_ShouldReturnFalseForInvalidCharactersInPath()
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            var result = fileSystem.File.Exists(@"C:""*/:<>?|abc");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void MockFile_Exists_ShouldReturnFalseForDirectories()
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
            Assert.That(result, Is.False);
        }
    }
}