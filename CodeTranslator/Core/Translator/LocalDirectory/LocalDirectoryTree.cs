using System.IO;
using System.Collections.Generic;

using CodeTranslator.Core.Model;

namespace CodeTranslator.Core.Translator.LocalDirectory
{
    public class LocalDirectoryTree : DirectoryTree
    {
        private readonly int _depth;
        private const int MAX_DEPTH = 255;

        public LocalDirectoryTree(string rootDirectoryPath) : base(rootDirectoryPath)
        {
            _depth = 0;
            PopulateDirectories();
        }

        /// <summary>
        /// Hidden constructor for generating child directories objects in the tree recursively
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="parentDirectory"></param>
        /// <param name="depth">
        /// Preventing the program from stepping too deep into sub-directories
        /// </param>
        private LocalDirectoryTree(string rootDirectoryPath, DirectoryTree parentDirectory, int depth)
            : base(rootDirectoryPath, parentDirectory)
        {
            _depth = depth;
            PopulateDirectories();
        }

        protected override void PopulateDirectories()
        {
            // no more than 255 sub-folders to be registered in the tree
            if (_depth >= MAX_DEPTH) return;

            var childDirs = new List<DirectoryTree>();

            // add child directories to the enumerator recursively
            foreach (var childDir in GetDirectoryInfo(FullDirectoryName).Directories)
                childDirs.Add(new LocalDirectoryTree(childDir.FullName, this, _depth + 1));

            ChildDirectories = childDirs;
        }

        public override CustomDirectoryInfo GetDirectoryInfo(string path)
            => new CustomDirectoryInfo(new DirectoryInfo(path));
    }
}
