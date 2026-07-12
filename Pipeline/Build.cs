using Fallout.Common;
using Fallout.Common.CI.GitHubActions;
using Fallout.Common.IO;
using Fallout.Common.ProjectModel;
using Fallout.Common.Tools.GitVersion;

namespace Build;

[GitHubActions(
	"Build",
	GitHubActionsImage.UbuntuLatest,
	AutoGenerate = false,
	ImportSecrets = [nameof(GithubToken),]
)]
partial class Build : FalloutBuild
{
	[Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
	readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

	[Parameter("Github Token")] readonly string GithubToken;

	[Required] [GitVersion(Framework = "net8.0", NoCache = true, NoFetch = true)] readonly GitVersion GitVersion;

	[Solution(GenerateProjects = true)] readonly Solution Solution;

	AbsolutePath ArtifactsDirectory => RootDirectory / "Artifacts";
	AbsolutePath TestResultsDirectory => RootDirectory / "TestResults";
	GitHubActions GitHubActions => GitHubActions.Instance;

	public static int Main() => Execute<Build>(x => x.UnitTests, x => x.Pack);
}
