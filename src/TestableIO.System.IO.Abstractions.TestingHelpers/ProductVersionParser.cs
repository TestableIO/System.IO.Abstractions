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
        /// <returns>
        /// A <see cref="ProductVersion"/> object containing the parsed major, minor, build, and private parts. 
        /// If the input is invalid, returns a <see cref="ProductVersion"/> with all parts set to 0.
        /// </returns>
        /// <remarks>
        /// The method splits the input string into segments separated by dots ('.') and attempts to extract
        /// the leading numeric value from each segment. A maximum of 4 segments are processed; if more than
        /// 4 segments are present, all segments are ignored. Additionally, if a segment does not contain 
        /// a valid numeric part at its start or it contains more than just a number, the rest of the segments are ignored.
        /// </remarks>
        public static ProductVersion Parse(string productVersion)
        {
            if (string.IsNullOrWhiteSpace(productVersion))
            {
                return new();
            }

            var segments = productVersion.Split('.');
            if (segments.Length > 4)
            {
                // if more than 4 segments are present, all segments are ignored
                return new();
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

            return new()
            {
                Major = parts[0],
                Minor = parts[1],
                Build = parts[2],
                PrivatePart = parts[3]
            };
        }

        /// <summary>
        /// Represents a product version with numeric parts for major, minor, build, and private versions.
        /// </summary>
        public class ProductVersion
        {
            /// <summary>
            /// Gets the major part of the version number
            /// </summary>
            public int Major { get; init; }

            /// <summary>
            /// Gets the minor part of the version number
            /// </summary>
            public int Minor { get; init; }

            /// <summary>
            /// Gets the build part of the version number
            /// </summary>
            public int Build { get; init; }

            /// <summary>
            /// Gets the private part of the version number
            /// </summary>
            public int PrivatePart { get; init; }
        }
    }
}
