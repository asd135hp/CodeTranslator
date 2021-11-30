using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using CodeTranslator.Model;
using Octokit;

namespace CodeTranslator.Github
{
    public class GithubAPIInfo : ICloneable
    {
        private const long TICKS = 10_000L;
        private const string DEFAULT_MAIN_BRANCH = "master";
        private const int DEFAULT_TIMEOUT = 5000;

        private static GitHubClient _client = null;

        private GithubAPIInfoStorage _storage;

        public Uri Url => _storage.Url;
        public string Branch => _storage.Branch;
        public string Owner => _storage.Owner;
        public string RepositoryName => _storage.RepositoryName;
        public string TreeSHA => _storage.TreeSHA;
        public RateLimit RateLimit => _client?.GetLastApiInfo()?.RateLimit;
        public bool IsRepositoryExist
            => this.AwaitTask(_client.Repository.Get(Owner, RepositoryName)) != null;

        public GithubAPIInfo()
        {
            _storage = new GithubAPIInfoStorage()
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
        public GithubAPIInfo SetGithubUrl(string url) => SetGithubUrl(new Uri(url));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public GithubAPIInfo SetGithubUrl(Uri url)
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
        public GithubAPIInfo SetBranch(string branch)
        {
            _storage.Branch = _storage.Branch.Length != 0 ? branch : DEFAULT_MAIN_BRANCH;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public GithubAPIInfo SetAccessToken(string accessToken)
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
        public GithubAPIInfo SetCommit(string commitReference)
        {
            // get commit if commit sha is different from what is stored
            if (commitReference != null
                && commitReference.Length != 0
                && commitReference != _storage.CommitSHA)
            {
                var commit = this.AwaitTask(_client.Git.Commit.Get(Owner, RepositoryName, commitReference));
                if (commit == null)
                    throw new ArgumentException($"Could not find Github commit ref: {commitReference}");
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
        public GithubAPIInfo SetTreeReference(string treeSha)
        {
            _storage.TreeSHA = treeSha;
            return this;
        }

        /// <summary>
        /// <para>
        /// Timeout duration for requesting from Github API in milliseconds.
        /// Default is 5000ms or 5 seconds
        /// </para>
        /// <para>
        /// Unless making the requests of future objects to take longer or less time is not needed
        /// then set this property to a value of your choice.
        /// </para>
        /// </summary>
        /// <param name="timeInMilliseconds"></param>
        public GithubAPIInfo SetTimeout(int timeInMilliseconds)
        {
            _storage.Timeout = timeInMilliseconds;
            _client?.SetRequestTimeout(new TimeSpan(timeInMilliseconds * TICKS));
            return this;
        }

        public object Clone()
            => new GithubAPIInfo
            {
                _storage = new GithubAPIInfoStorage
                {
                    Owner = Owner,
                    RepositoryName = RepositoryName,
                    Branch = Branch,
                    TreeSHA = TreeSHA,
                    Url = new Uri(Url.AbsoluteUri),
                    CommitSHA = _storage.CommitSHA
                }
            };

        #endregion


        #region Read operations for both GithubDirectoryInfo and GithubFileInfo

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
