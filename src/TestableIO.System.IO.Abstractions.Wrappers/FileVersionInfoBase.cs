using System.Diagnostics;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="IFileVersionInfo"/>
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public abstract class FileVersionInfoBase : IFileVersionInfo
    {
        /// <inheritdoc cref="IFileVersionInfo.Comments" />
        public abstract string Comments { get; }

        /// <inheritdoc cref="IFileVersionInfo.CompanyName" />
        public abstract string CompanyName { get; }

        /// <inheritdoc cref="IFileVersionInfo.FileBuildPart" />
        public abstract int FileBuildPart { get; }

        /// <inheritdoc cref="IFileVersionInfo.FileDescription" />
        public abstract string FileDescription { get; }

        /// <inheritdoc cref="IFileVersionInfo.FileMajorPart" />
        public abstract int FileMajorPart { get; }

        /// <inheritdoc cref="IFileVersionInfo.FileMinorPart" />
        public abstract int FileMinorPart { get; }

        /// <inheritdoc cref="IFileVersionInfo.FileName" />
        public abstract string FileName { get; }

        /// <inheritdoc cref="IFileVersionInfo.FilePrivatePart" />
        public abstract int FilePrivatePart { get; }

        /// <inheritdoc cref="IFileVersionInfo.FileVersion" />
        public abstract string FileVersion { get; }

        /// <inheritdoc cref="IFileVersionInfo.InternalName" />
        public abstract string InternalName { get; }

        /// <inheritdoc cref="IFileVersionInfo.IsDebug" />
        public abstract bool IsDebug { get; }

        /// <inheritdoc cref="IFileVersionInfo.IsPatched" />
        public abstract bool IsPatched { get; }

        /// <inheritdoc cref="IFileVersionInfo.IsPrivateBuild" />
        public abstract bool IsPrivateBuild { get; }

        /// <inheritdoc cref="IFileVersionInfo.IsPreRelease" />
        public abstract bool IsPreRelease { get; }

        /// <inheritdoc cref="IFileVersionInfo.IsSpecialBuild" />
        public abstract bool IsSpecialBuild { get; }

        /// <inheritdoc cref="IFileVersionInfo.Language" />
        public abstract string Language { get; }

        /// <inheritdoc cref="IFileVersionInfo.LegalCopyright" />
        public abstract string LegalCopyright { get; }

        /// <inheritdoc cref="IFileVersionInfo.LegalTrademarks" />
        public abstract string LegalTrademarks { get; }

        /// <inheritdoc cref="IFileVersionInfo.OriginalFilename" />
        public abstract string OriginalFilename { get; }

        /// <inheritdoc cref="IFileVersionInfo.PrivateBuild" />
        public abstract string PrivateBuild { get; }

        /// <inheritdoc cref="IFileVersionInfo.ProductBuildPart" />
        public abstract int ProductBuildPart { get; }

        /// <inheritdoc cref="IFileVersionInfo.ProductMajorPart" />
        public abstract int ProductMajorPart { get; }

        /// <inheritdoc cref="IFileVersionInfo.ProductMinorPart" />
        public abstract int ProductMinorPart { get; }

        /// <inheritdoc cref="IFileVersionInfo.ProductName" />
        public abstract string ProductName { get; }

        /// <inheritdoc cref="IFileVersionInfo.ProductPrivatePart" />
        public abstract int ProductPrivatePart { get; }

        /// <inheritdoc cref="IFileVersionInfo.ProductVersion" />
        public abstract string ProductVersion { get; }

        /// <inheritdoc cref="IFileVersionInfo.SpecialBuild" />
        public abstract string SpecialBuild { get; }

        /// <inheritdoc />
        public static implicit operator FileVersionInfoBase(FileVersionInfo fileVersionInfo)
        {
            if (fileVersionInfo == null)
            {
                return null;
            }

            return new FileVersionInfoWrapper(fileVersionInfo);
        }

        /// <inheritdoc />
        public new abstract string ToString();
    }
}
