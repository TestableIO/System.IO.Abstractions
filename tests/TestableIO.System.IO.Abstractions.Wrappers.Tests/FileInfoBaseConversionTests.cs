﻿namespace System.IO.Abstractions.Tests;

/// <summary>
/// Unit tests for the conversion operators of the <see cref="FileInfoBase"/> class.
/// </summary>
public class FileInfoBaseConversionTests
{
    /// <summary>
    /// Tests that a <c>null</c> <see cref="FileInfo"/> is correctly converted to a <c>null</c> <see cref="FileInfoBase"/> without exception.
    /// </summary>
    [Test]
    public async Task FileInfoBase_FromFileInfo_ShouldReturnNullIfFileInfoIsNull()
    {
        // Arrange
        FileInfo fileInfo = null;

        // Act
        FileInfoBase actual = fileInfo;

        // Assert
        await That(actual).IsNull();
    }
}