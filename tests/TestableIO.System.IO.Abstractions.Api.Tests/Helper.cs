using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.XPath;
using PublicApiGenerator;

namespace TestableIO.System.IO.Abstractions.Api.Tests;

public static class Helper
{
    public static string CreatePublicApi(string framework, string assemblyName)
    {
#if DEBUG
        var configuration = "Debug";
#else
		string configuration = "Release";
#endif
        var assemblyFile =
            CombinedPaths("src", assemblyName, "bin", configuration, framework, $"{assemblyName}.dll");
        var assembly = Assembly.LoadFile(assemblyFile);
        var publicApi = assembly.GeneratePublicApi(new ApiGeneratorOptions
        {
            AllowNamespacePrefixes = ["System.IO.Abstractions",],
        });
        return publicApi.Replace("\r\n", "\n");
    }

    public static string GetExpectedApi(string framework, string assemblyName)
    {
        var expectedPath = CombinedPaths("tests", "TestableIO.System.IO.Abstractions.Api.Tests",
            "Expected", $"{assemblyName}_{framework}.txt");
        try
        {
            return File.ReadAllText(expectedPath)
                .Replace("\r\n", "\n");
        }
        catch
        {
            return string.Empty;
        }
    }

    public static IEnumerable<string> GetTargetFrameworks()
    {
        var csproj = CombinedPaths("src", "Directory.Build.props");
        var project = XDocument.Load(csproj);
        var targetFrameworks =
            project.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");
        foreach (var targetFramework in targetFrameworks!.Value.Split(';')) yield return targetFramework;
    }

    public static void SetExpectedApi(string framework, string assemblyName, string publicApi)
    {
        var expectedPath = CombinedPaths("tests", "TestableIO.System.IO.Abstractions.Api.Tests",
            "Expected", $"{assemblyName}_{framework}.txt");
        Directory.CreateDirectory(Path.GetDirectoryName(expectedPath)!);
        File.WriteAllText(expectedPath, publicApi);
    }

    private static string CombinedPaths(params string[] paths)
    {
        return Path.GetFullPath(Path.Combine(paths.Prepend(GetSolutionDirectory()).ToArray()));
    }

    private static string GetSolutionDirectory([CallerFilePath] string path = "")
    {
        return Path.Combine(Path.GetDirectoryName(path)!, "..", "..");
    }
}