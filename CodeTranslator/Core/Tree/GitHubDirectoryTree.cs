using System;
using System.Linq;
using System.Threading.Tasks;
using CodeTranslator.IO;
using CodeTranslator.Model;

namespace CodeTranslator.Core.Tree
{
    public sealed class GitHubDirectoryTree : DirectoryTree<GitHubDirectoryInfo, GitHubFileInfo>
    {
        private const int MAX_DEPTH = 255;
        
        public GitHubDirectoryTree(
            string productName,
            string accessToken,
            string repoUrl,
            string commitReference,
            string branch = "")
            : base(null)
        {
            _nodeInfo = new GitHubDirectoryInfo(new GitHubAPIInfo(productName, accessToken) {
                Url = new Uri(repoUrl),
                CommitSHA = commitReference,
                Branch = branch
            });
        }

        public GitHubDirectoryTree(
            string subRepoUrl,
            string treeSha,
            DirectoryTree<GitHubDirectoryInfo, GitHubFileInfo> parentDirectory)
            : base(parentDirectory)
        {
            if (parentDirectory == null)
                throw new ArgumentNullException("Parent directory must not be null");

            // set up GitHub API info for fetching and retrieving files
            var castedParent = parentDirectory as GitHubDirectoryTree;
            var parentApiInfo = castedParent._nodeInfo.APIInfo.Clone() as GitHubAPIInfo;
            parentApiInfo.Url = new Uri(subRepoUrl);
            parentApiInfo.TreeSHA = treeSha;
            _nodeInfo = new GitHubDirectoryInfo(parentApiInfo);
        }

        public override async Task PopulateAll(
            string[] whiteListedExtensions = null,
            string[] blackListedExtensions = null)
        {
            // no more than 255 sub-folders deep to be registered in the tree
            if (Depth >= MAX_DEPTH) return;

            // add child directories to the enumerator
            foreach (GitHubDirectoryInfo childDir in await _nodeInfo.EnumerateDirectories())
                AddChildNode(new GitHubDirectoryTree(
                    childDir.AbsoluteUrl,
                    childDir.APIInfo.TreeSHA,
                    this
                ));

            // add files to the enumerator
            _files = (await _nodeInfo.EnumerateFiles())
                .Select(fileInfo => fileInfo as GitHubFileInfo);
        }
    }
}
