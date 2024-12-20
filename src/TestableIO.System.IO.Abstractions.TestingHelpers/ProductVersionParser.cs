using System.Reflection;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// Provides functionality to parse a product version string into its major, minor, build, and private parts.
    /// </summary>
    public static class ProductVersionParser
    {
        /// <summary>
        /// Parses a product version string and extracts the numeric values for the major, minor, build, and private parts,
        /// mimicking the behavior of the <see cref="AssemblyInformationalVersionAttribute"/> attribute.
        /// </summary>
        /// <param name="productVersion">The product version string to parse.</param>
        /// <param name="productMajorPart">Outputs the major part of the version. Defaults to 0 if not found.</param>
        /// <param name="productMinorPart">Outputs the minor part of the version. Defaults to 0 if not found.</param>
        /// <param name="productBuildPart">Outputs the build part of the version. Defaults to 0 if not found.</param>
        /// <param name="productPrivatePart">Outputs the private part of the version. Defaults to 0 if not found.</param>
        /// <remarks>
        /// The method splits the input string into segments separated by dots ('.') and attempts to extract
        /// the leading numeric value from each segment. A maximum of 4 segments are processed; if more than
        /// 4 segments are present, all segments are ignored. Additionally, if a segment does not contain 
        /// a valid numeric part at its start or it contains more than just a number, the rest of the segments are ignored.
        /// </remarks>
        public static void Parse(
           string productVersion,
           out int productMajorPart,
           out int productMinorPart,
           out int productBuildPart,
           out int productPrivatePart)
        {
            productMajorPart = 0;
            productMinorPart = 0;
            productBuildPart = 0;
            productPrivatePart = 0;

            if (string.IsNullOrWhiteSpace(productVersion))
            {
                return;
            }

            var segments = productVersion.Split('.');
            if (segments.Length > 4)
            {
                // if more than 4 segments are present, all segments are ignored
                return;
            }

            var regex = new Regex(@"^\d+");

            int[] parts = new int[4];

            for (int i = 0; i < segments.Length; i++)
            {
                var match = regex.Match(segments[i]);
                if (match.Success && int.TryParse(match.Value, out int number))
                {
                    parts[i] = number;

                    if (match.Value != segments[i])
                    {
                        // when a segment contains more than a number, the rest of the segments are ignored
                        break;
                    }
                }
                else
                {
                    // when a segment is not valid, the rest of the segments are ignored
                    break;
                }
            }

            productMajorPart = parts[0];
            productMinorPart = parts[1];
            productBuildPart = parts[2];
            productPrivatePart = parts[3];
        }
    }
}
