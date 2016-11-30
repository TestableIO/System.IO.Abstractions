using System.Collections.Generic;
using System.Security.AccessControl;
using Xunit;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockDirectoryArgumentPathTests
    {
        private static IEnumerable<object[]> GetFileSystemActionsForArgumentNullException()
        {
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.Delete(null))};
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.Delete(null, true))};
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.CreateDirectory(null))};
#if NET45
                yield return new object[] {(Action<DirectoryBase>) (ds => ds.CreateDirectory(null, new DirectorySecurity()))};
#endif
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.SetCreationTime(null, DateTime.Now))};
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.SetCreationTimeUtc(null, DateTime.Now))};
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.SetLastAccessTime(null, DateTime.Now))};
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.SetLastAccessTimeUtc(null, DateTime.Now))};
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.SetLastWriteTime(null, DateTime.Now))};
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.SetLastWriteTimeUtc(null, DateTime.Now))};
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.EnumerateDirectories(null))};
            yield return new object[] {(Action<DirectoryBase>) (ds => ds.EnumerateDirectories(null, "foo"))};
            yield return
                new object[] {(Action<DirectoryBase>) (ds => ds.EnumerateDirectories(null, "foo", SearchOption.AllDirectories))}
                ;
        }

        [Theory]
        [MemberData(nameof(GetFileSystemActionsForArgumentNullException))]
        public void Operations_ShouldThrowArgumentNullExceptionIfPathIsNull(Action<DirectoryBase> action)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action wrapped = () => action(fileSystem.Directory);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(wrapped);
            Assert.Equal("path", exception.ParamName);
        }
    }
}
