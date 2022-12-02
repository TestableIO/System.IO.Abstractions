namespace System.IO.Abstractions
{
    /// <summary>
    ///     Support ACL functionality on file system instances.
    /// </summary>
    public interface IFileSystemAclSupport
    {
        /// <summary>
        ///     Gets a access control object that encapsulates the access control list (ACL) entries for the file or directory in the file system.
        /// </summary>
        object GetAccessControl();

        /// <summary>
        ///     Gets a access control object that encapsulates the access control list (ACL) entries for the file or directory in the file system.
        /// </summary>
        /// <param name="includeSections">One of the <see cref="AccessControlSections" /> values that specifies the type of access control list (ACL) information to receive.</param>
        object GetAccessControl(AccessControlSections includeSections);

        /// <summary>
        ///     Applies access control list (ACL) entries described by the <paramref name="value"/> object to the file or directory in the file system.
        /// </summary>
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
            All = Group | Owner | Access | Audit
        }
    }
}