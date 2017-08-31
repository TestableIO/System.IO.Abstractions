namespace System.IO.Abstractions.TestingHelpers
{
    using System.IO.Abstractions;

    // The underlying FileVersionInfo class is immutable which means that our
    // abstraction should be immutable as well.  If we use an abstract class
    // to model the basic structure, we have to define properties as
    // get-only:
    //   public abstract int FileMajorVersion { get; }
    //
    // Because these are abstract definitions it becomes messy to implement the
    // Mock class that allows these fields to be publicly settable.  We either
    // have to insert a Writeable class in between the abstract base and  the
    // concrete Mock, solely to shadow the get-only properties of the base and
    // allow get-set versions in the mock.  Or have a set of SetX methods that
    // provide mutation of the property backers.
    //
    // Modelling the abstraction using an interface would alleviate this,
    // however it would break the existing semantics of implicit conversion
    // between System.Diagnostics.FileVersionInfo and
    // System.IO.Abstractions.FileVersionInfoBase.
    //
    // As libraries should hide the ugly stuff from clients, it is better to
    // have this mutable adapter class that shadows the get-only properties and
    // allows MockFileVersionInfo to look like it simply adds setters to the
    // abstract base.
    [Serializable]
    public abstract class MockFileVersionInfoMutableAdapter : FileVersionInfoBase
    {
        public abstract bool IsPrivateBuild_ { get; }

        public sealed override bool IsPrivateBuild
        {
            get { return IsPrivateBuild_; }
        }

        public abstract int ProductPrivatePart_ { get; }

        public sealed override int ProductPrivatePart
        {
            get { return ProductPrivatePart_; }
        }

        public abstract string ProductName_ { get; }

        public sealed override string ProductName
        {
            get { return ProductName_; }
        }

        public abstract int ProductMinorPart_ { get; }

        public sealed override int ProductMinorPart
        {
            get { return ProductMinorPart_; }
        }

        public abstract int ProductMajorPart_ { get; }

        public sealed override int ProductMajorPart
        {
            get { return ProductMajorPart_; }
        }

        public abstract int ProductBuildPart_ { get; }

        public sealed override int ProductBuildPart
        {
            get { return ProductBuildPart_; }
        }

        public abstract string PrivateBuild_ { get; }

        public sealed override string PrivateBuild
        {
            get { return PrivateBuild_; }
        }

        public abstract string OriginalFilename_ { get; }

        public sealed override string OriginalFilename
        {
            get { return OriginalFilename_; }
        }

        public abstract string LegalTrademarks_ { get; }

        public sealed override string LegalTrademarks
        {
            get { return LegalTrademarks_; }
        }

        public abstract string LegalCopyright_ { get; }

        public sealed override string LegalCopyright
        {
            get { return LegalCopyright_; }
        }

        public abstract string Language_ { get; }

        public sealed override string Language
        {
            get { return Language_; }
        }

        public abstract bool IsSpecialBuild_ { get; }

        public sealed override bool IsSpecialBuild
        {
            get { return IsSpecialBuild_; }
        }

        public abstract bool IsPreRelease_ { get; }

        public sealed override bool IsPreRelease
        {
            get { return IsPreRelease_; }
        }

        public abstract string ProductVersion_ { get; }

        public sealed override string ProductVersion
        {
            get { return ProductVersion_; }
        }

        public abstract string SpecialBuild_ { get; }

        public sealed override string SpecialBuild
        {
            get { return SpecialBuild_; }
        }

        public abstract bool IsDebug_ { get; }

        public sealed override bool IsDebug
        {
            get { return IsDebug_; }
        }

        public abstract string InternalName_ { get; }

        public sealed override string InternalName
        {
            get { return InternalName_; }
        }

        public abstract int FilePrivatePart_ { get; }

        public sealed override int FilePrivatePart
        {
            get { return FilePrivatePart_; }
        }

        public abstract string FileName_ { get; }

        public sealed override string FileName
        {
            get { return FileName_; }
        }

        public abstract int FileMinorPart_ { get; }

        public sealed override int FileMinorPart
        {
            get { return FileMinorPart_; }
        }

        public abstract int FileMajorPart_ { get; }

        public sealed override int FileMajorPart
        {
            get { return FileMajorPart_; }
        }

        public abstract string FileDescription_ { get; }

        public sealed override string FileDescription
        {
            get { return FileDescription_; }
        }

        public abstract int FileBuildPart_ { get; }

        public sealed override int FileBuildPart
        {
            get { return FileBuildPart_; }
        }

        public abstract string CompanyName_ { get; }

        public sealed override string CompanyName
        {
            get { return CompanyName_; }
        }

        public abstract string Comments_ { get; }

        public sealed override string Comments
        {
            get { return Comments_; }
        }

        public abstract bool IsPatched_ { get; }

        public override sealed bool IsPatched
        {
            get { return IsPatched_; }
        }
    }
}
