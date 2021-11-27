using System;
using System.Linq;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public abstract class GithubTreeItem : GithubOperationWrapper
    {
        private readonly string _absolutePath;
        protected bool _exists;

        public bool Exists => _exists && CommitReference != null;
        public string AbsolutePath => _absolutePath;
        public abstract string Name { get; }
        public abstract string Extension { get; }

        /// <summary>
        /// Public (and private for specific user) Github tree item reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        /// <param name="parentCommit"></param>
        protected GithubTreeItem(
            Uri githubUrl,
            string accessToken,
            string branch,
            Commit parentCommit)
            : base(githubUrl, accessToken, branch, parentCommit)
        {
            // check if the repo exists
            _exists = AwaitTask(_client.Repository.Get(Owner, RepositoryName)) != null;

            var segments = githubUrl.Segments;
            if(segments.Length > 5 && Exists)
                _absolutePath = string.Join("", segments.Skip(5)).Trim().TrimEnd('/', '\\');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async Task<TreeResponse> FetchGitHubTree()
            => !Exists ? null :
                await _client.Git.Tree.Get(Owner, RepositoryName, TreeSHA);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        protected Uri BuildURL(string path)
            => new UriBuilder("https://github.com")
            {
                Path = $"/{Owner}/{RepositoryName}/tree/{Branch}/{AbsolutePath}/{path}"
            }.Uri;
    }
}
