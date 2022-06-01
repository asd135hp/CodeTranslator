using System.Linq;
using Octokit;

namespace CodeTranslator.Model
{
    public abstract class GitHubTreeItem
    {
        private readonly string _absolutePath;
        private readonly GitHubAPIInfo _apiInfo;

        internal GitHubAPIInfo APIInfo => _apiInfo;

        // source of quick depletion on the number of remaining API calls - Debatable #1
        public bool Exists => APIInfo.IsRepositoryExist;
        public string AbsolutePath => _absolutePath;
        public string AbsoluteUrl => _apiInfo.Url.AbsoluteUri;
        public RateLimit RateLimit => _apiInfo.RateLimit;

        public abstract string Name { get; }
        public abstract string Extension { get; }

        /// <summary>
        /// Public (and private for specific user) GitHub tree item reader
        /// </summary>
        /// <param name="apiInfo"></param>
        protected GitHubTreeItem(GitHubAPIInfo apiInfo)
        {
            _apiInfo = apiInfo;
            var segments = apiInfo.Url.Segments;
            if(segments.Length > 5 && apiInfo.IsRepositoryExist)
                _absolutePath = string.Join("", segments.Skip(5)).Trim().TrimEnd('/', '\\');
        }
    }
}
