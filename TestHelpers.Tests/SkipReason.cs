namespace System.IO.Abstractions.TestingHelpers.Tests
{
    internal static class SkipReason
    {
        public const string NoDrivesOnUnix = "Unix does not have the concept of Drives";
        public const string NoACLsOnUnix = "Unix does not support ACLs";
        public const string NoUNCPathsOnUnix = "Unix does not have the concept of UNC paths";
        public const string WindowsOnlyPathRestrictions = "Unix does not have this path restriction";
    }
}
