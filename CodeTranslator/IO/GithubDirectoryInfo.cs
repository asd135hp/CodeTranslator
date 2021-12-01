using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using CodeTranslator.Model;
using Octokit;

namespace CodeTranslator.IO
{
    public sealed class GithubDirectoryInfo : GithubTreeItem, IDirectoryInfo
    {
        // could be a problem???
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
        public async Task<IDirectoryInfo[]> GetDirectories()
            => (await EnumerateDirectories()).ToArray();

        /// <summary>
        /// Async-based method for enumerating directories by fetching data with Github API
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IDirectoryInfo>> EnumerateDirectories()
            => await FetchTreeItems(
                TreeType.Tree,
                apiInfo => new GithubDirectoryInfo(apiInfo)
                {
                    Parent = this
                });

        /// <summary>
        /// Async-based method for enumerating files by fetching data with Github API
        /// </summary>
        /// <returns></returns>
        public async Task<IReadonlyFileInfo[]> GetFiles(params string[] acceptedExtensions)
            => (await EnumerateFiles(acceptedExtensions)).ToArray();

        /// <summary>
        /// Async-based method for enumerating files by fetching data with Github API
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IReadonlyFileInfo>> EnumerateFiles(
            params string[] acceptedExtensions)
            => await FetchTreeItems(
                TreeType.Blob,
                apiInfo => new GithubFileInfo(apiInfo)
                {
                    Directory = this
                },
                treeItem =>
                {
                    var extension = treeItem.Path.Split('.')[1];
                    return !acceptedExtensions.Contains(extension);
                });

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<TreeResponse> FetchGitHubTree()
            => Exists ? await APIInfo.FetchTree() : null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childPath"></param>
        /// <returns></returns>
        private Uri GetUrl(string childPath)
            => new Uri($"https://github.com/{APIInfo.Owner}/{APIInfo.RepositoryName}" +
                $"/tree/{APIInfo.Branch}/{AbsolutePath}/{childPath}");

        private async Task<IEnumerable<T>> FetchTreeItems<T>(
            TreeType type,
            Func<GithubAPIInfo, T> generateObj,
            Func<TreeItem, bool> filter = default)
            where T : GithubTreeItem
        {
            var list = new List<T>();
            _cache ??= (await FetchGitHubTree())?.Tree;

            if (_cache != null)
                foreach (var treeItem in _cache)
                    if (treeItem.Type == type)
                    {
                        if (filter?.Invoke(treeItem) ?? false) continue;

                        var url = GetUrl(treeItem.Path);
                        var apiInfo = (APIInfo.Clone() as GithubAPIInfo)
                            .SetGithubUrl(url)
                            .SetTreeReference(treeItem.Sha);

                        list.Add(generateObj.Invoke(apiInfo));
                    }

            return list;
        }
    }
}
