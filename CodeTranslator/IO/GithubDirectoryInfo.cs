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
        /// <param name="GitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GitHubDirectoryInfo(
            string GitHubUrl,
            string commitReference,
            string accessToken)
            : this(new Uri(GitHubUrl), commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) GitHub directory reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GitHubDirectoryInfo(
            Uri GitHubUrl,
            string commitReference,
            string accessToken)
            : this(GitHubUrl, commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) GitHub directory reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GitHubDirectoryInfo(
            string GitHubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new Uri(GitHubUrl), commitReference, accessToken, branch) { }

        /// <summary>
        /// Public (and private for specific user) GitHub directory reader
        /// </summary>
        /// <param name="GitHubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GitHubDirectoryInfo(
            Uri GitHubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new GitHubAPIInfo()
                  .SetAccessToken(accessToken)
                  .SetGitHubUrl(GitHubUrl)
                  .SetCommit(commitReference)
                  .SetBranch(branch)) { }

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
            => new Uri($"https://GitHub.com/{APIInfo.Owner}/{APIInfo.RepositoryName}" +
                $"/tree/{APIInfo.Branch}/{AbsolutePath}/{childPath}");

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
                        if (filter?.Invoke(treeItem) ?? false) continue;

                        var url = GetUrl(treeItem.Path);
                        var apiInfo = (APIInfo.Clone() as GitHubAPIInfo)
                            .SetGitHubUrl(url)
                            .SetTreeReference(treeItem.Sha);

                        list.Add(generateObj.Invoke(apiInfo));
                    }

            return list;
        }
    }
}
