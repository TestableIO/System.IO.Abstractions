﻿using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using aweXpect;
using static aweXpect.Expect;
using NUnit.Framework;
using static System.Reflection.BindingFlags;

namespace System.IO.Abstractions.Tests;

[TestFixture]
public class ApiParityTests
{
    [Test]
    public async Task File() =>
        await AssertParity(
            typeof(File),
            typeof(FileBase)
        );

    [Test]
    public async Task FileInfo() =>
        await AssertParity(
            typeof(FileInfo),
            typeof(FileInfoBase)
        );

    [Test]
    public async Task FileVersionInfo() =>
        await AssertParity(
            typeof(Diagnostics.FileVersionInfo),
            typeof(FileVersionInfoBase)
        );

    [Test]
    public async Task Directory() =>
        await AssertParity(
            typeof(Directory),
            typeof(DirectoryBase)
        );

    [Test]
    public async Task DirectoryInfo() =>
        await AssertParity(
            typeof(DirectoryInfo),
            typeof(DirectoryInfoBase)
        );

    [Test]
    public async Task DriveInfo() =>
        await AssertParity(
            typeof(DriveInfo),
            typeof(DriveInfoBase)
        );

    [Test]
    public async Task Path() =>
        await AssertParity(
            typeof(Path),
            typeof(PathBase)
        );

    [Test]
    public async Task FileSystemWatcher() =>
        await AssertParity(
            typeof(FileSystemWatcher),
            typeof(FileSystemWatcherBase)
        );

    private async Task AssertParity(Type referenceType, Type abstractionType)
    {
        static IEnumerable<string> GetMembers(Type type) => type
            .GetMembers(bindingAttr: Instance | Static | Public | FlattenHierarchy)
            .Select(x => x.ToString())
            .OrderBy(x => x, StringComparer.Ordinal);
        var referenceMembers = GetMembers(referenceType)
            .Select(x => x.Replace("System.IO.FileStream", "System.IO.Abstractions.FileSystemStream"))
            .Select(x => x.Replace("System.IO.Abstractions.FileSystemStreamOptions", "System.IO.FileStreamOptions"))
            .Select(x => x.Replace("System.IO.FileSystemInfo", "System.IO.Abstractions.IFileSystemInfo"))
            .Select(x => x.Replace("System.IO.FileInfo", "System.IO.Abstractions.IFileInfo"))
            .Select(x => x.Replace("System.IO.DirectoryInfo", "System.IO.Abstractions.IDirectoryInfo"))
            .Select(x => x.Replace("System.IO.DriveInfo", "System.IO.Abstractions.IDriveInfo"))
            .Select(x => x.Replace("System.IO.WaitForChangedResult", "System.IO.Abstractions.IWaitForChangedResult"))
            .Where(x => x != "System.Diagnostics.FileVersionInfo GetVersionInfo(System.String)");
        var abstractionMembers = GetMembers(abstractionType)
            .Where(x => !x.Contains("op_Implicit"))
            .Where(x => x != "System.IO.Abstractions.IFileSystem get_FileSystem()")
            .Where(x => x != "System.IO.Abstractions.IFileSystem FileSystem");
        var diff = new ApiDiff(
            extraMembers: abstractionMembers.Except(referenceMembers),
            missingMembers: referenceMembers.Except(abstractionMembers)
        );

        var serializedDiff = JsonSerializer.Serialize(diff, SerializerOptions);

        var snapshotPath = IO.Path.GetFullPath("../../../__snapshots__/");
        var fileName = $"ApiParityTests.{referenceType.Name}_{snapshotSuffix}.snap";
        var fileContent = IO.File.ReadAllText(IO.Path.Combine(snapshotPath, fileName));

        await That(fileContent).IsEqualTo(serializedDiff)
            .IgnoringNewlineStyle()
            .IgnoringTrailingWhiteSpace();
    }

    private static JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };
        
    private readonly struct ApiDiff
    {
        public ApiDiff(IEnumerable<string> extraMembers, IEnumerable<string> missingMembers)
        {
            ExtraMembers = extraMembers.ToArray();
            MissingMembers = missingMembers.ToArray();

        }

        public string[] ExtraMembers { get; }
        public string[] MissingMembers { get; }
    }

#if NET472
        private const string snapshotSuffix = ".NET Framework 4.7.2";
#elif NET6_0
        private const string snapshotSuffix = ".NET 6.0";
#elif NET8_0
        private const string snapshotSuffix = ".NET 8.0";
#elif NET9_0
    private const string snapshotSuffix = ".NET 9.0";
#else
#error Unknown target framework.
#endif
}