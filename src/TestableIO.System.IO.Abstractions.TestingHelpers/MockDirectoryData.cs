﻿using System.Runtime.Versioning;
using System.Security.AccessControl;

namespace System.IO.Abstractions.TestingHelpers
{
    /// <inheritdoc />
#if FEATURE_SERIALIZABLE
    [Serializable]
#endif
    public class MockDirectoryData : MockFileData
    {
#if FEATURE_SERIALIZABLE
        [NonSerialized]
#endif
        private DirectorySecurity accessControl;

        /// <inheritdoc />
        public MockDirectoryData() : base(string.Empty)
        {
            Attributes = FileAttributes.Directory;
        }

        /// <inheritdoc />
        [SupportedOSPlatform("windows")]
        public new DirectorySecurity AccessControl
        {
            get
            {
                // DirectorySecurity's constructor will throw PlatformNotSupportedException on non-Windows platform, so we initialize it in lazy way.
                // This let's us use this class as long as we don't use AccessControl property.
                return accessControl ?? (accessControl = new DirectorySecurity());
            }
            set { accessControl = value; }
        }
    }
}