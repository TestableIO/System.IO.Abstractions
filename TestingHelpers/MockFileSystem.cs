using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace System.IO.Abstractions.TestingHelpers
{
    [Serializable]
    public class MockFileSystem : IFileSystem, IMockFileDataAccessor
    {
        readonly IDictionary<string, MockFileData> files;
        readonly FileBase file;
        readonly DirectoryBase directory;
        readonly IFileInfoFactory fileInfoFactory;
        readonly PathBase path;
        readonly IDirectoryInfoFactory directoryInfoFactory;
        readonly string workingDirectory;

        public MockFileSystem(IEnumerable<KeyValuePair<string, MockFileData>> files = null, string workingDirectory = @"C:\")
        {
            file = new MockFile(this);
            directory = new MockDirectory(this, file);
            fileInfoFactory = new MockFileInfoFactory(this);
            path = new MockPath(this);
            directoryInfoFactory = new MockDirectoryInfoFactory(this);
            this.workingDirectory = FixPath(workingDirectory);

            //For each mock file add a file to the files dictionary
            //Also add a file entry for all directories leading up to this file
            this.files = new Dictionary<string, MockFileData>(StringComparer.InvariantCultureIgnoreCase);
            if (files != null)
            {
                foreach (var entry in files)
                {
                    var absolutePath = Path.GetFullPath(entry.Key);

                    var directoryPath = Path.GetDirectoryName(absolutePath);
                    if (!directory.Exists(directoryPath))
                        directory.CreateDirectory(directoryPath);

                    if (!file.Exists(absolutePath))
                        this.files.Add(absolutePath, entry.Value);
                }
            }
        }

        public string WorkingDirectory 
        {
            get { return workingDirectory; }
        }

        public FileBase File
        {
            get { return file; }
        }

        public DirectoryBase Directory
        {
            get { return directory; }
        }

        public IFileInfoFactory FileInfo
        {
            get { return fileInfoFactory; }
        }

        public PathBase Path
        {
            get { return path; }
        }

        public IDirectoryInfoFactory DirectoryInfo
        {
            get { return directoryInfoFactory; }
        }

        private string FixPath(string path)
        {
            return path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
        }

        public MockFileData GetFile(string path)
        {
            path = Path.GetFullPath(path);
            return FileExists(path) ? files[path] : null;
        }

        public void AddFile(string path, MockFileData mockFile)
        {
            path = Path.GetFullPath(path);
            files.Add(path, mockFile);
        }

        public void RemoveFile(string path)
        {
            path = Path.GetFullPath(path);
            files.Remove(path);
        }

        public bool FileExists(string path)
        {
            path = Path.GetFullPath(path);
            return files.ContainsKey(path);
        }

        public IEnumerable<string> AllPaths
        {
            get { return files.Keys; }
        }

        public IEnumerable<string> AllFiles {
            get { return files.Where(f => !f.Value.IsDirectory).Select(f => f.Key); }
        }

        public IEnumerable<string> AllDirectories {
            get { return files.Where(f => f.Value.IsDirectory).Select(f => f.Key); }
        }
    }
}