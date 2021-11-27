using System;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public abstract class GithubOperationWrapper
    {
        private const long TICKS = 10_000L;
        protected const string DEFAULT_MAIN_BRANCH = "master";

        private static int _timeoutDuration = 5000;
        protected static GitHubClient _client = null;

        private Commit _commit = null;
        private readonly string _branch, _owner, _repoName;

        protected Commit CommitReference => _commit;

        public string Branch => _branch;
        public string Owner => _owner;
        public string RepositoryName => _repoName;
        public RateLimit APIRateLimit => _client?.GetLastApiInfo()?.RateLimit;

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
        internal int TimeoutDuration
        {
            get => _timeoutDuration;
            set
            {
                _timeoutDuration = value;
                _client?.SetRequestTimeout(new TimeSpan(value * TICKS));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal string TreeSHA { get; set; }

        public string CommitSHA
        {
            get => _commit?.Sha ?? "";
            set
            {
                // get commit if commit sha is different from what is stored
                if (_commit == null ||
                    (_commit != null
                    && value.Length != 0
                    && value != _commit.Sha))
                {
                    _commit = AwaitTask(_client.Git.Commit.Get(_owner, _repoName, value));
                    TreeSHA = _commit.Tree.Sha;
                }
            }
        }

        public GithubOperationWrapper(
            Uri githubUrl,
            string accessToken,
            string branch,
            Commit parentCommit)
        {
            _branch = branch.Length == 0 ? DEFAULT_MAIN_BRANCH : branch;
            _owner = _repoName = string.Empty;

            if (_client == null)
            {
                _client = new GitHubClient(new ProductHeaderValue("code-translator"));
                _client.SetRequestTimeout(new TimeSpan(TimeoutDuration * TICKS));
            }

            if (accessToken != null && accessToken.Length != 0)
            {
                Console.WriteLine("Access token: {0}", accessToken);
                _client.Credentials = new Credentials(accessToken);
            }

            if (parentCommit != null) _commit = parentCommit;

            // normal url must contains at least 3 segments
            var segments = githubUrl.Segments;
            if (segments.Length > 2)
            {
                _owner = segments[1].Trim('/', '\\').Trim();
                _repoName = segments[2].Trim('/', '\\').Trim();
            }
        }

        protected T AwaitTask<T>(Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
