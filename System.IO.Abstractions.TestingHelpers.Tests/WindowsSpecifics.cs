namespace System.IO.Abstractions.TestingHelpers.Tests
{
    internal static class WindowsSpecifics
    {
        public const string Drives = "Drives are a Windows-only concept";

        public const string AccessControlLists = "ACLs are a Windows-only concept";

        public const string UNCPaths = "UNC paths are a Windows-only concept";
        
        public const string StrictPathRules = "Windows has stricter path rules than other platforms";
    }
}
