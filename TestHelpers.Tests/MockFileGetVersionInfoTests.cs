namespace System.IO.Abstractions.TestingHelpers.Tests
{
    using System.Diagnostics;
    using System.Reflection;
    using NUnit.Framework;

    internal class MockFileGetVersionInfoTests
    {
        [Test]
        public void MockFileVersionInfo_ToString_ShouldReturnMajorMinorBuildPrivate()
        {
            var version = new MockFileVersionInfo { FileMajorPart = 1, FileMinorPart = 2, FileBuildPart = 3, FilePrivatePart = 4 };
            Assert.AreEqual("1.2.3.4", version.FileVersion);
        }

        [Test]
        public void MockFileInfo_VersionInfo_ShouldBeAbleToSetMockVersionInfo()
        {
            var version = new MockFileVersionInfo { ProductName = "Testing MockFileVersionInfo" };
            var fileData = new MockFileData("Demo text content") { VersionInfo = version };
            Assert.AreSame(fileData.VersionInfo, version);
        }

        [Test]
        public void MockFileInfo_VersionInfo_ShouldReadFromDll()
        {
            var fs = new FileSystem();
            var path = Assembly.GetExecutingAssembly().Location;
            var abstractInfo = fs.FileInfo.FromFileName(path);
            var realVersionInfo = FileVersionInfo.GetVersionInfo(path);
            Assert.AreEqual(realVersionInfo.FileVersion, abstractInfo.GetVersion().FileVersion);
        }

        [Test]
        public void MockFileVersionInfo_CanBeCastFromFileVersionInfo()
        {
            var path = Assembly.GetExecutingAssembly().Location;
            FileVersionInfoBase abstractInfo = FileVersionInfo.GetVersionInfo(path);
            Assert.NotNull(abstractInfo);
        }

        [Test]
        public void FileInfoVersionWrapper_ConstructedWithNull_ShouldThrow()
        {
            TestDelegate wrapped = () => 
            {
                FileVersionInfoBase abstraction = (FileVersionInfo)null;
            };

            var exception = Assert.Throws<ArgumentNullException>(wrapped);
            Assert.AreEqual("instance", exception.ParamName);
        }
    }
}
