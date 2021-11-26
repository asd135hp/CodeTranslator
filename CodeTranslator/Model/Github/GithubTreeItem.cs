using System;
using System.Linq;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public abstract class GithubTreeItem
    {
        private const long TICKS = 10_000L;
        protected const string DEFAULT_MAIN_BRANCH = "master";

        private static GitHubClient _client = null;
        private static int _timeoutDuration = 5000;
        public static string AccessToken { get; set; }


        /// <summary>
        /// Timeout duration for requesting from Github API in milliseconds.
        /// Default is 5000ms or 5 seconds
        /// </summary>
        public static int TimeoutDuration
        {
            get => _timeoutDuration;
            set
            {
                _timeoutDuration = value;
                _client?.SetRequestTimeout(new TimeSpan(value * TICKS));
            }
        }
        
        private Commit _commit;

        public bool Exists { get; private set; }
        public string Branch { get; private set; }
        public string AbsolutePath { get; protected set; }
        public string Name { get; protected set; }
        public string Extension { get; protected set; }

        public readonly string Owner, RepositoryName;

        /// <summary>
        /// Public (and private for specific user) Github tree item reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitSHA"></param>
        /// <param name="accessToken"></param>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="GithubRepoNotFoundException"></exception>
        public GithubTreeItem(
            Uri githubUrl,
            string commitSHA,
            string branch = DEFAULT_MAIN_BRANCH)
        {
            Branch = branch;
            Exists = true;

            if(_client == null)
            {
                _client = new GitHubClient(new ProductHeaderValue("code-translator"));
                _client.SetRequestTimeout(new TimeSpan(TimeoutDuration * TICKS));
            }

            // store and set up new credentials for github client object
            if(AccessToken != null && AccessToken.Length != 0)
                _client.Credentials = new Credentials(AccessToken);

            // normal url must contains at least 3 segments
            var segments = githubUrl.Segments;
            if (segments.Length > 2)
            {
                Owner = segments[1].Trim('/', '\\').Trim();
                RepositoryName = segments[2].Trim('/', '\\').Trim();

                // get commit
                var commitTask = _client.Git.Commit.Get(Owner, RepositoryName, commitSHA);
                commitTask.Wait();
                _commit = commitTask.Result;

                // check if the repo exists
                var repoFetchTask = _client.Repository.Get(Owner, RepositoryName);
                repoFetchTask.Wait();
                if (repoFetchTask.Result == null) Exists = false;

                // set up name and extension of this tree item if possible
                if (segments.Length > 5)
                {
                    var splittedFileName = segments.Last().Split('.');
                    Name = splittedFileName[0];
                    if (splittedFileName.Length == 2) Extension = splittedFileName[1];

                    Branch = segments[4].Trim('/', '\\').Trim();
                    AbsolutePath = string.Join("", segments.Skip(5));
                }
            }
            else
            {
                Exists = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="GithubCommitNotFoundException"></exception>
        protected async Task<TreeResponse> FetchGitHubTree()
            => (Exists && _commit != null) ?
                await _client.Git.Tree.Get(Owner, RepositoryName, _commit.Tree.Sha) :
                null;

        protected string BuildURL(string path)
            => $"https://github.com/{Owner}/{RepositoryName}/tree/{Branch}/{AbsolutePath}/{path}";
    }
}
