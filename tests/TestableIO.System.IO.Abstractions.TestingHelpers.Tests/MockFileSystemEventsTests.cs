using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using System.IO.Abstractions.TestingHelpers.Events;

namespace System.IO.Abstractions.TestingHelpers.Tests;

using XFS = MockUnixSupport;

public class MockFileSystemEventsTests
{
    [Test]
    public void Events_WhenNotEnabled_ShouldNotFireEvents()
    {
        var fileSystem = new MockFileSystem();
        var eventFired = false;
        
        // Events exist but are not enabled
        Assert.That(fileSystem.Events, Is.Not.Null);
        Assert.That(fileSystem.Events.IsEnabled, Is.False);
        
        // Subscribe should work but events won't fire
        using (fileSystem.Events.Subscribe(args => eventFired = true))
        {
            fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
            Assert.That(eventFired, Is.False);
        }
    }
    
    [Test]
    public void Events_WhenEnabled_ShouldFireEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var events = new List<FileSystemOperationEventArgs>();
        
        Assert.That(fileSystem.Events.IsEnabled, Is.True);
        
        using (fileSystem.Events.Subscribe(args => events.Add(args)))
        {
            fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
            
            Assert.That(events.Count, Is.EqualTo(4)); // Create fires Create + Open events
            Assert.That(events[0].Operation, Is.EqualTo(FileOperation.Create));
            Assert.That(events[0].Phase, Is.EqualTo(OperationPhase.Before));
            Assert.That(events[1].Operation, Is.EqualTo(FileOperation.Open));
            Assert.That(events[1].Phase, Is.EqualTo(OperationPhase.Before));
            Assert.That(events[2].Operation, Is.EqualTo(FileOperation.Open));
            Assert.That(events[2].Phase, Is.EqualTo(OperationPhase.After));
            Assert.That(events[3].Operation, Is.EqualTo(FileOperation.Create));
            Assert.That(events[3].Phase, Is.EqualTo(OperationPhase.After));
        }
    }
    
    [Test]
    public void Events_Subscribe_ToSpecificOperation_ShouldOnlyReceiveThoseEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var createEvents = new List<FileSystemOperationEventArgs>();
        var allEvents = new List<FileSystemOperationEventArgs>();
        
        using (fileSystem.Events.Subscribe(FileOperation.Create, args => createEvents.Add(args)))
        using (fileSystem.Events.Subscribe(args => allEvents.Add(args)))
        {
            fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
            fileSystem.File.WriteAllText(XFS.Path(@"C:\test.txt"), "content");
            fileSystem.File.Delete(XFS.Path(@"C:\test.txt"));
            
            // Create subscription should only get Create events
            Assert.That(createEvents.Count, Is.EqualTo(2)); // Before and After
            foreach (var e in createEvents)
            {
                Assert.That(e.Operation, Is.EqualTo(FileOperation.Create));
            }
            
            // All subscription should get all events
            Assert.That(allEvents.Count, Is.EqualTo(8)); // Create(4) + Write(2) + Delete(2)
        }
    }
    
    [Test]
    public void Events_Subscribe_ToMultipleOperations_ShouldReceiveThoseEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var modificationEvents = new List<FileSystemOperationEventArgs>();
        
        using (fileSystem.Events.Subscribe(
            new[] { FileOperation.Create, FileOperation.Write, FileOperation.Delete }, 
            args => modificationEvents.Add(args)))
        {
            fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
            fileSystem.File.WriteAllText(XFS.Path(@"C:\test.txt"), "content");
            fileSystem.File.Delete(XFS.Path(@"C:\test.txt"));
            
            Assert.That(modificationEvents.Count, Is.EqualTo(6));
            var operations = modificationEvents.Select(e => e.Operation).Distinct().ToList();
            Assert.That(operations, Contains.Item(FileOperation.Create));
            Assert.That(operations, Contains.Item(FileOperation.Write));
            Assert.That(operations, Contains.Item(FileOperation.Delete));
        }
    }
    
    [Test]
    public void Events_Unsubscribe_ShouldStopReceivingEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var events = new List<FileSystemOperationEventArgs>();
        
        var subscription = fileSystem.Events.Subscribe(args => events.Add(args));
        
        fileSystem.File.Create(XFS.Path(@"C:\test1.txt")).Dispose();
        Assert.That(events.Count, Is.EqualTo(4)); // Create fires Create + Open events
        
        subscription.Dispose();
        
        fileSystem.File.Create(XFS.Path(@"C:\test2.txt")).Dispose();
        Assert.That(events.Count, Is.EqualTo(4)); // Should still be 4
    }
    
    [Test]
    public void Events_FileCreate_ShouldFireBeforeAndAfterEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var events = new List<FileSystemOperationEventArgs>();
        
        using (fileSystem.Events.Subscribe(args => events.Add(args)))
        {
            var path = XFS.Path(@"C:\test.txt");
            fileSystem.File.Create(path).Dispose();
            
            Assert.That(events.Count, Is.EqualTo(4)); // Create fires Create + Open events
            
            // Create Before event
            Assert.That(events[0].Path, Is.EqualTo(path));
            Assert.That(events[0].Operation, Is.EqualTo(FileOperation.Create));
            Assert.That(events[0].ResourceType, Is.EqualTo(ResourceType.File));
            Assert.That(events[0].Phase, Is.EqualTo(OperationPhase.Before));
            
            // Open Before event (Create calls Open)
            Assert.That(events[1].Path, Is.EqualTo(path));
            Assert.That(events[1].Operation, Is.EqualTo(FileOperation.Open));
            Assert.That(events[1].ResourceType, Is.EqualTo(ResourceType.File));
            Assert.That(events[1].Phase, Is.EqualTo(OperationPhase.Before));
            
            // Open After event
            Assert.That(events[2].Path, Is.EqualTo(path));
            Assert.That(events[2].Operation, Is.EqualTo(FileOperation.Open));
            Assert.That(events[2].ResourceType, Is.EqualTo(ResourceType.File));
            Assert.That(events[2].Phase, Is.EqualTo(OperationPhase.After));
            
            // Create After event
            Assert.That(events[3].Path, Is.EqualTo(path));
            Assert.That(events[3].Operation, Is.EqualTo(FileOperation.Create));
            Assert.That(events[3].ResourceType, Is.EqualTo(ResourceType.File));
            Assert.That(events[3].Phase, Is.EqualTo(OperationPhase.After));
        }
    }
    
    [Test]
    public void Events_FileDelete_ShouldFireBeforeAndAfterEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        fileSystem.AddFile(XFS.Path(@"C:\test.txt"), "content");
        
        var events = new List<FileSystemOperationEventArgs>();
        using (fileSystem.Events.Subscribe(args => events.Add(args)))
        {
            var path = XFS.Path(@"C:\test.txt");
            fileSystem.File.Delete(path);
            
            Assert.That(events.Count, Is.EqualTo(2));
            
            // Before event
            Assert.That(events[0].Path, Is.EqualTo(path));
            Assert.That(events[0].Operation, Is.EqualTo(FileOperation.Delete));
            Assert.That(events[0].ResourceType, Is.EqualTo(ResourceType.File));
            Assert.That(events[0].Phase, Is.EqualTo(OperationPhase.Before));
            
            // After event
            Assert.That(events[1].Path, Is.EqualTo(path));
            Assert.That(events[1].Operation, Is.EqualTo(FileOperation.Delete));
            Assert.That(events[1].ResourceType, Is.EqualTo(ResourceType.File));
            Assert.That(events[1].Phase, Is.EqualTo(OperationPhase.After));
        }
    }
    
    [Test]
    public void Events_FileWrite_ShouldFireBeforeAndAfterEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var events = new List<FileSystemOperationEventArgs>();
        
        using (fileSystem.Events.Subscribe(args => events.Add(args)))
        {
            var path = XFS.Path(@"C:\test.txt");
            fileSystem.File.WriteAllText(path, "test content");
            
            Assert.That(events.Count, Is.EqualTo(2));
            
            // Before event
            Assert.That(events[0].Path, Is.EqualTo(path));
            Assert.That(events[0].Operation, Is.EqualTo(FileOperation.Write));
            Assert.That(events[0].ResourceType, Is.EqualTo(ResourceType.File));
            Assert.That(events[0].Phase, Is.EqualTo(OperationPhase.Before));
            
            // After event
            Assert.That(events[1].Path, Is.EqualTo(path));
            Assert.That(events[1].Operation, Is.EqualTo(FileOperation.Write));
            Assert.That(events[1].ResourceType, Is.EqualTo(ResourceType.File));
            Assert.That(events[1].Phase, Is.EqualTo(OperationPhase.After));
        }
    }
    
    [Test]
    public void Events_DirectoryCreate_ShouldFireBeforeAndAfterEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var events = new List<FileSystemOperationEventArgs>();
        
        using (fileSystem.Events.Subscribe(args => events.Add(args)))
        {
            var path = XFS.Path(@"C:\testdir");
            fileSystem.Directory.CreateDirectory(path);
            
            Assert.That(events.Count, Is.EqualTo(2));
            
            // Before event
            Assert.That(events[0].Path, Is.EqualTo(path));
            Assert.That(events[0].Operation, Is.EqualTo(FileOperation.Create));
            Assert.That(events[0].ResourceType, Is.EqualTo(ResourceType.Directory));
            Assert.That(events[0].Phase, Is.EqualTo(OperationPhase.Before));
            
            // After event
            Assert.That(events[1].Path, Is.EqualTo(path));
            Assert.That(events[1].Operation, Is.EqualTo(FileOperation.Create));
            Assert.That(events[1].ResourceType, Is.EqualTo(ResourceType.Directory));
            Assert.That(events[1].Phase, Is.EqualTo(OperationPhase.After));
        }
    }
    
    [Test]
    public void Events_DirectoryDelete_ShouldFireBeforeAndAfterEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        fileSystem.AddDirectory(XFS.Path(@"C:\testdir"));
        
        var events = new List<FileSystemOperationEventArgs>();
        using (fileSystem.Events.Subscribe(args => events.Add(args)))
        {
            var path = XFS.Path(@"C:\testdir");
            fileSystem.Directory.Delete(path);
            
            Assert.That(events.Count, Is.EqualTo(2));
            
            // Before event
            Assert.That(events[0].Path, Is.EqualTo(path));
            Assert.That(events[0].Operation, Is.EqualTo(FileOperation.Delete));
            Assert.That(events[0].ResourceType, Is.EqualTo(ResourceType.Directory));
            Assert.That(events[0].Phase, Is.EqualTo(OperationPhase.Before));
            
            // After event
            Assert.That(events[1].Path, Is.EqualTo(path));
            Assert.That(events[1].Operation, Is.EqualTo(FileOperation.Delete));
            Assert.That(events[1].ResourceType, Is.EqualTo(ResourceType.Directory));
            Assert.That(events[1].Phase, Is.EqualTo(OperationPhase.After));
        }
    }
    
    [Test]
    public void Events_CanCancelOperation_InBeforePhase()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        
        using (fileSystem.Events.Subscribe(args =>
        {
            if (args.Phase == OperationPhase.Before && args.Operation == FileOperation.Create)
            {
                args.SetResponse(new OperationResponse { Cancel = true });
            }
        }))
        {
            Assert.Throws<OperationCanceledException>(() => 
                fileSystem.File.Create(XFS.Path(@"C:\test.txt")));
            
            Assert.That(fileSystem.File.Exists(XFS.Path(@"C:\test.txt")), Is.False);
        }
    }
    
    [Test]
    public void Events_CanThrowException_InBeforePhase()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var customException = new IOException("Disk full");
        
        using (fileSystem.Events.Subscribe(args =>
        {
            if (args.Phase == OperationPhase.Before && args.Operation == FileOperation.Write)
            {
                args.SetResponse(new OperationResponse { Exception = customException });
            }
        }))
        {
            var ex = Assert.Throws<IOException>(() => 
                fileSystem.File.WriteAllText(XFS.Path(@"C:\test.txt"), "content"));
            
            Assert.That(ex.Message, Is.EqualTo("Disk full"));
            Assert.That(fileSystem.File.Exists(XFS.Path(@"C:\test.txt")), Is.False);
        }
    }
    
    [Test]
    public void Events_SetResponse_InAfterPhase_ShouldThrow()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        InvalidOperationException thrownException = null;
        
        using (fileSystem.Events.Subscribe(args =>
        {
            if (args.Phase == OperationPhase.After)
            {
                try
                {
                    args.SetResponse(new OperationResponse());
                }
                catch (InvalidOperationException ex)
                {
                    thrownException = ex;
                }
            }
        }))
        {
            fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
            
            Assert.That(thrownException, Is.Not.Null);
            Assert.That(thrownException.Message, Does.Contain("Before phase"));
        }
    }
    
    [Test]
    public void Events_TrackOperationSequence()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var operations = new List<string>();
        
        using (fileSystem.Events.Subscribe(args =>
        {
            if (args.Phase == OperationPhase.After)
            {
                operations.Add($"{args.Operation} {args.Path}");
            }
        }))
        {
            fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
            fileSystem.File.WriteAllText(XFS.Path(@"C:\test.txt"), "content");
            fileSystem.File.Delete(XFS.Path(@"C:\test.txt"));
            
            Assert.That(operations.Count, Is.EqualTo(4)); // Create fires Create + Open, plus Write and Delete
            Assert.That(operations[0], Is.EqualTo($"Open {XFS.Path(@"C:\test.txt")}")); // Create calls Open first
            Assert.That(operations[1], Is.EqualTo($"Create {XFS.Path(@"C:\test.txt")}"));
            Assert.That(operations[2], Is.EqualTo($"Write {XFS.Path(@"C:\test.txt")}"));
            Assert.That(operations[3], Is.EqualTo($"Delete {XFS.Path(@"C:\test.txt")}"));
        }
    }
    
    [Test]
    public void Events_SimulateDiskFullError_OnWrite()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        
        using (fileSystem.Events.Subscribe(FileOperation.Write, args =>
        {
            if (args.Phase == OperationPhase.Before)
            {
                args.SetResponse(new OperationResponse 
                { 
                    Exception = new IOException("There is not enough space on the disk.") 
                });
            }
        }))
        {
            // Create should work
            fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
            Assert.That(fileSystem.File.Exists(XFS.Path(@"C:\test.txt")), Is.True);
            
            // Write should fail
            var exception = Assert.Throws<IOException>(() => 
                fileSystem.File.WriteAllText(XFS.Path(@"C:\test.txt"), "content"));
            
            Assert.That(exception.Message, Is.EqualTo("There is not enough space on the disk."));
        }
    }
    
    [Test]
    public void Events_SimulateReadOnlyFileSystem()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        fileSystem.AddFile(XFS.Path(@"C:\existing.txt"), "data");
        
        using (fileSystem.Events.Subscribe(
            new[] { FileOperation.Write, FileOperation.Create, FileOperation.Delete },
            args =>
            {
                if (args.Phase == OperationPhase.Before)
                {
                    args.SetResponse(new OperationResponse 
                    { 
                        Exception = new IOException("The media is write protected.") 
                    });
                }
            }))
        {
            // All write operations should fail
            Assert.Throws<IOException>(() => fileSystem.File.Create(XFS.Path(@"C:\new.txt")));
            Assert.Throws<IOException>(() => fileSystem.File.WriteAllText(XFS.Path(@"C:\existing.txt"), "new"));
            Assert.Throws<IOException>(() => fileSystem.File.Delete(XFS.Path(@"C:\existing.txt")));
            
            // Read should still work
            var content = fileSystem.File.ReadAllText(XFS.Path(@"C:\existing.txt"));
            Assert.That(content, Is.EqualTo("data"));
        }
    }
    
    [Test]
    public void Events_SimulateFileLocking_ForSpecificFiles()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        fileSystem.AddFile(XFS.Path(@"C:\important.db"), "data");
        fileSystem.AddFile(XFS.Path(@"C:\normal.txt"), "text");
        
        using (fileSystem.Events.Subscribe(args =>
        {
            if (args.Phase == OperationPhase.Before && 
                args.Path.EndsWith(".db") && 
                args.Operation == FileOperation.Delete)
            {
                args.SetResponse(new OperationResponse 
                { 
                    Exception = new IOException("The file is in use.") 
                });
            }
        }))
        {
            // Can delete normal files
            fileSystem.File.Delete(XFS.Path(@"C:\normal.txt"));
            Assert.That(fileSystem.File.Exists(XFS.Path(@"C:\normal.txt")), Is.False);
            
            // Cannot delete .db files
            var exception = Assert.Throws<IOException>(() => 
                fileSystem.File.Delete(XFS.Path(@"C:\important.db")));
            
            Assert.That(exception.Message, Is.EqualTo("The file is in use."));
            Assert.That(fileSystem.File.Exists(XFS.Path(@"C:\important.db")), Is.True);
        }
    }
    
    [Test]
    public void Events_MultipleSubscriptions_AllReceiveEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var handler1Events = new List<FileSystemOperationEventArgs>();
        var handler2Events = new List<FileSystemOperationEventArgs>();
        
        using (fileSystem.Events.Subscribe(args => handler1Events.Add(args)))
        using (fileSystem.Events.Subscribe(args => handler2Events.Add(args)))
        {
            fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
            
            Assert.That(handler1Events.Count, Is.EqualTo(4)); // Create fires Create + Open events
            Assert.That(handler2Events.Count, Is.EqualTo(4)); // Create fires Create + Open events
        }
    }
    
    [Test]
    public void Events_ExceptionInHandler_DoesNotAffectOtherHandlers()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var handler2Called = false;
        
        using (fileSystem.Events.Subscribe(args => throw new Exception("Handler 1 error")))
        using (fileSystem.Events.Subscribe(args => handler2Called = true))
        {
            // The operation should throw the handler exception
            Assert.Throws<Exception>(() => fileSystem.File.Create(XFS.Path(@"C:\test.txt")));
            
            // But handler2 should have been called
            Assert.That(handler2Called, Is.True);
        }
    }
    
    [Test]
    public void Events_ConcurrentSubscriptions_ShouldBeThreadSafe()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var exceptions = new List<Exception>();
        var subscriptions = new List<IDisposable>();
        var eventCount = 0;
        
        const int threadCount = 10;
        const int subscriptionsPerThread = 50;
        var threads = new Thread[threadCount];
        var barrier = new Barrier(threadCount);
        
        for (int i = 0; i < threadCount; i++)
        {
            threads[i] = new Thread(() =>
            {
                try
                {
                    barrier.SignalAndWait();
                    
                    for (int j = 0; j < subscriptionsPerThread; j++)
                    {
                        var subscription = fileSystem.Events.Subscribe(args =>
                        {
                            Interlocked.Increment(ref eventCount);
                        });
                        
                        lock (subscriptions)
                        {
                            subscriptions.Add(subscription);
                        }
                        
                        Thread.Sleep(1);
                    }
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            });
        }
        
        foreach (var thread in threads)
        {
            thread.Start();
        }
        
        foreach (var thread in threads)
        {
            thread.Join();
        }
        
        Assert.That(exceptions, Is.Empty, $"Concurrent subscriptions threw exceptions: {string.Join(", ", exceptions.Select(e => e.Message))}");
        
        fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
        
        Assert.That(eventCount, Is.EqualTo(threadCount * subscriptionsPerThread * 4)); // Create fires 4 events
        
        foreach (var subscription in subscriptions)
        {
            subscription.Dispose();
        }
    }
    
    [Test]
    public void Events_ParallelOperations_ShouldFireAllEvents()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var eventCount = 0;
        var exceptions = new List<Exception>();
        
        using (fileSystem.Events.Subscribe(args => Interlocked.Increment(ref eventCount)))
        {
            const int operationCount = 100;
            var tasks = new Task[operationCount];
            
            for (int i = 0; i < operationCount; i++)
            {
                int fileIndex = i;
                tasks[i] = Task.Run(() =>
                {
                    try
                    {
                        var path = XFS.Path($@"C:\test{fileIndex}.txt");
                        fileSystem.File.Create(path).Dispose();
                        fileSystem.File.WriteAllText(path, "content");
                        fileSystem.File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        lock (exceptions)
                        {
                            exceptions.Add(ex);
                        }
                    }
                });
            }
            
            Task.WaitAll(tasks);
            
            Assert.That(exceptions, Is.Empty, $"Parallel operations threw exceptions: {string.Join(", ", exceptions.Select(e => e.Message))}");
            Assert.That(eventCount, Is.EqualTo(operationCount * 8)); // Create(4) + Write(2) + Delete(2)
        }
    }
    
    [Test]
    public void Events_SubscriptionDisposal_DuringEventFiring_ShouldNotFail()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var exceptions = new List<Exception>();
        var subscription1 = fileSystem.Events.Subscribe(args => { });
        
        var subscription2 = fileSystem.Events.Subscribe(args =>
        {
            Task.Run(() =>
            {
                try
                {
                    subscription1.Dispose();
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            });
        });
        
        try
        {
            fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose();
        }
        catch (Exception ex)
        {
            exceptions.Add(ex);
        }
        
        subscription2.Dispose();
        
        Assert.That(exceptions, Is.Empty, $"Disposal during event firing threw exceptions: {string.Join(", ", exceptions.Select(e => e.Message))}");
    }
    
    [Test]
    public void Events_MultipleHandlerExceptions_ShouldAggregateExceptions()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        
        using (fileSystem.Events.Subscribe(args => throw new InvalidOperationException("Handler 1 failed")))
        using (fileSystem.Events.Subscribe(args => throw new ArgumentException("Handler 2 failed")))
        using (fileSystem.Events.Subscribe(args => throw new IOException("Handler 3 failed")))
        {
            var ex = Assert.Throws<AggregateException>(() =>
                fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose());
            
            Assert.That(ex.InnerExceptions.Count, Is.EqualTo(3));
            Assert.That(ex.InnerExceptions[0], Is.TypeOf<InvalidOperationException>());
            Assert.That(ex.InnerExceptions[1], Is.TypeOf<ArgumentException>());
            Assert.That(ex.InnerExceptions[2], Is.TypeOf<IOException>());
        }
    }
    
    [Test]
    public void Events_SingleHandlerException_ShouldThrowDirectly()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        
        using (fileSystem.Events.Subscribe(args => throw new InvalidOperationException("Single handler failed")))
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
                fileSystem.File.Create(XFS.Path(@"C:\test.txt")).Dispose());
            
            Assert.That(ex.Message, Is.EqualTo("Single handler failed"));
        }
    }
    
    [Test]
    public void Events_ConcurrentSubscriptionAndUnsubscription_ShouldBeThreadSafe()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        var exceptions = new List<Exception>();
        var running = true;
        
        var subscribeTask = Task.Run(() =>
        {
            while (running)
            {
                try
                {
                    var subscription = fileSystem.Events.Subscribe(args => { });
                    Thread.Sleep(1);
                    subscription.Dispose();
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
        });
        
        var operationTask = Task.Run(() =>
        {
            var fileIndex = 0;
            while (running)
            {
                try
                {
                    fileSystem.File.Create(XFS.Path($@"C:\test{fileIndex++}.txt")).Dispose();
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
        });
        
        Thread.Sleep(500);
        running = false;
        
        Task.WaitAll(subscribeTask, operationTask);
        
        Assert.That(exceptions, Is.Empty, $"Concurrent subscription/operation threw exceptions: {string.Join(", ", exceptions.Select(e => e.Message))}");
    }
    
#if !NET9_0_OR_GREATER
    [Test]
    public void Events_MockFileSystemEvents_WithoutSubscriptions_ShouldBeSerializable()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        
#pragma warning disable SYSLIB0011
        var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011
        
        using var stream = new MemoryStream();
        
        Assert.DoesNotThrow(() => formatter.Serialize(stream, fileSystem.Events),
            "MockFileSystemEvents without subscriptions should be serializable");
        
        stream.Position = 0;
        
        MockFileSystemEvents deserializedEvents = null;
        Assert.DoesNotThrow(() => 
            {
                deserializedEvents = (MockFileSystemEvents)formatter.Deserialize(stream);
            },
            "MockFileSystemEvents should be deserializable");
            
        Assert.That(deserializedEvents, Is.Not.Null);
        Assert.That(deserializedEvents.IsEnabled, Is.EqualTo(fileSystem.Events.IsEnabled));
    }
#endif
    
#if !NET9_0_OR_GREATER
    [Test]
    public void Events_MockFileSystem_WithEvents_WithoutSubscriptions_ShouldBeSerializable()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        fileSystem.AddFile(XFS.Path(@"C:\test.txt"), "content");
        
#pragma warning disable SYSLIB0011
        var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011
        
        using var stream = new MemoryStream();
        
        Assert.DoesNotThrow(() => formatter.Serialize(stream, fileSystem),
            "MockFileSystem with events should be serializable");
        
        stream.Position = 0;
        
        MockFileSystem deserializedFileSystem = null;
        Assert.DoesNotThrow(() => 
            {
                deserializedFileSystem = (MockFileSystem)formatter.Deserialize(stream);
            },
            "MockFileSystem with events should be deserializable");
            
        Assert.That(deserializedFileSystem, Is.Not.Null);
        Assert.That(deserializedFileSystem.Events, Is.Not.Null);
        Assert.That(deserializedFileSystem.File.Exists(XFS.Path(@"C:\test.txt")), Is.True);
        Assert.That(deserializedFileSystem.Events.IsEnabled, Is.True);
    }
#endif
    
#if !NET9_0_OR_GREATER
    [Test]
    public void Events_FileSystemOperationEventArgs_ShouldNotBeSerializable()
    {
        var args = new FileSystemOperationEventArgs(
            XFS.Path(@"C:\test.txt"), 
            FileOperation.Create, 
            ResourceType.File, 
            OperationPhase.Before);
        
#pragma warning disable SYSLIB0011
        var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011
        
        using var stream = new MemoryStream();
        
        Assert.Throws<SerializationException>(() => formatter.Serialize(stream, args),
            "FileSystemOperationEventArgs should not be serializable by design (contains Action delegates)");
    }
    
    [Test]
    public void Events_MockFileSystemWithSubscriptions_CanBeSerializedWithActiveHandlers()
    {
        var fileSystem = new MockFileSystem(new MockFileSystemOptions { EnableEvents = true });
        
        using var subscription = fileSystem.Events.Subscribe(args => { });
        
#pragma warning disable SYSLIB0011
        var formatter = new BinaryFormatter();
#pragma warning restore SYSLIB0011
        
        using var stream = new MemoryStream();
        
        Assert.DoesNotThrow(() => formatter.Serialize(stream, fileSystem),
            "MockFileSystem with active subscriptions should be serializable with [NonSerialized] handlers");
            
        stream.Position = 0;
        MockFileSystem deserializedFileSystem = null;
        Assert.DoesNotThrow(() => 
            {
                deserializedFileSystem = (MockFileSystem)formatter.Deserialize(stream);
            },
            "MockFileSystem with events should be deserializable");
            
        Assert.That(deserializedFileSystem, Is.Not.Null);
        Assert.That(deserializedFileSystem.Events, Is.Not.Null);
        Assert.That(deserializedFileSystem.Events.IsEnabled, Is.True);
    }
#endif
}