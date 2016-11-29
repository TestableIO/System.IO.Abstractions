using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileArgumentPathTests
    {
        private static IEnumerable<object[]> FileSystemActionsForArgumentNullException
        {
            get
            {
                yield return new object[] {(Action<FileBase>) (fs => fs.AppendAllLines(null, new[] {"does not matter"}))};
                yield return new object[] {(Action<FileBase>) (fs => fs.AppendAllLines(null, new[] {"does not matter"}, Encoding.ASCII))};
                yield return new object[] {(Action<FileBase>) (fs => fs.AppendAllText(null, "does not matter"))};
                yield return new object[] {(Action<FileBase>) (fs => fs.AppendAllText(null, "does not matter", Encoding.ASCII))};
                yield return new object[] {(Action<FileBase>) (fs => fs.AppendText(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.WriteAllBytes(null, new byte[] {0}))};
                yield return new object[] {(Action<FileBase>) (fs => fs.WriteAllLines(null, new[] {"does not matter"}))};
                yield return new object[] {(Action<FileBase>) (fs => fs.WriteAllLines(null, new[] {"does not matter"}, Encoding.ASCII))};
                yield return new object[] {(Action<FileBase>) (fs => fs.WriteAllLines(null, new[] {"does not matter"}.ToArray()))};
                yield return new object[] {(Action<FileBase>) (fs => fs.WriteAllLines(null, new[] {"does not matter"}.ToArray(), Encoding.ASCII))};
                yield return new object[] {(Action<FileBase>) (fs => fs.Create(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.Delete(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.GetCreationTime(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.GetCreationTimeUtc(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.GetLastAccessTime(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.GetLastAccessTimeUtc(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.GetLastWriteTime(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.GetLastWriteTimeUtc(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.WriteAllText(null, "does not matter"))};
                yield return new object[] {(Action<FileBase>) (fs => fs.WriteAllText(null, "does not matter", Encoding.ASCII))};
                yield return new object[] {(Action<FileBase>) (fs => fs.Open(null, FileMode.OpenOrCreate))};
                yield return new object[] {(Action<FileBase>) (fs => fs.Open(null, FileMode.OpenOrCreate, FileAccess.Read))};
                yield return new object[] {(Action<FileBase>) (fs => fs.Open(null, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Inheritable))};
                yield return new object[] {(Action<FileBase>) (fs => fs.OpenRead(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.OpenText(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.OpenWrite(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.ReadAllBytes(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.ReadAllLines(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.ReadAllLines(null, Encoding.ASCII))};
                yield return new object[] {(Action<FileBase>) (fs => fs.ReadAllText(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.ReadAllText(null, Encoding.ASCII))};
                yield return new object[] {(Action<FileBase>) (fs => fs.ReadLines(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.ReadLines(null, Encoding.ASCII))};
                yield return new object[] {(Action<FileBase>) (fs => fs.SetAttributes(null, FileAttributes.Archive))};
                yield return new object[] {(Action<FileBase>) (fs => fs.GetAttributes(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.SetCreationTime(null, DateTime.Now))};
                yield return new object[] {(Action<FileBase>) (fs => fs.SetCreationTimeUtc(null, DateTime.Now))};
                yield return new object[] {(Action<FileBase>) (fs => fs.SetLastAccessTime(null, DateTime.Now))};
                yield return new object[] {(Action<FileBase>) (fs => fs.SetLastAccessTimeUtc(null, DateTime.Now))};
                yield return new object[] {(Action<FileBase>) (fs => fs.SetLastWriteTime(null, DateTime.Now))};
                yield return new object[] {(Action<FileBase>) (fs => fs.SetLastWriteTimeUtc(null, DateTime.Now))};
#if NET45
                yield return new object[] {(Action<FileBase>) (fs => fs.Decrypt(null))};
                yield return new object[] {(Action<FileBase>) (fs => fs.Encrypt(null))};
#endif
            }
        }

        [Theory]
        [MemberData("GetFileSystemActionsForArgumentNullException")]
        public void Operations_ShouldThrowArgumentNullExceptionIfPathIsNull(Action<FileBase> action)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action wrapped = () => action(fileSystem.File);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(wrapped);
            Assert.Equal("path", exception.ParamName);
        }
    }
}
