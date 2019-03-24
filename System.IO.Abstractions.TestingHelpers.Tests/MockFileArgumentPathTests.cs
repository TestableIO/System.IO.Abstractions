using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockFileArgumentPathTests
    {
        private static IEnumerable<Action<IFile>> GetFileSystemActionsForArgumentNullException()
        {
            yield return fs => fs.AppendAllLines(null, new[] { "does not matter" });
            yield return fs => fs.AppendAllLines(null, new[] { "does not matter" }, Encoding.ASCII);
            yield return fs => fs.AppendAllText(null, "does not matter");
            yield return fs => fs.AppendAllText(null, "does not matter", Encoding.ASCII);
            yield return fs => fs.AppendText(null);
            yield return fs => fs.WriteAllBytes(null, new byte[] { 0 });
            yield return fs => fs.WriteAllLines(null, new[] { "does not matter" });
            yield return fs => fs.WriteAllLines(null, new[] { "does not matter" }, Encoding.ASCII);
            yield return fs => fs.WriteAllLines(null, new[] { "does not matter" }.ToArray());
            yield return fs => fs.WriteAllLines(null, new[] { "does not matter" }.ToArray(), Encoding.ASCII);
            yield return fs => fs.Create(null);
            yield return fs => fs.Delete(null);
            yield return fs => fs.GetCreationTime(null);
            yield return fs => fs.GetCreationTimeUtc(null);
            yield return fs => fs.GetLastAccessTime(null);
            yield return fs => fs.GetLastAccessTimeUtc(null);
            yield return fs => fs.GetLastWriteTime(null);
            yield return fs => fs.GetLastWriteTimeUtc(null);
            yield return fs => fs.WriteAllText(null, "does not matter");
            yield return fs => fs.WriteAllText(null, "does not matter", Encoding.ASCII);
            yield return fs => fs.Open(null, FileMode.OpenOrCreate);
            yield return fs => fs.Open(null, FileMode.OpenOrCreate, FileAccess.Read);
            yield return fs => fs.Open(null, FileMode.OpenOrCreate, FileAccess.Read, FileShare.Inheritable);
            yield return fs => fs.OpenRead(null);
            yield return fs => fs.OpenText(null);
            yield return fs => fs.OpenWrite(null);
            yield return fs => fs.ReadAllBytes(null);
            yield return fs => fs.ReadAllLines(null);
            yield return fs => fs.ReadAllLines(null, Encoding.ASCII);
            yield return fs => fs.ReadAllText(null);
            yield return fs => fs.ReadAllText(null, Encoding.ASCII);
            yield return fs => fs.ReadLines(null);
            yield return fs => fs.ReadLines(null, Encoding.ASCII);
            yield return fs => fs.SetAttributes(null, FileAttributes.Archive);
            yield return fs => fs.GetAttributes(null);
            yield return fs => fs.SetCreationTime(null, DateTime.Now);
            yield return fs => fs.SetCreationTimeUtc(null, DateTime.Now);
            yield return fs => fs.SetLastAccessTime(null, DateTime.Now);
            yield return fs => fs.SetLastAccessTimeUtc(null, DateTime.Now);
            yield return fs => fs.SetLastWriteTime(null, DateTime.Now);
            yield return fs => fs.SetLastWriteTimeUtc(null, DateTime.Now);
#if NET40
            yield return fs => fs.Decrypt(null);
            yield return fs => fs.Encrypt(null);
#endif
        }

        [TestCaseSource("GetFileSystemActionsForArgumentNullException")]
        public void Operations_ShouldThrowArgumentNullExceptionIfPathIsNull(Action<IFile> action)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            TestDelegate wrapped = () => action(fileSystem.File);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(wrapped);
            Assert.AreEqual("path", exception.ParamName);
        }
    }
}
