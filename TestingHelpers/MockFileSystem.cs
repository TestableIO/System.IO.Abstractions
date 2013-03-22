using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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

        public MockFileSystem(IDictionary<string, MockFileData> files)
        {
            file = new MockFile(this);
            directory = new MockDirectory(this, file);
            fileInfoFactory = new MockFileInfoFactory(this);
            path = new MockPath();
            directoryInfoFactory = new MockDirectoryInfoFactory(this);

            //For each mock file add a file to the files dictionary
            //Also add a file entry for all directories leading up to this file
            this.files = new Dictionary<string, MockFileData>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var entry in files)
            {
                this.files.Add(entry.Key, entry.Value);

                var remainingPath = entry.Key;
                do
                {
                    remainingPath = remainingPath.Substring(0, remainingPath.LastIndexOf(IO.Path.DirectorySeparatorChar));

                    //Don't add the volume as a directory
                    if (remainingPath.EndsWith(Path.VolumeSeparatorChar.ToString(CultureInfo.InvariantCulture)))
                        break;

                    //Don't create duplicate directories
                    if (!this.files.ContainsKey(remainingPath))
                        AddFile(remainingPath, new MockDirectoryData());

                } while (remainingPath.LastIndexOf(IO.Path.DirectorySeparatorChar) != -1);
            }
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

        public MockFileData GetFile(string path)
        {
            return FileExists(path) ? files[path] : null;
        }

        public void AddFile(string path, MockFileData mockFile)
        {
            files.Add(path, mockFile);
        }

        public void RemoveFile(string path)
        {
            files.Remove(path);
        }

        public bool FileExists(string path)
        {
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