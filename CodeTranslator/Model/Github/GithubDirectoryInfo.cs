using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public sealed class GithubDirectoryInfo : GithubTreeItem
    {
        public GithubDirectoryInfo Parent { get; private set; }

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitSHA"></param>
        /// <param name="accessToken"></param>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="GithubRepoNotFoundException"></exception>
        public GithubDirectoryInfo(
            string githubUrl,
            string commitSHA,
            string branch = DEFAULT_MAIN_BRANCH)
            : base(new Uri(githubUrl), commitSHA, branch)
        {

        }

        /// <summary>
        /// Public (and private for specific user) Github directory reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitSHA"></param>
        /// <param name="accessToken"></param>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="GithubRepoNotFoundException"></exception>
        public GithubDirectoryInfo(
            Uri githubUrl,
            string commitSHA,
            string branch = DEFAULT_MAIN_BRANCH)
            : base(githubUrl, commitSHA, branch)
        {

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
                    list.Add(new GithubDirectoryInfo(url, treeItem.Sha)
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
            var treeResponse = await FetchGitHubTree();
            var list = new List<GithubFileInfo>();

            foreach (var treeItem in treeResponse.Tree)
                if (treeItem.Type == TreeType.Blob)
                {
                    var url = BuildURL(treeItem.Path);
                    list.Add(new GithubFileInfo(url, treeItem.Sha)
                    {
                        Directory = this
                    });
                }

            return list;
        }
    }
}
