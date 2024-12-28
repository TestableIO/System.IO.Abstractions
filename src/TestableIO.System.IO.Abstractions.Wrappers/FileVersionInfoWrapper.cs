using System.Diagnostics;

namespace System.IO.Abstractions
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class FileVersionInfoWrapper : FileVersionInfoBase
    {
        private readonly FileVersionInfo instance;

        /// <inheritdoc />
        public FileVersionInfoWrapper(FileVersionInfo fileVersionInfo)
        {
            instance = fileVersionInfo;
        }

        /// <inheritdoc/>
        public override string Comments
        {
            get { return instance.Comments; }
        }

        /// <inheritdoc/>
        public override string CompanyName
        {
            get { return instance.CompanyName; }
        }

        /// <inheritdoc/>
        public override int FileBuildPart
        {
            get { return instance.FileBuildPart; }
        }

        /// <inheritdoc/>
        public override string FileDescription
        {
            get { return instance.FileDescription; }
        }

        /// <inheritdoc/>
        public override int FileMajorPart
        {
            get { return instance.FileMajorPart; }
        }

        /// <inheritdoc/>
        public override int FileMinorPart
        {
            get { return instance.FileMinorPart; }
        }

        /// <inheritdoc/>
        public override string FileName
        {
            get { return instance.FileName; }
        }

        /// <inheritdoc/>
        public override int FilePrivatePart
        {
            get { return instance.FilePrivatePart; }
        }

        /// <inheritdoc/>
        public override string FileVersion
        {
            get { return instance.FileVersion; }
        }

        /// <inheritdoc/>
        public override string InternalName
        {
            get { return instance.InternalName; }
        }

        /// <inheritdoc/>
        public override bool IsDebug
        {
            get { return instance.IsDebug; }
        }

        /// <inheritdoc/>
        public override bool IsPatched
        {
            get { return instance.IsPatched; }
        }

        /// <inheritdoc/>
        public override bool IsPrivateBuild
        {
            get { return instance.IsPrivateBuild; }
        }

        /// <inheritdoc/>
        public override bool IsPreRelease
        {
            get { return instance.IsPreRelease; }
        }

        /// <inheritdoc/>
        public override bool IsSpecialBuild
        {
            get { return instance.IsSpecialBuild; }
        }

        /// <inheritdoc/>
        public override string Language
        {
            get { return instance.Language; }
        }

        /// <inheritdoc/>
        public override string LegalCopyright
        {
            get { return instance.LegalCopyright; }
        }

        /// <inheritdoc/>
        public override string LegalTrademarks
        {
            get { return instance.LegalTrademarks; }
        }

        /// <inheritdoc/>
        public override string OriginalFilename
        {
            get { return instance.OriginalFilename; }
        }

        /// <inheritdoc/>
        public override string PrivateBuild
        {
            get { return instance.PrivateBuild; }
        }

        /// <inheritdoc/>
        public override int ProductBuildPart
        {
            get { return instance.ProductBuildPart; }
        }

        /// <inheritdoc/>
        public override int ProductMajorPart
        {
            get { return instance.ProductMajorPart; }
        }

        /// <inheritdoc/>
        public override int ProductMinorPart
        {
            get { return instance.ProductMinorPart; }
        }

        /// <inheritdoc/>
        public override string ProductName
        {
            get { return instance.ProductName; }
        }

        /// <inheritdoc/>
        public override int ProductPrivatePart
        {
            get { return instance.ProductPrivatePart; }
        }

        /// <inheritdoc/>
        public override string ProductVersion
        {
            get { return instance.ProductVersion; }
        }

        /// <inheritdoc/>
        public override string SpecialBuild
        {
            get { return instance.SpecialBuild; }
        }

        /// <inheritdoc cref="FileVersionInfo.ToString" />
        public override string ToString()
        {
            return instance.ToString();
        }
    }
}
