namespace System.IO.Abstractions
{
    using System.Diagnostics;

    /// <summary>
    /// Abstraction around System.Diagnostics.FileVersionInfo.
    ///
    /// You can get the File Version Info from the normal File Info of the IFileSystem:
    ///   filesystem.FileInfo.FromFileName(path).GetVersion();
    /// 
    /// When testing, MockFileData's VersionInfo property is a MockFileVersionInfo, which
    /// allows setting the various properties as desired:
    ///   mockFileSystem.AddFile(@"c:\file.dll", new MockFileData("content")
    ///   {
    ///       VersionInfo = new MockFileVersionInfo
    ///       {
    ///           FileMajorPart = 93,
    ///           FileMinorPart = 10,
    ///       },
    ///   });
    /// </summary>
    [Serializable]
    public abstract class FileVersionInfoBase
    {
        public static implicit operator FileVersionInfoBase(FileVersionInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return new FileVersionInfoWrapper(instance);
        }

        public abstract bool IsPrivateBuild { get; }

        public abstract int ProductPrivatePart { get; }

        public abstract string ProductName { get; }

        public abstract int ProductMinorPart { get; }

        public abstract int ProductMajorPart { get; }

        public abstract int ProductBuildPart { get; }

        public abstract string PrivateBuild { get; }

        public abstract string OriginalFilename { get; }

        public abstract string LegalTrademarks { get; }

        public abstract string LegalCopyright { get; }

        public abstract string Language { get; }

        public abstract bool IsSpecialBuild { get; }

        public abstract bool IsPreRelease { get; }

        public abstract string ProductVersion { get; }

        public abstract string SpecialBuild { get; }

        public abstract bool IsDebug { get; }

        public abstract string InternalName { get; }

        public abstract string FileVersion { get; }

        public abstract int FilePrivatePart { get; }

        public abstract string FileName { get; }

        public abstract int FileMinorPart { get; }

        public abstract int FileMajorPart { get; }

        public abstract string FileDescription { get; }

        public abstract int FileBuildPart { get; }

        public abstract string CompanyName { get; }

        public abstract string Comments { get; }

        public abstract bool IsPatched { get; }
    }
}
