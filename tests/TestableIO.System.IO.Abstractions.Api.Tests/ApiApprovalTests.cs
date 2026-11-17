using System.Collections.Generic;
using System.Linq;
using aweXpect.Core;
using aweXpect.Snapshots;
using aweXpect.Snapshots.Results;
// ReSharper disable ExplicitCallerInfoArgument

namespace TestableIO.System.IO.Abstractions.Api.Tests;

/// <summary>
///     Whenever a test fails, this means that the public API surface changed.
///     If the change was intentional, execute <see cref="AcceptApiChanges" />, rename the pending snapshot next to this
///     file or run the tests with the environment variable <c>AWEXPECT_ACCEPT_SNAPSHOTS</c> set to <c>all</c> to take
///     over the current public API surface. The changes will become part of the pull request and will be reviewed
///     accordingly.
/// </summary>
public sealed class ApiApprovalTests
{
    private static readonly string[] AssemblyNames =
    [
        "TestableIO.System.IO.Abstractions.Wrappers",
        "TestableIO.System.IO.Abstractions.TestingHelpers",
    ];

    [TestCaseSource(nameof(Builds))]
    public async Task VerifyPublicApi(string assemblyName, string framework)
        => await MatchesPublicApiSnapshot(assemblyName, framework);

    /// <summary>
    ///     Execute this test to update the expected public API to the current API surface.
    /// </summary>
    [Test]
    [Explicit]
    public async Task AcceptApiChanges()
    {
        foreach (var build in Builds())
        {
            try
            {
                await MatchesPublicApiSnapshot((string)build[0], (string)build[1])
                    .Accepting(SnapshotAcceptance.All);
            }
            catch (Exception exception) when (exception.Message.Contains("Accepted snapshot"))
            {
                // An accepted snapshot still fails the expectation.
            }
        }
    }

    private static IEnumerable<object[]> Builds()
        => AssemblyNames.SelectMany(assemblyName => Helper.GetTargetFrameworks()
            .Select(framework => new object[] { assemblyName, framework, }));

    /// <summary>
    ///     The snapshot is named after the assembly and the target framework, so that both tests use the same one.
    /// </summary>
    private static StringSnapshotResult<string, IThat<string>> MatchesPublicApiSnapshot(string assemblyName,
        string framework)
        => That(Helper.GetAssemblyFile(framework, assemblyName))
            .MatchesPublicApiSnapshot(callerMemberName: assemblyName).Named(framework);
}
