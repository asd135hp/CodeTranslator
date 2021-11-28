using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CodeTranslator.Model.Tree
{
    public abstract class DirectoryTree : NTree
    {
        public readonly string FullDirectoryName;
        public readonly string FolderName;

        public DirectoryInfo CurrentDirectoryInfo => new DirectoryInfo(FullDirectoryName);

        public DirectoryTree(string rootDirectoryPath)
        {
            FullDirectoryName = rootDirectoryPath;
            FolderName = new Regex("[\\/\\\\]").Split(rootDirectoryPath).Last();
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
        public abstract Task PopulateAll();
    }
}
