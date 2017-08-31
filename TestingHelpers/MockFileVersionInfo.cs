namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileVersionInfo : MockFileVersionInfoMutableAdapter
    {
        public override bool IsPrivateBuild_
        {
            get { return IsPrivateBuild; }
        }

        public new bool IsPrivateBuild { get; set; }

        public override int ProductPrivatePart_
        {
            get { return ProductPrivatePart; }
        }

        public new int ProductPrivatePart { get; set; }

        public override string ProductName_
        {
            get { return ProductName; }
        }

        public new string ProductName { get; set; }

        public override int ProductMinorPart_
        {
            get { return ProductMinorPart; }
        }

        public new int ProductMinorPart { get; set; }

        public override int ProductMajorPart_
        {
            get { return ProductMajorPart; }
        }

        public new int ProductMajorPart { get; set; }

        public override int ProductBuildPart_
        {
            get { return ProductBuildPart; }
        }

        public new int ProductBuildPart { get; set; }

        public override string PrivateBuild_
        {
            get { return PrivateBuild; }
        }

        public new string PrivateBuild { get; set; }

        public override string OriginalFilename_
        {
            get { return OriginalFilename; }
        }

        public new string OriginalFilename { get; set; }

        public override string LegalTrademarks_
        {
            get { return LegalTrademarks; }
        }

        public new string LegalTrademarks { get; set; }

        public override string LegalCopyright_
        {
            get { return LegalCopyright; }
        }

        public new string LegalCopyright { get; set; }

        public override string Language_
        {
            get { return Language; }
        }

        public new string Language { get; set; }

        public override bool IsSpecialBuild_
        {
            get { return IsSpecialBuild; }
        }

        public new bool IsSpecialBuild { get; set; }

        public override bool IsPreRelease_
        {
            get { return IsPreRelease; }
        }

        public new bool IsPreRelease { get; set; }

        public override string ProductVersion_
        {
            get { return ProductVersion; }
        }

        public new string ProductVersion { get; set; }

        public override string SpecialBuild_
        {
            get { return SpecialBuild; }
        }

        public new string SpecialBuild { get; set; }

        public override bool IsDebug_
        {
            get { return IsDebug; }
        }

        public new bool IsDebug { get; set; }

        public override string InternalName_
        {
            get { return InternalName; }
        }

        public new string InternalName { get; set; }

        public override string FileVersion
        {
            get { return string.Format("{0}.{1}.{2}.{3}", FileMajorPart, FileMinorPart, FileBuildPart, FilePrivatePart); }
        }

        public override int FilePrivatePart_
        {
            get { return FilePrivatePart; }
        }

        public new int FilePrivatePart { get; set; }

        public override string FileName_
        {
            get { return FileName; }
        }

        public new string FileName { get; set; }

        public override int FileMinorPart_
        {
            get { return FileMinorPart; }
        }

        public new int FileMinorPart { get; set; }

        public override int FileMajorPart_
        {
            get { return FileMajorPart; }
        }

        public new int FileMajorPart { get; set; }

        public override string FileDescription_
        {
            get { return FileDescription; }
        }

        public new string FileDescription { get; set; }

        public override int FileBuildPart_
        {
            get { return FileBuildPart; }
        }

        public new int FileBuildPart { get; set; }

        public override string CompanyName_
        {
            get { return CompanyName; }
        }

        public new string CompanyName { get; set; }

        public override string Comments_
        {
            get { return Comments; }
        }

        public new string Comments { get; set; }

        public override bool IsPatched_
        {
            get { return IsPatched; }
        }

        public new bool IsPatched { get; set; }
    }
}
