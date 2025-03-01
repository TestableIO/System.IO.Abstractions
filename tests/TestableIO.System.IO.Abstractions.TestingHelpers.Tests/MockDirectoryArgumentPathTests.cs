using System.Collections.Generic;
using System.Security.AccessControl;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    public class MockDirectoryArgumentPathTests
    {
        private static IEnumerable<Action<IDirectory>> GetFileSystemActionsForArgumentNullException()
        {
            yield return ds => ds.Delete(null);
            yield return ds => ds.Delete(null, true);
            yield return ds => ds.CreateDirectory(null);
            if (MockUnixSupport.IsWindowsPlatform())
            {
#pragma warning disable CA1416
                yield return ds => ds.CreateDirectory(null, new DirectorySecurity());
#pragma warning restore CA1416
            }
            yield return ds => ds.SetCreationTime(null, DateTime.Now);
            yield return ds => ds.SetCreationTimeUtc(null, DateTime.Now);
            yield return ds => ds.SetLastAccessTime(null, DateTime.Now);
            yield return ds => ds.SetLastAccessTimeUtc(null, DateTime.Now);
            yield return ds => ds.SetLastWriteTime(null, DateTime.Now);
            yield return ds => ds.SetLastWriteTimeUtc(null, DateTime.Now);
            yield return ds => ds.EnumerateDirectories(null);
            yield return ds => ds.EnumerateDirectories(null, "foo");
            yield return ds => ds.EnumerateDirectories(null, "foo", SearchOption.AllDirectories);
        }

        [TestCaseSource(nameof(GetFileSystemActionsForArgumentNullException))]
        public async Task Operations_ShouldThrowArgumentNullExceptionIfPathIsNull(Action<IDirectory> action)
        {
            // Arrange
            var fileSystem = new MockFileSystem();

            // Act
            Action wrapped = () => action(fileSystem.Directory);

            // Assert
            var exception = await That(wrapped).Throws<ArgumentNullException>();
            await That(exception.ParamName).IsEqualTo("path");
        }
    }
}
