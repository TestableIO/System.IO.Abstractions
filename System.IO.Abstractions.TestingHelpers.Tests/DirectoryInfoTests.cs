using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class DirectoryInfoTests
    {
        [Test]
        public void Parent_ForRootDirectory_ShouldReturnNull()
        {
            var wrapperFilesystem = new FileSystem();

            var directoryInfo = wrapperFilesystem.DirectoryInfo.FromDirectoryName("C:\\");
            var rootsParent = directoryInfo.Parent;
            Assert.IsNull(rootsParent);
        }
    }
}
