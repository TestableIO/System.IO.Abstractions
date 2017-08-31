namespace System.IO.Abstractions
{
    using System.Diagnostics;

    internal class FileVersionInfoWrapper : FileVersionInfoBase
    {
        private FileVersionInfo versionInfo;

        public FileVersionInfoWrapper(FileVersionInfo versionInfo)
        {
            this.versionInfo = versionInfo;
        }

        public override bool IsPrivateBuild
        {
            get { return versionInfo.IsPrivateBuild; }
        }

        public override int ProductPrivatePart
        {
            get { return versionInfo.ProductPrivatePart; }
        }

        public override string ProductName
        {
            get { return versionInfo.ProductName; }
        }

        public override int ProductMinorPart
        {
            get { return versionInfo.ProductMinorPart; }
        }

        public override int ProductMajorPart
        {
            get { return versionInfo.ProductMajorPart; }
        }

        public override int ProductBuildPart
        {
            get { return versionInfo.ProductBuildPart; }
        }

        public override string PrivateBuild
        {
            get { return versionInfo.PrivateBuild; }
        }

        public override string OriginalFilename
        {
            get { return versionInfo.OriginalFilename; }
        }

        public override string LegalTrademarks
        {
            get { return versionInfo.LegalTrademarks; }
        }

        public override string LegalCopyright
        {
            get { return versionInfo.LegalCopyright; }
        }

        public override string Language
        {
            get { return versionInfo.Language; }
        }

        public override bool IsSpecialBuild
        {
            get { return versionInfo.IsSpecialBuild; }
        }

        public override bool IsPreRelease
        {
            get { return versionInfo.IsPreRelease; }
        }

        public override string ProductVersion
        {
            get { return versionInfo.ProductVersion; }
        }

        public override string SpecialBuild
        {
            get { return versionInfo.SpecialBuild; }
        }

        public override bool IsDebug
        {
            get { return versionInfo.IsDebug; }
        }

        public override string InternalName
        {
            get { return versionInfo.InternalName; }
        }

        public override string FileVersion
        {
            get { return versionInfo.FileVersion; }
        }

        public override int FilePrivatePart
        {
            get { return versionInfo.FilePrivatePart; }
        }

        public override string FileName
        {
            get { return versionInfo.FileName; }
        }

        public override int FileMinorPart
        {
            get { return versionInfo.FileMinorPart; }
        }

        public override int FileMajorPart
        {
            get { return versionInfo.FileMajorPart; }
        }

        public override string FileDescription
        {
            get { return versionInfo.FileDescription; }
        }

        public override int FileBuildPart
        {
            get { return versionInfo.FileBuildPart; }
        }

        public override string CompanyName
        {
            get { return versionInfo.CompanyName; }
        }

        public override string Comments
        {
            get { return versionInfo.Comments; }
        }

        public override bool IsPatched
        {
            get { return versionInfo.IsPatched; }
        }
    }
}
