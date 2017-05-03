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
    [assembly: InternalsVisibleTo("System.IO.Abstractions.TestingHelpers.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001007731b03c9a02568e3a7b1fc00d942d3a6f4395e59863322743ba965c407adba2ca90dee13bbe3767773eb4f606d1db60bb99f8a74e9b47559b6625545cef5bb76094ff7fe3c14740be52dbfda75218c487aabd97de4f52847dcaa6773ee9bb16cc16d914aa9dc3f1030c95ffb9a1cea410e883c9113edfdffe81c275dbdf48b4")]
#else
    [assembly: InternalsVisibleTo("System.IO.Abstractions.TestingHelpers.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001007731b03c9a02568e3a7b1fc00d942d3a6f4395e59863322743ba965c407adba2ca90dee13bbe3767773eb4f606d1db60bb99f8a74e9b47559b6625545cef5bb76094ff7fe3c14740be52dbfda75218c487aabd97de4f52847dcaa6773ee9bb16cc16d914aa9dc3f1030c95ffb9a1cea410e883c9113edfdffe81c275dbdf48b4")]
#endif
