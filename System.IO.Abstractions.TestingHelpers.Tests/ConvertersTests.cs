using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace System.IO.Abstractions.TestingHelpers.Tests
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
        
            Assert.DoesNotThrow(() => crashingFileSystemInfos.WrapFileSystemInfos(new MockFileSystem()));
        }

        [Test]
        public void WrapFiles_with_IEnumerable_is_lazy()
        {
            var crashingFileInfos = new CrashingEnumerable<FileInfo>();
        
            Assert.DoesNotThrow(() => crashingFileInfos.WrapFiles(new MockFileSystem()));
        }
        [Test]
        public void WrapDirectories_with_IEnumerable_is_lazy()
        {
            var crashingDirectoryInfos = new CrashingEnumerable<DirectoryInfo>();
        
            Assert.DoesNotThrow(() => crashingDirectoryInfos.WrapDirectories(new MockFileSystem()));
        }

    }
}
