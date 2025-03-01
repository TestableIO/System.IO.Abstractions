namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using Collections.Generic;

    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using XFS = MockUnixSupport;
    class MockFileLockTests
    {
        [Test]
        public async Task MockFile_Lock_FileShareNoneThrows()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            await That(() => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read)).Throws<IOException>();
        }
        [Test]
        public async Task MockFile_Lock_FileShareReadDoesNotThrowOnRead()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Read }}
            });

            await That(() => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read)).DoesNotThrow();
        }
        [Test]
        public async Task MockFile_Lock_FileShareReadThrowsOnWrite()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Read }}
            });

            await That(() => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Write, FileShare.Read)).Throws<IOException>();
        }
        [Test]
        public async Task MockFile_Lock_FileShareWriteThrowsOnRead()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Write }}
            });

            await That(() => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.Read)).Throws<IOException>();
        }
        [Test]
        public async Task MockFile_Lock_FileShareWriteDoesNotThrowOnWrite()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Write }}
            });

            await That(() => filesystem.File.Open(filepath, FileMode.Open, FileAccess.Write, FileShare.Read)).DoesNotThrow();
        }


        [Test]
        public async Task MockFile_Lock_FileShareNoneThrowsOnOpenRead()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = await That(() => filesystem.File.OpenRead(filepath)).Throws<IOException>();
            await That(exception.Message).IsEqualTo($"The process cannot access the file '{filepath}' because it is being used by another process.");
        }
        [Test]
        public async Task MockFile_Lock_FileShareNoneThrowsOnWriteAllLines()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = await That(() => filesystem.File.WriteAllLines(filepath, new string[] { "hello", "world" })).Throws<IOException>();
            await That(exception.Message).IsEqualTo($"The process cannot access the file '{filepath}' because it is being used by another process.");
        }
        [Test]
        public async Task MockFile_Lock_FileShareNoneThrowsOnReadAllLines()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = await That(() => filesystem.File.ReadAllLines(filepath)).Throws<IOException>();
            await That(exception.Message).IsEqualTo($"The process cannot access the file '{filepath}' because it is being used by another process.");
        }
        [Test]
        public async Task MockFile_Lock_FileShareNoneThrowsOnReadAllText()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = await That(() => filesystem.File.ReadAllText(filepath)).Throws<IOException>();
            await That(exception.Message).IsEqualTo($"The process cannot access the file '{filepath}' because it is being used by another process.");
        }
        [Test]
        public async Task MockFile_Lock_FileShareNoneThrowsOnReadAllBytes()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = await That(() => filesystem.File.ReadAllBytes(filepath)).Throws<IOException>();
            await That(exception.Message).IsEqualTo($"The process cannot access the file '{filepath}' because it is being used by another process.");
        }
        [Test]
        public async Task MockFile_Lock_FileShareNoneThrowsOnAppendLines()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = await That(() => filesystem.File.AppendAllLines(filepath, new string[] { "hello", "world" })).Throws<IOException>();
            await That(exception.Message).IsEqualTo($"The process cannot access the file '{filepath}' because it is being used by another process.");
        }

        [Test]
        public async Task MockFile_Lock_FileShareNoneThrowsFileMove()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            string target = XFS.Path(@"c:\something\does\notexist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = await That(() => filesystem.File.Move(filepath, target)).Throws<IOException>();
            await That(exception.Message).IsEqualTo("The process cannot access the file because it is being used by another process.");
        }
        [Test]
        public async Task MockFile_Lock_FileShareDeleteDoesNotThrowFileMove()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            string target = XFS.Path(@"c:\something\does\notexist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Delete }}
            });

            await That(() => filesystem.File.Move(filepath, target)).DoesNotThrow();
        }
        [Test]
        public async Task MockFile_Lock_FileShareNoneThrowsDelete()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.None }}
            });

            var exception = await That(() => filesystem.File.Delete(filepath)).Throws<IOException>();
            await That(exception.Message).IsEqualTo($"The process cannot access the file '{filepath}' because it is being used by another process.");
        }
        [Test]
        public async Task MockFile_Lock_FileShareDeleteDoesNotThrowDelete()
        {
            string filepath = XFS.Path(@"c:\something\does\exist.txt");
            var filesystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { filepath, new MockFileData("I'm here") { AllowedFileShare = FileShare.Delete }}
            });

            await That(() => filesystem.File.Delete(filepath)).DoesNotThrow();
        }

        private static IResolveConstraint IOException() => Is.TypeOf<IOException>().And.Property("HResult").EqualTo(unchecked((int)0x80070020));
    }
}
