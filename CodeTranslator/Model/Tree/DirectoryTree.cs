using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeTranslator.Model.Tree
{
    public abstract class DirectoryTree : NTree
    {
        public readonly string FullDirectoryName;
        public readonly string FolderName;
        public readonly IEnumerable<FileInfo> Files;

        public DirectoryInfo CurrentDirectoryInfo => new DirectoryInfo(FullDirectoryName);

        public DirectoryTree(string rootDirectoryPath)
        {
            FullDirectoryName = rootDirectoryPath;
            FolderName = new Regex("[\\/\\\\]").Split(rootDirectoryPath).Last();
            Files = EnumerateFiles(rootDirectoryPath);
            PopulateDirectories();
        }

        protected DirectoryTree(string rootDirectoryPath, DirectoryTree parentDirectory)
            : this(rootDirectoryPath)
        {
            Parent = parentDirectory;
        }

        /// <summary>
        /// From given information in DirectoryTree,
        /// generate all possible child directories and their files
        /// </summary>
        internal abstract void PopulateDirectories();

        internal abstract IEnumerable<DirectoryInfo> EnumerateDirectories(string path);

        internal abstract IEnumerable<FileInfo> EnumerateFiles(string path);
    }
}
