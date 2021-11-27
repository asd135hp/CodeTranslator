using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public sealed class GithubDirectoryInfo : GithubTreeItem
    {
        private readonly string _name;

        public GithubDirectoryInfo Parent { get; private set; }

        public override string Name => _name;
        public override string Extension => "";

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="accessToken"></param>
        public GithubDirectoryInfo(
            string githubUrl,
            string accessToken)
            : this(new Uri(githubUrl), accessToken, DEFAULT_MAIN_BRANCH) { }

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="accessToken"></param>
        public GithubDirectoryInfo(
            Uri githubUrl,
            string accessToken)
            : this(githubUrl, accessToken, DEFAULT_MAIN_BRANCH) { }

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="branch"></param>
        /// <param name="accessToken"></param>
        public GithubDirectoryInfo(
            string githubUrl,
            string accessToken,
            string branch = DEFAULT_MAIN_BRANCH)
            : this(new Uri(githubUrl), accessToken, branch) { }

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="branch"></param>
        /// <param name="accessToken"></param>
        public GithubDirectoryInfo(
            Uri githubUrl,
            string accessToken,
            string branch = DEFAULT_MAIN_BRANCH)
            : this(githubUrl, accessToken, branch, null) { }

        internal GithubDirectoryInfo(
            Uri githubUrl,
            string accessToken,
            string branch,
            Commit parentCommit)
            : base(githubUrl, accessToken, branch, parentCommit)
        {
            // even Uri object determine the link as a file so
            // it will be an error then
            if (Exists && !githubUrl.IsFile)
                _name = githubUrl.Segments.Last().Trim().Trim('/', '\\');
            else _exists = false;
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
            var treeResponse = await FetchGitHubTree();
            var list = new List<GithubDirectoryInfo>();

            foreach(var treeItem in treeResponse.Tree)
                if (treeItem.Type == TreeType.Tree)
                {
                    var url = BuildURL(treeItem.Path);
                    list.Add(new GithubDirectoryInfo(url, "", Branch, CommitReference)
                    {
                        Parent = this,
                        TreeSHA = treeItem.Sha
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
            var treeResponse = await FetchGitHubTree();
            var list = new List<GithubFileInfo>();

            foreach (var treeItem in treeResponse.Tree)
                if (treeItem.Type == TreeType.Blob)
                {
                    var url = BuildURL(treeItem.Path);
                    list.Add(new GithubFileInfo(url, "", Branch, CommitReference)
                    {
                        Directory = this
                    });
                }

            return list;
        }
    }
}
