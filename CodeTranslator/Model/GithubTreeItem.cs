using System.Linq;
using CodeTranslator.IO;
using Octokit;

namespace CodeTranslator.Model
{
    public abstract class GitHubTreeItem
    {
        protected bool _exists;
        private readonly string _absolutePath;
        private readonly GitHubAPIInfo _apiInfo;

        internal GitHubAPIInfo APIInfo => _apiInfo;

        public bool Exists => _exists;
        public string Sha => _apiInfo.TreeSHA;
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
            _exists = apiInfo.IsRepositoryExist;

            var segments = apiInfo.Url.Segments;
            if(segments.Length > 5 && Exists)
                _absolutePath = string.Join("", segments.Skip(5)).Trim().TrimEnd('/', '\\');
        }
    }
}
