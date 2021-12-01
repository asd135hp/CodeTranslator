using System.Collections.Generic;
using System.Threading.Tasks;

using CodeTranslator.IO;
using CodeTranslator.Tree;

namespace CodeTranslator.Core.Translator
{
    public sealed class SingleFileTree : DirectoryTree<LocalDirectoryInfo, LocalFileInfo>
    {
        private readonly string _filePath;

        public SingleFileTree(string filePath) : base(null)
        {
            _filePath = filePath;
        }

        public override async Task PopulateAll()
            => await Task.Run(() =>
            {
                _nodeInfo = null;
                _files = new List<LocalFileInfo>()
                {
                    new LocalFileInfo(_filePath)
                };
            });
    }
}
