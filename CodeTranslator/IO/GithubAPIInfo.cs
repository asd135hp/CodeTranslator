using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CodeTranslator.Model;
using Microsoft.Extensions.Logging;
using Octokit;

namespace CodeTranslator.IO
{
    public class GitHubAPIInfo : ICloneable
    {
        private const long TICKS = 10_000L;
        private const string DEFAULT_MAIN_BRANCH = "master";
        private const int DEFAULT_TIMEOUT = 5000;

        private static GitHubClient _client = null;

        private GitHubAPIInfoStorage _storage;

        public Uri Url => _storage.Url;
        public string Branch => _storage.Branch;
        public string Owner => _storage.Owner;
        public string RepositoryName => _storage.RepositoryName;
        public string TreeSHA => _storage.TreeSHA;
        public RateLimit RateLimit => _client?.GetLastApiInfo()?.RateLimit;
        public bool IsRepositoryExist
            => this.AwaitTask(_client.Repository.Get(Owner, RepositoryName)) != null;

        public ILogger Logger { get; set; }

        public GitHubAPIInfo()
        {
            _storage = new GitHubAPIInfoStorage()
            {
                Owner = string.Empty,
                RepositoryName = string.Empty,
                CommitSHA = string.Empty,
                Timeout = DEFAULT_TIMEOUT,
                Branch = string.Empty,
                TreeSHA = string.Empty
            };

            if (_client == null)
            {
                _client = new GitHubClient(new ProductHeaderValue("code-translator"));
                _client.SetRequestTimeout(new TimeSpan(DEFAULT_TIMEOUT * TICKS));
            }
        }

        #region Create/Update operations

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GitHubAPIInfo SetGitHubUrl(string url) => SetGitHubUrl(new Uri(url));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GitHubAPIInfo SetGitHubUrl(Uri url)
        {
            // normal url must contains at least 3 segments
            var segments = url.Segments;
            if (segments.Length > 2)
            {
                _storage.Owner = segments[1].Trim('/', '\\').Trim();
                _storage.RepositoryName = segments[2].Trim('/', '\\').Trim();
            }
            
            _storage.Url = url;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public GitHubAPIInfo SetBranch(string branch)
        {
            _storage.Branch = _storage.Branch.Length != 0 ? branch : DEFAULT_MAIN_BRANCH;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public GitHubAPIInfo SetAccessToken(string accessToken)
        {
            _client.Credentials = new Credentials(accessToken);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commitReference"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public GitHubAPIInfo SetCommit(string commitReference)
        {
            // get commit if commit sha is different from what is stored
            if (commitReference != null
                && commitReference.Length != 0
                && commitReference != _storage.CommitSHA)
            {
                var commit = this.AwaitTask(_client.Git.Commit.Get(Owner, RepositoryName, commitReference));
                if (commit == null)
                    throw new ArgumentException($"Could not find GitHub commit ref: {commitReference}");
                SetTreeReference(commit.Tree.Sha);
                _storage.CommitSHA = commit.Tree.Sha;
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeSha"></param>
        /// <returns></returns>
        public GitHubAPIInfo SetTreeReference(string treeSha)
        {
            _storage.TreeSHA = treeSha;
            return this;
        }

        /// <summary>
        /// <para>
        /// Timeout duration for requesting from GitHub API in milliseconds.
        /// Default is 5000ms or 5 seconds
        /// </para>
        /// <para>
        /// Unless making the requests of future objects to take longer or less time is not needed
        /// then set this property to a value of your choice.
        /// </para>
        /// </summary>
        /// <param name="timeInMilliseconds"></param>
        public GitHubAPIInfo SetTimeout(int timeInMilliseconds)
        {
            _storage.Timeout = timeInMilliseconds;
            _client?.SetRequestTimeout(new TimeSpan(timeInMilliseconds * TICKS));
            return this;
        }

        public object Clone()
            => new GitHubAPIInfo
            {
                _storage = new GitHubAPIInfoStorage
                {
                    Owner = Owner,
                    RepositoryName = RepositoryName,
                    Branch = Branch,
                    TreeSHA = TreeSHA,
                    Url = new Uri(Url.AbsoluteUri),
                    CommitSHA = _storage.CommitSHA
                },
                Logger = Logger
            };

        #endregion

        #region Read operations for both GitHubDirectoryInfo and GitHubFileInfo

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal async Task<TreeResponse> FetchTree()
            => _storage.CommitSHA.Length != 0 ?
            await _client.Git.Tree.Get(Owner, RepositoryName, _storage.TreeSHA) :
            null;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal async Task<TreeResponse> FetchTreeRecursive()
            => _storage.CommitSHA.Length != 0 ?
            await _client.Git.Tree.GetRecursive(Owner, RepositoryName, _storage.TreeSHA) :
            null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <returns></returns>
        internal async Task<IReadOnlyList<RepositoryContent>> FetchFileContent(string absolutePath)
            => await _client.Repository
                .Content
                .GetAllContents(Owner, RepositoryName, absolutePath);

        #endregion
    }
}
