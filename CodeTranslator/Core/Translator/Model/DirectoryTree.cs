using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodeTranslator.Core.Translator.Model
{
    internal abstract class DirectoryTree
    {
        public readonly string RootDirectoryName;
        public readonly string FolderName;
        public readonly DirectoryTree ParentDirectory;
        public readonly List<DirectoryTree> ChildDirectories;

        public DirectoryTree(string rootDirectory)
        {
            RootDirectoryName = rootDirectory;
            FolderName = new Regex("[\\/]").Split(rootDirectory).Last();
            ParentDirectory = null;
            ChildDirectories = new List<DirectoryTree>();
        }

        public DirectoryTree(string rootDirectory, DirectoryTree parentDirectory)
            : this(rootDirectory)
        {
            ParentDirectory = parentDirectory;
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void GenerateChildDirectories();
    }
}
