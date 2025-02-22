using System;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.SonarScanner;
using Serilog;

namespace Build;

public static class BuildExtensions
{
	public static SonarScannerBeginSettings SetPullRequestOrBranchName(
		this SonarScannerBeginSettings settings,
		GitHubActions gitHubActions,
		GitVersion gitVersion)
	{
		if (gitHubActions?.IsPullRequest == true)
		{
			Log.Information("Use pull request analysis");
			return settings
				.SetPullRequestKey(gitHubActions.PullRequestNumber.ToString())
				.SetPullRequestBranch(gitHubActions.Ref)
				.SetPullRequestBase(gitHubActions.BaseRef);
		}

		if (gitHubActions?.Ref.StartsWith("refs/tags/", StringComparison.OrdinalIgnoreCase) == true)
		{
			string version = gitHubActions.Ref.Substring("refs/tags/".Length);
			string branchName = "release/" + version;
			Log.Information("Use release branch analysis for '{BranchName}'", branchName);
			return settings.SetBranchName(branchName);
		}

		Log.Information("Use branch analysis for '{BranchName}'", gitVersion.BranchName);
		return settings.SetBranchName(gitVersion.BranchName);
	}
}
