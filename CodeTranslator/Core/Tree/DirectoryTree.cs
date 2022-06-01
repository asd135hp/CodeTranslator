using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeTranslator.Core.Tree
{
    /// <summary>
    /// Reason for not using NTree<T> is that Data field might be too generic
    /// for this type of tree
    /// </summary>
    public abstract class DirectoryTree<IDirectoryInfo, IReadonlyFileInfo> : NTree
    {
        protected IEnumerable<IReadonlyFileInfo> _files;
        protected IDirectoryInfo _nodeInfo;

        public IEnumerable<IReadonlyFileInfo> Files => _files;
        public IDirectoryInfo Info => _nodeInfo;

        protected DirectoryTree(DirectoryTree<IDirectoryInfo, IReadonlyFileInfo> parentDirectory)
        {
            Parent = parentDirectory;
        }

        /// <summary>
        /// From given information in DirectoryTree,
        /// generate all possible child directories and their files
        /// </summary>
        public abstract Task PopulateAll(
            string[] whiteListedExtensions = null,
            string[] blackListedExtensions = null);

        /// <summary>
        /// From given information in DirectoryTree,
        /// generate all possible child directories and their files
        /// </summary>
        /// <param name="extensions">
        /// Name of the extensions to be either whitelisted or blacklisted
        /// </param>
        /// <param name="isWhiteListed">
        /// True if the extensions should be whitelisted, false otherwise
        /// </param>
        public Task PopulateAll(bool isWhiteListed, params string[] extensions)
            => isWhiteListed ? PopulateAll(extensions, null) : PopulateAll(null, extensions);
    }
}
