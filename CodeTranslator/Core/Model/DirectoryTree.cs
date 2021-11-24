using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeTranslator.Core.Model
{
    public abstract class DirectoryTree
    {
        public readonly string FullDirectoryName;
        public readonly string FolderName;
        public readonly IEnumerable<FileInfo> Files;
        public readonly DirectoryTree ParentDirectory;
        public IEnumerable<DirectoryTree> ChildDirectories { get; protected set; }

        public DirectoryTree(string rootDirectoryPath)
        {
            FullDirectoryName = rootDirectoryPath;
            FolderName = new Regex("[\\/\\\\]").Split(rootDirectoryPath).Last();
            ParentDirectory = null;
            Files = new DirectoryInfo(rootDirectoryPath).EnumerateFiles();
        }

        protected DirectoryTree(string rootDirectoryPath, DirectoryTree parentDirectory)
            : this(rootDirectoryPath)
        {
            ParentDirectory = parentDirectory;
        }

        /// <summary>
        /// From given information in DirectoryTree,
        /// generate all possible child directories and their files
        /// </summary>
        protected abstract void PopulateDirectories();

        public abstract CustomDirectoryInfo GetDirectoryInfo(string path);
    }
}
