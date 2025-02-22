using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.ReportGenerator;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

// ReSharper disable AllUnderscoreLocalParameterName

namespace Build;

partial class Build
{
	Target CodeCoverage => _ => _
		.DependsOn(UnitTests)
		.Executes(() =>
		{
			ReportGenerator(s => s
				.SetProcessToolPath(NuGetToolPathResolver.GetPackageExecutable("ReportGenerator", "ReportGenerator.dll",
					framework: "net8.0"))
				.SetTargetDirectory(TestResultsDirectory / "reports")
				.AddReports(TestResultsDirectory / "**/coverage.cobertura.xml")
				.AddReportTypes(ReportTypes.OpenCover)
				.AddFileFilters("-*.g.cs")
				.SetAssemblyFilters("+TestableIO*"));
		});
}
