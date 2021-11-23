using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeTranslator.Core.Translator.Model
{
    public abstract class DirectoryTree
    {
        public readonly string DirectoryName;
        public readonly string FolderName;
        public readonly DirectoryTree ParentDirectory;
        public IEnumerable<DirectoryTree> ChildDirectories { get; protected set; }
        public IEnumerable<TranslatedCodeFile> TranslatedCodeFiles { get; protected set; }

        public DirectoryTree(string rootDirectory)
        {
            DirectoryName = rootDirectory;
            FolderName = new Regex("[\\/\\\\]").Split(rootDirectory).Last();
            ParentDirectory = null;
        }

        protected DirectoryTree(string rootDirectory, DirectoryTree parentDirectory)
            : this(rootDirectory)
        {
            ParentDirectory = parentDirectory;
        }

        /// <summary>
        /// From given information in DirectoryTree,
        /// generate all possible child directories and their files
        /// </summary>
        protected abstract void PopulateFilesAndDirectories();
    }
}
