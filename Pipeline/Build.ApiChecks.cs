using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable AllUnderscoreLocalParameterName

namespace Build;

partial class Build
{
	Target ApiChecks => _ => _
		.DependsOn(Compile)
		.Executes(() =>
		{
			Project project = Solution.Tests.TestableIO_System_IO_Abstractions_Api_Tests;

			DotNetTest(s => s
				.SetConfiguration(Configuration == Configuration.Debug ? "Debug" : "Release")
				.SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
				.EnableNoBuild()
				.SetResultsDirectory(TestResultsDirectory)
				.CombineWith(cc => cc
					.SetProjectFile(project)
					.AddLoggers($"trx;LogFileName={project.Name}.trx")), completeOnFailure: true);
		});
}
