using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public sealed class GithubFileInfo : GithubTreeItem
    {
        private readonly string _name, _extension;

        public GithubDirectoryInfo Directory { get; internal set; }

        public override string Name => _name;
        public override string Extension => _extension;

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GithubFileInfo(
            string githubUrl,
            string commitReference,
            string accessToken)
            : this(new Uri(githubUrl), commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        public GithubFileInfo(
            Uri githubUrl,
            string commitReference,
            string accessToken)
            : this(githubUrl, commitReference, accessToken, "") { }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitReference"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GithubFileInfo(
            string githubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new Uri(githubUrl), commitReference, accessToken, branch) { }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="branch"></param>
        /// <param name="accessToken"></param>
        /// <param name="branch"></param>
        public GithubFileInfo(
            Uri githubUrl,
            string commitReference,
            string accessToken,
            string branch)
            : this(new GithubAPIInfo()
                  .SetAccessToken(accessToken)
                  .SetGithubUrl(githubUrl)
                  .SetCommit(commitReference)
                  .SetBranch(branch)) { }

        public GithubFileInfo(GithubAPIInfo apiInfo) : base(apiInfo)
        {
            var segments = apiInfo.Url.Segments;
            // set up name and extension of this tree item if possible
            if (segments.Length > 5 && Exists)
            {
                var fileName = segments.Last(); 
                var nameParts = fileName.Split('.').ToList();
                var namePrefix = fileName[0] == '.' && nameParts[0].Length != 0 ? "." : "";

                _extension = nameParts.Last();
                nameParts.RemoveAt(nameParts.Count - 1);
                _name = $"{namePrefix}{string.Join(".", nameParts)}";
            }
            else _exists = false;
        }
    }
}
