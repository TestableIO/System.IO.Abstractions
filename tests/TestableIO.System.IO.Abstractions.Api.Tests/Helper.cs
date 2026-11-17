using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using System.Xml.XPath;

namespace TestableIO.System.IO.Abstractions.Api.Tests;

public static class Helper
{
    /// <summary>
    ///     Returns the build of the <paramref name="assemblyName" /> for the <paramref name="framework" />, so that the
    ///     builds of all target frameworks are verified, including those that the tests cannot run on.
    /// </summary>
    public static FileInfo GetAssemblyFile(string framework, string assemblyName)
    {
#if DEBUG
        var configuration = "Debug";
#else
        var configuration = "Release";
#endif
        return new FileInfo(
            CombinedPaths("src", assemblyName, "bin", configuration, framework, $"{assemblyName}.dll"));
    }

    public static IEnumerable<string> GetTargetFrameworks()
    {
        var csproj = CombinedPaths("src", "Directory.Build.props");
        var project = XDocument.Load(csproj);
        var targetFrameworks =
            project.XPathSelectElement("/Project/PropertyGroup/TargetFrameworks");
        foreach (var targetFramework in targetFrameworks!.Value.Split(';')) yield return targetFramework;
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