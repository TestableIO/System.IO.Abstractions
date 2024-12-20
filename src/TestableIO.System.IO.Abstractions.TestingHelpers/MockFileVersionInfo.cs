﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class MockFileVersionInfo : FileVersionInfoBase
    {
        /// <inheritdoc />
        public MockFileVersionInfo(
            string fileName,
            string fileVersion,
            string productVersion,
            string fileDescription,
            string productName,
            string companyName,
            string comments,
            string internalName,
            bool isDebug,
            bool isPatched,
            bool isPrivateBuild,
            bool isPreRelease,
            bool isSpecialBuild,
            string language,
            string legalCopyright,
            string legalTrademarks,
            string originalFilename,
            string privateBuild,
            string specialBuild)
        {
            FileName = fileName;
            FileVersion = fileVersion;
            ProductVersion = productVersion;
            FileDescription = fileDescription;
            ProductName = productName;
            CompanyName = companyName;
            Comments = comments;
            InternalName = internalName;
            IsDebug = isDebug;
            IsPatched = isPatched;
            IsPrivateBuild = isPrivateBuild;
            IsPreRelease = isPreRelease;
            IsSpecialBuild = isSpecialBuild;
            Language = language;
            LegalCopyright = legalCopyright;
            LegalTrademarks = legalTrademarks;
            OriginalFilename = originalFilename;
            PrivateBuild = privateBuild;
            SpecialBuild = specialBuild;

            if (Version.TryParse(fileVersion, out Version version))
            {
                FileMajorPart = version.Major;
                FileMinorPart = version.Minor;
                FileBuildPart = version.Build;
                FilePrivatePart = version.Revision;
            }

            ProductVersionParser.Parse(
                productVersion,
                out int productMajor,
                out int productMinor,
                out int productBuild,
                out int productPrivate);

            ProductMajorPart = productMajor;
            ProductMinorPart = productMinor;
            ProductBuildPart = productBuild;
            ProductPrivatePart = productPrivate;
        }

        /// <inheritdoc/>
        public MockFileVersionInfo(string fileName)
            : this(fileName, null, null, null, null, null, null, null, false, false, false, false, false, null, null, null, null, null, null) { }

        /// <inheritdoc/>
        public MockFileVersionInfo(string fileName, string fileVersion, string productVersion)
            : this(fileName, fileVersion, productVersion, null, null, null, null, null, false, false, false, false, false, null, null, null, null, null, null) { }

        /// <inheritdoc/>
        public MockFileVersionInfo()
            : this(null, null, null, null, null, null, null, null, false, false, false, false, false, null, null, null, null, null, null) { }

        /// <inheritdoc/>
        public override string FileName { get; }

        /// <inheritdoc/>
        public override string FileVersion { get; }

        /// <inheritdoc/>
        public override string ProductVersion { get; }

        /// <inheritdoc/>
        public override string FileDescription { get; }

        /// <inheritdoc/>
        public override string ProductName { get; }

        /// <inheritdoc/>
        public override string CompanyName { get; }

        /// <inheritdoc/>
        public override string Comments { get; }

        /// <inheritdoc/>
        public override string InternalName { get; }

        /// <inheritdoc/>
        public override bool IsDebug { get; }

        /// <inheritdoc/>
        public override bool IsPatched { get; }

        /// <inheritdoc/>
        public override bool IsPrivateBuild { get; }

        /// <inheritdoc/>
        public override bool IsPreRelease { get; }

        /// <inheritdoc/>
        public override bool IsSpecialBuild { get; }

        /// <inheritdoc/>
        public override string Language { get; }

        /// <inheritdoc/>
        public override string LegalCopyright { get; }

        /// <inheritdoc/>
        public override string LegalTrademarks { get; }

        /// <inheritdoc/>
        public override string OriginalFilename { get; }

        /// <inheritdoc/>
        public override string PrivateBuild { get; }

        /// <inheritdoc/>
        public override string SpecialBuild { get; }

        /// <inheritdoc/>
        public override int FileMajorPart { get; }

        /// <inheritdoc/>
        public override int FileMinorPart { get; }

        /// <inheritdoc/>
        public override int FileBuildPart { get; }

        /// <inheritdoc/>
        public override int FilePrivatePart { get; }

        /// <inheritdoc/>
        public override int ProductMajorPart { get; }

        /// <inheritdoc/>
        public override int ProductMinorPart { get; }

        /// <inheritdoc/>
        public override int ProductBuildPart { get; }

        /// <inheritdoc/>
        public override int ProductPrivatePart { get; }

        /// <inheritdoc cref="FileVersionInfo.ToString" />
        public override string ToString()
        {
            // An initial capacity of 512 was chosen because it is large enough to cover
            // the size of the static strings with enough capacity left over to cover
            // average length property values.
            var sb = new StringBuilder(512);
            sb.Append("File:             ").AppendLine(FileName);
            sb.Append("InternalName:     ").AppendLine(InternalName);
            sb.Append("OriginalFilename: ").AppendLine(OriginalFilename);
            sb.Append("FileVersion:      ").AppendLine(FileVersion);
            sb.Append("FileDescription:  ").AppendLine(FileDescription);
            sb.Append("Product:          ").AppendLine(ProductName);
            sb.Append("ProductVersion:   ").AppendLine(ProductVersion);
            sb.Append("Debug:            ").AppendLine(IsDebug.ToString());
            sb.Append("Patched:          ").AppendLine(IsPatched.ToString());
            sb.Append("PreRelease:       ").AppendLine(IsPreRelease.ToString());
            sb.Append("PrivateBuild:     ").AppendLine(IsPrivateBuild.ToString());
            sb.Append("SpecialBuild:     ").AppendLine(IsSpecialBuild.ToString());
            sb.Append("Language:         ").AppendLine(Language);
            return sb.ToString();
        }
    }
}
