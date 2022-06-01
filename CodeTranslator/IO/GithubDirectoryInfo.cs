using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using CodeTranslator.Model;
using Octokit;

namespace CodeTranslator.IO
{
    public sealed class GitHubDirectoryInfo : GitHubTreeItem, IDirectoryInfo
    {
        // could be a problem???
        private IEnumerable<TreeItem> _cache;
        private readonly string _name, _fullName;

        public GitHubDirectoryInfo Parent { get; private set; }

        public override string Name => _name;
        public override string Extension => "";
        public string FullName => _fullName;

        /// <summary>
        /// Public (and private for specific user) GitHub directory reader
        /// </summary>
        /// <param name="client"></param>
        /// <param name="gitHubUrl"></param>
        /// <param name="commitReference"></param>
        public GitHubDirectoryInfo(
            GitHubClient client,
            string gitHubUrl,
            string commitReference)
            : this(client, new Uri(gitHubUrl), commitReference) { }

        /// <summary>
        /// Public (and private for specific user) GitHub directory reader
        /// </summary>
        /// <param name="client"></param>
        /// <param name="gitHubUrl"></param>
        /// <param name="commitReference"></param>
        public GitHubDirectoryInfo(
            GitHubClient client,
            Uri gitHubUrl,
            string commitReference)
            : this(client, gitHubUrl, commitReference, "") { }

        /// <summary>
        /// Public (and private for specific user) GitHub directory reader
        /// </summary>
        /// <param name="client"></param>
        /// <param name="gitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="branch"></param>
        public GitHubDirectoryInfo(
            GitHubClient client,
            string gitHubUrl,
            string commitReference,
            string branch)
            : this(client, new Uri(gitHubUrl), commitReference, branch) { }

        /// <summary>
        /// Public (and private for specific user) GitHub directory reader
        /// </summary>
        /// <param name="client"></param>
        /// <param name="gitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="branch"></param>
        public GitHubDirectoryInfo(
            GitHubClient client,
            Uri gitHubUrl,
            string commitReference,
            string branch)
            : this(new GitHubAPIInfo(client)
            {
                Url = gitHubUrl,
                CommitSHA = commitReference,
                Branch = branch,
            }) { }

        /// <summary>
        /// Public (and private for specific user) GitHub directory reader
        /// </summary>
        /// <param name="apiInfo"></param>
        public GitHubDirectoryInfo(GitHubAPIInfo apiInfo) : base(apiInfo)
        {
            if (Exists)
            {
                _fullName = apiInfo.Url.AbsolutePath;
                _name = apiInfo.Url.Segments.Last().Trim().Trim('/', '\\');
            }
            _cache = null;
        }

        /// <summary>
        /// Async-based method for enumerating directories by fetching data with GitHub API
        /// </summary>
        /// <returns></returns>
        public async Task<IDirectoryInfo[]> GetDirectories()
            => (await EnumerateDirectories()).ToArray();

        /// <summary>
        /// Async-based method for enumerating directories by fetching data with GitHub API
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IDirectoryInfo>> EnumerateDirectories()
            => await FetchTreeItems(
                TreeType.Tree,
                apiInfo => new GitHubDirectoryInfo(apiInfo)
                {
                    Parent = this
                });

        /// <summary>
        /// Async-based method for enumerating files by fetching data with GitHub API
        /// </summary>
        /// <returns></returns>
        public async Task<IReadonlyFileInfo[]> GetFiles(params string[] acceptedExtensions)
            => (await EnumerateFiles(acceptedExtensions)).ToArray();

        /// <summary>
        /// Async-based method for enumerating files by fetching data with GitHub API
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IReadonlyFileInfo>> EnumerateFiles(
            params string[] acceptedExtensions)
            => await FetchTreeItems(
                TreeType.Blob,
                apiInfo => new GitHubFileInfo(apiInfo)
                {
                    Directory = this
                },
                treeItem =>
                {
                    var extension = treeItem.Path.Split('.')[1];
                    return !acceptedExtensions.Contains(extension);
                });

        /// <summary>
        /// Fetch this GitHub tree
        /// </summary>
        /// <returns></returns>
        private async Task<TreeResponse> FetchGitHubTree()
            => Exists ? (
                APIInfo.CommitSHA.Length != 0 ?
                    await APIInfo.Client.Git.Tree.Get(
                        APIInfo.Owner,
                        APIInfo.RepositoryName,
                        APIInfo.TreeSHA
                    ) : null
            ) : null;

        /// <summary>
        /// Re-cast path into GitHub API url
        /// </summary>
        /// <param name="childPath"></param>
        /// <returns></returns>
        private Uri GetUrl(string childPath)
            => new Uri($"https://github.com/{APIInfo.Owner}/{APIInfo.RepositoryName}" +
                $"/tree/{APIInfo.Branch}/{AbsolutePath}/{childPath}");

        /// <summary>
        /// Fetch all child tree items
        /// <para>
        /// (It is debatable whether to cache data to save API calls
        /// or to not cache data to save computer resources)
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="generateObj"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private async Task<IEnumerable<T>> FetchTreeItems<T>(
            TreeType type,
            Func<GitHubAPIInfo, T> generateObj,
            Func<TreeItem, bool> filter = default)
            where T : GitHubTreeItem
        {
            var list = new List<T>();
            _cache ??= (await FetchGitHubTree())?.Tree;

            if (_cache != null)
                foreach (var treeItem in _cache)
                    if (treeItem.Type == type)
                    {
                        if (filter != null && filter.Invoke(treeItem)) continue;

                        var url = GetUrl(treeItem.Path);
                        var apiInfo = APIInfo.Clone() as GitHubAPIInfo;

                        apiInfo.Url = url;
                        apiInfo.TreeSHA = treeItem.Sha;
                        list.Add(generateObj.Invoke(apiInfo));
                    }

            return list;
        }
    }
}
