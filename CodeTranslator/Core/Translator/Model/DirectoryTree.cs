using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeTranslator.Core.Translator.Model
{
    public abstract class DirectoryTree
    {
        public readonly string RootDirectoryName;
        public readonly string FolderName;
        public readonly DirectoryTree ParentDirectory;
        public IEnumerable<DirectoryTree> ChildDirectories { get; protected set; }
        public IEnumerable<TranslatedCodeFile> TranslatedCodeFiles { get; protected set; }

        public DirectoryTree(string rootDirectory)
        {
            RootDirectoryName = rootDirectory;
            FolderName = new Regex("[\\/\\\\]").Split(rootDirectory).Last();
            ParentDirectory = null;
            GenerateChildDirectories();
        }

        protected DirectoryTree(string rootDirectory, DirectoryTree parentDirectory)
            : this(rootDirectory)
        {
            ParentDirectory = parentDirectory;
        }

        /// <summary>
        /// From given information in DirectoryTree,
        /// generate all possible child directories
        /// </summary>
        protected abstract void GenerateChildDirectories();
    }
}
