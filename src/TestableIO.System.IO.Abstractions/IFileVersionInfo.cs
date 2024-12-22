using System.Diagnostics;

namespace System.IO.Abstractions
{
    /// <inheritdoc cref="FileVersionInfo" />
    public interface IFileVersionInfo
    {
        /// <inheritdoc cref="FileVersionInfo.Comments" />
        string? Comments { get; }

        /// <inheritdoc cref="FileVersionInfo.CompanyName" />
        string? CompanyName { get; }

        /// <inheritdoc cref="FileVersionInfo.FileBuildPart" />
        int FileBuildPart { get; }

        /// <inheritdoc cref="FileVersionInfo.FileDescription" />
        string? FileDescription { get; }

        /// <inheritdoc cref="FileVersionInfo.FileMajorPart" />
        int FileMajorPart { get; }

        /// <inheritdoc cref="FileVersionInfo.FileMinorPart" />
        int FileMinorPart { get; }

        /// <inheritdoc cref="FileVersionInfo.FileName" />
        string FileName { get; }

        /// <inheritdoc cref="FileVersionInfo.FilePrivatePart" />
        int FilePrivatePart { get; }

        /// <inheritdoc cref="FileVersionInfo.FileVersion" />
        string? FileVersion { get; }

        /// <inheritdoc cref="FileVersionInfo.InternalName" />
        string? InternalName { get; }

        /// <inheritdoc cref="FileVersionInfo.IsDebug" />
        bool IsDebug { get; }

        /// <inheritdoc cref="FileVersionInfo.IsPatched" />
        bool IsPatched { get; }

        /// <inheritdoc cref="FileVersionInfo.IsPrivateBuild" />
        bool IsPrivateBuild { get; }

        /// <inheritdoc cref="FileVersionInfo.IsPreRelease" />
        bool IsPreRelease { get; }

        /// <inheritdoc cref="FileVersionInfo.IsSpecialBuild" />
        bool IsSpecialBuild { get; }

        /// <inheritdoc cref="FileVersionInfo.Language" />
        string? Language { get; }

        /// <inheritdoc cref="FileVersionInfo.LegalCopyright" />
        string? LegalCopyright { get; }

        /// <inheritdoc cref="FileVersionInfo.LegalTrademarks" />
        string? LegalTrademarks { get; }

        /// <inheritdoc cref="FileVersionInfo.OriginalFilename" />
        string? OriginalFilename { get; }

        /// <inheritdoc cref="FileVersionInfo.PrivateBuild" />
        string? PrivateBuild { get; }

        /// <inheritdoc cref="FileVersionInfo.ProductBuildPart" />
        int ProductBuildPart { get; }

        /// <inheritdoc cref="FileVersionInfo.ProductMajorPart" />
        int ProductMajorPart { get; }

        /// <inheritdoc cref="FileVersionInfo.ProductMinorPart" />
        int ProductMinorPart { get; }

        /// <inheritdoc cref="FileVersionInfo.ProductName" />
        string? ProductName { get; }

        /// <inheritdoc cref="FileVersionInfo.ProductPrivatePart" />
        int ProductPrivatePart { get; }

        /// <inheritdoc cref="FileVersionInfo.ProductVersion" />
        string? ProductVersion { get; }

        /// <inheritdoc cref="FileVersionInfo.SpecialBuild" />
        string? SpecialBuild { get; }
    }
}