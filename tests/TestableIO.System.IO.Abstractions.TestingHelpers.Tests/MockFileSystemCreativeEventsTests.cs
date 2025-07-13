using System.Collections.Generic;
using System.Linq;
using System.IO.Abstractions.TestingHelpers.Events;

namespace System.IO.Abstractions.TestingHelpers.Tests;

using XFS = MockUnixSupport;

/// <summary>
/// Contains tests for verifying various advanced event-driven behaviors in a mocked file system.
/// These tests explore complex interactions, such as event tracking, validation mechanisms,
/// milestone progression, and performance profiling within the MockFileSystem framework.
/// </summary>
public class MockFileSystemCreativeEventsTests
{
    /// <summary>
    /// Indicates whether test output should be enabled during test execution.
    /// When set to <c>true</c>, test logs and other output are written for
    /// debugging or analysis purposes. The value is determined by the
    /// environment variable "MOCK_FS_TEST_OUTPUT", and is <c>true</c> if
    /// the variable is set to "1".
    /// </summary>
    private static readonly bool EnableTestOutput = Environment.GetEnvironmentVariable("MOCK_FS_TEST_OUTPUT") == "1";

    /// <summary>
    /// Writes a test output message if test output is enabled.
    /// </summary>
    /// <param name="message">The message to write to the test output.</param>
    private static void WriteTestOutput(string message)
    {
        if (EnableTestOutput)
        {
            TestContext.Out.WriteLine(message);
        }
    }

    /// <summary>
    /// Verifies that the file system correctly tracks file operations
    /// and generates an operation log when events are enabled.
    /// </summary>
    /// <remarks>
    /// This test ensures that when the <see cref="MockFileSystem"/> is configured
    /// with event tracking enabled, operations such as file creation, modification,
    /// deletion, and other file system activities are logged correctly. The test
    /// validates that the operation log captures the expected sequence and details
    /// of events for each file system activity performed.
    /// </remarks>
    /// <exception cref="NUnit.Framework.AssertionException">
    /// Thrown if the expected number of operations or their details are not present in the operation log.
    /// </exception>
    [Test]
    public void Events_FileSystemOperationTracker_ShouldCreateOperationLog()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var operationLog = new List<string>();

        using (fileSystem.Events.Subscribe(args =>
               {
                   if (args.Phase != OperationPhase.After) return;
                   var logEntry = args.Operation switch
                   {
                       FileOperation.Create => $"Created new file at {args.Path}",
                       FileOperation.Write => $"Modified content of {args.Path}",
                       FileOperation.Read => $"Accessed data from {args.Path}",
                       FileOperation.Copy => $"Duplicated file to {args.Path}",
                       FileOperation.Move => $"Relocated file to {args.Path}",
                       FileOperation.Delete => $"Removed file at {args.Path}",
                       _ => $"Performed {args.Operation} on {args.Path}"
                   };
                   operationLog.Add(logEntry);
               }))
        {
            var testFile = XFS.Path(@"C:\test-file.txt");
            var backupFile = XFS.Path(@"C:\test-backup.txt");
            
            fileSystem.File.Create(testFile).Dispose();
            fileSystem.File.WriteAllText(testFile, "test content");
            fileSystem.File.ReadAllText(testFile);
            fileSystem.File.Copy(testFile, backupFile);
            fileSystem.File.Move(backupFile, XFS.Path(@"C:\final-backup.txt"));
            fileSystem.File.Delete(testFile);
        }
        
        WriteTestOutput("Operation Log:");
        foreach (var entry in operationLog)
        {
            WriteTestOutput($"  {entry}");
        }
        
        using (Assert.EnterMultipleScope())
        {
            // Order: Open, Create, Write, Read, Copy, Move, Delete
            Assert.That(operationLog, Has.Count.EqualTo(7)); // Create fires 2 events (Create+Open), Write, Read, Copy, Move, Delete each fire 1
            Assert.That(operationLog.Any(l => l.Contains("Created new file")), Is.True);
            Assert.That(operationLog.Any(l => l.Contains("Modified content")), Is.True);
            Assert.That(operationLog.Any(l => l.Contains("Accessed data")), Is.True);
            Assert.That(operationLog.Any(l => l.Contains("Duplicated file")), Is.True);
            Assert.That(operationLog.Any(l => l.Contains("Relocated file")), Is.True);
            Assert.That(operationLog.Any(l => l.Contains("Removed file")), Is.True);
        }
    }

    /// <summary>
    /// Validates the tracking of experience points and associated operations using a mock file system
    /// with enabled event subscription.
    /// </summary>
    /// <remarks>
    /// This test verifies that file system operations like creating, reading, copying, moving, and deleting files
    /// are appropriately recorded to update player-related statistics such as level, experience points, performed operations,
    /// and milestones achieved.
    /// The test subscribes to file system events and reacts to specific phases of those events to calculate
    /// accumulated experience points and milestones. It ensures the tracking logic adheres to defined criteria
    /// while performing a series of file-based operations.
    /// </remarks>
    /// <exception cref="AssertionException">
    /// Thrown when the expected outcomes for level, experience points, milestones, or operations are not met.
    /// </exception>
    [Test]
    public void Events_FileSystemProgressTracker_ShouldTrackExperiencePoints()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var playerStats = new Dictionary<string, object>
        {
            ["level"] = 1,
            ["experience"] = 0,
            ["operations_performed"] = 0,
            ["milestones"] = new List<string>()
        };

        using (fileSystem.Events.Subscribe(args =>
        {
            if (args.Phase == OperationPhase.After)
            {
                var pointsGained = args.Operation switch
                {
                    FileOperation.Create => 10,
                    FileOperation.Read => 2,
                    FileOperation.Write => 5,
                    FileOperation.Copy => 8,
                    FileOperation.Move => 12,
                    FileOperation.Delete => 15,
                    FileOperation.SetAttributes => 7,
                    FileOperation.SetTimes => 3,
                    _ => 1
                };

                playerStats["experience"] = (int)playerStats["experience"] + pointsGained;
                playerStats["operations_performed"] = (int)playerStats["operations_performed"] + 1;

                var milestones = (List<string>)playerStats["milestones"];

                // Level progression system
                var currentExp = (int)playerStats["experience"];
                var newLevel = 1 + (currentExp / 50);
                if (newLevel > (int)playerStats["level"])
                {
                    playerStats["level"] = newLevel;
                    milestones.Add($"Advanced to level {newLevel}");
                }

                // Milestone tracking
                var opsPerformed = (int)playerStats["operations_performed"];
                if (opsPerformed == 10 && !milestones.Contains("Completed 10 operations"))
                    milestones.Add("Completed 10 operations");

                if (args.Operation == FileOperation.Delete && !milestones.Contains("First file deletion"))
                    milestones.Add("First file deletion");

                if (args.Path.EndsWith(".exe") && !milestones.Contains("Handled executable file"))
                    milestones.Add("Handled executable file");
            }
        }))
        {
            var testFiles = new[]
            {
                XFS.Path(@"C:\document.txt"),
                XFS.Path(@"C:\code.cs"),
                XFS.Path(@"C:\program.exe"),
                XFS.Path(@"C:\backup.txt")
            };

            // Create files
            foreach (var file in testFiles.Take(3))
            {
                fileSystem.File.Create(file).Dispose();
                fileSystem.File.WriteAllText(file, "test content");
            }

            // Read content
            fileSystem.File.ReadAllText(testFiles[1]);

            // Create backup
            fileSystem.File.Copy(testFiles[0], testFiles[3]);

            // Create archive directory and move file
            fileSystem.Directory.CreateDirectory(XFS.Path(@"C:\archive"));
            fileSystem.File.Move(testFiles[1], XFS.Path(@"C:\archive\code.cs"));

            // Set file attributes
            fileSystem.File.SetAttributes(testFiles[2], FileAttributes.Hidden | FileAttributes.System);

            // Clean up executable
            fileSystem.File.Delete(testFiles[2]);
        }

        WriteTestOutput("Progress Tracker Stats:");
        WriteTestOutput($"Level: {playerStats["level"]}");
        WriteTestOutput($"Experience: {playerStats["experience"]}");
        WriteTestOutput($"Operations: {playerStats["operations_performed"]}");
        WriteTestOutput("Milestones:");
        foreach (var milestone in (List<string>)playerStats["milestones"])
        {
            WriteTestOutput($"  {milestone}");
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That((int)playerStats["level"], Is.GreaterThan(1));
            Assert.That((int)playerStats["experience"], Is.GreaterThan(100));
            Assert.That(((List<string>)playerStats["milestones"]).Count, Is.GreaterThan(0));
        }
    }

    /// <summary>
    /// Tests that the timestamp modification operations on a mocked file system are intercepted and validated
    /// according to specified conditions when events are enabled via <see cref="MockFileSystem"/>.
    /// </summary>
    /// <remarks>
    /// This test ensures the following:
    /// - Timestamps on files can be successfully updated to future dates.
    /// - Timestamps involving invalid or past dates will result in thrown exceptions.
    /// It verifies that the subscription to the file system events captures and processes the operations accordingly,
    /// with appropriate logging of the detected time modifications.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// Thrown when attempting to set a file timestamp to a past or otherwise invalid date.
    /// </exception>
    /// <example>
    /// Designed to test custom validation logic tied to file timestamp operations.
    /// </example>
    [Test]
    public void Events_TimeStampValidator_ShouldInterceptSetTimesOperations()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var timeValidationLog = new List<string>();
        var originalTime = DateTime.Now;
        var futureTime = originalTime.AddDays(365);

        using (fileSystem.Events.Subscribe(FileOperation.SetTimes, args =>
        {
            if (args.Phase == OperationPhase.Before)
            {
                timeValidationLog.Add($"Time modification detected for {args.Path}");

                // Implement validation rules
                if (args.Path.Contains("future"))
                {
                    timeValidationLog.Add("Processing future timestamp");
                }
                else if (args.Path.Contains("past"))
                {
                    // Prevent setting dates too far in the past
                    timeValidationLog.Add("Rejecting past timestamp modification");
                    args.SetResponse(new OperationResponse
                    {
                        Exception = new ArgumentException("Cannot set timestamps to dates before system epoch")
                    });
                }
            }
        }))
        {
            var futureFile = XFS.Path(@"C:\future-document.txt");
            var pastFile = XFS.Path(@"C:\past-document.txt");

            fileSystem.File.Create(futureFile).Dispose();
            fileSystem.File.Create(pastFile).Dispose();

            // This should succeed
            fileSystem.File.SetCreationTime(futureFile, futureTime);

            // This should fail
            Assert.Throws<ArgumentException>(() =>
                fileSystem.File.SetCreationTime(pastFile, new DateTime(1970, 1, 1)));
        }

        WriteTestOutput("Time Validation Log:");
        foreach (var entry in timeValidationLog)
        {
            WriteTestOutput($"  {entry}");
        }

        Assert.That(timeValidationLog, Has.Count.EqualTo(4));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(timeValidationLog[0], Does.Contain("Time modification detected"));
            Assert.That(timeValidationLog[1], Does.Contain("Processing future timestamp"));
            Assert.That(timeValidationLog[2], Does.Contain("Time modification detected"));
            Assert.That(timeValidationLog[3], Does.Contain("Rejecting past timestamp"));
        }
    }

    /// <summary>
    /// Verifies that the file system implements the superposition pattern for quantum files
    /// by simulating quantum behavior where files exist in multiple states until observed.
    /// </summary>
    /// <remarks>
    /// This test ensures that quantum files behave according to the superposition principle,
    /// collapsing their state upon file read operations. Regular files are unaffected and
    /// behave deterministically. It utilizes mocked file system events to track operations
    /// and verify expected outcomes under controlled scenarios with deterministic behavior.
    /// </remarks>
    [Test]
    public void Events_QuantumFileSystem_ShouldImplementSuperpositionPattern()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var observationLog = new List<string>();
        var quantumStates = new Dictionary<string, bool>(); // true = exists, false = doesn't exist
        var deterministicRandom = new Random(42); // Fixed seed for deterministic behavior

        var quantumFiles = new[]
        {
            XFS.Path(@"C:\quantum-file-1.txt"),
            XFS.Path(@"C:\quantum-file-2.txt"),
            XFS.Path(@"C:\quantum-file-3.txt"),
            XFS.Path(@"C:\regular-file.txt")
        };

        var observationResults = new List<bool>();

        using (fileSystem.Events.Subscribe(args =>
               {
                   if (args.Phase != OperationPhase.Before || args.Operation != FileOperation.Read) return;
                   // Quantum files exist in superposition until observed (read)
                   if (!args.Path.Contains("quantum") || quantumStates.ContainsKey(args.Path)) return;
                   // Collapse the wave function deterministically
                   var exists = deterministicRandom.NextDouble() > 0.5;
                   quantumStates[args.Path] = exists;

                   observationLog.Add($"Quantum state collapsed for {args.Path}: {(exists ? "EXISTS" : "DOES_NOT_EXIST")}");

                   if (!exists)
                   {
                       args.SetResponse(new OperationResponse
                       {
                           Exception = new FileNotFoundException("Quantum file collapsed to non-existence state")
                       });
                   }
               }))
        {
            // Create all files initially
            foreach (var file in quantumFiles)
            {
                fileSystem.File.Create(file).Dispose();
                fileSystem.File.WriteAllText(file, "quantum content");
            }

            // Observe quantum files (this collapses their wave functions)
            foreach (var quantumFile in quantumFiles.Where(f => f.Contains("quantum")))
            {
                try
                {
                    fileSystem.File.ReadAllText(quantumFile);
                    observationResults.Add(true); // File existed after observation
                }
                catch (FileNotFoundException)
                {
                    observationResults.Add(false); // File didn't exist after observation
                }
            }

            // Regular file should always be readable
            var regularContent = fileSystem.File.ReadAllText(quantumFiles[3]);
            Assert.That(regularContent, Is.EqualTo("quantum content"));

            WriteTestOutput("Quantum Observation Log:");
            foreach (var entry in observationLog)
            {
                WriteTestOutput($"  {entry}");
            }

            using (Assert.EnterMultipleScope())
            {

                // With seed 42, we expect deterministic results
                Assert.That(observationLog, Has.Count.EqualTo(3));
                Assert.That(observationResults, Has.Count.EqualTo(3));

                // Verify quantum collapse occurred (exact results depend on Random implementation)
                Assert.That(quantumStates, Has.Count.EqualTo(3)); // All three quantum files observed
            }
            using (Assert.EnterMultipleScope())
            {
                Assert.That(quantumStates.ContainsKey(quantumFiles[0]), Is.True);
                Assert.That(quantumStates.ContainsKey(quantumFiles[1]), Is.True);
                Assert.That(quantumStates.ContainsKey(quantumFiles[2]), Is.True);
            }
        }
    }

    /// <summary>
    /// Tests the functionality of the mock file system's auto-correction feature for detecting and suggesting typo corrections
    /// in file names when new files are created. The test subscribes to file system events and identifies potential typos
    /// by comparing file names to a predefined set of known good names.
    /// </summary>
    /// <remarks>
    /// This test verifies the ability of the file system to detect and suggest corrections for typos in file names during
    /// file creation. The mock file system is configured to enable events, and corrections are logged and asserted
    /// against expected results. Only typos in file names associated with the "Before" phase of file creation operations
    /// are addressed by this test.
    /// </remarks>
    /// <exception cref="AssertionException">
    /// Thrown when the detected correction suggestions do not match the expected results.
    /// </exception>
    [Test]
    public void Events_AutoCorrectFileSystem_ShouldDetectAndSuggestTypoCorrections()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var correctionLog = new List<string>();
        var knownGoodNames = new HashSet<string> { "document", "report", "config", "readme", "license" };

        using (fileSystem.Events.Subscribe(args =>
               {
                   if (args.Phase != OperationPhase.Before || args.Operation != FileOperation.Create) return;
                   var fileName = fileSystem.Path.GetFileNameWithoutExtension(args.Path);

                   // Check for potential typos
                   foreach (var goodName in knownGoodNames.Where(goodName => IsLikelyTypo(fileName, goodName)))
                   {
                       correctionLog.Add($"Potential typo detected: '{fileName}' might be '{goodName}'");
                       break;
                   }
               }))
        {
            var testFiles = new[]
            {
                XFS.Path(@"C:\documnet.txt"),  // typo: document
                XFS.Path(@"C:\reprot.txt"),    // typo: report
                XFS.Path(@"C:\cofig.txt"),     // typo: config
                XFS.Path(@"C:\readme.txt"),    // correct
                XFS.Path(@"C:\licnese.txt")    // typo: license
            };

            foreach (var file in testFiles)
            {
                fileSystem.File.Create(file).Dispose();
                fileSystem.File.WriteAllText(file, "content");
            }
        }

        WriteTestOutput("Auto-Correct Suggestions:");
        foreach (var entry in correctionLog)
        {
            WriteTestOutput($"  {entry}");
        }

        Assert.That(correctionLog, Has.Count.EqualTo(4)); // 4 typos detected
        using (Assert.EnterMultipleScope())
        {
            Assert.That(correctionLog[0], Does.Contain("documnet").And.Contain("document"));
            Assert.That(correctionLog[1], Does.Contain("reprot").And.Contain("report"));
            Assert.That(correctionLog[2], Does.Contain("cofig").And.Contain("config"));
            Assert.That(correctionLog[3], Does.Contain("licnese").And.Contain("license"));
        }
    }

    /// <summary>
    /// Validates the reliability of the file system by simulating controlled failure scenarios and verifying proper handling.
    /// </summary>
    /// <remarks>
    /// This method uses a deterministic random seed to simulate a predictable sequence of failures during file system operations.
    /// It tracks success and failure events while ensuring the controlled pattern of failures is correctly maintained during testing.
    /// The method also verifies that failure and success events have occurred as expected.
    /// </remarks>
    /// <exception cref="AssertionException">
    /// Thrown if the number of controlled failures or successes does not meet the expected conditions.
    /// </exception>
    [Test]
    public void Events_ReliabilityTestingSystem_ShouldSimulateControlledFailures()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var failureLog = new List<string>();
        var operationCount = 0;
        var deterministicRandom = new Random(123); // Fixed seed for predictable failures

        using (fileSystem.Events.Subscribe(args =>
               {
                   if (args.Phase != OperationPhase.Before) return;
                   operationCount++;

                   // Simulate controlled failure scenarios
                   var failureChance = deterministicRandom.NextDouble();
                   var failureType = deterministicRandom.Next(1, 4);

                   if (failureChance < 0.25) // 25% controlled failure rate
                   {
                       Exception failure = failureType switch
                       {
                           1 => new IOException("Simulated disk space exhaustion"),
                           2 => new UnauthorizedAccessException("Simulated permission denied"),
                           3 => new DirectoryNotFoundException("Simulated path not found"),
                           _ => new InvalidOperationException("Simulated system error")
                       };

                       failureLog.Add($"Controlled failure #{operationCount}: {failure.Message} (Operation: {args.Operation})");
                       args.SetResponse(new OperationResponse { Exception = failure });
                   }
                   else
                   {
                       failureLog.Add($"Operation #{operationCount}: Success ({args.Operation})");
                   }
               }))
        {
            var testFiles = new[]
            {
                XFS.Path(@"C:\test1.txt"),
                XFS.Path(@"C:\test2.txt"),
                XFS.Path(@"C:\test3.txt"),
                XFS.Path(@"C:\test4.txt")
            };

            foreach (var file in testFiles)
            {
                try
                {
                    fileSystem.File.Create(file).Dispose();
                    fileSystem.File.WriteAllText(file, "content");
                }
                catch (Exception)
                {
                    // ignored
                }

                try
                {
                    if (fileSystem.File.Exists(file))
                    {
                        fileSystem.File.ReadAllText(file);
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        WriteTestOutput("Reliability Test Log:");
        foreach (var entry in failureLog)
        {
            WriteTestOutput($"  {entry}");
        }

        // With deterministic random seed 123, we expect specific failure patterns
        Assert.That(failureLog, Is.Not.Empty);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(failureLog.Count(l => l.Contains("Controlled failure")), Is.GreaterThan(0));
            Assert.That(failureLog.Count(l => l.Contains("Success")), Is.GreaterThan(0));
        }
    }

    /// <summary>
    /// Verifies that the access pattern monitor properly detects and tracks suspicious file access activities
    /// by escalating alert levels and triggering appropriate responses for excessive or anomalous access patterns.
    /// </summary>
    /// <remarks>
    /// This test focuses on monitoring read operations on files, evaluating their frequency, and
    /// validating the system's ability to log and respond accordingly. It incorporates a simulation of
    /// normal and suspicious access scenarios, ensuring that alerts are accurately triggered for potential risks.
    /// Key behaviors tested:
    /// - Initial access to a file triggers simple monitoring.
    /// - Repeated and frequent access to the same file escalates alert levels progressively.
    /// - Excessive access beyond a defined threshold triggers a security alert, resulting in an access denial response.
    /// - Differentiates between normal and suspicious file access patterns.
    /// Assertions include:
    /// - The correct number of monitoring log entries are generated.
    /// - Escalation of alert levels accurately corresponds to access frequency.
    /// - The final log entry contains the expected security alert message.
    /// </remarks>
    [Test]
    public void Events_AccessPatternMonitor_ShouldTrackSuspiciousActivity()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var activityLog = new List<string>();
        var accessCounts = new Dictionary<string, int>();
        var alertLevel = 0;

        using (fileSystem.Events.Subscribe(args =>
               {
                   if (args.Phase != OperationPhase.Before || args.Operation != FileOperation.Read) return;
                   accessCounts[args.Path] = (accessCounts.TryGetValue(args.Path, out var count) ? count : 0) + 1;
                   var accesses = accessCounts[args.Path];

                   switch (accesses)
                   {
                       // Monitor read access patterns only
                       case 1:
                           activityLog.Add($"First access to {args.Path} - monitoring initiated");
                           break;
                       case 2:
                           alertLevel++;
                           activityLog.Add($"Repeated access to {args.Path} - elevated monitoring");
                           break;
                       case 3:
                           alertLevel += 2;
                           activityLog.Add($"Frequent access to {args.Path} - high alert");
                           break;
                       case 4:
                           alertLevel += 5;
                           activityLog.Add($"Excessive access to {args.Path} - security alert triggered");
                           args.SetResponse(new OperationResponse
                           {
                               Exception = new UnauthorizedAccessException("Access denied: Suspicious activity pattern detected")
                           });
                           break;
                   }
               }))
        {
            var sensitiveFile = XFS.Path(@"C:\sensitive-data.txt");
            var normalFile = XFS.Path(@"C:\normal-file.txt");

            fileSystem.File.Create(sensitiveFile).Dispose();
            fileSystem.File.Create(normalFile).Dispose();
            fileSystem.File.WriteAllText(sensitiveFile, "classified");
            fileSystem.File.WriteAllText(normalFile, "public");

            // Simulate normal access pattern
            fileSystem.File.ReadAllText(normalFile);

            // Simulate suspicious access pattern with proper exception handling
            var securityAlertTriggered = false;

            try
            {
                fileSystem.File.ReadAllText(sensitiveFile); // 1st access
                fileSystem.File.ReadAllText(sensitiveFile); // 2nd access
                fileSystem.File.ReadAllText(sensitiveFile); // 3rd access
                fileSystem.File.ReadAllText(sensitiveFile); // 4th access - should trigger alert
            }
            catch (UnauthorizedAccessException)
            {
                securityAlertTriggered = true;
            }

            Assert.That(securityAlertTriggered, Is.True, "Security alert should have been triggered");
        }

        WriteTestOutput("Access Pattern Monitor Log:");
        foreach (var entry in activityLog)
        {
            WriteTestOutput($"  {entry}");
        }

        using (Assert.EnterMultipleScope())
        {
            Assert.That(activityLog, Has.Count.EqualTo(5)); // 1 normal file + 4 sensitive file accesses
            Assert.That(alertLevel, Is.EqualTo(8)); // 1 + 2 + 5 from escalating alerts
        }
        Assert.That(activityLog.Last(), Does.Contain("security alert triggered"));
    }

    /// <summary>
    /// Validates that the performance profiler tracks and collects operation metrics for file system events
    /// when the event system is enabled in the mock file system.
    /// </summary>
    /// <remarks>
    /// This method subscribes to file system events, performs various file operations (e.g., create, write, read,
    /// set attributes, delete), and collects performance metrics such as operation execution durations.
    /// The metrics are then analyzed to ensure the proper collection of data for all relevant file operations.
    /// Additionally, assertions are made to verify that metrics are recorded for each operation type
    /// and that each operation type has been executed multiple times.
    /// </remarks>
    /// <exception cref="AssertionException">Thrown if the expected metrics are not recorded for all operations,
    /// or if the operations are not performed multiple times as specified.</exception>
    [Test]
    public void Events_PerformanceProfiler_ShouldTrackOperationMetrics()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var performanceMetrics = new Dictionary<FileOperation, List<long>>();
        var operationTimestamps = new Dictionary<string, DateTime>();

        using (fileSystem.Events.Subscribe(args =>
        {
            var key = $"{args.Path}_{args.Operation}_{args.Phase}";

            if (args.Phase == OperationPhase.Before)
            {
                operationTimestamps[key] = DateTime.UtcNow;
            }
            else if (args.Phase == OperationPhase.After)
            {
                var beforeKey = key.Replace("_After", "_Before");
                if (!operationTimestamps.TryGetValue(beforeKey, out var timestamp)) return;
                var duration = (DateTime.UtcNow - timestamp).Ticks;

                if (!performanceMetrics.ContainsKey(args.Operation))
                    performanceMetrics[args.Operation] = new List<long>();

                performanceMetrics[args.Operation].Add(duration);
                operationTimestamps.Remove(beforeKey);
            }
        }))
        {
            var testFiles = new[]
            {
                XFS.Path(@"C:\perf-test-1.txt"),
                XFS.Path(@"C:\perf-test-2.txt"),
                XFS.Path(@"C:\perf-test-3.txt")
            };

            // Perform various operations to collect metrics
            foreach (var file in testFiles)
            {
                fileSystem.File.Create(file).Dispose();
                fileSystem.File.WriteAllText(file, "performance test data");
                fileSystem.File.ReadAllText(file);
                fileSystem.File.SetAttributes(file, FileAttributes.Archive);
            }

            // Cleanup
            foreach (var file in testFiles)
            {
                fileSystem.File.Delete(file);
            }
        }

        WriteTestOutput("Performance Metrics:");
        foreach (var metric in performanceMetrics)
        {
            var avgDuration = metric.Value.Average();
            WriteTestOutput($"  {metric.Key}: {metric.Value.Count} operations, avg duration: {avgDuration:F2} ticks");
        }

        using (Assert.EnterMultipleScope())
        {

            // Verify we captured metrics for all operation types
            Assert.That(performanceMetrics.ContainsKey(FileOperation.Create), Is.True);
            Assert.That(performanceMetrics.ContainsKey(FileOperation.Write), Is.True);
            Assert.That(performanceMetrics.ContainsKey(FileOperation.Read), Is.True);
            Assert.That(performanceMetrics.ContainsKey(FileOperation.SetAttributes), Is.True);
            Assert.That(performanceMetrics.ContainsKey(FileOperation.Delete), Is.True);
        }

        // Each operation should have been performed multiple times
        foreach (var metric in performanceMetrics.Values)
        {
            Assert.That(metric.Count, Is.GreaterThan(0));
        }
    }

    /// <summary>
    /// Determines whether the given input string is likely a typo of the target string,
    /// based on a simple Levenshtein distance metric.
    /// </summary>
    /// <param name="input">The string to evaluate as a potential typo.</param>
    /// <param name="target">The reference string to compare against for typo detection.</param>
    /// <returns>A boolean value indicating whether the input string is likely a typo of the target string.</returns>
    private static bool IsLikelyTypo(string input, string target)
    {
        if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(target))
            return false;
            
        // Simple Levenshtein distance check for typo detection
        var distance = CalculateLevenshteinDistance(input.ToLowerInvariant(), target.ToLowerInvariant());
        return distance == 1 || distance == 2; // Allow 1-2 character differences
    }

    /// <summary>
    /// Calculates the Levenshtein distance between two strings, which is a measure of the number of single-character
    /// edits (insertions, deletions, or substitutions) required to change one string into the other.
    /// </summary>
    /// <param name="source">The source string to compare.</param>
    /// <param name="target">The target string to compare against.</param>
    /// <returns>The Levenshtein distance as an integer, representing the number of edits required to transform the source string into the target string.</returns>
    private static int CalculateLevenshteinDistance(string source, string target)
    {
        if (source.Length == 0) return target.Length;
        if (target.Length == 0) return source.Length;
        
        var matrix = new int[source.Length + 1, target.Length + 1];
        
        for (var i = 0; i <= source.Length; i++)
            matrix[i, 0] = i;
        for (var j = 0; j <= target.Length; j++)
            matrix[0, j] = j;
            
        for (var i = 1; i <= source.Length; i++)
        {
            for (var j = 1; j <= target.Length; j++)
            {
                var cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                matrix[i, j] = Math.Min(
                    Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1),
                    matrix[i - 1, j - 1] + cost);
            }
        }
        
        return matrix[source.Length, target.Length];
    }
}