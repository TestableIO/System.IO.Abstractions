namespace System.IO.Abstractions.TestingHelpers
{
    /// <summary>
    /// Flags indicating which times to adjust for a <see cref="MockFileData"/>.
    /// </summary>
    [Flags]
    public enum TimeAdjustments
    {
        /// <summary>
        /// Adjusts no times on the <see cref="MockFileData"/>
        /// </summary>
        None = 0,
        /// <summary>
        /// Adjusts the <see cref="MockFileData.CreationTime"/>
        /// </summary>
        CreationTime = 1 << 0,
        /// <summary>
        /// Adjusts the <see cref="MockFileData.LastAccessTime"/>
        /// </summary>
        LastAccessTime = 1 << 1,
        /// <summary>
        /// Adjusts the <see cref="MockFileData.LastWriteTime"/>
        /// </summary>
        LastWriteTime = 1 << 2,
        /// <summary>
        /// Adjusts all times on the <see cref="MockFileData"/>
        /// </summary>
        All = ~0
    }
}
