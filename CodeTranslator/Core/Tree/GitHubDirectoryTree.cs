using System.Linq;
using System.Threading.Tasks;
using CodeTranslator.IO;

namespace CodeTranslator.Core.Tree
{
    public sealed class GitHubDirectoryTree : DirectoryTree<GitHubDirectoryInfo, GitHubFileInfo>
    {
        private const int MAX_DEPTH = 255;
        
        public GitHubDirectoryTree(
            string repoUrl,
            string commitReference,
            string accessToken = "",
            string branch = "")
            : base(null)
        {
            _nodeInfo = new GitHubDirectoryInfo(repoUrl, commitReference, accessToken, branch);
        }

        public GitHubDirectoryTree(
            string subRepoUrl,
            string treeSha,
            DirectoryTree<GitHubDirectoryInfo, GitHubFileInfo> parentDirectory)
            : base(parentDirectory)
        {
            var castedParent = parentDirectory as GitHubDirectoryTree;
            var parentApiInfo = castedParent._nodeInfo.APIInfo.Clone() as GitHubAPIInfo;
            _nodeInfo = new GitHubDirectoryInfo(
                parentApiInfo
                    .SetGitHubUrl(subRepoUrl)
                    .SetTreeReference(treeSha));
        }

        public override async Task PopulateAll()
        {
            // no more than 255 sub-folders deep to be registered in the tree
            if (Depth >= MAX_DEPTH) return;

            // add child directories to the enumerator
            foreach (GitHubDirectoryInfo childDir in await _nodeInfo.EnumerateDirectories())
                AddChildNode(new GitHubDirectoryTree(
                    childDir.AbsoluteUrl,
                    childDir.Sha,
                    this
                ));

            // add files to the enumerator
            _files = (await _nodeInfo.EnumerateFiles())
                .Select(fileInfo => fileInfo as GitHubFileInfo);
        }
    }
}
