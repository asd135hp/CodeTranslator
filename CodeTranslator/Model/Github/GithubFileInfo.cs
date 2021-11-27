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
        /// <param name="accessToken"></param>
        public GithubFileInfo(
            string githubUrl,
            string accessToken)
            : this(new Uri(githubUrl), accessToken, DEFAULT_MAIN_BRANCH) { }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="accessToken"></param>
        public GithubFileInfo(
            Uri githubUrl,
            string accessToken)
            : this(githubUrl, accessToken, DEFAULT_MAIN_BRANCH) { }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="branch"></param>
        /// <param name="accessToken"></param>
        public GithubFileInfo(
            string githubUrl,
            string accessToken,
            string branch = DEFAULT_MAIN_BRANCH)
            : this(new Uri(githubUrl), accessToken, branch) { }

        /// <summary>
        /// Public (and private for specific user) Github file reader
        /// </summary>
        /// <param name="githubUrl"></param>
        /// <param name="branch"></param>
        /// <param name="accessToken"></param>
        public GithubFileInfo(
            Uri githubUrl,
            string accessToken,
            string branch = DEFAULT_MAIN_BRANCH)
            : this(githubUrl, accessToken, branch, null)
        {
            var segments = githubUrl.Segments;
            // set up name and extension of this tree item if possible
            if (segments.Length > 5 && Exists && githubUrl.IsFile)
            {
                string fileName = segments.Last(),
                    namePrefix = fileName[0] == '.' ? "." : "";
                var splittedFileName = fileName.Split('.').ToList();

                _extension = splittedFileName.Last();
                splittedFileName.RemoveAt(splittedFileName.Count - 1);
                _name = $"{namePrefix}{string.Join(".", splittedFileName)}";
            }
            else _exists = false;
        }

        internal GithubFileInfo(
            Uri githubUrl,
            string accessToken,
            string branch,
            Commit parentCommit)
            : base(githubUrl, accessToken, branch, parentCommit) { }
    }
}
