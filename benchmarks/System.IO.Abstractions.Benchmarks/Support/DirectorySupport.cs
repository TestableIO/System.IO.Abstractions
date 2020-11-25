using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO.Abstractions.Benchmarks.Support
{
    public class DirectorySupport
    {
        #region Members
        private IFileSystem _fileSystem;
        #endregion

        #region CTOR's
        public DirectorySupport(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public DirectorySupport() : this(new FileSystem())
        {
            // Default implementation for FileSystem
        }
        #endregion

        #region Methods
        public bool IsDirectory(string path)
        {
            return _fileSystem.Directory.Exists(path);
        }

        private string GetRandomTempDirectory()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        public string CreateRandomDirectory()
        {
            var randomPath = this.GetRandomTempDirectory();
            _fileSystem.Directory.CreateDirectory(randomPath);
            return randomPath;
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs = true, bool overwrite = true)
        {
            // Get the subdirectories for the specified directory.
            var dir = _fileSystem.DirectoryInfo.FromDirectoryName(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            var dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!_fileSystem.Directory.Exists(destDirName))
            {
                _fileSystem.Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles();
            foreach (var file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, overwrite);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (var subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public void CreateIfNotExists(string directory)
        {
            if (!_fileSystem.Directory.Exists(directory))
            {
                _fileSystem.Directory.CreateDirectory(directory);
            }
        }

        public bool Exists(string directory)
        {
            return _fileSystem.Directory.Exists(directory);
        }
        #endregion
    }
}
