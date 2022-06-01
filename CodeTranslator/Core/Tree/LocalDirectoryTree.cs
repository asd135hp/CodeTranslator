using System.Linq;
using System.IO;
using System.Threading.Tasks;
using CodeTranslator.IO;

namespace CodeTranslator.Core.Tree
{
    public sealed class LocalDirectoryTree : DirectoryTree<LocalDirectoryInfo, LocalFileInfo>
    {
        private const int MAX_DEPTH = 255;

        public LocalDirectoryTree(string rootDirectory) : this(rootDirectory, null) { }

        /// <summary>
        /// Hidden constructor for generating child directories objects in the tree recursively
        /// </summary>
        /// <param name="rootDirectory"></param>
        /// <param name="parentDirectory"></param>
        /// <param name="depth">
        /// Preventing the program from stepping too deep into sub-directories
        /// </param>
        public LocalDirectoryTree(
            string rootDirectory,
            DirectoryTree<LocalDirectoryInfo, LocalFileInfo> parentDirectory)
            : base(parentDirectory)
        {
            if (!Directory.Exists(rootDirectory))
                throw new DirectoryNotFoundException(
                    $"Could not find local directory: {rootDirectory}");

            _nodeInfo = new LocalDirectoryInfo(rootDirectory);
        }

        /// <summary>
        /// Populate directory tree on demand instead of recursively to save performance
        /// </summary>
        public override async Task PopulateAll(
            string[] whiteListedExtensions = null,
            string[] blackListedExtensions = null)
            => await Task.Run(async () => {
                // no more than 255 sub-folders deep to be registered in the tree
                if (Depth >= MAX_DEPTH) return;

                // add child directories to the enumerator recursively
                foreach (LocalDirectoryInfo childDir in await _nodeInfo.EnumerateDirectories())
                    AddChildNode(new LocalDirectoryTree(childDir.FullName, this));

                // add files to the enumerator recursively
                _files = (await _nodeInfo.EnumerateFiles())
                    .Select(fileInfo => fileInfo as LocalFileInfo);
            });
    }
}
