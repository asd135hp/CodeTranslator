using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CodeTranslator.Github;
using CodeTranslator.Tree;

namespace CodeTranslator.Core.Translator
{
    public sealed class GithubDirectoryTree : DirectoryTree
    {
        private const int MAX_DEPTH = 255;
        private IEnumerable<GithubFileInfo> _files;
        private readonly GithubDirectoryInfo _nodeInfo;

        public IEnumerable<GithubFileInfo> Files => _files;
        public GithubDirectoryInfo Info => _nodeInfo;
        
        public GithubDirectoryTree(
            string repoUrl,
            string commitReference,
            string accessToken = "",
            string branch = "")
            : base(repoUrl)
        {
            _nodeInfo = new GithubDirectoryInfo(repoUrl, commitReference, accessToken, branch);
        }

        private GithubDirectoryTree(
            string subRepoUrl,
            string treeSha,
            DirectoryTree parentDirectory)
            : base(subRepoUrl, parentDirectory)
        {
            var castedParent = parentDirectory as GithubDirectoryTree;
            var parentApiInfo = castedParent._nodeInfo.APIInfo.Clone() as GithubAPIInfo;
            _nodeInfo = new GithubDirectoryInfo(
                parentApiInfo
                    .SetGithubUrl(subRepoUrl)
                    .SetTreeReference(treeSha));
        }

        public override async Task PopulateAll()
        {
            // no more than 255 sub-folders deep to be registered in the tree
            if (Depth >= MAX_DEPTH) return;

            // add child directories to the enumerator recursively
            foreach (var childDir in await _nodeInfo.EnumerateDirectories())
                AddChildNode(new GithubDirectoryTree(
                    childDir.AbsoluteUrl,
                    childDir.Sha,
                    this
                ));

            // add files to the enumerator recursively
            _files = await _nodeInfo.EnumerateFiles();
        }
    }
}
