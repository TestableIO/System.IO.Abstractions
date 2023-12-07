using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace System.IO.Abstractions.Tests
{
    [TestFixture]
    public class ConvertersTests
    {
        private sealed class CrashingEnumerable<T> : IEnumerable<T>
        {
            private sealed class CrashingEnumerator : IEnumerator<T>
            {
                object IEnumerator.Current => throw new NotSupportedException();

                public T Current => throw new NotSupportedException();

                public bool MoveNext() { throw new NotSupportedException(); }

                public void Reset() { }

                public void Dispose() { }
            }

            public IEnumerator<T> GetEnumerator() => new CrashingEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Test]
        public void WrapFileSystemInfos_with_IEnumerable_is_lazy()
        {
            var crashingFileSystemInfos = new CrashingEnumerable<FileSystemInfo>();

            Assert.DoesNotThrow(() => crashingFileSystemInfos.WrapFileSystemInfos(new FileSystem()));
        }

        [Test]
        public void WrapFiles_with_IEnumerable_is_lazy()
        {
            var crashingFileInfos = new CrashingEnumerable<FileInfo>();

            Assert.DoesNotThrow(() => crashingFileInfos.WrapFiles(new FileSystem()));
        }

        [Test]
        public void WrapDirectories_with_IEnumerable_is_lazy()
        {
            var crashingDirectoryInfos = new CrashingEnumerable<DirectoryInfo>();

            Assert.DoesNotThrow(() => crashingDirectoryInfos.WrapDirectories(new FileSystem()));
        }

        [Test]
        public void WrapFileSystemInfo_handles_null_FileSystemInfo()
        {
            Assert.That(Converters.WrapFileSystemInfo(null, new FileSystem()), Is.Null);
        }

        [Test]
        public void WrapDirectories_handles_null_DirectoryInfo()
        {
            List<DirectoryInfo> directoryInfos = new() { null };
            Assert.That(directoryInfos.WrapDirectories(new FileSystem()).Single(), Is.Null);
        }

        [Test]
        public void WrapFiles_handles_null_FileInfo()
        {
            List<FileInfo> fileInfos = new() { null };
            Assert.That(fileInfos.WrapFiles(new FileSystem()).Single(), Is.Null);
        }
    }
}
