# System.IO.Abstractions Development Guide

System.IO.Abstractions is a .NET library that provides testable abstractions for System.IO operations, enabling developers to write unit tests that don't rely on the actual file system.

**ALWAYS reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Working Effectively

### Bootstrap and Build Process
- **CRITICAL**: Install .NET SDK 9.0.304 (required version specified in global.json):
  ```bash
  curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.304
  export PATH="$HOME/.dotnet:$PATH"
  ```
- Verify installation: `dotnet --version` should return `9.0.304`
- **Build the solution**: `dotnet build` -- takes ~70 seconds. NEVER CANCEL. Set timeout to 120+ minutes.
- **Run all tests**: `dotnet test --configuration Release` -- takes ~30 seconds. NEVER CANCEL. Set timeout to 60+ minutes.

### Code Quality and Formatting
- **ALWAYS run code formatting before committing**: `dotnet format` 
- **Verify formatting compliance**: `dotnet format --verify-no-changes` -- takes ~40 seconds. NEVER CANCEL.
- The codebase uses EditorConfig with CRLF line endings and 4-space indentation for C# files
- **CRITICAL**: All formatting issues must be resolved before CI will pass

### Build System Details
- **Primary build method**: `dotnet build` and `dotnet test` commands work reliably
- **NUKE build script**: `./build.sh` available but may have GitVersion issues with shallow clones
- **Available NUKE targets**: UnitTests, ApiChecks, CodeCoverage, CodeAnalysis, Pack
- **Build artifacts**: Generated in `Artifacts/` and `TestResults/` directories

## Project Structure

### Key Projects
- **System.IO.Abstractions**: Core abstractions and interfaces
- **TestableIO.System.IO.Abstractions**: Main implementation
- **TestableIO.System.IO.Abstractions.Wrappers**: Wrapper implementations
- **TestableIO.System.IO.Abstractions.TestingHelpers**: Mock implementations for testing
- **Multiple test projects**: Comprehensive test coverage across different scenarios

### Target Frameworks
- .NET Framework 4.7.2 (net472)
- .NET Standard 2.0, 2.1 (netstandard2.0, netstandard2.1)  
- .NET 6.0, 8.0, 9.0 (net6.0, net8.0, net9.0)

### Important Directories
- `src/`: All source code projects
- `tests/`: All test projects including unit tests, API tests, and parity tests
- `benchmarks/`: Performance benchmarking projects
- `Pipeline/`: NUKE build system configuration
- `.github/workflows/`: CI/CD pipeline definitions

## Validation

### Manual Validation Steps
After making changes, ALWAYS validate functionality by:

1. **Build verification**: `dotnet build` must succeed
2. **Test execution**: `dotnet test --configuration Release` must pass
3. **Code formatting**: `dotnet format --verify-no-changes` must pass
4. **Functional validation**: Create a test scenario to verify your changes work:

```csharp
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;

// Test MockFileSystem functionality
var mockFileSystem = new MockFileSystem();
mockFileSystem.File.WriteAllText(@"C:\test.txt", "Test content");
var content = mockFileSystem.File.ReadAllText(@"C:\test.txt");

// Test real FileSystem functionality  
var realFileSystem = new FileSystem();
var tempFile = "/tmp/test.txt";
realFileSystem.File.WriteAllText(tempFile, "Real test");
var realContent = realFileSystem.File.ReadAllText(tempFile);
realFileSystem.File.Delete(tempFile);
```

### Test Suite Information
- **Total tests**: ~2,234 tests across all projects
- **Expected passing**: ~1,993 tests (some platform-specific tests are skipped on Linux)
- **Test categories**: Unit tests, API compatibility tests, parity tests
- **Platform considerations**: Some Windows-specific functionality is skipped on Linux/macOS

### Continuous Integration Requirements
The CI pipeline (.github/workflows/ci.yml) requires:
- All unit tests to pass on Ubuntu, Windows, and macOS
- API compatibility checks to succeed
- Code formatting compliance
- Static code analysis (SonarCloud) to pass

## Common Development Tasks

### Adding New Functionality
- Implement abstractions in the main System.IO.Abstractions project
- Add wrapper implementations in TestableIO.System.IO.Abstractions.Wrappers
- Create mock implementations in TestableIO.System.IO.Abstractions.TestingHelpers
- Add comprehensive tests covering all target frameworks
- Update XML documentation for all public APIs

### Debugging Build Issues
- Check .NET SDK version compatibility first
- Verify all package references are properly restored
- For NUKE build issues, use direct `dotnet` commands instead
- Review build logs in Artifacts/ directory for detailed error information

### Package Management
- Uses Central Package Management (Directory.Packages.props)
- NuGet source: nuget.org only (configured in nuget.config)
- Package versioning uses Nerdbank.GitVersioning

## Pull Request Guidelines

### Pull Request Title
To communicate intent to the consumers of your library, the title of the pull requests is prefixed with one of the following elements:
- `fix:`: patches a bug
- `feat:`: introduces a new feature
- `refactor:`: improves internal structure without changing the observable behavior
- `docs:`: updates documentation or XML comments
- `chore:`: updates to dependencies, build pipelines, ...

## Performance Expectations

### Command Timing (with appropriate timeouts)
- **dotnet build**: ~70 seconds (set timeout: 120+ minutes)
- **dotnet test**: ~30 seconds (set timeout: 60+ minutes) 
- **dotnet format**: ~40 seconds (set timeout: 10+ minutes)
- **NUKE build restore**: ~30 seconds (set timeout: 10+ minutes)

### **NEVER CANCEL** long-running operations
Builds and tests may occasionally take longer than expected. Always wait for completion rather than canceling operations.

## Troubleshooting

### Common Issues
- **GitVersion errors**: Use direct `dotnet` commands instead of NUKE build script
- **Shallow clone issues**: Expected in GitHub Actions environment, doesn't affect functionality
- **Platform-specific test failures**: Normal for Windows-specific functionality on Linux/macOS
- **Code formatting failures**: Run `dotnet format` to fix automatically

### SDK Installation Issues
If .NET SDK 9.0.304 is not available:
- Check global.json for exact version requirement
- Use dotnet-install script with specific version
- Ensure PATH includes the installed SDK location

**Remember**: This is a mature, well-tested library with extensive CI/CD. Focus on maintaining compatibility across all target frameworks and ensuring comprehensive test coverage for any changes.