namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using XFS = MockUnixSupport;
    class MockFileLockTests
    {
        [Test]
        public void MockFile_Lock_FileShareNoneThrows()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            Assert.Throws(IOException(), () => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read));
        }
        [Test]
        public void MockFile_Lock_FileShareReadDoesNotThrowOnRead()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Read }}
            });

            Assert.DoesNotThrow(() => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read));
        }
        [Test]
        public void MockFile_Lock_FileShareReadThrowsOnWrite()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Read }}
            });

            Assert.Throws(IOException(), () => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Write, FileShare.Read));
        }
        [Test]
        public void MockFile_Lock_FileShareWriteThrowsOnRead()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Write }}
            });

            Assert.Throws(IOException(), () => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read));
        }
        [Test]
        public void MockFile_Lock_FileShareWriteDoesNotThrowOnWrite()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Write }}
            });

            Assert.DoesNotThrow(() => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Write, FileShare.Read));
        }


        [Test]
        public void MockFile_Lock_FileShareNoneThrowsOnOpenRead()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = Assert.Throws(IOException(), () => filesystem.File.OpenRead(filepath));
            Assert.That(exception.Message, Is.EqualTo($"The process cannot access the file '{filepath}' because it is being used by another process."));
        }
        [Test]
        public void MockFile_Lock_FileShareNoneThrowsOnWriteAllLines()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = Assert.Throws(IOException(), () => filesystem.File.WriteAllLines(filepath, new string[] { "hello", "world" }));
            Assert.That(exception.Message, Is.EqualTo($"The process cannot access the file '{filepath}' because it is being used by another process."));
        }
        [Test]
        public void MockFile_Lock_FileShareNoneThrowsOnReadAllLines()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = Assert.Throws(IOException(), () => filesystem.File.ReadAllLines(filepath));
            Assert.That(exception.Message, Is.EqualTo($"The process cannot access the file '{filepath}' because it is being used by another process."));
        }
        [Test]
        public void MockFile_Lock_FileShareNoneThrowsOnReadAllText()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = Assert.Throws(IOException(), () => filesystem.File.ReadAllText(filepath));
            Assert.That(exception.Message, Is.EqualTo($"The process cannot access the file '{filepath}' because it is being used by another process."));
        }
        [Test]
        public void MockFile_Lock_FileShareNoneThrowsOnReadAllBytes()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = Assert.Throws(IOException(), () => filesystem.File.ReadAllBytes(filepath));
            Assert.That(exception.Message, Is.EqualTo($"The process cannot access the file '{filepath}' because it is being used by another process."));
        }
        [Test]
        public void MockFile_Lock_FileShareNoneThrowsOnAppendLines()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = Assert.Throws(IOException(), () => filesystem.File.AppendAllLines(filepath, new string[] { "hello", "world" }));
            Assert.That(exception.Message, Is.EqualTo($"The process cannot access the file '{filepath}' because it is being used by another process."));
        }

        [Test]
        public void MockFile_Lock_FileShareNoneThrowsFileMove()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            string target = XFS.Path(@"c:\something\does\notexist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = Assert.Throws(IOException(), () => filesystem.File.Move(filepath, target));
            Assert.That(exception.Message, Is.EqualTo("The process cannot access the file because it is being used by another process."));
        }
        [Test]
        public void MockFile_Lock_FileShareDeleteDoesNotThrowFileMove()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            string target = XFS.Path(@"c:\something\does\notexist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Delete }}
            });

            Assert.DoesNotThrow(() => filesystem.File.Move(filepath, target));
        }
        [Test]
        public void MockFile_Lock_FileShareNoneThrowsDelete()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = Assert.Throws(IOException(), () => filesystem.File.Delete(filepath));
            Assert.That(exception.Message, Is.EqualTo($"The process cannot access the file '{filepath}' because it is being used by another process."));
        }
        [Test]
        public void MockFile_Lock_FileShareDeleteDoesNotThrowDelete()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Delete }}
            });

            Assert.DoesNotThrow(() => filesystem.File.Delete(filepath));
        }

        private static IResolveConstraint IOException() => Is.TypeOf<IOException>().And.Property("HResult").EqualTo(unchecked((int) 0x80070020));
    }
}
