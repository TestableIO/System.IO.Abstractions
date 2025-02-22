using System.Collections.Generic;
using aweXpect;

namespace TestableIO.System.IO.Abstractions.Api.Tests;

/// <summary>
///     Whenever a test fails, this means that the public API surface changed.
///     If the change was intentional, execute the <see cref="ApiAcceptance.AcceptApiChanges()" /> test to take over the
///     current public API surface. The changes will become part of the pull request and will be reviewed accordingly.
/// </summary>
public sealed class ApiApprovalTests
{
    [TestCaseSource(nameof(TargetFrameworksTheoryData))]
    public async Task VerifyPublicApiForWrappers(string framework)
    {
        const string assemblyName = "TestableIO.System.IO.Abstractions.Wrappers";

        var publicApi = Helper.CreatePublicApi(framework, assemblyName);
        var expectedApi = Helper.GetExpectedApi(framework, assemblyName);

        await Expect.That(publicApi).IsEqualTo(expectedApi);
    }

    [TestCaseSource(nameof(TargetFrameworksTheoryData))]
    public async Task VerifyPublicApiForTestingHelpers(string framework)
    {
        const string assemblyName = "TestableIO.System.IO.Abstractions.TestingHelpers";

        var publicApi = Helper.CreatePublicApi(framework, assemblyName);
        var expectedApi = Helper.GetExpectedApi(framework, assemblyName);

        await Expect.That(publicApi).IsEqualTo(expectedApi);
    }

    private static IEnumerable<string> TargetFrameworksTheoryData()
    {
        List<string> theoryData = new();
        foreach (var targetFramework in Helper.GetTargetFrameworks())
        {
            theoryData.Add(targetFramework);
        }

        return theoryData;
    }
}