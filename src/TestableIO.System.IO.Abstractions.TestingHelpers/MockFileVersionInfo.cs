using System.Diagnostics;
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
            string comments = null,
            string companyName = null,
            string fileDescription = null,
            string fileName = null,
            string fileVersion = null,
            string internalName = null,
            bool isDebug = false,
            bool isPatched = false,
            bool isPrivateBuild = false,
            bool isPreRelease = false,
            bool isSpecialBuild = false,
            string language = null,
            string legalCopyright = null,
            string legalTrademarks = null,
            string originalFilename = null,
            string privateBuild = null,
            string productName = null,
            string productVersion = null,
            string specialBuild = null)
        {
            Comments = comments;
            CompanyName = companyName;
            FileDescription = fileDescription;
            FileName = fileName;
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
            ProductName = productName;
            SpecialBuild = specialBuild;

            if (fileVersion != null)
            {
                ParseVersion(fileVersion, out int fileMajor, out int fileMinor, out int fileBuild, out int filePrivate);
                FileMajorPart = fileMajor;
                FileMinorPart = fileMinor;
                FileBuildPart = fileBuild;
                FilePrivatePart = filePrivate;
            }

            if (productVersion != null)
            {
                ParseVersion(productVersion, out int productMajor, out int productMinor, out int productBuild, out int productPrivate);
                ProductMajorPart = productMajor;
                ProductMinorPart = productMinor;
                ProductBuildPart = productBuild;
                ProductPrivatePart = productPrivate;
            }
        }

        /// <inheritdoc/>
        public override string Comments
        {
            get;
        }

        /// <inheritdoc/>
        public override string CompanyName
        {
            get;
        }

        /// <inheritdoc/>
        public override int FileBuildPart
        {
            get;
        }

        /// <inheritdoc/>
        public override string FileDescription
        {
            get;
        }

        /// <inheritdoc/>
        public override int FileMajorPart
        {
            get;
        }

        /// <inheritdoc/>
        public override int FileMinorPart
        {
            get;
        }

        /// <inheritdoc/>
        public override string FileName
        {
            get;
        }

        /// <inheritdoc/>
        public override int FilePrivatePart
        {
            get;
        }

        /// <inheritdoc/>
        public override string FileVersion
        {
            get
            {
                return $"{FileMajorPart}.{FileMinorPart}.{FileBuildPart}.{FilePrivatePart}";
            }
        }

        /// <inheritdoc/>
        public override string InternalName
        {
            get;
        }

        /// <inheritdoc/>
        public override bool IsDebug
        {
            get;
        }

        /// <inheritdoc/>
        public override bool IsPatched
        {
            get;
        }

        /// <inheritdoc/>
        public override bool IsPrivateBuild
        {
            get;
        }

        /// <inheritdoc/>
        public override bool IsPreRelease
        {
            get;
        }

        /// <inheritdoc/>
        public override bool IsSpecialBuild
        {
            get;
        }

        /// <inheritdoc/>
        public override string Language
        {
            get;
        }

        /// <inheritdoc/>
        public override string LegalCopyright
        {
            get;
        }

        /// <inheritdoc/>
        public override string LegalTrademarks
        {
            get;
        }

        /// <inheritdoc/>
        public override string OriginalFilename
        {
            get;
        }

        /// <inheritdoc/>
        public override string PrivateBuild
        {
            get;
        }

        /// <inheritdoc/>
        public override int ProductBuildPart
        {
            get;
        }

        /// <inheritdoc/>
        public override int ProductMajorPart
        {
            get;
        }

        /// <inheritdoc/>
        public override int ProductMinorPart
        {
            get;
        }

        /// <inheritdoc/>
        public override string ProductName
        {
            get;
        }

        /// <inheritdoc/>
        public override int ProductPrivatePart
        {
            get;
        }

        /// <inheritdoc/>
        public override string ProductVersion
        {
            get
            {
                return $"{ProductMajorPart}.{ProductMinorPart}.{ProductBuildPart}.{ProductPrivatePart}";
            }
        }

        /// <inheritdoc/>
        public override string SpecialBuild
        {
            get;
        }

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

        private static void ParseVersion(string version, out int major, out int minor, out int build, out int revision)
        {
            var parts = version.Split('.');
            if (parts.Length != 4)
            {
                throw new FormatException("Version string must have the format 'major.minor.build.revision'.");
            }

            if (!int.TryParse(parts[0], out major) ||
                !int.TryParse(parts[1], out minor) ||
                !int.TryParse(parts[2], out build) ||
                !int.TryParse(parts[3], out revision))
            {
                throw new FormatException("Version parts must be numeric.");
            }
        }
    }
}
