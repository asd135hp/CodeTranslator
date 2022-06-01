using System;
using Octokit;

namespace CodeTranslator.Model
{
    public class GitHubAPIInfo : ICloneable
    {
        private const long TICKS = 10_000L;
        private const int DEFAULT_TIMEOUT = 5000;
        private const string DEFAULT_MAIN_BRANCH = "master";

        private Uri _url;
        private string _branch, _commitSHA;
        private int _timeOut;

        /// <summary>
        /// A reference to GitHubClient object from its source
        /// </summary>
        public GitHubClient Client { get; private set; }

        /// <summary>
        /// Url to the current tree pointer
        /// </summary>
        public Uri Url
        {
            get { return _url; }
            set {
                var segments = value.Segments;
                if (segments.Length > 2)
                {
                    Owner = segments[1].Trim('/', '\\').Trim();
                    RepositoryName = segments[2].Trim('/', '\\').Trim();
                }

                _url = value;
            } 
        }

        /// <summary>
        /// Branch of the current tree (default: master)
        /// </summary>
        public string Branch
        {
            get { return _branch; }
            set
            {
                _branch = value.Length != 0 ? value : DEFAULT_MAIN_BRANCH;
            }
        }

        /// <summary>
        /// Owner of the current repo
        /// </summary>
        public string Owner { get; private set; }

        /// <summary>
        /// Name of the current repo
        /// </summary>
        public string RepositoryName { get; private set; }

        /// <summary>
        /// SHA digest to the current tree of the current repo
        /// </summary>
        public string TreeSHA { get; set; }

        /// <summary>
        /// SHA digest to the current commit of the current repo
        /// </summary>
        public string CommitSHA
        {
            get { return _commitSHA; }
            set
            {
                if (value != null && value.Length != 0 && value != _commitSHA)
                {
                    var commit = this.AwaitTask(Client.Git.Commit.Get(Owner, RepositoryName, value));
                    if (commit == null)
                        throw new ArgumentException($"Could not find GitHub commit ref: {value}");

                    TreeSHA = commit.Tree.Sha;
                }
                _commitSHA = value;
            }
        }

        /// <summary>
        /// RateLimit API of Octokit to know how much GitHub API calls left
        /// </summary>
        public RateLimit RateLimit => Client?.GetLastApiInfo()?.RateLimit;

        /// <summary>
        /// Check whether the current repo exists or not
        /// </summary>
        public bool IsRepositoryExist
            => this.AwaitTask(Client.Repository.Get(Owner, RepositoryName)) != null;

        /// <summary>
        /// Set timeout threshold to GitHubClient object held by this object
        /// </summary>
        public int TimeOut
        {
            get { return _timeOut; }
            set
            {
                _timeOut = value;
                Client?.SetRequestTimeout(new TimeSpan(value * TICKS));
            }
        }

        public GitHubAPIInfo(GitHubClient client)
        {
            Client = client;
        }

        public GitHubAPIInfo(string productName, string githubAccessToken)
        {
            Client = new GitHubClient(new Connection(new ProductHeaderValue(productName)))
            {
                Credentials = new Credentials(githubAccessToken)
            };
        }

        public GitHubAPIInfo(string productName, string username, string password)
        {
            Client = new GitHubClient(new Connection(new ProductHeaderValue(productName)))
            {
                Credentials = new Credentials(username, password)
            };
        }

        // probably a bad decision - Debatable #2
        public object Clone()
            => new GitHubAPIInfo(Client)
            {
                _url = _url,
                _branch = _branch,
                _commitSHA = _commitSHA,
                _timeOut = _timeOut,
                Owner = Owner,
                RepositoryName = RepositoryName,
                TreeSHA = TreeSHA
            };
    }
}
