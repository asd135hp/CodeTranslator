using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public sealed class GithubDirectoryInfo : GithubTreeItem
    {
        private IEnumerable<TreeItem> _cache;
        private readonly string _name;

        public GithubDirectoryInfo Parent { get; private set; }

        public override string Name => _name;
        public override string Extension => "";

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GithubDirectoryInfo(
            string githubUrl,
            string commitReference,
            string accessToken)
            : this(new Uri(githubUrl), commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GithubDirectoryInfo(
            Uri githubUrl,
            string commitReference,
            string accessToken)
            : this(githubUrl, commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GithubDirectoryInfo(
            string githubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new Uri(githubUrl), commitReference, accessToken, branch) { }

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GithubDirectoryInfo(
            Uri githubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new GithubAPIInfo()
                  .SetAccessToken(accessToken)
                  .SetGithubUrl(githubUrl)
                  .SetCommit(commitReference)
                  .SetBranch(branch)) { }

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="apiInfo"></param>
        public GithubDirectoryInfo(GithubAPIInfo apiInfo) : base(apiInfo)
        {
            if (Exists) _name = apiInfo.Url.Segments.Last().Trim().Trim('/', '\\');
            _cache = null;
        }

        /// <summary>
        /// Async-based method for enumerating directories by fetching data with Github API
        /// </summary>
        /// <returns></returns>
        public async Task<GithubDirectoryInfo[]> GetDirectories()
            => (await EnumerateDirectories()).ToArray();

        /// <summary>
        /// Async-based method for enumerating directories by fetching data with Github API
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GithubDirectoryInfo>> EnumerateDirectories()
        {
            var list = new List<GithubDirectoryInfo>();
            _cache ??= (await FetchGitHubTree())?.Tree;

            if(_cache != null)
                foreach(var treeItem in _cache)
                    if (treeItem.Type == TreeType.Tree)
                    {
                        var url = APIInfo.GetChildUrl(AbsolutePath, treeItem.Path);
                        var apiInfo = (APIInfo.Clone() as GithubAPIInfo)
                            .SetGithubUrl(url)
                            .SetTreeReference(treeItem.Sha);

                        list.Add(new GithubDirectoryInfo(apiInfo)
                        {
                            Parent = this
                        });
                    }

            return list;
        }

        /// <summary>
        /// Async-based method for enumerating files by fetching data with Github API
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GithubFileInfo>> GetFiles()
            => (await EnumerateFiles()).ToArray();

        /// <summary>
        /// Async-based method for enumerating files by fetching data with Github API
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GithubFileInfo>> EnumerateFiles()
        {
            var list = new List<GithubFileInfo>();
            _cache ??= (await FetchGitHubTree())?.Tree;

            if (_cache != null)
                foreach (var treeItem in _cache)
                    if (treeItem.Type == TreeType.Blob)
                    {
                        var url = APIInfo.GetChildUrl(AbsolutePath, treeItem.Path);
                        var apiInfo = (APIInfo.Clone() as GithubAPIInfo)
                            .SetGithubUrl(url)
                            .SetTreeReference(treeItem.Sha);

                        list.Add(new GithubFileInfo(apiInfo)
                        {
                            Directory = this
                        });
                    }

            return list;
        }
    }
}
