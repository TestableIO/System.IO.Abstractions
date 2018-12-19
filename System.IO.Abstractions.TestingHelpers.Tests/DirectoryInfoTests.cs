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

            var current = wrapperFilesystem.Directory.GetCurrentDirectory();
            var root = wrapperFilesystem.DirectoryInfo.FromDirectoryName(current).Root;
            var rootsParent = root.Parent;
            Assert.IsNull(rootsParent);
        }
    }
}
