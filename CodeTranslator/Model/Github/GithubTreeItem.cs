using System.Linq;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public abstract class GithubTreeItem
    {
        protected bool _exists;
        private readonly string _absolutePath;
        private readonly GithubAPIInfo _apiInfo;

        public bool Exists => _exists;
        public string AbsolutePath => _absolutePath;
        public GithubAPIInfo APIInfo => _apiInfo;

        public abstract string Name { get; }
        public abstract string Extension { get; }

        /// <summary>
        /// Public (and private for specific user) Github tree item reader
        /// </summary>
        /// <param name="apiInfo"></param>
        protected GithubTreeItem(GithubAPIInfo apiInfo)
        {
            _apiInfo = apiInfo;
            _exists = apiInfo.IsRepositoryExist;

            var segments = apiInfo.Url.Segments;
            if(segments.Length > 5 && Exists)
                _absolutePath = string.Join("", segments.Skip(5)).Trim().TrimEnd('/', '\\');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected async Task<TreeResponse> FetchGitHubTree()
            => Exists ? await _apiInfo.FetchTree() : null;
    }
}
