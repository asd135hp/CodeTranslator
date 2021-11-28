using System;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public class GithubAPIInfo : ICloneable
    {
        private const long TICKS = 10_000L;
        private const string DEFAULT_MAIN_BRANCH = "master";

        private static int _timeoutDuration = 5000;
        private static GitHubClient _client = null;

        private Uri _url;
        private Commit _commit;
        private string _branch, _owner, _repoName, _treeSHA;

        public Uri Url => _url;
        public bool IsRepositoryExist => AwaitTask(_client.Repository.Get(Owner, RepositoryName)) != null;
        public string Branch => _branch;
        public string Owner => _owner;
        public string RepositoryName => _repoName;
        public RateLimit RateLimit => _client?.GetLastApiInfo()?.RateLimit;

        public GithubAPIInfo()
        {
            _owner = _repoName = string.Empty;

            if (_client == null)
            {
                _client = new GitHubClient(new ProductHeaderValue("code-translator"));
                _client.SetRequestTimeout(new TimeSpan(_timeoutDuration * TICKS));
            }
        }

        private T AwaitTask<T>(Task<T> task)
        {
            task.Wait();
            return task.Result;
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
                _owner = segments[1].Trim('/', '\\').Trim();
                _repoName = segments[2].Trim('/', '\\').Trim();
            }

            _url = url;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="branch"></param>
        /// <returns></returns>
        public GithubAPIInfo SetBranch(string branch)
        {
            _branch = _branch != null && _branch.Length != 0 ? branch : DEFAULT_MAIN_BRANCH;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public GithubAPIInfo SetAccessToken(string accessToken)
        {
            Console.WriteLine("Access token: {0}", accessToken);
            _client.Credentials = new Credentials(accessToken);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commitReference"></param>
        /// <returns></returns>
        public GithubAPIInfo SetCommit(string commitReference)
        {
            // get commit if commit sha is different from what is stored
            if (_commit == null ||
                (_commit != null
                && commitReference.Length != 0
                && commitReference != _commit.Sha))
            {
                _commit = AwaitTask(_client.Git.Commit.Get(_owner, _repoName, commitReference));
                SetTreeReference(_commit.Tree.Sha);
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
            _treeSHA = treeSha;
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
            _timeoutDuration = timeInMilliseconds;
            _client?.SetRequestTimeout(new TimeSpan(timeInMilliseconds * TICKS));
            return this;
        }


        public object Clone()
        {
            var clone = new GithubAPIInfo();
            clone._commit = _commit;
            clone._branch = _branch;
            clone._owner = _owner;
            clone._repoName = _repoName;
            clone._treeSHA = _treeSHA;
            clone._url = new Uri(_url.AbsoluteUri);
            return clone;
        }

        #endregion


        #region Read operations

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<TreeResponse> FetchTree()
            => _commit != null ?
            await _client.Git.Tree.Get(Owner, RepositoryName, _treeSHA) :
            null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="absolutePath"></param>
        /// <param name="childPath"></param>
        /// <returns></returns>
        internal Uri GetChildUrl(string absolutePath, string childPath)
            => new Uri($"https://github.com/{Owner}/{RepositoryName}" +
                $"/tree/{Branch}/{absolutePath}/{childPath}");

        #endregion
    }
}
