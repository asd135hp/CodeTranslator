using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using CodeTranslator.Tree;

namespace CodeTranslator.Core.Translator
{
    public sealed class LocalDirectoryTree : DirectoryTree
    {
        private const int MAX_DEPTH = 255;
        private IEnumerable<FileInfo> _files;

        public IEnumerable<FileInfo> Files => _files;

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

        /// <summary>
        /// Populate directory tree on demand instead of recursively to save performance
        /// </summary>
        public override Task PopulateAll()
            => Task.Run(() => {
                // no more than 255 sub-folders deep to be registered in the tree
                if (Depth >= MAX_DEPTH) return;

                var dirInfo = new DirectoryInfo(FullDirectoryName);

                // add child directories to the enumerator recursively
                foreach (var childDir in dirInfo.EnumerateDirectories())
                    AddChildNode(new LocalDirectoryTree(childDir.FullName, this));

                // add files to the enumerator recursively
                _files = dirInfo.EnumerateFiles();
            });
    }
}
