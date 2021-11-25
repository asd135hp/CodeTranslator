using System.Collections.Generic;
using System.IO;

using CodeTranslator.Model;
using CodeTranslator.Model.Tree;

namespace CodeTranslator.Core.Translator.LocalDirectory
{
    public sealed class LocalDirectoryTree : DirectoryTree
    {
        private const int MAX_DEPTH = 255;

        public LocalDirectoryTree(string rootDirectoryPath) : base(rootDirectoryPath)
        {}

        /// <summary>
        /// Hidden constructor for generating child directories objects in the tree recursively
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="parentDirectory"></param>
        /// <param name="depth">
        /// Preventing the program from stepping too deep into sub-directories
        /// </param>
        private LocalDirectoryTree(string rootDirectoryPath, DirectoryTree parentDirectory)
            : base(rootDirectoryPath, parentDirectory)
        {}

        internal override void PopulateDirectories()
        {
            // no more than 255 sub-folders deep to be registered in the tree
            if (Depth >= MAX_DEPTH) return;

            // add child directories to the enumerator recursively
            foreach (var childDir in EnumerateDirectories(FullDirectoryName))
                AddChildNode(new LocalDirectoryTree(childDir.FullName, this));
        }

        internal override IEnumerable<DirectoryInfo> EnumerateDirectories(string path)
            => new DirectoryInfo(path).EnumerateDirectories();

        internal override IEnumerable<FileInfo> EnumerateFiles(string path)
            => new DirectoryInfo(path).EnumerateFiles();
    }
}
