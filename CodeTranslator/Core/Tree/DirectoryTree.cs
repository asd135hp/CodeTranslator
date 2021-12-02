using System.Collections.Generic;
using System.Threading.Tasks;
using CodeTranslator.Model;

namespace CodeTranslator.Core.Tree
{
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
        public abstract Task PopulateAll();
    }
}
