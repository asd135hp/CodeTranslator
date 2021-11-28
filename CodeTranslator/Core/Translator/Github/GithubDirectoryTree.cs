using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CodeTranslator.Model.Github;
using CodeTranslator.Model.Tree;

namespace CodeTranslator.Core.Translator.Github
{
    public sealed class GithubDirectoryTree : DirectoryTree
    {
        private const int MAX_DEPTH = 255;
        private IEnumerable<GithubFileInfo> _files;
        private readonly GithubDirectoryInfo _nodeInfo;

        public IEnumerable<GithubFileInfo> Files => _files;
        public GithubDirectoryInfo Info => _nodeInfo;
        
        public GithubDirectoryTree(string rootDirectoryPath) : base(rootDirectoryPath)
        {
        }

        private GithubDirectoryTree(string rootDirectoryPath, DirectoryTree parentDirectory)
            : base(rootDirectoryPath, parentDirectory)
        {
        }

        //??
        public override async Task PopulateAll()
        {
            // no more than 255 sub-folders deep to be registered in the tree
            if (Depth >= MAX_DEPTH) return;

            // add child directories to the enumerator recursively
            foreach (var childDir in await _nodeInfo.EnumerateDirectories())
                AddChildNode(new GithubDirectoryTree(
                    "",
                    this
                ));

            // add files to the enumerator recursively
            _files = await _nodeInfo.EnumerateFiles();
        }
    }
}
