namespace TestableIO.System.IO.Abstractions.Api.Tests;

public sealed class ApiAcceptance
{
	/// <summary>
	///     Execute this test to update the expected public API to the current API surface.
	/// </summary>
	[TestCase]
	[Explicit]
	public async Task AcceptApiChanges()
	{
		string[] assemblyNames =
		[
            "TestableIO.System.IO.Abstractions.Wrappers",
            "TestableIO.System.IO.Abstractions.TestingHelpers",
		];

		foreach (string assemblyName in assemblyNames)
		{
			foreach (string framework in Helper.GetTargetFrameworks())
			{
				string publicApi = Helper.CreatePublicApi(framework, assemblyName)
					.Replace("\n", Environment.NewLine);
				Helper.SetExpectedApi(framework, assemblyName, publicApi);
			}
		}

        await That(assemblyNames).IsNotEmpty();
	}
}
