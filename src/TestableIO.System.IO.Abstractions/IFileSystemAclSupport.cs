using System.Runtime.Versioning;

namespace System.IO.Abstractions
{
    /// <summary>
    ///     Support ACL functionality on file system instances.
    /// </summary>
    public interface IFileSystemAclSupport
    {
        /// <summary>
        ///     Retrieves previously stored access control information.
        /// </summary>
        [SupportedOSPlatform("windows")]
        object GetAccessControl(AccessControlSections includeSections = AccessControlSections.Default);

        /// <summary>
        ///     Stores access control information.
        /// </summary>
        [SupportedOSPlatform("windows")]
        void SetAccessControl(object value);

        /// <summary>
        /// Specifies which sections of a security descriptor to save or load.</summary>
        [Flags]
        public enum AccessControlSections
        {
            /// <summary>
            /// No sections.
            /// </summary>
            None = 0,

            /// <summary>
            /// The system access control list (SACL).
            /// </summary>
            Audit = 1,

            /// <summary>
            /// The discretionary access control list (DACL).
            /// </summary>
            Access = 2,

            /// <summary>
            /// The owner.
            /// </summary>
            Owner = 4,

            /// <summary>
            /// The primary group.
            /// </summary>
            Group = 8,

            /// <summary>
            /// The entire security descriptor.
            /// </summary>
            All = Group | Owner | Access | Audit,

            /// <summary>
            /// The default value when no <see cref="AccessControlSections"/> are specified.#
            /// </summary>
            Default = Access | Owner | Group,
        }
    }
}