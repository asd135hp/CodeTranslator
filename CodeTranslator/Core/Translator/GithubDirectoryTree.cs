using System.Linq;
using System.Threading.Tasks;

using CodeTranslator.IO;
using CodeTranslator.Tree;

namespace CodeTranslator.Core.Translator
{
    public sealed class GithubDirectoryTree : DirectoryTree<GithubDirectoryInfo, GithubFileInfo>
    {
        private const int MAX_DEPTH = 255;
        
        public GithubDirectoryTree(
            string repoUrl,
            string commitReference,
            string accessToken = "",
            string branch = "")
            : base(null)
        {
            _nodeInfo = new GithubDirectoryInfo(repoUrl, commitReference, accessToken, branch);
        }

        public GithubDirectoryTree(
            string subRepoUrl,
            string treeSha,
            DirectoryTree<GithubDirectoryInfo, GithubFileInfo> parentDirectory)
            : base(parentDirectory)
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

            // add child directories to the enumerator
            foreach (GithubDirectoryInfo childDir in await _nodeInfo.EnumerateDirectories())
                AddChildNode(new GithubDirectoryTree(
                    childDir.AbsoluteUrl,
                    childDir.Sha,
                    this
                ));

            // add files to the enumerator
            _files = (await _nodeInfo.EnumerateFiles())
                .Select(fileInfo => fileInfo as GithubFileInfo);
        }
    }
}
