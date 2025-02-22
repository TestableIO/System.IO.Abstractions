using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.NUnit;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

// ReSharper disable AllUnderscoreLocalParameterName

namespace Build;

partial class Build
{
    Target UnitTests => _ => _
        .DependsOn(DotNetFrameworkUnitTests)
        .DependsOn(DotNetUnitTests);

    Project[] UnitTestProjects =>
    [
        Solution.Tests.TestableIO_System_IO_Abstractions_Wrappers_Tests,
        Solution.Tests.TestableIO_System_IO_Abstractions_TestingHelpers_Tests,
        Solution.Tests.TestableIO_System_IO_Abstractions_Parity_Tests,
    ];

    Target DotNetFrameworkUnitTests => _ => _
        .Unlisted()
        .DependsOn(Compile)
        .OnlyWhenDynamic(() => EnvironmentInfo.IsWin)
        .Executes(() =>
        {
            var testAssemblies = UnitTestProjects
                .SelectMany(project =>
                    project.Directory.GlobFiles(
                        $"bin/{(Configuration == Configuration.Debug ? "Debug" : "Release")}/net472/*.Tests.dll"))
                .Select(p => p.ToString())
                .ToArray();

            Assert.NotEmpty(testAssemblies.ToList());
            
            foreach (var testAssembly in testAssemblies)
            {
                var currentDirectory = Path.GetDirectoryName(testAssembly);

                NUnitTasks.NUnit3(s => s
                    .SetInputFiles(testAssembly)
                    .SetProcessWorkingDirectory(currentDirectory)
                );
            }
        });

    Target DotNetUnitTests => _ => _
        .Unlisted()
        .DependsOn(Compile)
        .Executes(() =>
        {
            string[] excludedFrameworks = ["net48",];
            DotNetTest(s => s
                    .SetConfiguration(Configuration)
                    .SetProcessEnvironmentVariable("DOTNET_CLI_UI_LANGUAGE", "en-US")
                    .EnableNoBuild()
                    .SetDataCollector("XPlat Code Coverage")
                    .SetResultsDirectory(TestResultsDirectory)
                    .CombineWith(
                        UnitTestProjects,
                        (settings, project) => settings
                            .SetProjectFile(project)
                            .CombineWith(
                                project.GetTargetFrameworks()?.Except(excludedFrameworks),
                                (frameworkSettings, framework) => frameworkSettings
                                    .SetFramework(framework)
                                    .AddLoggers($"trx;LogFileName={project.Name}_{framework}.trx")
                            )
                    ), completeOnFailure: true
            );
        });
}