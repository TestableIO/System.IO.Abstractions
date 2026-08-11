using Fallout.Common;
using Fallout.Common.Tooling;
using Fallout.Common.Tools.ReportGenerator;
using static Fallout.Common.Tools.ReportGenerator.ReportGeneratorTasks;

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
				.SetAssemblyFilters("+TestableIO*", "+System.IO.Abstractions*"));
		});
}
