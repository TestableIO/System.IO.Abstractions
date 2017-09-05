using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyVersion("0.0.0.1")]
[assembly: AssemblyFileVersion("0.0.0.1")]

[assembly: AssemblyTitle("System.IO.Abstractions.TestingHelpers")]
[assembly: AssemblyDescription("A set of pre-built mocks to help when testing file system interactions.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("System.IO.Abstractions")]
[assembly: AssemblyCopyright("Copyright © Tatham Oddie 2010")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

[assembly: CLSCompliant(true)]

#if DEBUG
    [assembly: InternalsVisibleTo("System.IO.Abstractions.TestingHelpers.Tests")]
#else
    [assembly: InternalsVisibleTo("System.IO.Abstractions.TestingHelpers.Tests, PublicKey=002400000480000094000000060200000024000052534131000400000100010051bf2aa00ba30d507d4cebcab1751dfa13768a6f5235ce52da572260e33a11f52b87707f858fe4bbe32cd51830a8dd73245f688902707fa797c07205ff9b5212f93760d52f6d13022a286ff7daa13a0cd9eb958e888fcd7d9ed1f7cf76b19a5391835a7b633418a5f584d10925d76810f782f6b814cc34a2326b438abdc3b5bd")]
#endif
