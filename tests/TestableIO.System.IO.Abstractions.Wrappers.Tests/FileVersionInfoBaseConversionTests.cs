using System.Diagnostics;

namespace System.IO.Abstractions.Tests
{
    /// <summary>
    /// Unit tests for the conversion operators of the <see cref="FileVersionInfoBase"/> class.
    /// </summary>
    public class FileVersionInfoBaseConversionTests
    {
        /// <summary>
        /// Tests that a <c>null</c> <see cref="FileVersionInfo"/> is correctly converted to a <c>null</c> <see cref="FileVersionInfoBase"/> without exception.
        /// </summary>
        [Test]
        public async Task FileVersionInfoBase_FromFileVersionInfo_ShouldReturnNullIfFileVersionInfoIsNull()
        {
            // Arrange
            FileVersionInfo fileVersionInfo = null;

            // Act
            FileVersionInfoBase actual = fileVersionInfo;

            // Assert
            await That(actual).IsNull();
        }
    }
}
