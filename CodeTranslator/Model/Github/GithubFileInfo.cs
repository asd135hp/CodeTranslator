using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Octokit;

namespace CodeTranslator.Model.Github
{
    public sealed class GithubFileInfo : GithubTreeItem
    {
        public GithubDirectoryInfo Directory { get; internal set; }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitSHA"></param>
        /// <param name="accessToken"></param>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="GithubRepoNotFoundException"></exception>
        public GithubFileInfo(
            string githubUrl,
            string commitSHA,
            string branch = DEFAULT_MAIN_BRANCH)
            : base(new Uri(githubUrl), commitSHA, branch)
        {

        }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="commitSHA"></param>
        /// <param name="accessToken"></param>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="GithubRepoNotFoundException"></exception>
        public GithubFileInfo(
            Uri githubUrl,
            string commitSHA,
            string branch = DEFAULT_MAIN_BRANCH)
            : base(githubUrl, commitSHA, branch)
        {

        }
    }
}
